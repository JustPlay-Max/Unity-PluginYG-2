namespace YG.Translations
{
    public class ReviewTranslations
    {
#if RU_YG2
        public const string UpdateDataOnEnableTooltip = "Обновлять информацию при каждой активации объекта (в OnEnable)?";
#else
        public const string UpdateDataOnEnableTooltip = "Update information every time an object is activated (in OnEnable)?";
#endif
    }
}