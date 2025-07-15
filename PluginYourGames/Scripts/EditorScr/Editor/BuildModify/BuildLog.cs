using System.IO;

namespace YG.EditorScr.BuildModify
{
    public class BuildLog
    {
        private static string FILE_PATCH
        {
            get { return InfoYG.PATCH_PC_EDITOR + "/BuildLogYG2.txt"; }
        }
        private readonly static string SEPARAOTR = ": ";

        public static void WritingLog(string buildPath)
        {
            string[] linesBasic = new string[]
            {
                "Build path", // 0
                "Build number", // 1
                "PluginYG version" // 2
            };

            if (!File.Exists(FILE_PATCH))
            {
                string newLines = "";
                for (int i = 0; i < linesBasic.Length; i++)
                {
                    newLines += linesBasic[i] + SEPARAOTR + "\n";
                }

                File.WriteAllText(FILE_PATCH, newLines);
            }

            string[] lines = File.ReadAllLines(FILE_PATCH);


            /// Write lines log:
            // Build patch
            WritingLine(linesBasic[0], buildPath);

            // Build number
            string readBuildNumber = ReadingLine(linesBasic[1]);
            int oldBuildNumber = 0;

            if (readBuildNumber != null && readBuildNumber != "")
                oldBuildNumber = int.Parse(ReadingLine(linesBasic[1]));

            string newBuildNumber = (oldBuildNumber + 1).ToString();
            WritingLine(linesBasic[1], newBuildNumber);

            // PluginYG version
            WritingLine(linesBasic[2], InfoYG.VERSION_YG2);


            File.WriteAllLines(FILE_PATCH, lines);


            void WritingLine(string searchString, string write)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(searchString + SEPARAOTR))
                    {
                        string newLine = searchString + SEPARAOTR + write;
                        lines[i] = newLine;
                        break;
                    }
                }
            }

            string ReadingLine(string searchString)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(searchString))
                    {
                        return lines[i].Replace(searchString + SEPARAOTR, string.Empty);
                    }
                }
                return null;
            }
        }

        public static string ReadProperty(string property)
        {
            if (File.Exists(FILE_PATCH))
            {
                string[] lines = File.ReadAllLines(FILE_PATCH);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(property + SEPARAOTR))
                    {
                        return lines[i].Replace(property + SEPARAOTR, string.Empty);
                    }
                }
            }

            return null;
        }
    }
}