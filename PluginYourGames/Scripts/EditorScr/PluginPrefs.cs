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

        public static void Load()
        {
            if (File.Exists(savePath))
            {
                var json = File.ReadAllText(savePath);
                data = JsonUtility.FromJson<PluginPrefsData>(json);

                stringPrefs.Clear();
                intPrefs.Clear();
                floatPrefs.Clear();

                foreach (var pair in data.stringPrefs) stringPrefs[pair.key] = pair.value;
                foreach (var pair in data.intPrefs) intPrefs[pair.key] = pair.value;
                foreach (var pair in data.floatPrefs) floatPrefs[pair.key] = pair.value;
            }
            else
            {
                data = new PluginPrefsData();
                Save();
            }
        }

        private static void Save()
        {
            data.stringPrefs.Clear();
            data.intPrefs.Clear();
            data.floatPrefs.Clear();

            foreach (var kvp in stringPrefs) data.stringPrefs.Add(new StringPair { key = kvp.Key, value = kvp.Value });
            foreach (var kvp in intPrefs) data.intPrefs.Add(new IntPair { key = kvp.Key, value = kvp.Value });
            foreach (var kvp in floatPrefs) data.floatPrefs.Add(new FloatPair { key = kvp.Key, value = kvp.Value });

            var json = JsonUtility.ToJson(data, true);
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            File.WriteAllText(savePath, json);
            UnityEditor.AssetDatabase.Refresh();
        }

        public static void SetString(string key, string value)
        {
            stringPrefs[key] = value;
            Save();
        }

        public static string GetString(string key, string defaultValue = "")
        {
            return stringPrefs.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void SetInt(string key, int value)
        {
            intPrefs[key] = value;
            Save();
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            return intPrefs.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void SetFloat(string key, float value)
        {
            floatPrefs[key] = value;
            Save();
        }

        public static float GetFloat(string key, float defaultValue = 0f)
        {
            return floatPrefs.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void DeleteKey(string key)
        {
            stringPrefs.Remove(key);
            intPrefs.Remove(key);
            floatPrefs.Remove(key);
            Save();
        }

        public static void DeleteAll()
        {
            stringPrefs.Clear();
            intPrefs.Clear();
            floatPrefs.Clear();
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                UnityEditor.AssetDatabase.Refresh();
            }
        }
    }
}
#endif