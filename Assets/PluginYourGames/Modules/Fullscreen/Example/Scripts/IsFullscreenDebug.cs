using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class IsFullscreenDebug : MonoBehaviour
    {
        public Text debugText;

        private void Start()
        {
            UpdateDebug();
        }

        public void UpdateDebug()
        {
            debugText.text = "is Fullscreen = " + YG2.isFullscreen;
        }

        public void UpdateDebugInvoke()
        {
            Invoke("UpdateDebug", 0.1f);
        }
    }
}