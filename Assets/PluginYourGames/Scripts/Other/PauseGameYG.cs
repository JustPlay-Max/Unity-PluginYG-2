﻿﻿using UnityEngine;
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

        private static bool deleteProcessing;

        public void Setup(bool timeScale, bool audioPause, bool cursor, bool eventSystem)
        {
            if (!inst)
            {
                deleteProcessing = false;

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
                    ApplyCursorState(true, CursorLockMode.None);
                }

                EventSystemDisable();
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void SetState(
            float timeScale,
            bool audioPause,
            bool cursorVisible,
            CursorLockMode cursorLockState)
        {
            if (inst && inst.editTimeScale)
                inst.timeScale_save = timeScale;
            else
                Time.timeScale = timeScale;

            if (inst && inst.editAudioPause)
                inst.audioPause_save = audioPause;
            else
                AudioListener.pause = audioPause;

            if (inst && inst.editCursor)
            {
                inst.cursorVisible_save = cursorVisible;
                inst.cursorLockState_save = cursorLockState;
            }
            else
            {
                ApplyCursorState(cursorVisible, cursorLockState);
            }
        }

        public static void SetState(
            float timeScale,
            bool audioPause,
            bool cursorEnable)
        {
            SetState(
                timeScale,
                audioPause,
                cursorEnable,
                cursorEnable ? CursorLockMode.None : CursorLockMode.Locked);
        }

        private static void ApplyCursorState(bool visible, CursorLockMode lockState)
        {
            if (lockState == CursorLockMode.None)
            {
                Cursor.visible = visible;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = visible;
                Cursor.lockState = lockState;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EventSystemDisable();
        }

        private void EventSystemDisable()
        {
            if (editEventSystem && !eventSystem)
            {
                eventSystem = GameObject.FindAnyObjectByType<EventSystem>();
                if (eventSystem)
                {
                    eventSystem_save = eventSystem.enabled;
                    eventSystem.enabled = false;
                }
            }
        }

        private void Update()
        {
            if (!YG2.isPauseGame)
            {
                if (!deleteProcessing)
                    PauseDisabled();

                return;
            }

            if (editTimeScale && Time.timeScale != 0)
            {
                timeScale_save = Time.timeScale;
                Time.timeScale = 0;
            }

            if (editAudioPause && !AudioListener.pause)
            {
                audioPause_save = AudioListener.pause;
                AudioListener.pause = true;
            }

            if (editCursor)
            {
                if (!Cursor.visible)
                {
                    cursorVisible_save = Cursor.visible;
                    Cursor.visible = true;
                }
                else if (Cursor.lockState != CursorLockMode.None)
                {
                    cursorLockState_save = Cursor.lockState;
                    Cursor.lockState = CursorLockMode.None;
                }
            }

            if (editEventSystem && eventSystem && eventSystem.enabled)
            {
                eventSystem_save = eventSystem.enabled;
                eventSystem.enabled = false;
            }
        }

        public void PauseDisabled()
        {
            inst = null;
            deleteProcessing = true;

            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (editTimeScale)
                Time.timeScale = timeScale_save;

            if (editAudioPause)
                AudioListener.pause = audioPause_save;

            if (editCursor)
            {
                ApplyCursorState(cursorVisible_save, cursorLockState_save);
            }

            if (editEventSystem && eventSystem)
                eventSystem.enabled = eventSystem_save;

            Destroy(gameObject);
        }
    }
}
