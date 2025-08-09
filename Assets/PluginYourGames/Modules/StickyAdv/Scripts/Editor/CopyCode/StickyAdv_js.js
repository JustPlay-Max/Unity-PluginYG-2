
function StickyAdActivity(show) {
    try {
        ysdk.adv.getBannerAdvStatus().then(({ stickyAdvIsShowing, reason }) => {
            if (stickyAdvIsShowing) {
                if (!show) {
                    ysdk.adv.hideBannerAdv();
                }
            }
            else if (reason) {
                LogStyledMessage('StickyAdv are not shown. Reason:', reason);
            }
            else if (show) {
                ysdk.adv.showBannerAdv();
            }
        })
    } catch (e) {
        console.error('CRASH StickyAdv activity: ', e.message);
    }
}