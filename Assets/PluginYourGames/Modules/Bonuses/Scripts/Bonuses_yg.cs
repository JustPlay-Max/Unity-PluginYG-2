using System;
using YG.Insides;
using UnityEngine;
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using YG.EditorScr;
#endif

namespace YG
{
    public partial class YG2
    {
        public static Action<ConsumptionResponse> onConsumeBonus;
        public static Action<BonusCatalogResponse> onGetBonusCatalog;
        public static Action<PlayerBonusesResponse> onGetPlayerBonuses;
        public static Action onErrorBonuses;

#if UNITY_EDITOR
        [InitYG]
        private static void ResetStaticBonuses()
        {
            onConsumeBonus = null;
            onGetBonusCatalog = null;
            onGetPlayerBonuses = null;
            onErrorBonuses = null;
        }
#endif

        /// <summary>
        /// Потребить бонус по токену
        /// </summary>
        /// <param name="token">Токен бонуса для потребления</param>
        public static void ConsumeBonus(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
#if RU_YG2
                Message("Ошибка: токен бонуса не может быть пустым");
#else
                Message("Error: bonus token cannot be empty");
#endif
                return;
            }

#if !UNITY_EDITOR
            Message($"Consume Bonus: {token}");
            iPlatform.ConsumeBonus(token);
#else
            Message($"Consume Bonus (Editor): {token}");
            // Симуляция успешного потребления бонуса в редакторе
            YGInsides.OnConsumeBonusSuccess("{\"token\":\"" + token + "\"}");
#endif
        }

        /// <summary>
        /// Получить каталог доступных бонусов
        /// </summary>
        public static void GetBonusCatalog()
        {
#if !UNITY_EDITOR
            Message("Get Bonus Catalog");
            iPlatform.GetBonusCatalog();
#else
            Message("Get Bonus Catalog (Editor)");
            // Симуляция каталога бонусов в редакторе
            string mockCatalog = "[{\"id\":\"bonus1\",\"title\":\"Test Bonus\",\"description\":\"Test bonus description\",\"imageURI\":\"https://example.com/bonus1.png\"}]";
            YGInsides.OnGetBonusCatalogSuccess(mockCatalog);
#endif
        }

        /// <summary>
        /// Получить бонусы игрока
        /// </summary>
        public static void GetPlayerBonuses()
        {
#if !UNITY_EDITOR
            Message("Get Player Bonuses");
            iPlatform.GetPlayerBonuses();
#else
            Message("Get Player Bonuses (Editor)");
            // Симуляция бонусов игрока в редакторе
            string mockPlayerBonuses = "[{\"id\":\"bonus1\",\"title\":\"Test Bonus\",\"description\":\"Test bonus description\",\"imageURI\":\"https://example.com/bonus1.png\",\"receivedAt\":1640995200000,\"token\":\"test_token_123\"}]";
            YGInsides.OnGetPlayerBonusesSuccess(mockPlayerBonuses);
#endif
        }
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public static void OnConsumeBonusSuccess(string data)
        {
            try
            {
                ConsumptionResponse response = JsonUtility.FromJson<ConsumptionResponse>(data);
                response.isSignedResponse = !string.IsNullOrEmpty(response.signature);
                YG2.onConsumeBonus?.Invoke(response);
            }
            catch (Exception e)
            {
                Message("Error parsing consume bonus response: " + e.Message);
                YG2.onErrorBonuses?.Invoke();
            }
        }

        public static void OnGetBonusCatalogSuccess(string data)
        {
            try
            {
                // Создаем wrapper класс для парсинга массива
                string wrappedData = "{\"bonuses\":" + data + "}";
                var wrapper = JsonUtility.FromJson<BonusCatalogWrapper>(wrappedData);
                BonusCatalogResponse response = new BonusCatalogResponse { bonuses = wrapper.bonuses };
                YG2.onGetBonusCatalog?.Invoke(response);
            }
            catch (Exception e)
            {
                Message("Error parsing bonus catalog response: " + e.Message);
                YG2.onErrorBonuses?.Invoke();
            }
        }

        public static void OnGetPlayerBonusesSuccess(string data)
        {
            try
            {
                if (data.StartsWith("{\"signature\":"))
                {
                    // Signed response
                    PlayerBonusesResponse response = JsonUtility.FromJson<PlayerBonusesResponse>(data);
                    response.isSignedResponse = true;
                    YG2.onGetPlayerBonuses?.Invoke(response);
                }
                else
                {
                    // Array response
                    string wrappedData = "{\"bonuses\":" + data + "}";
                    var wrapper = JsonUtility.FromJson<PlayerBonusesWrapper>(wrappedData);
                    PlayerBonusesResponse response = new PlayerBonusesResponse
                    {
                        bonuses = wrapper.bonuses,
                        isSignedResponse = false
                    };
                    YG2.onGetPlayerBonuses?.Invoke(response);
                }
            }
            catch (Exception e)
            {
                Message("Error parsing player bonuses response: " + e.Message);
                YG2.onErrorBonuses?.Invoke();
            }
        }

        public static void OnBonusesError()
        {
            YG2.onErrorBonuses?.Invoke();
        }
    }

    public partial class YGSendMessage
    {
        public void OnConsumeBonusSuccess(string data) => YGInsides.OnConsumeBonusSuccess(data);
        public void OnGetBonusCatalogSuccess(string data) => YGInsides.OnGetBonusCatalogSuccess(data);
        public void OnGetPlayerBonusesSuccess(string data) => YGInsides.OnGetPlayerBonusesSuccess(data);
        public void OnBonusesError() => YGInsides.OnBonusesError();
    }
}