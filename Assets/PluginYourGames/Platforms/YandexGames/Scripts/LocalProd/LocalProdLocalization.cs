#if UNITY_EDITOR
namespace YG.Localization
{
    public static class LocalProdLocalization
    {
#if RU_YG2
        public const string runLocalBuildInYandexGames = "Запустить локальную сборку в Яндекс Играх";
        public const string enableAutorunAfterBuild = "Включить автозапуск после сборки";
        public const string header = "Локальный запуск в production";
        public const string gameIdTooltip = "ID черновика игры в консоли Яндекс Игр. Локальный WebGL-билд будет запущен в production-окружении этого черновика.";
        public const string portTooltip = "Порт сервера для локального запуска WebGL-билда через production-окружение Яндекс Игр. По умолчанию 8080. Измените, если этот порт занят.";
        public const string useCspTooltip = "Добавляет к локальному билду правила безопасности (CSP) из черновика Яндекс Игр. Когда включено, запросы к неразрешённым адресам могут блокироваться: например, к серверу мультиплеера или удалённым ресурсам Addressables. Когда выключено, proxy не добавляет эти правила, но на Яндекс Играх ограничения всё равно будут действовать. Нужный домен можно добавить в белый список в настройках CSP черновика игры.";
        public const string buildFolderNotFound = "Папка последнего WebGL-билда не найдена. Сначала соберите проект для WebGL.";
        public const string indexNotFound = "В папке билда не найден index.html. Укажите полноценный WebGL-билд.";
        public const string invalidGameId = "В настройках платформы укажите корректный положительный Game ID черновика Яндекс Игр.";
        public const string invalidPort = "Порт локального сервера должен быть в диапазоне от 1 до 65535.";
        public const string nodeNotFound = "Не найден npx. Установите Node.js вместе с npm с сайта https://nodejs.org/ и перезапустите Unity. Сам пакет sdk-dev-proxy будет автоматически загружен через npx при запуске.";
        public const string launchFailed = "Не удалось запустить локальный WebGL-билд через Yandex Games SDK Dev Proxy.";
        public const string processNotStarted = "Процесс proxy не был создан.";
        public const string processNotStopped = "Не удалось остановить ранее запущенный proxy-сервер.";
        public const string launchInfo = "Запуск локального WebGL-билда через Yandex Games SDK Dev Proxy\nGame ID: {0}\nПуть к билду: {1}\nПорт: {2}\nCSP: {3}\nКоманда: {4}";
#else
        public const string runLocalBuildInYandexGames = "Run local build in YandexGames";
        public const string enableAutorunAfterBuild = "Enable autorun after build";
        public const string header = "Local production launch";
        public const string gameIdTooltip = "The ID of a game draft in the Yandex Games Console. The local WebGL build will run in this draft's production environment.";
        public const string portTooltip = "Server port for running the local WebGL build in the Yandex Games production environment. The default is 8080. Change it if the port is in use.";
        public const string useCspTooltip = "Adds the Yandex Games draft's Content Security Policy (CSP) to the local build. When enabled, requests to disallowed addresses may be blocked, such as a multiplayer server or remote Addressables content. When disabled, the proxy does not add these rules, but they still apply on Yandex Games. You can add the required domain to the allowlist in the game draft's CSP settings.";
        public const string buildFolderNotFound = "The last WebGL build folder was not found. Build the project for WebGL first.";
        public const string indexNotFound = "index.html was not found in the build folder. Select a complete WebGL build.";
        public const string invalidGameId = "Enter a valid positive Yandex Games draft Game ID in the platform settings.";
        public const string invalidPort = "The local server port must be between 1 and 65535.";
        public const string nodeNotFound = "npx was not found. Install Node.js with npm from https://nodejs.org/ and restart Unity. The sdk-dev-proxy package will be downloaded automatically by npx when launched.";
        public const string launchFailed = "Failed to run the local WebGL build through Yandex Games SDK Dev Proxy.";
        public const string processNotStarted = "The proxy process was not created.";
        public const string processNotStopped = "Failed to stop the previously started proxy server.";
        public const string launchInfo = "Starting the local WebGL build through Yandex Games SDK Dev Proxy\nGame ID: {0}\nBuild path: {1}\nPort: {2}\nCSP: {3}\nCommand: {4}";
#endif
    }
}
#endif
