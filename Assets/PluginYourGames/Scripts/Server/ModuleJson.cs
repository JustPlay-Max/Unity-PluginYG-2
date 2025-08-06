#if UNITY_EDITOR
namespace YG.EditorScr
{
    [System.Serializable]
    public class ModuleJson
    {
        public string name;
        public string version;
        public string download;
        public string doc;
        public bool critical;
        public bool noLoad;
        public bool platform;
        public string dependencies;
    }
}
#endif