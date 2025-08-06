using UnityEditor;

namespace YG.EditorScr
{
    [CustomEditor(typeof(AdNotificationYG))]
    public class AdNotificationYGEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (WarningPostponeCall.Draw()
                && EditorUtils.IsMouseOverWindow(serializedObject.targetObject.name))
            {
                Repaint();
            }
        }
    }
}