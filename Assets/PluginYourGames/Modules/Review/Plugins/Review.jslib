mergeInto(LibraryManager.library,
{
	InitReview_js: function()
	{
		var returnStr = reviewData;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
	
	Review_js: function()
	{
		Review();
	}
});