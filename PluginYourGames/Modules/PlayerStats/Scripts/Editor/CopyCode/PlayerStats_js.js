
var statsSaves = NO_DATA;

function GetStats() {
    return new Promise((resolve) => {
        if (ysdk == null) {
            Final(NO_DATA);
            return;
        }
        try {
            player.getStats()
                .then(stats => {
                    Final(JSON.stringify(stats));
                }).catch(e => {
                    console.error('GetStats Error!', e.message);
                    Final(NO_DATA);
                });
        }
        catch (e) {
            console.error('CRASH GetStats: ', e.message);
            Final(NO_DATA);
        }

        function Final(res) {
            statsSaves = res;
            YG2Instance('ReceiveStats', res);
            resolve(res);
        }
    });
}

function SetStats(jsonStats) {
    if (ysdk == null || player == null) {
        console.error('SetStats: Error initialization');
        return;
    }
    try {
        player.setStats(JSON.parse(jsonStats));
    } catch (e) {
        console.error('CRASH SetStats:', e.message);
    }
}