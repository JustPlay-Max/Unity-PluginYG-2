namespace YG.Insides
{
#if PLATFORM_WEBGL
    using System.Runtime.InteropServices;
#endif

    public static partial class YGInsides
    {
        public static InfoYG infoYG { get => YG2.infoYG; }
        public static IPlatformsYG2 iPlatform { get => YG2.iPlatform; }
        public static void Message(string message) => YG2.Message(message);
        public static void GetDataInvoke() => YG2.GetDataInvoke();


#if PLATFORM_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void FreeBuffer_js(System.IntPtr ptr);
#endif
        public static void FreeBuffer(System.IntPtr ptr)
        {
#if PLATFORM_WEBGL && !UNITY_EDITOR
            FreeBuffer_js(ptr);
#endif
        }
    }
}