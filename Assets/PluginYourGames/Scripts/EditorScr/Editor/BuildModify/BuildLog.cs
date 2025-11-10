using System.Diagnostics;
using System.IO;
using System.Text;

namespace YG.EditorScr.BuildModify
{
    public class BuildLog
    {
        private const string BUILD_LOG_FILE_NAME = "BuildLogYG2.txt";
        private static string BUILD_PATCH => $"{InfoYG.PATCH_PC_EDITOR}/{BUILD_LOG_FILE_NAME}";
        public static void WritingLog()
        {
            string[] buildLogHeaderLines = new string[]
            {
                "Build path: ",      // 0
                "Build number: ",    // 1
                "PluginYG version: " // 2
            };

            if (!File.Exists(BUILD_PATCH))
            {
                string readLines = string.Join("\n", buildLogHeaderLines);

                File.WriteAllText(BUILD_PATCH, readLines, Encoding.UTF8);
            }

            string[] buildLog = File.ReadAllLines(BUILD_PATCH, Encoding.UTF8);

            // Write lines log:
            // Build patch
            buildLog[0] = $"{buildLogHeaderLines[0]}{ProcessBuild.BuildPath}";

            // Build number
            buildLog[1] = $"{buildLogHeaderLines[1]}{GetBuildNumber() + 1}";

            // PluginYG version
            buildLog[2] = $"{buildLogHeaderLines[2]}{InfoYG.VERSION_YG2}";

            File.WriteAllLines(BUILD_PATCH, buildLog, Encoding.UTF8);
        }

        public static int GetBuildNumber()
        {
            string propertyString = ReadProperty("Build number");
            if (!string.IsNullOrEmpty(propertyString))
            {
                if (int.TryParse(propertyString, out int buildNumber))
                {
                    return buildNumber;
                }
            }

            return -1;
        }

        public static string ReadProperty(string property)
        {
            if (File.Exists(BUILD_PATCH))
            {
                string[] lines = FileYG.ReadAllLines(BUILD_PATCH);

                foreach (string line in lines)
                {
                    if (line.Contains(property))
                    {
                        int index = line.IndexOf(':') + 2;

                        if (index > line.Length)
                        {
                            UnityEngine.Debug.LogWarning($"[{property.ToUpper()}] index out of bounds");

                            return null;
                        }

                        return line.Substring(index);
                    }
                }
            }

            return null;
        }
    }
}