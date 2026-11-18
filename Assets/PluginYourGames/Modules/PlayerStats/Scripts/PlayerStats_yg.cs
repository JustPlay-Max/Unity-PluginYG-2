using System;
using System.Collections.Generic;
using UnityEngine;
using YG.Utils;
using YG.Translations;

namespace YG
{
    public static partial class YG2
    {
        private static Dictionary<string, int> playerStats;
        private const string KEY_STATES = "PlayerStatsYG";
        private static readonly Queue<Action> statsIncrementQueue = new Queue<Action>();
        private static bool statsIncrementInProgress;

        /// <summary>Raised when the local stats cache changes or is loaded. Does not confirm a save.</summary>
        public static event Action onStatsChanged;

        [InitYG]
        private static void InitStats()
        {
            statsIncrementQueue.Clear();
            statsIncrementInProgress = false;
#if UNITY_EDITOR
            ReceiveStatsJson(PlayerPrefs.GetString(KEY_STATES));
#else
            iPlatform.InitStats();
#endif
        }

        public static void LoadStats()
        {
#if UNITY_EDITOR
            ReceiveStatsJson(PlayerPrefs.GetString(KEY_STATES));
#else
            iPlatform.LoadStats();
#endif
        }

        public static int GetState(string key)
        {
            if (!_SDKEnabled)
            {
                Debug.LogError(PlayerStatsTranslations.LoadStateErrorMsg);
                return 0;
            }

            if (playerStats.ContainsKey(key))
                return playerStats[key];
            else
                return 0;
        }

        public static Dictionary<string, int> GetAllStats() => playerStats;

        public static void SetState(string key, int value) => SetState(key, value, null);

        /// <summary>Saves a numeric value and reports the platform save result. The local cache is updated immediately.</summary>
        public static void SetState(string key, int value, Action<bool> completed)
        {
            if (!_SDKEnabled || playerStats == null)
            {
                Debug.LogError(PlayerStatsTranslations.SaveStateErrorMsg);
                InvokeStatsSaveCallback(completed, false);
                return;
            }

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("[PluginYG PlayerStats] A stat key is required.");
                InvokeStatsSaveCallback(completed, false);
                return;
            }

            playerStats[key] = value;
            SaveStats(completed);
            NotifyStatsChanged();
        }

        public static void IncrementState(string key, int value) => IncrementState(key, value, null);

        /// <summary>Adds a value to a stat. Updates the cache after a successful save, then invokes the callback.</summary>
        /// <remarks>Increment requests run sequentially. Await the callback before setting or reloading the same stats.</remarks>
        public static void IncrementState(string key, int value, Action<bool> completed)
        {
            if (!_SDKEnabled || playerStats == null || string.IsNullOrEmpty(key))
            {
                Debug.LogError("[PluginYG PlayerStats] Increment requires initialized stats and a non-empty key.");
                InvokeStatsSaveCallback(completed, false);
                return;
            }

            statsIncrementQueue.Enqueue(() => IncrementStats(key, value, completed));
            RunNextStatsIncrement();
        }

        private static void RunNextStatsIncrement()
        {
            if (statsIncrementInProgress || statsIncrementQueue.Count == 0) return;
            statsIncrementInProgress = true;
            statsIncrementQueue.Dequeue().Invoke();
        }

        private static void IncrementStats(string key, int value, Action<bool> completed)
        {
            bool finished = false;
            void Complete(bool success, int result)
            {
                if (finished) return;
                finished = true;
                if (success)
                {
                    playerStats[key] = result;
                    NotifyStatsChanged();
                }
                InvokeStatsSaveCallback(completed, success);
                statsIncrementInProgress = false;
                RunNextStatsIncrement();
            }

            try
            {
#if UNITY_EDITOR
                var snapshot = new Dictionary<string, int>(playerStats);
                snapshot.TryGetValue(key, out int current);
                int result = checked(current + value);
                snapshot[key] = result;
                Complete(LocalStorage.TrySetKey(KEY_STATES, JsonYG.SerializeDictionary(snapshot)), result);
#else
                iPlatform.IncrementStat(key, value, Complete);
#endif
            }
            catch (Exception e)
            {
                Debug.LogError("[PluginYG PlayerStats] Increment failed: " + e.Message);
                Complete(false, 0);
            }
        }

        public static void SetAllStats(Dictionary<string, int> stats) => SetAllStats(stats, null);

        /// <summary>Saves numeric values and reports the platform save result. The local cache is updated immediately.</summary>
        public static void SetAllStats(Dictionary<string, int> stats, Action<bool> completed)
        {
            if (!_SDKEnabled || stats == null)
            {
                Debug.LogError(PlayerStatsTranslations.SaveStateErrorMsg);
                InvokeStatsSaveCallback(completed, false);
                return;
            }

            playerStats = stats;
            SaveStats(completed);
            NotifyStatsChanged();
        }

        private static void SaveStats(Action<bool> completed)
        {
            bool finished = false;
            void Complete(bool success)
            {
                if (finished) return;
                finished = true;
                InvokeStatsSaveCallback(completed, success);
            }

            try
            {
#if UNITY_EDITOR
                Complete(LocalStorage.TrySetKey(KEY_STATES, JsonYG.SerializeDictionary(playerStats)));
#else
                var snapshot = new Dictionary<string, int>(playerStats);
                if (completed == null)
                    iPlatform.SetStats(snapshot);
                else
                    iPlatform.SetStats(snapshot, Complete);
#endif
            }
            catch (Exception e)
            {
                Debug.LogError("[PluginYG PlayerStats] Save failed: " + e.Message);
                Complete(false);
            }
        }

        private static void InvokeStatsSaveCallback(Action<bool> completed, bool success)
        {
            try { completed?.Invoke(success); }
            catch (Exception e) { Debug.LogException(e); }
        }

        private static void NotifyStatsChanged()
        {
            if (onStatsChanged == null) return;
            foreach (Action callback in onStatsChanged.GetInvocationList())
            {
                try { callback(); }
                catch (Exception e) { Debug.LogException(e); }
            }
        }

        public static void ReceiveStatsJson(string jsonStats)
        {
            if (jsonStats == InfoYG.NO_DATA)
            {
                playerStats = new Dictionary<string, int>();
                NotifyStatsChanged();
                return;
            }

            playerStats = JsonYG.DeserializeDictionary(jsonStats);
            NotifyStatsChanged();
            GetDataInvoke();
        }
    }
}

