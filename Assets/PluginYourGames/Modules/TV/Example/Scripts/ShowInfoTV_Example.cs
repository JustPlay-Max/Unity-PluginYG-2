using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class ShowInfoTV_Example : MonoBehaviour
    {
        public Text keyDownText, keyUpText;

        // Пример. Подписываемся на ивенты ввода кнопок для телевизора
        private void OnEnable()
        {
            YG2.onTVKeyDown += OnTVKeyDown; // Ивент нажатия на кнопку
            YG2.onTVKeyUp += OnTVKeyUp; // Ивент отжатия кнопки
            YG2.onTVKeyBack += OnTVKeyBack;  // Ивент кнопки "назад" (Back)
        }

        // Пример. Отписываемся от ивентов ввода кнопок для телевизора
        private void OnDisable()
        {
            YG2.onTVKeyDown -= OnTVKeyDown;
            YG2.onTVKeyUp -= OnTVKeyUp;
            YG2.onTVKeyBack -= OnTVKeyBack;
        }

        // Пример. Метод закрытия игры на телевизре
        public void ExitTVGame() // Дублирующий метод
        {
            YG2.ExitTVGame(); // Метод из YG2
        }

        private void OnTVKeyDown(string value)
        {
            keyDownText.text = KeyCounterCalculate(keyDownText.text, value);
        }

        private void OnTVKeyUp(string value)
        {
            keyUpText.text = KeyCounterCalculate(keyUpText.text, value);
        }

        private void OnTVKeyBack()
        {
            string value = "Back";
            keyDownText.text = KeyCounterCalculate(keyDownText.text, value);
            keyUpText.text = KeyCounterCalculate(keyUpText.text, value);
        }

        private string KeyCounterCalculate(string oldText, string newText)
        {
            if (oldText == null || oldText == "")
            {
                return newText;
            }

            string[] oldSplit = oldText.Split();

            if (newText == oldSplit[0])
            {
                if (oldSplit.Length == 1)
                { 
                    return newText + " +1";
                }
                else
                {
                    int indexOfPlus = oldText.IndexOf('+');
                    int number = int.Parse(oldText.Substring(indexOfPlus + 1));
                    
                    return newText + " +" + (number + 1).ToString();
                }
            }
            else
            {
                return newText;
            }
        }
    }
}