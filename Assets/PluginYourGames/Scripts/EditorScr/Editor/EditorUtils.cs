using UnityEditor;

namespace YG.EditorScr
{
    public static class EditorUtils
    {
        public static bool IsMouseOverWindow(EditorWindow window)
        {
            if (window == EditorWindow.mouseOverWindow)
                return true;
            else
                return false;
        }

        public static bool IsMouseOverWindow(string nameWindow, bool includeInspector = true)
        {
            EditorWindow windowUnderMouse = EditorWindow.mouseOverWindow;

            if (windowUnderMouse == null)
                return false;

            if (nameWindow == windowUnderMouse.titleContent.ToString())
            {
                return true;
            }
            else if (includeInspector && windowUnderMouse.titleContent.ToString() == "Inspector")
            {
                return true;
            }

            return false;
        }
    }
}