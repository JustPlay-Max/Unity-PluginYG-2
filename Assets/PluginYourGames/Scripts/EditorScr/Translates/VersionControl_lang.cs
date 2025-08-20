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
        public const string updateInfo = "Обновить список";
        public const string changelog = "Журнал обновлений";
        public const string correctDelete = "Корректное удаление";
        public const string fullDeletePluginYG = "Вы уверены, что хотите полностью удалить PluginYG2 со всеми модулями и другой информацией?";
        public const string protectionAccidentalDeletion = "Защита от случайного удаления";
        public const string fullDeletePluginYGComplete = "Закройте Unity и удалите папки PluginYourGames и WebGLTemplate";
        public const string deleteAll = "Удалить всё";
        public const string deleteModule = "Удалить модуль";
        public const string importDependenciesDialog = "требуется сначала импортировать зависимости.\nВ проекте отсутствуют следующие модули:";
        public const string importModule = "Импорт модуля";
        public const string importAllModules = "Рекомендуется устанавливать только необходимые для вашего проекта модули. Действительно установить все модули?";
        public const string updateWarningDialog = "Если процесс обновления не завершится до конца и вы увидите ошибки, то запустите обновление ещё раз.";
        public const string dependenciesDialog = "Необходимо установить следующие зависимости:";
        public const string dontShowAnymore = "Больше не показывать";
        public const string youHaveUpdates = "У вас есть обновления!";
        public const string updateAll = "Обновить всё";
#else
        public const string removeBeforeImport = "Remove the module before updating (recommended)";
        public const string projectVersion = "Installed version";
        public const string latestVersion = "Latest version";
        public const string control = "Control";
        public const string updateInfo = "Update list";
        public const string changelog = "Changelog";
        public const string correctDelete = "Correct deletion";
        public const string fullDeletePluginYG = "Are you sure you want to completely remove PluginYG2 with all modules and other information?";
        public const string protectionAccidentalDeletion = "Protection against accidental deletion";
        public const string fullDeletePluginYGComplete = "Close Unity and delete the PluginYourGames and WebGLTemplate folders";
        public const string deleteAll = "Delete all";
        public const string deleteModule = "Delete package";
        public const string importDependenciesDialog = "you need to import dependencies first.\nThe following modules are missing from the project:";
        public const string importModule = "Import module";
        public const string importAllModules = "It is recommended to install only the necessary modules for your project. Really install all the modules?";
        public const string updateWarningDialog = "If the update process does not complete completely and you see errors, then run the update again.";
        public const string dependenciesDialog = "The following dependencies must be installed:";
        public const string dontShowAnymore = "Don't Show it anymore";
        public const string youHaveUpdates = "You have updates!";
        public const string updateAll = "Update all";
#endif
    }
}
#endif