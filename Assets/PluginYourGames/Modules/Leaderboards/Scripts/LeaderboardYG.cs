using System;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using YG.Insides;
using YG.Utils.LB;
using System.Collections.Generic;

#if Localization_yg
using YG.Utils.Lang;
#endif

namespace YG
{
    public class LeaderboardYG : MonoBehaviour
    {
#if UNITY_EDITOR
        [Tooltip(Langs.t_nameLB)]
#endif
        public string nameLB;
#if UNITY_EDITOR
        [Tooltip(Langs.t_maxQuantityPlayers)]
#endif
        public int maxQuantityPlayers = 20;
#if UNITY_EDITOR
        [Range(1, 20), Tooltip(Langs.t_quantityTop)]
#endif
        public int quantityTop = 3;
#if UNITY_EDITOR
        [Range(1, 10), Tooltip(Langs.t_quantityAround)]
#endif
        public int quantityAround = 6;

        public enum UpdateLBMethod { Start, OnEnable, DoNotUpdate };
#if UNITY_EDITOR
        [Tooltip(Langs.t_updateLBMethod)]
#endif
        public UpdateLBMethod updateLBMethod = UpdateLBMethod.OnEnable;
#if UNITY_EDITOR
        [Tooltip(Langs.t_entriesText)]
#endif
        public Text entriesText;
#if UNITY_EDITOR
        [Tooltip(Langs.t_advanced)]
#endif
        public bool advanced;
#if UNITY_EDITOR
        [NestedYG(nameof(advanced)), Tooltip(Langs.t_rootSpawnPlayersData)]
#endif
        public Transform rootSpawnPlayersData;
#if UNITY_EDITOR
        [NestedYG(nameof(advanced)), Tooltip(Langs.t_playerDataPrefab)]
#endif
        public GameObject playerDataPrefab;

        public enum PlayerPhoto { NonePhoto, Small, Medium, Large };
#if UNITY_EDITOR
        [NestedYG(nameof(advanced)), Tooltip(Langs.t_playerPhoto)]
#endif
        public PlayerPhoto playerPhoto = PlayerPhoto.Small;
#if UNITY_EDITOR
        [NestedYG(nameof(advanced)), Tooltip(Langs.t_isHiddenPlayerPhoto)]
#endif
        public Sprite isHiddenPlayerPhoto;
#if UNITY_EDITOR
        [NestedYG(nameof(advanced)), Tooltip(Langs.t_timeTypeConvert)]
#endif
        public bool timeTypeConvert;
#if UNITY_EDITOR
        [NestedYG("timeTypeConvert"), Range(0, 3), Tooltip(Langs.t_decimalSize)]
#endif
        public int decimalSize = 1;

        public UnityEvent onUpdateData;

        private LBPlayerDataYG[] players = new LBPlayerDataYG[0];

        private void OnEnable()
        {
            YG2.onGetLeaderboard += OnUpdateLB;

            if (updateLBMethod == UpdateLBMethod.OnEnable)
                UpdateLB();
        }
        private void OnDisable()
        {
            YG2.onGetLeaderboard -= OnUpdateLB;
        }

        private void Start()
        {
            if (updateLBMethod == UpdateLBMethod.Start)
                UpdateLB();
        }

        private void OnUpdateLB(LBData lbData)
        {
            if (lbData.technoName != nameLB)
                return;

            string noData = string.Empty;
#if Localization_yg
            if (lbData.entries == InfoYG.NO_DATA)
            {
                noData = YG2.lang switch
                {
                    "ru" => "Нет данных",
                    "en" => "No data",
                    "tr" => "Veri yok",
                    _ => string.Empty,
                };
            }
#endif
            if (!advanced)
            {
#if Localization_yg
                lbData.entries = lbData.entries.Replace(InfoYG.ANONYMOUS, UtilsLang.IsHiddenTextTranslate());
#else
                lbData.entries = lbData.entries.Replace(InfoYG.ANONYMOUS, "---");
#endif
                entriesText.text = lbData.entries;
            }
            else
            {
                DestroyLBList();

                if (lbData.entries == InfoYG.NO_DATA)
                {
                    players = new LBPlayerDataYG[1];
                    GameObject playerObj = Instantiate(playerDataPrefab, rootSpawnPlayersData);

                    players[0] = playerObj.GetComponent<LBPlayerDataYG>();
                    players[0].data.name = noData;
                    players[0].data.photoUrl = null;
                    players[0].data.rank = null;
                    players[0].data.score = null;
                    players[0].data.inTop = false;
                    players[0].data.currentPlayer = false;
                    players[0].data.photoSprite = null;
                    players[0].UpdateEntries();
                }
                else
                {
#if UNITY_EDITOR
                    lbData = LBMethods.SortLB(lbData, maxQuantityPlayers, quantityTop, quantityAround);
#else
                    if (lbData.players.Length > maxQuantityPlayers)
                    {
                        int currentPlayer = -1;

                        for (int i = 0; i < lbData.players.Length; i++)
                        {
                            if (lbData.players[i].uniqueID == YG2.player.id)
                            {
                                currentPlayer = i;
                                break;
                            }
                        }
                        
                        if (currentPlayer >= maxQuantityPlayers)
                        {
                            List<LBPlayerData> topPlayers = new List<LBPlayerData>();

                            for (int i = 0; i < quantityTop; i++)
                                topPlayers.Add(lbData.players[i]);

                            int minusPlayers = lbData.players.Length - maxQuantityPlayers;
                            List<LBPlayerData> otherPlayers = new List<LBPlayerData>();

                            for (int i = quantityTop + minusPlayers; i < lbData.players.Length; i++)
                                otherPlayers.Add(lbData.players[i]);

                            List<LBPlayerData> finalPlayers = topPlayers;
                            finalPlayers.AddRange(otherPlayers);

                            lbData.players = finalPlayers.ToArray();
                        }
                        else
                        {
                            Array.Resize(ref lbData.players, maxQuantityPlayers);
                        }
                    }
#endif
                    SpawnPlayersList(lbData);
                }
            }
            onUpdateData?.Invoke();
        }

        private void DestroyLBList()
        {
            int childCount = rootSpawnPlayersData.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(rootSpawnPlayersData.GetChild(i).gameObject);
            }
        }

        private void SpawnPlayersList(LBData lb)
        {
            players = new LBPlayerDataYG[lb.players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                GameObject playerObj = Instantiate(playerDataPrefab, rootSpawnPlayersData);

                players[i] = playerObj.GetComponent<LBPlayerDataYG>();

                int rank = lb.players[i].rank;

                players[i].data.name = LBMethods.AnonymousName(lb.players[i].name);
                players[i].data.rank = rank.ToString();

                if (rank <= quantityTop)
                {
                    players[i].data.inTop = true;
                }
                else
                {
                    players[i].data.inTop = false;
                }

                if (lb.players[i].uniqueID == YG2.player.id)
                {
                    players[i].data.currentPlayer = true;
                }
                else
                {
                    players[i].data.currentPlayer = false;
                }

                if (timeTypeConvert)
                {
                    string timeScore = TimeTypeConvert(lb.players[i].score);
                    players[i].data.score = timeScore;
                }
                else
                {
                    players[i].data.score = lb.players[i].score.ToString();
                }

                if (playerPhoto != PlayerPhoto.NonePhoto)
                {
                    if (isHiddenPlayerPhoto && (lb.players[i].photo.Contains("/avatar/0/") || lb.players[i].photo == InfoYG.ANONYMOUS))
                    {
                        players[i].data.photoSprite = isHiddenPlayerPhoto;
                    }
                    else
                    {
                        players[i].data.photoUrl = lb.players[i].photo;
                    }
                }

                players[i].UpdateEntries();
            }
        }

        public void UpdateLB()
        {
            string photoSize = "nonePhoto";

            switch (playerPhoto)
            {
                case PlayerPhoto.Small:
                    photoSize = "small";
                    break;
                case PlayerPhoto.Medium:
                    photoSize = "medium";
                    break;
                case PlayerPhoto.Large:
                    photoSize = "large";
                    break;
            }

            YG2.GetLeaderboard(nameLB, quantityTop, quantityAround, photoSize);
        }

        public void SetLeaderboard(int score) => YG2.SetLeaderboard(nameLB, score);

        public void SetLBTimeConvert(float score) => YG2.SetLBTimeConvert(nameLB, score);

        public string TimeTypeConvert(int score)
        {
            return LBMethods.TimeTypeConvertStatic(score, decimalSize);
        }
    }
}

