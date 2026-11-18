using System;
using UnityEngine;
using YG.Insides;
using YG.Utils.Pay;

namespace YG
{
    public partial class InfoYG
    {
        public PaymentsSettings Payments;

        [Serializable]
        public partial class PaymentsSettings
        {
#if RU_YG2
            [Tooltip("Автоматически подтверждать клиентские покупки Яндекс Игр после события выдачи. Отключите для ручной выдачи, сохранения и подтверждения по purchaseToken.")]
#else
            [Tooltip("Automatically consume Yandex Games client purchases after the reward event. Disable to grant, save and confirm manually by purchaseToken.")]
#endif
            public bool AutoConsume = true;

            [HideInInspector]
            public bool useBackend;
            [HideInInspector]
            [NestedYG(nameof(useBackend))]
            public string backendURL = string.Empty;
            [HideInInspector]
            [NestedYG(nameof(useBackend))]
            public bool advancedBackend;
            [HideInInspector]
            [NestedYG(nameof(useBackend), nameof(advancedBackend)), Min(1)]
            public int pollAttempts = 8;
            [HideInInspector]
            [NestedYG(nameof(useBackend), nameof(advancedBackend)), Min(0.2f)]
            public float pollDelay = 1.5f;

#if UNITY_EDITOR
            [HeaderYG(Langs.simulation, 5), Min(0)]
            public float durationPayPanel = 1;
#if RU_YG2
            [Tooltip("Проверка неудачной покупки. В Unity Editor доступна базовая симуляция ручного подтверждения по токену.")]
#else
            [Tooltip("Simulate a failed purchase. Unity Editor supports basic manual confirmation by token.")]
#endif
            public bool testBuyFail;

            public Purchase[] purshases = new Purchase[]
            {
                new Purchase
                {
                    id = "gun",
                    title = "Gun",
                    description = "Product - Gun",
                    imageURI = InfoYG.DEMO_IMAGE,
                    price = "5 YAN",
                    priceValue = "5",
                    priceCurrencyCode = "YAN",
                    currencyImageURL = "https://yastatic.net/s3/games-static/static-data/images/payments/sdk/currency-icon-s@2x.png",
                    consumed = true
                },
                new Purchase
                {
                    id = "armor",
                    title = "Armor",
                    description = "Product - Armor",
                    imageURI = InfoYG.DEMO_IMAGE,
                    price = "10 YAN",
                    priceValue = "10",
                    priceCurrencyCode = "YAN",
                    currencyImageURL = "https://yastatic.net/s3/games-static/static-data/images/payments/sdk/currency-icon-s@2x.png",
                    consumed = true
                },
                new Purchase
                {
                    id = "grenade",
                    title = "Grenade",
                    description = "Product - Grenade",
                    imageURI = InfoYG.DEMO_IMAGE,
                    price = "30 YAN",
                    priceValue = "30",
                    priceCurrencyCode = "YAN",
                    currencyImageURL = "https://yastatic.net/s3/games-static/static-data/images/payments/sdk/currency-icon-s@2x.png",
                    consumed = true
                }
            };
#endif
        }
    }
}
