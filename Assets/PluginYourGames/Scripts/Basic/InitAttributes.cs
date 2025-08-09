using System;

namespace YG
{
    public static partial class YG2
    {
        [AttributeUsage(AttributeTargets.Method)]
        private class InitYG_0Attribute : Attribute { }

        [AttributeUsage(AttributeTargets.Method)]
        private class InitYG_1Attribute : Attribute { }

        [AttributeUsage(AttributeTargets.Method)]
        private class InitYG_2Attribute : Attribute { }

        [AttributeUsage(AttributeTargets.Method)]
        private class InitYGAttribute : Attribute { }

        [AttributeUsage(AttributeTargets.Method)]
        private class StartYGAttribute : Attribute { }
    }
}