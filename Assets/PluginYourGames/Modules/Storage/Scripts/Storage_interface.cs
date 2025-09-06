using YG.Insides;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        void SaveCloud()
        {
            if (!YG2.infoYG.Storage.saveLocal)
            {
                YGInsides.SaveLocal();
            }
        }

        void LoadCloud()
        {
            if (!YG2.infoYG.Storage.saveLocal)
            {
                YGInsides.LoadLocal();
            }
        }
    }
}
