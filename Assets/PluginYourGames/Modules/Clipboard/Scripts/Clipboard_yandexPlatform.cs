#if YandexGamesPlatform_yg
using System;
using System.Runtime.InteropServices;
using UnityEditor;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern void SetClipboardText_js(string text);

        public void SetClipboardText(string text)
        {
            SetClipboardText_js(text);
        }

        [DllImport("__Internal")]
        private static extern void GetClipboardTextAsync_js(IntPtr callbackPointer);

        [DllImport("__Internal")]
        private static extern void FreeClipboardText_js(IntPtr textPointer);

        private delegate void ClipboardCallback(int textPointer);

        private Action<string> clipboardCallbackAction;

        public void GetClipboardTextAsync(Action<string> callback)
        {
#if UNITY_EDITOR || !PLATFORM_WEBGL
            string copyBuffer = EditorGUIUtility.systemCopyBuffer;
            callback?.Invoke(copyBuffer);
#else
            clipboardCallbackAction = callback;
            ClipboardCallback internalCallback = StaticClipboardCallback;
            IntPtr callbackPointer = Marshal.GetFunctionPointerForDelegate(internalCallback);
            GetClipboardTextAsync_js(callbackPointer);
#endif
        }

        [MonoPInvokeCallback(typeof(ClipboardCallback))]
        private void StaticClipboardCallback(int textPointer)
        {
            if (clipboardCallbackAction == null)
                return;

            if (textPointer == 0)
            {
                clipboardCallbackAction.Invoke(null);
            }
            else
            {
                string clipboardText = Marshal.PtrToStringAuto((IntPtr)textPointer);
                FreeClipboardText_js((IntPtr)textPointer);
                clipboardCallbackAction.Invoke(clipboardText);
            }

            clipboardCallbackAction = null;
        }
    }
}
#endif