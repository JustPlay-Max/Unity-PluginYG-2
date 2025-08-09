#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace YG.Insides
{
    public partial class AdvCallingSimulation
    {
        public static void PaymentsBuy(string id)
        {
            AdvCallingSimulation call = CreateCallSimulation();
            call.StartCoroutine(call.PaymentsBuy(YG2.infoYG.Payments.durationPayPanel, id));
        }

        public IEnumerator PaymentsBuy(float duration, string id)
        {
            yield return new WaitForSecondsRealtime(YG2.infoYG.Simulation.loadAdv);

            if (!YG2.infoYG.Payments.testBuyFail)
            {
                DrawScreen(new Color(1, 0, 1, 0.5f));
            }
            else
            {
                DrawScreen(new Color(1, 0, 0, 0.5f));
            }

            yield return new WaitForSecondsRealtime(duration);

            if (!YG2.infoYG.Payments.testBuyFail)
            {
                YGInsides.OnPurchaseSuccess(id);
            }
            else
            {
                YG2.Message($"Buy Payment - Fail Test");
                YGInsides.OnPurchaseFailed(id);
            }

            Destroy(gameObject);
        }
    }
}
#endif