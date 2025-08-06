using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class PlayerStatsExample : MonoBehaviour
    {
        public Text statsText;
        public Text setInputName;
        public Text setInputValue;
        public Text getInputName;
        public Text getInputValue;

        private void Start()
        {
            if (YG2.isSDKEnabled)
                UpdateDrawStats();
        }

        private void UpdateDrawStats()
        {
            Dictionary<string, int> stats = YG2.GetAllStats();
            string formattedText = string.Empty;

            if (stats.Count == 0)
            {
                statsText.text = "No Saved Stats";
                return;
            }

            foreach (KeyValuePair<string, int> stat in stats)
                formattedText += $"{stat.Key}: {stat.Value}\n";

            statsText.text = formattedText;
        }

        public void SetState()
        {
            YG2.SetState(setInputName.text, int.Parse(setInputValue.text));
            UpdateDrawStats();
        }

        public void GetState()
        {
            getInputValue.text = YG2.GetState(getInputName.text).ToString();
        }

        public void DeleteAllStats()
        {
            Dictionary<string, int> newStats = new Dictionary<string, int>();
            YG2.SetAllStats(newStats);
            UpdateDrawStats();
        }

        public void LoadStats()
        {
            YG2.onGetSDKData += OnGetSDKData;
            YG2.LoadStats();

            void OnGetSDKData()
            {
                YG2.onGetSDKData -= OnGetSDKData;

                Dictionary<string, int> allStats = YG2.GetAllStats();
                string jsonStr = YG.Utils.JsonYG.SerializeDictionary(allStats);
                Debug.Log(jsonStr);
            }
        }
    }
}