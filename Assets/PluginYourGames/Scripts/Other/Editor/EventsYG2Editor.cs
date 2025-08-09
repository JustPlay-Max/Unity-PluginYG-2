using UnityEngine;
using UnityEditor;

namespace YG.EditorScr
{
    [CustomEditor(typeof(EventsYG2), true), CanEditMultipleObjects]
    public class EventsYG2Editor : Editor
    {
        private SerializedProperty m_DelegatesProperty;

        private GUIContent m_IconToolbarMinus;
        private GUIContent m_EventIDName;
        private GUIContent[] m_EventTypes;
        private GUIContent m_AddButonContent;

        protected virtual void OnEnable()
        {
#if RU_YG2
            m_AddButonContent = EditorGUIUtility.TrTextContent("Добавить новый тип события");
#else
            m_AddButonContent = EditorGUIUtility.TrTextContent("Add New Event Type");
#endif
            m_DelegatesProperty = serializedObject.FindProperty("m_Delegates");
            m_EventIDName = new GUIContent("");
            m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            m_IconToolbarMinus.tooltip = "Remove all events in this list.";

            m_EventTypes = new GUIContent[]
            {
                new GUIContent("GetSDKData"),
                new GUIContent("PauseStopGame"),
                new GUIContent("PauseResumeGame"),
#if InterstitialAdv_yg
                new GUIContent("OpenInterAdv"),
                new GUIContent("CloseInterAdv"),
                new GUIContent("ErrorInterAdv"),
#endif
#if RewardedAdv_yg
                new GUIContent("OpenRewardedAdv"),
                new GUIContent("CloseRewardedAdv"),
                new GUIContent("RewardAdv"),
                new GUIContent("ErrorRewardedAdv"),
#endif
#if Authorization_yg
                new GUIContent("AuthTrue"),
                new GUIContent("AuthFalse"),
#endif
#if Payments_yg
                new GUIContent("PurchaseSuccess"),
                new GUIContent("PurchaseFailed"),
#endif
#if Review_yg
                new GUIContent("ReviewSuccess"),
                new GUIContent("ReviewFailed"),
#endif
#if GameLabel_yg
                new GUIContent("GameLabelSuccess"),
                new GUIContent("GameLabelFail")
#endif
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space();

            int toBeRemovedEntry = -1;
            Vector2 removeButtonSize = GUIStyle.none.CalcSize(m_IconToolbarMinus);

            for (int i = 0; i < m_DelegatesProperty.arraySize; ++i)
            {
                SerializedProperty delegateProperty = m_DelegatesProperty.GetArrayElementAtIndex(i);
                SerializedProperty eventProperty = delegateProperty.FindPropertyRelative("eventID");
                SerializedProperty callbacksProperty = delegateProperty.FindPropertyRelative("callback");
                m_EventIDName.text = eventProperty.enumDisplayNames[eventProperty.enumValueIndex];

                EditorGUILayout.PropertyField(callbacksProperty, m_EventIDName);
                Rect callbackRect = GUILayoutUtility.GetLastRect();

                Rect removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
                if (GUI.Button(removeButtonPos, m_IconToolbarMinus, GUIStyle.none))
                {
                    toBeRemovedEntry = i;
                }

                EditorGUILayout.Space();
            }

            if (toBeRemovedEntry > -1)
            {
                RemoveEntry(toBeRemovedEntry);
            }

            Rect btPosition = GUILayoutUtility.GetRect(m_AddButonContent, GUI.skin.button);
            const float addButonWidth = 200f;
            btPosition.x = btPosition.x + (btPosition.width - addButonWidth) / 2;
            btPosition.width = addButonWidth;
            if (GUI.Button(btPosition, m_AddButonContent, YGEditorStyles.button))
            {
                ShowAddTriggerMenu();
            }

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            if (EditorUtils.IsMouseOverWindow(serializedObject.targetObject.name))
                Repaint();
        }

        private void RemoveEntry(int toBeRemovedEntry)
        {
            foreach (var t in targets)
            {
                SerializedObject serializedTarget = new SerializedObject(t);
                SerializedProperty delegatesProperty = serializedTarget.FindProperty("m_Delegates");
                delegatesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
                serializedTarget.ApplyModifiedProperties();
            }
        }

        private void ShowAddTriggerMenu()
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < m_EventTypes.Length; ++i)
            {
                bool active = true;

                foreach (var t in targets)
                {
                    SerializedObject serializedTarget = new SerializedObject(t);
                    SerializedProperty delegatesProperty = serializedTarget.FindProperty("m_Delegates");

                    for (int p = 0; p < delegatesProperty.arraySize; ++p)
                    {
                        SerializedProperty delegateEntry = delegatesProperty.GetArrayElementAtIndex(p);
                        SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
                        if (eventProperty.enumValueIndex == i)
                        {
                            active = false;
                        }
                    }
                }

                if (active)
                    menu.AddItem(m_EventTypes[i], false, OnAddNewSelected, i);
                else
                    menu.AddDisabledItem(m_EventTypes[i]);
            }
            menu.ShowAsContext();
            Event.current.Use();
        }

        private void OnAddNewSelected(object index)
        {
            int selected = (int)index;

            foreach (var t in targets)
            {
                SerializedObject serializedTarget = new SerializedObject(t);
                SerializedProperty delegatesProperty = serializedTarget.FindProperty("m_Delegates");

                delegatesProperty.arraySize += 1;
                SerializedProperty delegateEntry = delegatesProperty.GetArrayElementAtIndex(delegatesProperty.arraySize - 1);
                SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
                eventProperty.enumValueIndex = selected;
                serializedTarget.ApplyModifiedProperties();
            }
        }
    }
}
