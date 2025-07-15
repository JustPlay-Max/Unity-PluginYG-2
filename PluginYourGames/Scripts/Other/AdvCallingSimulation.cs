#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace YG.Insides
{
    public partial class AdvCallingSimulation : MonoBehaviour
    {
        private static AdvCallingSimulation CreateCallSimulation()
        {
            GameObject obj = new GameObject { name = "Calling Simulation" };
            DontDestroyOnLoad(obj);
            return obj.AddComponent(typeof(AdvCallingSimulation)) as AdvCallingSimulation;
        }

        private void DrawScreen(Color color)
        {
            GameObject obj = gameObject;
            Canvas canvas = obj.AddComponent<Canvas>();
            canvas.sortingOrder = 32767;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            obj.AddComponent<GraphicRaycaster>();
            obj.AddComponent<RawImage>().color = color;
        }
    }
}
#endif