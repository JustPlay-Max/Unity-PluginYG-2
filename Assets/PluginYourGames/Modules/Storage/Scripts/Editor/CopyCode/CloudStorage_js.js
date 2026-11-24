let initialCloudLoadResult = '';
let awaitingInitialCloudLoad = false;

function GetInitialCloudLoadResult() {
    awaitingInitialCloudLoad = initialCloudLoadResult === '';
    return initialCloudLoadResult;
}

async function SaveCloud(jsonData, flush, requestID) {
    let success = false;
    try {
        if (player == null)
            throw new Error('Player is not initialized');
        await player.setData({
            saves: [jsonData],
        }, flush);
        success = true;
    } catch (e) {
        console.warn('Save Cloud failed: ', e);
    }
    if (requestID !== undefined) {
        try {
            YG2Instance('SetSaveCloudResult', requestID + '|' + (success ? '1' : '0'));
        } catch (e) {
            console.warn('Save Cloud callback failed: ', e);
        }
    }
}

async function LoadCloud(reload = false) {
    if (reload)
        awaitingInitialCloudLoad = false;

    let success = false;
    let result;
    try {
        if (ysdk == null)
            throw new Error('Yandex Games SDK is not initialized');
        const data = await player.getData(["saves"]);
        result = data.saves ? JSON.stringify(data.saves) : NO_DATA;
        success = true;
    } catch (e) {
        console.error('Load Cloud Error!', e);
        result = e && e.message ? e.message : String(e);
    }

    if (!reload) {
        initialCloudLoadResult = (success ? '1' : '0') + result;
        // Unity normally consumes the preload from the cache during initialization.
        if (!awaitingInitialCloudLoad)
            return;
        awaitingInitialCloudLoad = false;
    }

    YG2Instance(success ? 'SetLoadSaves' : 'LoadSavesFailed', result);
}
