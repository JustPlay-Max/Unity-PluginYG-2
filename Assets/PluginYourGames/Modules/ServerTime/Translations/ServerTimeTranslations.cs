#if UNITY_EDITOR

namespace YG.Translations
{
    public class ServerTimeTranslations
    {
#if RU_YG2
        public const string ManualDateSetupTooltip = "TRUE - возвращает значение даты заданное в полях ниже.\nFALSE - возвращает текущую всемирное время.";
#else
        public const string ManualDateSetupTooltip = "TRUE - returns the date value specified in the fields below.\nFALSE - returns the current universal time.";
#endif
    }
}

#endif