#if UNITY_EDITOR
namespace YG.Insides
{
    public static partial class Langs
    {
#if RU_YG2
        public const string modulesSettings = "Модули";
        public const string simulation = "Симуляция";
        public const string platform = "Платформа";
        public const string applySettings = "Применять настройки проекта под выбранную платформу при переключении платформы и перед сборкой билда?";
        public const string logoImageFormat = "Формат логотипа, который отображается при загрузке игры.\n\nAssets/WebGLTemplates/YandexGames/Images/logo.format";
        public const string backgroundImgFormat = "Формат изображения на заднем плане при загрузке игры.\n\nAssets/WebGLTemplates/YandexGames/Images/background.format";
        public const string t_debugInEditor = "Запись лога в консоль в Unity Editor.";
        public const string t_editEventSystem = "Деактивировать компонент EventSystem во время паузы?\nИногда из-за активного EventSystem во время паузы случаются баги, при нажатии на клавиши, например. Отключите эту опцию, если у вас баги наоборот из-за деактивации EventSystem.";
        public const string t_autoGRA = "При запуске игры выполнять метод Game Ready API.\n\nМомент, когда игра загрузила все ресурсы и готова к взаимодействию с пользователем.\n\nЕсли данный параметр 'AutoGRA' включен, то плагин сам выполнит метод Game Ready API сразу после загрузки игры.\n\rЕсли в Вашей игре имеются свои реализации загрузки игры, например, загрузка первой сцены, то Вам необходимо снять галку 'AutoGRA' и самостоятельно выполнять этот метод, когда игра будет полностью загружена. Выполнение метода: `YG2.GameReadyAPI();`";
        public const string t_autoPauseGame = "При просмотре рекламы и при других ситуациях когда это необходимо - игра будет ставиться на паузу.";
        public const string t_archivingBuild = "Включить автоматическую архивацию билда?\n\n •  После успешного создания билда игры, папка с содержанием билда пакуется в zip архив. Подпись _b... это номер билда. Если архив с таким же номером билда уже есть, он перезапишется новым.";
        public const string t_syncInitSDK = "Для платформы Яндекс Игры опцию Sync Init SDK можно не включать. Но для всех других веб платформ обычно требуется активация данной опции.\n\n •  False - сначала пройдёт полная инициализация SDK платформы, затем игра начнет загружаться. Это даёт наибольшее удобство и гарантию, что при старте все функции SDK доступны для использования. Но загрузка при таком методе может быть немного дольше.\n\n •  True - инициализация платформы будет происходить одновременно с загрузкой игры или после загрузки игры, это зависит от платформы. При таком сценарии, скрипты в проекте должны быть готовы к тому, что при старте игры функционал плагина ещё не готов для использования, т.к. SDK ещё не инициализирован. Требуется тестирование на работоспособность всех функций как в Unity Editor, так и в сборке. В ином случае, можно просто перезагружать сцену после инициализации SDK, или сделать пустую первую сцену и после инициализации загружать основную сцену. Для этого воспользуйтесь готовым решением настроив вложенные ниже опции.";
        public const string t_loadSceneIfSDKLate = "Если игра загрузится раньше, чем инициализируется SDK платформы, то после инициализации произойдёт загрузки выбранной сцены. Попробуйте параметр Load Scene Index оставить 0. Тогда после инициализации первая загруженная сцена перезагрузится. В таком случае проверьте нет ли каких то проблем из-за того, что загружается рабочая сцена, которая может использовать функции плагина до инициализации SDK. При возникновении проблем, ошибок - устраните их, либо сделайте пустую сцену и поместите её на нулевую позицию в Build Settings. Пустая сцена не вызовет ошибок. В параметр Load Scene Index запишите индекс сцены, которую хотите загрузить при старте игры. В Unity Editor в таком случае текущая открытая сцена будет просто перезагружаться независимо от того, какой индекс сцены вы указали. Это сделано для того, чтобы при каждом запуске игры при тестировании не открывалась начальная сцена.";
        public const string t_gradient = "Настройки градиента. Для одноцветного изображения заполните color 1 и color 2 одним и тем же цветом.";
        public const string t_gradient_radial = "При радиальном градиенте первый цвет будет идти из центра плавно перетекать во второй цвет по краям. Radial = false - линейный градиент.";
        public const string t_gradient_angle = "Для линейного градиента можно изменить градус наклона.";
        public const string t_customProgressBar = "Изменить дизайн полосы загрузки.";
        public const string t_progressBar_round = "Скруглить края полосы загрузки.";
        public const string t_progressBar_width = "Ширина полосы загрузки.";
        public const string t_progressBar_color1 = "Основной цвет полосы загрузки.";
        public const string t_progressBar_color2 = "Второстепенный  цвет полосы загрузки (по краям). Для получения однородного цвета всей полосы загрузки укажите один и тот же цвет для color 1 и color 2.";

        public const string t_pixelRatio = "Снижение качества изображения игры в угоду оптимизации для мобильных устройств.\nЧем выше число, тем выше качество. Диапазон от 1 до 2. Попробуйте поставить 1.3.";
        public const string t_developerBuild = "Тестовая сборка. Отображает номер билда в углу экрана (нажмите на текст чтобы скрыть). Помогает убедиться в том, что на сервере черновика актуальный билд. Служит напоминанием, что перед сборкой релизного билда необходимо установить соответствующие значения для параметра Code Optimization в Build Settings.";
        public const string t_fixedAspectRatio = "Фиксировать соотношение сторон, чтобы предотвратить произвольное растягивание экрана.";
        public const string t_aspectRatio = "Пример: 16/9 - это горизонтальный экран, 9/16 - вертикальный.";
        public const string t_disableForMobile = "Для мобильных устройств фиксация экрана будет отключена. Экран на мобильных устройств зачастую и так будет зафиксирован в удобном формате, по этому данная опция может быть полезна.";
        public const string t_fillBackground = "Заполнить задний фон изображением, цветом или градиентом.\n• Поставьте флаг Use Background Image, чтобы использовать изображение от загрузочного экрана игры.\n• Чтобы использовать заполняющий цвет или градиент, уберите флаг и настройте структуру Gradient Background By Aspect Ratio.";
        public const string t_imageBackground = "Включите данную опцию если хотите установить изображение на задний фон.";
        public const string t_imageName = "Название изображения для отображения на заднем фоне расположенное в папке:\nWebGLTemplates/YandexGames/Images\n\nНапример: background.jpg\n\nМожно использовать изображение загрузочного экрана.";

        public const string t_simulationInEditor = "Настройки для симуляции в Unity Editor. Можете тестировать игру, например, на разных языках.";
        public const string advSimHeader = "Реклама";
        public const string advSimLabel = "Настройки симуляции в General Simulation";
        public const string t_advIntervalSimulation = "Симуляция интервала запросов на вызов рекламы в секундах для Unity Editor. Реальный таймер настройте в настройках модуля рекламы.";
        public const string t_advDurationAdv = "Длительность симуляции показа рекламы.";
        public const string t_loadAdv = "Задержка открытия рекламы. Может быть полезна для тестирования уведомления о том, что скоро откроется реклама, перед ёё показом (в момент ожидания рекламы).";

        public const string switchLang = "Русский";
        public const string documentation = "Документация";
        public const string community = "Сообщество";
        public const string video = "Видео";

        public const string versionsActual = "Все версии актуальные";
        public const string versionsUpdate = "Доступны обновления";
        public const string versionsCritical = "Есть критически важные обновления!";

        public const string importNJSON = "Импортировать Newtonsoft JSON";
        public const string activateNJSONForSave = "Активировать NJSON for Storage";
        public const string deactivateNJSONForSave = "Деактивировать NJSON for Storage";

        public const string resetInfoSettings = "Настройки по умолчанию";
        public const string resetInfoSettings_dialog = "Сбросить все настройки плагина?\n(вернитесь назад с помощью ctrl + z)";

        public const string demoScenesInBuildSettings = "Демо Сцены в Build Settings";
        public const string addDemoScenes = "Добавить демо сцены";
        public const string demoScenes = "Демо сцены:\n";
        public const string demoNotAdded = "Демо сцены не добавлены.";
        public const string deleteAllDemoMaterialsFromProject = "Удалить все демо материалы из проекта";
        public const string removeAllDemoMaterials = "Удалить все демо материалы";
        public const string removeAllDemoDialog = "Все демонстрационные сцены и скрипты будут удалены. Восстановить их можно будет повторным импортом. Демо материалы содержатся в пакете самого плагина и в отдельных модулях.";

#else
        public const string platform = "Platform";
        public const string simulation = "Simulation";
        public const string applySettings = "Should I apply the project settings for the selected platform when switching platforms and before building a build?";
        public const string logoImageFormat = "The format of the logo that is displayed when loading the game.\n\nAssets/WebGLTemplates/YandexGames/Images/logo.format";
        public const string backgroundImgFormat = "The format of the image in the background when loading the game.\n\nAssets/WebGLTemplates/YandexGames/Images/background.format";
        public const string t_debugInEditor = "Writing a log to the console in Unity Editor.";
        public const string t_editEventSystem = "Deactivate the Event System component during a pause?\nSometimes, due to the active EventSystem, bugs occur during the pause, when pressing the keys, for example. Disable this option if you have bugs on the contrary due to deactivation of the EventSystem.";
        public const string t_autoGRA = "When starting the game, execute the Game Ready API method.\n\n When the game has loaded all resources and is ready to interact with the user.\n\nIf this 'AutoGRA' parameter is enabled, the plugin will execute the Game Ready API method itself immediately after downloading the game.\n\rIf your game has its own game loading implementations, for example, loading the first scene, then you need to uncheck the 'AutoGRA' checkbox and perform this method yourself when the game is fully loaded. Method execution: `YG2.GameReadyAPI();`";
        public const string t_autoPauseGame = "When viewing ads and in other situations when it is necessary, the game will be paused.";
        public const string t_archivingBuild = "Should I enable automatic build archiving?\n\n • After the successful creation of the game build, the folder with the contents of the build is packed in a zip archive. Signature _b... this is the build number. If there is already an archive with the same build number, it will be overwritten with a new one.";
        public const string t_syncInitSDK = "You don't need to enable the Sync Init SDK option for the Yandex Games platform. However, all other web platforms usually require activation of this option.\n\n • False - the platform SDK will be fully initialized first, then the game will start loading. This provides the greatest convenience and guarantees that all SDK functions are available for use at startup. However, it may take a little longer to load using this method.\n\n • True - initialization of the platform will occur simultaneously with the download of the game or after the download of the game, it depends on the platform. In this scenario, the scripts in the project should be prepared for the fact that when the game starts, the plug-in functionality is not yet ready for use, because the SDK has not yet been initialized. Testing is required for the functionality of all functions in both the Unity Editor and the build. Otherwise, you can simply reload the scene after initializing the SDK, or create an empty first scene and load the main scene after initialization. To do this, use the ready-made solution by configuring the options listed below.";
        public const string t_loadSceneIfSDKLate = "If the game loads before the platform SDK is initialized, then the selected scene will load after initialization. Try leaving the Load Scene Index parameter at 0. Then, after initialization, the first loaded scene will reboot. In this case, check if there are any problems due to the fact that the working scene is loading, which can use the plugin functions before initializing the SDK. If problems or errors occur, fix them, or make an empty scene and place it at the zero position in the Build Settings. An empty scene will not cause errors. In the Load Scene Index parameter, write down the index of the scene that you want to load at the start of the game. In Unity Editor, in this case, the current open scene will simply be reloaded, regardless of which index of the scene you specified. This is done so that the initial scene does not open every time the game is started during testing.";
        public const string t_gradient = "Gradient settings. For a monochrome image, fill color 1 and color 2 with the same color.";
        public const string t_gradient_radial = "With a radial gradient, the first color will flow from the center smoothly into the second color along the edges. Radial = false - linear gradient.";

        public const string t_pixelRatio = "Reducing the image quality of the game for the sake of optimization for mobile devices.\nThe higher the number, the higher the quality. The range is from 1 to 2. Try to put 1.3.";
        public const string t_developerBuild = "Test build. Displays the build number in the corner of the screen (click on the text to hide it). It helps to make sure that the current build is on the draft server. It serves, apparently, in order for the assembly unit to be able to set the appropriate values for optimizing the program code in the assembly settings.";

        public const string t_fixedAspectRatio = "Fix the aspect ratio to prevent arbitrary stretching of the screen.";
        public const string t_aspectRatio = "Example: 16/9 is a horizontal screen, 9/16 is a vertical screen.";
        public const string t_disableForMobile = "Screen locking will be disabled for mobile devices. The screen on mobile devices will often be fixed in a convenient format anyway, so this option can be useful.";
        public const string t_fillBackground = "Fill the background with an image, color, or gradient.\n• Set the Use Background Image flag to use the image from the game's loading screen.\n• To use a fill color or gradient, remove the flag and adjust the Gradient Background By Aspect Ratio structure.";
        public const string t_imageBackground = "Enable this option if you want to set the image to the background.";
        public const string t_imageName = "The name of the image to display in the background located in the folder:\nWebGLTemplates/YandexGames/Images\n\nexample: background.jpg\n\n You can use the image of the loading screen.";

        public const string t_simulationInEditor = "Simulation settings in Unity Editor. You can test the game, for example, in different languages.";
        public const string advSimHeader = "Advertisement";
        public const string advSimLabel = "Ad Simulation Settings in General Simulation";
        public const string t_advIntervalSimulation = "Simulation of the interval of ad requests in seconds for Unity Editor. Set up the real timer in the settings of the advertising module.";
        public const string t_advDurationAdv = "The duration of the simulation of the ad display.";
        public const string t_loadAdv = "The delay in opening the ad. It can be useful for testing notifications that an ad is about to open before it is shown (while waiting for an ad).";
        public const string t_gradient_angle = "For a linear gradient, you can change the degree of inclination.";
        public const string t_customProgressBar = "Change the design of the download bar.";
        public const string t_progressBar_round = "Round off the edges of the loading strip.";
        public const string t_progressBar_width = "The width of the download bandwidth.";
        public const string t_progressBar_color1 = "The main color of the loading bar.";
        public const string t_progressBar_color2 = "The secondary color of the loading bar (along the edges). To get a uniform color for the entire loading strip, specify the same color for color 1 and color 2.";
        public const string switchLang = "English";
        public const string documentation = "Documentation";
        public const string community = "Community";
        public const string video = "Video";

        public const string modulesSettings = "Modules settings";
        public const string versionsActual = "All versions are current";
        public const string versionsUpdate = "Updates are available";
        public const string versionsCritical = "There are critical updates!";

        public const string importNJSON = "Import Newtonsoft JSON";
        public const string activateNJSONForSave = "Activate NJSON for Storage";
        public const string deactivateNJSONForSave = "Deactivate NJSON for Storage";

        public const string resetInfoSettings = "Set default settings";
        public const string resetInfoSettings_dialog = "Reset all plugin settings?\n(go back using ctrl + z)";

        public const string demoScenesInBuildSettings = "Demo Scenes in Build Settings";
        public const string addDemoScenes = "Add Demo Scenes";
        public const string demoScenes = "Demo scenes:\n";
        public const string demoNotAdded = "Demo scenes have not been added.";
        public const string deleteAllDemoMaterialsFromProject = "Remove all demo materials from the project";
        public const string removeAllDemoMaterials = "Remove all demo materials";
        public const string removeAllDemoDialog = "All demo scenes and scripts will be deleted. They can be restored by re-importing. Demo materials are contained in the package of the plugin itself and in separate modules.";
#endif
    }
}
#endif