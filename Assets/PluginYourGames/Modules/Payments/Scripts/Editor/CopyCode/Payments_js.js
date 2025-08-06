var paymentsData = NO_DATA;
let payments = null;

function InitPayments(sendback) {
    return new Promise((resolve) => {
        try {
            if (ysdk == null) {
                Final(NO_DATA);
                return;
            }

            ysdk.getPayments().then(_payments => {
                payments = _payments;

                payments.getCatalog()
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

                        payments.getPurchases().then(purchases => {
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
                                for (i2 = 0; i2 < purchases.length; i2++) {
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
                LogStyledMessage('Purchases are not available', e.message);
                Final(NO_DATA);
            })
        } catch (e) {
            console.error('CRASH Init Payments: ', e.message);
            Final(NO_DATA);
        }

        function Final(res) {
            paymentsData = res;
            YG2Instance('PaymentsEntries', res);
            resolve(res);
        }
    });
}

function BuyPayments(id) {
    try {
        if (payments != null) {
            payments.purchase(id).then(() => {
                LogStyledMessage('Purchase Success');
                ConsumePurchase(id, true);
                FocusGame();
            }).catch(e => {
                console.error('Purchase Failed', e.message);
                YG2Instance('OnPurchaseFailed', id);
                FocusGame();
            })
        }
        else {
            LogStyledMessage('Payments == null');
            YG2Instance('OnPurchaseFailed', id);
        }
    } catch (e) {
        console.error('CRASH Buy Payments: ', e.message);
        YG2Instance('OnPurchaseFailed', id);
        FocusGame();
    }
}

function ConsumePurchase(id, onPurchaseSuccess) {
    try {
        if (payments != null) {
            payments.getPurchases().then(purchases => {
                for (i = 0; i < purchases.length; i++) {
                    if (purchases[i].productID === id) {
                        payments.consumePurchase(purchases[i].purchaseToken);

                        if (onPurchaseSuccess)
                            YG2Instance('OnPurchaseSuccess', id);
                    }
                }
            });
        }
        else {
            LogStyledMessage('Consume purchase: payments null');
        }
    } catch (e) {
        console.error('CRASH Consume Purchase: ', e.message);
    }
}

function ConsumePurchases(onPurchaseSuccess) {
    try {
        if (payments != null) {
            payments.getPurchases().then(purchases => {
                LogStyledMessage('Unprocessed purchases: ', purchases.length);
                for (i = 0; i < purchases.length; i++) {
                    payments.consumePurchase(purchases[i].purchaseToken);

                    if (onPurchaseSuccess)
                        YG2Instance('OnPurchaseSuccess', purchases[i].productID);
                }
            });
        }
        else {
            LogStyledMessage('Consume purchases: payments null');
        }
    } catch (e) {
        console.error('CRASH Consume purchases: ', e.message);
    }
}