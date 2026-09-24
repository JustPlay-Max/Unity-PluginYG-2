#if UNITY_EDITOR
namespace YG.Insides
{
    public partial class ProjectSettings
    {
        public bool useBackendPayments;

        [ApplySettings]
        private void Payments_ApplySettings()
        {
            if (YG2.infoYG.platformToggles.useBackendPayments)
                YG2.infoYG.Payments.useBackend = useBackendPayments;
        }
    }

    public partial class PlatformToggles
    {
        public bool useBackendPayments;
    }
}
#endif