#if UNITY_EDITOR
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace YG.EditorScr
{
    public static class TextStyles
    {
        public static Color colorHeader
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                    return new Color(1.0f, 0.5f, 0.0f);
                else
                    return new Color(0.85f, 0.45f, 0.0f);
            }
        }

        public static Color colorLabel
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                    return new Color(1.5f, 0.5f, 0.0f);
                else
                    return new Color(0.9f, 0.4f, 0.0f);
            }
        }

        public static Color colorGreen
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                    return new Color(0.3f, 1.0f, 0.2f);
                else
                    return new Color(0.1f, 0.6f, 0.3f);
            }
        }

        public static GUIStyle Header(Color color)
        {
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = color;
            style.hover.textColor = color;
            style.active.textColor = color;
            style.focused.textColor = color;
            style.onNormal.textColor = color;
            style.onHover.textColor = color;
            style.onActive.textColor = color;
            style.onFocused.textColor = color;
            return style;
        }

        public static GUIStyle Header()
        {
            return Header(colorHeader);
        }

        public static GUIStyle Orange()
        {
            return LabelStyleColor(colorLabel);
        }

        public static GUIStyle Green()
        {
            return LabelStyleColor(colorGreen);
        }

        public static GUIStyle Gray()
        {
            return LabelStyleColor(new Color(0.5f, 0.5f, 0.5f));
        }

        public static GUIStyle Red()
        {
            return LabelStyleColor(Color.red);
        }

        public static GUIStyle White()
        {
            if (EditorGUIUtility.isProSkin)
                return LabelStyleColor(Color.white);
            else
                return LabelStyleColor(Color.black);
        }

        public static GUIStyle LabelStyleColor(Color color)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);

            style.normal.textColor = color;
            style.hover.textColor = color;
            style.active.textColor = color;
            style.focused.textColor = color;
            style.onNormal.textColor = color;
            style.onHover.textColor = color;
            style.onActive.textColor = color;
            style.onFocused.textColor = color;

            return style;
        }

        public static string AddSpaces(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Базовое разделение (между CamelCase и цифрами)
            string result = Regex.Replace(input,
                @"(?<=[a-z])(?=[A-Z])|
                  (?<=[A-Z])(?=[A-Z][a-z])|
                  (?<=[a-zA-Z])(?=\d)|
                  (?<=\d)(?=[a-zA-Z])",
                " ",
                RegexOptions.IgnorePatternWhitespace);

            // Убираем лишние пробелы между "одна буква + цифра" (например, Y 8 → Y8)
            result = Regex.Replace(result, @"\b([A-Za-z]) (\d)\b", "$1$2");

            return result;
        }

        public static string RemoveLowercaseLetters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            StringBuilder result = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (char.IsUpper(c) || char.IsDigit(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }
}
#endif
