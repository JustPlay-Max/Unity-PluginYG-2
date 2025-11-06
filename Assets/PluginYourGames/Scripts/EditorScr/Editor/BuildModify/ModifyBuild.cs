using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEditor;
using UnityEngine;
using YG.Insides;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        private const string ERROR_COLOR = "#ff4f00";
        private const string WARNING_COLOR = "#fccf03";
        private static string BUILD_PATCH = string.Empty;
        private const string INDEX_FILE_NAME = "index.html";
        private const string STYLE_FILE_NAME = "style.css";
        private static InfoYG infoYG = null;
        private static string indexFile = string.Empty;
        private static string styleFile = string.Empty;
        private static string methodName = string.Empty;
        private enum CodeType { HeadNative, BodyNative, JS, Head, Body, Init0, Init1, Init2, Init, Start };
        public static event Action onModifyComplete = null;

        public static void ModifyIndex(string buildPath)
        {
            infoYG = YG2.infoYG;
            BUILD_PATCH = buildPath;
            List<string> errors = new List<string>();

#if PLATFORM_WEBGL
            string indexFilePath = Path.Combine(buildPath, INDEX_FILE_NAME);
            if (File.Exists(indexFilePath))
                indexFile = FileYG.ReadAllText(indexFilePath);
            else
                Debug.LogError($"{INDEX_FILE_NAME} file not found");

            string styleFilePath = Path.Combine(buildPath, STYLE_FILE_NAME);
            if (File.Exists(styleFilePath))
                styleFile = FileYG.ReadAllText(styleFilePath);

            Type type = typeof(ModifyBuild);
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (MethodInfo method in methods)
            {
                try
                {
                    if (method.Name != nameof(ModifyIndex) && method.GetParameters().Length == 0)
                    {
                        methodName = method.Name;
                        ModifyBuild scrCopy = new ModifyBuild();
                        method.Invoke(scrCopy, BindingFlags.Static | BindingFlags.Public, null, null, null);
                    }
                }
                catch (Exception ex)
                {
#if RU_YG2
                    Debug.LogError($"(Модуль <color={ERROR_COLOR}>{methodName}</color>) При модификации файлов билда возникла ошибка!\n{ex}");
#else
                    Debug.LogError($"(Module <color={ERROR_COLOR}>{methodName}</color>) Error occurred when modifying build files!\n{ex}");
#endif
                    errors.Add(methodName);
                }
            }
#endif
            int buildNumber = BuildLog.GetBuildNumber() + 1;

#if PLATFORM_WEBGL
            string logText = $"[{InfoYG.NAME_PLUGIN} v{InfoYG.VERSION_YG2}] [Build: {buildNumber}] [Platform: {YG2.platform}]";
#if YandexGamesPlatform_yg
            string initFunction = $"LogStyledMessage('{logText}');";
            AddIndexCode(initFunction, CodeType.JS);
#else
            string initFunction = $"<script>console.log('%c' + '{logText}', 'color: #FFDF73; background-color: #454545');</script>";
            AddIndexCode(initFunction, CodeType.BodyNative);
#endif
            FileYG.WriteAllText(indexFilePath, indexFile);

            if (File.Exists(styleFilePath))
                FileYG.WriteAllText(styleFilePath, styleFile);

#endif
            EditorApplication.delayCall += () =>
            {
#if RU_YG2
                string logBuildCompleteText = "Сборка завершена!";
#else
                string logBuildCompleteText = "Build complete!";
#endif
                Debug.Log($"<color=#00FF00>{InfoYG.NAME_PLUGIN} - {logBuildCompleteText}  Platform - {PlatformSettings.currentPlatformBaseName}.  Build number: {buildNumber}</color>");

                if (InfoYG.instance.Basic.platform == null)
                {
#if RU_YG2
                    Debug.LogWarning($"<color={WARNING_COLOR}>Обратите внимание!</color> <color={ERROR_COLOR}>В настройках {InfoYG.NAME_PLUGIN} не выбрана платформа. </color><color={WARNING_COLOR}>Проигнорируйте данное сообщение, если вы намеренно оставили поле пустым.</color>");
#else
                    Debug.LogWarning($"<color={WARNING_COLOR}>Please note!</color> <color={ERROR_COLOR}>In the settings {InfoYG.NAME_PLUGIN} no platform selected. </color><color={WARNING_COLOR}>Ignore this message if you intentionally left the field blank.</color>");
#endif
                }

                if (errors.Count > 0)
                {
                    string errorModulesText = string.Empty;

                    for (int i = 0; i < errors.Count; i++)
                    {
                        errorModulesText += errors[i];

                        if (i < errors.Count - 1)
                            errorModulesText += ", ";
                    }
#if RU_YG2
                    Debug.LogError($"<color={ERROR_COLOR}>Сборка завершена с ошибкой!</color> Необходимо устранить ошибки, чтобы модули: <color={ERROR_COLOR}>{errorModulesText}</color> - работали исправно.");
#else
                    Debug.LogError($"<color={ERROR_COLOR}>The build was completed with an error!</color> It is necessary to eliminate errors so that the <color={ERROR_COLOR}>{errorModulesText}</color> modules work properly.");
#endif
                }

                onModifyComplete?.Invoke();
            };
        }

        public static void ModifyIndex()
        {
            string buildPath = ProcessBuild.BuildPath;

            if (string.IsNullOrEmpty(buildPath))
            {
                Debug.LogError($"Path not found:\n{buildPath}");
            }
            else
            {
                ModifyIndex(buildPath);
#if UNITY_EDITOR_WIN
                Process.Start("explorer.exe", buildPath.Replace("/", "\\"));
#endif
            }
        }

        private static void AddIndexCode(string code, CodeType codeType)
        {
            string commentHelper = codeType switch
            {
                CodeType.HeadNative => "</head>",
                CodeType.BodyNative => "</body>",
                CodeType.Head => "<!-- Additional head modules -->",
                CodeType.Body => "<!-- Additional body modules -->",
                CodeType.Init0 => "// Additional init0 modules",
                CodeType.Init1 => "// Additional init1 modules",
                CodeType.Init2 => "// Additional init2 modules",
                CodeType.Init => "// Additional init modules",
                CodeType.Start => "// Additional start modules",
                _ => "// Additional script modules"
            };

            StringBuilder sb = new StringBuilder(indexFile);
            int insertIndex = sb.ToString().IndexOf(commentHelper);
            if (insertIndex >= 0)
            {
                if (codeType != CodeType.HeadNative && codeType != CodeType.BodyNative)
                    insertIndex += commentHelper.Length;

                sb.Insert(insertIndex, $"\n{code}\n");
                indexFile = sb.ToString();
            }
        }

        public static string FileTextCopy(string fileName)
        {
            string file = $"{InfoYG.PATCH_PC_MODULES}/{methodName}/Scripts/Editor/CopyCode/{fileName}";
            return FileYG.ReadAllText(file);
        }

        public static string ManualFileTextCopy(string filePath)
        {
            string file = $"{Application.dataPath}/{filePath}";
            return FileYG.ReadAllText(file);
        }

        private static void InitFunction(string methodName, CodeType codeType = CodeType.Init)
        {
            string initFunction = $"await {methodName}();\nLogStyledMessage('Init {ModifyBuild.methodName} ysdk');";
            AddIndexCode(initFunction, codeType);
        }

        private static string ConvertToRGBA(Color color)
        {
            int red = (int)(color.r * 255f);
            int green = (int)(color.g * 255f);
            int blue = (int)(color.b * 255f);
            float alpha = color.a;

            return $"rgba({red}, {green}, {blue}, {alpha.ToString().Replace(",", ".")})";
        }
    }
}