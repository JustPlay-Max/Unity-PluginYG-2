using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YG.Example.DemoScene
{
    public class AllDemoScenes : MonoBehaviour
    {
        public GameObject spawnButtonPrefab;
        public Transform spawnButtonsTransform;

        private string[] sceneNames = new string[0];
        private string baseSceneName = null;

        private const string FILE_SCENES_LIST = "DemoSceneNames";
        public static AllDemoScenes inst;

        private void Awake()
        {
            if (inst == null)
            {
                inst = this;
                DontDestroyOnLoad(gameObject);

                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            TextAsset sceneListFile = Resources.Load<TextAsset>(FILE_SCENES_LIST);
            if (sceneListFile != null)
            {
                string fileContent = sceneListFile.text;
                sceneNames = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                sceneNames = new string[0];
            }

            UpdateList();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (baseSceneName == null)
            {
                baseSceneName = SceneManager.GetActiveScene().name;
                return;
            }

            if (scene.name == baseSceneName)
                UpdateList();
        }

        public void UpdateList()
        {
            DestroyList();
            SpawnList();
        }

        private void DestroyList()
        {
            int childCount = spawnButtonsTransform.childCount;

            for (int i = childCount - 1; i >= 0; i--)
                Destroy(spawnButtonsTransform.GetChild(i).gameObject);

            Transform rootPartialsGrid = RootPartialsGrid();
            if (rootPartialsGrid != null)
            {
                childCount = rootPartialsGrid.childCount;

                for (int i = childCount - 1; i >= 0; i--)
                {
                    if (rootPartialsGrid.GetChild(i).gameObject.name != "BasicAPI")
                        Destroy(rootPartialsGrid.GetChild(i).gameObject);
                }
            }
        }

        private Transform RootPartialsGrid()
        {
            DemoScenePartial[] rootPartialsCandidates = FindObjectsByType<DemoScenePartial>(FindObjectsSortMode.None);

            for (int i = 0; i < rootPartialsCandidates.Length; i++)
            {
                if (rootPartialsCandidates[i].rootPartialsGrid)
                    return rootPartialsCandidates[i].transform;
            }
            return null;
        }

        private async void SpawnList()
        {
            Transform rootPartialsGrid = RootPartialsGrid();
            if (rootPartialsGrid == null)
                return;

            for (int i = 0; i < sceneNames.Length; i++)
            {
                GameObject obj = Instantiate(spawnButtonPrefab, spawnButtonsTransform);
                obj.GetComponent<DemoSceneLoadButton>().Setup(sceneNames[i]);

                if (i == 0)
                    continue;

                if (SceneManager.GetActiveScene().name != sceneNames[i])
                {
                    int index = i;
                    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);

                    asyncLoad.completed += (AsyncOperation operation) =>
                    {
                        Scene loadedScene = SceneManager.GetSceneByName(sceneNames[index]);

                        if (loadedScene.isLoaded)
                        {
                            GameObject[] rootObjects = loadedScene.GetRootGameObjects();
                            List<DemoScenePartial> partials = new List<DemoScenePartial>();

                            foreach (GameObject rootObj in rootObjects)
                            {
                                DemoScenePartial[] foundPartials = rootObj.GetComponentsInChildren<DemoScenePartial>(true);
                                partials.AddRange(foundPartials);
                            }

                            foreach (DemoScenePartial partial in partials)
                                partial.transform.SetParent(rootPartialsGrid);

                            foreach (GameObject obj in loadedScene.GetRootGameObjects())
                                DestroyImmediate(obj);

                            SceneManager.UnloadSceneAsync(sceneNames[index]);
                        }
                    };
                }
            }

            await Task.Delay(1000);
#if UNITY_2023_2_OR_NEWER
            await Resources.UnloadUnusedAssets();
#else
            Resources.UnloadUnusedAssets();
#endif
            GC.Collect();
        }
    }
}
