#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void SetPixelRatioMobile()
        {
            if (infoYG.Templates.pixelRatioEnable)
            {
                string pixelRatioValue = infoYG.Templates.pixelRatioValue
                    .ToString()
                    .Replace(",", ".");

                indexFile = indexFile.Replace("//config.devicePixelRatio = 1", $"config.devicePixelRatio = {pixelRatioValue}");
            }
            else
            {
                indexFile = indexFile.Replace("//config.devicePixelRatio = 1;", string.Empty);
            }
        }
    }
}
#endif