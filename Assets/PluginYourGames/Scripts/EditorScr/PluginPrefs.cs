#if UNITY_EDITOR
namespace YG.EditorScr
{
    using UnityEngine;
    using System;
    using System.IO;
    using System.Collections.Generic;

    [Serializable]
    public class StringPair { public string key; public string value; }
    [Serializable]
    public class IntPair { public string key; public int value; }
    [Serializable]
    public class FloatPair { public string key; public float value; }

    [Serializable]
    public class PluginPrefsData
    {
        public List<StringPair> stringPrefs = new();
        public List<IntPair> intPrefs = new();
        public List<FloatPair> floatPrefs = new();
    }

    public static class PluginPrefs
    {
        private static PluginPrefsData data;
        private static readonly string savePath = Path.Combine(InfoYG.PATCH_PC_EDITOR, "PluginPrefs.json");

        private static Dictionary<string, string> stringPrefs = new();
        private static Dictionary<string, int> intPrefs = new();
        private static Dictionary<string, float> floatPrefs = new();
        private static bool loaded;

        public static void Load()
        {
            stringPrefs.Clear();
            intPrefs.Clear();
            floatPrefs.Clear();

            if (File.Exists(savePath))
            {
                try
                {
                    var json = File.ReadAllText(savePath);
                    data = JsonUtility.FromJson<PluginPrefsData>(json);
                }
                catch
                {
                    data = null;
                }

                if (data == null)
                    data = new PluginPrefsData();

                EnsureDataCollections();

                if (data.stringPrefs != null)
                    foreach (var pair in data.stringPrefs)
                        if (pair != null && !string.IsNullOrEmpty(pair.key))
                            stringPrefs[pair.key] = pair.value;

                if (data.intPrefs != null)
                    foreach (var pair in data.intPrefs)
                        if (pair != null && !string.IsNullOrEmpty(pair.key))
                            intPrefs[pair.key] = pair.value;

                if (data.floatPrefs != null)
                    foreach (var pair in data.floatPrefs)
                        if (pair != null && !string.IsNullOrEmpty(pair.key))
                            floatPrefs[pair.key] = pair.value;
            }
            else
            {
                data = new PluginPrefsData();
                Save();
            }

            loaded = true;
        }

        private static void Save()
        {
            EnsureDataCollections();

            data.stringPrefs.Clear();
            data.intPrefs.Clear();
            data.floatPrefs.Clear();

            foreach (var kvp in stringPrefs) data.stringPrefs.Add(new StringPair { key = kvp.Key, value = kvp.Value });
            foreach (var kvp in intPrefs) data.intPrefs.Add(new IntPair { key = kvp.Key, value = kvp.Value });
            foreach (var kvp in floatPrefs) data.floatPrefs.Add(new FloatPair { key = kvp.Key, value = kvp.Value });

            var json = JsonUtility.ToJson(data, true);
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            if (File.Exists(savePath) && File.ReadAllText(savePath) == json)
                return;

            File.WriteAllText(savePath, json);
        }

        private static void EnsureDataCollections()
        {
            if (data == null)
                data = new PluginPrefsData();

            if (data.stringPrefs == null)
                data.stringPrefs = new List<StringPair>();

            if (data.intPrefs == null)
                data.intPrefs = new List<IntPair>();

            if (data.floatPrefs == null)
                data.floatPrefs = new List<FloatPair>();
        }

        public static void SetString(string key, string value)
        {
            EnsureLoaded();
            stringPrefs[key] = value;
            Save();
        }

        public static string GetString(string key, string defaultValue = "")
        {
            EnsureLoaded();
            return stringPrefs.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void SetInt(string key, int value)
        {
            EnsureLoaded();
            intPrefs[key] = value;
            Save();
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            EnsureLoaded();
            return intPrefs.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void SetFloat(string key, float value)
        {
            EnsureLoaded();
            floatPrefs[key] = value;
            Save();
        }

        public static float GetFloat(string key, float defaultValue = 0f)
        {
            EnsureLoaded();
            return floatPrefs.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void DeleteKey(string key)
        {
            EnsureLoaded();
            stringPrefs.Remove(key);
            intPrefs.Remove(key);
            floatPrefs.Remove(key);
            Save();
        }

        public static void DeleteAll()
        {
            EnsureLoaded();
            stringPrefs.Clear();
            intPrefs.Clear();
            floatPrefs.Clear();
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                UnityEditor.AssetDatabase.Refresh();
            }
        }

        private static void EnsureLoaded()
        {
            if (!loaded)
                Load();
        }
    }
}
#endif
