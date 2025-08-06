using UnityEngine;
using UnityEditor;

namespace YG
{
    public sealed class NestedYGAttribute : PropertyAttribute
    {
        public string[] propertyNames { get; }
        public bool drawLine { get; }
        public int offset { get; }
        public int offsetLine { get; }

        public NestedYGAttribute(params string[] propNames)
        {
            propertyNames = propNames;
            drawLine = true;
            offset = 20;
        }

        public NestedYGAttribute(bool drawLine, params string[] propNames)
        {
            propertyNames = propNames;
            this.drawLine = drawLine;
            offset = 20;
        }

        public NestedYGAttribute(int offset, params string[] propNames)
        {
            propertyNames = propNames;
            this.offset = offset;
            drawLine = offset != 0;
        }

        public NestedYGAttribute(bool drawLine, int offset, params string[] propNames)
        {
            propertyNames = propNames;
            this.drawLine = drawLine;
            this.offset = offset;
        }

        public NestedYGAttribute(int offsetLine, int offset, params string[] propNames)
        {
            propertyNames = propNames;
            drawLine = true;
            this.offsetLine = offsetLine;
            this.offset = offset;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(NestedYGAttribute))]
    public class NestedYGDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldDisplay(property))
            {
                NestedYGAttribute attr = (NestedYGAttribute)attribute;

                float offset = attr.offset;

                if (attr.propertyNames != null && attr.propertyNames.Length > 1)
                {
                    offset *= attr.propertyNames.Length;
                }

                Rect newPosition = new Rect(position.x + offset, position.y, position.width - offset, position.height);

                if (attr.drawLine)
                {
                    float angleSize = 10f;
                    if (!IsScriptableObject(property))
                        offset -= 15f;

                    offset += attr.offsetLine;

                    Handles.color = Color.grey;
                    Handles.DrawLine(new Vector3(position.x + offset, position.y), new Vector3(position.x + offset, position.y + angleSize));
                    Handles.DrawLine(new Vector3(position.x + offset, position.y + angleSize), new Vector3(position.x + offset + angleSize, position.y + angleSize));
                }

                EditorGUI.PropertyField(newPosition, property, label, includeChildren: true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldDisplay(property)
                ? EditorGUI.GetPropertyHeight(property, label, includeChildren: true)
                : 0;
        }

        private bool IsScriptableObject(SerializedProperty property)
        {
            return property.serializedObject.targetObject is ScriptableObject;
        }

        private bool ShouldDisplay(SerializedProperty property)
        {
            NestedYGAttribute attr = (NestedYGAttribute)attribute;

            if (attr.propertyNames == null || attr.propertyNames.Length == 0)
                return true;

            for (int i = 0; i < attr.propertyNames.Length; i++)
            {
                bool invertValue = false;
                string propName = attr.propertyNames[i];

                if (propName[0].ToString() == "!")
                {
                    invertValue = true;
                    propName = propName.Substring(1);
                }

                SerializedProperty dependentProp = FindPropertyRelative(property, propName);

                if (dependentProp == null)
                {
                    Debug.LogError($"[{nameof(NestedYGAttribute)}] {property.name};  Field '{propName}' not foud!");
                    return true;
                }

                bool boolValue = dependentProp.boolValue;
                if (invertValue)
                    boolValue = !boolValue;

                if (!boolValue)
                    return false;
            }

            return true;
        }

        private SerializedProperty FindPropertyRelative(SerializedProperty property, string propertyPath)
        {
            if (property == null)
                return null;

            string path = property.propertyPath.Replace(property.name, propertyPath);
            return property.serializedObject.FindProperty(path);
        }
    }
#endif
}
