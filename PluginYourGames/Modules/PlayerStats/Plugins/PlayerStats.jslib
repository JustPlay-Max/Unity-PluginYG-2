mergeInto(LibraryManager.library,
{
	InitStats_js: function () {
        var returnStr = statsSaves;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
    },
	
	GetStats_js: function () {
        GetStats();
    },
	
	SetStats_js: function (jsonStats) {
        SetStats(UTF8ToString(jsonStats));
    }
});