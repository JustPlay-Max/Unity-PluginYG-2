#if UNITY_EDITOR
namespace YG.Insides
{
    public static partial class Langs
    {
#if RU_YG2
        public const string t_rootSpawnPurchases = "Родительский объект для спавна в нём покупок";
        public const string t_purchasePrefab = "Префаб покупки (объект со компонентом PurchaseYG)";
        public const string t_showCurrencyCode = "Добавить код валюты к строке цены. (Например YAN)";
        public const string t_purchaseImageLoad = "Компонент ImageLoadYG для загрузки изображения покупки.";
        public const string t_currencyImageLoad = "Компонент ImageLoadYG для загрузки изображения валюты.";
#else
        public const string t_rootSpawnPurchases = "The parent object for spawning purchases in it";
        public const string t_purchasePrefab = "Purchase prefab (an object with the PurchaseYG component)";
        public const string t_showCurrencyCode = "Add the currency code to the price line. (For example, YAN)";
        public const string t_purchaseImageLoad = "The ImageLoadYG component for uploading a purchase image.";
        public const string t_currencyImageLoad = "The ImageLoadYG component for uploading a currency image.";
#endif
    }
}
#endif