mergeInto(LibraryManager.library,
{
	InitGameLabe_js: function()
	{
		var returnStr = gameLabelData;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
	
	GameLabelShowDialog_js: function()
	{
		GameLabelShowDialog();
	}
});