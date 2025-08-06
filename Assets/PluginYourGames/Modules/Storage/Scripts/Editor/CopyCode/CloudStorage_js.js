var cloudSaves = NO_DATA;

function SaveCloud(jsonData, flush) {
    if (player == null) {
        console.error('CRASH Save Cloud: ', 'Didnt have time to load');
        return;
    }
    try {
        player.setData({
            saves: [jsonData],
        }, flush);
    } catch (e) {
        console.error('CRASH Save Cloud: ', e.message);
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