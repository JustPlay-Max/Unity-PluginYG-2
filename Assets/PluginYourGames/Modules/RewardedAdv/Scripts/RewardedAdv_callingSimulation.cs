#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace YG.Insides
{
    public partial class AdvCallingSimulation
    {
        public static void RewardedAdvOpen(string id)
        {
            AdvCallingSimulation call = CreateCallSimulation();
            call.StartCoroutine(call.RewardedAdvOpen(YG2.infoYG.Simulation.durationAdv, id));
        }

        public IEnumerator RewardedAdvOpen(float duration, string id)
        {
            yield return new WaitForSecondsRealtime(YG2.infoYG.Simulation.loadAdv);

            YGInsides.OpenRewardedAdv();
            if (!YG2.infoYG.Simulation.testFailAds)
                DrawScreen(new Color(0, 0, 1, 0.5f));
            else
                DrawScreen(new Color(1, 0, 0, 0.5f));

            yield return new WaitForSecondsRealtime(duration);
            YGInsides.RewardAdv(id);
            YGInsides.CloseRewardedAdv();
            Destroy(gameObject);
        }
    }
}
#endif