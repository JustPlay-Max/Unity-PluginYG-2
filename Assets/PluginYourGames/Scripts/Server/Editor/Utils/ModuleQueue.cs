using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;           // SessionState
using UnityEditor.Callbacks; // DidReloadScripts
using UnityEngine;           // JsonUtility

namespace YG.EditorScr
{
    public static class ModuleQueue
    {
        private const string KEY = "ImportModuleListYG";

        public static event Action onModuleLoaded;

        [Serializable]
        public class ImportModuleInfo
        {
            public string name;

            public ImportModuleInfo(string name)
            {
                this.name = name;
            }
        }

        // JsonUtility не умеет сериализовать List<> в корне, нужен контейнер
        [Serializable]
        private class ImportModuleListWrapper
        {
            public List<ImportModuleInfo> items = new List<ImportModuleInfo>();
        }

        /// <summary>
        /// Вызывается Unity после каждой перезагрузки скриптов (после компиляции).
        /// Здесь выполняем проверку модулей и действия над ними.
        /// </summary>
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            // Вызов с задержкой позволяет дождаться, когда Unity полностью "проснётся" после релоада
            EditorApplication.delayCall += ProcessInstallModulesInTurn;
        }

        /// <summary>
        /// Импортируем первый модуль из списка ожидания
        /// После импорта произойдёт рекомпиляция и импорт следующего модуля из списка ожидания
        /// </summary>
        public static async void ProcessInstallModulesInTurn()
        {
            List<ImportModuleInfo> list = LoadList();
            int listCount = list.Count;

            if (list == null || listCount == 0) return;

            string moduleName = list[0].name;

            RemoveList();
            listCount--;

            bool isImport = await ModulesInstaller.ImportPackageAsync(moduleName);
            if (!isImport)
            {
                list = new List<ImportModuleInfo>();
                CleanList();
                Debug.LogError($"The {moduleName} module was not imported");
            }

            if (listCount > 0)
            {
                ProcessInstallModulesInTurn();
            }
            else
            {
                await Task.Delay(100);
                AssetDatabase.Refresh();
                onModuleLoaded?.Invoke();

                if (!VersionControlWindow.isOpenWindow)
                    VersionControlWindow.ShowWindow();
            }

            void RemoveList()
            {
                int idx = list.FindIndex(m => string.Equals(m.name, list[0].name, StringComparison.Ordinal));
                if (idx >= 0)
                {
                    list.RemoveAt(idx);
                    SaveList(list);
                }
            }
        }

        /// <summary>Добавить модуль в очередь обновлений (если его там ещё нет).</summary>
        public static void AddList(string moduleName, bool addDependencies = true)
        {
            List<ImportModuleInfo> list = LoadList();

            if (FindByName(list, moduleName) == null)
            {
                ImportModuleInfo module = new ImportModuleInfo(moduleName);

                if (addDependencies)
                    AddDependenciesRecursive(ModulesInstaller.GetModuleByName(moduleName));

                list.Add(module);
                SaveList(list);
            }
        }

        /// <summary>Прочитать текущий список модулей.</summary>
        public static List<ImportModuleInfo> LoadList()
        {
            string json = SessionState.GetString(KEY, string.Empty);
            if (string.IsNullOrEmpty(json))
                return new List<ImportModuleInfo>();

            try
            {
                var wrapper = JsonUtility.FromJson<ImportModuleListWrapper>(json);
                return wrapper?.items ?? new List<ImportModuleInfo>();
            }
            catch
            {
                return new List<ImportModuleInfo>();
            }
        }

        public static void CleanList() => SessionState.SetString(KEY, string.Empty);

        public static void QueueInstalModule(Module module)
        {
            if (module.nameModule != InfoYG.NAME_PLUGIN && !ModulesInstaller.PluginHasBeenUpdated())
            {
                AddList(InfoYG.NAME_PLUGIN, false);
                return;
            }

            AddList(module.nameModule, true);
            ProcessInstallModulesInTurn();
        }

        public static List<Module> GetModuleDependencies(Module module)
        {
            List<Module> dependencies = new List<Module>();

            if (string.IsNullOrEmpty(module.dependencies))
                return dependencies;

            string depsRaw = module.dependencies ?? string.Empty;

            // HashSet гарантирует уникальность зависимостей
            HashSet<string> hashDependencies = depsRaw
                .Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(d => d.Trim())
                .Where(d => !string.IsNullOrEmpty(d))
                .ToHashSet(StringComparer.Ordinal);

            foreach (string depStr in hashDependencies)
            {
                Module depModule = ModulesInstaller.GetModuleByName(depStr);

                if (!ModulesInstaller.IsModuleCurrentVersion(depModule))
                {
                    dependencies.Add(depModule);
                }
            }

            return dependencies;
        }

        public static void AddDependenciesRecursive(Module module)
        {
            foreach (var dep in GetModuleDependencies(module))
            {
                Module depModule = ModulesInstaller.GetModuleByName(dep.nameModule);

                if (depModule == null)
                    continue;

                AddList(depModule.nameModule);
            }
        }

        private static ImportModuleInfo FindByName(List<ImportModuleInfo> list, string moduleName)
        {
            return list.Find(m => string.Equals(m.name, moduleName, StringComparison.Ordinal));
        }

        private static void SaveList(List<ImportModuleInfo> list)
        {
            var wrapper = new ImportModuleListWrapper { items = list ?? new List<ImportModuleInfo>() };
            string json = JsonUtility.ToJson(wrapper);
            SessionState.SetString(KEY, json);
        }
    }
}
