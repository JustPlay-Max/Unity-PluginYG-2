
var gameLabelData = 'false';

function InitGameLabel() {
    return new Promise((resolve) => {
        if (ysdk == null) {
            resolve('false');
            return;
        }
        try {
            ysdk.shortcut.canShowPrompt().then(prompt => {
                if (prompt.canShow) {
                    gameLabelData = 'true';
                    resolve('true');
                }
                else {
                    resolve('false');
                }
            });
        }
        catch (e) {
            console.error('CRASH Init Game Label: ', e.message);
            resolve('false');
        }
    });
}

function GameLabelShowDialog() {
    try {
        ysdk.shortcut.showPrompt()
            .then(result => {
                LogStyledMessage('Shortcut created?:', result);
                if (result.outcome === 'accepted') {
                    LogStyledMessage('Game Label Success');
                    YG2Instance('OnGameLabelSuccess');
                }
                else {
                    YG2Instance('OnGameLabelFail');
                }
                FocusGame();
            });
    }
    catch (e) {
        console.error('CRASH Game Label Show: ', e.message);
        FocusGame();
    }
}