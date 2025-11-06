#if PLATFORM_WEBGL
using UnityEngine;
using System.Runtime.InteropServices;

namespace YG.Insides
{
    public static class UnadjustedMovement
    {
        [DllImport("__Internal")]
        public static extern void UnadjustedMovement_js();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Enabling()
        {
            UnadjustedMovement_js();
        }
    }
}
#endif