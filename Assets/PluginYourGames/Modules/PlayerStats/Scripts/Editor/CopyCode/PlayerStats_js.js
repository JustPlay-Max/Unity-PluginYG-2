
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

function SetStats(jsonStats, requestID) {
    function Complete(success) {
        if (typeof requestID === 'number') {
            YG2Instance('SetStatsResult', requestID + '|' + (success ? '1' : '0'));
        }
    }

    try {
        if (typeof ysdk === 'undefined' || ysdk == null ||
            typeof player === 'undefined' || player == null) {
            throw new Error('PlayerStats are not initialized');
        }

        Promise.resolve(player.setStats(JSON.parse(jsonStats))).then(
            () => Complete(true),
            error => {
                console.error('SetStats Error:', error);
                Complete(false);
            }
        );
    } catch (e) {
        console.error('CRASH SetStats:', e.message);
        Complete(false);
    }
}

function IncrementStat(key, value, requestID) {
    function Complete(success, result) {
        YG2Instance('IncrementStatsResult', requestID + '|' + (success ? '1|' + result : '0'));
    }

    try {
        if (typeof ysdk === 'undefined' || ysdk == null ||
            typeof player === 'undefined' || player == null) {
            throw new Error('PlayerStats are not initialized');
        }

        const increments = { [key]: value };
        Promise.resolve(player.incrementStats(increments))
            .then(stats => {
                if (stats && typeof stats[key] === 'number')
                    return stats;

                // Some SDK responses confirm the write without returning the updated value.
                return player.getStats([key]);
            })
            .then(stats => {
                const result = stats && stats[key];
                if (typeof result !== 'number') {
                    console.error('IncrementStats: SDK did not return a numeric value for key:', key);
                    Complete(false);
                    return;
                }
                if (!Number.isInteger(result) || result < -2147483648 || result > 2147483647) {
                    console.error('IncrementStats: saved value is not an Int32:', result);
                    Complete(false);
                    return;
                }
                Complete(true, result);
            },
            error => {
                console.error('IncrementStats Error:', error);
                Complete(false);
            });
    } catch (e) {
        console.error('CRASH IncrementStats:', e.message);
        Complete(false);
    }
}
