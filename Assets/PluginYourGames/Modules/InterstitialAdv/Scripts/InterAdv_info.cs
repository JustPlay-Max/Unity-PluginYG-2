using System;
using UnityEngine;
using YG.Insides;
using YG.Localization;

namespace YG
{
    public partial class InfoYG
    {
        public InterstitialAdvSettings InterstitialAdv = new InterstitialAdvSettings();

        [Serializable]
        public partial class InterstitialAdvSettings
        {
            [Tooltip(InterstitialAdvLocalization.ShowFirstAdv)]
            public bool showFirstAdv;

            [Tooltip(InterstitialAdvLocalization.InterAdvInterval)]
            [Min(0)] public int interAdvInterval = 60;

            [Tooltip(InterstitialAdvLocalization.PostponeCallByFail)]
            public bool postponeCallByFail;

#if UNITY_EDITOR
            [NestedYG(nameof(postponeCallByFail)), Min(1)]
#endif
            public int postponeCallTimer = 10;

#if UNITY_EDITOR
            [SerializeField, LabelYG(Langs.advSimLabel)]
            private bool labelAdvSimLabel;
#endif
        }
    }
}