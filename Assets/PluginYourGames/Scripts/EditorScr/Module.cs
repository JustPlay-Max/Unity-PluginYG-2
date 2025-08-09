#if UNITY_EDITOR
using System;

namespace YG.EditorScr
{
    [Serializable]
    public class Module
    {
        public string nameModule;
        public string projectVersion;
        public string lastVersion;
        public string download;
        public string doc;
        public bool critical;
        public bool noLoad;
        public bool platform;
        public string dependencies;
    }
}
#endif