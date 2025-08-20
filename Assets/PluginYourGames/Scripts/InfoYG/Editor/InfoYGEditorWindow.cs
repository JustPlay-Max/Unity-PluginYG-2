namespace YG.EditorScr
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using YG.Insides;
    using YG.EditorScr.BuildModify;

    public class InfoYGEditorWindow : EditorWindow
    {
        private InfoYG scr;
        private SerializedObject serializedObject;
        private string lastPlatform;
        private int versionUpdates;
        private bool isExampleFiles = true;

        private Texture2D iconPluginYG2, iconSettings, iconDebugging, iconTemplate, iconConnect, iconPlatform;
        private Vector2 scrollPosition;

        private List<SerializedProperty> moduleIterators = new List<SerializedProperty>();
        private Texture2D[] moduleTextures = new Texture2D[0];

        [MenuItem("Tools/YG2/" + Langs.settings, false, 0)]
        public static void ShowWindow()
        {
            InfoYGEditorWindow window = (InfoYGEditorWindow)GetWindow(typeof(InfoYGEditorWindow));
            window.titleContent = new GUIContent("  Settings YG2", (Texture2D)AssetDatabase.LoadAssetAtPath(InfoYG.PACH_ASSETS_ICON_YG2, typeof(Texture2D)));
            window.minSize = new Vector2(400, 700);
            window.Show();
        }

        private void OnEnable()
        {
            scr = InfoYG.Inst();
            serializedObject = new SerializedObject(scr);

            isExampleFiles = Directory.Exists(InfoYG.PATCH_PC_EXAMPLE);
            ExampleScenes.LoadSceneList();

            if (scr != null && scr.Basic.platform != null)
                lastPlatform = PlatformSettings.currentPlatformFullName;

            Serialize();
            YGEditorStyles.ReinitializeStyles();

            ServerInfo.onLoadServerInfo += OnLoadServerInfo;
            ModuleQueue.onModuleLoaded += OnLoadServerInfo;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            ModifyBuild.onModifyComplete += Serialize;
        }

        private void OnDisable()
        {
            ServerInfo.onLoadServerInfo -= OnLoadServerInfo;
            ModuleQueue.onModuleLoaded -= OnLoadServerInfo;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            ModifyBuild.onModifyComplete -= Serialize;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state) => Serialize();
        private void OnLoadServerInfo() => versionUpdates = VersionUpdatesLabel();

        private void Serialize()
        {
            OnLoadServerInfo();

            CreateIcon(InfoYG.PATCH_PC_ICON_YG2, out iconPluginYG2);
            CreateIcon(Path.Combine(InfoYG.PATCH_PC_ICONS, "Settings.png"), out iconSettings);
            CreateIcon(Path.Combine(InfoYG.PATCH_PC_ICONS, "Debugging.png"), out iconDebugging);
            CreateIcon(Path.Combine(InfoYG.PATCH_PC_ICONS, "ImageLoad.png"), out iconTemplate);
            CreateIcon(Path.Combine(InfoYG.PATCH_PC_ICONS, "Platform.png"), out iconConnect);
            CreateIcon(PlatformSettingsEditor.GetIconCurrentPlatformPath(PlatformSettings.currentPlatformBaseName), out iconPlatform);

            string[] modules = Directory.GetDirectories(InfoYG.PATCH_PC_MODULES);
            for (int i = 0; i < modules.Length; i++)
                modules[i] = Path.GetFileName(modules[i]);

            SerializedObject serializedObject = new SerializedObject(scr);
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                bool cont = false;
                for (int i = 0; i < moduleIterators.Count; i++)
                {
                    if (moduleIterators[i].name == iterator.name)
                        cont = true;
                }
                if (cont)
                    continue;

                for (int i = 0; i < modules.Length; i++)
                {
                    if (modules[i] == iterator.name)
                    {
                        SerializedProperty newProperty = serializedObject.FindProperty(iterator.name);
                        moduleIterators.Add(newProperty);
                        break;
                    }
                }
            }

            moduleTextures = new Texture2D[moduleIterators.Count];
            for (int i = 0; i < moduleIterators.Count; i++)
            {
                string modulName = moduleIterators[i].name;
                string texturePath = Path.Combine(InfoYG.PATCH_PC_MODULES, modulName, "Scripts", "Editor", "Icons", modulName + ".png");

                if (File.Exists(texturePath))
                    CreateIcon(texturePath, out moduleTextures[i]);
            }

            Repaint();
        }

        private void Reserialize()
        {
            OnDisable();
            moduleIterators = new List<SerializedProperty>();
            moduleTextures = new Texture2D[0];
            OnEnable();
        }

        public static bool CreateIcon(string pach, out Texture2D textureProperty)
        {
            if (!File.Exists(pach))
            {
                textureProperty = null;
                return false;
            }

            byte[] fileData = File.ReadAllBytes(pach);
            textureProperty = new Texture2D(2, 2);
            textureProperty.LoadImage(fileData);
            return true;
        }

        private void OnGUI()
        {
            if (serializedObject == null || serializedObject.targetObject == null)
            {
                Reserialize();
            }

            serializedObject.Update();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            if (scr == null)
            {
                GUILayout.Label("Error!", EditorStyles.boldLabel);
                return;
            }
            GUIStyle styleOrange = TextStyles.Orange();
            GUIStyle styleGray = TextStyles.Gray();

            GUILayout.BeginVertical(YGEditorStyles.box);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Langs.switchLang, YGEditorStyles.button))
            {
#if RU_YG2
                DefineSymbols.RemoveDefine("RU_YG2");
#else
                DefineSymbols.AddDefine("RU_YG2");
#endif
                Repaint();
            }
            GUILayout.Label($"    PluginYG v{InfoYG.VERSION_YG2}", styleOrange);
            GUILayout.Label($"Unity v{Application.unityVersion}", styleOrange);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            DocumentationEditor.DocButton();
            DocumentationEditor.HelpButton();
            DocumentationEditor.VideoButton();
            GUILayout.EndHorizontal();
            GUILayout.Space(2);

            GUIStyle buttonStyle = new GUIStyle(YGEditorStyles.button);
            string versionButtonText = Langs.versionControl + "  (";

            if (versionUpdates == 0)
            {
                versionButtonText += Langs.versionsActual.ToLower();
            }
            else if (versionUpdates == 1)
            {
                versionButtonText += Langs.versionsUpdate.ToLower();
                buttonStyle.normal.textColor = TextStyles.colorGreen;
            }
            else if (versionUpdates == 2)
            {
                versionButtonText += Langs.versionsCritical.ToLower();
                buttonStyle.normal.textColor = new Color(1.5f, 0.4f, 0.1f);
            }

            versionButtonText += ")";

            if (GUILayout.Button(versionButtonText, buttonStyle, GUILayout.Height(23)))
                VersionControlWindow.ShowWindow();

            GUILayout.EndVertical();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            GUIStyle styleHeader = TextStyles.White();
            styleHeader.fontSize = 18;
            styleHeader.fontStyle = FontStyle.Bold;

            GUIStyle styleHeader2 = TextStyles.Gray();
            styleHeader2.fontSize = 12;
            styleHeader2.fontStyle = FontStyle.Bold;

            if (iconPluginYG2)
            {
                GUILayout.Space(20);

                Rect textureRect = GUILayoutUtility.GetRect(40, 40, GUILayout.ExpandWidth(false));
                GUI.DrawTexture(textureRect, iconPluginYG2);
                GUILayout.Space(10);

                Rect textureMiddleRect = GUILayoutUtility.GetRect(20, 20, GUILayout.ExpandWidth(false));
                Vector2 pivot = new Vector2(textureMiddleRect.x + textureMiddleRect.width / 2, textureMiddleRect.y + textureMiddleRect.height / 2);
                Matrix4x4 originalMatrix = GUI.matrix;
                GUIUtility.RotateAroundPivot(45f, pivot);
                Rect rotatedRect = new Rect(textureMiddleRect.x + 7, textureMiddleRect.y + 7, textureMiddleRect.width, textureMiddleRect.height);
                GUI.DrawTexture(rotatedRect, iconConnect);
                GUI.matrix = originalMatrix;
                GUILayout.Space(8);

                if (iconPlatform)
                {
                    Rect textureLastRect = GUILayoutUtility.GetRect(40, 40, GUILayout.ExpandWidth(false));
                    GUI.DrawTexture(textureLastRect, iconPlatform);
                    GUILayout.Space(10);
                }
            }
            else
            {
                styleHeader.alignment = TextAnchor.MiddleCenter;
                styleHeader2.alignment = TextAnchor.MiddleCenter;
            }

            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("PLUGIN YG2", styleHeader);

            EditorGUILayout.LabelField(Langs.fullNamePlugin.ToUpper(), styleHeader2);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal(YGEditorStyles.boxLight);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Basic"), new GUIContent("Basic Settings"), true);
            if (iconSettings)
            {
                Rect textureRect = GUILayoutUtility.GetRect(18, 18, GUILayout.ExpandWidth(false));
                GUI.DrawTexture(textureRect, iconSettings);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(YGEditorStyles.boxLight);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Simulation"), new GUIContent("General Simulation"), true);
            if (iconDebugging)
            {
                Rect textureRect = GUILayoutUtility.GetRect(18, 18, GUILayout.ExpandWidth(false));
                GUI.DrawTexture(textureRect, iconDebugging);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(YGEditorStyles.boxLight);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Templates"), true);
            if (iconTemplate)
            {
                Rect textureRect = GUILayoutUtility.GetRect(18, 18, GUILayout.ExpandWidth(false));
                GUI.DrawTexture(textureRect, iconTemplate);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (moduleIterators.Count > 0)
            {
                EditorGUILayout.LabelField(Langs.modulesSettings, TextStyles.Header());

                for (int i = 0; i < moduleIterators.Count; i++)
                {
                    moduleIterators[i].serializedObject.Update();

                    if (moduleTextures[i] == null)
                    {
                        GUILayout.BeginVertical(YGEditorStyles.boxLight);
                        EditorGUILayout.PropertyField(moduleIterators[i], true);
                        GUILayout.EndVertical();
                    }
                    else
                    {
                        GUILayout.BeginHorizontal(YGEditorStyles.boxLight);

                        EditorGUILayout.PropertyField(moduleIterators[i], true);

                        Rect textureRect = GUILayoutUtility.GetRect(18, 18, GUILayout.ExpandWidth(false));
                        GUI.DrawTexture(textureRect, moduleTextures[i]);

                        GUILayout.EndHorizontal();
                    }

                    moduleIterators[i].serializedObject.ApplyModifiedProperties();
                }
            }

            string currentPlatform = PlatformSettings.currentPlatformFullName;

            if (lastPlatform != currentPlatform)
            {
                InfoYG.SetPlatform(currentPlatform);

                if (currentPlatform == "NullPlatform")
                {
                    YG2.infoYG.Basic.platform = null;
                }
                else
                {
                    if (scr.Basic.autoApplySettings)
                        scr.Basic.platform.ApplyProjectSettings();

                    PlatformSettings.SelectPlatform();
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            }
            lastPlatform = currentPlatform;

            GUILayout.Space(20);

            if (FastButton.Stringy(Langs.resetInfoSettings))
            {
                if (EditorUtility.DisplayDialog(Langs.resetInfoSettings, Langs.resetInfoSettings_dialog, "Ok", Langs.cancel))
                {
                    Undo.RecordObject(scr, "Set default settings PluginYG2");
                    PlatformSettings platformSave = YG2.infoYG.Basic.platform;

                    InfoYG tempInfoYG = ScriptableObject.CreateInstance<InfoYG>();
                    tempInfoYG.Basic.platform = platformSave;

                    string tempPath = "Assets/SettingsYG2.asset";
                    AssetDatabase.CreateAsset(tempInfoYG, tempPath);
                    AssetDatabase.SaveAssets();

                    EditorUtility.CopySerialized(tempInfoYG, InfoYG.instance);
                    AssetDatabase.DeleteAsset(tempPath);

                    EditorUtility.SetDirty(InfoYG.Inst());
                    AssetDatabase.Refresh();
                    Reserialize();
                }
            }

            if (!DefineSymbols.CheckDefine(DefineSymbols.NJSON_DEFINE))
            {
                if (FastButton.Stringy(Langs.importNJSON))
                    UnityPackagesManager.ImportPackage(DefineSymbols.NJSON_PACKAGE);

            }
#if Storage_yg
            if (DefineSymbols.CheckDefine(DefineSymbols.NJSON_DEFINE))
            {
                if (!DefineSymbols.CheckDefine(DefineSymbols.NJSON_STORAGE_DEFINE))
                {
                    if (FastButton.Stringy(Langs.activateNJSONForSave))
                        DefineSymbols.AddDefine(DefineSymbols.NJSON_STORAGE_DEFINE);
                }
                else
                {
                    if (FastButton.Stringy(Langs.deactivateNJSONForSave))
                        DefineSymbols.RemoveDefine(DefineSymbols.NJSON_STORAGE_DEFINE);
                }
            }
#endif

            if (isExampleFiles)
            {
                GUILayout.Space(20);
                GUILayout.BeginVertical(YGEditorStyles.box);

                GUILayout.Label(Langs.demoScenesInBuildSettings, styleOrange);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(Langs.addDemoScenes, YGEditorStyles.button))
                    ExampleScenes.AddScenesToBuildSettings();
                if (GUILayout.Button(Langs.delete, YGEditorStyles.button))
                    ExampleScenes.RemoveScenesFromBuildSettings();
                GUILayout.EndHorizontal();

                string scenesStr;
                if (ExampleScenes.sceneNames.Length > 0)
                {
                    scenesStr = Langs.demoScenes;
                    foreach (var scene in ExampleScenes.sceneNames)
                    {
                        scenesStr += $"{scene},  ";
                    }
                    scenesStr = scenesStr.TrimEnd(',', ' ');
                }
                else
                {
                    scenesStr = Langs.demoNotAdded;
                }
                GUILayout.Label(scenesStr, EditorStyles.helpBox);

                GUILayout.Space(7);
                if (GUILayout.Button(Langs.deleteAllDemoMaterialsFromProject, YGEditorStyles.button))
                {
                    if (EditorUtility.DisplayDialog(Langs.removeAllDemoMaterials, Langs.removeAllDemoDialog, Langs.removeAllDemoMaterials, Langs.cancel))
                    {
                        ExampleScenes.RemoveScenesFromBuildSettings();

                        List<string> directories = Directory.GetDirectories(InfoYG.PATCH_PC_MODULES).ToList();
                        for (int i = 0; i < directories.Count; i++)
                        {
                            directories[i] += "/Example";
                            if (Directory.Exists(directories[i]))
                                FileYG.DeleteDirectory(directories[i]);
                        }

                        FileYG.DeleteDirectory(InfoYG.PATCH_PC_EXAMPLE);
                        AssetDatabase.Refresh();
                    }
                }
                GUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            if (EditorUtils.IsMouseOverWindow(this))
                Repaint();
        }

        private int VersionUpdatesLabel()
        {
            int versionUpdates = 0;
            List<string> modulesStr = new List<string>
            {
                $"PluginYG2 v{InfoYG.VERSION_YG2}"
            };

            string[] platfomFolders = Directory.GetDirectories(InfoYG.PATCH_PC_PLATFORMS);
            string[] platfomNames = new string[platfomFolders.Length];

            for (int i = 0; i < platfomFolders.Length; i++)
                platfomNames[i] = Path.GetFileName(platfomFolders[i]);

            for (int i = 0; i < platfomNames.Length; i++)
            {
                string platfomVersionPathc = $"{platfomFolders[i]}/Version.txt";

                if (File.Exists(platfomVersionPathc))
                {
                    string version = File.ReadAllText(platfomVersionPathc);
                    platfomNames[i] += " " + version;
                }
            }

            modulesStr.AddRange(platfomNames);

            if (File.Exists(InfoYG.FILE_MODULES_PC))
                modulesStr.AddRange(File.ReadAllLines(InfoYG.FILE_MODULES_PC).ToList());

            for (int i = 0; i < modulesStr.Count; i++)
            {
                if (modulesStr[i] == string.Empty)
                    continue;

                string name = modulesStr[i];
                string version = "0";

                int spaceIndex = modulesStr[i].IndexOf(" ");
                if (spaceIndex > -1)
                {
                    name = modulesStr[i].Remove(spaceIndex);
                    version = modulesStr[i].Remove(0, spaceIndex + 2);
                }

                for (int j = 0; j < ServerInfo.saveInfo.modules.Length; j++)
                {
                    if (name == ServerInfo.saveInfo.modules[j].name)
                    {
                        float.TryParse(version, NumberStyles.Float, CultureInfo.InvariantCulture, out float projectVersion);
                        float.TryParse(ServerInfo.saveInfo.modules[j].version, NumberStyles.Float, CultureInfo.InvariantCulture, out float lastVersion);

                        if (lastVersion > projectVersion)
                        {
                            versionUpdates = 1;
                            if (ServerInfo.saveInfo.modules[j].critical)
                                return 2;
                        }
                    }
                }
            }
            return versionUpdates;
        }

        [Serializable]
        class AsmdefJson
        {
            public string name;
            public string rootNamespace;
            public string[] references;
            public string[] includePlatforms;
            public string[] excludePlatforms;
            public bool allowUnsafeCode;
            public bool[] overrideReferences;
            public string[] precompiledReferences;
            public bool autoReferenced;
            public string[] defineConstraints;
            public string[] versionDefines;
            public bool noEngineReferences;
        }
    }
}
