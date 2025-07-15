#if UNITY_EDITOR
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
            [HeaderYG(Langs.simulation, 5), Min(0)]
            public float durationPayPanel = 1;
#if RU_YG2
            [Tooltip("Провека неудачной покупки. Консумирование в Unity Editor не симулируется!")]
#else
            [Tooltip("Verification of a failed purchase. Consummation in Unity Editor is not simulated!")]
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
        }
    }
}
#endif