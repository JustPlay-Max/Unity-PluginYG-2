using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YG.EditorScr
{
    public static class ExampleScenes
    {
        public static string[] sceneNames = new string[0];

        private const string FILE_NAME_SCENES_LIST = "DemoSceneNames";
        private static string FILE_PATH_SCENES_LIST
        {
            get { return $"{InfoYG.PATCH_PC_YG2}/Example/Resources/{FILE_NAME_SCENES_LIST}.txt"; }
        }

        public static void LoadSceneList()
        {
            TextAsset sceneListFile = Resources.Load<TextAsset>(FILE_NAME_SCENES_LIST);
            if (sceneListFile != null)
            {
                string fileContent = sceneListFile.text;
                sceneNames = fileContent.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                sceneNames = new string[0];
            }
        }

        public static void AddScenesToBuildSettings()
        {
            RemoveScenesFromBuildSettings();

            if (!Directory.Exists($"{InfoYG.PATCH_PC_YG2}/Example"))
            {
#if RU_YG2
                EditorUtility.DisplayDialog($"Сообщение", "Демо материалы были удалены!", "Ok");
#else
                EditorUtility.DisplayDialog($"Message", "The demo materials have been deleted!", "Ok");
#endif
                return;
            }

            List<string> scenePathsToAdd = GetScenesModules();
            sceneNames = scenePathsToAdd.ToArray();

            EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;

            foreach (EditorBuildSettingsScene existScene in existingScenes)
            {
                for (int i = scenePathsToAdd.Count - 1; i >= 0; i--)
                {
                    if (scenePathsToAdd[i] == existScene.path)
                        scenePathsToAdd.RemoveAt(i);
                }
            }

            EditorBuildSettingsScene[] newScenes = new EditorBuildSettingsScene[scenePathsToAdd.Count + existingScenes.Length];

            for (int i = 0; i < scenePathsToAdd.Count; i++)
                newScenes[i] = new EditorBuildSettingsScene(scenePathsToAdd[i], true);

            for (int i = 0; i < existingScenes.Length; i++)
                newScenes[scenePathsToAdd.Count + i] = existingScenes[i];

            EditorBuildSettings.scenes = newScenes;

            using (StreamWriter writer = new StreamWriter(FILE_PATH_SCENES_LIST, false))
            {
                for (int i = 0; i < sceneNames.Length; i++)
                {
                    int lastSlashIndex = sceneNames[i].LastIndexOf('/');
                    sceneNames[i] = sceneNames[i].Substring(lastSlashIndex + 1).Replace(".unity", string.Empty);
                    writer.WriteLine(sceneNames[i]);
                }
            }
            AssetDatabase.Refresh();
        }

        public static void RemoveScenesFromBuildSettings()
        {
            List<string> scenePathsToRemove = GetScenesModules();

            EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;
            List<EditorBuildSettingsScene> remainingScenes = new List<EditorBuildSettingsScene>();

            foreach (var scene in existingScenes)
            {
                bool contains = false;
                for (int i = 0; i < scenePathsToRemove.Count; i++)
                {
                    if (scene.path == scenePathsToRemove[i])
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                    remainingScenes.Add(scene);
            }

            EditorBuildSettings.scenes = remainingScenes.ToArray();

            sceneNames = new string[0];
            using (FileStream fs = new FileStream(FILE_PATH_SCENES_LIST, FileMode.Truncate)) { }
            AssetDatabase.Refresh();
        }

        public static List<string> GetScenesModules()
        {
            List<string> scenePathsToAdd = new List<string>();

            string patchExample = $"{InfoYG.PATCH_PC_YG2}/Example";

            if (!Directory.Exists(patchExample))
            {
                return scenePathsToAdd;
            }
            else
            {
                string directory = $"{patchExample}/Resources";
                string file = $"{directory}/DemoSceneNames.txt";

                if (!File.Exists(file))
                {
                    Directory.CreateDirectory(directory);
                    FileYG.WriteAllText(file, string.Empty);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            List<string> directories = new List<string>
            {
                $"{InfoYG.PATCH_PC_YG2}"
            };
            directories.AddRange(Directory.GetDirectories(InfoYG.PATCH_PC_MODULES).ToList());

            foreach (string directory in directories)
            {
                string dir = directory + "/Example/Scenes";
                if (!Directory.Exists(dir))
                    continue;

                string[] files = Directory.GetFiles(dir);
                IEnumerable<string> sceneFiles = files.Where(file => Path.GetExtension(file).Equals(".unity", System.StringComparison.OrdinalIgnoreCase));
                List<string> sceneList = sceneFiles.ToList();

                for (int i = 0; i < sceneList.Count; i++)
                {
                    sceneList[i] = sceneList[i].Remove(0, Application.dataPath.Length - 6);
                    sceneList[i] = sceneList[i].Replace("\\", "/");
                }

                scenePathsToAdd.AddRange(sceneList);
            }

            return scenePathsToAdd;
        }
    }
}
