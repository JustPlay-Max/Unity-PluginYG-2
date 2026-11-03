using System;
using YG.Insides;
using System.Collections.Generic;
using YG.Utils.Pay;

namespace YG
{
    public partial class YG2
    {
        public static Purchase[] purchases = new Purchase[0];
        public static string langPayments = "ru";

        public static Action onGetPayments;
        public static Action<string> onPurchaseSuccess;
        public static Action<string> onPurchaseFailed;
        public static Action<PaymentTransaction> onPurchaseReceived;
        public static Action<PaymentTransaction[]> onGetPendingPurchases;
        public static Action<string> onGetPendingPurchasesFailed;
        public static Action<string> onPurchaseConsumed;
        public static Action<string> onPurchaseConsumeFailed;
#if UNITY_EDITOR
        private static readonly List<PaymentTransaction> simulatedPendingPurchases = new List<PaymentTransaction>();
#endif

        [InitYG]
        private static void InitPayments()
        {
#if UNITY_EDITOR
            // Reset static for ECS
            onGetPayments = null;
            onPurchaseSuccess = null;
            onPurchaseFailed = null;
            onPurchaseReceived = null;
            onGetPendingPurchases = null;
            onGetPendingPurchasesFailed = null;
            onPurchaseConsumed = null;
            onPurchaseConsumeFailed = null;
            simulatedPendingPurchases.Clear();

            purchases = infoYG.Payments.purshases;
            langPayments = "ru";
#if EnvirData_yg
            langPayments = infoYG.Simulation.language;
#endif
            onGetPayments?.Invoke();
#else
            iPlatform.InitPayments();
#endif
        }

        public static void BuyPayments(string id)
        {
#if !UNITY_EDITOR
            iPlatform.BuyPayments(id);
#else
            Message($"Buy Payment. ID: {id}");
            AdvCallingSimulation.PaymentsBuy(id);
#endif
        }

        public static void ConsumePurchases(bool onPurchaseSuccess)
        {
            iPlatform.ConsumePurchases(onPurchaseSuccess);
        }
        public static void ConsumePurchases()
        {
            iPlatform.ConsumePurchases(true);
        }

        public static void ConsumePurchaseByID(string id, bool onPurchaseSuccess)
        {
            iPlatform.ConsumePurchaseByID(id, onPurchaseSuccess);
        }
        public static void ConsumePurchaseByID(string id)
        {
            iPlatform.ConsumePurchaseByID(id, true);
        }

        /// <summary>Fetches unconsumed transactions without granting or consuming them.</summary>
        public static void GetPendingPurchases()
        {
#if UNITY_EDITOR
            onGetPendingPurchases?.Invoke(simulatedPendingPurchases.ToArray());
#else
            iPlatform.GetPendingPurchases();
#endif
        }

        /// <summary>Confirms one transaction after its reward and processed token have been saved.</summary>
        public static void ConsumePurchaseByToken(string purchaseToken)
        {
            if (string.IsNullOrEmpty(purchaseToken))
            {
                onPurchaseConsumeFailed?.Invoke(purchaseToken);
                return;
            }
#if UNITY_EDITOR
            int index = simulatedPendingPurchases.FindIndex(p => p.purchaseToken == purchaseToken);
            if (index < 0)
            {
                onPurchaseConsumeFailed?.Invoke(purchaseToken);
                return;
            }
            simulatedPendingPurchases.RemoveAt(index);
            onPurchaseConsumed?.Invoke(purchaseToken);
#else
            iPlatform.ConsumePurchaseByToken(purchaseToken);
#endif
        }

#if UNITY_EDITOR
        internal static void ReceiveSimulatedPurchase(string id)
        {
            if (infoYG.Payments.AutoConsume)
            {
                YGInsides.OnPurchaseSuccess(id);
                return;
            }
            var transaction = new PaymentTransaction { productID = id, purchaseToken = Guid.NewGuid().ToString("N") };
            simulatedPendingPurchases.Add(transaction);
            try { onPurchaseReceived?.Invoke(transaction); }
            finally { PauseGame(false); }
        }
#endif

        public static Purchase PurchaseByID(string ID)
        {
            for (int i = 0; i < purchases.Length; i++)
            {
                if (purchases[i].id == ID)
                {
                    return purchases[i];
                }
            }

            return null;
        }
    }
}

namespace YG.Utils.Pay
{
    [Serializable]
    public class PaymentTransaction
    {
        public string productID;
        public string purchaseToken;
    }

    [Serializable]
    internal class PaymentTransactionList
    {
        public PaymentTransaction[] purchases;
    }

    [Serializable]
    public class Purchase
    {
        public string id;
        public string title;
        public string description;
        public string imageURI;
        public string price;
        public string priceValue;
        public string priceCurrencyCode;
        public string currencyImageURL;
        public bool consumed;
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public static void OnPurchaseSuccess(string id)
        {
            YG2.onPurchaseSuccess?.Invoke(id);
            YG2.PauseGame(false);
        }

        public static void OnPurchaseFailed(string id)
        {
            YG2.onPurchaseFailed?.Invoke(id);
            YG2.PauseGame(false);
        }
    }
}
