using System;
using UnityEngine;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        public InterstitialAdvSettings InterstitialAdv = new InterstitialAdvSettings();

        [Serializable]
        public partial class InterstitialAdvSettings
        {
#if RU_YG2
            [Tooltip("Показывать рекламу при загрузке игры? (Первая реклама при открытии игры). В Unity Editor первая реклама симулироваться не будет - чтобы не мешала. В Яндекс Играх первый показ рекламы регулируется платформой, по этому значение данной опции для ЯИ не имеет значения.")]
#else
            [Tooltip("Should I show ads when loading the game? (The first advertisement when opening the game). In Unity Editor, the first advertisement will not be simulated - so as not to interfere. In Yandex Games, the first display of ads is regulated by the platform, so this option is disabled for Yandex Games.")]
#endif
            public bool showFirstAdv;
#if RU_YG2
            [Tooltip("Интервал запросов на вызов interstitial рекламу в секундах.")]
#else
            [Tooltip("The interval of requests to call interstitial adv in seconds.")]
#endif
            [Min(0)]
            public int interAdvInterval = 60;
#if RU_YG2
            [Tooltip("Когда таймер закончился и можно показать рекламу, отправляется запрос на открытие рекламы. Платформа может отказать в запросе. В таком случае, по умолчанию - при следующих выполнениях метода показа рекламы будут отправляться новые запросы, пока реклама не будет успешно показана.\n\nНо вы можете установить таймер после провального запроса. Например, поставьте таймер на 10 секунд, и если реклама не была показана, то будет поставлен таймер на следующий запрос рекламы.")]
#else
            [Tooltip("When the timer is over and the ad can be shown, a request is sent to open the ad. The platform may refuse the request. In this case, by default, new requests will be sent during the next execution of the ad display method until the ad is successfully displayed.\n\nBut you can set a timer after a failed request. For example, set a timer for 10 seconds, and if the ad was not shown, a timer will be set for the next ad request.")]
#endif
            public bool postponeCallByFail;
#if UNITY_EDITOR
            [NestedYG(nameof(postponeCallByFail)), Min(1)]
#endif
            public int postponeCallTimer = 10;

#if UNITY_EDITOR
            [SerializeField, LabelYG(Langs.advSimLabel)]
            private bool labelAdvSimLabel;
#endif
        }
    }
}