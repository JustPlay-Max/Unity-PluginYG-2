mergeInto(LibraryManager.library,
{
	SetFullscreen_js: function (fullscreen) {
		if (ysdk !== null) {
			if (fullscreen) {
				if (ysdk.screen.fullscreen.status != 'on')
					ysdk.screen.fullscreen.request();
			}
			else if (ysdk.screen.fullscreen.status != 'off')
				ysdk.screen.fullscreen.exit();
		}
	},
	
	IsFullscreen_js: function () {
		if (ysdk !== null) {
			if (ysdk.screen.fullscreen.status == 'on')
				return true;
			else
				return false;
		}
		return false;
	}
});