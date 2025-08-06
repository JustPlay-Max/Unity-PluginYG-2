using UnityEngine;
using UnityEditor;
using YG.Insides;

namespace YG.EditorScr
{
    public class DocumentationEditor : Editor
    {
#if RU_YG2
        [MenuItem("Tools/YG2/Помощь/Документация")]
#else
        [MenuItem("Tools/YG2/Help/Documentation")]
#endif
        public static void DocMenuItem()
        {
            Application.OpenURL(ServerInfo.saveInfo.documentation);
        }
        public static void DocButton()
        {
            if (GUILayout.Button(Langs.documentation, YGEditorStyles.button))
            {
                Application.OpenURL(ServerInfo.saveInfo.documentation);
            }
        }

#if RU_YG2
        [MenuItem("Tools/YG2/Помощь/Чат")]
#else
        [MenuItem("Tools/YG2/Help/Chat")]
#endif
        public static void ChatMenuItem()
        {
            Application.OpenURL(ServerInfo.saveInfo.chat);
        }
        public static void ChatButton()
        {
            if (GUILayout.Button(Langs.helpChat, YGEditorStyles.button))
            {
                Application.OpenURL(ServerInfo.saveInfo.chat);
            }
        }

#if RU_YG2
        [MenuItem("Tools/YG2/Помощь/Видео")]
#else
        [MenuItem("Tools/YG2/Help/Video")]
#endif
        public static void VideoMenuItem()
        {
            Application.OpenURL(ServerInfo.saveInfo.video);
        }
        public static void VideoButton()
        {
            if (GUILayout.Button(Langs.video, YGEditorStyles.button))
            {
                Application.OpenURL(ServerInfo.saveInfo.video);
            }
        }
    }
}