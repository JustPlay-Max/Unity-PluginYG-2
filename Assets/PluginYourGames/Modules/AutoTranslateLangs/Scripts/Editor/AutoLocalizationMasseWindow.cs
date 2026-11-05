using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

#if TMP_YG2
using TMPro;
#endif

namespace YG.LanguageLegacy
{
    public class AutoLocalizationMasse : EditorWindow
    {
        [MenuItem("Tools/YG2/Auto Translate Langs/Auto Localization Masse")]
        public static void ShowWindow()
            => GetWindow<AutoLocalizationMasse>("Auto Localization Masse");

        Vector2 scrollPosition;
        private readonly List<GameObject> objectsTranlate = new();

        private void OnGUI()
        {
            Color defaultColor = GUI.backgroundColor;

            GUILayout.Space(10);

            if (GUILayout.Button("Search for all objects on the scene by type TEXT (Legacy)", GUILayout.Height(30)))
                AddObjectsToTranslate<Text>();
#if TMP_YG2
            if (GUILayout.Button("Search for all objects on the scene by type TEXT_MESH_PRO", GUILayout.Height(30)))
                AddObjectsToTranslate<TMP_Text>();
#endif
            if (GUILayout.Button("Search for all objects on the scene by type LANGUAGE_YG", GUILayout.Height(30)))
                AddObjectsToTranslate<LanguageYG>();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add selected from list", GUILayout.Height(22)))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Text>()
#if TMP_YG2
                        || obj.GetComponent<TMP_Text>()
#endif
                        )
                    {
                        bool check = false;

                        for (int i = 0; i < objectsTranlate.Count; i++)
                            if (obj == objectsTranlate[i])
                                check = true;

                        if (!check)
                            objectsTranlate.Add(obj);
                    }
                }
            }

            if (GUILayout.Button("Remove selected from list", GUILayout.Height(22)))
                foreach (GameObject obj in Selection.gameObjects)
                    objectsTranlate.Remove(obj);

            if (GUILayout.Button("Clear list", GUILayout.Height(22)))
                objectsTranlate.Clear();

            GUILayout.EndHorizontal();

            if (objectsTranlate.Count > 0)
            {
                GUILayout.Space(10);

                if (GUILayout.Button("TRANSLATE", GUILayout.Height(30)))
                {
                    foreach (GameObject obj in objectsTranlate)
                    {
                        LanguageYG scrAL = obj.GetComponent<LanguageYG>();

                        if (scrAL == null)
                            scrAL = obj.AddComponent<LanguageYG>();

                        scrAL.Serialize();
                        scrAL.componentTextField = true;
                        scrAL.Translate(19);
                    }
                }

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Reserialize LanguageYG components", GUILayout.Height(22)))
                {
                    foreach (GameObject obj in objectsTranlate)
                    {
                        LanguageYG scrAL = obj.GetComponent<LanguageYG>();

                        if (scrAL)
                            scrAL.Serialize();
                    }
                }

                GUI.backgroundColor = Color.red;

                if (GUILayout.Button("Remove component LanguageYG", GUILayout.Height(22)))
                {
                    foreach (GameObject obj in objectsTranlate)
                    {
                        LanguageYG scrAL = obj.GetComponent<LanguageYG>();

                        if (scrAL)
                            DestroyImmediate(scrAL);
                    }
                }

                GUI.backgroundColor = defaultColor;

                GUILayout.EndHorizontal();
            }

            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            GUILayout.Label($"({objectsTranlate.Count} objects in the list)", style, GUILayout.ExpandWidth(true));

            if (objectsTranlate.Count > 10 && position.height < objectsTranlate.Count * 20.6f + 190)
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Height(position.height - 190));

            for (int i = 0; i < objectsTranlate.Count; i++)
                objectsTranlate[i] = (GameObject)EditorGUILayout.ObjectField($"{i + 1}. [{objectsTranlate[i].name}]", objectsTranlate[i], typeof(GameObject), false);

            if (objectsTranlate.Count > 10 && position.height < objectsTranlate.Count * 20.6f + 190)
                GUILayout.EndScrollView();

            if (GUI.changed && objectsTranlate.Count > 0)
            {
                EditorUtility.SetDirty(objectsTranlate[0].gameObject);
                EditorSceneManager.MarkSceneDirty(objectsTranlate[0].gameObject.scene);
            }
        }

        private void AddObjectsToTranslate<T>() where T : Component
        {
            objectsTranlate.Clear();

            foreach (T component in SceneAsset.FindObjectsByType<T>(FindObjectsInactive.Include))
                objectsTranlate.Add(component.gameObject);
        }
    }
}

