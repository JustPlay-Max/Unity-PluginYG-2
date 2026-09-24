#if YandexGamesPlatform_yg
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using YG.Insides;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        private delegate void YandexPurchaseSuccessCallback(string signature);
        private delegate void YandexPurchaseFailedCallback(string error);
        private delegate void YandexPurchasesRestoreSuccessCallback(string signature);
        private delegate void YandexPurchasesRestoreFailedCallback(string error);

        [DllImport("__Internal")]
        private static extern string InitPayments_js();

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void YandexPayments_InitSigned();

        [DllImport("__Internal")]
        private static extern void YandexPayments_PurchaseSigned(string id, YandexPurchaseSuccessCallback successCallback, YandexPurchaseFailedCallback errorCallback);

        [DllImport("__Internal")]
        private static extern void YandexPayments_ConsumeToken(string token);

        [DllImport("__Internal")]
        private static extern void YandexPayments_GetPurchasesSigned(YandexPurchasesRestoreSuccessCallback successCallback, YandexPurchasesRestoreFailedCallback errorCallback);
#endif

        private static string yandexCurrentProductId;
        private static string yandexCurrentOrderId;
        private static bool yandexRestoreOnPurchaseSuccess;

        public void InitPayments()
        {
            if (PaymentsBackendYG.Enabled)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                YandexPayments_InitSigned();
#endif
                YG2.sendMessage.PaymentsEntries(InitPayments_js());
                return;
            }

            YG2.sendMessage.PaymentsEntries(InitPayments_js());
        }

        [DllImport("__Internal")]
        private static extern void BuyPayments_js(string id);
        public void BuyPayments(string id)
        {
            if (PaymentsBackendYG.Enabled)
            {
                BuyPaymentsBackend(id);
                return;
            }

            BuyPayments_js(id);
        }

        [DllImport("__Internal")]
        private static extern void ConsumePurchases_js(bool onPurchaseSuccess);
        public void ConsumePurchases(bool onPurchaseSuccess)
        {
            if (PaymentsBackendYG.Enabled)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                yandexRestoreOnPurchaseSuccess = onPurchaseSuccess;
                YandexPayments_GetPurchasesSigned(OnYandexPurchasesRestoreSuccess, OnYandexPurchasesRestoreFailed);
#endif
                return;
            }

#if !UNITY_EDITOR
            ConsumePurchases_js(onPurchaseSuccess);
#endif
        }

        [DllImport("__Internal")]
        private static extern void ConsumePurchase_js(string id, bool onPurchaseSuccess);
        public void ConsumePurchaseByID(string id, bool onPurchaseSuccess)
        {
            if (PaymentsBackendYG.Enabled)
            {
                string userId = PaymentsUserId();
                PaymentsBackendYG.CheckPurchase("yandex", string.Empty, id, userId, success =>
                {
                    if (success && onPurchaseSuccess)
                    {
                        YGInsides.OnPurchaseSuccess(id);
                        PaymentsBackendYG.ConsumeOrder("yandex", string.Empty, id, userId);
                    }
                });
                return;
            }

#if !UNITY_EDITOR
            ConsumePurchase_js(id, onPurchaseSuccess);
#endif
        }

        private static void BuyPaymentsBackend(string id)
        {
            yandexCurrentProductId = id;

            PaymentsBackendYG.StartOrder(new PaymentsBackendYG.StartOrderRequest
            {
                platform = "yandex",
                productId = id,
                userId = PaymentsUserId()
            }, response =>
            {
                if (response == null || !response.success)
                {
                    Debug.LogWarning("[PluginYG Payments] Yandex order start failed: " + (response?.message ?? "no response"));
                    YGInsides.OnPurchaseFailed(id);
                    return;
                }

                yandexCurrentOrderId = response.orderId;
                string providerProductId = string.IsNullOrEmpty(response.providerProductId) ? id : response.providerProductId;

#if UNITY_WEBGL && !UNITY_EDITOR
                YandexPayments_PurchaseSigned(providerProductId, OnYandexPurchaseSuccess, OnYandexPurchaseFailed);
#else
                YGInsides.OnPurchaseFailed(id);
#endif
            });
        }

        [MonoPInvokeCallback(typeof(YandexPurchaseSuccessCallback))]
        private static void OnYandexPurchaseSuccess(string signature)
        {
            string id = yandexCurrentProductId;
            string orderId = yandexCurrentOrderId;
            string userId = PaymentsUserId();

            PaymentsBackendYG.VerifyPurchase(new PaymentsBackendYG.VerifyOrderRequest
            {
                platform = "yandex",
                productId = id,
                orderId = orderId,
                userId = userId,
                signature = signature,
                receipt = signature
            }, response =>
            {
                if (response != null && response.success)
                {
                    YGInsides.OnPurchaseSuccess(id);
                    PaymentsBackendYG.ConsumeOrder("yandex", orderId, id, userId);

#if UNITY_WEBGL && !UNITY_EDITOR
                    if (!string.IsNullOrEmpty(response.purchaseToken))
                        YandexPayments_ConsumeToken(response.purchaseToken);
#endif
                }
                else
                {
                    Debug.LogWarning("[PluginYG Payments] Yandex purchase verify failed: " + (response?.message ?? "no response"));
                    YGInsides.OnPurchaseFailed(id);
                }
            });
        }

        [MonoPInvokeCallback(typeof(YandexPurchaseFailedCallback))]
        private static void OnYandexPurchaseFailed(string error)
        {
            Debug.LogWarning("[PluginYG Payments] Yandex purchase failed: " + error);
            YGInsides.OnPurchaseFailed(yandexCurrentProductId);
        }

        [MonoPInvokeCallback(typeof(YandexPurchasesRestoreSuccessCallback))]
        private static void OnYandexPurchasesRestoreSuccess(string signature)
        {
            string userId = PaymentsUserId();

            PaymentsBackendYG.RestorePurchases(new PaymentsBackendYG.VerifyOrderRequest
            {
                platform = "yandex",
                userId = userId,
                signature = signature,
                receipt = signature
            }, response =>
            {
                if (response == null || !response.success || response.productId == null)
                    return;

                for (int i = 0; i < response.productId.Length; i++)
                {
                    string id = response.productId[i];
                    if (string.IsNullOrEmpty(id))
                        continue;

                    if (yandexRestoreOnPurchaseSuccess)
                        YGInsides.OnPurchaseSuccess(id);

                    string orderId = response.orderId != null && i < response.orderId.Length
                        ? response.orderId[i]
                        : string.Empty;

                    PaymentsBackendYG.ConsumeOrder("yandex", orderId, id, userId);

#if UNITY_WEBGL && !UNITY_EDITOR
                    string token = response.purchaseToken != null && i < response.purchaseToken.Length
                        ? response.purchaseToken[i]
                        : string.Empty;

                    if (!string.IsNullOrEmpty(token))
                        YandexPayments_ConsumeToken(token);
#endif
                }
            });
        }

        [MonoPInvokeCallback(typeof(YandexPurchasesRestoreFailedCallback))]
        private static void OnYandexPurchasesRestoreFailed(string error)
        {
            if (!string.IsNullOrEmpty(error) && error != "Yandex purchases signature is empty")
                Debug.LogWarning("[PluginYG Payments] Yandex purchases restore failed: " + error);
        }

        private static string PaymentsUserId()
        {
#if Authorization_yg
            return YG2.player.id ?? string.Empty;
#else
            return string.Empty;
#endif
        }
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void PaymentsEntries(string data)
        {
            PaymentsJsonYG.ApplyCatalog(data);
        }

        public void OnPurchaseSuccess(string id)
        {
            YG2.PurchaseByID(id).consumed = true;
            YGInsides.OnPurchaseSuccess(id);
        }

        public void OnPurchaseFailed(string id)
        {
            YGInsides.OnPurchaseFailed(id);
        }
    }
}
#endif
