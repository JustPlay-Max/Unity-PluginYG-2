mergeInto(LibraryManager.library,
{
	InitCloud_js: function()
	{
		var returnStr = cloudSaves;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
	
	LoadCloud_js: function () {
        LoadCloud();
    },	

	SaveCloud_js: function (jsonData, flush)
	{
		SaveCloud(UTF8ToString(jsonData), flush);
	}
});