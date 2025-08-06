
mergeInto(LibraryManager.library,
{
	InitPlayer_js: function ()
	{
		var returnStr = playerData;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
	
	OpenAuthDialog_js: function ()
	{
		OpenAuthDialog();
	},
	
	RequestAuth_js: function () {
        InitPlayer();
    }
});