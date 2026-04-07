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

            List<Module> dependencies = ModuleQueue.GetModuleDependencies(module);

            if (dependencies.Count > 0)
            {
                string dialogText = Langs.dependenciesDialog + "\n";

                foreach (Module dependency in dependencies)
                    dialogText += "\n- " + dependency.nameModule;

                if (!EditorUtility.DisplayDialog($"Dependencies found for {module.nameModule}", dialogText, "Ok", Langs.cancel))
                {
                    return;
                }
            }

            ModuleQueue.QueueInstalModule(module);
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
                if (!isDownload) return false;

                if (removeBeforeImport && module.nameModule != InfoYG.NAME_PLUGIN)
                {
                    string patchModules = $"{InfoYG.PATCH_PC_MODULES}/{module.nameModule}";
                    string patchPlatforms = $"{InfoYG.PATCH_PC_PLATFORMS}/{module.nameModule}";

                    if (Directory.Exists(patchModules))
                    {
                        FileYG.DeleteDirectory(patchModules);
                    }
                    else if (Directory.Exists(patchPlatforms) || Directory.Exists(patchPlatforms.Replace("Integration", "")))
                    {
                        if (module.nameModule != "YandexGames")
                            DeletePlatformWebGLTemplate(module.nameModule);

                        PlatformSettings.DeletePlatform();
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
                    float.TryParse(modules[i].projectVersion, NumberStyles.Float, CultureInfo.InvariantCulture, out float projectVersion);
                    float.TryParse(modules[i].lastVersion, NumberStyles.Float, CultureInfo.InvariantCulture, out float lastVersion);

                    if (projectVersion >= lastVersion)
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
                FileYG.DeleteDirectory(deleteDirectory);
            }
            else
            {
                deleteDirectory += "Integration";
                if (Directory.Exists(deleteDirectory))
                    FileYG.DeleteDirectory(deleteDirectory);
            }

            if (FileYG.IsFolderEmpty(InfoYG.PATCH_PC_WEBGLTEMPLATES))
                Directory.Delete(InfoYG.PATCH_PC_WEBGLTEMPLATES);
        }

        public static bool IsModuleCurrentVersion(Module module)
        {
            if (module == null)
                return true;

            if (!TryParseVersion(module.projectVersion, out float projectVersion))
                return true;

            if (!TryParseVersion(module.lastVersion, out float lastVersion))
                return true;

            return lastVersion <= projectVersion;
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

        private static bool TryParseVersion(string v, out float value)
        {
            value = 0f;

            if (string.IsNullOrWhiteSpace(v))
                return false;

            v = v.Replace("v", string.Empty).Replace(",", ".").Trim();

            if (string.Equals(v, "imported", StringComparison.OrdinalIgnoreCase))
                return false;

            return float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
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
            if (TryParseVersion(a, out float valueA) && TryParseVersion(b, out float valueB))
                return Mathf.Abs(valueA - valueB) < 0.0001f;

            string normalizedA = NormalizeVersionToken(a);
            string normalizedB = NormalizeVersionToken(b);
            return string.Equals(normalizedA, normalizedB, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsVersionBefore(string version, string threshold)
        {
            if (TryParseVersion(version, out float parsedVersion) && TryParseVersion(threshold, out float parsedThreshold))
                return parsedVersion < parsedThreshold;

            return false;
        }

        private static bool IsVersionAtOrAfter(string version, string threshold)
        {
            if (TryParseVersion(version, out float parsedVersion) && TryParseVersion(threshold, out float parsedThreshold))
                return parsedVersion >= parsedThreshold;

            // If latest version is unknown, fallback to conservative behavior:
            // do not show critical state for range-based matching.
            return false;
        }

        private static string NormalizeVersionToken(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value
                .Replace("v", string.Empty)
                .Replace("V", string.Empty)
                .Replace(",", ".")
                .Trim();
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
