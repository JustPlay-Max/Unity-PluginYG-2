#if YandexGamesPlatform_yg
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using YG.Utils;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        private static readonly Dictionary<int, Action<bool>> statsSaveCallbacks = new Dictionary<int, Action<bool>>();
        private static readonly Dictionary<int, Action<bool, int>> statsIncrementCallbacks = new Dictionary<int, Action<bool, int>>();
        private static int statsSaveRequestID;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatsSaveCallbacks()
        {
            statsSaveCallbacks.Clear();
            statsIncrementCallbacks.Clear();
        }

        [DllImport("__Internal")]
        private static extern string InitStats_js();

        public void InitStats()
        {
            YG2.ReceiveStatsJson(InitStats_js());
        }

        [DllImport("__Internal")]
        private static extern void GetStats_js();
        public void LoadStats()
        {
            GetStats_js();
        }

        [DllImport("__Internal")]
        private static extern void SetStats_js(string jsonStats);
        public void SetStats(Dictionary<string, int> stats)
        {
            SetStats_js(JsonYG.SerializeDictionary(stats));
        }

        [DllImport("__Internal")]
        private static extern void SetStatsWithCallback_js(string jsonStats, int requestID);

        public void SetStats(Dictionary<string, int> stats, Action<bool> completed)
        {
            string json = JsonYG.SerializeDictionary(stats);
            int requestID = ++statsSaveRequestID;
            statsSaveCallbacks.Add(requestID, completed);
            try
            {
                SetStatsWithCallback_js(json, requestID);
            }
            catch
            {
                statsSaveCallbacks.Remove(requestID);
                throw;
            }
        }

        internal static void CompleteStatsSave(string result)
        {
            if (string.IsNullOrEmpty(result)) return;
            int separator = result.IndexOf('|');
            if (separator < 0 || !int.TryParse(result.Substring(0, separator), out int requestID))
                return;
            if (!statsSaveCallbacks.TryGetValue(requestID, out var completed))
                return;

            statsSaveCallbacks.Remove(requestID);
            completed?.Invoke(result.Substring(separator + 1) == "1");
        }

        [DllImport("__Internal")]
        private static extern void IncrementStat_js(string key, int value, int requestID);

        public void IncrementStat(string key, int value, Action<bool, int> completed)
        {
            int requestID = ++statsSaveRequestID;
            statsIncrementCallbacks.Add(requestID, completed);
            try
            {
                IncrementStat_js(key, value, requestID);
            }
            catch
            {
                statsIncrementCallbacks.Remove(requestID);
                throw;
            }
        }

        internal static void CompleteStatsIncrement(string result)
        {
            if (string.IsNullOrEmpty(result)) return;
            string[] parts = result.Split('|');
            if (parts.Length < 2 || !int.TryParse(parts[0], out int requestID)) return;
            if (!statsIncrementCallbacks.TryGetValue(requestID, out var completed)) return;
            statsIncrementCallbacks.Remove(requestID);
            int value = 0;
            bool success = parts.Length == 3 && parts[1] == "1" && int.TryParse(parts[2], out value);
            completed?.Invoke(success, value);
        }
    }
}
namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void ReceiveStats(string json) => YG2.ReceiveStatsJson(json);
        public void SetStatsResult(string result) => PlatformYG2.CompleteStatsSave(result);
        public void IncrementStatsResult(string result) => PlatformYG2.CompleteStatsIncrement(result);
    }
}
#endif
