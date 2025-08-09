using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class DebugViewing : MonoBehaviour
    {
        public Transform rotationObj;
        public Text timeScaleText, audioPauseText;
        public GameObject pauseObj;

        private Color startColor = new Color(1.0f, 0.592f, 0.259f, 1.0f);

        private void OnEnable()
        {
            YG2.onPauseGame += OnPause;
        }

        private void OnDisable()
        {
            YG2.onPauseGame -= OnPause;
        }

        private void OnPause(bool pause)
        {
            pauseObj.SetActive(pause);
        }

        private void Update()
        {
            rotationObj.Rotate(Vector3.up * 30 * Time.deltaTime);

            timeScaleText.text = "Time Scale: " + Time.timeScale;
            audioPauseText.text = "Audio Pause: " + AudioListener.pause;

            if (Time.timeScale != 1)
            {
                timeScaleText.color = Color.red;
                timeScaleText.fontStyle = FontStyle.Bold;
            }
            else
            {
                timeScaleText.color = startColor;
                timeScaleText.fontStyle = FontStyle.Normal;
            }

            if (AudioListener.pause)
            {
                audioPauseText.color = Color.red;
                audioPauseText.fontStyle = FontStyle.Bold;
            }
            else
            {
                audioPauseText.color = startColor;
                audioPauseText.fontStyle = FontStyle.Normal;
            }
        }
    }
}