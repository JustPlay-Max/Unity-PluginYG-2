mergeInto(LibraryManager.library,
{
	SetLB_js: function (nameLB, score, extraData)
	{
        SetLeaderboard(UTF8ToString(nameLB), score, UTF8ToString(extraData));
	},
	
	GetLB_js: function (nameLB, quantityTop, quantityAround, photoSizeLB, auth)
	{
		GetLeaderboard(UTF8ToString(nameLB), quantityTop, quantityAround, UTF8ToString(photoSizeLB), auth);
	}
});