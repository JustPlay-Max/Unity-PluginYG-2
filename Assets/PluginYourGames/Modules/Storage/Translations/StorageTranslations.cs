namespace YG.Translations
{
    public class StorageTranslations
    {
#if RU_YG2
        public const string SaveLocalTooltip = "Локальные сохранения. При включённых облачных сохранениях, локальные и облачные синхронизируются! Для платформы Yandex Games лучше выключить локальные сохранения и использовать только облачные! Для других платформ, локальные сохранения могут хорошо подойти. В WebGL сохранение будет производиться в локальное хранилище браузера. Для остальных устройств в PlayerPrefs.";
        public const string SaveCloudTooltip = "Облачные сохранения.";
        public const string FlushTooltip = "Flush — определяет очередность отправки данных. При значении «true» данные будут отправлены на сервер немедленно; «false» (значение по умолчанию) — запрос на отправку данных будет поставлен в очередь. (Рекомендуется оставить данный параметр выключенным)";
        public const string AuthorizationModuleWarningLabelYg = "Требуется модуль авторизации";
#else
        public const string SaveLocalTooltip = "Local saves. When cloud saves are enabled, local and cloud are synchronized! For the Yandex Games platform, it is better to turn off local saves and use only cloud ones! For other platforms, local saves may be well suited. In WebGL, saving will be done to the browser's local storage. For all other devices in PlayerPrefs.";
        public const string SaveCloudTooltip = "Cloud saves.";
        public const string FlushTooltip = "Flush — determines the order in which data is sent. If the value is «true», the data will be sent to the server immediately; «false» (default value) — the request to send data will be queued. (It is recommended to leave this option disabled)";
        public const string AuthorizationModuleWarningLabelYg = "An authorization module is required";
#endif
    }
}