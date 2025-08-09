using System;
using UnityEngine;

namespace YG
{
    public partial class InfoYG
    {
        public StorageSettings Storage = new StorageSettings();

        [Serializable]
        public partial class StorageSettings
        {
#if RU_YG2
            [Tooltip("Локальные сохранения. При включённых облачных сохранениях, локальные и облачные синхронизируются! Для платформы Yandex Games лучше выключить локальные сохранения и использовать только облачные! Для других платформ, локальные сохранения могут хорошо подойти. В WebGL сохранение будет производиться в локальное хранилище браузера. Для остальных устройств в PlayerPrefs.")]
#else
            [Tooltip("Local saves. When cloud saves are enabled, local and cloud are synchronized! For the Yandex Games platform, it is better to turn off local saves and use only cloud ones! For other platforms, local saves may be well suited. In WebGL, saving will be done to the browser's local storage. For all other devices in PlayerPrefs.")]
#endif
            public bool saveLocal;
#if RU_YG2
            [Tooltip("Облачные сохранения.")]
#endif
            public bool saveCloud = true;
#if RU_YG2
            [Tooltip("Flush — определяет очередность отправки данных. При значении «true» данные будут отправлены на сервер немедленно; «false» (значение по умолчанию) — запрос на отправку данных будет поставлен в очередь. (Рекомендуется оставить данный параметр выключенным)")]
#else
            [Tooltip("Flush — determines the order in which data is sent. If the value is «true», the data will be sent to the server immediately; «false» (default value) — the request to send data will be queued. (It is recommended to leave this option disabled)")]
#endif
            [NestedYG(nameof(saveCloud))]
            public bool flush;

#if UNITY_EDITOR && YandexGamesPlatform_yg && !Authorization_yg
#if RU_YG2
            [SerializeField, NestedYG(false, "saveCloud"), LabelYG("Требуется модуль авторизации", "red")]
#else
            [SerializeField, NestedYG(false, "saveCloud"), LabelYG("An authorization module is required", "red")]
#endif
            private bool labelAdvSimLabel;
#endif
        }
    }
}