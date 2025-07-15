#if YandexGamesPlatform_yg
using UnityEngine;
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern string InitPlayer_js();

        public void InitAuth()
        {
            string playerData = InitPlayer_js();
            YG2.sendMessage.SetAuth(playerData);
        }

        [DllImport("__Internal")]
        public static extern void RequestAuth_js();

        public void GetAuth()
        {
            RequestAuth_js();
        }

        [DllImport("__Internal")]
        private static extern void OpenAuthDialog_js();

        public void OpenAuthDialog()
        {
            if (!YG2.player.auth)
            {
                YG2.Message("Open Auth Dialog");
            }
            else
            {
#if RU_YG2
                YG2.Message("SDK Яндекс Игр предлагает войти в аккаунт только тем пользователям, которые ещё не вошли.");
#else
                YG2.Message("The Yandex Games SDK offers to log in to your account only to those users who have not logged in yet.");
#endif
            }
#if !UNITY_EDITOR
            OpenAuthDialog_js();
#else
            YG2.player.auth = true;
            Insides.YGInsides.LoggedIn();
#endif
        }

        public class JsonAuth
        {
            public string playerAuth;
            public string playerName;
            public string playerId;
            public string playerPhoto;
            public string payingStatus;
        }
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        PlatformYG2.JsonAuth jsonAuth = new PlatformYG2.JsonAuth();

        public void SetAuth(string data)
        {
            if (data == InfoYG.NO_DATA || data == string.Empty || data == null)
            {
                YG2.player.auth = false;
                YG2.player.name = "unauthorized";
                YG2.player.id = null;
                YG2.player.photo = null;
                Debug.LogError("Failed init player data");
                return;
            }

            jsonAuth = JsonUtility.FromJson<PlatformYG2.JsonAuth>(data);

            if (jsonAuth.playerAuth.ToString() == "resolved")
                YG2.player.auth = true;
            else if (jsonAuth.playerAuth.ToString() == "rejected")
                YG2.player.auth = false;

            if (!string.IsNullOrEmpty(YG2.player.name))
                YG2.player.name = jsonAuth.playerName.ToString();
            else
                YG2.player.name = InfoYG.ANONYMOUS;

            if (YG2.player.photo != InfoYG.NO_DATA)
                YG2.player.photo = jsonAuth.playerPhoto.ToString();
            else
                YG2.player.photo = null;

            YG2.player.id = jsonAuth.playerId.ToString();

            YG2.PayingStatus payingStatus;
            switch (jsonAuth.payingStatus.ToString())
            {
                case "paying":
                    payingStatus = YG2.PayingStatus.Paying;
                    break;
                case "partially_paying":
                    payingStatus = YG2.PayingStatus.PartiallyPaying;
                    break;
                case "not_paying":
                    payingStatus = YG2.PayingStatus.NotPaying;
                    break;
                default:
                    payingStatus = YG2.PayingStatus.Unknown;
                    break;
            }
            YG2.player.payingStatus = payingStatus;
#if !UNITY_EDITOR
            YG2.Message("auth = " + YG2.player.auth);
#endif
            YG2.GetDataInvoke();
        }
    }
}
#endif