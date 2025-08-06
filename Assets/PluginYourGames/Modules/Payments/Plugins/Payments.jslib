mergeInto(LibraryManager.library,
{
	InitPayments_js: function()
	{
		var returnStr = paymentsData;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
	
	ConsumePurchase_js: function(id, onPurchaseSuccess)
	{
		ConsumePurchase(UTF8ToString(id, onPurchaseSuccess));
	},
	
	ConsumePurchase_js: function(onPurchaseSuccess)
	{
		ConsumePurchases(onPurchaseSuccess);
	},
	
	BuyPayments_js: function(id)
	{
		BuyPayments(UTF8ToString(id));
	}
});