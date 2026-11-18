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

        private void OnEnable()
        {
            YG2.onGetSDKData += UpdateDrawStats;
            YG2.onStatsChanged += UpdateDrawStats;
            if (YG2.isSDKEnabled)
                UpdateDrawStats();
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= UpdateDrawStats;
            YG2.onStatsChanged -= UpdateDrawStats;
        }

        private void UpdateDrawStats()
        {
            if (!YG2.isSDKEnabled) return;
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
            YG2.SetState(setInputName.GetComponentInParent<InputField>().text,
                int.Parse(setInputValue.GetComponentInParent<InputField>().text));
        }

        public void IncrementState()
        {
            YG2.IncrementState(setInputName.GetComponentInParent<InputField>().text,
                int.Parse(setInputValue.GetComponentInParent<InputField>().text));
        }

        public void GetState()
        {
            // У дочернего Text длинная строка может содержать только видимую часть ввода.
            string key = getInputName.GetComponentInParent<InputField>().text;
            getInputValue.text = YG2.GetState(key).ToString();
        }

        public void DeleteAllStats()
        {
            Dictionary<string, int> newStats = new Dictionary<string, int>();
            YG2.SetAllStats(newStats);
        }

        public void LoadStats() => YG2.LoadStats();
    }
}
