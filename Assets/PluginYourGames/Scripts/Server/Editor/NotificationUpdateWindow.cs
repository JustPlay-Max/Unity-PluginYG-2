using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YG.Insides;

namespace YG.EditorScr
{
    public class NotificationUpdateWindow : EditorWindow
    {
        private const string NOTIFICATION_UPDATE_KEY = "NotificationUpdate_YG2";
        private static List<Module> modules = new List<Module>();

        private bool closing;
        private bool allowShowWindow;
        private Vector2 scroll;

        public static void OpenWindowIfExistUpdate()
        {
            modules = ModulesList.GetGeneratedList(ServerInfo.saveInfo);

            if (!ModulesInstaller.ExistUpdates(modules)) return;

            PluginPrefs.Load();
            if (PluginPrefs.GetInt(NOTIFICATION_UPDATE_KEY, 0) == 1) return;

            ShowWindow();
        }

#if RU_YG2
        [MenuItem("Tools/YG2/Сводка обновлений", false, 2)]
#else
        [MenuItem("Tools/YG2/Updates Summary", false, 2)]
#endif
        public static void ShowWindow()
        {
            NotificationUpdateWindow window = GetWindow<NotificationUpdateWindow>($"Updates Summary {InfoYG.NAME_PLUGIN}");
            window.position = new Rect(800, 150, 400, 400);
            window.minSize = new Vector2(400, 200);
            window.maxSize = new Vector2(500, 1000);
        }

        private void OnEnable()
        {
            modules = ModulesList.GetGeneratedList(ServerInfo.saveInfo);

            PluginPrefs.Load();
            allowShowWindow = PluginPrefs.GetInt(NOTIFICATION_UPDATE_KEY, 0) == 1;

            ServerInfo.onLoadServerInfo += UpdateInfo;
            ModuleQueue.onModuleLoaded += UpdateInfo;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            ServerInfo.onLoadServerInfo -= UpdateInfo;
            ModuleQueue.onModuleLoaded -= UpdateInfo;
        }

        private void UpdateInfo()
        {
            if (closing || !this) return;

            modules = ModulesList.GetGeneratedList(ServerInfo.saveInfo);

            if (!ModulesInstaller.ExistUpdates(modules))
            {
                closing = true;
                Unsubscribe();

                EditorApplication.delayCall += () =>
                {
                    if (this) Close();
                };
            }
        }

        private void OnGUI()
        {
            if (modules == null) return;

            var updatable = modules
                .Where(m => !string.IsNullOrEmpty(m.projectVersion))
                .Where(m => !ModulesInstaller.IsModuleCurrentVersion(m))
                .ToList();

            GUILayout.Space(8);

            if (updatable.Count == 0)
            {
                var ok = TextStyles.Header();
                ok.alignment = TextAnchor.MiddleCenter;
#if RU_YG2
                EditorGUILayout.LabelField("Все модули актуальны ✅", ok);
#else
                EditorGUILayout.LabelField("All modules are relevant ✅", ok);
#endif
                return;
            }

            var header = TextStyles.Header();
            header.alignment = TextAnchor.MiddleCenter;
            header.fontSize = 15;
            EditorGUILayout.LabelField(Langs.youHaveUpdates, header);
            GUILayout.Space(6);

            using (var scroll = new EditorGUILayout.ScrollViewScope(this.scroll))
            {
                this.scroll = scroll.scrollPosition;

                foreach (var m in updatable)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace(); // выравнивание всей строки по центру

                    // Имя модуля (левый край)
                    var nameStyle = TextStyles.White();
                    nameStyle.alignment = TextAnchor.MiddleLeft;
                    GUILayout.Label(TextStyles.AddSpaces(m.nameModule), nameStyle, GUILayout.Width(150));

                    // Текущая версия (белым)
                    string cur = SafeVer(m.projectVersion);
                    string last = SafeVer(m.lastVersion);

                    var curStyle = TextStyles.White();
                    curStyle.alignment = TextAnchor.MiddleCenter;
                    GUILayout.Label(cur, curStyle, GUILayout.Width(60));

                    // Стрелка (белым)
                    GUILayout.Label("→", curStyle, GUILayout.Width(20));

                    // Новая версия (зелёным)
                    var lastStyle = TextStyles.Green();
                    lastStyle.alignment = TextAnchor.MiddleCenter;
                    GUILayout.Label(last, lastStyle, GUILayout.Width(60));

                    // Critical (если есть) — красным
                    if (m.critical)
                    {
                        var critStyle = TextStyles.Red();
                        critStyle.alignment = TextAnchor.MiddleCenter;
                        GUILayout.Label(" critical!", critStyle, GUILayout.Width(80));
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4); // отступ между строками
                }

            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(Langs.updateAll, YGEditorStyles.button, GUILayout.Width(160), GUILayout.Height(26)))
            {
                if (ModulesInstaller.ApprovalDownload())
                {
                    foreach (var m in updatable)
                        ModuleQueue.AddList(m.nameModule);

                    ModuleQueue.ProcessInstallModulesInTurn();
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUI.BeginChangeCheck();
            allowShowWindow = EditorGUILayout.ToggleLeft(Langs.dontShowAnymore, allowShowWindow);
            if (EditorGUI.EndChangeCheck())
                PluginPrefs.SetInt(NOTIFICATION_UPDATE_KEY, allowShowWindow ? 1 : 0);

            if (EditorUtils.IsMouseOverWindow(this)) Repaint();

            string SafeVer(string v)
            {
                if (string.IsNullOrEmpty(v)) return "—";
                v = v.Replace(",", ".").Trim();
                if (double.TryParse(v, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var d))
                {
                    return (d % 1 == 0)
                        ? d.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)
                        : d.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                return v;
            }
        }
    }
}