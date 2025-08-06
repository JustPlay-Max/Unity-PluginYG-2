
function RewardedAdvShow(id) {
    try {
        ysdk.adv.showRewardedVideo(
            {
                callbacks:
                {
                    onOpen: () => {
                        LogStyledMessage('Opened Rewarded Adv');
                        YG2Instance('OpenRewardedAdv');
                    },
                    onClose: () => {
                        LogStyledMessage('Closed Rewarded Adv');
                        YG2Instance('CloseRewardedAdv');
                        FocusGame();
                    },
                    onRewarded: () => {
                        YG2Instance('RewardAdv', id);
                    },
                    onError: (err) => {
                        console.error('Error Rewarded Adv', err);
                        YG2Instance('ErrorRewardedAdv');
                    }
                }
            });
    } catch (err) {
        console.error('CRASH Rewarded Adv Show: ', err.message);
    }
}