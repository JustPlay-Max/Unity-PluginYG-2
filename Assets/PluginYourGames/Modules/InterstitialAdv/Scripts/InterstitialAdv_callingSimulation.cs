#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace YG.Insides
{
    public partial class AdvCallingSimulation
    {
        public static void InterstitialAdvOpen()
        {
            AdvCallingSimulation call = CreateCallSimulation();
            call.StartCoroutine(call.InterstitialAdvOpen(YG2.infoYG.Simulation.durationAdv));
        }

        public IEnumerator InterstitialAdvOpen(float duration)
        {
            yield return new WaitForSecondsRealtime(YG2.infoYG.Simulation.loadAdv);
            YGInsides.OpenInterAdv();

            if (!YG2.infoYG.Simulation.testFailAds)
                DrawScreen(new Color(0, 1, 0, 0.5f));
            else
                DrawScreen(new Color(1, 0, 0, 0.5f));

            yield return new WaitForSecondsRealtime(duration);

            if (!YG2.infoYG.Simulation.testFailAds)
                YGInsides.CloseInterAdv(true);
            else
                YGInsides.CloseInterAdv(false);

            Destroy(gameObject);
        }
    }
}
#endif