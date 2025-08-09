using System;
using UnityEngine;
#if PLATFORM_WEBGL
using System.Runtime.InteropServices;
#endif

namespace YG
{
    public static partial class YG2
    {
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern void SetClipboardText_js(string text);

        [DllImport("__Internal")]
        private static extern void GetClipboardTextAsync_js(IntPtr callbackPointer);

        [DllImport("__Internal")]
        private static extern void FreeClipboardText_js(IntPtr textPointer);

        private delegate void ClipboardCallback(int textPointer);

        private static Action<string> clipboardCallbackAction;

        [MonoPInvokeCallback(typeof(ClipboardCallback))]
        private static void StaticClipboardCallback(int textPointer)
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
#endif

        public static void SetClipboardText(string text)
        {
#if UNITY_EDITOR || !PLATFORM_WEBGL
            GUIUtility.systemCopyBuffer = text;
#else
            SetClipboardText_js(text);
#endif
        }

        public static void GetClipboardTextAsync(Action<string> callback)
        {
#if UNITY_EDITOR || !PLATFORM_WEBGL
            string copyBuffer = GUIUtility.systemCopyBuffer;
            callback?.Invoke(copyBuffer);
#else
            clipboardCallbackAction = callback;
            ClipboardCallback internalCallback = StaticClipboardCallback;
            IntPtr callbackPointer = Marshal.GetFunctionPointerForDelegate(internalCallback);
            GetClipboardTextAsync_js(callbackPointer);
#endif
        }
    }
}