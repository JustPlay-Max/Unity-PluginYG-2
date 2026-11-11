using System;
using UnityEngine;
using YG.Translations;

namespace YG
{
    public partial class InfoYG
    {
        public StorageSettings Storage = new StorageSettings();

        [Serializable]
        public partial class StorageSettings
        {
            [Tooltip(StorageTranslations.SaveLocalTooltip)]
            public bool saveLocal;

            [Tooltip(StorageTranslations.SaveCloudTooltip)]
            public bool saveCloud = true;

            [NestedYG(nameof(saveCloud))]
            [Tooltip(StorageTranslations.FlushTooltip)]
            public bool flush;

#if UNITY_EDITOR && YandexGamesPlatform_yg && !Authorization_yg
            [NestedYG(false, nameof(saveCloud))]
            [LabelYG(StorageTranslations.AuthorizationModuleWarningLabelYg, "red")]
            [SerializeField] private bool labelAdvSimLabel;
#endif
        }
    }
}