var environmentData = NO_DATA;

function RequestingEnvironmentData() {
    return new Promise((resolve) => {
        if (ysdk == null) {
            Final(NO_DATA);
            return;
        }
        try {
            var browser = navigator.userAgent;
            if (browser.includes('YaBrowser') || browser.includes('YaSearchBrowser'))
                browser = 'Yandex';
            else if (browser.includes('Opera') || browser.includes('OPR'))
                browser = 'Opera';
            else if (browser.includes('Firefox'))
                browser = 'Firefox';
            else if (browser.includes('MSIE'))
                browser = 'IE';
            else if (browser.includes('Edge'))
                browser = 'Edge';
            else if (browser.includes('Chrome'))
                browser = 'Chrome';
            else if (browser.includes('Safari'))
                browser = 'Safari';
            else
                browser = 'Other';

            let jsonEnvir = {
                "language": ysdk.environment.i18n.lang,
                "domain": ysdk.environment.i18n.tld,
                "deviceType": ysdk.deviceInfo.type,
                "isMobile": ysdk.deviceInfo.isMobile(),
                "isDesktop": ysdk.deviceInfo.isDesktop(),
                "isTablet": ysdk.deviceInfo.isTablet(),
                "isTV": ysdk.deviceInfo.isTV(),
                "appID": ysdk.environment.app.id,
                "browserLang": ysdk.environment.browser.lang,
                "payload": ysdk.environment.payload,
                "platform": navigator.platform,
                "browser": browser
            };

            Final(JSON.stringify(jsonEnvir));
            LogStyledMessage("Environment Data: " + JSON.stringify(jsonEnvir));
        }
        catch (e) {
            console.error('CRASH Requesting Environment Data: ', e.message);
            Final(NO_DATA);
        }

        function Final(res) {
            environmentData = res;
            YG2Instance('SetEnvirData', res);
            resolve(res);
        }
    });
}