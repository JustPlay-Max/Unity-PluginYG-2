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

	SaveCloudWithCallback_js: function (jsonData, requestID)
	{
		SaveCloud(UTF8ToString(jsonData), true, requestID);
	},
	SaveCloud_js: function (jsonData, flush)
	{
		SaveCloud(UTF8ToString(jsonData), flush);
	}
});
