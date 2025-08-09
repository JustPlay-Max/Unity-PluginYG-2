namespace YG
{
    public static partial class YG2
    {
        public static EnvirData envir = new EnvirData();

        public class EnvirData
        {
            public string language = "ru";
            public string domain = "ru";
            public string deviceType = "desktop";
            public bool isDesktop = true;
            public bool isMobile;
            public bool isTablet;
            public bool isTV;
            public string appID;
            public string browserLang;
            public string payload;
            public string platform = "Win32";
            public string browser = "Other";

            public Device device
            {
                get
                {
                    switch (deviceType)
                    {
                        case "desktop":
                            return Device.Desktop;
                        case "mobile":
                            return Device.Mobile;
                        case "tablet":
                            return Device.Tablet;
                        case "tv":
                            return Device.TV;
                        default:
                            return Device.Desktop;
                    }
                }
            }
        }

        [InitYG_0]
        private static void InitEnvirData()
        {
#if !UNITY_EDITOR
            Message("Init Envir inGame");
            iPlatform.InitEnirData();
#else
            envir = new EnvirData();  // Reset static for ECS
            InitEnvirForEditor();
#endif
        }

        public static void GetEnvirData()
        {
#if !UNITY_EDITOR
            iPlatform.GetEnvirData();
#else
            InitEnvirForEditor();
#endif
        }

#if UNITY_EDITOR
        private static void InitEnvirForEditor()
        {
            envir.isDesktop = false;
            envir.isMobile = false;
            envir.isTablet = false;
            envir.isTV = false;

            switch (infoYG.Simulation.device)
            {
                case Device.Desktop:
                    envir.deviceType = "desktop";
                    envir.isDesktop = true;
                    break;
                case Device.Mobile:
                    envir.deviceType = "mobile";
                    envir.isMobile = true;
                    break;
                case Device.Tablet:
                    envir.deviceType = "tablet";
                    envir.isTablet = true;
                    break;
                case Device.TV:
                    envir.deviceType = "tv";
                    envir.isTV = true;
                    break;
            }

            if (infoYG.Simulation.language != "" &&
                infoYG.Simulation.language != null)
                envir.language = infoYG.Simulation.language;

            GetDataInvoke();
        }
#endif
    }
}
