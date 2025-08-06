using YG.Utils;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        // String
        void SetString(string key, string value)
        {
#if !Storage_yg
            LocalStorage.SetKey(key, value);
#else
            for (int i = 0; i < YG2.saves.stringKeys.Count; i++)
            {
                if (YG2.saves.stringKeys[i] == key)
                {
                    YG2.saves.stringValues[i] = value;
                    return;
                }
            }
            YG2.saves.stringKeys.Add(key);
            YG2.saves.stringValues.Add(value);
#endif
        }

        string GetString(string key)
        {
#if !Storage_yg
            return LocalStorage.GetKey(key);
#else
            for (int i = 0; i < YG2.saves.stringKeys.Count; i++)
            {
                if (YG2.saves.stringKeys[i] == key)
                    return YG2.saves.stringValues[i];
            }
            return string.Empty;
#endif
        }

        string GetString(string key, string defaultValue)
        {
#if !Storage_yg
            return LocalStorage.GetKey(key, defaultValue);
#else
            for (int i = 0; i < YG2.saves.stringKeys.Count; i++)
            {
                if (YG2.saves.stringKeys[i] == key)
                    return YG2.saves.stringValues[i];
            }
            return defaultValue;
#endif
        }

        // Float
        void SetFloat(string key, float value)
        {
#if !Storage_yg
            LocalStorage.SetKey(key, value.ToString());
#else
            for (int i = 0; i < YG2.saves.floatKeys.Count; i++)
            {
                if (YG2.saves.floatKeys[i] == key)
                {
                    YG2.saves.floatValues[i] = value;
                    return;
                }
            }
            YG2.saves.floatKeys.Add(key);
            YG2.saves.floatValues.Add(value);
#endif
        }

        float GetFloat(string key)
        {
#if !Storage_yg
            if (float.TryParse(LocalStorage.GetKey(key), out float res))
                return res;
            else
                return 0f;
#else
            for (int i = 0; i < YG2.saves.floatKeys.Count; i++)
            {
                if (YG2.saves.floatKeys[i] == key)
                    return YG2.saves.floatValues[i];
            }
            return 0;
#endif
        }

        float GetFloat(string key, float defaultValue)
        {
#if !Storage_yg
            if (!LocalStorage.HasKey(key))
                return defaultValue;
            else
                return float.Parse(LocalStorage.GetKey(key));
#else
            for (int i = 0; i < YG2.saves.floatKeys.Count; i++)
            {
                if (YG2.saves.floatKeys[i] == key)
                    return YG2.saves.floatValues[i];
            }
            return defaultValue;
#endif
        }

        // Integer
        void SetInt(string key, int value)
        {
#if !Storage_yg
            LocalStorage.SetKey(key, value.ToString());
#else
            for (int i = 0; i < YG2.saves.intKeys.Count; i++)
            {
                if (YG2.saves.intKeys[i] == key)
                {
                    YG2.saves.intValues[i] = value;
                    return;
                }
            }
            YG2.saves.intKeys.Add(key);
            YG2.saves.intValues.Add(value);
#endif
        }

        int GetInt(string key)
        {
#if !Storage_yg
            if (int.TryParse(LocalStorage.GetKey(key), out int res))
                return res;
            else
                return 0;
#else
            for (int i = 0; i < YG2.saves.intKeys.Count; i++)
            {
                if (YG2.saves.intKeys[i] == key)
                    return YG2.saves.intValues[i];
            }
            return 0;
#endif
        }

        int GetInt(string key, int defaultValue)
        {
#if !Storage_yg
            if (!LocalStorage.HasKey(key))
                return defaultValue;
            else
                return int.Parse(LocalStorage.GetKey(key));
#else
            for (int i = 0; i < YG2.saves.intKeys.Count; i++)
            {
                if (YG2.saves.intKeys[i] == key)
                    return YG2.saves.intValues[i];
            }
            return defaultValue;
#endif
        }

        // Other
        bool HasKey(string key)
        {
#if !Storage_yg
            return LocalStorage.HasKey(key);
#else
            for (int i = 0; i < YG2.saves.stringKeys.Count; i++)
            {
                if (YG2.saves.stringKeys[i] == key)
                    return true;
            }

            for (int i = 0; i < YG2.saves.floatKeys.Count; i++)
            {
                if (YG2.saves.floatKeys[i] == key)
                    return true;
            }

            for (int i = 0; i < YG2.saves.intKeys.Count; i++)
            {
                if (YG2.saves.intKeys[i] == key)
                    return true;
            }

            return false;
#endif
        }

        void DeleteKey(string key)
        {
#if !Storage_yg
            LocalStorage.DeleteKey(key);
#else
            for (int i = 0; i < YG2.saves.stringKeys.Count; i++)
            {
                if (YG2.saves.stringKeys[i] == key)
                {
                    YG2.saves.stringKeys.RemoveAt(i);
                    YG2.saves.stringValues.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i < YG2.saves.floatKeys.Count; i++)
            {
                if (YG2.saves.floatKeys[i] == key)
                {
                    YG2.saves.floatKeys.RemoveAt(i);
                    YG2.saves.floatValues.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i < YG2.saves.intKeys.Count; i++)
            {
                if (YG2.saves.intKeys[i] == key)
                {
                    YG2.saves.intKeys.RemoveAt(i);
                    YG2.saves.intValues.RemoveAt(i);
                    return;
                }
            }
#endif
        }

        void DeleteAll()
        {
#if !Storage_yg
            LocalStorage.DeleteAll();
#else
            YG2.saves.stringKeys.Clear();
            YG2.saves.stringValues.Clear();

            YG2.saves.floatKeys.Clear();
            YG2.saves.floatValues.Clear();

            YG2.saves.intKeys.Clear();
            YG2.saves.intValues.Clear();
#endif
        }

        void Save()
        {
#if Storage_yg
            YG2.SaveProgress();
#endif
        }
    }
}
