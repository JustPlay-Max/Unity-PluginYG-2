using UnityEngine;
using UnityEngine.UI;
using YG.Utils.OpenURL;
#if TMP_YG2
using TMPro;
#endif

namespace YG
{
    public class GameYG : MonoBehaviour
    {
        public bool loadByAppID;
        [NestedYG(nameof(loadByAppID))]
        public int appID;
        [NestedYG(nameof(loadByAppID))]
        public bool deleteObjIfNull = true;
        [NestedYG(nameof(loadByAppID))]
        public GameInfo.ImageURL imageURLType;

        public ImageLoadYG imageLoad;
        public Text title;
#if TMP_YG2
        public TextMeshProUGUI titleTMP;
#endif
        private string url;

        private void Start()
        {
            if (loadByAppID)
            {
                GameInfo data = YG2.GetGameByID(appID);

                if (data != null)
                    Setup(data, imageURLType);
                else if (deleteObjIfNull)
                    Destroy(gameObject);
            }
        }

        public void Setup(GameInfo data, GameInfo.ImageURL imageURL)
        {
            appID = data.appID;
            url = data.url;

            if (title)
                title.text = data.title;
#if TMP_YG2
            if (titleTMP)
                titleTMP.text = data.title;
#endif
            if (imageLoad)
            {
                if (imageURL == GameInfo.ImageURL.Icon)
                    imageLoad.Load(data.iconURL);
                else if (imageURL == GameInfo.ImageURL.Cover)
                    imageLoad.Load(data.coverURL);
            }
        }

        public void OnGameURL()
        {
            YG2.OnURL(url);
        }
    }
}