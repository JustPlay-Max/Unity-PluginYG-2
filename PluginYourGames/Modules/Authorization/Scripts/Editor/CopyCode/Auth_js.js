var playerData = NO_DATA;
let player = null;

async function InitPlayer() {
    return new Promise(async (resolve) => {
        try {
            if (!ysdk)
                return Final(NotAuthorized(false));

            player = await ysdk.getPlayer();

            if (!player.isAuthorized())
                return Final(NotAuthorized());

            const authJson = {
                "playerAuth": "resolved",
                "playerName": player.getName(),
                "playerId": player.getUniqueID(),
                "playerPhoto": player.getPhoto('___photoSize___'),
                "payingStatus": player.getPayingStatus()
            };

            return Final(JSON.stringify(authJson));
        } catch (e) {
            console.error('CRASH init Player: ', e.message);
            return Final(NotAuthorized(false));
        }

        function Final(res) {
            playerData = res;
            YG2Instance('SetAuth', res);
            resolve(res);
        }
    });
}


function NotAuthorized(isInitSDK = true) {
    let authJson = {
        "playerAuth": "rejected",
        "playerName": "unauthorized",
        "playerId": isInitSDK ? player.getUniqueID() : "unauthorized",
        "playerPhoto": "no data",
        "payingStatus": "unknown"
    };
    return JSON.stringify(authJson);
}

function OpenAuthDialog() {
    if (ysdk !== null) {
        try {
            ysdk.auth.openAuthDialog().then(() => {
                InitPlayer()
                    .then(() => {
                        YG2Instance('LoggedIn');
                    });
            });
        }
        catch (e) {
            LogStyledMessage('CRASH Open Auth Dialog: ', e.message);
        }
    }
}