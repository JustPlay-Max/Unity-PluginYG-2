using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

#if TMP_YG2
using TMPro;
#endif

namespace YG.LanguageLegacy
{
    public class AutoLocalizationMasse : EditorWindow
    {
        private const float ButtonHeight = 26f;

        private bool _textLegacySearch;
        private bool _tmpSearch;
        private bool _languageYgSearch;

        [MenuItem("Tools/YG2/Auto Translate Langs/Auto Localization Masse")]
        public static void ShowWindow()
            => GetWindow<AutoLocalizationMasse>("Auto Localization Masse");

        Vector2 _scrollPosition;

        private readonly List<GameObject> objectsTranlate = new();

        private void OnGUI()
        {
            Color defaultColor = GUI.backgroundColor;

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Space(5);
                    GUILayout.Label("Select components for search:", EditorStyles.boldLabel);

                    _textLegacySearch = GUILayout.Toggle(_textLegacySearch, "Text (Legacy) search all on the scene", GUILayout.Height(ButtonHeight));
#if TMP_YG2
                    _tmpSearch = GUILayout.Toggle(_tmpSearch, "TMP_Text search all on the scene", GUILayout.Height(ButtonHeight));
#endif
                    _languageYgSearch = GUILayout.Toggle(_languageYgSearch, "LanguageYG search all on the scene", GUILayout.Height(ButtonHeight));
                }
            }

            if (!_textLegacySearch && !_tmpSearch && !_languageYgSearch)
                return;

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Search", GUILayout.Height(ButtonHeight)))
                    {
                        objectsTranlate.Clear();

                        if (_textLegacySearch)
                            AddObjectsToTranslate<Text>();

                        if (_tmpSearch)
                            AddObjectsToTranslate<TMP_Text>();

                        if (_languageYgSearch)
                            AddObjectsToTranslate<LanguageYG>();
                    }

                    if (GUILayout.Button("Clear list", GUILayout.Height(ButtonHeight)))
                        objectsTranlate.Clear();
                }

                GUILayout.Space(5);

                if (objectsTranlate.Count > 0)
                {
                    GUI.backgroundColor = Color.green;

                    if (GUILayout.Button("TRANSLATE", GUILayout.Height(ButtonHeight)))
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

                    GUI.backgroundColor = defaultColor;
                }

                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField(
                        $"[{objectsTranlate.Count} objects in the list]",
                        new GUIStyle(EditorStyles.boldLabel)
                        {
                            alignment = TextAnchor.MiddleCenter,
                        });

                    using (var scroll = new GUILayout.ScrollViewScope(_scrollPosition))
                    {
                        _scrollPosition = scroll.scrollPosition;

                        for (int i = 0; i < objectsTranlate.Count; i++)
                        {
                            GUILayout.BeginHorizontal();

                            objectsTranlate[i] = (GameObject)EditorGUILayout.ObjectField(
                                $"{i + 1}) [ {objectsTranlate[i].name} ]",
                                objectsTranlate[i],
                                typeof(GameObject),
                                true);

                            GUI.backgroundColor = Color.red;

                            if (GUILayout.Button("X", GUILayout.Width(ButtonHeight)))
                            {
                                objectsTranlate.RemoveAt(i);
                                i--;
                            }

                            GUI.backgroundColor = defaultColor;
                            GUILayout.EndHorizontal();
                        }
                    }
                }
            }
        }

        private void AddObjectsToTranslate<T>() where T : Component
        {
#if UNITY_6000_6_OR_NEWER
            foreach (T component in SceneAsset.FindObjectsByType<T>(FindObjectsInactive.Include))
#else
            foreach (T component in SceneAsset.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None))
#endif
            {
                objectsTranlate.Add(component.gameObject);
            }
        }
    }
}

