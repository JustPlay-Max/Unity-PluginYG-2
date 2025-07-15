using System;
using UnityEngine;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        public AuthorizationSettings Authorization;

        [Serializable]
        public partial class AuthorizationSettings
        {
            //#if UNITY_EDITOR
            //            [Tooltip(Langs.t_scopes), Space(5)]
            //#endif
            //            public bool scopes = true;
            public enum PlayerPhotoSize { small, medium, large };
            //#if UNITY_EDITOR
            //            [NestedYG(nameof(scopes)), Tooltip(Langs.t_playerPhotoSize)]
            //#endif
            public PlayerPhotoSize playerPhotoSize = PlayerPhotoSize.medium;

#if UNITY_EDITOR
            [HeaderYG(Langs.simulation, 5)]
            public bool authorized = true;
            public string playerName = "Player current";
            public string uniqueID = "000";
            public string playerPhoto = DEMO_IMAGE;
            [Tooltip(Langs.t_payingStatus)]
            public YG2.PayingStatus payingStatus;
#endif

            public string GetPlayerPhotoSize()
            {
                if (playerPhotoSize == PlayerPhotoSize.small)
                    return "small";
                else if (playerPhotoSize == PlayerPhotoSize.medium)
                    return "medium";
                else if (playerPhotoSize == PlayerPhotoSize.large)
                    return "large";

                return null;
            }
        }
    }
}