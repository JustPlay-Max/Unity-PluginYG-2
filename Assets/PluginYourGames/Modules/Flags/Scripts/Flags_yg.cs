using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YG
{
    public partial class YG2
    {
        public static Flag[] flags = new Flag[0];

        public static Dictionary<string, string> flagsDictionary
        {
            get
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                for (int i = 0; i < flags.Length; i++)
                    dictionary.Add(flags[i].name, flags[i].value);

                return dictionary;
            }
        }

        [Serializable]
        public struct Flag
        {
            public string name;
            public string value;
        }

        [InitYG]
        private static void InitFlags()
        {
#if !UNITY_EDITOR
            iPlatform.InitFlags();
#else
            flags = infoYG.Flags.flags;
#endif
        }

        public static string GetFlag(string name)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                if (flags[i].name == name)
                    return flags[i].value;
            }
            return null;
        }

        public static bool TryGetFlag(string name, out string flag)
        {
            flag = string.Empty;
            for (int i = 0; i < flags.Length; i++)
            {
                if (flags[i].name == name)
                {
                    flag = flags[i].value;
                    return true;
                }
            }
            return false;
        }

        public static bool TryGetFlagAsInt(string name, out int flagValue)
        {
            flagValue = 0;

            if (TryGetFlag(name, out string flag))
            {
                string cleanedFlag = Regex.Replace(flag, @"[^\d+-]", "");

                if (int.TryParse(cleanedFlag, out int parsedValue))
                {
                    flagValue = parsedValue;
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetFlagAsFloat(string name, out float flagValue)
        {
            flagValue = 0f;

            if (TryGetFlag(name, out string flag))
            {
                string cleanedFlag = Regex.Replace(flag, @"[^\d+-.]", "");

                cleanedFlag = cleanedFlag.Replace(",", ".");

                if (float.TryParse(cleanedFlag, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float parsedValue))
                {
                    flagValue = parsedValue;
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetFlagAsBool(string name, out bool flagValue)
        {
            flagValue = false;

            for (int i = 0; i < flags.Length; i++)
            {
                if (flags[i].name == name)
                {
                    string rawValue = flags[i].value.Trim();

                    if (bool.TryParse(rawValue.ToLower(), out bool boolResult))
                    {
                        flagValue = boolResult;
                        return true;
                    }

                    if (rawValue == "0")
                    {
                        flagValue = false;
                        return true;
                    }
                    if (rawValue == "1")
                    {
                        flagValue = true;
                        return true;
                    }

                    break;
                }
            }

            return false;
        }

    }
}