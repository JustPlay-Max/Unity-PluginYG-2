#if YandexGamesPlatform_yg
using UnityEngine;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void ProgressBar()
        {
            if (infoYG.Templates.customProgressBar)
            {
                // Color and Width
                string textCopy = ManualFileTextCopy($"{InfoYG.CORE_FOLDER_YG2}/Platforms/YandexGames/Scripts/Editor/ProgressBar/ColorProgressBar.css");

                textCopy = textCopy.Replace("___COLOR_BAR___", ConvertToRGBA(infoYG.Templates.progressBarSettigs.color1));
                textCopy = textCopy.Replace("___COLOR_BORDER___", ConvertToRGBA(infoYG.Templates.progressBarSettigs.color2));

                Color colorHalf = infoYG.Templates.progressBarSettigs.color2;
                colorHalf = new Color(colorHalf.r, colorHalf.g, colorHalf.b, 0.2f);
                textCopy = textCopy.Replace("___WIDTH_BAR___", infoYG.Templates.progressBarSettigs.width.ToString());

                textCopy = textCopy.Replace("___COLOR_BAR_HALF___", ConvertToRGBA(colorHalf));
                styleFile += $"\n\n\n{textCopy}";

                // Hight
                textCopy = ManualFileTextCopy($"{InfoYG.CORE_FOLDER_YG2}/Platforms/YandexGames/Scripts/Editor/ProgressBar/HightProgressBar.css");
                textCopy = textCopy.Replace("___HIGHT_BAR___", infoYG.Templates.progressBarSettigs.height.ToString());
                styleFile += $"\n\n\n{textCopy}";

                // Round
                if (infoYG.Templates.progressBarSettigs.round)
                {
                    textCopy = ManualFileTextCopy($"{InfoYG.CORE_FOLDER_YG2}/Platforms/YandexGames/Scripts/Editor/ProgressBar/RoundProgressBar.css");

                    textCopy = textCopy.Replace("__ROUND_BAR_EMPTY__", (infoYG.Templates.progressBarSettigs.roundAngle + 2).ToString());
                    textCopy = textCopy.Replace("__ROUND_BAR_FULL__", infoYG.Templates.progressBarSettigs.roundAngle.ToString());

                    styleFile += $"\n\n\n{textCopy}";
                }
            }
        }
    }
}
#endif