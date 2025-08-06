namespace YG
{
    public partial interface IPlatformsYG2
    {
        void InterstitialAdvShow() { }
        void FirstInterAdvShow() => YG2.InterstitialAdvShow();
        void OtherInterAdvShow() => YG2.InterstitialAdvShow();
        void LoadInterAdv() { }
    }
}