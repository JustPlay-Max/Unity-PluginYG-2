using System;
using System.Collections.Generic;
using UnityEngine;
using YG.Utils.OpenURL;

namespace YG
{
    public class AllGamesYG : MonoBehaviour
    {
        public Transform rootSpawnGames;
        public GameObject gameInfoPrefab;
        public GameInfo.ImageURL imageURLType;
        public int maxGameSpawn = 10;
#if Flags_yg
        public bool sortUsingFlags;
        [NestedYG(nameof(sortUsingFlags))]
        public string nameFlag = "SortListGamesYG";
        [NestedYG(nameof(sortUsingFlags))]
        public bool onlyGamesFromFlag;
#endif
        private void Start()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            DestroyList();
            SpawnList();
        }

        public void DestroyList()
        {
            int childCount = rootSpawnGames.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(rootSpawnGames.GetChild(i).gameObject);
            }
        }

        private void SpawnList()
        {
            int count = Math.Clamp(YG2.allGames.Length, 0, maxGameSpawn);

            void DefaultSpawnList()
            {
                for (int i = 0; i < count; i++)
                {
                    GameObject obj = Instantiate(gameInfoPrefab, rootSpawnGames);
                    obj.GetComponent<GameYG>().Setup(YG2.allGames[i], imageURLType);
                }
            }
#if !Flags_yg
            DefaultSpawnList();
#else
            if (sortUsingFlags)
            {
                string flag = YG2.GetFlag(nameFlag);

                if (flag == null)
                {
                    DefaultSpawnList();
                    return;
                }

                string[] gamesStr = flag.Split(new[] { ", ", ",", ",  ", " , ", " ", "  " }, StringSplitOptions.RemoveEmptyEntries);

                if (gamesStr.Length == 0)
                {
                    DefaultSpawnList();
                    return;
                }

                int[] gamesID = new int[gamesStr.Length];

                for (int i = 0; i < gamesStr.Length; i++)
                {
                    if (!int.TryParse(gamesStr[i], out gamesID[i]))
                        gamesID[i] = 0;
                }

                List<int> resGames = new List<int>();

                for (int i = 0; i < gamesID.Length; i++)
                {
                    for (int j = 0; j < YG2.allGames.Length; j++)
                    {
                        if (gamesID[i] == YG2.allGames[j].appID)
                        {
                            resGames.Add(gamesID[i]);
                            break;
                        }
                    }
                }

                if (!onlyGamesFromFlag)
                {
                    List<int> otherGames = new List<int>();
                    HashSet<int> existingGames = new HashSet<int>(resGames);

                    for (int a = 0; a < YG2.allGames.Length; a++)
                    {
                        if (!existingGames.Contains(YG2.allGames[a].appID))
                        {
                            otherGames.Add(YG2.allGames[a].appID);
                        }
                    }
                    resGames.AddRange(otherGames);
                }

                count = Math.Clamp(resGames.Count, 0, maxGameSpawn);

                for (int i = 0; i < count; i++)
                {
                    GameInfo game = YG2.GetGameByID(resGames[i]);
                    GameObject obj = Instantiate(gameInfoPrefab, rootSpawnGames);
                    obj.GetComponent<GameYG>().Setup(game, imageURLType);
                }
            }
            else
            {
                DefaultSpawnList();
            }
#endif
        }

        public void OnDeveloperURL() => YG2.OnDeveloperURL();
        public void OnGameURL(int id) => YG2.OnGameURL(id);
    }
}