using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using YG.Insides;

namespace YG.EditorScr
{
    [CustomEditor(typeof(PlatformEventsYG2)), CanEditMultipleObjects]
    public class PlatformEventsYG2Editor : Editor
    {
        private SerializedProperty platforms;
        private SerializedProperty whenToEvent;
        private SerializedProperty unityEvents;
        private GUIContent[] options;
        private GUIContent m_AddButonContent;

        private void OnEnable()
        {
            platforms = serializedObject.FindProperty("platforms");
            whenToEvent = serializedObject.FindProperty("whenToEvent");
            unityEvents = serializedObject.FindProperty("platformAction");

#if RU_YG2
            m_AddButonContent = EditorGUIUtility.TrTextContent("Добавить платформу");
#else
            m_AddButonContent = EditorGUIUtility.TrTextContent("Add platform");
#endif

            string[] allPlatforms = GetAllPlatforms();
            options = new GUIContent[allPlatforms.Length];

            for (int i = 0; i < options.Length; i++)
            {
                options[i] = new GUIContent(allPlatforms[i]);
            }
        }

        private string[] GetAllPlatforms()
        {
            List<string> result = new List<string>();

            string[] pathesPlatform = Directory.GetDirectories(InfoYG.PATCH_PC_PLATFORMS);
            for (int i = 0; i < pathesPlatform.Length; i++)
            {
                string[] filesSearchScr = Directory.GetFiles(pathesPlatform[i]);
                for (int f = 0; f < filesSearchScr.Length; f++)
                {
                    if (filesSearchScr[f].Contains("Version.txt") || filesSearchScr[f].Contains(".meta"))
                        continue;

                    string folder = Path.GetFileName(pathesPlatform[i]);
                    string file = Path.GetFileName(filesSearchScr[f]);
                    string path = $"{InfoYG.PATCH_ASSETS_PLATFORMS}/{folder}/{file}";

                    PlatformSettings platform = AssetDatabase.LoadAssetAtPath<PlatformSettings>(path);
                    if (platform != null)
                    {
                        result.Add(platform.NameBase());
                    }
                }
            }

            return result.ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            Rect btPosition = GUILayoutUtility.GetRect(m_AddButonContent, GUI.skin.button);
            const float addButonWidth = 200f;
            btPosition.x = btPosition.x + (btPosition.width - addButonWidth) / 2;
            btPosition.width = addButonWidth;
            if (GUI.Button(btPosition, m_AddButonContent, YGEditorStyles.button))
            {
                ShowAddTriggerMenu();
            }

#if RU_YG2
            string nameProp = "Платформы для которых cработают методы";
#else
            string nameProp = "Platforms for which methods will be triggered";
#endif
            EditorGUILayout.PropertyField(platforms, new GUIContent(nameProp), true);

            EditorGUILayout.Space(10);
#if RU_YG2
            nameProp = "Когда выполнить";
#else
            nameProp = "When to Execute";
#endif
            EditorGUILayout.PropertyField(whenToEvent, new GUIContent(nameProp), true);

            int listenerCount = ((PlatformEventsYG2)target).platformAction.GetPersistentEventCount();
            if (listenerCount == 1 && ((PlatformEventsYG2)target).platformAction.GetPersistentTarget(0) != null)
            {
                string methodName = ((PlatformEventsYG2)target).platformAction.GetPersistentMethodName(0);
                Object targetObject = ((PlatformEventsYG2)target).platformAction.GetPersistentTarget(0);

                if (targetObject.name == ((PlatformEventsYG2)target).gameObject.name && methodName == "DeactivateGameObject")
                {
#if RU_YG2
                    string tooltip = "По умолчанию привязан метод деактивации объекта";
#else
                    string tooltip = "By default, the deactivation method of object is linked";
#endif
                    EditorGUILayout.LabelField(tooltip, TextStyles.Header());
                }
            }

            EditorGUILayout.PropertyField(unityEvents, new GUIContent("Events"), true);

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            if (EditorUtils.IsMouseOverWindow(serializedObject.targetObject.name))
                Repaint();
        }

        private void ShowAddTriggerMenu()
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < options.Length; ++i)
            {
                bool active = false;
                foreach (var t in targets)
                {
                    PlatformEventsYG2 scr = (PlatformEventsYG2)t;
                    if (scr.platforms.Contains(options[i].text))
                    {
                        active = true;
                        break;
                    }
                }
                menu.AddItem(options[i], active, OnAddNewSelected, i);
            }
            menu.ShowAsContext();
            Event.current.Use();
        }

        private void OnAddNewSelected(object index)
        {
            int selected = (int)index;

            foreach (var t in targets)
            {
                PlatformEventsYG2 scr = (PlatformEventsYG2)t;

                if (scr.platforms.Contains(options[selected].text))
                {
                    scr.platforms.Remove(options[selected].text);
                }
                else
                {
                    scr.platforms.Add(options[selected].text);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
