using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class BannerExample : MonoBehaviour
    {
        public Dropdown dropdownBannerPosition;

        public void SetBannerPosition()
        {
            switch (dropdownBannerPosition.value)
            {
                case 0:
                    YG2.SetBannerPosition(YG2.BannerPosition.Top);
                    break;
                case 1:
                    YG2.SetBannerPosition(YG2.BannerPosition.Bottom);
                    break;
                case 2:
                    YG2.SetBannerPosition(YG2.BannerPosition.Left);
                    break;
                case 3:
                    YG2.SetBannerPosition(YG2.BannerPosition.Right);
                    break;
            }
        }

        public void LoadBanner()
        {
            YG2.LoadBanner();
        }

        public void ShowBanner()
        {
            YG2.ShowBanner();
        }

        public void HideBanner()
        {
            YG2.HideBanner();
        }

        public void DestroyBanner()
        {
            YG2.DestroyBanner();
        }
    }
}
