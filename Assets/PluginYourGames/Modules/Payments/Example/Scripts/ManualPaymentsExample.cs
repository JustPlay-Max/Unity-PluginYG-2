// Требуются модули Storage и PlayerStats.
// Для активации скрипта удалите предпроцессор (закомментируйте условие: //&& PAYMENTS_MANUAL_EXAMPLE)
#if Storage_yg && PlayerStats_yg && PAYMENTS_MANUAL_EXAMPLE
using System.Collections.Generic;
using UnityEngine;
using YG.Utils.Pay;

namespace YG.Example
{
    // В Payments выключите AutoConsume. Скрипт сам дождётся готовности SDK.
    public class ManualPaymentsExample : MonoBehaviour
    {
        // Многоразовая покупка: ID пакета монет в каталоге платформы.
        public string coinsProductID = "coins100";
        // Количество монет за одну покупку.
        [Min(1)] public int coinsPerPurchase = 100;
        // Постоянная покупка: ID товара, например отключения рекламы.
        public string permanentProductID = "remove_ads";

        private const string PermanentStatKey = "paymentsExamplePermanent";

        public int Coins => YG2.saves.paymentsExampleCoins;
        // Подтверждённое право: загружено из PlayerStats или успешно сохранено в этой сессии.
        public bool PermanentUnlocked { get; private set; }

        // Полученные покупки ждут в очереди, пока сохраняется предыдущая награда.
        private readonly Queue<PaymentTransaction> pending = new Queue<PaymentTransaction>();
        // Идёт сохранение; следующую покупку пока не обрабатываем.
        private bool saving;

        private void OnEnable()
        {
            if (YG2.isSDKEnabled)
                Initialize();
            else
                YG2.onGetSDKData += Initialize;
        }

        private void Initialize()
        {
            YG2.onGetSDKData -= Initialize;
            YG2.onPurchaseReceived += ReceivePurchase;
            YG2.onGetPendingPurchases += ReceivePendingPurchases;
            YG2.onPurchaseFailed += PurchaseFailed;
            YG2.onGetPendingPurchasesFailed += PendingPurchasesFailed;
            YG2.onPurchaseConsumed += PurchaseConsumed;
            YG2.onPurchaseConsumeFailed += PurchaseConsumeFailed;

            PermanentUnlocked = YG2.GetState(PermanentStatKey) == 1;
            RestorePurchases();
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= Initialize;
            YG2.onPurchaseReceived -= ReceivePurchase;
            YG2.onGetPendingPurchases -= ReceivePendingPurchases;
            YG2.onPurchaseFailed -= PurchaseFailed;
            YG2.onGetPendingPurchasesFailed -= PendingPurchasesFailed;
            YG2.onPurchaseConsumed -= PurchaseConsumed;
            YG2.onPurchaseConsumeFailed -= PurchaseConsumeFailed;
        }

        [ContextMenu("Payments/Buy coins")]
        public void BuyCoins() => YG2.BuyPayments(coinsProductID);

        [ContextMenu("Payments/Buy permanent product")]
        public void BuyPermanent()
        {
            if (!PermanentUnlocked) YG2.BuyPayments(permanentProductID);
        }

        [ContextMenu("Payments/Check pending purchases")]
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
                    Debug.LogWarning("Unknown purchase or missing token; left unconsumed. Product ID: " + purchase.productID);
                    continue;
                }

                saving = true;
                if (isPermanent)
                {
                    // Постоянное право храним в PlayerStats: 1 — открыто. Покупку не потребляем.
                    YG2.SetState(PermanentStatKey, 1, success => CompletePurchase(purchase, success));
                    return;
                }

                var saves = YG2.saves;
                if (saves.paymentsExampleTokens == null)
                    saves.paymentsExampleTokens = new List<string>();

                // Выдаём монеты только за новый токен: повторная доставка не должна дублировать награду.
                if (!saves.paymentsExampleTokens.Contains(purchase.purchaseToken))
                {
                    saves.paymentsExampleCoins += coinsPerPurchase;
                    saves.paymentsExampleTokens.Add(purchase.purchaseToken);
                }
                // Монеты и токены сохраняем вместе в Storage, затем подтверждаем транзакцию.
                YG2.SaveProgress(success => CompletePurchase(purchase, success));
                return;
            }
        }

        private void CompletePurchase(PaymentTransaction purchase, bool success)
        {
            saving = false;
            if (!success)
            {
                // Покупки остаются на платформе для повторного RestorePurchases.
                pending.Clear();
                Debug.LogError("Save not confirmed. Restart the game or click Check pending purchases after restoring the connection.");
                return;
            }
            if (this == null) return;

            if (purchase.productID == permanentProductID)
                PermanentUnlocked = true;
            else
                YG2.ConsumePurchaseByToken(purchase.purchaseToken);

            Debug.Log($"Purchase saved. Coins: {Coins}, permanent product: {PermanentUnlocked}");
            ProcessNext();
        }

        private void PurchaseFailed(string id) => Debug.LogError("Purchase was not completed: " + id);
        private void PendingPurchasesFailed(string error) => Debug.LogError("Failed to retrieve pending purchases: " + error);
        private void PurchaseConsumed(string token) => Debug.Log("Purchase consumed.");
        private void PurchaseConsumeFailed(string token) => Debug.LogError("Failed to consume purchase. Click Check pending purchases to retry.");

        // Тестовая панель видна в Game View, пока компонент включён.
        private void OnGUI()
        {
            float width = Mathf.Min(340, Screen.width - 20);
            var panel = new Rect(Mathf.Max(10, Screen.width - width - 10),
                Mathf.Max(10, Screen.height - 205), width, 195);
            bool previousEnabled = GUI.enabled;

            GUILayout.BeginArea(panel, GUI.skin.box);
            GUILayout.Label("Purchase testing");
            if (YG2.isSDKEnabled)
            {
                GUILayout.Label($"{coinsProductID}: {Coins} (Storage)");
                GUILayout.Label($"{PermanentStatKey}: {(PermanentUnlocked ? 1 : 0)} (PlayerStats)");
            }
            else
            {
                GUILayout.Label("Waiting for SDK...");
            }

            GUI.enabled = previousEnabled && YG2.isSDKEnabled && !saving;
            if (GUILayout.Button($"Buy {coinsProductID}", GUILayout.Height(30)))
                BuyCoins();

            GUI.enabled = previousEnabled && YG2.isSDKEnabled && !saving && !PermanentUnlocked;
            if (GUILayout.Button($"Buy {permanentProductID}", GUILayout.Height(30)))
                BuyPermanent();

            GUI.enabled = previousEnabled && YG2.isSDKEnabled && !saving;
            if (GUILayout.Button("Check pending purchases", GUILayout.Height(30)))
                RestorePurchases();

            GUI.enabled = previousEnabled;
            GUILayout.EndArea();
        }
    }
}

namespace YG
{
    public partial class SavesYG
    {
        // Баланс монет примера.
        public int paymentsExampleCoins;
        // Токены уже выданных пакетов монет: защита от повторного начисления.
        public List<string> paymentsExampleTokens = new List<string>();
    }
}
#endif
