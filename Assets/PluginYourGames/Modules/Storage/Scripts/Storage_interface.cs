using YG.Insides;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        void SaveCloud(System.Action<bool> completed)
        {
#if PLATFORM_WEBGL
            // Existing integrations keep saving, but cannot confirm their cloud result yet.
            SaveCloud();
            UnityEngine.Debug.LogWarning("[PluginYG Storage] Cloud save callbacks are not supported on this platform yet.");
            completed(false);
#else
            // Native platforms use the same local fallback as the parameterless SaveCloud.
            completed(YG2.infoYG.Storage.saveLocal || YGInsides.SaveLocalWithResult());
#endif
        }

        void SaveCloud()
        {
            if (!YG2.infoYG.Storage.saveLocal)
            {
                YGInsides.SaveLocal();
            }
        }

        void LoadCloud()
        {
            if (!YG2.infoYG.Storage.saveLocal)
            {
                YGInsides.LoadLocal();
            }
        }
    }
}
