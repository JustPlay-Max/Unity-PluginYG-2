using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if Localization_yg
using YG.Utils.Lang;
#endif

namespace YG.Utils.LB
{
    [Serializable]
    public class LBData
    {
        public string technoName;
        public string entries;
        public bool isDefault;
        public bool isInvertSortOrder;
        public int decimalOffset;
        public string type;
        public LBPlayerData[] players;
        public LBCurrentPlayerData currentPlayer;
    }

    [Serializable]
    public class LBPlayerData
    {
        public int rank;
        public string name;
        public int score;
        public string photo;
        public string uniqueID;
        public string extraData;
    }

    [Serializable]
    public class LBCurrentPlayerData
    {
        public int rank;
        public int score;
        public string extraData;
    }

    public static class LBMethods
    {
        public static string TimeTypeConvertStatic(int score, int decimalSize)
        {
            string format;
            switch (decimalSize)
            {
                case 1:
                    format = "mm':'ss'.'f";
                    break;
                case 2:
                    format = "mm':'ss'.'ff";
                    break;
                case 3:
                    format = "mm':'ss'.'fff";
                    break;
                default:
                    format = "mm':'ss";
                    break;
            }

            string formattedTime = TimeSpan.FromMilliseconds(score).ToString(format);
            return formattedTime;
        }

        public static string TimeTypeConvertStatic(int score)
        {
            return TimeTypeConvertStatic(score, 0);
        }

        public static string AnonymousName(string origName)
        {
            if (origName != "anonymous")
                return origName;
            else
#if Localization_yg
                return UtilsLang.IsHiddenTextTranslate();
#else
                return "---";
#endif
        }

        public static void CopyLBData(out LBData copy, LBData original)
        {
            if (original == null)
            {
                copy = null;
                Debug.LogError("Original leaderboard data null!");
                return;
            }

            copy = new LBData()
            {
                type = original.type,
                technoName = original.technoName,
                entries = original.entries,
                isDefault = original.isDefault,
                isInvertSortOrder = original.isInvertSortOrder,
                decimalOffset = original.decimalOffset,
                currentPlayer = new LBCurrentPlayerData
                {
                    rank = original.currentPlayer.rank,
                    score = original.currentPlayer.score,
                    extraData = original.currentPlayer.extraData,
                },
                players = new LBPlayerData[original.players.Length]
            };

            for (int i = 0; i < copy.players.Length; i++)
            {
                copy.players[i] = new LBPlayerData
                {
                    rank = original.players[i].rank,
                    name = original.players[i].name,
                    score = original.players[i].score,
                    photo = original.players[i].photo,
                    uniqueID = original.players[i].uniqueID,
                    extraData = original.players[i].extraData
                };
            }
        }

        public static LBData SortLB(LBData lbData, int maxQuantityPlayers, int quantityTop, int quantityAround)
        {
            CopyLBData(out LBData lb, lbData);

            LBPlayerData thisPlayer = null;

            for (int i = 0; i < lb.players.Length; i++)
            {
                if (lb.players[i].uniqueID == YG2.player.id)
                    thisPlayer = lb.players[i];
            }

            if (thisPlayer != null)
            {
                List<LBPlayerData> top = new List<LBPlayerData>();

                int topMaxCount = quantityTop;
                topMaxCount = Mathf.Clamp(topMaxCount, 0, lb.players.Length);

                for (int i = 0; i < topMaxCount; i++)
                    top.Add(lb.players[i]);

                List<LBPlayerData> around = new List<LBPlayerData>();
                thisPlayer = null;
                int tPlayerIndex = 0;

                if (top.Count == quantityTop)
                {
                    for (int i = quantityTop; i < lb.players.Length; i++)
                    {
                        around.Add(lb.players[i]);

                        if (lb.players[i].uniqueID == YG2.player.id)
                        {
                            thisPlayer = lb.players[i];
                            tPlayerIndex = around.Count - 1;
                        }
                    }
                }

                if (around.Count > 0 && thisPlayer != null)
                {
                    List<LBPlayerData> beforePlayers = new List<LBPlayerData>();
                    List<LBPlayerData> afterPlayers = new List<LBPlayerData>();

                    for (int i = 0; i < tPlayerIndex; i++)
                        beforePlayers.Add(around[i]);

                    for (int i = tPlayerIndex + 1; i < around.Count; i++)
                        afterPlayers.Add(around[i]);

                    int beforeCountResult = quantityAround;
                    int afterCountResult = quantityAround;

                    if (beforePlayers.Count < quantityAround || afterPlayers.Count < quantityAround)
                    {
                        if (beforePlayers.Count < afterPlayers.Count)
                        {
                            beforeCountResult = beforePlayers.Count;

                            afterCountResult = around.Count - 1 - beforeCountResult;
                            afterCountResult = Mathf.Clamp(afterCountResult, 0, quantityAround * 2 - beforeCountResult);
                        }
                        else
                        {
                            afterCountResult = afterPlayers.Count;

                            beforeCountResult = around.Count - 1 - afterCountResult;
                            beforeCountResult = Mathf.Clamp(beforeCountResult, 0, quantityAround * 2 - afterCountResult);
                        }
                    }

                    int beforePlayersCount = beforePlayers.Count;
                    beforePlayers = new List<LBPlayerData>();

                    for (int i = beforePlayersCount - beforeCountResult; i < beforePlayersCount; i++)
                        beforePlayers.Add(around[i]);

                    afterPlayers = new List<LBPlayerData>();
                    for (int i = tPlayerIndex + 1; i < tPlayerIndex + afterCountResult + 1; i++)
                        afterPlayers.Add(around[i]);

                    around = beforePlayers;
                    around.Add(thisPlayer);
                    around.AddRange(afterPlayers);
                }

                top.AddRange(around);

                if (top.Count > maxQuantityPlayers)
                {
                    List<LBPlayerData> trimmedList = top.GetRange(0, maxQuantityPlayers);
                    lb.players = trimmedList.ToArray();
                }
                else
                {
                    lb.players = top.ToArray();
                }
            }
            else
            {
                if (lb.players.Length > maxQuantityPlayers)
                {
                    List<LBPlayerData> trimmedList = lb.players.ToList().GetRange(0, maxQuantityPlayers);
                    lb.players = trimmedList.ToArray();
                }
            }

            return lb;
        }
    }
}