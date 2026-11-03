var paymentsData = NO_DATA;
let payments = null;

function InitPayments(sendback) {
    return new Promise((resolve) => {
        let finalized = false;
        try {
            if (ysdk == null) {
                Final(NO_DATA);
                return;
            }

            ysdk.getPayments().then(_payments => {
                payments = _payments;

                return payments.getCatalog()
                    .then(products => {
                        let productID = [];
                        let title = [];
                        let description = [];
                        let imageURI = [];
                        let price = [];
                        let priceValue = [];
                        let priceCurrencyCode = [];
                        let currencyImageURL = [];
                        let consumed = [];

                        return payments.getPurchases().then(purchases => {
                            for (let i = 0; i < products.length; i++) {
                                productID[i] = products[i].id;
                                title[i] = products[i].title;
                                description[i] = products[i].description;
                                imageURI[i] = products[i].imageURI;
                                price[i] = products[i].price;
                                priceValue[i] = products[i].priceValue;
                                priceCurrencyCode[i] = products[i].priceCurrencyCode;
                                currencyImageURL[i] = products[i].getPriceCurrencyImage("medium");

                                consumed[i] = true;
                                for (let i2 = 0; i2 < purchases.length; i2++) {
                                    if (purchases[i2].productID === productID[i]) {
                                        consumed[i] = false;
                                        break;
                                    }
                                }
                            }

                            let jsonPayments = {
                                "id": productID,
                                "title": title,
                                "description": description,
                                "imageURI": imageURI,
                                "price": price,
                                "priceValue": priceValue,
                                "priceCurrencyCode": priceCurrencyCode,
                                "currencyImageURL": currencyImageURL,
                                "consumed": consumed,
                                "language": ysdk.environment.i18n.lang
                            };

                            Final(JSON.stringify(jsonPayments));
                        });
                    });

            }).catch(e => {
                LogStyledMessage('Purchases are not available', e);
                Final(NO_DATA);
            })
        } catch (e) {
            console.error('CRASH Init Payments: ', e.message);
            Final(NO_DATA);
        }

        function Final(res) {
            if (finalized)
                return;
            finalized = true;
            paymentsData = res;
            try {
                YG2Instance('PaymentsEntries', res);
            } catch (e) {
                console.warn('Payments initialization callback failed: ', e);
            } finally {
                resolve(res);
            }
        }
    });
}

// Shared by automatic recovery and explicit token confirmation.
const paymentConsumesInFlight = new Map();
const paymentTokensConsumed = new Set();
const paymentTokensProcessing = new Set();
const paymentTokensNotified = new Set();
let paymentsAutoConsume = true;

async function ConsumePaymentToken(token) {
    if (!payments || !token)
        throw new Error('Payments or purchase token is missing');
    if (paymentTokensConsumed.has(token))
        return;
    if (paymentConsumesInFlight.has(token))
        return paymentConsumesInFlight.get(token);

    // Defer the SDK call so the shared request exists even for synchronous callbacks/errors.
    const request = Promise.resolve().then(() => payments.consumePurchase(token));
    paymentConsumesInFlight.set(token, request);
    try {
        await request;
        paymentTokensConsumed.add(token);
    } finally {
        paymentConsumesInFlight.delete(token);
    }
}

async function ProcessAutomaticPurchase(purchase, onPurchaseSuccess) {
    const token = purchase.purchaseToken;
    if (!token)
        throw new Error('Purchase token is missing');
    if (paymentTokensProcessing.has(token) || paymentTokensConsumed.has(token))
        return;

    // Claim before notifying Unity: the reward handler may request consumption again.
    paymentTokensProcessing.add(token);
    try {
        if (onPurchaseSuccess && !paymentTokensNotified.has(token)) {
            paymentTokensNotified.add(token);
            YG2Instance('OnPurchaseSuccess', purchase.productID);
        }
        await ConsumePaymentToken(token);
    } catch (e) {
        // Payment is already made. A confirmation error is not a failed purchase.
        console.warn('Purchase processing failed: ', e);
    } finally {
        paymentTokensProcessing.delete(token);
    }
}

async function BuyPayments(id) {
    let purchase;
    try {
        if (!payments)
            throw new Error('Payments are not initialized');
        YG2Instance('SetPauseGame', 'true');
        purchase = await payments.purchase(id);
    } catch (e) {
        console.warn('Purchase failed: ', e);
        YG2Instance('OnPurchaseFailed', id);
        FocusGame();
        return;
    }

    try {
        if (paymentsAutoConsume) {
            await ProcessAutomaticPurchase(purchase, true);
        } else {
            YG2Instance('PurchaseReceived', JSON.stringify({
                productID: purchase.productID,
                purchaseToken: purchase.purchaseToken
            }));
        }
    } catch (e) {
        console.warn('Purchase delivery failed: ', e);
    } finally {
        YG2Instance('SetPauseGame', 'false');
        FocusGame();
    }
}

async function ConsumePurchase(id, onPurchaseSuccess) {
    try {
        if (!payments)
            throw new Error('Payments are not initialized');
        const purchases = await payments.getPurchases();
        await Promise.all(purchases.filter(p => p.productID === id)
            .map(p => ProcessAutomaticPurchase(p, onPurchaseSuccess)));
    } catch (e) {
        console.warn('Consume purchase failed: ', e);
    }
}

async function ConsumePurchases(onPurchaseSuccess) {
    try {
        if (!payments)
            throw new Error('Payments are not initialized');
        const purchases = await payments.getPurchases();
        LogStyledMessage('Unprocessed purchases: ', purchases.length);
        await Promise.all(purchases.map(p => ProcessAutomaticPurchase(p, onPurchaseSuccess)));
    } catch (e) {
        console.warn('Consume purchases failed: ', e);
    }
}

async function GetPendingPurchases() {
    try {
        if (!payments)
            throw new Error('Payments are not initialized');
        const purchases = await payments.getPurchases();
        YG2Instance('PendingPurchasesReceived', JSON.stringify({
            purchases: purchases.map(p => ({ productID: p.productID, purchaseToken: p.purchaseToken }))
        }));
    } catch (e) {
        console.warn('Get pending purchases failed: ', e);
        YG2Instance('PendingPurchasesFailed', String(e && e.message ? e.message : e));
    }
}

async function ConsumePurchaseByToken(token) {
    try {
        await ConsumePaymentToken(token);
        YG2Instance('PurchaseConsumed', token);
    } catch (e) {
        console.warn('Consume purchase token failed: ', e);
        YG2Instance('PurchaseConsumeFailed', token);
    }
}
