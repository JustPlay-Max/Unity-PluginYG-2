#if UNITY_EDITOR

namespace YG.Translations
{
    public class AutoTranslateLangsTranslations
    {
#if RU_YG2
        public const string NjsonNeedImportErrorMsg = "Для авто локализации требуется импортировать пакет Newtonsoft JSON. Сделать это можно в настройках";
#else
        public const string NjsonNeedImportErrorMsg = "For auto localization, you need to import the Newtonsoft JSON package. You can do this in the settings";
#endif
    }
}

#endif