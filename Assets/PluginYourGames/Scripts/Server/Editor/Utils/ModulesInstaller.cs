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
                    dialogText += "\n• " + dependency.nameModule;

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
            float.TryParse(module.projectVersion, NumberStyles.Float, CultureInfo.InvariantCulture, out float projectVersion);
            float.TryParse(module.lastVersion, NumberStyles.Float, CultureInfo.InvariantCulture, out float lastVersion);

            if (lastVersion > projectVersion)
                return false;
            return true;
        }

        public static bool ExistUpdates(List<Module> modules)
        {
            for (int i = 1; i < modules.Count; i++)
            {
                if (!string.IsNullOrEmpty(modules[i].projectVersion) && !ModulesInstaller.IsModuleCurrentVersion(modules[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}