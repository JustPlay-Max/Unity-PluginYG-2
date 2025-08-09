#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace YG.EditorScr
{
    [InitializeOnLoad]
    public static class ServerInfo
    {
        public static event Action onLoadServerInfo;

        private static ServerJson _saveInfo = null;
        public static ServerJson saveInfo
        {
            get
            {
                if (_saveInfo == null)
                    Read();
                return _saveInfo;
            }
        }

        public static void DoActionLoadServerInfo()
        {
            onLoadServerInfo?.Invoke();
        }

        public static void Read()
        {
            if (File.Exists(InfoYG.FILE_SERVER_INFO))
            {
                string infoText = File.ReadAllText(InfoYG.FILE_SERVER_INFO);
                _saveInfo = JsonUtility.FromJson<ServerJson>(infoText);
            }
            else
            {
                _saveInfo = new ServerJson();
            }
        }
    }
}
#endif