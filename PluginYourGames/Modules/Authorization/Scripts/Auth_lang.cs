#if UNITY_EDITOR
namespace YG.Insides
{
    public static partial class Langs
    {
#if RU_YG2
        //public const string t_scopes = "При инициализации объекта Player авторизованному игроку будет показано диалоговое окно с запросом на предоставление доступа к персональным данным. Запрашивается доступ только к аватару и имени, идентификатор пользователя всегда передается автоматически. Примерное содержание: Игра запрашивает доступ к вашему аватару и имени пользователя на сервисах Яндекса.\nЕсли вам достаточно знать идентификатор, а имя и аватар пользователя не нужны, используйте опциональный параметр scopes: false. В этом случае диалоговое окно не будет показано.";
        public const string t_playerPhotoSize = "Размер подкачанного изображения пользователя.";
        public const string t_payingStatus = "Поле `YG2.PayingStatus` — имеет четыре возможных значения, зависящих от частоты и объема покупок пользователя:\n\n •  paying\nПользователь купил портальную валюту на сумму более 500 рублей за последний месяц.\n\n •  partially_paying\nУ пользователя была хотя бы одна покупка портальной валюты реальными деньгами за последний год.\n\n •  not_paying\nПользователь не делал покупок портальной валюты реальными деньгами за последний год.\n\n •  unknown\nПользователь не из РФ или он не разрешил передачу такой информации разработчику. В плагине unknown ещё означает — неавторизованный.";
#else
        //public const string t_scopes = "When initializing the Player object, an authorized player will be shown a dialog box requesting access to personal data. Access is requested only to the avatar and name, the user ID is always transmitted automatically. Approximate content: The game requests access to your avatar and username on Yandex services.\nIf it's enough for you to know the ID, but you don't need the user's name and avatar, use the optional scopes: false parameter. In this case, the dialog box will not be displayed.";
        public const string t_playerPhotoSize = "The size of the user's uploaded image.";
        public const string t_payingStatus = "The `YG2.PayingStatus` field has four possible values, depending on the frequency and volume of user purchases: \n\n • paying \n The user bought a portal currency worth more than 500 rubles in the last month. \n\n • partially_paying \n The user had at least one purchase of a portal currency with real money for the last year. \n\n • not_paying \n The user has not made purchases of the portal currency with real money in the last year. \n\n • unknown \n The user is not from the Russian Federation or he did not allow the transfer of such information to the developer. In the plugin, unknown also means unauthorized.";
#endif
    }
}
#endif