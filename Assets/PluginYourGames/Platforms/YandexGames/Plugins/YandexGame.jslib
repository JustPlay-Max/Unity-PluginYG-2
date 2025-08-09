mergeInto(LibraryManager.library,
{
	IsInitSDK_js: function ()
	{
		return initYSDK ? 1 : 0;
	},
	
	InitGame_js: function ()
	{
		InitGame();
	},
	
	GameReadyAPI_js: function() {
		if (ysdk !== null && ysdk.features.LoadingAPI !== undefined && ysdk.features.LoadingAPI !== null) {
			ysdk.features.LoadingAPI.ready();
			LogStyledMessage('Game Ready');
		}
		else{
			console.error('Failed - Game Ready');
		}
	},
	
	GameplayStart_js: function () {
		if (ysdk !== null && ysdk.features !== undefined && ysdk.features.GameplayAPI !== undefined) {
			ysdk.features.GameplayAPI.start();
		}
		else {
			if (ysdk == null) console.error('Gameplay start rejected. The SDK is not initialized!');
			else console.error('Gameplay start undefined!');
		}
	},
	
	GameplayStop_js: function () {
		if (ysdk !== null && ysdk.features !== undefined && ysdk.features.GameplayAPI !== undefined) {
			ysdk.features.GameplayAPI.stop();
		}
		else {
			if (ysdk == null) console.error('Gameplay stop rejected. The SDK is not initialized!');
			else console.error('Gameplay stop undefined!');
		}
	},
	
    LogStyledMessage: function(message, style) {
		LogStyledMessage(UTF8ToString(message), UTF8ToString(style));
    },
	
	LogStyledMessage: function(message) {
		LogStyledMessage(UTF8ToString(message));
    }
});