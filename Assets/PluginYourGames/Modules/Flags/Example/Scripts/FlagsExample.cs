using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class FlagsExample : MonoBehaviour
    {
        public Text textDataOutput;

        private void Start()
        {
            // Вывод в тексте всех флагов путём перебора массива:

            textDataOutput.text = string.Empty;

            if (YG2.flagsDictionary.Count > 0)
            {
                foreach (var flag in YG2.flagsDictionary)
                {
                    textDataOutput.text += $"{flag.Key}: {flag.Value}\n";
                }
            }
            else
            {
                textDataOutput.text = "No flags found!";
            }

            // Пример получения и обработки флагов:
            // Допустим, из облака мы получаем уровень сложности

            string value = YG2.GetFlag("difficult");

            if (value == "easy")
            {
                // Установите лёгкий уровень сложности.
                //Debug.Log("Difficulty: easy");
            }
            else if (value == "middle")
            {
                // Установите средний уровень сложности.
                //Debug.Log("Difficulty: middle");
            }
            else if (value == "hard")
            {
                // Установите сложный уровень сложности.
                //Debug.Log("Difficulty: hard");
            }
            else
            {
                // Значение флага не определено, установите дефолтное значение.
                // Если значение не определено, метод GetFlag вернёт null.
                //Debug.Log("Difficulty: middle");
            }


            // Пример получения флага, если такой флаг существует

            if (YG2.TryGetFlag("difficult", out string difficult))
            {
                // Флаг существует, пользуемся им!
                Debug.Log(difficult);
            }

            if (YG2.TryGetFlagAsInt("intType", out int intType))
            {
                // Флаг существует и преобразован в тип int
            }

            if (YG2.TryGetFlagAsFloat("floatType", out float floatType))
            {
                // Флаг существует и преобразован в тип float
            }

            if (YG2.TryGetFlagAsBool("boolType", out bool boolType))
            {
                // Флаг существует и преобразован в тип bool
            }
        }
    }
}