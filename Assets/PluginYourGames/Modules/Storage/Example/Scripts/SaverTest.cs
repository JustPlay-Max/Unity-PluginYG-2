using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class SaverTest : MonoBehaviour
    {
        public InputField stringifyText;
        public InputField integerText;
        public Toggle[] booleanArrayToggle;

        // Подписываемся на ивент onGetSDKData
        // Ивент onGetSDKData срабатывает при загрузке сохранений и при других обновлениях данных
        // В данном случае, при нажатии кнопки Set Default Saves будет вызов ивента onGetSDKData и мы обновим данные при сбросе сохранений
        private void OnEnable()
        {
            YG2.onGetSDKData += GetData;
        }

        // Отписываемся от ивента onGetSDKData
        private void OnDisable()
        {
            YG2.onGetSDKData -= GetData;
        }

        private void Awake()
        {
            GetData();
        }

        public void SetData()
        {
            if (int.TryParse(integerText.text, out int intSave)) // Это всё корректная обработка int, для демо сцены
            {
                YG2.saves.intExample = intSave; // Вот пример записи данных для сохранения
            }
            else
            {
                if (integerText.text == string.Empty)
                {
                    YG2.saves.intExample = 0;
                }
                else
                {
#if RU_YG2
                    Debug.LogError("Введите значение типа integer");
#else
                    Debug.LogError("Enter an integer value");
#endif
                }
            }

            YG2.saves.strExample = stringifyText.text.ToString(); // И вот пример

            for (int i = 0; i < booleanArrayToggle.Length; i++)
                YG2.saves.boolExample[i] = booleanArrayToggle[i].isOn;

            // Тут же можно сразу и сохранять данные с помощью YG2.SaveProgress();
        }

        public void GetData()
        {
            integerText.text = string.Empty;
            stringifyText.text = string.Empty;

            integerText.placeholder.GetComponent<Text>().text = YG2.saves.intExample.ToString();
            stringifyText.placeholder.GetComponent<Text>().text = YG2.saves.strExample;

            for (int i = 0; i < booleanArrayToggle.Length; i++)
                booleanArrayToggle[i].isOn = YG2.saves.boolExample[i];
        }

        public void Save()
        {
            YG2.SaveProgress();
        }
    }
}