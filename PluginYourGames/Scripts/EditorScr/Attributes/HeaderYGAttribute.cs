using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using YG.EditorScr;
#endif

namespace YG
{
    public class HeaderYGAttribute : PropertyAttribute
    {
        public string header { get; private set; }
        public Color color { get; private set; }
        public int indent { get; private set; }

        public HeaderYGAttribute(string header)
        {
            this.header = header;
#if UNITY_EDITOR
            color = TextStyles.colorHeader;
#endif
            indent = 20;
        }

        public HeaderYGAttribute(string header, float r, float g, float b)
        {
            this.header = header;
            color = new Color(r, g, b);
            indent = 20;
        }

        public HeaderYGAttribute(string header, float r, float g, float b, float a)
        {
            this.header = header;
            color = new Color(r, g, b, a);
            indent = 20;
        }

        public HeaderYGAttribute(string header, string color)
        {
            this.header = header;
            this.color = ConvertStrinColor(color);
            indent = 20;
        }

        public HeaderYGAttribute(string header, int indent)
        {
            this.header = header;
            color = new Color(1.0f, 0.5f, 0.0f);
            this.indent = indent;
        }

        public HeaderYGAttribute(string header, string color, int indent)
        {
            this.header = header;
            this.color = ConvertStrinColor(color);
            this.indent = indent;
        }

        private Color ConvertStrinColor(string str)
        {
            if (str == "white")
                return Color.white;
            else if (str == "black")
                return Color.black;
            else if (str == "gray")
                return Color.gray;
            else if (str == "red")
                return Color.red;
            else if (str == "blue")
                return Color.blue;
            else if (str == "yellow")
                return Color.yellow;
            else if (str == "green")
                return Color.green;
            else if (str == "cyan")
                return Color.cyan;
            else
                return color;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HeaderYGAttribute))]
    public class YGHeaderDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            HeaderYGAttribute attr = (HeaderYGAttribute)attribute;

            GUIStyle headerStyle = TextStyles.Header();
            headerStyle.normal.textColor = attr.color;

            position.y += attr.indent / 2;
            EditorGUI.LabelField(position, attr.header, headerStyle);
        }

        public override float GetHeight()
        {
            HeaderYGAttribute attr = (HeaderYGAttribute)attribute;
            return base.GetHeight() + attr.indent;
        }
    }
#endif
}
