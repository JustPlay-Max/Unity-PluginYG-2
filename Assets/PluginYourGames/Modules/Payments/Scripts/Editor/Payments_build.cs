#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void Payments()
        {
            InitFunction("InitPayments");

            string copyCode = FileTextCopy("Payments_js.js");
            copyCode = copyCode.Replace("let paymentsAutoConsume = true;",
                $"let paymentsAutoConsume = {infoYG.Payments.AutoConsume.ToString().ToLowerInvariant()};");
            AddIndexCode(copyCode, CodeType.JS);
        }
    }
}
#endif
