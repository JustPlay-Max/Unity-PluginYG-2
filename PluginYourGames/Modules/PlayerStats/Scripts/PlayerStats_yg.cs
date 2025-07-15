using System.Collections.Generic;
using UnityEngine;
using YG.Utils;

namespace YG
{
    public static partial class YG2
    {
        private static Dictionary<string, int> playerStats;
        private const string KEY_STATES = "PlayerStatsYG";

        [InitYG]
        private static void InitStats()
        {
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
                Debug.LogError("State cannot be load. The SDK has not been initialized yet!");
                return 0;
            }

            if (playerStats.ContainsKey(key))
                return playerStats[key];
            else
                return 0;
        }

        public static Dictionary<string, int> GetAllStats() => playerStats;


        public static void SetState(string key, int value)
        {
            if (!_SDKEnabled)
            {
                Debug.LogError("State cannot be saved. The SDK has not been initialized yet!");
                return;
            }

            if (playerStats.ContainsKey(key))
                playerStats[key] = value;
            else
                playerStats.Add(key, value);

#if UNITY_EDITOR
            PlayerPrefs.SetString(KEY_STATES, JsonYG.SerializeDictionary(playerStats));
#else
            iPlatform.SetStats(playerStats);
#endif
        }

        public static void SetAllStats(Dictionary<string, int> stats)
        {
            if (!_SDKEnabled)
            {
                Debug.LogError("State cannot be saved. The SDK has not been initialized yet!");
                return;
            }

            playerStats = stats;
#if UNITY_EDITOR
            PlayerPrefs.SetString(KEY_STATES, JsonYG.SerializeDictionary(stats));
#else
            iPlatform.SetStats(stats);
#endif
        }

        public static void ReceiveStatsJson(string jsonStats)
        {
            if (jsonStats == InfoYG.NO_DATA)
            {
                playerStats = new Dictionary<string, int>();
                return;
            }

            playerStats = JsonYG.DeserializeDictionary(jsonStats);
            GetDataInvoke();
        }
    }
}

