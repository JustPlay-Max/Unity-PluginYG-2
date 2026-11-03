#if YandexGamesPlatform_yg
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using YG.Insides;
#if NJSON_STORAGE_YG2
using Newtonsoft.Json;
#endif

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        private static readonly Dictionary<int, Action<bool>> cloudSaveCallbacks = new Dictionary<int, Action<bool>>();

#if UNITY_WEBGL && !UNITY_EDITOR
        private static int cloudSaveRequestID;
        [DllImport("__Internal")]
        private static extern void SaveCloudWithCallback_js(string jsonData, int requestID);
#endif

        public void SaveCloud(Action<bool> completed)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
#if NJSON_STORAGE_YG2
            string json = JsonConvert.SerializeObject(YG2.saves);
#else
            string json = JsonUtility.ToJson(YG2.saves);
#endif
            int requestID = ++cloudSaveRequestID;
            cloudSaveCallbacks.Add(requestID, completed);
            try
            {
                SaveCloudWithCallback_js(json, requestID);
            }
            catch
            {
                cloudSaveCallbacks.Remove(requestID);
                throw;
            }
#else
            completed(YG2.infoYG.Storage.saveLocal || YGInsides.SaveLocalWithResult());
#endif
        }

        internal static void CompleteCloudSave(string result)
        {
            int separator = result.IndexOf('|');
            if (separator < 0 || !int.TryParse(result.Substring(0, separator), out int requestID))
                return;
            if (!cloudSaveCallbacks.TryGetValue(requestID, out var completed))
                return;
            cloudSaveCallbacks.Remove(requestID);
            completed(result.Substring(separator + 1) == "1");
        }

        [DllImport("__Internal")]
        private static extern string InitCloud_js();

        [DllImport("__Internal")]
        private static extern string LoadCloud_js();

        public void LoadCloud()
        {
            if (!YG2.isSDKEnabled)
            {
                YGInsides.SetLoadSaves(InitCloud_js());
            }
            else
            {
                LoadCloud_js();
            }
        }

        [DllImport("__Internal")]
        private static extern void SaveCloud_js(string jsonData, bool flush);
        public void SaveCloud()
        {
#if NJSON_STORAGE_YG2
            SaveCloud_js(JsonConvert.SerializeObject(YG2.saves), YG2.infoYG.Storage.flush);
#else
            SaveCloud_js(JsonUtility.ToJson(YG2.saves), YG2.infoYG.Storage.flush);
#endif
        }
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void SetSaveCloudResult(string result) => PlatformYG2.CompleteCloudSave(result);
        public void SetLoadSaves(string data) => YGInsides.SetLoadSaves(data);
    }
}
#endif
