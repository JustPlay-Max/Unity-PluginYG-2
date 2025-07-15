#if UNITY_EDITOR
namespace YG.Insides
{
    public partial class ProjectSettings
    {
        public bool saveLocal;
        public bool saveCloud = true;

        [ApplySettings]
        private void Storage_ApplySettings()
        {
            if (YG2.infoYG.platformToggles.saveLocal)
                YG2.infoYG.Storage.saveLocal = saveLocal;

            if (YG2.infoYG.platformToggles.saveCloud)
                YG2.infoYG.Storage.saveCloud = saveCloud;
        }
    }

    public partial class PlatformToggles
    {
        public bool saveLocal;
        public bool saveCloud;
    }
}
#endif