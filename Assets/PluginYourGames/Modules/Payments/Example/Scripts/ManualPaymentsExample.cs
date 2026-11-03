#if Storage_yg
using System.Collections.Generic;
using UnityEngine;
using YG.Utils.Pay;

namespace YG.Example
{
    // Пример запускается после загрузки сохранений. В Payments выключите AutoConsume.
    public class ManualPaymentsExample : MonoBehaviour
    {
        public string coinsProductID = "coins100";
        [Min(1)] public int coinsPerPurchase = 100;
        public string permanentProductID = "remove_ads";

        public int Coins => YG2.saves.paymentsExampleCoins;
        public bool PermanentUnlocked => YG2.saves.paymentsExamplePermanent;

        private readonly Queue<PaymentTransaction> pending = new Queue<PaymentTransaction>();
        private bool saving;

        private void OnEnable()
        {
            YG2.onPurchaseReceived += ReceivePurchase;
            YG2.onGetPendingPurchases += ReceivePendingPurchases;
            YG2.onPurchaseFailed += PurchaseFailed;
            YG2.onGetPendingPurchasesFailed += PendingPurchasesFailed;
            YG2.onPurchaseConsumed += PurchaseConsumed;
            YG2.onPurchaseConsumeFailed += PurchaseConsumeFailed;
        }

        private void OnDisable()
        {
            YG2.onPurchaseReceived -= ReceivePurchase;
            YG2.onGetPendingPurchases -= ReceivePendingPurchases;
            YG2.onPurchaseFailed -= PurchaseFailed;
            YG2.onGetPendingPurchasesFailed -= PendingPurchasesFailed;
            YG2.onPurchaseConsumed -= PurchaseConsumed;
            YG2.onPurchaseConsumeFailed -= PurchaseConsumeFailed;
        }

        private void Start() => RestorePurchases();

        [ContextMenu("Payments/Buy coins")]
        public void BuyCoins() => YG2.BuyPayments(coinsProductID);

        [ContextMenu("Payments/Buy permanent product")]
        public void BuyPermanent()
        {
            if (!PermanentUnlocked) YG2.BuyPayments(permanentProductID);
        }

        [ContextMenu("Payments/Restore purchases")]
        public void RestorePurchases() => YG2.GetPendingPurchases();

        private void ReceivePendingPurchases(PaymentTransaction[] purchases)
        {
            foreach (var purchase in purchases)
                ReceivePurchase(purchase);
        }

        private void ReceivePurchase(PaymentTransaction purchase)
        {
            pending.Enqueue(purchase);
            ProcessNext();
        }

        private void ProcessNext()
        {
            if (saving) return;
            while (pending.Count > 0)
            {
                var purchase = pending.Dequeue();
                bool isCoins = purchase.productID == coinsProductID;
                bool isPermanent = purchase.productID == permanentProductID;
                if ((!isCoins && !isPermanent) || string.IsNullOrEmpty(purchase.purchaseToken))
                {
                    Debug.LogWarning("Неизвестная покупка оставлена без подтверждения: " + purchase.productID);
                    continue;
                }

                var saves = YG2.saves;
                if (saves.paymentsExampleTokens == null)
                    saves.paymentsExampleTokens = new List<string>();

                if (isCoins && !saves.paymentsExampleTokens.Contains(purchase.purchaseToken))
                {
                    // Награда и токен сохраняются вместе. Повторный токен не добавляет монеты.
                    saves.paymentsExampleCoins += coinsPerPurchase;
                    saves.paymentsExampleTokens.Add(purchase.purchaseToken);
                }
                if (isPermanent)
                    saves.paymentsExamplePermanent = true;

                saving = true;
                YG2.SaveProgress(success =>
                {
                    saving = false;
                    if (!success)
                    {
                        pending.Clear();
                        Debug.LogWarning("Сохранение не подтверждено. Повторите RestorePurchases после восстановления связи.");
                        return;
                    }
                    if (this == null) return;
                    Debug.Log($"Покупка сохранена. Монеты: {Coins}, постоянный товар: {PermanentUnlocked}");
                    if (isCoins)
                        YG2.ConsumePurchaseByToken(purchase.purchaseToken);
                    // Постоянный товар остаётся на платформе для восстановления.
                    ProcessNext();
                });
                return;
            }
        }

        private void PurchaseFailed(string id) => Debug.LogWarning("Оплата не завершена: " + id);
        private void PendingPurchasesFailed(string error) => Debug.LogWarning("Ошибка получения покупок: " + error);
        private void PurchaseConsumed(string token) => Debug.Log("Подтверждена покупка: " + token);
        private void PurchaseConsumeFailed(string token) => Debug.LogWarning("Повторите RestorePurchases для подтверждения: " + token);
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public int paymentsExampleCoins;
        public bool paymentsExamplePermanent;
        public List<string> paymentsExampleTokens = new List<string>();
    }
}
#endif
