using UnityEngine;
using UnityEditor;
using YG.Insides;

namespace YG.EditorScr
{
    public class DocumentationEditor : Editor
    {
#if RU_YG2
        [MenuItem("Tools/YG2/Помощь/Документация", false, 3)]
#else
        [MenuItem("Tools/YG2/Help/Documentation", false, 3)]
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
        [MenuItem("Tools/YG2/Помощь/" + Langs.community, false, 4)]
#else
        [MenuItem("Tools/YG2/Help/" + Langs.community, false, 4)]
#endif
        public static void HelpMenuItem()
        {
            Application.OpenURL(ServerInfo.saveInfo.chat);
        }
        public static void HelpButton()
        {
            if (GUILayout.Button(Langs.community, YGEditorStyles.button))
            {
                Application.OpenURL(ServerInfo.saveInfo.chat);
            }
        }

#if RU_YG2
        [MenuItem("Tools/YG2/Помощь/Видео", false, 5)]
#else
        [MenuItem("Tools/YG2/Help/Video", false, 5)]
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