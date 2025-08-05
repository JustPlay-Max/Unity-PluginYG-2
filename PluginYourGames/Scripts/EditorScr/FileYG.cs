#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace YG.EditorScr
{
    public static class FileYG
    {
        public static void DeleteDirectory(string folderDelete)
        {
            if (!Directory.Exists(folderDelete))
            {
                Debug.LogError($"The directory was not found for deletion! Patch directory:\n{folderDelete}");
                return;
            }

            FileUtil.DeleteFileOrDirectory(folderDelete);
            FileUtil.DeleteFileOrDirectory(folderDelete + ".meta");
        }

        public static void Delete(string fileDelete)
        {
            if (!File.Exists(fileDelete))
            {
                Debug.LogError($"The directory was not found for deletion! Patch directory:\n{fileDelete}");
                return;
            }

            File.Delete(fileDelete);
            File.Delete(fileDelete + ".meta");
        }

        public static bool IsFolderEmpty(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return false;

            string[] files = Directory.GetFiles(folderPath);
            string[] directories = Directory.GetDirectories(folderPath);

            return files.Length == 0 && directories.Length == 0;
        }
    }
}
#endif