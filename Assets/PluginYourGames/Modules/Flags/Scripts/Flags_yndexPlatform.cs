#if YandexGamesPlatform_yg
using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [Serializable]
        private struct JsonFlags
        {
            public string[] names;
            public string[] values;
        }

        [DllImport("__Internal")]
        private static extern string FlagsInit_js();

        public void InitFlags()
        {
            string data = FlagsInit_js();

            if (data == InfoYG.NO_DATA)
                return;

            JsonFlags jsonFlags = JsonUtility.FromJson<JsonFlags>(FlagsInit_js());
            YG2.flags = new YG2.Flag[jsonFlags.names.Length];

            for (int i = 0; i < jsonFlags.names.Length; i++)
            {
                YG2.flags[i].name = jsonFlags.names[i];
                YG2.flags[i].value = jsonFlags.values[i];
            }
        }
    }
}
#endif