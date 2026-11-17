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
    },

    SetStatsWithCallback_js: function (jsonStats, requestID) {
        SetStats(UTF8ToString(jsonStats), requestID);
    },

    IncrementStat_js: function (key, value, requestID) {
        IncrementStat(UTF8ToString(key), value, requestID);
    }
});
