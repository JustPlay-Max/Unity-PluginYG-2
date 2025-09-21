using System.Diagnostics;
using System.IO;
using System.Text;

namespace YG.EditorScr.BuildModify
{
    public class BuildLog
    {
        private const string BUILDLOGFILE_NAME = "BuildLogYG2.txt";
        private static string BUILDLOGFILE_PATH => $"{InfoYG.PATCH_PC_EDITOR}/{BUILDLOGFILE_NAME}";
        public static void WritingLog()
        {
            string[] buildLogHeaderLines = new string[]
            {
                "Build path: ",      // 0
                "Build number: ",    // 1
                "PluginYG version: " // 2
            };

            if (!File.Exists(BUILDLOGFILE_PATH))
            {
                string readLines = string.Join("\n", buildLogHeaderLines);

                File.WriteAllText(BUILDLOGFILE_PATH, readLines, Encoding.UTF8);
            }

            string[] buildLog = File.ReadAllLines(BUILDLOGFILE_PATH, Encoding.UTF8);

            // Write lines log:
            // Build patch
            buildLog[0] = $"{buildLogHeaderLines[0]}{ProcessBuild.BuildPath}";

            // Build number
            buildLog[1] = $"{buildLogHeaderLines[1]}{GetBuildNumber() + 1}";

            // PluginYG version
            buildLog[2] = $"{buildLogHeaderLines[2]}{InfoYG.VERSION_YG2}";

            File.WriteAllLines(BUILDLOGFILE_PATH, buildLog, Encoding.UTF8);
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

        private static string ReadProperty(string property)
        {
            if (File.Exists(BUILDLOGFILE_PATH))
            {
                string[] lines = File.ReadAllLines(BUILDLOGFILE_PATH);

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