#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace YG.EditorScr
{
    public static class PrefsList
    {
        public enum StoreType
        {
            EditorPrefs,
            SessionState,
            PluginPrefs
        }

        // Получить список
        public static List<string> GetList(string key, StoreType store = StoreType.EditorPrefs)
        {
            string str = GetString(key, store).Trim();
            if (string.IsNullOrEmpty(str))
                return new List<string>();

            return str.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim())
                      .ToList();
        }

        // Проверить наличие
        public static bool Contains(string key, string element, StoreType store = StoreType.EditorPrefs)
        {
            var list = GetList(key, store);
            return list.Contains(element);
        }

        // Добавить элемент
        public static void Add(string key, string element, StoreType store = StoreType.EditorPrefs)
        {
            var list = GetList(key, store);
            if (!list.Contains(element))
            {
                list.Add(element);
                SaveList(key, list, store);
            }
        }

        // Удалить элемент
        public static void Remove(string key, string element, StoreType store = StoreType.EditorPrefs)
        {
            var list = GetList(key, store);
            if (list.Remove(element))
            {
                SaveList(key, list, store);
            }
        }

        // Очистить список
        public static void Clear(string key, StoreType store = StoreType.EditorPrefs)
        {
            SaveList(key, new List<string>(), store);
        }

        // ---------- Внутреннее ----------

        private static void SaveList(string key, List<string> list, StoreType store)
        {
            string str = string.Join(";", list);
            SetString(key, str, store);
        }

        private static string GetString(string key, StoreType store)
        {
            switch (store)
            {
                case StoreType.SessionState:
                    return SessionState.GetString(key, string.Empty);
                case StoreType.EditorPrefs:
                default:
                    return EditorPrefs.GetString(key, string.Empty);
            }
        }

        private static void SetString(string key, string value, StoreType store)
        {
            switch (store)
            {
                case StoreType.SessionState:
                    SessionState.SetString(key, value ?? string.Empty);
                    break;
                case StoreType.EditorPrefs:
                    EditorPrefs.SetString(key, value ?? string.Empty);
                    break;
                case StoreType.PluginPrefs:
                    EditorPrefs.SetString(key, value ?? string.Empty);
                    break;
            }
        }
    }
}
#endif