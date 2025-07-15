using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace YG
{
    public class PauseGameYG : MonoBehaviour
    {
        public static PauseGameYG inst;

        private float timeScale_save;
        private bool audioPause_save;
        private bool cursorVisible_save;
        private CursorLockMode cursorLockState_save;
        private bool eventSystem_save;

        private bool editTimeScale;
        private bool editAudioPause;
        private bool editCursor;
        private bool editEventSystem;
        private EventSystem eventSystem;

        public void Setup(bool timeScale, bool audioPause, bool cursor, bool eventSystem)
        {
            if (inst == null)
            {
                inst = this;
                DontDestroyOnLoad(inst);

                editTimeScale = timeScale;
                editAudioPause = audioPause;
                editCursor = cursor;
                editEventSystem = eventSystem;

                if (editTimeScale)
                {
                    timeScale_save = Time.timeScale;
                    Time.timeScale = 0;
                }

                if (editAudioPause)
                {
                    audioPause_save = AudioListener.pause;
                    AudioListener.pause = true;
                }

                if (editCursor)
                {
                    cursorVisible_save = Cursor.visible;
                    cursorLockState_save = Cursor.lockState;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

                EventSystemDisable();
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EventSystemDisable();
        }

        private void EventSystemDisable()
        {
            if (editEventSystem && eventSystem == null)
            {
                eventSystem = GameObject.FindAnyObjectByType<EventSystem>();
                if (eventSystem != null)
                {
                    eventSystem_save = eventSystem.enabled;
                    eventSystem.enabled = false;
                }
            }
        }

        private void LateUpdate()
        {
            if (editTimeScale && Time.timeScale != 0)
            {
                timeScale_save = Time.timeScale;
                Time.timeScale = 0;
            }

            if (editAudioPause && AudioListener.pause != true)
            {
                audioPause_save = AudioListener.pause;
                AudioListener.pause = true;
            }

            if (editCursor)
            {
                if (Cursor.visible != true)
                {
                    cursorVisible_save = Cursor.visible;
                    Cursor.visible = true;
                }

                if (Cursor.lockState != CursorLockMode.None)
                {
                    cursorLockState_save = Cursor.lockState;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }

        public void PauseDisabled()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (editTimeScale)
                Time.timeScale = timeScale_save;

            if (editAudioPause)
                AudioListener.pause = audioPause_save;

            if (editCursor)
            {
                Cursor.visible = cursorVisible_save;
                Cursor.lockState = cursorLockState_save;
            }

            if (editEventSystem && eventSystem != null)
                eventSystem.enabled = eventSystem_save;

            inst = null;
            DestroyImmediate(gameObject);
        }
    }
}
