using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using YG.EditorScr;
#endif

namespace YG
{
    public class LabelYGAttribute : PropertyAttribute
    {
        public string label { get; private set; }
        public string color { get; private set; }

        public LabelYGAttribute(string label)
        {
            this.label = label;
        }

        public LabelYGAttribute(string label, string color)
        {
            this.label = label;
            this.color = color;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LabelYGAttribute))]
    public class YGLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LabelYGAttribute attr = (LabelYGAttribute)attribute;

            GUIStyle labelStyle = TextStyles.Orange();

            if (attr.color == "white")
                labelStyle = TextStyles.White();
            else if (attr.color == "gray")
                labelStyle = TextStyles.Gray();
            else if (attr.color == "red")
                labelStyle = TextStyles.Red();
            else if (attr.color == "green")
                labelStyle = TextStyles.Green();

            EditorGUI.LabelField(position, attr.label, labelStyle);

            return;
        }
    }
#endif
}
