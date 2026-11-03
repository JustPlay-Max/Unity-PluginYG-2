using UnityEngine;

namespace YG
{
    public class ConsumePurchasesYG : MonoBehaviour
    {
        private static bool consumed;

        private void Start()
        {
            if ((YG2.infoYG.Payments.AutoConsume || YG.Insides.PaymentsBackendYG.Enabled) && !consumed)
            {
                consumed = true;
                YG2.ConsumePurchases();
            }
        }
    }
}
