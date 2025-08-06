namespace YG
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class YG2
    {
        public enum BannerPosition { Top, Bottom, Left, Right };
        public static BannerPosition position = BannerPosition.Top;

        public static Action onBannerClicked;
        public static Action onBannerError;

        private static GameObject bannerSimulation;

        public static void SetBannerPosition(BannerPosition bannerPosition)
        {
            position = bannerPosition;
        }
        public static void LoadBanner()
        {
#if !UNITY_EDITOR
            iPlatform.LoadBanner(position);
#endif
        }
        public static void ShowBanner()
        {
#if UNITY_EDITOR
            ShowBannerSimulation();
#else
            iPlatform.ShowBanner(position);
#endif
        }
        public static void HideBanner()
        {
#if UNITY_EDITOR
            HideBannerSimulation();
#else
            iPlatform.HideBanner();
#endif
        }
        public static void DestroyBanner()
        {
#if UNITY_EDITOR
            HideBannerSimulation();
#else
            iPlatform.DestroyBanner();
#endif
        }

#if UNITY_EDITOR
        private static void ShowBannerSimulation()
        {
            HideBannerSimulation();

            bannerSimulation = new GameObject { name = "Banner Simulation" };
            MonoBehaviour.DontDestroyOnLoad(bannerSimulation);

            Canvas canvas = bannerSimulation.AddComponent<Canvas>();
            canvas.sortingOrder = 32765;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject banner = new GameObject("Banner");
            banner.transform.SetParent(canvas.transform, false);

            RawImage rawImage = banner.AddComponent<RawImage>();
            rawImage.color = infoYG.BannerAdv.color;

            RectTransform rect = banner.GetComponent<RectTransform>();

            Vector2 size = new Vector2(Screen.width, 100);

            switch (position)
            {
                case BannerPosition.Top:
                    rect.anchorMin = new Vector2(0, 1);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.pivot = new Vector2(0.5f, 1);
                    rect.sizeDelta = new Vector2(0, infoYG.BannerAdv.size);
                    rect.anchoredPosition = new Vector2(0, 0);
                    break;

                case BannerPosition.Bottom:
                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(1, 0);
                    rect.pivot = new Vector2(0.5f, 0);
                    rect.sizeDelta = new Vector2(0, infoYG.BannerAdv.size);
                    rect.anchoredPosition = new Vector2(0, 0);
                    break;

                case BannerPosition.Left:
                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(0, 1);
                    rect.pivot = new Vector2(0, 0.5f);
                    rect.sizeDelta = new Vector2(infoYG.BannerAdv.size, 0);
                    rect.anchoredPosition = new Vector2(0, 0);
                    break;

                case BannerPosition.Right:
                    rect.anchorMin = new Vector2(1, 0);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.pivot = new Vector2(1, 0.5f);
                    rect.sizeDelta = new Vector2(infoYG.BannerAdv.size, 0);
                    rect.anchoredPosition = new Vector2(0, 0);
                    break;
            }
        }

        private static void HideBannerSimulation()
        {
            if (bannerSimulation)
                MonoBehaviour.Destroy(bannerSimulation);
        }
#endif
    }
}
