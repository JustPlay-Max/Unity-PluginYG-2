using System;
using UnityEngine;
using YG.Utils.Pay;

namespace YG.Insides
{
    public static partial class PaymentsJsonYG
    {
        [Serializable]
        private class JsonPayments
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

        public static bool ApplyCatalog(string data)
        {
            if (string.IsNullOrEmpty(data) || data == InfoYG.NO_DATA)
                return false;

            JsonPayments paymentsData;
            try
            {
                paymentsData = JsonUtility.FromJson<JsonPayments>(data);
            }
            catch (Exception e)
            {
                Debug.LogWarning("[PluginYG Payments] Catalog parse failed: " + e.Message);
                return false;
            }

            if (paymentsData == null || paymentsData.id == null)
                return false;

            YG2.purchases = new Purchase[paymentsData.id.Length];

            for (int i = 0; i < YG2.purchases.Length; i++)
            {
                YG2.purchases[i] = new Purchase
                {
                    id = Read(paymentsData.id, i),
                    title = Read(paymentsData.title, i),
                    description = Read(paymentsData.description, i),
                    imageURI = Read(paymentsData.imageURI, i),
                    price = Read(paymentsData.price, i),
                    priceValue = Read(paymentsData.priceValue, i),
                    priceCurrencyCode = Read(paymentsData.priceCurrencyCode, i),
                    currencyImageURL = Read(paymentsData.currencyImageURL, i),
                    consumed = Read(paymentsData.consumed, i, true)
                };
            }

            if (!string.IsNullOrEmpty(paymentsData.language))
                YG2.langPayments = paymentsData.language;

            YG2.onGetPayments?.Invoke();
            return true;
        }

        private static string Read(string[] array, int index)
        {
            if (array == null || index >= array.Length)
                return string.Empty;
            return array[index] ?? string.Empty;
        }

        private static bool Read(bool[] array, int index, bool defaultValue)
        {
            if (array == null || index >= array.Length)
                return defaultValue;
            return array[index];
        }
    }
}
