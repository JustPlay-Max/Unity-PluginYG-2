using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace YG.EditorScr
{
    [InitializeOnLoad]
    public static class Server
    {
        public const string LOAD_COMPLETE_KEY = "PluginYG_LoadServerComplete";
        private const string URL_KEY = "PluginYG_URLCloudInfo";
        private const string STANDART_URL = "https://max-games.ru/public/pluginYG2/data.json";
        private static string testUrl = "";
        public static bool loadComplete
        {
            get { return SessionState.GetBool(LOAD_COMPLETE_KEY, false); }
        }

        private static int loadCount;

        static Server()
        {
            EditorApplication.delayCall += () =>
            {
                if (PluginPrefs.GetInt(InfoYG.FIRST_STARTUP_KEY) != 0 &&
                    SessionState.GetBool(LOAD_COMPLETE_KEY, false) == false)
                {
                    LoadServerInfo();
                }
            };
        }

        public static async void LoadServerInfo(bool core = false)
        {
            if (core == false)
            {
                loadCount = 0;
                SessionState.SetBool(LOAD_COMPLETE_KEY, false);
            }

            try
            {
                loadCount++;
                if (loadCount < 4)
                {
                    string fileContent = null;

                    if (testUrl == "")
                    {
                        fileContent = await ReadFileFromURL(PluginPrefs.GetString(URL_KEY, STANDART_URL));

                        if (fileContent == null)
                        {
                            PluginPrefs.SetString(URL_KEY, STANDART_URL);
                            fileContent = await ReadFileFromURL(STANDART_URL);
                        }
                        else
                        {
                            ServerJson cloud = JsonUtility.FromJson<ServerJson>(fileContent);

                            if (cloud.redirection != string.Empty && cloud.redirection != PluginPrefs.GetString(URL_KEY))
                            {
                                PluginPrefs.SetString(URL_KEY, cloud.redirection);
                                LoadServerInfo(true);
                                return;
                            }
                        }
                    }
                    else
                    {
                        fileContent = await ReadFileFromURL(PluginPrefs.GetString(URL_KEY, testUrl));
                        ServerJson cloud = JsonUtility.FromJson<ServerJson>(fileContent);
                    }

                    if (!string.IsNullOrEmpty(fileContent))
                    {
                        FileYG.WriteAllText(InfoYG.FILE_SERVER_INFO, fileContent);
                        ServerInfo.Read();
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
#if RU_YG2
                        Debug.LogError($"Информация для {InfoYG.NAME_PLUGIN} не была загружена из-за отсутствия Интернета или неверного URL-адреса.");
#else
                        Debug.LogError($"The information for the {InfoYG.NAME_PLUGIN} was not uploaded due to a lack of Internet or an incorrect URL.");
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading server info: {ex.Message}");
            }
            finally
            {
                await Task.Delay(100);
                SessionState.SetBool(LOAD_COMPLETE_KEY, true);
                ServerInfo.DoActionLoadServerInfo();

                NotificationUpdateWindow.OpenWindowIfExistUpdate();
            }
        }

        private static async Task<string> ReadFileFromURL(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    Debug.LogError($"Server info request failed: {ex.Message}");
                    return null;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Server info request error: {ex.Message}");
                    return null;
                }
            }
        }

        public static void DeletePrefs()
        {
            PluginPrefs.DeleteAll();
            SessionState.SetBool(LOAD_COMPLETE_KEY, false);
        }
    }
}