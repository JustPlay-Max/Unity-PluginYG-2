using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using YG.Insides;
using System.Linq;

namespace YG.EditorScr
{
    [CustomEditor(typeof(PlatformSettings))]
    public class PlatformSettingsEditor : Editor
    {
        private PlatformSettings scr;
        private Texture2D iconPlatform;

        private SerializedObject serializedScr;
        private SerializedObject serializedInfoYG;

        private void OnEnable()
        {
            scr = (PlatformSettings)target;

            serializedScr = new SerializedObject(scr);
            serializedInfoYG = new SerializedObject(YG2.infoYG);

            string path = AssetDatabase.GetAssetPath(scr);
            if (string.IsNullOrEmpty(path))
                return;

            string folderPath = Path.GetDirectoryName(path);
            string modulName = Path.GetFileName(folderPath);

            scr.nameFull = modulName + "Platform";

            EditorUtility.SetDirty(scr);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            string iconPath = GetIconCurrentPlatformPath(modulName);

            if (File.Exists(iconPath))
            {
                byte[] fileData = File.ReadAllBytes(iconPath);
                iconPlatform = new Texture2D(2, 2);
                iconPlatform.LoadImage(fileData);
            }
        }

        public static string GetIconCurrentPlatformPath(string modulName)
        {
            string iconPath = Path.Combine(InfoYG.PATCH_PC_WEBGLTEMPLATES, modulName, "thumbnail.png");
            if (File.Exists(iconPath))
                return iconPath;

            iconPath = Path.Combine(InfoYG.PATCH_PC_PLATFORMS, modulName, "Editor", "thumbnail.png");
            if (File.Exists(iconPath))
                return iconPath;

            return null;
        }

        public override void OnInspectorGUI()
        {
            serializedScr.Update();
            serializedInfoYG.Update();

            Undo.RecordObject(scr, "Platform Settings Change");
            Undo.RecordObject(YG2.infoYG, "InfoYG Change");
            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal();

            if (iconPlatform)
            {
                Rect textureRect = GUILayoutUtility.GetRect(45, 45, GUILayout.ExpandWidth(false));
                GUI.DrawTexture(textureRect, iconPlatform);
            }

            GUILayout.Space(7);
            GUILayout.BeginVertical();

            GUIStyle styleNamePlatform = TextStyles.White();
            styleNamePlatform.fontSize = 18;
            styleNamePlatform.fontStyle = FontStyle.Bold;
            if (!iconPlatform)
                styleNamePlatform.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField(new GUIContent(scr.NameBase()), styleNamePlatform);

            styleNamePlatform = TextStyles.LabelStyleColor(Color.gray);
            styleNamePlatform.fontSize = 12;
            styleNamePlatform.fontStyle = FontStyle.Bold;
            if (!iconPlatform)
                styleNamePlatform.alignment = TextAnchor.MiddleCenter;

            GUIContent tooltip = new GUIContent(scr.nameFull + "_yg", Langs.t_nameDefining);
            EditorGUILayout.LabelField(tooltip, styleNamePlatform);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            if (YG2.infoYG.platformInfo.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Any(field =>
                {
                    var attribute = field.GetCustomAttribute<PlatformAttribute>();
                    return attribute != null && attribute.name == scr.NameBase();
                }))
            {
                EditorGUILayout.LabelField($"{Langs.platformInfo} {scr.NameBase()}", TextStyles.Header());
                DisplayFieldsWithToggles(YG2.infoYG.platformInfo, scr.NameBase());
                GUILayout.Space(20);
            }

            EditorGUILayout.LabelField($"{Langs.projectSettings} {scr.NameBase()}", TextStyles.Header());
            DisplayFieldsWithToggles(scr.projectSettings);

            if (YG2.infoYG.common.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Length > 0)
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField(Langs.addProjectSettings, TextStyles.Header());
                DisplayFieldsWithToggles(YG2.infoYG.common);
            }

            GUILayout.Space(20);
            if (FastButton.Stringy(Langs.applySettingsProject))
                scr.ApplyProjectSettings();

            serializedScr.ApplyModifiedProperties();
            serializedInfoYG.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(scr);
                EditorUtility.SetDirty(YG2.infoYG);
                AssetDatabase.SaveAssets();
            }

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            if (EditorUtils.IsMouseOverWindow(scr.NameBase()))
                Repaint();
        }

        private void DisplayFieldsWithToggles(object scrObject, string checkPlatform = null)
        {
            FieldInfo[] fields = scrObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            object toggleScrObject = YG2.infoYG.platformToggles;
            FieldInfo[] toggleFields = toggleScrObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                if (checkPlatform != null)
                {
                    var platformAttribute = field.GetCustomAttribute<PlatformAttribute>();
                    if (platformAttribute == null || platformAttribute.name != checkPlatform)
                    {
                        continue;
                    }
                }

                bool isToggle = false;

                for (int i = 0; i < toggleFields.Length; i++)
                {
                    FieldInfo toggle = toggleFields[i];

                    if (toggle.Name == field.Name)
                    {
                        EditorGUILayout.BeginHorizontal(YGEditorStyles.selectable);

                        Undo.RecordObject(YG2.infoYG, "Change PlatformToggles");

                        bool toggleValue = (bool)toggle.GetValue(toggleScrObject);
                        bool newToggleValue = EditorGUILayout.Toggle(toggleValue, GUILayout.Width(20));

                        if (newToggleValue != toggleValue)
                        {
                            toggle.SetValue(toggleScrObject, newToggleValue);
                            EditorUtility.SetDirty(YG2.infoYG);
                        }

                        EditorGUI.BeginDisabledGroup(!newToggleValue);
                        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(field.Name), GUILayout.Width(180));
                        GUILayout.FlexibleSpace();
                        DrawField(field, scrObject);
                        EditorGUI.EndDisabledGroup();

                        EditorGUILayout.EndHorizontal();

                        isToggle = true;
                        break;
                    }
                }

                if (!isToggle)
                {
                    EditorGUILayout.BeginHorizontal(YGEditorStyles.selectable);
                    EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(field.Name), GUILayout.Width(200));
                    GUILayout.FlexibleSpace();
                    DrawField(field, scrObject);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void DrawField(FieldInfo field, object target)
        {
            object value = field.GetValue(target);

            GUILayoutOption valueWidth = GUILayout.Width(120);

            if (field.FieldType == typeof(bool))
            {
                field.SetValue(target, EditorGUILayout.Toggle((bool)value, valueWidth));
            }
            else if (field.FieldType == typeof(string))
            {
                field.SetValue(target, EditorGUILayout.TextField((string)value, valueWidth));
            }
            else if (field.FieldType == typeof(int))
            {
                field.SetValue(target, EditorGUILayout.IntField((int)value, valueWidth));
            }
            else if (field.FieldType == typeof(float))
            {
                field.SetValue(target, EditorGUILayout.FloatField((float)value, valueWidth));
            }
            else if (field.FieldType.IsEnum)
            {
                field.SetValue(target, EditorGUILayout.EnumPopup((Enum)value, valueWidth));
            }
            else
            {
                EditorGUILayout.LabelField(field.Name, $"Unsupported field type: {field.FieldType}");
            }
        }
    }
}
