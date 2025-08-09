#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void SetPixelRatioMobile()
        {
            if (infoYG.Templates.pixelRatioEnable)
            {
                string value = infoYG.Templates.pixelRatioValue.ToString();
                value = value.Replace(",", ".");

                indexFile = indexFile.Replace("//config.devicePixelRatio = 1", "config.devicePixelRatio = " + value);
            }
            else
            {
                indexFile = indexFile.Replace("//config.devicePixelRatio = 1;", string.Empty);
            }
        }
    }
}
#endif