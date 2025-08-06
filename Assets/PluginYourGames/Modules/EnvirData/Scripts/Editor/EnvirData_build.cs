#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void EnvirData()
        {
            InitFunction("RequestingEnvironmentData", CodeType.Init0);

            string copyCode = FileTextCopy("EnvirData_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif