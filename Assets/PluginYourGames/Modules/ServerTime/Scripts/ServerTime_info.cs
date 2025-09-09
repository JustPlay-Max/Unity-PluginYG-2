#if UNITY_EDITOR
using System;
using UnityEngine;
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
            public long serverTime => manualTimeSetup ? timeInMs : DateTimeOffset.Now.ToUnixTimeMilliseconds();
#if RU_YG2
            [Tooltip("true - возвращает значение времени заданное в поле ниже.\nfalse - возвращает текущую всемирное время.")]
#else
            [Tooltip("true - returns the time value specified in the field below.\nfalse - returns the current universal time.")]
#endif
            public bool manualTimeSetup = false;
            [NestedYG(nameof(manualTimeSetup))]
            public long timeInMs = 1721201231000;
        }
    }
}
#endif