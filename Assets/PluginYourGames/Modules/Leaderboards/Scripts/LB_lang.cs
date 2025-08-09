#if UNITY_EDITOR
namespace YG.Insides
{
    public static partial class Langs
    {
#if RU_YG2
        public const string t_nameLB = "Техническое название соревновательной таблицы";
        public const string t_maxQuantityPlayers = "Максимальное количество игроков в таблице";
        public const string t_quantityTop = "Кол-во получения верхних топ игроков";
        public const string t_quantityAround = "Кол-во получения верхних топ игроков";
        public const string t_updateLBMethod = "Когда следует обновлять лидерборд?\nStart - Обновлять в методе Start.\nOnEnable - Обновлять при каждой активации объекта (в методе OnEnable)\nDoNotUpdate - Не обновлять лидерборд с помощью данного скрипта (подразоумивается, что метод обновления 'UpdateLB' вы будете запускать сами, когда вам потребуется.";
        public const string t_entriesText = "Перетащите компонент Text для записи описания таблицы, если вы не выбрали продвинутую таблицу (advanced)";
        public const string t_advanced = "Продвинутая таблица. Поддерживает подгрузку авата и конвертацию рекордов в тип Time. Подгружает все данные в отдельные элементы интерфейса.";
        public const string t_rootSpawnPlayersData = "Родительский объект для спавна в нём объектов 'playerDataPrefab'";
        public const string t_playerDataPrefab = "Префаб отображаемых данных игрока (объект со компонентом LBPlayerDataYG)";
        public const string t_playerPhoto = "Размер подгружаемых изображений игроков. NonePhoto = не подгружать изображение";
        public const string t_isHiddenPlayerPhoto = "Использовать кастомный спрайт для отображения аваторок скрытых пользователей";
        public const string t_timeTypeConvert = "Конвертация полученных рекордов в Time тип";
        public const string t_decimalSize = "Размер десятичной части счёта (при использовании Time type).\n  Например:\n  0 = 00:00\n  1 = 00:00.0\n  2 = 00:00.00\n  3 = 00:00.000\nВы можете проверить это в Unity не прибегая к тестированию в сборке.";
#else
        public const string t_nameLB = "Technical name of the competition table";
        public const string t_maxQuantityPlayers = "The maximum number of players in the table";
        public const string t_quantityTop = "Number of top players to receive";
        public const string t_quantityAround = "Number of records received near the user";
        public const string t_updateLBMethod = "When should the leaderboard be updated?\nStart - Update in the Start method.\nOnEnable - Update each time the object is activated (in the OnEnable method)\nDoNotUpdate - Do not update the leaderboard using this script (it is assumed that you will run the 'UpdateLB' update method yourself when you need it.";
        public const string t_entriesText = "Drag the Text component to write the table description if you have not selected the advanced table";
        public const string t_advanced = "An advanced table. Supports uploading an avatar and converting records to the Time type. Loads all data into separate interface elements.";
        public const string t_rootSpawnPlayersData = "The parent object for spawning 'playerDataPrefab' objects in it";
        public const string t_playerDataPrefab = "Prefab of the player's displayed data (an object with the LBPlayerDataYG component)";
        public const string t_playerPhoto = "The size of the uploaded images of the players. NonePhoto = do not upload an image";
        public const string t_isHiddenPlayerPhoto = "Use a custom sprite to display avatars of hidden users";
        public const string t_timeTypeConvert = "Converting received records to Time type";
        public const string t_decimalSize = "The size of the decimal part of the account (when using Time type).\n For example:\n 0 = 00:00\n 1 = 00:00.0\n 2 = 00:00.00\n 3 = 00:00.000\n You can check this in Unity without resorting to testing in the build.";
#endif
    }
}
#endif