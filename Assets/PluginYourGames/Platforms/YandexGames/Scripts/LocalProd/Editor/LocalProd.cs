#if UNITY_EDITOR && YandexGamesPlatform_yg
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using YG.EditorScr.BuildModify;
using YG.Insides;
using YG.Localization;
using Debug = UnityEngine.Debug;

namespace YG.EditorScr
{
    [InitializeOnLoad]
    public static class LocalProd
    {
        private const string PACKAGE_NAME = "@yandex-games/sdk-dev-proxy";
        private const string PROCESS_ID_KEY = "YG2.LocalProd.ProcessId";
        private const string PROCESS_START_TIME_KEY = "YG2.LocalProd.ProcessStartTime";
        private const string AUTO_RUN_KEY = "YG2.LocalProd.AutoRunAfterBuild";
        private const string SUPPRESS_BROWSER_SCRIPT = "Assets/PluginYourGames/Platforms/YandexGames/Scripts/LocalProd/Editor/SuppressProxyBrowser.cjs";
        private const string MENU_ROOT = "Tools/YG2/";
        private const string RUN_MENU_PATH = MENU_ROOT + LocalProdLocalization.runLocalBuildInYandexGames;
        private const string AUTO_RUN_MENU_PATH = MENU_ROOT + LocalProdLocalization.enableAutorunAfterBuild;
        private static int launchVersion;

        static LocalProd()
        {
            ModifyBuild.onModifyComplete += OnBuildComplete;
        }

        [MenuItem(RUN_MENU_PATH, false, 30)]
        public static void Run()
        {
            try
            {
                PlatformInfo settings = YG2.infoYG.platformInfo;
                string buildPath = BuildLog.ReadProperty("Build path");

                if (string.IsNullOrWhiteSpace(buildPath) || !Directory.Exists(buildPath))
                {
                    Debug.LogError($"{LocalProdLocalization.buildFolderNotFound}\n{buildPath}");
                    return;
                }

                buildPath = Path.GetFullPath(buildPath);
                string indexPath = Path.Combine(buildPath, "index.html");

                if (!File.Exists(indexPath))
                {
                    Debug.LogError($"{LocalProdLocalization.indexNotFound}\n{indexPath}");
                    return;
                }

                if (!long.TryParse(settings.localProdGameId, out long gameId) || gameId <= 0)
                {
                    Debug.LogError(LocalProdLocalization.invalidGameId);
                    return;
                }

                if (settings.localProdPort < 1 || settings.localProdPort > 65535)
                {
                    Debug.LogError(LocalProdLocalization.invalidPort);
                    return;
                }

                if (!IsNpxAvailable())
                {
                    Debug.LogError(LocalProdLocalization.nodeNotFound);
                    return;
                }

                StopPreviousServer();

                string command = BuildCommand(buildPath, gameId, settings);
                Debug.Log(string.Format(LocalProdLocalization.launchInfo,
                    gameId, buildPath, settings.localProdPort, settings.localProdUseCsp, command));

                StartServer(command);
                OpenProductionPageWhenReady(gameId, settings.localProdPort);
            }
            catch (Exception exception)
            {
                Debug.LogError($"{LocalProdLocalization.launchFailed}\n{exception}\n\n{LocalProdLocalization.nodeNotFound}");
            }
        }

        [MenuItem(AUTO_RUN_MENU_PATH, false, 31)]
        private static void ToggleAutoRun()
        {
            bool enabled = !IsAutoRunEnabled();
            PluginPrefs.SetInt(AUTO_RUN_KEY, enabled ? 1 : 0);
            Menu.SetChecked(AUTO_RUN_MENU_PATH, enabled);
        }

        [MenuItem(AUTO_RUN_MENU_PATH, true)]
        private static bool ValidateAutoRun()
        {
            Menu.SetChecked(AUTO_RUN_MENU_PATH, IsAutoRunEnabled());
            return true;
        }

        private static bool IsAutoRunEnabled()
        {
            int value = PluginPrefs.GetInt(AUTO_RUN_KEY, -1);
            if (value >= 0)
                return value != 0;

            if (!EditorPrefs.HasKey(AUTO_RUN_KEY))
                return false;

            bool enabled = EditorPrefs.GetBool(AUTO_RUN_KEY, false);
            PluginPrefs.SetInt(AUTO_RUN_KEY, enabled ? 1 : 0);
            EditorPrefs.DeleteKey(AUTO_RUN_KEY);
            return enabled;
        }

        private static void OnBuildComplete()
        {
            if (!UnityEngine.Application.isBatchMode && IsAutoRunEnabled())
                Run();
        }

        private static string BuildCommand(string buildPath, long gameId, PlatformInfo settings)
        {
            string path = buildPath.Replace("\"", "\\\"");
            string command = $"npx --yes {PACKAGE_NAME} -p \"{path}\" --port={settings.localProdPort} --app-id={gameId}";

            if (settings.localProdUseCsp)
                command += " --csp";

            return command;
        }

        private static void OpenProductionPageWhenReady(long gameId, int port)
        {
            int version = Interlocked.Increment(ref launchVersion);
            string localGameUrl = Uri.EscapeDataString($"https://localhost:{port}");
            string url = $"https://yandex.ru/games/app/{gameId}?draft=true&game_url={localGameUrl}";

            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int attempt = 0; attempt < 120 && version == Interlocked.CompareExchange(ref launchVersion, 0, 0); attempt++)
                {
                    try
                    {
                        using (TcpClient client = new TcpClient())
                        {
                            client.Connect("127.0.0.1", port);
                            if (version == Interlocked.CompareExchange(ref launchVersion, 0, 0))
                                OpenUrl(url);
                            return;
                        }
                    }
                    catch (SocketException)
                    {
                        // The proxy has not started listening yet.
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"{LocalProdLocalization.launchFailed}\n{exception}");
                        return;
                    }

                    Thread.Sleep(500);
                }
            });
        }

        private static void OpenUrl(string url)
        {
#if UNITY_EDITOR_WIN
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
#elif UNITY_EDITOR_OSX
            Process.Start("open", url);
#else
            Process.Start("xdg-open", url);
#endif
        }

        private static bool IsNpxAvailable()
        {
#if UNITY_EDITOR_WIN
            ProcessStartInfo checkInfo = new ProcessStartInfo("where.exe", "npx")
#else
            ProcessStartInfo checkInfo = new ProcessStartInfo("/usr/bin/env", "sh -lc \"command -v npx\"")
#endif
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = Process.Start(checkInfo))
            {
                if (process == null)
                    return false;

                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }

        private static void StartServer(string command)
        {
            string browserScript = Path.GetFullPath(SUPPRESS_BROWSER_SCRIPT).Replace('\\', '/');
            string nodeOptions = Environment.GetEnvironmentVariable("NODE_OPTIONS") ?? string.Empty;

#if UNITY_EDITOR_WIN
            string batchPath = Path.GetFullPath("Library/PluginYG/LocalProd.cmd");
            Directory.CreateDirectory(Path.GetDirectoryName(batchPath));
            File.WriteAllText(batchPath,
                $"@echo off\r\nset \"NODE_OPTIONS={nodeOptions} --require=\"{browserScript}\"\"\r\n{command}\r\n");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k \"{batchPath}\"",
                UseShellExecute = true,
                CreateNoWindow = false
            };
#else
            string escapedCommand = command.Replace("'", "'\\''");
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "/bin/sh",
                Arguments = $"-lc '{escapedCommand}'",
                UseShellExecute = false,
                CreateNoWindow = false
            };
            // sdk-dev-proxy 0.0.2 opens its own browser tab when --app-id is set.
            // Keep --app-id for the draft's CSP, but let Unity open the tested draft URL once.
            startInfo.EnvironmentVariables["NODE_OPTIONS"] = $"{nodeOptions} --require \"{browserScript}\"".Trim();
#endif

            Process process = Process.Start(startInfo);

            if (process == null)
                throw new InvalidOperationException(LocalProdLocalization.processNotStarted);

            SessionState.SetInt(PROCESS_ID_KEY, process.Id);
            SessionState.SetString(PROCESS_START_TIME_KEY, process.StartTime.ToUniversalTime().Ticks.ToString());
            process.Dispose();
        }

        private static void StopPreviousServer()
        {
            int processId = SessionState.GetInt(PROCESS_ID_KEY, 0);
            string savedStartTime = SessionState.GetString(PROCESS_START_TIME_KEY, string.Empty);
            ClearSavedProcess();

            if (processId <= 0 || !long.TryParse(savedStartTime, out long startTimeTicks))
                return;

            try
            {
                using (Process process = Process.GetProcessById(processId))
                {
                    if (process.StartTime.ToUniversalTime().Ticks != startTimeTicks)
                        return;

#if UNITY_EDITOR_WIN
                    ProcessStartInfo stopInfo = new ProcessStartInfo
                    {
                        FileName = "taskkill.exe",
                        Arguments = $"/PID {processId} /T /F",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (Process stopProcess = Process.Start(stopInfo))
                    {
                        if (stopProcess == null)
                            throw new InvalidOperationException(LocalProdLocalization.processNotStopped);

                        stopProcess.WaitForExit();

                        if (stopProcess.ExitCode != 0 && !process.HasExited)
                            throw new InvalidOperationException(LocalProdLocalization.processNotStopped);
                    }
#else
                    process.Kill();
                    process.WaitForExit();
#endif
                }
            }
            catch (ArgumentException)
            {
                // The previously started process has already exited.
            }
        }

        private static void ClearSavedProcess()
        {
            SessionState.EraseInt(PROCESS_ID_KEY);
            SessionState.EraseString(PROCESS_START_TIME_KEY);
        }

    }
}
#endif
