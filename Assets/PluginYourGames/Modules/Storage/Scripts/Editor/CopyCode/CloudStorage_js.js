var cloudSaves = NO_DATA;

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

function LoadCloud() {
    return new Promise((resolve) => {
        if (ysdk == null) {
            Final(NO_DATA);
            return;
        }
        try {
            player.getData(["saves"]).then(data => {
                if (data.saves) {
                    Final(JSON.stringify(data.saves));
                } else {
                    Final(NO_DATA);
                }
            }).catch(() => {
                console.error('Load Cloud Error!');
                Final(NO_DATA);
            });
        }
        catch (e) {
            console.error('CRASH Load saves Cloud: ', e.message);
            Final(NO_DATA);
        }

        function Final(res) {
            cloudSaves = res;
            YG2Instance('SetLoadSaves', res);
            resolve(res);
        }
    });
}
