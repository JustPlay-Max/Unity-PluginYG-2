const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');
const tick = () => new Promise(resolve => setImmediate(resolve));
const read = file => fs.readFileSync(path.join(__dirname, '..', file), 'utf8');

function setup(player) {
    const events = [];
    const context = vm.createContext({
        player, NO_DATA: 'NO_DATA', console: { warn() {} },
        YG2Instance: (...args) => events.push(args),
        LibraryManager: { library: {} },
        mergeInto: (target, value) => Object.assign(target, value),
        UTF8ToString: value => value
    });
    vm.runInContext(read('Assets/PluginYourGames/Modules/Storage/Scripts/Editor/CopyCode/CloudStorage_js.js'), context);
    vm.runInContext(read('Assets/PluginYourGames/Modules/Storage/Plugins/CloudStorage.jslib'), context);
    return { context, events, bridge: context.LibraryManager.library };
}

test('callback save forces flush and waits for server response', async () => {
    let resolve;
    let sent;
    const { bridge, events } = setup({ setData: (data, flush) => {
        sent = JSON.parse(JSON.stringify({ data, flush }));
        return new Promise(done => { resolve = done; });
    } });
    bridge.SaveCloudWithCallback_js('{"coins":100}', 7);
    assert.deepEqual(sent, { data: { saves: ['{"coins":100}'] }, flush: true });
    assert.deepEqual(events, []);
    resolve(); await tick();
    assert.deepEqual(events, [['SetSaveCloudResult', '7|1']]);
});

test('concurrent saves return their own result even in reverse completion order', async () => {
    const requests = [];
    const { bridge, events } = setup({ setData: () => new Promise((resolve, reject) => requests.push({ resolve, reject })) });
    bridge.SaveCloudWithCallback_js('first', 1);
    bridge.SaveCloudWithCallback_js('second', 2);
    requests[1].resolve(); await tick();
    requests[0].reject(new Error('network')); await tick();
    assert.deepEqual(events, [['SetSaveCloudResult', '2|1'], ['SetSaveCloudResult', '1|0']]);
});

test('missing player, synchronous errors and rejected promises report failure', async () => {
    for (const player of [null,
        { setData() { throw new Error('sync'); } },
        { setData: () => Promise.reject(new Error('network')) }]) {
        const { bridge, events } = setup(player);
        bridge.SaveCloudWithCallback_js('data', 3); await tick();
        assert.deepEqual(events, [['SetSaveCloudResult', '3|0']]);
    }
});

test('legacy saves retain configured flush and do not emit callbacks', async () => {
    const flushValues = [];
    const { bridge, events } = setup({ setData: (data, flush) => {
        flushValues.push(flush);
        return Promise.reject(new Error('network'));
    } });
    bridge.SaveCloud_js('data', false);
    bridge.SaveCloud_js('data', true);
    await tick();
    assert.deepEqual(flushValues, [false, true]);
    assert.deepEqual(events, []);
});

test('an exception delivering the cloud callback does not reject SaveCloud', async () => {
    const { context } = setup({ setData: () => Promise.resolve() });
    context.YG2Instance = () => { throw new Error('Unity callback'); };
    await context.SaveCloud('data', true, 4);
});

test('browser local writes return success or quota failure', () => {
    const context = vm.createContext({
        LibraryManager: { library: {} },
        mergeInto: (target, value) => Object.assign(target, value),
        UTF8ToString: value => value,
        console: { warn() {} },
        localStorage: { setItem(key, value) { assert.equal(key, 'saves'); assert.equal(value, 'data'); } }
    });
    vm.runInContext(read('Assets/PluginYourGames/Scripts/Utils/Plugins/LocalStorage.jslib'), context);
    assert.equal(context.LibraryManager.library.TrySetKey_LocalStorage_js('saves', 'data'), 1);
    context.localStorage.setItem = () => { throw new Error('QuotaExceededError'); };
    assert.equal(context.LibraryManager.library.TrySetKey_LocalStorage_js('saves', 'data'), 0);
});
