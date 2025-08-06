#if UNITY_EDITOR
namespace YG.Insides
{
    public static partial class Langs
    {
#if RU_YG2
        public const string removeBeforeImport = "Удалять модуль перед обновлением (рекомендуется)";
        public const string projectVersion = "Установленная версия";
        public const string latestVersion = "Доступная версия";
        public const string control = "Контроль";
        public const string updateInfo = "Обновить информацию";
        public const string correctDelete = "Корректное удаление";
        public const string fullDeletePluginYG = "Вы уверены, что хотите полностью удалить PluginYG2 со всеми модулями и другой информацией?";
        public const string fullDeletePluginYGComplete = "Перезагрузите Unity Editor.";
        public const string deleteAll = "Удалить всё";
        public const string deletePackage = "Удалить пакет";
        public const string importDependenciesDialog1 = "Для пакета";
        public const string importDependenciesDialog2 = "требуется сначала импортировать зависимости.\nВ проекте отсутствуют следующие модули:";
        public const string importPackage = "Импорт пакета";
        public const string thirdPartyDialog = "Согласны ли вы загрузить пакет со стороннего ресурса?";
        public const string quickImport = "Поставьте галочки над теми модулями, которые хотите импортировать.\n\nПри быстром импорте вы должны понимать - какие модули зависят от друг-друга.\nЕсли возникают непредвиденные ошибки, импортируйте модули отдельно.";
        public const string updatePluginFirst = "Сначала обновите Plugin Your Games. Затем уже модули.\n\nНе обращайте внимания, если в процессе обновления возникнут ошибки компиляции. Просто обновите все модули до последних версий.";
#else
        public const string removeBeforeImport = "Remove the module before updating (recommended)";
        public const string projectVersion = "Installed version";
        public const string latestVersion = "Latest version";
        public const string control = "Control";
        public const string updateInfo = "Update info";
        public const string correctDelete = "Correct deletion";
        public const string fullDeletePluginYG = "Are you sure you want to completely remove PluginYG2 with all modules and other information?";
        public const string fullDeletePluginYGComplete = "Restart the Unity Editor.";
        public const string deleteAll = "Delete all";
        public const string deletePackage = "Delete package";
        public const string importDependenciesDialog1 = "For the package";
        public const string importDependenciesDialog2 = "you need to import dependencies first.\nThe following modules are missing from the project:";
        public const string importPackage = "Import package";
        public const string thirdPartyDialog = "Do you agree to download the package from a third-party resource?";
        public const string quickImport = "Check the boxes above the modules that you want to import.\n\nWhen importing quickly, you need to understand which modules depend on each other.\nIf unexpected errors occur, import the modules separately.";
        public const string updatePluginFirst = "Update Plugin first. Then the modules.\n\nDo not pay attention if compilation errors occur during the update process. Just update all modules to the latest versions.";
#endif
    }
}
#endif