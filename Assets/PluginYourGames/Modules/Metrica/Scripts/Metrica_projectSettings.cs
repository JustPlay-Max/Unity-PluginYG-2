#if UNITY_EDITOR
namespace YG.Insides
{
    public partial class ProjectSettings
    {
        public bool useYandexMetrica = true;
        public int metricaCounterID;

        [ApplySettings]
        public void Metrica_ApplySettings()
        {
            if (YG2.infoYG.platformToggles.useYandexMetrica)
                YG2.infoYG.Metrica.useYandexMetrica = useYandexMetrica;

            if (YG2.infoYG.platformToggles.metricaCounterID)
                YG2.infoYG.Metrica.metricaCounterID = metricaCounterID;
        }
    }

    public partial class PlatformToggles
    {
        public bool useYandexMetrica;
        public bool metricaCounterID;
    }
}
#endif