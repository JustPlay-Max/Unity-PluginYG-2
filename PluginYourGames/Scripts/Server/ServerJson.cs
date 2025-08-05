#if UNITY_EDITOR
namespace YG.EditorScr
{
    [System.Serializable]
    public class ServerJson
    {
        public string redirection = string.Empty;
        public string documentation = "https://max-games.ru/plugin-yg";
        public string chat = "https://t.me/pluginYG";
        public string video = "https://www.youtube.com/playlist?list=PLjS5DHdpBH0Q9qKLenTNbYwaB5IIIsCya";
        public string playerImage = "https://max-games.ru/public/pluginYG2/images/player.png";
        public string purchaseImage = "https://max-games.ru/public/pluginYG2/images/purchase1.png";
        public ModuleJson[] modules = new ModuleJson[0];
    }
}
#endif