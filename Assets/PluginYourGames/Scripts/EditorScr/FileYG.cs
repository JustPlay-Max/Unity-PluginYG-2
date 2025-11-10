#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        /// <summary>
        /// Безопасно читает все строки из файла, даже если он заблокирован Unity.
        /// </summary>
        public static string[] ReadAllLines(string path, int retryCount = 5, int delayMs = 50)
        {
            if (!File.Exists(path))
                return Array.Empty<string>();

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(fs))
                    {
                        var list = new List<string>();
                        while (!reader.EndOfStream)
                            list.Add(reader.ReadLine());
                        return list.ToArray();
                    }
                }
                catch (IOException)
                {
                    if (i == retryCount - 1) throw;
                    Thread.Sleep(delayMs);
                }
            }

            return Array.Empty<string>();
        }

        /// <summary>
        /// Безопасно читает весь текст из файла.
        /// </summary>
        public static string ReadAllText(string path, int retryCount = 5, int delayMs = 50)
        {
            if (!File.Exists(path))
                return string.Empty;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(fs))
                        return reader.ReadToEnd();
                }
                catch (IOException)
                {
                    if (i == retryCount - 1) throw;
                    Thread.Sleep(delayMs);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Безопасно записывает текст в файл (создаёт директорию при необходимости).
        /// </summary>
        public static void WriteAllText(string path, string content, int retryCount = 5, int delayMs = 50)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                    using (var writer = new StreamWriter(fs))
                    {
                        writer.Write(content);
                    }
                    return;
                }
                catch (IOException)
                {
                    if (i == retryCount - 1) throw;
                    Thread.Sleep(delayMs);
                }
            }
        }
    }

    public sealed class ReloadScope : System.IDisposable
    {
        public ReloadScope() { EditorApplication.LockReloadAssemblies(); }
        public void Dispose() { EditorApplication.UnlockReloadAssemblies(); }
    }

    public sealed class AssetEditScope : System.IDisposable
    {
        public AssetEditScope() { AssetDatabase.StartAssetEditing(); }
        public void Dispose() { AssetDatabase.StopAssetEditing(); }
    }
}
#endif