using System;
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
        private const string TEST_URL = "";
        private const int MAX_REDIRECTS = 3;

        private static string testUrl = string.Empty;

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
            if (!core)
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
                    bool useTestUrl = !string.IsNullOrWhiteSpace(TEST_URL);

                    if (useTestUrl)
                    {
                        fileContent = await ReadFileFromURL(TEST_URL);
                    }
                    else
                    {
                        string currentUrl = PluginPrefs.GetString(URL_KEY, STANDART_URL);

                        for (int redirectStep = 0; redirectStep <= MAX_REDIRECTS; redirectStep++)
                        {
                            fileContent = await ReadFileFromURL(currentUrl);

                            if (string.IsNullOrEmpty(fileContent))
                            {
                                if (currentUrl != STANDART_URL)
                                {
                                    currentUrl = STANDART_URL;
                                    PluginPrefs.SetString(URL_KEY, STANDART_URL);
                                    continue;
                                }

                                break;
                            }

                            if (!TryParseServerJson(fileContent, out ServerJson cloud))
                            {
                                if (currentUrl != STANDART_URL)
                                {
                                    currentUrl = STANDART_URL;
                                    PluginPrefs.SetString(URL_KEY, STANDART_URL);
                                    continue;
                                }

#if RU_YG2
                                Debug.LogError($"Сервер вернул невалидный JSON для данных плагина. URL: {currentUrl}");
#else
                                Debug.LogError($"Server returned invalid JSON for plugin data. URL: {currentUrl}");
#endif
                                fileContent = null;
                                break;
                            }

                            string redirectUrl = string.IsNullOrWhiteSpace(cloud.redirection) ? string.Empty : cloud.redirection.Trim();

                            if (!string.IsNullOrWhiteSpace(redirectUrl) && redirectUrl != currentUrl)
                            {
                                currentUrl = redirectUrl;
                                PluginPrefs.SetString(URL_KEY, redirectUrl);

                                if (redirectStep >= MAX_REDIRECTS)
                                    Debug.LogError($"Too many redirects while loading server info. Last URL: {redirectUrl}");

                                continue;
                            }

                            break;
                        }
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

        private static bool TryParseServerJson(string json, out ServerJson cloud)
        {
            cloud = null;

            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                cloud = JsonUtility.FromJson<ServerJson>(json);
                return cloud != null;
            }
            catch
            {
                return false;
            }
        }

        public static void DeletePrefs()
        {
            PluginPrefs.DeleteAll();
            SessionState.SetBool(LOAD_COMPLETE_KEY, false);
        }
    }
}
