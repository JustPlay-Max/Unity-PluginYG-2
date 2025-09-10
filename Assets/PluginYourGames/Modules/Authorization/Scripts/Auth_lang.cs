#if UNITY_EDITOR
namespace YG.Insides
{
    public static partial class Langs
    {
#if RU_YG2
        public const string t_playerPhotoSize = "Размер подкачанного изображения пользователя.";
        public const string t_payingStatus = "Поле `YG2.PayingStatus` — имеет четыре возможных значения, зависящих от частоты и объема покупок пользователя:\n\n •  paying\nПользователь купил портальную валюту на сумму более 500 рублей за последний месяц.\n\n •  partially_paying\nУ пользователя была хотя бы одна покупка портальной валюты реальными деньгами за последний год.\n\n •  not_paying\nПользователь не делал покупок портальной валюты реальными деньгами за последний год.\n\n •  unknown\nПользователь не из РФ или он не разрешил передачу такой информации разработчику. В плагине unknown ещё означает — неавторизованный.";
#else
        public const string t_playerPhotoSize = "The size of the user's uploaded image.";
        public const string t_payingStatus = "The `YG2.PayingStatus` field has four possible values, depending on the frequency and volume of user purchases: \n\n • paying \n The user bought a portal currency worth more than 500 rubles in the last month. \n\n • partially_paying \n The user had at least one purchase of a portal currency with real money for the last year. \n\n • not_paying \n The user has not made purchases of the portal currency with real money in the last year. \n\n • unknown \n The user is not from the Russian Federation or he did not allow the transfer of such information to the developer. In the plugin, unknown also means unauthorized.";
#endif
    }
}
#endif