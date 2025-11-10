mergeInto(LibraryManager.library, {
	ConsumeBonus_js: function (token)
	{
		var tokenStr = UTF8ToString(token);
		ConsumeBonus(tokenStr);
	},

	GetBonusCatalog_js: function ()
	{
		GetBonusCatalog();
	},

	GetPlayerBonuses_js: function ()
	{
		GetPlayerBonuses();
	},
});
