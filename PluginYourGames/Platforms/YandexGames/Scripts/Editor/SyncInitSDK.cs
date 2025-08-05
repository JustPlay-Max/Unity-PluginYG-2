#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void SyncInitSDK()
        {
            if (infoYG.Basic.syncInitSDK)
            {
                indexFile = indexFile.Replace("let syncInit = false;", "let syncInit = true;");
            }
        }
    }
}
#endif