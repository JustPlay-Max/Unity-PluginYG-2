using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace YG.Example.DemoScene
{
    [DefaultExecutionOrder(-5001)]
    public class DemoScenePartial : MonoBehaviour
    {
        public bool rootPartialsGrid;
        private static GameObject eventSystemObj;

        private void Awake()
        {
            if (SceneManager.sceneCount > 1)
            {
                GameObject[] objects = gameObject.scene.GetRootGameObjects();

                foreach (GameObject obj in objects)
                {
                    DemoScenePartial[] foundPartials = obj.GetComponentsInChildren<DemoScenePartial>(true);

                    foreach (DemoScenePartial partial in foundPartials)
                        partial.transform.SetParent(null);
                }

                objects = gameObject.scene.GetRootGameObjects();

                for (int i = objects.Length - 1; i >= 0; i--)
                {
                    if (!objects[i].GetComponent<DemoScenePartial>())
                        DestroyImmediate(objects[i]);
                }
            }
        }

        private void Start()
        {
            EventSystem[] eventSystem = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

            if (eventSystem.Length > 0)
            {
                if (eventSystemObj == null)
                {
                    if (eventSystem.Length > 1)
                    {
                        for (int i = 1; i < eventSystem.Length; i++)
                            Destroy(eventSystem[i].gameObject);
                    }

                    eventSystemObj = eventSystem[0].gameObject;
                    DontDestroyOnLoad(eventSystemObj);
                }
                else
                {
                    if (eventSystem.Length > 1)
                    {
                        for (int i = 0; i < eventSystem.Length; i++)
                        {
                            GameObject esObj = eventSystem[i].gameObject;
                            if (esObj != eventSystemObj)
                                Destroy(esObj);
                        }
                    }
                }
            }
            else
            {
                eventSystemObj = new GameObject { name = "EventSystemBase" };
                eventSystemObj.AddComponent<EventSystem>();
                eventSystemObj.AddComponent<StandaloneInputModule>();
                DontDestroyOnLoad(eventSystemObj);
            }
        }
    }
}