using System;
using System.IO;
using UnityEngine;
using YG.Utils;
using YG.Insides;
#if NJSON_STORAGE_YG2
using Newtonsoft.Json;
#endif

namespace YG
{
    public static partial class YG2
    {
        public static SavesYG saves = new SavesYG();
        public static Action onDefaultSaves;

        private static bool isFirstSession;

        [InitYG]
        private static void InitStorage()
        {
#if UNITY_EDITOR
            // Reset static for ECS
            saves = new SavesYG();
            onDefaultSaves = null;

#endif
            YGInsides.LoadProgress();

#if YandexGamesPlatform_yg && !Authorization_yg
            if (infoYG.Storage.saveCloud)
            {
#if UNITY_EDITOR
                Debug.LogError("При использовании облачных сохранений необходимо импортировать зависимость - модуль Authorization. Для локальных сохранений авторизация не требуется.");
#else
                Debug.LogError("When using cloud saves, you need to import a dependency - Authorization module. Authorization is not required for local saves.");
#endif
            }
#endif
        }

        [StartYG]
        private static void OnDefaultSaves()
        {
            if (isFirstSession)
            {
                isFirstSession = false;
                onDefaultSaves?.Invoke();
            }
        }

        public static void SetDefaultSaves()
        {
            Message("Set Default Saves");
            int idSave = saves.idSave;
            saves = new SavesYG { idSave = idSave };

            if (Time.unscaledTime < 0.5f)
            {
                isFirstSession = true;
            }
            else
            {
                onDefaultSaves?.Invoke();
                GetDataInvoke();
            }
        }

        public static void SaveProgress()
        {
            if (!_SDKEnabled)
            {
#if RU_YG2
                Debug.LogError("Не используйте методы сохранения прогресса до полной инициализации PluginYG2.");
#else
                Debug.LogError("Do not use methods to save progress until PluginYG2 is fully initialized.");
#endif
                return;
            }

            saves.idSave++;
#if !UNITY_EDITOR
            if (infoYG.Storage.saveLocal)
                YGInsides.SaveLocal();

            if (infoYG.Storage.saveCloud)
                YGInsides.SaveCloud();
#else
            YGInsides.SaveEditor();
#endif
        }
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        private enum DataState { Exist, NotExist, Broken };
        private const string STORAGE_KEY = "YG2_SavesData";

        public static void LoadProgress()
        {
#if !UNITY_EDITOR
            if (infoYG.Storage.saveCloud)
                LoadCloud();
            else 
                LoadLocal();
#else
            LoadEditor();
#endif
            if (YG2.saves.idSave > 0)
                GetDataInvoke();
        }

#if UNITY_EDITOR
        private static string PATH_SAVES_EDITOR
        {
            get { return Path.Combine(InfoYG.PATCH_PC_EDITOR, "SavesEditorYG2.json"); }
        }

        public static void SaveEditor()
        {
            Message("Save Progress (in Editor)");

            bool fileExits = false;

            if (File.Exists(PATH_SAVES_EDITOR))
                fileExits = true;

#if NJSON_STORAGE_YG2
            string json = JsonConvert.SerializeObject(YG2.saves, Formatting.Indented);
#else
            string json = JsonUtility.ToJson(YG2.saves, true);
#endif
            File.WriteAllText(PATH_SAVES_EDITOR, json);

            if (!fileExits && File.Exists(PATH_SAVES_EDITOR))
                UnityEditor.AssetDatabase.Refresh();
        }

        public static void LoadEditor()
        {
            if (File.Exists(PATH_SAVES_EDITOR))
            {
                string json = File.ReadAllText(PATH_SAVES_EDITOR);
#if NJSON_STORAGE_YG2
                YG2.saves = JsonConvert.DeserializeObject<SavesYG>(json);
#else
                YG2.saves = JsonUtility.FromJson<SavesYG>(json);
#endif
            }
            else
            {
                YG2.SetDefaultSaves();
            }
        }
#endif
        public static void SaveLocal()
        {
            Message("Save Local");
#if !UNITY_EDITOR
#if NJSON_STORAGE_YG2
            LocalStorage.SetKey(STORAGE_KEY, JsonConvert.SerializeObject(YG2.saves));
#else
            LocalStorage.SetKey(STORAGE_KEY, JsonUtility.ToJson(YG2.saves));
#endif
#endif
        }

        public static void LoadLocal()
        {
            Message("Load Local");

            if (!LocalStorage.HasKey(STORAGE_KEY))
            {
                YG2.SetDefaultSaves();
            }
            else
            {
#if NJSON_STORAGE_YG2
                YG2.saves = JsonConvert.DeserializeObject<SavesYG>(LocalStorage.GetKey(STORAGE_KEY));
#else
                YG2.saves = JsonUtility.FromJson<SavesYG>(LocalStorage.GetKey(STORAGE_KEY));
#endif
            }
        }

        public static void SaveCloud(bool ignoreTimer = false)
        {
            Message("Save Cloud");
            iPlatform.SaveCloud();
        }

        public static void LoadCloud()
        {
            Message("Load Cloud");
#if !UNITY_EDITOR
            iPlatform.LoadCloud();
#else
            LoadEditor();
#endif
        }

        public static void SetLoadSaves(string data)
        {
            DataState cloudDataState = DataState.Exist;
            DataState localDataState = DataState.Exist;
            SavesYG cloudData = new SavesYG();
            SavesYG localData = new SavesYG();

            if (data != InfoYG.NO_DATA && !string.IsNullOrEmpty(data))
            {
#if YandexGamesPlatform_yg
                data = data.Remove(0, 2);
                data = data.Remove(data.Length - 2, 2);
                data = data.Replace(@"\\\", '\u0002'.ToString());
                data = data.Replace(@"\", "");
                data = data.Replace('\u0002'.ToString(), @"\");
#endif
                try
                {
#if NJSON_STORAGE_YG2
                    cloudData = JsonConvert.DeserializeObject<SavesYG>(data);
#else
                    cloudData = JsonUtility.FromJson<SavesYG>(data);
#endif
                }
                catch (Exception e)
                {
                    Debug.LogError("Cloud Load Error: " + e.Message);
                    cloudDataState = DataState.Broken;
                }
            }
            else cloudDataState = DataState.NotExist;

            if (infoYG.Storage.saveLocal == false)
            {
                if (cloudDataState == DataState.NotExist)
                {
                    Message("No cloud saves. Local saves are disabled.");
                    YG2.SetDefaultSaves();
                }
                else
                {
                    if (cloudDataState == DataState.Broken)
                        Message("Load Cloud Broken! But we tried to restore and load cloud saves. Local saves are disabled.");
                    else Message("Load Cloud Complete! Local saves are disabled.");

                    YG2.saves = cloudData;
                }
                GetDataInvoke();
                return;
            }

            if (LocalStorage.HasKey(STORAGE_KEY))
            {
                try
                {
#if NJSON_STORAGE_YG2
                    localData = JsonConvert.DeserializeObject<SavesYG>(LocalStorage.GetKey(STORAGE_KEY));
#else
                    localData = JsonUtility.FromJson<SavesYG>(LocalStorage.GetKey(STORAGE_KEY));
#endif
                }
                catch (Exception e)
                {
                    Debug.LogError("Local Load Error: " + e.Message);
                    localDataState = DataState.Broken;
                }
            }
            else localDataState = DataState.NotExist;

            if (cloudDataState == DataState.Exist && localDataState == DataState.Exist)
            {
                if (cloudData.idSave >= localData.idSave)
                {
                    Message($"Load Cloud Complete! ID Cloud Save: {cloudData.idSave}, ID Local Save: {localData.idSave}");
                    YG2.saves = cloudData;
                }
                else
                {
                    Message($"Load Local Complete! ID Cloud Save: {cloudData.idSave}, ID Local Save: {localData.idSave}");
                    YG2.saves = localData;
                }
            }
            else if (cloudDataState == DataState.Exist)
            {
                YG2.saves = cloudData;
                Message("Load Cloud Complete! Local Data - " + localDataState);
            }
            else if (localDataState == DataState.Exist)
            {
                YG2.saves = localData;
                Message("Load Local Complete! Cloud Data - " + cloudDataState);
            }
            else if (cloudDataState == DataState.Broken ||
                (cloudDataState == DataState.Broken && localDataState == DataState.Broken))
            {
                Message("Local Saves - " + localDataState);
                Message("Cloud Saves - Broken! Data Recovering...");
                YG2.SetDefaultSaves();
#if NJSON_STORAGE_YG2
                YG2.saves = JsonConvert.DeserializeObject<SavesYG>(data);
#else
                YG2.saves = JsonUtility.FromJson<SavesYG>(data);
#endif
                Message("Cloud Saves Partially Restored!");
            }
            else if (localDataState == DataState.Broken)
            {
                Message("Cloud Saves - " + cloudDataState);
                Message("Local Saves - Broken! Data Recovering...");
                YG2.SetDefaultSaves();
#if NJSON_STORAGE_YG2
                YG2.saves = JsonConvert.DeserializeObject<SavesYG>(LocalStorage.GetKey(STORAGE_KEY));
#else
                YG2.saves = JsonUtility.FromJson<SavesYG>(LocalStorage.GetKey(STORAGE_KEY));
#endif
                Message("Local Saves Partially Restored!");
            }
            else
            {
                Message("No Saves");
                YG2.SetDefaultSaves();
            }

            GetDataInvoke();
        }
    }
}