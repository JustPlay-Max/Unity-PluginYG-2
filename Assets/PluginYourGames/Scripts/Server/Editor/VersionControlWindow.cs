using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using YG.Insides;

namespace YG.EditorScr
{
    public class VersionControlWindow : EditorWindow
    {
        public const string REMOVE_BEFORE_IMPORT_TOGGLE_KEY = "RemoveBeforeImport_YG2";
        public const string SELECT_MODULES_KEY = "SelectModuleToggle_YG2";
        private const string TAB_KEY = "YG2_VersionControl_Tab";
        private enum TabSection { Modules = 0, Platforms = 1, Tools = 2 }
        private static TabSection currentTab = TabSection.Modules;

        public static bool isOpenWindow { get => instance; }
        public static VersionControlWindow instance;

        private float rowHeight = 20f;
        private static VersionControlWindow window;
        private static List<Module> modulesAll = new List<Module>();
        private static List<Module> modules = new List<Module>();

        private static bool isDownloadProcess;
        private static bool removeBeforeImport;

        private Vector2 scrollPosition;
        private bool cloudComplete = true;

        [MenuItem("Tools/YG2/" + Langs.versionControl, false, 1)]
        public static void ShowWindow()
        {
            window = GetWindow<VersionControlWindow>(Langs.versionControl + " YG2");
            window.position = new Rect(300, 200, 900, 800);
            window.minSize = new Vector2(700, 500);
            Server.LoadServerInfo(true);
        }

        private void OnEnable()
        {
            instance = this;

            if (PluginPrefs.GetInt(InfoYG.FIRST_STARTUP_KEY) == 0)
                return;

            removeBeforeImport = EditorPrefs.GetBool(REMOVE_BEFORE_IMPORT_TOGGLE_KEY, true);
            currentTab = (TabSection)EditorPrefs.GetInt(TAB_KEY, (int)TabSection.Modules);

            ServerInfo.onLoadServerInfo += OnLoadServerInfo;
            EditorApplication.projectChanged += OnLoadServerInfo;
            ModulesInstaller.onDownloadProcess += OnDownloadProcess;
            ModuleQueue.onModuleLoaded += OnModuleLoaded;

            if (!Server.loadComplete)
                InitData(null);
            else
                InitData(ServerInfo.saveInfo);
        }

        private void OnDisable()
        {
            instance = null;

            if (PluginPrefs.GetInt(InfoYG.FIRST_STARTUP_KEY) == 0)
                return;

            ServerInfo.onLoadServerInfo -= OnLoadServerInfo;
            EditorApplication.projectChanged -= OnLoadServerInfo;
            ModulesInstaller.onDownloadProcess -= OnDownloadProcess;
            ModuleQueue.onModuleLoaded -= OnModuleLoaded;
        }

        private void OnModuleLoaded() => InitData(ServerInfo.saveInfo);
        private void OnLoadServerInfo() => InitData(ServerInfo.saveInfo);
        private void OnDownloadProcess(bool isProcess) => isDownloadProcess = isProcess;

        private void InitData(ServerJson cloud)
        {
            cloudComplete = cloud != null;
            modulesAll = ModulesList.GetGeneratedList(cloud);
            ApplyTabFilterAndBuildVisibleList();
        }

        private void ApplyTabFilterAndBuildVisibleList()
        {
            IEnumerable<Module> filtered = modulesAll;

            switch (currentTab)
            {
                case TabSection.Modules:
                    filtered = modulesAll.Where(m => !m.platform && !m.tool);
                    break;

                case TabSection.Platforms:
                    filtered = modulesAll.Where(m => m.platform && !m.tool);
                    break;

                case TabSection.Tools:
                    filtered = modulesAll.Where(m => m.tool);
                    break;
            }

            modules = filtered.ToList();

            // Select Panel
            if (modules.Count > 1)
            {
                Module allModule = new Module
                {
                    nameModule = SELECT_MODULES_KEY,
                    projectVersion = "0",
                    doc = modules[0].doc,
                    select = PrefsList.Contains(SELECT_MODULES_KEY, SELECT_MODULES_KEY)
                };
                modules.Insert(0, allModule);
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            removeBeforeImport = EditorGUILayout.ToggleLeft(Langs.removeBeforeImport, removeBeforeImport);
            if (EditorGUI.EndChangeCheck())
                EditorPrefs.SetBool(REMOVE_BEFORE_IMPORT_TOGGLE_KEY, removeBeforeImport);

            GUIStyle allowUpdateStyle = ButtonStyle();
            Rect rect = new Rect();
            Rect btnRectC = new Rect();

            allowUpdateStyle = HasAnyUpdatesAll() ? GreenButtonStyle() : ButtonStyle();

            if (GUILayout.Button(Langs.changelog, allowUpdateStyle, GUILayout.Width(150)))
            {
                DocumentationEditor.HelpMenuItem();
            }
            GUILayout.Space(5);

            if (FastButton.Stringy(Langs.updateInfo))
            {
                InitData(null);
                Server.LoadServerInfo();
            }

            GUILayout.EndHorizontal();

            DrawTabsBar();

            if (isDownloadProcess || modules == null || modules.Count == 0)
            {
                GUILayout.Space(20);
                GUIStyle centeredStyle = TextStyles.Header();
                centeredStyle.alignment = TextAnchor.MiddleCenter;
                centeredStyle.fontSize = 14;
                EditorGUILayout.LabelField(Langs.loading, centeredStyle);
                return;
            }

            GUIStyle labelStyleGreen = TextStyles.Green();
            GUIStyle labelStyleGray = TextStyles.Gray();
            labelStyleGray.fontSize = 11;
            GUIStyle labelStyleOrange = TextStyles.Header();

            float columnWidth_Toggle = 30;
            float columnWidth = (position.width - columnWidth_Toggle) / 4;
            float columnWidth_Name = columnWidth + 20;
            float columnWidth_ProjectVersion = columnWidth - 20;
            float columnWidth_LatestVersion = columnWidth - 30;
            float columnWidth_Control = columnWidth - 10;

            //GUILayout.Space(10);
            using (new GUILayout.HorizontalScope(YGEditorStyles.box))
            {
                GUILayout.Space(columnWidth_Toggle);
                GUILayout.Label(Langs.name, labelStyleOrange, GUILayout.Width(columnWidth_Name - 6), GUILayout.Height(rowHeight));
                GUILayout.Label(Langs.projectVersion, labelStyleOrange, GUILayout.Width(columnWidth_ProjectVersion), GUILayout.Height(rowHeight));
                GUILayout.Label(Langs.latestVersion, labelStyleOrange, GUILayout.Width(columnWidth_LatestVersion), GUILayout.Height(rowHeight));
                GUIStyle headerCenter = TextStyles.Header();
                headerCenter.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label(Langs.control, headerCenter, GUILayout.Width(columnWidth_Control), GUILayout.Height(rowHeight));
            }

            using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scroll.scrollPosition;

                for (int i = 0; i < modules.Count; i++)
                {
                    Module module = modules[i];
                    bool isSelectPanel = i == 0;

                    using (new GUILayout.HorizontalScope(isSelectPanel || !EditorUtils.IsMouseOverWindow(this) ? YGEditorStyles.deselectable : YGEditorStyles.selectable))
                    {
                        bool imported = true;
                        string projectVersionStr = CorrectVersionString(module.projectVersion);
                        string lastVersionStr = CorrectVersionString(module.lastVersion);

                        if (!isSelectPanel)
                        {
                            if (string.IsNullOrEmpty(projectVersionStr))
                            {
                                imported = false;
                                projectVersionStr = "not imported";
                            }
                        }

                        string CorrectVersionString(string versionString)
                        {
                            if (string.IsNullOrEmpty(versionString))
                                return string.Empty;

                            versionString = versionString.Replace(",", ".").Trim();

                            if (double.TryParse(versionString, NumberStyles.Float,
                                                CultureInfo.InvariantCulture, out var value))
                            {
                                return (value % 1 == 0)
                                    ? value.ToString("0.0", CultureInfo.InvariantCulture)
                                    : value.ToString(CultureInfo.InvariantCulture);
                            }

                            var sb = new System.Text.StringBuilder();
                            bool seenDigit = false, seenDot = false;
                            foreach (char ch in versionString)
                            {
                                if (char.IsDigit(ch)) { sb.Append(ch); seenDigit = true; continue; }
                                if (ch == '.' && !seenDot) { sb.Append('.'); seenDot = true; continue; }
                                if (seenDigit) break;
                            }

                            var token = sb.ToString();
                            if (token.Length > 0 && double.TryParse(token, NumberStyles.Float,
                                                                    CultureInfo.InvariantCulture, out value))
                            {
                                return (value % 1 == 0)
                                    ? value.ToString("0.0", CultureInfo.InvariantCulture)
                                    : value.ToString(CultureInfo.InvariantCulture);
                            }

                            return versionString;
                        }

                        // Toggle Select
                        rect = GUILayoutUtility.GetRect(new GUIContent("Toggles"), GUIStyle.none, GUILayout.Width(columnWidth_Toggle), GUILayout.Height(rowHeight));

                        if (isSelectPanel)
                        {
                            EditorGUI.BeginChangeCheck();
                            module.select = GUI.Toggle(rect, module.select, "");

                            if (EditorGUI.EndChangeCheck())
                            {
                                if (module.select)
                                {
                                    for (int k = 1; k < modules.Count; k++)
                                    {
                                        modules[k].select = true;
                                        PrefsList.Add(SELECT_MODULES_KEY, modules[k].nameModule);
                                    }
                                    PrefsList.Add(SELECT_MODULES_KEY, SELECT_MODULES_KEY);
                                }
                                else
                                {
                                    for (int k = 1; k < modules.Count; k++)
                                        modules[k].select = false;

                                    PrefsList.Remove(SELECT_MODULES_KEY, SELECT_MODULES_KEY);
                                    for (int k = 1; k < modules.Count; k++)
                                        PrefsList.Remove(SELECT_MODULES_KEY, modules[k].nameModule);
                                }
                            }
                        }
                        else
                        {
                            EditorGUI.BeginChangeCheck();
                            module.select = GUI.Toggle(rect, module.select, "");

                            if (EditorGUI.EndChangeCheck())
                            {
                                if (module.select)
                                {
                                    PrefsList.Add(SELECT_MODULES_KEY, module.nameModule);
                                }
                                else
                                {
                                    PrefsList.Remove(SELECT_MODULES_KEY, module.nameModule);
                                    PrefsList.Remove(SELECT_MODULES_KEY, SELECT_MODULES_KEY);
                                    modules[0].select = false;
                                }
                            }
                        }

                        // Name
                        rect = GUILayoutUtility.GetRect(new GUIContent(module.nameModule), GUIStyle.none, GUILayout.Width(columnWidth_Name), GUILayout.Height(rowHeight));
                        GUIStyle labelStyleName;

                        if (isSelectPanel)
                        {
                            int selectedCount = 0;
                            for (int m = 1; m < modules.Count; m++)
                            {
                                if (modules[m].select)
                                    selectedCount++;
                            }

                            if (selectedCount != 0)
                                labelStyleName = TextStyles.Header();
                            else
                                labelStyleName = TextStyles.LabelStyleColor(new Color(0.7f, 0.7f, 0.7f));

                            GUI.Label(rect, $"Selected modules: {selectedCount}", labelStyleName);
                        }
                        else
                        {
                            if (imported)
                            {
                                if (ModulesInstaller.IsModuleCurrentVersion(module))
                                    labelStyleName = TextStyles.White();
                                else
                                    labelStyleName = TextStyles.Green();
                            }
                            else
                            {
                                if (EditorGUIUtility.isProSkin)
                                    labelStyleName = TextStyles.LabelStyleColor(new Color(0.75f, 0.75f, 0.75f));
                                else
                                    labelStyleName = TextStyles.LabelStyleColor(new Color(0.3f, 0.3f, 0.3f));
                            }

                            string drawName = module.nameModule;

                            if (drawName == InfoYG.NAME_PLUGIN)
                                drawName = InfoYG.FULL_NAME_PLUGIN;
                            else
                                drawName = TextStyles.AddSpaces(drawName);

                            // можно добавить визуальную пометку

                            //if (module.platform)
                            //    drawName += " - platform";

                            // if (module.tool)
                            //     drawName += " - tool";

                            GUI.Label(rect, drawName, labelStyleName);
                        }

                        // Project version
                        rect = GUILayoutUtility.GetRect(new GUIContent(projectVersionStr), GUIStyle.none, GUILayout.Width(columnWidth_ProjectVersion), GUILayout.Height(rowHeight));

                        if (isSelectPanel)
                        {
                            List<Module> allowModules = new List<Module>();

                            for (int m = 1; m < modules.Count; m++)
                            {
                                if (modules[m].select && !string.IsNullOrEmpty(modules[m].projectVersion))
                                    allowModules.Add(modules[m]);
                            }

                            Rect btnRect = new Rect(rect.x, rect.y, 125, rect.height);

                            if (allowModules.Count > 0)
                            {
                                if (GUI.Button(btnRect, "Delete selected", ButtonStyle()))
                                {
                                    if (modules.Count > 1 && modules[1].select && modules[1].nameModule == InfoYG.NAME_PLUGIN)
                                    {
                                        if (WarningDeletePlugin())
                                        {
                                            DeletePlugin();
                                        }
                                    }
                                    else
                                    {
                                        string dialogText = $"{Langs.deleteModule}:\n";
                                        foreach (Module m in allowModules)
                                            dialogText += "\n• " + m.nameModule;

                                        if (EditorUtility.DisplayDialog(Langs.deleteModule, dialogText, "Ok", Langs.cancel))
                                        {
                                            foreach (Module m in allowModules)
                                                DeleteModule(m, btnRect, true);

                                            AssetDatabase.Refresh();
                                            InitData(ServerInfo.saveInfo);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Rect labelRect = new Rect(rect.x + 30, rect.y, rect.width, rect.height);

                            if (imported)
                            {
                                if (!ModulesInstaller.IsModuleCurrentVersion(module) && module.critical)
                                    GUI.Label(labelRect, projectVersionStr, TextStyles.Red());
                                else
                                    GUI.Label(labelRect, projectVersionStr);
                            }
                            else
                            {
                                GUI.Label(labelRect, projectVersionStr, labelStyleGray);
                            }
                        }

                        // Latest version
                        rect = GUILayoutUtility.GetRect(new GUIContent(lastVersionStr), GUIStyle.none, GUILayout.Width(columnWidth_LatestVersion), GUILayout.Height(rowHeight));

                        if (isSelectPanel)
                        {
                            List<Module> allowModules = new List<Module>();

                            for (int m = 1; m < modules.Count; m++)
                            {
                                if (!modules[m].select || modules[m].noLoad || string.IsNullOrEmpty(modules[m].download))
                                    continue;

                                if (!string.IsNullOrEmpty(modules[m].projectVersion) && ModulesInstaller.IsModuleCurrentVersion(modules[m]))
                                    continue;

                                allowModules.Add(modules[m]);
                            }

                            Rect btnRect = new Rect(rect.x, rect.y, 125, rect.height);

                            if (allowModules.Count > 0)
                            {
                                if (GUI.Button(btnRect, "Install selected", ButtonStyle()))
                                {
                                    if (ModulesInstaller.ApprovalDownload())
                                    {
                                        int selectedCount = 0;
                                        for (int m = 1; m < modules.Count; m++)
                                            if (modules[m].select)
                                                selectedCount++;

                                        bool executeInstall = true;
                                        if (selectedCount >= modules.Count - 1)
                                        {
                                            if (!EditorUtility.DisplayDialog("Install all modules?", Langs.importAllModules, "Ok", Langs.cancel))
                                                executeInstall = false;
                                        }

                                        if (executeInstall)
                                        {
                                            for (int m = 0; m < allowModules.Count; m++)
                                                ModuleQueue.AddList(allowModules[m].nameModule);

                                            ModuleQueue.ProcessInstallModulesInTurn();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Rect labelRect = new Rect(rect.x + 30, rect.y, rect.width, rect.height);

                            if (cloudComplete)
                            {
                                if (lastVersionStr == string.Empty || lastVersionStr == null)
                                {
                                    lastVersionStr = "not found";
                                    GUI.Label(labelRect, lastVersionStr, labelStyleGray);
                                }
                                else if (imported && !ModulesInstaller.IsModuleCurrentVersion(module))
                                {
                                    if (module.critical)
                                        lastVersionStr += "  critical!";

                                    GUI.Label(labelRect, lastVersionStr, labelStyleGreen);
                                }
                                else
                                {
                                    GUI.Label(labelRect, lastVersionStr);
                                }
                            }
                            else
                            {
                                GUI.Label(labelRect, Langs.loading, labelStyleGray);
                            }
                        }

                        // Control
                        rect = GUILayoutUtility.GetRect(new GUIContent("Control"), GUIStyle.none, GUILayout.Width(columnWidth_Control / 1.5f), GUILayout.Height(rowHeight));
                        btnRectC = new Rect(rect.x, rect.y, 125, rect.height);

                        if (!cloudComplete)
                        {
                            GUI.Label(rect, Langs.loading, labelStyleGray);
                        }
                        else
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                if (isSelectPanel)
                                {
                                    bool anyUpdates = HasAnyUpdatesInList(modules);
                                    bool anyBatchUpdates = HasBatchUpdatableUpdatesInList(modules);

                                    if (anyUpdates && anyBatchUpdates)
                                    {
                                        if (GUI.Button(btnRectC, Langs.updateAll, allowUpdateStyle))
                                        {
                                            if (ModulesInstaller.ApprovalDownload())
                                            {
                                                for (int m = 1; m < modules.Count; m++)
                                                {
                                                    if (modules[m] == null) continue;
                                                    if (modules[m].noLoad) continue;

                                                    if (!string.IsNullOrEmpty(modules[m].projectVersion) &&
                                                        !ModulesInstaller.IsModuleCurrentVersion(modules[m]))
                                                    {
                                                        ModuleQueue.AddList(modules[m].nameModule);
                                                    }
                                                }

                                                ModuleQueue.ProcessInstallModulesInTurn();
                                            }
                                        }
                                    }
                                }
                                else if (imported)
                                {
                                    if (!ModulesInstaller.IsModuleCurrentVersion(module))
                                    {
                                        ButtonUpdate();
                                    }
                                    else
                                    {
                                        DeleteModule(module, btnRectC, false);
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(module.download))
                                    {
                                        if (!module.noLoad)
                                        {
                                            if (GUI.Button(btnRectC, "Import", ButtonStyle()))
                                                ModulesInstaller.InstallModule(module);
                                        }
                                        else
                                        {
                                            if (GUI.Button(btnRectC, "Import by link", ButtonStyle()))
                                                Application.OpenURL(module.download);
                                        }
                                    }
                                    else
                                    {
                                        GUI.Label(rect, "package not found", labelStyleGray);
                                    }
                                }

                                rect = GUILayoutUtility.GetRect(new GUIContent("Doc"), GUIStyle.none, GUILayout.Width(columnWidth_Control / 3 - 10), GUILayout.Height(rowHeight));

                                if (isSelectPanel)
                                { }
                                else if (module.doc != string.Empty && module.doc != null)
                                {
                                    rect.x += 23;

                                    if (GUI.Button(rect, "Doc", ButtonStyle()))
                                    {
#if RU_YG2
                                        Application.OpenURL(module.doc);
#else
                                        Application.OpenURL(module.doc + "?en=");
#endif
                                    }
                                }
                                else
                                {
                                    rect.x += 23;
                                    GUI.Label(rect, "not doc", labelStyleGray);
                                }
                            }
                        }
                    }

                    void ButtonUpdate()
                    {
                        if (GUI.Button(btnRectC, "Update", allowUpdateStyle))
                        {
                            if (!module.noLoad)
                            {
                                if (module.nameModule == InfoYG.NAME_PLUGIN && removeBeforeImport)
                                {
                                    DeleteAndUpdatePlugin(module);
                                }
                                else
                                {
                                    ModulesInstaller.InstallModule(module);
                                }
                            }
                            else
                            {
                                Application.OpenURL(module.download);
                            }
                        }
                    }

                    void DeleteModule(Module module, Rect rectForButtons, bool massDeletion)
                    {
                        string pathDelete = string.Empty;
                        bool isModulPlatform = false;

                        if (module.nameModule == InfoYG.NAME_PLUGIN)
                        {
                            pathDelete = InfoYG.PATCH_PC_YG2;
                        }
                        else
                        {
                            string patchModules = $"{InfoYG.PATCH_PC_MODULES}/{module.nameModule}";
                            string patchPlatforms = $"{InfoYG.PATCH_PC_PLATFORMS}/{module.nameModule}";
                            string patchFormatPlatforms = $"{InfoYG.PATCH_PC_PLATFORMS}/{module.nameModule + "Integration"}";

                            if (Directory.Exists(patchModules))
                                pathDelete = patchModules;

                            if (Directory.Exists(patchPlatforms))
                            {
                                pathDelete = patchPlatforms;
                                isModulPlatform = true;
                            }
                            else if (Directory.Exists(patchFormatPlatforms))
                            {
                                pathDelete = patchFormatPlatforms;
                                isModulPlatform = true;
                            }
                        }

                        if (pathDelete != string.Empty)
                        {
                            bool allowDeletion = false;

                            if (module.nameModule == InfoYG.NAME_PLUGIN)
                            {
                                if (massDeletion)
                                {
                                    allowDeletion = true;
                                }
                                else if (GUI.Button(rectForButtons, "Delete all", ButtonStyle()))
                                {
                                    if (WarningDeletePlugin())
                                        allowDeletion = true;
                                }

                                if (allowDeletion)
                                {
                                    DeletePlugin();
                                }
                            }
                            else
                            {
                                if (massDeletion)
                                    allowDeletion = true;
                                else if (GUI.Button(rectForButtons, "Delete", ButtonStyle()))
                                    allowDeletion = true;

                                if (allowDeletion)
                                {
                                    if (allowDeletion || EditorUtility.DisplayDialog(Langs.deleteModule, $"{Langs.deleteModule} {module.nameModule}?", "Ok", Langs.cancel))
                                    {
                                        if (isModulPlatform)
                                        {
                                            PlatformSettings.DeletePlatform();
                                            ModulesInstaller.DeletePlatformWebGLTemplate(module.nameModule);
                                            DefineSymbols.RemoveDefine(module.nameModule + "Platform_yg");
                                        }
                                        else
                                        {
                                            DefineSymbols.RemoveDefine(module.nameModule + "_yg");
                                        }

                                        FileYG.DeleteDirectory(pathDelete);
                                        DefineSymbols.ModulesDefineSymbols();
                                        InitData(ServerInfo.saveInfo);

                                        if (!allowDeletion)
                                            AssetDatabase.Refresh();
                                    }
                                }
                            }
                        }
                        else
                        {
                            GUI.Label(rect, "manual deletion", labelStyleGray);
                        }
                    }
                }
            }

            if (EditorUtils.IsMouseOverWindow(this))
                Repaint();

            GUIStyle ButtonStyle()
            {
                if (EditorUtils.IsMouseOverWindow(this))
                    return YGEditorStyles.button;
                else
                    return YGEditorStyles.debutton;
            }

            GUIStyle GreenButtonStyle()
            {
                GUIStyle style = new GUIStyle(ButtonStyle());

                style.normal.textColor =
                    style.hover.textColor =
                    style.active.textColor =
                    style.focused.textColor =
                    style.onNormal.textColor =
                    style.onHover.textColor =
                    style.onActive.textColor =
                    style.onFocused.textColor = TextStyles.colorGreen;

                return style;
            }
        }

        private void DrawTabsBar()
        {
            GUILayout.Space(6);

            using (new GUILayout.HorizontalScope(YGEditorStyles.box))
            {
                var hasUpdatesModules = HasUpdatesForTab(TabSection.Modules);
                var hasUpdatesPlatforms = HasUpdatesForTab(TabSection.Platforms);
                var hasUpdatesTools = HasUpdatesForTab(TabSection.Tools);

                string name;
#if RU_YG2
                name = "Модули";
#else
                name = "Modules";
#endif
                if (DrawTabButton(name, currentTab == TabSection.Modules, hasUpdatesModules))
                    SetTab(TabSection.Modules);
#if RU_YG2
                name = "Платформы";
#else
                name = "Platforms";
#endif
                if (DrawTabButton(name, currentTab == TabSection.Platforms, hasUpdatesPlatforms))
                    SetTab(TabSection.Platforms);
#if RU_YG2
                name = "Инструменты";
#else
                name = "Tools";
#endif
                if (DrawTabButton(name, currentTab == TabSection.Tools, hasUpdatesTools))
                    SetTab(TabSection.Tools);
            }
        }

        // CHANGED: затемняется только фон кнопки, текст остаётся неизменным
        private bool DrawTabButton(string title, bool active, bool highlightGreen)
        {
            GUIStyle style = new GUIStyle(YGEditorStyles.button);

            // Зелёный текст при наличии обновлений — оставляем
            if (highlightGreen)
            {
                style.normal.textColor =
                    style.hover.textColor =
                    style.active.textColor =
                    style.focused.textColor = TextStyles.colorGreen;
            }

            Rect r = GUILayoutUtility.GetRect(140, 22, style);
            bool hover = r.Contains(Event.current.mousePosition);

            Color prevBg = GUI.backgroundColor;

            // Затемняем ТОЛЬКО фон и только если:
            // - вкладка не активна
            // - мышь НЕ наведена
            if (!active && !hover)
            {
                // степень затемнения регулируется этим значением
                GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1f);

                // альтернативы:
                // GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1f); // лёгкое затемнение
                // GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f, 1f); // сильнее
            }
            else
            {
                GUI.backgroundColor = prevBg;
            }

            bool pressed = GUI.Button(r, title, style);

            GUI.backgroundColor = prevBg;
            return pressed;
        }

        private void SetTab(TabSection tab)
        {
            if (currentTab == tab)
                return;

            currentTab = tab;
            EditorPrefs.SetInt(TAB_KEY, (int)currentTab);

            ApplyTabFilterAndBuildVisibleList();
            Repaint();
        }

        private bool HasUpdatesForTab(TabSection tab)
        {
            if (!cloudComplete || modulesAll == null || modulesAll.Count == 0)
                return false;

            IEnumerable<Module> filtered;

            switch (tab)
            {
                case TabSection.Modules:
                    filtered = modulesAll.Where(m => !m.platform && !m.tool);
                    break;
                case TabSection.Platforms:
                    filtered = modulesAll.Where(m => m.platform && !m.tool);
                    break;
                case TabSection.Tools:
                    filtered = modulesAll.Where(m => m.tool);
                    break;
                default:
                    filtered = modulesAll;
                    break;
            }

            foreach (var m in filtered)
            {
                if (string.IsNullOrEmpty(m.projectVersion))
                    continue; // не импортирован

                // наличие обновления = не текущая версия
                if (!ModulesInstaller.IsModuleCurrentVersion(m))
                    return true;
            }

            return false;
        }

        // NEW: есть ли вообще обновления в списке (включая noLoad)
        private bool HasAnyUpdatesInList(List<Module> list)
        {
            if (!cloudComplete || list == null || list.Count == 0)
                return false;

            for (int i = 0; i < list.Count; i++)
            {
                var m = list[i];
                if (m == null) continue;
                if (m.nameModule == SELECT_MODULES_KEY) continue;

                if (string.IsNullOrEmpty(m.projectVersion))
                    continue;

                if (!ModulesInstaller.IsModuleCurrentVersion(m))
                    return true;
            }

            return false;
        }

        private bool HasBatchUpdatableUpdatesInList(List<Module> list)
        {
            if (!cloudComplete || list == null || list.Count == 0)
                return false;

            for (int i = 0; i < list.Count; i++)
            {
                var m = list[i];
                if (m == null) continue;
                if (m.nameModule == SELECT_MODULES_KEY) continue;

                if (m.noLoad)
                    continue;

                if (string.IsNullOrEmpty(m.projectVersion))
                    continue;

                if (!ModulesInstaller.IsModuleCurrentVersion(m))
                    return true;
            }

            return false;
        }


        private bool WarningDeletePlugin()
        {
            if (!EditorUtility.DisplayDialog($"{Langs.correctDelete} {InfoYG.NAME_PLUGIN}", Langs.fullDeletePluginYG, Langs.deleteAll, Langs.cancel))
                return false;
            if (!EditorUtility.DisplayDialog($"{Langs.correctDelete} {InfoYG.NAME_PLUGIN}", Langs.protectionAccidentalDeletion, Langs.deleteAll, Langs.cancel))
                return false;
            return true;
        }

        private void DeletePlugin()
        {
            EditorUtility.DisplayDialog($"{Langs.correctDelete} {InfoYG.NAME_PLUGIN}", Langs.fullDeletePluginYGComplete, "Ok");

            EditorPrefs.DeleteKey(REMOVE_BEFORE_IMPORT_TOGGLE_KEY);
            Server.DeletePrefs();

            for (int i = 0; i < modules.Count; i++)
                DefineSymbols.RemoveDefine(modules[i].nameModule + "_yg");

            if (InfoYG.Inst().Basic.platform)
                DefineSymbols.RemoveDefine(PlatformSettings.currentPlatformFullName + "_yg");

            DefineSymbols.RemoveDefine(DefineSymbols.YG2_DEFINE);
            DefineSymbols.RemoveDefine(DefineSymbols.LANG_DEFINE);
            DefineSymbols.RemoveDefine(DefineSymbols.TMP_DEFINE);
            DefineSymbols.RemoveDefine(DefineSymbols.NJSON_DEFINE);
            DefineSymbols.RemoveDefine(DefineSymbols.NJSON_STORAGE_DEFINE);

            Close();

            string[] templateFolders = Directory.GetDirectories(InfoYG.PATCH_PC_PLATFORMS);
            for (int i = 0; i < templateFolders.Length; i++)
                ModulesInstaller.DeletePlatformWebGLTemplate(Path.GetFileName(templateFolders[i]));

            FileYG.DeleteDirectory(InfoYG.PATCH_PC_YG2);
        }

        private void DeleteAndUpdatePlugin(Module module)
        {
            if (module.download == string.Empty)
                return;

            string tempScrName = "UpdatePluginYGTemp";
            string tempScrText = FileYG.ReadAllText($"{InfoYG.PATCH_PC_YG2}/Scripts/Server/Editor/{tempScrName}.txt");
            tempScrText = tempScrText.Replace("DOWNLOAD_URL_KEY", module.download);
            tempScrText = tempScrText.Replace("PATH_YG2", InfoYG.CORE_FOLDER_YG2);

            string scenesFile = $"{InfoYG.PATCH_PC_EXAMPLE}/Resources/DemoSceneNames.txt";
            if (File.Exists(scenesFile))
            {
                string scenesText = FileYG.ReadAllText(scenesFile);
                tempScrText = tempScrText.Replace("EXAMPLE_SCENES = string.Empty", @$"EXAMPLE_SCENES = @""{scenesText}""");
            }

            FileYG.WriteAllText($"{Application.dataPath}/{tempScrName}.cs", tempScrText);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Close();
            CompilationPipeline.RequestScriptCompilation();
        }

        private bool HasAnyUpdatesAll()
        {
            if (!cloudComplete || modulesAll == null || modulesAll.Count == 0)
                return false;

            foreach (var m in modulesAll)
            {
                if (string.IsNullOrEmpty(m.projectVersion))
                    continue;

                if (!ModulesInstaller.IsModuleCurrentVersion(m))
                    return true;
            }

            return false;
        }
    }
}
