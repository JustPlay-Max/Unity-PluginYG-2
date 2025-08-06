using System;
using UnityEngine;
using YG.Utils.LB;
using System.Linq;
using YG.Insides;
#if UNITY_EDITOR
using YG.EditorScr;
#endif

namespace YG
{
    public partial class YG2
    {
        public static Action<LBData> onGetLeaderboard;

#if UNITY_EDITOR
        [InitYG]
        private static void ResetStaticLB()
        {
            onGetLeaderboard = null;
        }
#endif

        public static void SetLeaderboard(string nameLB, int score, string extraData)
        {
            if (infoYG.Leaderboards.enable && player.auth)
            {
                if (infoYG.Leaderboards.saveScoreAnonymousPlayers == false && player.name == InfoYG.ANONYMOUS)
                    return;

#if !UNITY_EDITOR
                Message("Set Leaderboard: " + score);
                iPlatform.SetLeaderboard(nameLB, score, extraData);
#else
                Message($"Set Leaderboard «{nameLB}»: {score}");
#endif
            }
        }

        public static void SetLeaderboard(string nameLB, int score)
        {
            SetLeaderboard(nameLB, score, null);
        }

        public static void SetLBTimeConvert(string nameLB, float secondsScore, string extraData)
        {
            if (infoYG.Leaderboards.enable && player.auth)
            {
                if (infoYG.Leaderboards.saveScoreAnonymousPlayers == false && player.name == InfoYG.ANONYMOUS)
                    return;

                if (secondsScore <= 0)
                {
                    Debug.LogError("The score must be greater than zero!");
                    return;
                }

                string rec = secondsScore.ToString("F3", System.Globalization.CultureInfo.InvariantCulture);
                string[] parts = rec.Split('.', ',');

                int result;
                if (parts.Length == 1)
                {
                    result = int.Parse(parts[0]);
                }
                else
                {
                    string sec = parts[0];
                    string milSec = parts[1];

                    milSec = milSec.Length > 3 ? milSec.Substring(0, 3) : milSec.PadRight(3, '0');
                    result = int.Parse(sec + milSec);
                }

                SetLeaderboard(nameLB, result, extraData);
            }
        }

        public static void SetLBTimeConvert(string nameLB, float secondsScore)
        {
            SetLBTimeConvert(nameLB, secondsScore, null);
        }

        public static void GetLeaderboard(string nameLB, int quantityTop, int quantityAround, string photoSizeLB)
        {
#if !UNITY_EDITOR
            iPlatform.GetLeaderboard(nameLB, quantityTop, quantityAround, photoSizeLB);
#else
            if (infoYG.Leaderboards.enable)
            {
                Message($"Get Leaderboard «{nameLB}»");

                LBData lb = null;
                LBData[] LBs = new LBData[infoYG.Leaderboards.listLBSim.Length];

                for (int i = 0; i < infoYG.Leaderboards.listLBSim.Length; i++)
                    LBMethods.CopyLBData(out LBs[i], infoYG.Leaderboards.listLBSim[i]);

                for (int i = 0; i < LBs.Length; i++)
                {
                    if (nameLB == LBs[i].technoName)
                    {
                        lb = LBs[i];

                        foreach (LBPlayerData playerData in lb.players)
                        {
                            if (playerData.name == InfoYG.ANONYMOUS)
                            {
                                playerData.photo = InfoYG.ANONYMOUS;
                                continue;
                            }

                            if (playerData.photo == InfoYG.DEMO_IMAGE)
                                playerData.photo = ServerInfo.saveInfo.playerImage;
                        }
                        break;
                    }
                }

                if (lb != null)
                {
                    lb.players = lb.players.OrderBy(item => item.score).ToArray();

                    if (!lb.isInvertSortOrder)
                        Array.Reverse(lb.players);

                    for (int i = 0; i < lb.players.Length; i++)
                        lb.players[i].rank = i + 1;

                    onGetLeaderboard?.Invoke(lb);
                }
                else
                {
                    onGetLeaderboard?.Invoke(YGInsides.NoLBData(nameLB));
                }
            }
            else
            {
                onGetLeaderboard?.Invoke(YGInsides.NoLBData(nameLB));
            }
#endif
        }

        public static void GetLeaderboard(string nameLB, int quantityTop, int quantityAround)
        {
            GetLeaderboard(nameLB, quantityTop, quantityAround, "small");
        }

        public static void GetLeaderboard(string nameLB)
        {
            GetLeaderboard(nameLB, 3, 3, "small");
        }
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public static LBData NoLBData(string nameLB)
        {
            LBData lb = new LBData()
            {
                technoName = nameLB,
                entries = InfoYG.NO_DATA,
                players = new LBPlayerData[1]
                {
                    new LBPlayerData()
                    {
                        name = InfoYG.NO_DATA,
                        photo = null
                    }
                }
            };
            return lb;
        }
    }
}
