
mergeInto(LibraryManager.library,
{
	InitEnvironmentData_js: function ()
	{
		var returnStr = environmentData;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},	
	
	RequestingEnvironmentData_js: function () {
        RequestingEnvironmentData();
    },
	
	GeneralEnvirData_js: function () {
		var language = navigator.language || navigator.userLanguage || "en";
		var browserLang = navigator.language || "en";

		var deviceType = "desktop";
		var isMobile = false;
		var isTablet = false;
		var isTV = false;

		var userAgent = navigator.userAgent.toLowerCase();
		if (/mobile/i.test(userAgent)) {
			deviceType = "mobile";
			isMobile = true;
		} else if (/tablet/i.test(userAgent)) {
			deviceType = "tablet";
			isTablet = true;
		} else if (/tv/i.test(userAgent)) {
			deviceType = "tv";
			isTV = true;
		}

		var isDesktop = !isMobile && !isTablet && !isTV;

		var urlParams = new URLSearchParams(window.location.search);
		var payload = urlParams.has('payload') ? urlParams.get('payload') : "";

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

		var envirData = {
			"language": language,
			"deviceType": deviceType,
			"isMobile": isMobile,
			"isDesktop": isDesktop,
			"isTablet": isTablet,
			"isTV": isTV,
			"browserLang": browserLang,
			"payload": payload,
			"platform": navigator.platform,
			"browser": browser
		};

		var returnStr = JSON.stringify(envirData);
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	}
});