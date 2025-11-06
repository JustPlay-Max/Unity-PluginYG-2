using System.Collections.Generic;
using System.IO;

namespace YG.EditorScr
{
    public static class ModulesList
    {
        public static List<Module> GetGeneratedList() => GetGeneratedList(ServerInfo.saveInfo);

        public static List<Module> GetGeneratedList(ServerJson cloud)
        {
            DefineSymbols.UpdateDefineSymbols();
            List<Module> modules = new List<Module>();

            // PluginYG2 module
            string pluginVersionPatch = InfoYG.PATCH_PC_YG2 + "/Version.txt";
            string pluginVersion = string.Empty;

            if (File.Exists(pluginVersionPatch))
            {
                pluginVersion = FileYG.ReadAllText(pluginVersionPatch);
                pluginVersion = pluginVersion.Replace("v", string.Empty);
            }

            Module pluginModule = new Module
            {
                nameModule = InfoYG.NAME_PLUGIN,
                projectVersion = pluginVersion
            };
            modules.Add(pluginModule);

            // Modules
            string[] modulesTextLines = null;

            if (File.Exists(InfoYG.FILE_MODULES_PC))
                modulesTextLines = FileYG.ReadAllLines(InfoYG.FILE_MODULES_PC);

            for (int i = 0; i < modulesTextLines.Length; i++)
            {
                if (modulesTextLines[i] == string.Empty)
                    continue;

                string name = modulesTextLines[i];
                string version = "imported";

                int spaceIndex = modulesTextLines[i].IndexOf(' ');
                if (spaceIndex > -1)
                {
                    name = modulesTextLines[i].Remove(spaceIndex);
                    version = modulesTextLines[i].Remove(0, spaceIndex + 2);
                }

                Module module = new Module
                {
                    nameModule = name,
                    projectVersion = version
                };
               
                modules.Add(module);
            }

            // Platforms
            string[] platfomFolders = Directory.GetDirectories(InfoYG.PATCH_PC_PLATFORMS);
            string[] platfomNames = new string[platfomFolders.Length];

            for (int i = 0; i < platfomFolders.Length; i++)
                platfomNames[i] = Path.GetFileName(platfomFolders[i]);

            for (int i = 0; i < platfomNames.Length; i++)
            {
                string version = "imported";
                string platfomVersionPathc = $"{InfoYG.PATCH_PC_PLATFORMS}/{platfomNames[i]}/Version.txt";

                if (File.Exists(platfomVersionPathc))
                {
                    version = FileYG.ReadAllText(platfomVersionPathc);
                    version = version.Replace("v", string.Empty);
                }

                Module module = new Module
                {
                    nameModule = platfomNames[i].Replace("Integration", ""),
                    projectVersion = version,
                    platform = true
                };
                modules.Add(module);
            }

            // Cloud
            if (cloud != null && cloud.modules.Length > 0)
            {
                for (int i = 0; i < cloud.modules.Length; i++)
                {
                    bool found = false;

                    for (int j = 0; j < modules.Count; j++)
                    {
                        if (cloud.modules[i].name == modules[j].nameModule)
                        {
                            Module module = new Module
                            {
                                nameModule = modules[j].nameModule,
                                projectVersion = modules[j].projectVersion,
                                lastVersion = cloud.modules[i].version,
                                download = cloud.modules[i].download,
                                doc = cloud.modules[i].doc,
                                critical = cloud.modules[i].critical,
                                noLoad = cloud.modules[i].noLoad,
                                platform = modules[j].platform,
                                dependencies = cloud.modules[i].dependencies
                            };
                            modules[j] = module;

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        Module module = new Module
                        {
                            nameModule = cloud.modules[i].name,
                            projectVersion = string.Empty,
                            lastVersion = cloud.modules[i].version,
                            download = cloud.modules[i].download,
                            doc = cloud.modules[i].doc,
                            critical = cloud.modules[i].critical,
                            noLoad = cloud.modules[i].noLoad,
                            platform = cloud.modules[i].platform,
                            dependencies = cloud.modules[i].dependencies
                        };
                        modules.Add(module);
                    }
                }
            }

            List<string> selectModulesList = PrefsList.GetList(VersionControlWindow.SELECT_MODULES_KEY);

            foreach (Module module in modules)
            {
                if (selectModulesList.Contains(module.nameModule))
                {
                    module.select = true;
                }
            }

            return modules;
        }
    }
}