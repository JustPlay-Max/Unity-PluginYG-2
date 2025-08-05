using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class CanvasScaleForMobile : MonoBehaviour
    {
        public CanvasScaler canvasScaler;
        public float scaleFactor = 1.4f;
        public Vector2 referenceResolution = new Vector2(800, 670);

#if EnvirData_yg
        private void Start()
        {
            if (YG2.envir.isMobile || YG2.envir.isTablet)
            {
                canvasScaler.scaleFactor = scaleFactor;
                canvasScaler.referenceResolution = referenceResolution;
            }
        }
#endif
    }
}
