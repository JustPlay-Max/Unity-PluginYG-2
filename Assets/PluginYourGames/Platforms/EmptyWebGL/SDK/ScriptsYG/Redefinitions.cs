using YG.Insides;

#if EmptyWebGLPlatform_yg
namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
#if RewardedAdv_yg
        public void RewardedAdvShow(string id)
        {
            if (YG2.infoYG.platformInfo.giveReward)
            {
                YGInsides.RewardAdv(id);
            }
        }
#endif
    }
}
#endif