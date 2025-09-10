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
            if (YG2.player.auth)
            {
#if RU_YG2
                YG2.Message("SDK Яндекс Игр предлагает войти в аккаунт только тем пользователям, которые ещё не вошли.");
#else
                YG2.Message("The Yandex Games SDK offers to log in to your account only to those users who have not logged in yet.");
#endif
            }
            else
            {
                YG2.Message("Open Auth Dialog");
            }

#if UNITY_EDITOR
            YG2.player.auth = true;
            Insides.YGInsides.LoggedIn();
#else
            OpenAuthDialog_js();
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
            if (data == InfoYG.NO_DATA || string.IsNullOrEmpty(data))
            {
                YG2.player.auth = false;
                YG2.player.name = "unauthorized";
                YG2.player.id = null;
                YG2.player.photo = null;
                Debug.LogError("Failed init player data");

                return;
            }

            jsonAuth = JsonUtility.FromJson<PlatformYG2.JsonAuth>(data);

            YG2.player.auth = jsonAuth.playerAuth.ToString() switch
            {
                "resolved" => true,
                "rejected" => false,
                _ => false
            };

            YG2.player.name = string.IsNullOrEmpty(YG2.player.name)
                ? InfoYG.ANONYMOUS
                : jsonAuth.playerName.ToString();

            YG2.player.photo = YG2.player.photo == InfoYG.NO_DATA
                ? null
                : jsonAuth.playerPhoto.ToString();

            YG2.player.id = jsonAuth.playerId.ToString();

            YG2.player.payingStatus = jsonAuth.payingStatus.ToString() switch
            {
                "paying" => YG2.PayingStatus.Paying,
                "partially_paying" => YG2.PayingStatus.PartiallyPaying,
                "not_paying" => YG2.PayingStatus.NotPaying,
                _ => YG2.PayingStatus.Unknown
            };
            
#if UNITY_EDITOR
            YG2.GetDataInvoke();
#endif
            YG2.Message($"Authorization: {YG2.player.auth}");
        }
    }
}
#endif