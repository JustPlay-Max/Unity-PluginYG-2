
namespace YG
{
    public static partial class YG2
    {
        public static long ServerTime()
        {
#if UNITY_EDITOR
            return infoYG.ServerTime.serverTime;
#else
            return iPlatform.ServerTime();
#endif
        }
    }
}