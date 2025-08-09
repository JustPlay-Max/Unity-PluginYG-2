#if UNITY_EDITOR
namespace YG.Insides
{
    public static partial class Langs
    {
#if RU_YG2
        public const string basicSettings = "Основные Настройки";
        public const string delete = "Удалить";
        public const string versionControl = "Контроль версий";
        public const string settings = "Настройки";
        public const string cancel = "Отменить";
        public const string continue_ = "Продолжить";
        public const string name = "Имя";
        public const string fullNamePlugin = "Подключайте Свои Игры";
        public const string loading = "загрузка...";
        public const string InstallDependencies = "Установите зависимости";
        public const string other = "Остальное";
        public const string notFound = "Не найдено";
#else
        public const string basicSettings = "Basic Settings";
        public const string delete = "Delete";
        public const string versionControl = "Version control";
        public const string settings = "Settings";
        public const string cancel = "Cancel";
        public const string continue_ = "Continue";
        public const string name = "Name";
        public const string fullNamePlugin = "Plugin Your Games";
        public const string loading = "loading...";
        public const string InstallDependencies = "Install dependencies";
        public const string other = "Other";
        public const string notFound = "Not found";
#endif
    }
}
#endif