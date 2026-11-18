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

        internal static bool cloudLoadResolved;
        internal static bool cloudLoadFailed;

        [InitYG]
        private static void InitStorage()
        {
            cloudLoadResolved = !infoYG.Storage.saveCloud;
            cloudLoadFailed = false;
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

        /// <summary>Reports the progress of the recording. The platform’s cloud stores data while waiting for a response from the server..</summary>
        public static void SaveProgress(Action<bool> completed)
        {
            if (completed == null)
            {
                SaveProgress();
                return;
            }

            bool finished = false;
            void Complete(bool success)
            {
                if (finished) return;
                finished = true;
                try { completed(success); }
                catch (Exception e) { Debug.LogException(e); }
            }

            if (!_SDKEnabled)
            {
                Debug.LogError("[PluginYG Storage] SaveProgress requires initialized PluginYG2.");
                Complete(false);
                return;
            }

            try
            {
#if !UNITY_EDITOR
                if (infoYG.Storage.saveCloud && (!cloudLoadResolved || cloudLoadFailed))
                {
                    Debug.LogWarning("[PluginYG Storage] Save blocked until cloud saves load successfully. Retry with YG2.LoadCloud().");
                    Complete(false);
                    return;
                }
#endif
                saves.idSave++;
#if UNITY_EDITOR
                YGInsides.SaveEditor();
                Complete(true);
#else
                if (infoYG.Storage.saveLocal && !YGInsides.SaveLocalWithResult())
                {
                    Complete(false);
                    return;
                }
                if (infoYG.Storage.saveCloud)
                    iPlatform.SaveCloud(Complete);
                else
                    Complete(infoYG.Storage.saveLocal);
#endif
            }
            catch (Exception e)
            {
                Debug.LogError("[PluginYG Storage] Save failed: " + e.Message);
                Complete(false);
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

#if !UNITY_EDITOR
            if (infoYG.Storage.saveCloud && (!YG2.cloudLoadResolved || YG2.cloudLoadFailed))
            {
                Debug.LogWarning("[PluginYG Storage] Save blocked until cloud saves load successfully. Retry with YG2.LoadCloud().");
                return;
            }
#endif

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

        public static void LoadCloud() => YGInsides.LoadCloud(true);
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
        public static bool SaveLocalWithResult()
        {
#if NJSON_STORAGE_YG2
            return LocalStorage.TrySetKey(STORAGE_KEY, JsonConvert.SerializeObject(YG2.saves));
#else
            return LocalStorage.TrySetKey(STORAGE_KEY, JsonUtility.ToJson(YG2.saves));
#endif
        }

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
#if !UNITY_EDITOR
            if (infoYG.Storage.saveCloud && (!YG2.cloudLoadResolved || YG2.cloudLoadFailed))
            {
                Debug.LogWarning("[PluginYG Storage] Cloud save blocked until cloud saves load successfully. Retry with YG2.LoadCloud().");
                return;
            }
#endif
            Message("Save Cloud");
            iPlatform.SaveCloud();
        }

        public static void LoadCloud(bool forceReload = false)
        {
            Message("Load Cloud");
            if (forceReload)
            {
                YG2.cloudLoadResolved = false;
                YG2.cloudLoadFailed = false;
            }
#if !UNITY_EDITOR
            iPlatform.LoadCloud();
#else
            LoadEditor();
#endif
        }

        public static void SetLoadSaves(string data)
        {
            SetLoadSaves(data, true);
        }

        public static void SetLoadSavesFailed(string error)
        {
            YG2.cloudLoadResolved = true;
            YG2.cloudLoadFailed = true;
            Message("Cloud load failed: " + error);
        }

        public static void SetLoadSaves(string data, bool success)
        {
            YG2.cloudLoadResolved = true;
            YG2.cloudLoadFailed = !success;
            if (!success)
            {
                Message("Cloud load failed. Existing saves were left untouched.");
                return;
            }

            DataState cloudDataState = DataState.Exist;
            DataState localDataState = DataState.Exist;
            SavesYG cloudData = new SavesYG();
            SavesYG localData = new SavesYG();

            if (data != InfoYG.NO_DATA && !string.IsNullOrEmpty(data))
            {
                try
                {
#if YandexGamesPlatform_yg
                    data = data.Remove(0, 2);
                    data = data.Remove(data.Length - 2, 2);
                    data = data.Replace(@"\\\", '\u0002'.ToString());
                    data = data.Replace(@"\", "");
                    data = data.Replace('\u0002'.ToString(), @"\");
#endif
#if NJSON_STORAGE_YG2
                    cloudData = JsonConvert.DeserializeObject<SavesYG>(data);
#else
                    cloudData = JsonUtility.FromJson<SavesYG>(data);
#endif
                    if (cloudData == null)
                        cloudDataState = DataState.Broken;
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
                    {
                        SetLoadSavesFailed("Cloud save data is invalid");
                        Message("Load Cloud Broken! Local saves are disabled; existing game data was left untouched.");
                        return;
                    }
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
            else if (cloudDataState == DataState.Broken)
            {
                if (localDataState != DataState.Exist)
                {
                    SetLoadSavesFailed("Cloud save data is invalid and no local save can recover it");
                    return;
                }
                Message("Cloud Saves - Broken! Restored from local saves.");
                YG2.saves = localData;
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
