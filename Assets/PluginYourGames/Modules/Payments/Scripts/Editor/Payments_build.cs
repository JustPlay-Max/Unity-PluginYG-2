#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void Payments()
        {
            InitFunction("InitPayments");

            string copyCode = FileTextCopy("Payments_js.js");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif