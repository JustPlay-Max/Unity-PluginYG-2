#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void RewardedAdv()
        {
            string copyCode = FileTextCopy("RewardedAdv_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif