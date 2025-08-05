using UnityEngine;

namespace YG.EditorScr
{
    public class FastButton
    {
        public static bool Standart(string label)
        {
            return GUILayout.Button(label, YGEditorStyles.button);
        }

        public static bool Stringy(string label)
        {
            int width = label.Length * 8 + 30;
            return GUILayout.Button(label, YGEditorStyles.button, GUILayout.Width(width));
        }

        public static bool Lang(string labelEN, string labelRU, bool stringy = true)
        {
            string label;
#if RU_YG2
            label = labelRU;
#else
            label = labelEN;
#endif
            if (stringy)
                return Stringy(label);
            else
                return Standart(label);
        }
    }
}
