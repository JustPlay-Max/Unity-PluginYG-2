using YG;
/// using PlayerPrefs = RedefineYG.PlayerPrefs;

namespace RedefineYG
{
    public static class PlayerPrefs
    {
        public static void SetString(string key, string value) => YG2.iPlatform.SetString(key, value);
        public static string GetString(string key) => YG2.iPlatform.GetString(key);
        public static string GetString(string key, string defaultValue) => YG2.iPlatform.GetString(key, defaultValue);

        public static void SetFloat(string key, float value) => YG2.iPlatform.SetFloat(key, value);
        public static float GetFloat(string key) => YG2.iPlatform.GetFloat(key);
        public static float GetFloat(string key, float defaultValue) => YG2.iPlatform.GetFloat(key, defaultValue);

        public static void SetInt(string key, int value) => YG2.iPlatform.SetInt(key, value);
        public static int GetInt(string key) => YG2.iPlatform.GetInt(key);
        public static int GetInt(string key, int defaultValue) => YG2.iPlatform.GetInt(key, defaultValue);

        public static bool HasKey(string key) => YG2.iPlatform.HasKey(key);
        public static void DeleteKey(string key) => YG2.iPlatform.DeleteKey(key);
        public static void DeleteAll() => YG2.iPlatform.DeleteAll();
        public static void Save() => YG2.iPlatform.Save();
    }
}