using System.Collections.Generic;

namespace YG.Utils.Metrica
{
    public static class JsonUtils
    {
        public static string ToJson(IDictionary<string, object> dictionary)
        {
            var jsonString = "{";
            var kvpCount = 0;

            foreach (var kvp in dictionary)
            {
                if (kvp.Value == null || string.IsNullOrEmpty(kvp.Key)) continue;

                if (kvp.Value is IDictionary<string, object> nestedDict)
                {
                    jsonString += $"\"{kvp.Key}\":{ToJson(nestedDict)},";
                }
                else
                {
                    jsonString += $"\"{kvp.Key}\":{GetValueString(kvp.Value)},";
                }
                kvpCount++;
            }

            if (kvpCount == 0) return string.Empty;

            if (dictionary.Count > 0)
            {
                jsonString = jsonString.Remove(jsonString.Length - 1);
            }

            jsonString += "}";

            return jsonString;
        }

        public static string ToJson(Dictionary<string, string> dictionary)
        {
            var jsonString = "{";
            var kvpCount = 0;

            foreach (var kvp in dictionary)
            {
                if (string.IsNullOrEmpty(kvp.Key) || string.IsNullOrEmpty(kvp.Value)) continue;

                jsonString += $"\"{kvp.Key}\":\"{kvp.Value}\",";
                kvpCount++;
            }

            if (kvpCount == 0) return string.Empty;

            if (dictionary.Count > 0)
            {
                jsonString = jsonString.Remove(jsonString.Length - 1);
            }

            jsonString += "}";

            return jsonString;
        }

        private static string GetValueString(object value)
        {
            if (value is int || value is float || value is double)
            {
                return value.ToString();
            }

            if (value is bool boolValue)
            {
                return boolValue.ToString().ToLower();
            }

            if (value is string stringValue)
            {
                return $"\"{stringValue.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
            }

            if (value is IList<object> listValue)
            {
                var listString = new List<string>();
                foreach (var item in listValue)
                {
                    listString.Add(GetValueString(item));
                }
                return "[" + string.Join(",", listString) + "]";
            }

            return $"\"{value}\"";
        }

        [System.Serializable]
        public class SerializableDictionary<TKey, TValue>
        {
            public List<TKey> keys = new List<TKey>();
            public List<TValue> values = new List<TValue>();

            public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
            {
                foreach (var kvp in dictionary)
                {
                    keys.Add(kvp.Key);
                    values.Add(kvp.Value);
                }
            }
        }
    }
}