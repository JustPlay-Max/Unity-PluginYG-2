using System;
using UnityEngine;

namespace YG.Insides
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApplySettingsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class SelectPlatformAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class DeletePlatformAttribute : Attribute { }

    public class PlatformAttribute : PropertyAttribute
    {
        public string name { get; private set; }

        public PlatformAttribute(string platformName)
        {
            name = platformName;
        }
    }
}