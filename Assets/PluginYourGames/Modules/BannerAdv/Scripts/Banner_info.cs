using System;
using UnityEngine;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        public BannerAdvSettings BannerAdv = new BannerAdvSettings();

        [Serializable]
        public partial class BannerAdvSettings
        {
#if UNITY_EDITOR
            [HeaderYG(Langs.simulation, 5)]
            public bool simulationInEditor = true;

            [NestedYG(nameof(simulationInEditor)), Min(50)]
            public int size = 100;

            [NestedYG(nameof(simulationInEditor))]
            public Color color = new Color(0, 1, 0, 0.5f);
#endif
        }
    }
}