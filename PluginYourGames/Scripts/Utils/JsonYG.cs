using System.Collections.Generic;
using System.Text;

namespace YG.Utils
{
    public static class JsonYG
    {
        public static string SerializeDictionary(Dictionary<string, int> dict)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            foreach (var kvp in dict)
                sb.AppendFormat("\"{0}\":{1},", kvp.Key, kvp.Value);

            if (dict.Count > 0)
                sb.Length--;

            sb.Append("}");
            return sb.ToString();
        }

        public static Dictionary<string, int> DeserializeDictionary(string json)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            json = json.Trim('{', '}');
            string[] pairs = json.Split(',');

            foreach (string pair in pairs)
            {
                if (string.IsNullOrEmpty(pair)) continue;

                string[] keyValue = pair.Split(':');
                string key = keyValue[0].Trim('\"');
                int value = int.Parse(keyValue[1]);

                dict[key] = value;
            }

            return dict;
        }
    }
}