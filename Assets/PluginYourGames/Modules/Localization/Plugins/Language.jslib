mergeInto(LibraryManager.library,
{
	LangRequest_js: function ()
	{
		if (ysdk !== null){
			var returnStr = ysdk.environment.i18n.lang;
			var bufferSize = lengthBytesUTF8(returnStr) + 1;
			var buffer = _malloc(bufferSize);
			stringToUTF8(returnStr, buffer, bufferSize);
			return buffer;
		}
		else {
			return '';
		}
	},
	
	GeneralLanguage_js: function ()
	{
		var returnStr = navigator.language || navigator.userLanguage || "en";
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	}
});