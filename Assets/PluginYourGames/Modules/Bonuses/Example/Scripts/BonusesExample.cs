using UnityEngine;
using UnityEngine.UI;
using YG;

public class BonusesExample : MonoBehaviour
{
    [Header("UI Elements")]
    public Button getBonusCatalogButton;
    public Button getPlayerBonusesButton;
    public Button consumeBonusButton;
    public InputField tokenInputField;
    public Text statusText;
    public Text catalogText;
    public Text playerBonusesText;

    private void Start()
    {
        // Подписываемся на события
        YG2.onGetBonusCatalog += OnGetBonusCatalog;
        YG2.onGetPlayerBonuses += OnGetPlayerBonuses;
        YG2.onConsumeBonus += OnConsumeBonus;
        YG2.onErrorBonuses += OnBonusesError;

        // Настраиваем кнопки
        if (getBonusCatalogButton != null)
            getBonusCatalogButton.onClick.AddListener(GetBonusCatalog);

        if (getPlayerBonusesButton != null)
            getPlayerBonusesButton.onClick.AddListener(GetPlayerBonuses);

        if (consumeBonusButton != null)
            consumeBonusButton.onClick.AddListener(ConsumeBonus);

        UpdateStatus("Bonuses Example готов к использованию");
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        YG2.onGetBonusCatalog -= OnGetBonusCatalog;
        YG2.onGetPlayerBonuses -= OnGetPlayerBonuses;
        YG2.onConsumeBonus -= OnConsumeBonus;
        YG2.onErrorBonuses -= OnBonusesError;
    }

    public void GetBonusCatalog()
    {
        UpdateStatus("Получение каталога бонусов...");
        YG2.GetBonusCatalog();
    }

    public void GetPlayerBonuses()
    {
        UpdateStatus("Получение бонусов игрока...");
        YG2.GetPlayerBonuses();
    }

    public void ConsumeBonus()
    {
        string token = tokenInputField != null ? tokenInputField.text : "";

        if (string.IsNullOrEmpty(token))
        {
            UpdateStatus("Ошибка: введите токен бонуса");
            return;
        }

        UpdateStatus($"Потребление бонуса с токеном: {token}");
        YG2.ConsumeBonus(token);
    }

    private void OnGetBonusCatalog(BonusCatalogResponse response)
    {
        UpdateStatus("Каталог бонусов получен успешно");

        if (catalogText != null)
        {
            string catalogInfo = "Каталог бонусов:\n";

            if (response.bonuses != null && response.bonuses.Length > 0)
            {
                foreach (var bonus in response.bonuses)
                {
                    catalogInfo += $"ID: {bonus.id}\n";
                    catalogInfo += $"Название: {bonus.title}\n";
                    catalogInfo += $"Описание: {bonus.description}\n";
                    catalogInfo += $"Изображение: {bonus.imageURI}\n\n";
                }
            }
            else
            {
                catalogInfo += "Нет доступных бонусов";
            }

            catalogText.text = catalogInfo;
        }
    }

    private void OnGetPlayerBonuses(PlayerBonusesResponse response)
    {
        UpdateStatus("Бонусы игрока получены успешно");

        if (playerBonusesText != null)
        {
            string bonusesInfo = "Бонусы игрока:\n";

            if (response.isSignedResponse)
            {
                bonusesInfo += $"Подписанный ответ: {response.signature}";
            }
            else if (response.bonuses != null && response.bonuses.Length > 0)
            {
                foreach (var bonus in response.bonuses)
                {
                    bonusesInfo += $"ID: {bonus.id}\n";
                    bonusesInfo += $"Название: {bonus.title}\n";
                    bonusesInfo += $"Токен: {bonus.token}\n";
                    bonusesInfo += $"Получен: {System.DateTimeOffset.FromUnixTimeMilliseconds(bonus.receivedAt).ToString()}\n\n";
                }
            }
            else
            {
                bonusesInfo += "Нет активных бонусов";
            }

            playerBonusesText.text = bonusesInfo;
        }
    }

    private void OnConsumeBonus(ConsumptionResponse response)
    {
        if (response.isSignedResponse)
        {
            UpdateStatus($"Бонус потреблен успешно (подписанный ответ): {response.signature}");
        }
        else
        {
            UpdateStatus($"Бонус потреблен успешно, токен: {response.token}");
        }
    }

    private void OnBonusesError()
    {
        UpdateStatus("Ошибка при работе с бонусами");
    }

    private void UpdateStatus(string message)
    {
        Debug.Log($"[BonusesExample] {message}");

        if (statusText != null)
            statusText.text = message;
    }
}