using YG.Insides;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        void SaveCloud() { }
        void LoadCloud() => YGInsides.LoadLocal();
    }
}
