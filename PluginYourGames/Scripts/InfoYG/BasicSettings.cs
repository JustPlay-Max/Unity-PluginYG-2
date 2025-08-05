using System;
using UnityEngine;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        public BasicSettings Basic = new BasicSettings();

        [Serializable]
        public partial class BasicSettings
        {
#if UNITY_EDITOR
            [HeaderYG(Langs.platform, 10)]
#endif
            public PlatformSettings platform;
#if UNITY_EDITOR
            [Tooltip(Langs.applySettings)]
#endif
            public bool autoApplySettings = true;

#if UNITY_EDITOR
            [HeaderYG(Langs.other)]
            [Tooltip(Langs.t_autoPauseGame)]
#endif
            public bool autoPauseGame = true;
#if UNITY_EDITOR
            [NestedYG(nameof(autoPauseGame)), Tooltip(Langs.t_editEventSystem)]
#endif
            public bool editEventSystem = true;
#if UNITY_EDITOR
            [Tooltip(Langs.t_autoGRA)]
#endif
            public bool autoGRA = true;

#if UNITY_EDITOR
            [Tooltip(Langs.t_debugInEditor)]
#endif
            public bool logInEditor = true;
#if UNITY_EDITOR
            [Tooltip(Langs.t_archivingBuild)]
#endif
            public bool archivingBuild = true;
#if UNITY_EDITOR
            [Tooltip(Langs.t_syncInitSDK)]
#endif
            public bool syncInitSDK;
#if UNITY_EDITOR
            [NestedYG(nameof(syncInitSDK)), Tooltip(Langs.t_loadSceneIfSDKLate)]
#endif
            public bool loadSceneIfSDKLate;
#if UNITY_EDITOR
            [NestedYG(nameof(loadSceneIfSDKLate), nameof(syncInitSDK)), Min(0)]
#endif
            public int loadSceneIndex;
#if UNITY_EDITOR
            [NestedYG(nameof(loadSceneIfSDKLate), nameof(syncInitSDK)), Min(0)]
#endif
            public bool simulationLoadScene;
        }
    }
}
