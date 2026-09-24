#if UNITY_EDITOR
using UnityEngine;

namespace YG.Insides
{
    public partial class AdvCallingSimulation : MonoBehaviour
    {
        private bool showScreen;
        private Color screenColor;

        private static AdvCallingSimulation CreateCallSimulation()
        {
            GameObject obj = new GameObject { name = "Calling Simulation" };
            DontDestroyOnLoad(obj);
            return obj.AddComponent(typeof(AdvCallingSimulation)) as AdvCallingSimulation;
        }

        private void DrawScreen(Color color)
        {
            screenColor = color;
            showScreen = true;
        }

        private void OnGUI()
        {
            if (!showScreen)
                return;

            int previousDepth = GUI.depth;
            Color previousColor = GUI.color;

            GUI.depth = int.MinValue;
            GUI.color = screenColor;
            GUI.DrawTexture(
                new Rect(0f, 0f, Screen.width, Screen.height),
                Texture2D.whiteTexture,
                ScaleMode.StretchToFill);

            GUI.color = previousColor;
            GUI.depth = previousDepth;
        }
    }
}
#endif