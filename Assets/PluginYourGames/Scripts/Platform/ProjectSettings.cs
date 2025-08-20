#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build;

namespace YG.Insides
{
    [Serializable]
    public partial class ProjectSettings
    {
        public bool autoPauseGame = true;
        public bool selectWebGLTemplate = true;
        public bool runInBackground = false;
        public WebGLExceptionSupport enableExceptions = WebGLExceptionSupport.FullWithoutStacktrace;
        public WebGLCompressionFormat compressionFormat = WebGLCompressionFormat.Brotli;
        public bool decompressionFallback;
        public bool autoGraphicsAPI = true;
        public ManagedStrippingLevel managedStrippingLevel = ManagedStrippingLevel.Minimal;
        public bool dataCaching = true;
        public ColorSpace colorSpace = ColorSpace.Gamma;
        public bool archivingBuild = true;
        public bool syncInitSDK;

        public void ApplySettings()
        {
            PlatformToggles toggles = YG2.infoYG.platformToggles;

            if (toggles.autoPauseGame)
                YG2.infoYG.Basic.autoPauseGame = autoPauseGame;

            if (toggles.runInBackground)
                PlayerSettings.runInBackground = runInBackground;

            if (toggles.enableExceptions)
                PlayerSettings.WebGL.exceptionSupport = enableExceptions;

            if (toggles.compressionFormat)
                PlayerSettings.WebGL.compressionFormat = compressionFormat;

            if (toggles.decompressionFallback)
                PlayerSettings.WebGL.decompressionFallback = decompressionFallback;

            if (toggles.autoGraphicsAPI)
            {
                bool currentAutoGraphicsAPIState = PlayerSettings.GetUseDefaultGraphicsAPIs(EditorUserBuildSettings.activeBuildTarget);
                if (currentAutoGraphicsAPIState != autoGraphicsAPI)
                    PlayerSettings.SetUseDefaultGraphicsAPIs(EditorUserBuildSettings.activeBuildTarget, autoGraphicsAPI);
            }

            if (toggles.managedStrippingLevel)
                SetManagedStrippingLevel(managedStrippingLevel);

            if (toggles.dataCaching)
                PlayerSettings.WebGL.dataCaching = dataCaching;

            if (toggles.colorSpace)
                PlayerSettings.colorSpace = colorSpace;

            if (toggles.archivingBuild)
                YG2.infoYG.Basic.archivingBuild = archivingBuild;

            if (toggles.syncInitSDK)
                YG2.infoYG.Basic.syncInitSDK = syncInitSDK;

            CallAction.CallIByAttribute(typeof(ApplySettingsAttribute), GetType(), this);
            AssetDatabase.SaveAssets();
        }

        private static void SetManagedStrippingLevel(ManagedStrippingLevel strippingLevel)
        {
            NamedBuildTarget buildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            if (buildTarget == NamedBuildTarget.Unknown)
                return;

            PlayerSettings.SetManagedStrippingLevel(buildTarget, strippingLevel);
        }
    }
}
#endif