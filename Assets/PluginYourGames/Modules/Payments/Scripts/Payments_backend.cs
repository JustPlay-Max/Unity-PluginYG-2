using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace YG.Insides
{
    public static class PaymentsBackendYG
    {
        private const string ApiPath = "/api/v1";

        [Serializable]
        public class StartOrderRequest
        {
            public string platform;
            public string productId;
            public string userId;
            public string userToken;
            public string providerToken;
            public string language;
        }

        [Serializable]
        public class StartOrderResponse
        {
            public bool success;
            public string orderId;
            public string providerProductId;
            public string paymentUrl;
            public string paymentToken;
            public string title;
            public string description;
            public string price;
            public int priceValue;
            public bool immediateSuccess;
            public string message;
        }

        [Serializable]
        public class VerifyOrderRequest
        {
            public string platform;
            public string productId;
            public string orderId;
            public string userId;
            public string providerToken;
            public string purchaseToken;
            public string receipt;
            public string signature;
            public string providerTransactionId;
        }

        [Serializable]
        public class VerifyOrderResponse
        {
            public bool success;
            public string status;
            public string productId;
            public string orderId;
            public string providerTransactionId;
            public string purchaseToken;
            public string message;
        }

        [Serializable]
        public class RestorePurchasesResponse
        {
            public bool success;
            public string[] orderId;
            public string[] productId;
            public string[] purchaseToken;
            public string message;
        }

        [Serializable]
        public class StatusResponse
        {
            public bool success;
            public string status;
            public string productId;
            public string message;
        }

        public static bool Enabled =>
            YG2.infoYG != null &&
            YG2.infoYG.Payments.useBackend &&
            !string.IsNullOrEmpty(YG2.infoYG.Payments.backendURL);

        public static void LoadCatalog(string platform, Action<bool> completed = null)
        {
            if (!Enabled)
            {
                completed?.Invoke(false);
                return;
            }

            Runner.StartCoroutine(LoadCatalogRoutine(platform, completed));
        }

        public static void StartOrder(StartOrderRequest request, Action<StartOrderResponse> completed)
        {
            if (string.IsNullOrEmpty(request.language))
                request.language = CatalogLanguage();

            if (!Enabled)
            {
                completed?.Invoke(new StartOrderResponse
                {
                    success = true,
                    providerProductId = request.productId
                });
                return;
            }

            Runner.StartCoroutine(PostRoutine("orders/start", JsonUtility.ToJson(request), completed));
        }

        public static void VerifyPurchase(VerifyOrderRequest request, Action<VerifyOrderResponse> completed)
        {
            if (!Enabled)
            {
                completed?.Invoke(new VerifyOrderResponse
                {
                    success = true,
                    status = "paid",
                    productId = request.productId,
                    orderId = request.orderId
                });
                return;
            }

            Runner.StartCoroutine(PostRoutine("orders/verify", JsonUtility.ToJson(request), completed));
        }

        public static void RestorePurchases(VerifyOrderRequest request, Action<RestorePurchasesResponse> completed)
        {
            if (!Enabled)
            {
                completed?.Invoke(new RestorePurchasesResponse
                {
                    success = false,
                    orderId = new string[0],
                    productId = new string[0],
                    purchaseToken = new string[0]
                });
                return;
            }

            Runner.StartCoroutine(PostRoutine("orders/restore", JsonUtility.ToJson(request), completed));
        }

        public static void CheckPurchase(string platform, string orderId, string productId, string userId, Action<bool> completed)
        {
            if (!Enabled)
            {
                completed?.Invoke(false);
                return;
            }

            if (string.IsNullOrEmpty(platform) || (string.IsNullOrEmpty(orderId) && (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(userId))))
            {
                completed?.Invoke(false);
                return;
            }

            Runner.StartCoroutine(CheckPurchaseRoutine(platform, orderId, productId, userId, completed));
        }

        public static void ConsumePurchases(string platform, string userId, bool onPurchaseSuccess)
        {
            if (!Enabled)
                return;

            if (string.IsNullOrEmpty(platform) || string.IsNullOrEmpty(userId))
                return;

            Runner.StartCoroutine(ConsumePurchasesRoutine(platform, userId, onPurchaseSuccess));
        }

        public static void ConsumeOrder(string platform, string orderId, string productId, string userId)
        {
            if (!Enabled)
                return;

            if (string.IsNullOrEmpty(orderId) && (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(userId)))
                return;

            VerifyOrderRequest request = new VerifyOrderRequest
            {
                platform = platform,
                orderId = orderId,
                productId = productId,
                userId = userId
            };

            Runner.StartCoroutine(PostRoutine<VerifyOrderResponse>("orders/consume", JsonUtility.ToJson(request), null));
        }

        private static IEnumerator LoadCatalogRoutine(string platform, Action<bool> completed)
        {
            string url = Url("products")
                + "?platform=" + UnityWebRequest.EscapeURL(platform)
                + "&language=" + UnityWebRequest.EscapeURL(CatalogLanguage());

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (IsRequestError(request))
                {
                    Debug.LogWarning("[PluginYG Payments] Catalog load failed: " + request.error);
                    completed?.Invoke(false);
                    yield break;
                }

                bool result = PaymentsJsonYG.ApplyCatalog(request.downloadHandler.text);
                completed?.Invoke(result);
            }
        }

        private static IEnumerator PostRoutine<T>(string path, string json, Action<T> completed)
        {
            using (UnityWebRequest request = new UnityWebRequest(Url(path), UnityWebRequest.kHttpVerbPOST))
            {
                byte[] body = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(body);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (IsRequestError(request))
                {
                    Debug.LogWarning("[PluginYG Payments] Backend request failed: " + request.error);
                    completed?.Invoke(default(T));
                    yield break;
                }

                try
                {
                    completed?.Invoke(JsonUtility.FromJson<T>(request.downloadHandler.text));
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[PluginYG Payments] Backend response parse failed: " + e.Message);
                    completed?.Invoke(default(T));
                }
            }
        }

        private static IEnumerator CheckPurchaseRoutine(string platform, string orderId, string productId, string userId, Action<bool> completed)
        {
            int attempts = Mathf.Max(1, YG2.infoYG.Payments.pollAttempts);
            float delay = Mathf.Max(0.2f, YG2.infoYG.Payments.pollDelay);

            for (int i = 0; i < attempts; i++)
            {
                string url = Url("orders/status")
                    + "?platform=" + UnityWebRequest.EscapeURL(platform)
                    + "&productId=" + UnityWebRequest.EscapeURL(productId ?? string.Empty)
                    + "&orderId=" + UnityWebRequest.EscapeURL(orderId ?? string.Empty)
                    + "&userId=" + UnityWebRequest.EscapeURL(userId ?? string.Empty);

                using (UnityWebRequest request = UnityWebRequest.Get(url))
                {
                    yield return request.SendWebRequest();

                    if (!IsRequestError(request))
                    {
                        StatusResponse response = null;
                        try
                        {
                            response = JsonUtility.FromJson<StatusResponse>(request.downloadHandler.text);
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning("[PluginYG Payments] Status response parse failed: " + e.Message);
                        }

                        if (response != null && response.success && IsPaid(response.status))
                        {
                            completed?.Invoke(true);
                            yield break;
                        }
                    }
                }

                yield return new WaitForSecondsRealtime(delay);
            }

            completed?.Invoke(false);
        }

        private static IEnumerator ConsumePurchasesRoutine(string platform, string userId, bool onPurchaseSuccess)
        {
            string url = Url("orders/pending")
                + "?platform=" + UnityWebRequest.EscapeURL(platform)
                + "&userId=" + UnityWebRequest.EscapeURL(userId ?? string.Empty);

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (IsRequestError(request))
                    yield break;

                PendingPurchases pending;
                try
                {
                    pending = JsonUtility.FromJson<PendingPurchases>(request.downloadHandler.text);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[PluginYG Payments] Pending purchases parse failed: " + e.Message);
                    yield break;
                }

                if (pending == null || pending.productId == null)
                    yield break;

                for (int i = 0; i < pending.productId.Length; i++)
                {
                    if (onPurchaseSuccess)
                        YGInsides.OnPurchaseSuccess(pending.productId[i]);
                }
            }
        }

        private static bool IsPaid(string status)
        {
            return status == "paid" || status == "granted";
        }

        private static bool IsRequestError(UnityWebRequest request)
        {
#if UNITY_2020_1_OR_NEWER
            return request.result == UnityWebRequest.Result.ConnectionError ||
                   request.result == UnityWebRequest.Result.ProtocolError;
#else
            return request.isNetworkError || request.isHttpError;
#endif
        }

        private static string Url(string path)
        {
            string baseUrl = YG2.infoYG.Payments.backendURL.TrimEnd('/');
            string apiPath = ApiPath.Trim('/');
            return string.IsNullOrEmpty(apiPath)
                ? baseUrl + "/" + path
                : baseUrl + "/" + apiPath + "/" + path;
        }

        private static string CatalogLanguage()
        {
#if Localization_yg
            return YG2.lang ?? string.Empty;
#elif EnvirData_yg
            return YG2.envir.language ?? string.Empty;
#else
            return string.Empty;
#endif
        }

        private static CoroutineRunner Runner
        {
            get
            {
                if (runner == null)
                {
                    GameObject obj = new GameObject("PaymentsBackendYG");
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                    runner = obj.AddComponent<CoroutineRunner>();
                }

                return runner;
            }
        }

        private static CoroutineRunner runner;

        private class CoroutineRunner : MonoBehaviour { }

        [Serializable]
        private class PendingPurchases
        {
            public string[] productId;
        }
    }
}
