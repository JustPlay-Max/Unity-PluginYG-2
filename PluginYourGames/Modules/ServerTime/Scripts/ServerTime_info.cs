#if UNITY_EDITOR
using System;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        public ServerTimeSettings ServerTime;

        [Serializable]
        public partial class ServerTimeSettings
        {
            [HeaderYG(Langs.simulation, 5)]
            public long serverTime = 1721201231000;
        }
    }
}
#endif