using UnityEngine;
using UnityEditor;
using System.IO;

namespace YG.EditorScr
{
    [InitializeOnLoad]
    public static class IconEditorSaveFile
    {
        private const string PATH_FILE = "Editor/SavesEditorYG2.json";
        private static Texture2D customIcon;

        static IconEditorSaveFile()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.update += Setup;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            Setup();
        }

        private static void Setup()
        {
            if (customIcon != null)
                return;

            string texturePath = $"Storage/Scripts/Editor/Icons/Storage.png";

            if (!File.Exists($"{InfoYG.PATCH_PC_MODULES}/{texturePath}") || !File.Exists($"{InfoYG.PATCH_PC_YG2}/{PATH_FILE}"))
                return;

            customIcon = AssetDatabase.LoadAssetAtPath<Texture2D>($"{InfoYG.PATCH_ASSETS_MODULES}/{texturePath}");
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemOnGUI;
        }

        private static void OnProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (path == $"{InfoYG.PATCH_ASSETS_YG2}/{PATH_FILE}")
            {
                Rect iconRect = new Rect(selectionRect.x + 4, selectionRect.y + 1, 14, 14);
                GUI.DrawTexture(iconRect, customIcon);
            }
        }
    }
}