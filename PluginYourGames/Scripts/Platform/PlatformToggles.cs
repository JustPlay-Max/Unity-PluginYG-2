using UnityEngine;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        [HideInInspector]
        public PlatformToggles platformToggles = new PlatformToggles();
    }
}

namespace YG.Insides
{
    [System.Serializable]
    public partial class PlatformToggles
    {
#if UNITY_EDITOR
        public bool selectWebGLTemplate = true;
        public bool runInBackground = true;
        public bool enableExceptions = true;
        public bool compressionFormat = true;
        public bool decompressionFallback = true;
        public bool autoGraphicsAPI = true;
        public bool minimalCodeCompression = true;
        public bool dataCaching;
        public bool colorSpace;
        public bool archivingBuild;
        public bool syncInitSDK;
#endif
    }
}