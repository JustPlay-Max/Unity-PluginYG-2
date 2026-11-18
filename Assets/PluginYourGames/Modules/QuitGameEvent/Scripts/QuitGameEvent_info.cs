using System;
using UnityEngine;
using YG.Translations;

namespace YG
{
    public partial class InfoYG
    {
        public QuitGameEventSettings QuitGameEvent = new QuitGameEventSettings();

        [Serializable]
        public partial class QuitGameEventSettings
        {
            [Tooltip(QuitGameEventTranslations.EnableTooltip)]
            public bool enable;

            [NestedYG(nameof(enable))]
            [Tooltip(QuitGameEventTranslations.ObjectNameTooltip)]
            public string objectName = "ObjectName";

            [NestedYG(nameof(enable))]
            [Tooltip(QuitGameEventTranslations.MethodNameTooltip)]
            public string methodName = "MethodName";
        }
    }
}