mergeInto(LibraryManager.library,
{
	ExitTVGame_js: function() {
		ysdk.dispatchEvent(ysdk.EVENTS.EXIT);
	}
});