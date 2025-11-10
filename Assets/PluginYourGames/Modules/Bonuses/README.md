# Модуль Bonuses для PluginYG2

Модуль Bonuses предоставляет интеграцию с API бонусов Yandex Games, позволяя разработчикам работать с системой бонусов платформы.

## Возможности

- **Получение каталога бонусов** - получить список всех доступных бонусов
- **Получение бонусов игрока** - получить список активных бонусов конкретного игрока
- **Потребление бонусов** - использовать бонус по токену

## Установка

1. Импортируйте модуль Bonuses в ваш проект
2. Убедитесь, что модуль Authorization также импортирован (зависимость)
3. Модуль автоматически интегрируется с системой сборки PluginYG2

## Основные методы

### YG2.GetBonusCatalog()

Получает каталог всех доступных бонусов.

```csharp
YG2.GetBonusCatalog();
```

### YG2.GetPlayerBonuses()

Получает список активных бонусов игрока.

```csharp
YG2.GetPlayerBonuses();
```

### YG2.ConsumeBonus(string token)

Потребляет бонус по указанному токену.

```csharp
YG2.ConsumeBonus("bonus_token_123");
```

## События

### YG2.onGetBonusCatalog

Вызывается при успешном получении каталога бонусов.

```csharp
YG2.onGetBonusCatalog += (BonusCatalogResponse response) => {
    foreach (var bonus in response.bonuses)
    {
        Debug.Log($"Бонус: {bonus.title} - {bonus.description}");
    }
};
```

### YG2.onGetPlayerBonuses

Вызывается при успешном получении бонусов игрока.

```csharp
YG2.onGetPlayerBonuses += (PlayerBonusesResponse response) => {
    if (response.isSignedResponse)
    {
        Debug.Log($"Подписанный ответ: {response.signature}");
    }
    else
    {
        foreach (var bonus in response.bonuses)
        {
            Debug.Log($"Активный бонус: {bonus.title}, токен: {bonus.token}");
        }
    }
};
```

### YG2.onConsumeBonus

Вызывается при успешном потреблении бонуса.

```csharp
YG2.onConsumeBonus += (ConsumptionResponse response) => {
    if (response.isSignedResponse)
    {
        Debug.Log($"Бонус потреблен (подписанный ответ): {response.signature}");
    }
    else
    {
        Debug.Log($"Бонус потреблен, токен: {response.token}");
    }
};
```

### YG2.onErrorBonuses

Вызывается при ошибке в работе с бонусами.

```csharp
YG2.onErrorBonuses += () => {
    Debug.LogError("Ошибка при работе с бонусами");
};
```

## Типы данных

### BonusData

Базовая информация о бонусе:

- `string id` - уникальный идентификатор
- `string title` - название бонуса
- `string description` - описание бонуса
- `string imageURI` - ссылка на изображение

### PlayerBonusData

Расширенная информация о бонусе игрока (наследует BonusData):

- `long receivedAt` - время получения бонуса (Unix timestamp)
- `string token` - токен для потребления бонуса

### ConsumptionResponse

Ответ на потребление бонуса:

- `string token` - токен бонуса
- `string signature` - подпись (для подписанных ответов)
- `bool isSignedResponse` - флаг подписанного ответа

### PlayerBonusesResponse

Ответ на запрос бонусов игрока:

- `PlayerBonusData[] bonuses` - массив бонусов
- `string signature` - подпись (для подписанных ответов)
- `bool isSignedResponse` - флаг подписанного ответа

## Пример использования

```csharp
using UnityEngine;
using YG;

public class BonusManager : MonoBehaviour
{
    private void Start()
    {
        // Подписываемся на события
        YG2.onGetBonusCatalog += OnCatalogReceived;
        YG2.onGetPlayerBonuses += OnPlayerBonusesReceived;
        YG2.onConsumeBonus += OnBonusConsumed;
        YG2.onErrorBonuses += OnBonusError;

        // Получаем каталог бонусов
        YG2.GetBonusCatalog();
    }

    private void OnCatalogReceived(BonusCatalogResponse response)
    {
        Debug.Log($"Получено бонусов в каталоге: {response.bonuses.Length}");

        // Получаем бонусы игрока
        YG2.GetPlayerBonuses();
    }

    private void OnPlayerBonusesReceived(PlayerBonusesResponse response)
    {
        if (!response.isSignedResponse && response.bonuses.Length > 0)
        {
            // Потребляем первый доступный бонус
            YG2.ConsumeBonus(response.bonuses[0].token);
        }
    }

    private void OnBonusConsumed(ConsumptionResponse response)
    {
        Debug.Log("Бонус успешно потреблен!");
    }

    private void OnBonusError()
    {
        Debug.LogError("Произошла ошибка при работе с бонусами");
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        YG2.onGetBonusCatalog -= OnCatalogReceived;
        YG2.onGetPlayerBonuses -= OnPlayerBonusesReceived;
        YG2.onConsumeBonus -= OnBonusConsumed;
        YG2.onErrorBonuses -= OnBonusError;
    }
}
```

## Особенности

1. **Подписанные ответы**: API может возвращать подписанные ответы для дополнительной безопасности
2. **Симуляция в редакторе**: Модуль поддерживает симуляцию работы в Unity Editor
3. **Обработка ошибок**: Все методы имеют встроенную обработку ошибок
4. **Зависимости**: Требует модуль Authorization для корректной работы

## Версия

v1.000 - Первоначальная реализация модуля Bonuses
