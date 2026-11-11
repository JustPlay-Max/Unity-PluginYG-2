using System;
using UnityEngine;
using YG.Insides;

#if UNITY_EDITOR
using YG.Translations;
#endif

namespace YG
{
    public partial class InfoYG
    {
        public AuthorizationSettings Authorization;

        [Serializable]
        public partial class AuthorizationSettings
        {
            public enum PlayerPhotoSize : byte { small = 0, medium, large };
            public PlayerPhotoSize playerPhotoSize = PlayerPhotoSize.medium;

#if UNITY_EDITOR
            [HeaderYG(Langs.simulation, 5)]
            public bool authorized = true;
            public string playerName = "Player current";
            public string uniqueID = "000";
            public string playerPhoto = DEMO_IMAGE;

            [Tooltip(AuthorizationTranslations.PayingStatusTooltip)]
            public YG2.PayingStatus payingStatus;
#endif

            public string GetPlayerPhotoSize()
            {
                return playerPhotoSize switch
                {
                    PlayerPhotoSize.small => "small",
                    PlayerPhotoSize.medium => "medium",
                    PlayerPhotoSize.large => "large",
                    _ => null
                };
            }
        }
    }
}