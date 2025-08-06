using UnityEngine;

namespace YG
{
    public class TVTesting : MonoBehaviour
    {
        public static TVTesting Instance;

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (YG2.envir.isMobile)
                {
                    Destroy(gameObject);
                    return;
                }

                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public static void Create()
        {
            GameObject tvTestObj = new GameObject { name = "TV Testing" };
            tvTestObj.AddComponent<TVTesting>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                YG2.onTVKeyDown?.Invoke("Up");
            }
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                YG2.onTVKeyUp?.Invoke("Up");
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                YG2.onTVKeyDown?.Invoke("Left");
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                YG2.onTVKeyUp?.Invoke("Left");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                YG2.onTVKeyDown?.Invoke("Down");
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                YG2.onTVKeyUp?.Invoke("Down");
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                YG2.onTVKeyDown?.Invoke("Right");
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                YG2.onTVKeyUp?.Invoke("Right");
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                YG2.onTVKeyDown?.Invoke("Enter");
            }
            if (Input.GetKeyUp(KeyCode.Return))
            {
                YG2.onTVKeyUp?.Invoke("Enter");
            }

            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                YG2.onTVKeyBack?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                YG2.onTVKeyDown?.Invoke("MediaRewind");
            }
            if (Input.GetKeyUp(KeyCode.F6))
            {
                YG2.onTVKeyUp?.Invoke("MediaRewind");
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                YG2.onTVKeyDown?.Invoke("MediaPlayPause");
            }
            if (Input.GetKeyUp(KeyCode.F7))
            {
                YG2.onTVKeyUp?.Invoke("MediaPlayPause");
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                YG2.onTVKeyDown?.Invoke("MediaFastForward");
            }
            if (Input.GetKeyUp(KeyCode.F8))
            {
                YG2.onTVKeyUp?.Invoke("MediaFastForward");
            }
        }
    }
}