using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YG
{
    public class RewardedAdvLockTimer : MonoBehaviour
    {
        public string rewardID;
        public int timerDurationLock = 60;
        public GameObject timerObject;
        public Text timerText;
        public bool timerComplete
        {
            get
            {
                if (!timersList.ContainsKey(rewardID))
                    return true;
                else if (Time.realtimeSinceStartup >= timersList[rewardID] + timerDurationLock)
                    return true;
                else
                    return false;
            }
        }

        private static Dictionary<string, float> timersList = new Dictionary<string, float>();
        private Coroutine coroutine;

        private void OnEnable() => YG2.onRewardAdv += SetTimer;
        private void OnDisable() => YG2.onRewardAdv -= SetTimer;

        private void Start()
        {
            timerObject.SetActive(false);

            if (timersList.ContainsKey(rewardID))
            {
                if (timerComplete)
                    timersList.Remove(rewardID);
                else
                    coroutine = StartCoroutine(ShowTimer());
            }
        }

        private void SetTimer(string id)
        {
            if (id != rewardID)
                return;

            if (timersList.ContainsKey(id))
            {
                timersList[id] = Time.realtimeSinceStartup;
            }
            else
            {
                timersList.Add(id, Time.realtimeSinceStartup);
                coroutine = StartCoroutine(ShowTimer());
            }
        }

        IEnumerator ShowTimer()
        {
            timerObject.SetActive(true);

            while (!timerComplete)
            {
                float timeInSeconds = timersList[rewardID] + timerDurationLock - Time.realtimeSinceStartup;

                int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
                int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

                if (minutes <= 0)
                {
                    timerText.text = string.Format(seconds.ToString());
                }
                else
                {
                    string str = string.Format("{0:00}:{1:00}", minutes, seconds);
                    if (str[0].ToString() == "0")
                        str = str.Substring(1);
                    timerText.text = str;
                }

                if (timerText.text == "0")
                    break;

                yield return new WaitForSecondsRealtime(1);
            }

            timersList.Remove(rewardID);
            timerObject.SetActive(false);
        }
    }
}