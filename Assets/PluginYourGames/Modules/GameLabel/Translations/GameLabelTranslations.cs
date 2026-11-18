namespace YG.Translations
{
    public class GameLabelTranslations
    {
#if RU_YG2
        public const string SerializeHeader = "Заполнить";
        public const string EventsHeader = "События:";
        public const string NotSupportedTooltip = "Объект (отключённая кнопка или текст), который будет сообщать о том, что ярлык не поддерживается. Данный объект можно не указывать, тогда, если ярлык не будет поддерживаться - ничего не будет отображаться.";
        public const string DoneTooltip = "Объект (отключённая кнопка или текст), который будет сообщать о том, что ярлык уже установлен. Данный объект можно не указывать, тогда, если ярлык уже установлен - ничего не будет отображаться.";
        public const string ShowDialogTooltip = "Объект с кнопкой, которая будет предлагать установить ярлык на рабочий стол (возможно, за вознаграждение). При клике на кнопку необходимо запускать метод GameLabelShowDialog через данный скрипт или через EventsYG2 скрипт.";
#else
        public const string SerializeHeader = "Serialize";
        public const string EventsHeader = "Events:";
        public const string NotSupportedTooltip = "An object (disabled button or text) that will indicate that the shortcut is not supported. This object can be omitted, then if the shortcut is not supported, nothing will be displayed.";
        public const string DoneTooltip = "An object (a disabled button or text) that will indicate that the shortcut has already been installed. This object can be omitted, then if the shortcut is already installed, nothing will be displayed.";
        public const string ShowDialogTooltip = "An object with a button that will offer to install a shortcut on the desktop (possibly for a fee). When clicking on the button, you need to run the GameLabelShowDialog method through this script or through the EventsYG2 script.";
#endif
    }
}