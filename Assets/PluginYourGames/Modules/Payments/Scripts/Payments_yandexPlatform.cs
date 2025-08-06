#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern string InitPayments_js();
        public void InitPayments()
        {
            YG2.sendMessage.PaymentsEntries(InitPayments_js());
        }

        [DllImport("__Internal")]
        private static extern void BuyPayments_js(string id);
        public void BuyPayments(string id)
        {
            BuyPayments_js(id);
        }

        [DllImport("__Internal")]
        private static extern void ConsumePurchase_js(bool onPurchaseSuccess);
        public void ConsumePurchases(bool onPurchaseSuccess)
        {
#if !UNITY_EDITOR
            ConsumePurchase_js(onPurchaseSuccess);
#endif
        }

        [DllImport("__Internal")]
        private static extern void ConsumePurchase_js(string id, bool onPurchaseSuccess);
        public void ConsumePurchaseByID(string id, bool onPurchaseSuccess)
        {
#if !UNITY_EDITOR
            ConsumePurchase_js(id, onPurchaseSuccess);
#endif
        }
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        class JsonPayments
        {
            public string[] id;
            public string[] title;
            public string[] description;
            public string[] imageURI;
            public string[] price;
            public string[] priceValue;
            public string[] priceCurrencyCode;
            public string[] currencyImageURL;
            public bool[] consumed;
            public string language;
        }

        public void PaymentsEntries(string data)
        {
            if (data == InfoYG.NO_DATA)
                return;

            JsonPayments paymentsData = UnityEngine.JsonUtility.FromJson<JsonPayments>(data);
            YG2.purchases = new Utils.Pay.Purchase[paymentsData.id.Length];

            for (int i = 0; i < YG2.purchases.Length; i++)
            {
                YG2.purchases[i] = new Utils.Pay.Purchase();
                YG2.purchases[i].id = paymentsData.id[i];
                YG2.purchases[i].title = paymentsData.title[i];
                YG2.purchases[i].description = paymentsData.description[i];
                YG2.purchases[i].imageURI = paymentsData.imageURI[i];
                YG2.purchases[i].price = paymentsData.price[i];
                YG2.purchases[i].priceValue = paymentsData.priceValue[i];
                YG2.purchases[i].priceCurrencyCode = paymentsData.priceCurrencyCode[i];
                YG2.purchases[i].currencyImageURL = paymentsData.currencyImageURL[i];
                YG2.purchases[i].consumed = paymentsData.consumed[i];
            }
            YG2.langPayments = paymentsData.language;

            YG2.onGetPayments?.Invoke();
        }

        public void OnPurchaseSuccess(string id)
        {
            YG2.PurchaseByID(id).consumed = true;
            YGInsides.OnPurchaseSuccess(id);
        }

        public void OnPurchaseFailed(string id)
        {
            YGInsides.OnPurchaseFailed(id);
        }
    }
}
#endif