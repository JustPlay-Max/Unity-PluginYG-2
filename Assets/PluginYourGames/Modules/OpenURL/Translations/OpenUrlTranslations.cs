namespace YG.Translations
{
    public class OpenUrlTranslations
    {
#if RU_YG2
        public const string OnUrlErrorMsg = "Первый способ перехода по ссылке не удался!\nВместо первого метода попробуем вызвать второй метод [Application.OpenURL]\nОшибка:\n";
#else
        public const string OnUrlErrorMsg = "The first method of following the link failed!\nInstead of the first method, let's try to call the second method [Application.OpenURL]\nError:\n";
#endif
    }
}