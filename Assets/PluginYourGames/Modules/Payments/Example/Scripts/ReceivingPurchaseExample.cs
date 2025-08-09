using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YG.Example
{
    public class ReceivingPurchaseExample : MonoBehaviour
    {
        public Text textExample;

        // Пример Unity событий, на которые можно подписать,
        // например, открытие уведомление о успешности совершения покупки
        public UnityEvent successPurchased;
        public UnityEvent failedPurchased;

        private void OnEnable()
        {
            YG2.onPurchaseSuccess += SuccessPurchased;
            YG2.onPurchaseFailed += FailedPurchased;
        }

        private void OnDisable()
        {
            YG2.onPurchaseSuccess -= SuccessPurchased;
            YG2.onPurchaseFailed -= FailedPurchased;
        }

        private void SuccessPurchased(string id)
        {
            successPurchased?.Invoke();

            textExample.text = "Success purchase - " + id;

            // Ваш код для обработки покупки. Например:

            //string coinsKey = "coins";
            //int coins = YG2.GetState(coinsKey);

            //if (id == "50")
            //    YG2.SetState(coinsKey, coins + 50);
            //else if (id == "250")
            //    YG2.SetState(coinsKey, coins + 250);
            //else if (id == "1500")
            //    YG2.SetState(coinsKey, coins + 1500);
        }

        private void FailedPurchased(string id)
        {
            failedPurchased?.Invoke();

            textExample.text = "Failed purchase - " + id;
        }
    }
}