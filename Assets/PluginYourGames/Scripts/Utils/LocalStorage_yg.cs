using UnityEngine;
using System.Runtime.InteropServices;

namespace YG.Utils
{
    public static class LocalStorage
    {
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        public static extern void SetKey_LocalStorage_js(string key, string value);
#endif
        public static void SetKey(string key, string value)
        {
#if PLATFORM_WEBGL && !UNITY_EDITOR
            SetKey_LocalStorage_js(key, value);
#else
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
#endif
        }
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        public static extern string GetKey_LocalStorage_js(string key);
#endif
        public static string GetKey(string key, string defaultValue = "")
        {
            if (!HasKey(key))
                return defaultValue;

#if PLATFORM_WEBGL && !UNITY_EDITOR
            return GetKey_LocalStorage_js(key);
#else
            return PlayerPrefs.GetString(key);
#endif
        }
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern int HasKey_LocalStorage_js(string key);
#endif
        public static bool HasKey(string key)
        {
#if PLATFORM_WEBGL && !UNITY_EDITOR
            try
            {
                return HasKey_LocalStorage_js(key) == 1;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
#else
            return PlayerPrefs.HasKey(key);
#endif
        }
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        public static extern void DeleteKey_LocalStorage_js(string key);
#endif
        public static void DeleteKey(string key)
        {
#if PLATFORM_WEBGL && !UNITY_EDITOR
            DeleteKey_LocalStorage_js(key);
#else
            PlayerPrefs.DeleteKey(key);
#endif
        }
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        public static extern void ClearAllKeys_LocalStorage_js();
#endif
        public static void DeleteAll()
        {
#if PLATFORM_WEBGL && !UNITY_EDITOR
            ClearAllKeys_LocalStorage_js();
#else
            PlayerPrefs.DeleteAll();
#endif
        }
    }
}
