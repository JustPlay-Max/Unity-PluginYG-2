const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');
const path = require('node:path');

const source = fs.readFileSync(path.join(__dirname,
    '../Assets/PluginYourGames/Modules/Payments/Scripts/Editor/CopyCode/Payments_js.js'), 'utf8');
const purchase = (id = 'coins', token = 'token') => ({ productID: id, purchaseToken: token });
const failure = () => Promise.reject(new Error('Payment failure'));
const tick = () => new Promise(resolve => setImmediate(resolve));

async function setup({ auto = true, overrides = {}, onMessage } = {}) {
    const events = [];
    const consumed = [];
    const sdk = {
        getCatalog: async () => [],
        getPurchases: async () => [purchase()],
        purchase: async () => purchase(),
        consumePurchase: async token => consumed.push(token),
        ...overrides
    };
    const context = vm.createContext({
        NO_DATA: 'NO_DATA',
        ysdk: { getPayments: async () => sdk, environment: { i18n: { lang: 'en' } } },
        console: { warn() {}, error() {} }, LogStyledMessage() {}, FocusGame() {},
        YG2Instance: (method, data) => {
            events.push([method, data]);
            if (onMessage) onMessage(method, data, context, consumed);
        }
    });
    vm.runInContext(source.replace('let paymentsAutoConsume = true;', `let paymentsAutoConsume = ${auto};`), context);
    await context.InitPayments();
    events.length = 0;
    return { context, sdk, events, consumed };
}

test('all init SDK rejections finish with NO_DATA', async () => {
    for (const method of ['getCatalog', 'getPurchases']) {
        const { context, events } = await setup({ overrides: { [method]: failure } });
        assert.equal(await context.InitPayments(), 'NO_DATA');
        assert.deepEqual(events, [['PaymentsEntries', 'NO_DATA']]);
    }
    const { context } = await setup();
    context.ysdk.getPayments = failure;
    assert.equal(await context.InitPayments(), 'NO_DATA');
});

test('automatic purchase grants before consuming and consumes only returned transaction', async () => {
    const { context, events, consumed } = await setup({
        overrides: { getPurchases: async () => [purchase(), purchase('coins', 'older')] },
        onMessage: (method, data, context, consumed) => {
            if (method === 'OnPurchaseSuccess') assert.equal(consumed.length, 0);
        }
    });
    await context.BuyPayments('coins');
    assert.deepEqual(consumed, ['token']);
    assert.equal(events.filter(e => e[0] === 'OnPurchaseSuccess').length, 1);
});

test('manual purchase and pending list never consume or send legacy reward', async () => {
    const { context, events, consumed } = await setup({ auto: false });
    await context.BuyPayments('coins');
    await context.GetPendingPurchases();
    assert.deepEqual(consumed, []);
    assert.equal(events.some(e => e[0] === 'OnPurchaseSuccess'), false);
    assert.deepEqual(JSON.parse(events.find(e => e[0] === 'PurchaseReceived')[1]), purchase());
    assert.deepEqual(JSON.parse(events.find(e => e[0] === 'PendingPurchasesReceived')[1]), { purchases: [purchase()] });
    events.length = 0;
    await context.ConsumePurchaseByToken('token');
    assert.deepEqual(consumed, ['token']);
    assert.deepEqual(events, [['PurchaseConsumed', 'token']]);
});

test('concurrent and reentrant legacy calls grant and consume once per token', async () => {
    const { context, events, consumed } = await setup({
        onMessage: (method, data, context) => {
            if (method === 'OnPurchaseSuccess') void context.ConsumePurchase(data, true);
        }
    });
    await Promise.all([context.ConsumePurchases(true), context.ConsumePurchase('coins', true)]);
    await tick();
    await context.ConsumePurchases(true); // A stale SDK list must not re-grant.
    assert.deepEqual(consumed, ['token']);
    assert.equal(events.filter(e => e[0] === 'OnPurchaseSuccess').length, 1);
});

test('failed automatic confirmation can retry without re-granting or reporting failed payment', async () => {
    const { context, sdk, events, consumed } = await setup({ overrides: { consumePurchase: failure } });
    await context.BuyPayments('coins');
    sdk.consumePurchase = async token => consumed.push(token);
    await context.ConsumePurchases(true);
    assert.deepEqual(consumed, ['token']);
    assert.equal(events.filter(e => e[0] === 'OnPurchaseSuccess').length, 1);
    assert.equal(events.some(e => e[0] === 'OnPurchaseFailed'), false);
});

test('batch failure does not prevent confirming other tokens', async () => {
    const { context, consumed } = await setup({ overrides: {
        getPurchases: async () => [purchase('bad', 'bad'), purchase('good', 'good')],
        consumePurchase: async token => {
            if (token === 'bad') throw new Error('Payment failure');
            consumed.push(token);
        }
    } });
    await context.ConsumePurchases(true);
    assert.deepEqual(consumed, ['good']);
});

test('manual API reports read and confirmation errors without reward events', async () => {
    const { context, events } = await setup({ auto: false, overrides: {
        getPurchases: failure, consumePurchase: failure
    } });
    await context.GetPendingPurchases();
    await context.ConsumePurchaseByToken('token');
    assert.deepEqual(events, [['PendingPurchasesFailed', 'Payment failure'], ['PurchaseConsumeFailed', 'token']]);
});

test('concurrent explicit confirmations share one SDK request', async () => {
    const { context, consumed } = await setup({ auto: false });
    await Promise.all([context.ConsumePurchaseByToken('token'), context.ConsumePurchaseByToken('token')]);
    assert.deepEqual(consumed, ['token']);
});

test('purchase rejection sends failure and never consumes', async () => {
    const { context, events, consumed } = await setup({ overrides: { purchase: failure } });
    await context.BuyPayments('coins');
    assert.deepEqual(consumed, []);
    assert.equal(events.filter(e => e[0] === 'OnPurchaseFailed').length, 1);
});

test('synchronous SDK failures are handled', async () => {
    const { context, events } = await setup({ overrides: {
        consumePurchase: () => { throw new Error('sync failure'); }
    } });
    await context.ConsumePurchaseByToken('token');
    assert.deepEqual(events, [['PurchaseConsumeFailed', 'token']]);
});

test('initialization finishes even if the Unity initialization callback throws', async () => {
    const { context } = await setup({ onMessage: method => {
        if (method === 'PaymentsEntries') throw new Error('callback failure');
    } });
    assert.notEqual(await context.InitPayments(), 'NO_DATA');
});

test('jslib exposes pending query and token confirmation to C#', async () => {
    const { context, events, consumed } = await setup({ auto: false });
    context.LibraryManager = { library: {} };
    context.mergeInto = (target, library) => Object.assign(target, library);
    context.UTF8ToString = value => value;
    const bridge = fs.readFileSync(path.join(__dirname,
        '../Assets/PluginYourGames/Modules/Payments/Plugins/Payments.jslib'), 'utf8');
    vm.runInContext(bridge, context);
    context.LibraryManager.library.GetPendingPurchases_js();
    await tick();
    assert.equal(events[0][0], 'PendingPurchasesReceived');
    context.LibraryManager.library.ConsumePurchaseByToken_js('token');
    await tick();
    assert.deepEqual(consumed, ['token']);
    assert.deepEqual(events[1], ['PurchaseConsumed', 'token']);
});

