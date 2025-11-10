#if UNITY_EDITOR
using YG.EditorScr;
#endif

namespace YG
{
    public partial class YG2
    {
        public static PlayerData player = new PlayerData();
        public enum PayingStatus : byte { Paying = 0, PartiallyPaying, NotPaying, Unknown };

        public class PlayerData
        {
            public bool auth = false;
            public string name = "unauthorized";
            public string id = string.Empty;
            public string photo = string.Empty;
            public PayingStatus payingStatus = PayingStatus.Unknown;
        }

        [InitYG_2]
        private static void InitAuth()
        {
#if UNITY_EDITOR
            InitPlayerForEditor();
#else
            iPlatform.InitAuth();
#endif
        }

        public static void GetAuth()
        {
#if UNITY_EDITOR
            InitPlayerForEditor();
#else
            iPlatform.GetAuth();
#endif
        }

#if UNITY_EDITOR
        private static void InitPlayerForEditor()
        {
            player.auth = infoYG.Authorization.authorized;
            player.id = infoYG.Authorization.uniqueID;
            player.payingStatus = infoYG.Authorization.payingStatus;

            player.name = infoYG.Authorization.authorized
                ? infoYG.Authorization.playerName
                : "unauthorized";

            player.photo = infoYG.Authorization.playerPhoto == InfoYG.DEMO_IMAGE
                ? ServerInfo.saveInfo.playerImage
                : infoYG.Authorization.playerPhoto;

            GetDataInvoke();
        }
#endif

        public static void OpenAuthDialog()
        {
            iPlatform.OpenAuthDialog();
        }
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public static void LoggedIn()
        {
#if Storage_yg
            LoadProgress();
#else
            YG2.GetDataInvoke();
#endif
        }
    }

    public partial class YGSendMessage
    {
        public void LoggedIn() => YGInsides.LoggedIn();
    }
}