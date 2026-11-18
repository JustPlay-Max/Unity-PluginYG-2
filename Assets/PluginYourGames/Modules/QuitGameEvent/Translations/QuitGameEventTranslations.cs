namespace YG.Translations
{
    public class QuitGameEventTranslations
    {
#if RU_YG2
        public const string EnableTooltip = "Выполнять определённый метод при закрытии или обновлении страницы игры.";
        public const string ObjectNameTooltip = "Имя объекта, который содержит нужный метод для выполнения после закрытия игры.";
        public const string MethodNameTooltip = "Имя метода. Подходит публичный метод без перегрузок.";
#else
        public const string EnableTooltip = "Perform a specific method when closing or refreshing the game page.";
        public const string ObjectNameTooltip = "The name of the object that contains the desired method to execute after the game is closed.";
        public const string MethodNameTooltip = "The name of the method. A public method without overloads is suitable.";
#endif
    }
}