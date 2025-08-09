mergeInto(LibraryManager.library,
{
	ServerTime_js: function() {
        if (ysdk !== null) {
            var serverTime = ysdk.serverTime().toString();
            var lengthBytes = lengthBytesUTF8(serverTime) + 1;
            var stringOnWasmHeap = _malloc(lengthBytes);
            stringToUTF8(serverTime, stringOnWasmHeap, lengthBytes);
            return stringOnWasmHeap;
        }
        return 0;
    }	
});