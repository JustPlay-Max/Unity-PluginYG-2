using System.Collections.Generic;
using YG.Utils.Metrica;

namespace YG
{
    public static partial class YG2
    {
        [InitYG]
        private static void InitMetrica()
        {
#if !UNITY_EDITOR
            iPlatform.InitMetrica();
#endif
        }

        /// <summary>
        /// Отправить событие в Метрику.
        /// </summary>
        /// <param name="eventName">Имя события.</param>
        public static void MetricaSend(string eventName)
        {
            Log(eventName, null);
#if !UNITY_EDITOR
            iPlatform.MetricaSend(eventName);
#endif
        }

        /// <summary>
        /// Отправить событие в Метрику с параметрами Dictionary<string, object>.
        /// </summary>
        /// <param name="eventName">Имя события.</param>
        /// <param name="eventData">Данные события в формате ключ-значение (object).</param>
        public static void MetricaSend(string eventName, Dictionary<string, object> eventData)
        {
            string jsonData = "{}";
            if (eventData != null)
            {
                jsonData = JsonUtils.ToJson(eventData);
            }

            Log(eventName, jsonData);
#if !UNITY_EDITOR
            iPlatform.MetricaSend(eventName, jsonData);
#endif
        }

        /// <summary>
        /// Отправить событие в Метрику с параметрами Dictionary<string, string>.
        /// </summary>
        /// <param name="eventName">Имя события.</param>
        /// <param name="eventData">Данные события в формате ключ-значение (string).</param>
        public static void MetricaSend(string eventName, Dictionary<string, string> eventData)
        {
            string jsonData = "{}";
            if (eventData != null)
            {
                // Сериализация данных через JsonUtils
                jsonData = JsonUtils.ToJson(eventData);
            }

            Log(eventName, jsonData);
#if !UNITY_EDITOR
            iPlatform.MetricaSend(eventName, jsonData);
#endif
        }

        /// <summary>
        /// Отправить событие в Метрику с вложенными параметрами.
        /// </summary>
        /// <param name="eventName">Имя события.</param>
        /// <param name="nestedParam">Имя вложенного параметра (2 уровень).</param>
        /// <param name="subNestedParam">Имя вложенного параметра (3 уровень).</param>
        public static void MetricaSend(string eventName, string nestedParam, string subNestedParam)
        {
            var eventData = new Dictionary<string, string>
            {
                { nestedParam, subNestedParam }
            };

            string jsonData = JsonUtils.ToJson(eventData);

            Log(eventName, jsonData);
#if !UNITY_EDITOR
            iPlatform.MetricaSend(eventName, jsonData);
#endif
        }

        /// <summary>
        /// Отправить событие в Метрику с вложенными параметрами.
        /// </summary>
        /// <param name="eventName">Имя события.</param>
        /// <param name="nestedParam">Имя вложенного параметра (2 уровень).</param>
        /// <param name="subNestedData">Объект из вложенных параметров (3 уровень).</param>
        public static void MetricaSend(string eventName, string nestedParam, Dictionary<string, object> subNestedData)
        {
            var eventData = new Dictionary<string, object>
            {
                { nestedParam, subNestedData }
            };

            string jsonData = JsonUtils.ToJson(eventData);

            Log(eventName, jsonData);
#if !UNITY_EDITOR
            iPlatform.MetricaSend(eventName, jsonData);
#endif
        }

        private static void Log(string eventName, string eventParam)
        {
            if (!infoYG.Metrica.log)
                return;
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(eventParam) || eventParam == "{}")
            {
                Message($"Metrica send: {eventName}");
            }
            else
            {
                Message($"Metrica send: {eventName}; {eventParam}");
            }
#else
            Message($"Metrica send");
#endif
        }
    }
}

