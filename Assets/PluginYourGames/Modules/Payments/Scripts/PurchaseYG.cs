using System;
using UnityEngine;
using UnityEngine.UI;
using YG.Utils.Pay;
#if UNITY_EDITOR
using YG.EditorScr;
using YG.Insides;
#endif
#if TMP_YG2
using TMPro;
#endif

namespace YG
{
    public class PurchaseYG : MonoBehaviour
    {
        public string id;
#if UNITY_EDITOR
        [Tooltip(Langs.t_purchaseImageLoad)]
#endif
        public ImageLoadYG purchaseImageLoad;
#if UNITY_EDITOR
        [Tooltip(Langs.t_currencyImageLoad)]
#endif
        public ImageLoadYG currencyImageLoad;
#if UNITY_EDITOR
        [Tooltip(Langs.t_showCurrencyCode)]
#endif
        public bool showCurrencyCode;

        [Serializable]
        public struct TextLegasy
        {
            public Text title, description, priceValue;
        }
        public TextLegasy textLegasy;

#if TMP_YG2
        [Serializable]
        public struct TextMP
        {
            public TextMeshProUGUI title, description, priceValue;
        }
        public TextMP textMP;
#endif

        private void Start() => UpdateEntries(YG2.PurchaseByID(id));

        public void UpdateEntries(Purchase data)
        {
            if (data == null)
            {
                Debug.LogError($"No product with ID found: {id}");
                return;
            }

            if (textLegasy.title) textLegasy.title.text = data.title;
            if (textLegasy.description) textLegasy.description.text = data.description;
            if (textLegasy.priceValue)
            {
                if (showCurrencyCode) textLegasy.priceValue.text = data.price;
                else textLegasy.priceValue.text = data.priceValue;
            }

#if TMP_YG2
            if (textMP.title) textMP.title.text = data.title;
            if (textMP.description) textMP.description.text = data.description;
            if (textMP.priceValue)
            {
                if (showCurrencyCode) textMP.priceValue.text = data.price;
                else textMP.priceValue.text = data.priceValue;
            }
#endif
            if (purchaseImageLoad)
            {
#if UNITY_EDITOR
                if (data.imageURI == InfoYG.DEMO_IMAGE)
                    purchaseImageLoad.Load(ServerInfo.saveInfo.purchaseImage);
                else
                    purchaseImageLoad.Load(data.imageURI);
#else
                purchaseImageLoad.Load(data.imageURI);
#endif
            }

            if (currencyImageLoad && data.currencyImageURL != string.Empty && data.currencyImageURL != null)
                currencyImageLoad.Load(data.currencyImageURL);
        }

        public void BuyPurchase() => YG2.BuyPayments(id);
    }
}
