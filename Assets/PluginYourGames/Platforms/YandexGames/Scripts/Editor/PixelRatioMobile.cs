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

                indexFileContent = indexFileContent.Replace("//config.devicePixelRatio = 1", $"config.devicePixelRatio = {pixelRatioValue}");
            }
            else
            {
                indexFileContent = indexFileContent.Replace("//config.devicePixelRatio = 1;", string.Empty);
            }
        }
    }
}
#endif