var playerData = NO_DATA;
let player = null;

async function InitPlayer() {
    try {
        if (!ysdk) {
            return Final(NotAuthorized(false));
        }

        player = await ysdk.getPlayer();

        if (!player || !player.isAuthorized()) {
            return Final(NotAuthorized(true));
        }

        const authJson = {
            playerAuth: "resolved",
            playerName: player.getName(),
            playerId: player.getUniqueID(),
            playerPhoto: player.getPhoto('___photoSize___'),
            payingStatus: player.getPayingStatus()
        };

        return Final(JSON.stringify(authJson));
    } catch (e) {
        console.error('CRASH InitPlayer:', e?.message ?? e);
        player = null;
        return Final(NotAuthorized(false));
    }

    function Final(res) {
        playerData = res;
        YG2Instance('SetAuth', res);
        return res;
    }
}

function NotAuthorized(hasPlayer = false) {
    const authJson = {
        playerAuth: "rejected",
        playerName: "unauthorized",
        playerId: hasPlayer && player ? player.getUniqueID() : "unauthorized",
        playerPhoto: "no data",
        payingStatus: "unknown"
    };

    return JSON.stringify(authJson);
}

async function OpenAuthDialog() {
    if (!ysdk) {
        LogStyledMessage('OpenAuthDialog: ysdk is null');
        return;
    }

    try {
        player = await ysdk.getPlayer();

        if (player.isAuthorized()) {
            await InitPlayer();
            YG2Instance('LoggedIn');
            return;
        }

        try {
            await ysdk.auth.openAuthDialog();
            player = await ysdk.getPlayer();

            if (player.isAuthorized()) {
                await InitPlayer();
                YG2Instance('LoggedIn');
            } else {
                await InitPlayer();
                LogStyledMessage('Authorization dialog closed, player is still unauthorized');
            }
        } catch (e) {
            await InitPlayer();
            LogStyledMessage('Authorization canceled or failed:', e?.message ?? e);
        }
    } catch (e) {
        player = null;
        await InitPlayer();
        LogStyledMessage('CRASH OpenAuthDialog / getPlayer:', e?.message ?? e);
    }
}