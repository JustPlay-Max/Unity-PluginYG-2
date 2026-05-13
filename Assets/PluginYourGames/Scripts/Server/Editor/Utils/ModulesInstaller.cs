using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using YG.Insides;

namespace YG.EditorScr
{
    public static class ModulesInstaller
    {
        public static event Action<bool> onDownloadProcess;

        public static bool ApprovalDownload()
        {
            string key = "updateWarningDialogYG2";

            if (!EditorPrefs.HasKey(key))
            {
                if (EditorUtility.DisplayDialog(Langs.importModule, Langs.updateWarningDialog, "Ok"))
                {
                    EditorPrefs.SetInt(key, 1);
                    return true;
                }
                return false;
            }
            return true;
        }

        public static void InstallModule(Module module)
        {
            if (!ApprovalDownload())
                return;

            if (string.IsNullOrEmpty(module.download))
            {
                Debug.LogError("URL is empty! (Import package)");
                return;
            }

            ModuleQueue.QueueInstalModule(module);
        }

        public static bool ApprovalDependencies(Module module) => ApprovalDependencies(new List<Module> { module });

        public static bool ApprovalDependencies(IEnumerable<Module> modules)
        {
            List<Module> dependencies = GetUniqueDependencies(modules);

            if (dependencies.Count == 0)
                return true;

            string dialogText = Langs.dependenciesDialog + "\n";

            foreach (Module dependency in dependencies)
                dialogText += "\n• " + dependency.nameModule;

            return EditorUtility.DisplayDialog(Langs.importModule, dialogText, "Ok", Langs.cancel);
        }

        private static List<Module> GetUniqueDependencies(IEnumerable<Module> modules)
        {
            List<Module> dependencies = new List<Module>();
            HashSet<string> moduleNames = new HashSet<string>();
            HashSet<string> dependencyNames = new HashSet<string>();

            if (modules == null)
                return dependencies;

            foreach (Module module in modules)
            {
                if (module != null && !string.IsNullOrEmpty(module.nameModule))
                    moduleNames.Add(module.nameModule);
            }

            foreach (Module module in modules)
            {
                foreach (Module dependency in ModuleQueue.GetModuleDependencies(module))
                {
                    if (dependency == null || string.IsNullOrEmpty(dependency.nameModule))
                        continue;

                    if (moduleNames.Contains(dependency.nameModule))
                        continue;

                    if (dependencyNames.Add(dependency.nameModule))
                        dependencies.Add(dependency);
                }
            }

            return dependencies;
        }

        public static async Task<bool> ImportPackageAsync(string module) => await ImportPackageAsync(GetModuleByName(module));
        public static async Task<bool> ImportPackageAsync(Module module)
        {
            bool removeBeforeImport = EditorPrefs.GetBool(VersionControlWindow.REMOVE_BEFORE_IMPORT_TOGGLE_KEY, true);

            try
            {
                string downloadPath = $"{InfoYG.PATCH_PC_EDITOR}/{module.nameModule}_tempYG.unitypackage";
                onDownloadProcess?.Invoke(true);

                bool isDownload = await DownloadPackageAsync(module.download, downloadPath);
                if (!isDownload)
                {
                    onDownloadProcess?.Invoke(false);
                    return false;
                }

                if (removeBeforeImport && module.nameModule != InfoYG.NAME_PLUGIN)
                {
                    string patchModules = $"{InfoYG.PATCH_PC_MODULES}/{module.nameModule}";
                    string patchPlatforms = $"{InfoYG.PATCH_PC_PLATFORMS}/{module.nameModule}";

                    if (Directory.Exists(patchModules))
                    {
                        DeleteAssetDirectory(patchModules);
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    }
                    else if (Directory.Exists(patchPlatforms) || Directory.Exists(patchPlatforms.Replace("Integration", "")))
                    {
                        if (module.nameModule != "YandexGames")
                            DeletePlatformWebGLTemplate(module.nameModule);

                        PlatformSettings.DeletePlatform();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    }
                }

                AssetDatabase.ImportPackage(downloadPath, false);
                File.Delete(downloadPath);
                onDownloadProcess?.Invoke(false);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error downloading or importing package '{module.nameModule}': {e.Message}");
                onDownloadProcess?.Invoke(false);
                return false;
            }
        }

        public static async Task<bool> DownloadPackageAsync(string packageUrl, string savePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(packageUrl);
                    if (!response.IsSuccessStatusCode)
                        return false;

                    byte[] packageBytes = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(savePath, packageBytes);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error downloading package: {e.Message}");
                return false;
            }
        }

        public static Module GetModuleByName(string name)
        {
            foreach (Module module in ModulesList.GetGeneratedList())
            {
                if (name == module.nameModule)
                    return module;
            }
            return null;
        }

        public static bool PluginHasBeenUpdated()
        {
            List<Module> modules = ModulesList.GetGeneratedList();

            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].nameModule == InfoYG.NAME_PLUGIN)
                {
                    if (IsModuleCurrentVersion(modules[i]))
                        return true;
                    else
                        break;
                }
            }
            return false;
        }

        public static void DeletePlatformWebGLTemplate(string folderNamePlatform)
        {
            string deleteDirectory = $"{InfoYG.PATCH_PC_WEBGLTEMPLATES}/{folderNamePlatform}";

            if (Directory.Exists(deleteDirectory))
            {
                DeleteAssetDirectory(deleteDirectory);
            }
            else
            {
                deleteDirectory += "Integration";
                if (Directory.Exists(deleteDirectory))
                    DeleteAssetDirectory(deleteDirectory);
            }

            if (FileYG.IsFolderEmpty(InfoYG.PATCH_PC_WEBGLTEMPLATES))
                DeleteAssetDirectory(InfoYG.PATCH_PC_WEBGLTEMPLATES);
        }

        private static void DeleteAssetDirectory(string pathDelete)
        {
            string assetPath = ToAssetPath(pathDelete);

            if (!string.IsNullOrEmpty(assetPath))
            {
                if (AssetDatabase.DeleteAsset(assetPath))
                    return;
            }

            FileYG.DeleteDirectory(pathDelete);
        }

        private static string ToAssetPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                return null;

            string dataPath = Path.GetFullPath(Application.dataPath).Replace("\\", "/");
            string normalizedPath = Path.GetFullPath(fullPath).Replace("\\", "/");

            if (!normalizedPath.StartsWith(dataPath + "/", StringComparison.OrdinalIgnoreCase))
                return null;

            return "Assets" + normalizedPath.Substring(dataPath.Length);
        }

        public static bool IsModuleCurrentVersion(Module module)
        {
            if (module == null)
                return true;

            if (!TryCompareVersions(module.projectVersion, module.lastVersion, out int comparison))
                return true;

            return comparison >= 0;
        }

        public static bool IsCriticalUpdate(Module module)
        {
            if (module == null)
                return false;

            if (string.IsNullOrEmpty(module.projectVersion))
                return false;

            if (IsModuleCurrentVersion(module))
                return false;

            // Legacy behavior: the latest version itself is marked critical.
            if (module.critical)
                return true;

            // New behavior: critical if update path crosses important versions.
            return IsImportantInstalledVersion(module.nameModule, module.projectVersion, module.lastVersion);
        }

        public static bool IsImportantInstalledVersion(string moduleName, string installedVersion, string availableVersion = null)
        {
            if (string.IsNullOrWhiteSpace(moduleName) || string.IsNullOrWhiteSpace(installedVersion))
                return false;

            ServerJson cloud = ServerInfo.saveInfo;
            if (cloud == null || cloud.importantVersions == null || cloud.importantVersions.Length == 0)
                return false;

            for (int i = 0; i < cloud.importantVersions.Length; i++)
            {
                if (!TryParseImportantVersionEntry(cloud.importantVersions[i], out string importantModule, out string importantVersion))
                    continue;

                if (!string.Equals(moduleName.Trim(), importantModule, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Mark as critical when user is below important version
                // and available update is on/after important version.
                if (IsVersionBefore(installedVersion, importantVersion) &&
                    IsVersionAtOrAfter(availableVersion, importantVersion))
                    return true;
            }

            return false;
        }

        private static bool TryCompareVersions(string version, string threshold, out int comparison)
        {
            comparison = 0;

            string normalizedVersion = NormalizeVersionToken(version);
            string normalizedThreshold = NormalizeVersionToken(threshold);

            if (string.IsNullOrEmpty(normalizedVersion) || string.IsNullOrEmpty(normalizedThreshold))
                return false;

            if (string.Equals(normalizedVersion, normalizedThreshold, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!decimal.TryParse(normalizedVersion, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal parsedVersion))
                return false;

            if (!decimal.TryParse(normalizedThreshold, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal parsedThreshold))
                return false;

            comparison = decimal.Compare(parsedVersion, parsedThreshold);
            return true;
        }

        private static bool TryParseImportantVersionEntry(string entry, out string moduleName, out string version)
        {
            moduleName = string.Empty;
            version = string.Empty;

            if (string.IsNullOrWhiteSpace(entry))
                return false;

            string value = entry.Trim();
            int separatorIndex = value.IndexOf('/');

            if (separatorIndex <= 0 || separatorIndex >= value.Length - 1)
                return false;

            moduleName = value.Substring(0, separatorIndex).Trim();
            version = value.Substring(separatorIndex + 1).Trim();
            return moduleName.Length > 0 && version.Length > 0;
        }

        private static bool AreVersionsEqual(string a, string b)
        {
            if (TryCompareVersions(a, b, out int comparison))
                return comparison == 0;

            string normalizedA = NormalizeVersionToken(a);
            string normalizedB = NormalizeVersionToken(b);
            return string.Equals(normalizedA, normalizedB, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsVersionBefore(string version, string threshold)
        {
            if (TryCompareVersions(version, threshold, out int comparison))
                return comparison < 0;

            return false;
        }

        private static bool IsVersionAtOrAfter(string version, string threshold)
        {
            if (TryCompareVersions(version, threshold, out int comparison))
                return comparison >= 0;

            // If latest version is unknown, fallback to conservative behavior:
            // do not show critical state for range-based matching.
            return false;
        }

        private static string NormalizeVersionToken(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string normalized = value.Replace(",", ".").Trim();

            if (string.Equals(normalized, "imported", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            if (normalized.Length > 0 && (normalized[0] == 'v' || normalized[0] == 'V'))
                normalized = normalized.Substring(1).TrimStart();

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            bool hasDigit = false;
            bool lastWasDot = false;

            for (int i = 0; i < normalized.Length; i++)
            {
                char ch = normalized[i];

                if (char.IsDigit(ch))
                {
                    builder.Append(ch);
                    hasDigit = true;
                    lastWasDot = false;
                    continue;
                }

                if (ch == '.' && hasDigit && !lastWasDot)
                {
                    builder.Append(ch);
                    lastWasDot = true;
                    continue;
                }

                if (hasDigit)
                    break;
            }

            string token = builder.ToString().Trim('.');
            return token;
        }

        public static bool ExistUpdates(List<Module> modules)
        {
            if (modules == null || modules.Count == 0)
                return false;

            for (int i = 0; i < modules.Count; i++)
            {
                var m = modules[i];
                if (m == null) continue;

                if (m.nameModule == VersionControlWindow.SELECT_MODULES_KEY)
                    continue;

                if (!string.IsNullOrEmpty(m.projectVersion) && !IsModuleCurrentVersion(m))
                    return true;
            }

            return false;
        }

    }
}
