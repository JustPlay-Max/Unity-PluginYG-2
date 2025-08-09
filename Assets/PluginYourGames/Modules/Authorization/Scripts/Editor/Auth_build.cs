#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void Authorization()
        {
            InitFunction("InitPlayer", CodeType.Init2);

            string copyCode = FileTextCopy("Auth_js.js");
            //copyCode = copyCode.Replace("___scopes___", infoYG.Authorization.scopes.ToString().ToLower());
            copyCode = copyCode.Replace("___photoSize___", infoYG.Authorization.GetPlayerPhotoSize());

            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif