namespace YG.Localization
{
        public class InterstitialAdvLocalization
        {
#if RU_YG2
                public const string SecondsPanelObject = "Объект таймера перед показом рекламы. Он будет активироваться и деактивироваться в нужное время.";
                public const string SecondObjects = "Массив объектов, которые будут показываться по очереди через секунду. Сколько объектов вы поместите в массив, столько секунд будет отчитываться перед показом рекламы.\n\nНапример, поместите в массив три объекта: певый с текстом '3', второй с текстом '2', третий с текстом '1'.\nВ таком случае произойдёт отчет трёх секунд с показом объектов с цифрами перед рекламой.";
                public const string NotificationObj = "Объект, который будет активироваться перед открытием рекламы. И деактивироваться при открытии.";
                public const string WaitingForAds = "Максимальное время показа объекта заглушки перед рекламой. Если реклама так и не будет показана, то объект скроется через указанное в данном параметре время.";
                public const string ShowFirstAdv = "Показывать рекламу при загрузке игры? (Первая реклама при открытии игры). В Unity Editor первая реклама симулироваться не будет - чтобы не мешала. В Яндекс Играх первый показ рекламы регулируется платформой, по этому значение данной опции для ЯИ не имеет значения.";
                public const string InterAdvInterval = "Интервал запросов на вызов interstitial рекламу в секундах.";
                public const string PostponeCallByFail = "Когда таймер закончился и можно показать рекламу, отправляется запрос на открытие рекламы. Платформа может отказать в запросе. В таком случае, по умолчанию - при следующих выполнениях метода показа рекламы будут отправляться новые запросы, пока реклама не будет успешно показана.\n\nНо вы можете установить таймер после провального запроса. Например, поставьте таймер на 10 секунд, и если реклама не была показана, то будет поставлен таймер на следующий запрос рекламы.";
#else
                public const string SecondsPanelObject = "The timer object before the ad is shown. It will activate and deactivate at the right time.";
                public const string SecondObjects = "An array of objects that will be displayed in turn in a second. How many objects you put in the array will be reported for as many seconds before the ad is shown.\n\nFor example, put three objects in the array: the left with the text '3', the second with the text '2', the third with the text '1'.\nIn this case, a three-second report will occur showing objects with numbers before advertising.";
                public const string NotificationObj = "The object that will be activated before opening the ad. And deactivate when opened.";
                public const string WaitingForAds = "The maximum time for displaying the stub object before advertising. If the advertisement is not shown, the object will disappear after the time specified in this parameter.";
                public const string ShowFirstAdv = "Should I show ads when loading the game? (The first advertisement when opening the game). In Unity Editor, the first advertisement will not be simulated - so as not to interfere. In Yandex Games, the first display of ads is regulated by the platform, so this option is disabled for Yandex Games.";
                public const string InterAdvInterval = "The interval of requests to call interstitial adv in seconds.";
                public const string PostponeCallByFail = "When the timer is over and the ad can be shown, a request is sent to open the ad. The platform may refuse the request. In this case, by default, new requests will be sent during the next execution of the ad display method until the ad is successfully displayed.\n\nBut you can set a timer after a failed request. For example, set a timer for 10 seconds, and if the ad was not shown, a timer will be set for the next ad request.";
#endif
        }
}