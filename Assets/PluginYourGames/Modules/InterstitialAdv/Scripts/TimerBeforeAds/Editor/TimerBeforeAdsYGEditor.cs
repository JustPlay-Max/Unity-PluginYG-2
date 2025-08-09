using UnityEditor;

namespace YG.EditorScr
{
    [CustomEditor(typeof(TimerBeforeAdsYG))]
    public class TimerBeforeAdsYGEditor : Editor
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