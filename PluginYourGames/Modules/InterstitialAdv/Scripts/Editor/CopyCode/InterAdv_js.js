
let nowFullAdOpen = false;

function InterAdvShow() {
    try {
        if (ysdk == null){
            LogStyledMessage('Cancel InterAdvShow: SDK is not initialized');
            return;
        }
        if (nowFullAdOpen == true){
            LogStyledMessage('Cancel InterAdvShow: The advertisement is already open');
            return;
        }

        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onOpen: () => {
                    LogStyledMessage('Open Interstitial Adv');
                    nowFullAdOpen = true;
                    if (initGame === true) {
                        YG2Instance('OpenInterAdv');
                    }
                },
                onClose: (wasShown) => {
                    LogStyledMessage('Close Interstitial Adv');
                    nowFullAdOpen = false;
                    if (initGame === true) {
                        if (wasShown) {
                            YG2Instance('CloseInterAdv', 'true');
                        }
                            else {
                            YG2Instance('CloseInterAdv', 'false');
                        }
                    }
                    FocusGame();
                },
                onError: (error) => {
                    console.error('Error Interstitial Adv', error);
                    nowFullAdOpen = false;
                    YG2Instance('ErrorInterAdv');
                    FocusGame();
                }
            }
        });
    }
    catch (e) {
        console.error('CRASH Interstitial Adv Show: ', e.message);
    }
}