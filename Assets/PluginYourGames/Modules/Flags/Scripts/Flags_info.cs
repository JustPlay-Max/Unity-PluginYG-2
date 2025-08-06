#if UNITY_EDITOR
using System;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        public FlagsSettings Flags;

        [Serializable]
        public partial class FlagsSettings
        {
            [HeaderYG(Langs.simulation, 5)]
            public YG2.Flag[] flags;
        }
    }
}
#endif