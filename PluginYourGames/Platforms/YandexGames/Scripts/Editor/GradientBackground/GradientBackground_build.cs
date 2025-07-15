#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void GradientBackground()
        {
            string pathCSS = $"{InfoYG.CORE_FOLDER_YG2}/Platforms/YandexGames/Scripts/Editor/GradientBackground/GradientBackground.css";

            if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.Gradient)
            {
                string textCopy = ManualFileTextCopy(pathCSS);

                textCopy = textCopy.Replace("___CLASS___", "#unity-canvas");

                textCopy = textCopy.Replace("color1", ConvertToRGBA(infoYG.Templates.gradientBackgroundByLoadGame.color1));
                textCopy = textCopy.Replace("color2", ConvertToRGBA(infoYG.Templates.gradientBackgroundByLoadGame.color2));

                if (infoYG.Templates.gradientBackgroundByLoadGame.radial == false)
                {
                    textCopy = textCopy.Replace("radial-gradient", "linear-gradient");
                    textCopy = textCopy.Replace("circle", $"{infoYG.Templates.gradientBackgroundByLoadGame.angleInclination}deg");
                }

                styleFile += $"\n\n\n{textCopy}";
            }

            if (infoYG.Templates.fixedAspectRatio && infoYG.Templates.fillBackground)
            {
                if (infoYG.Templates.imageBackground)
                {
                    string imageName = infoYG.Templates.imageName;
                    string line = $"document.body.style.background = \"url('Images/{imageName}') center / cover no-repeat\";";
                    indexFile = indexFile.Replace("// Fill Background [Build Modify]", line);
                }
                else
                {
                    string textCopy = ManualFileTextCopy(pathCSS);

                    textCopy = textCopy.Replace("___CLASS___", "body");

                    textCopy = textCopy.Replace("color1", ConvertToRGBA(infoYG.Templates.gradientBackgroundByAspectRatio.color1));
                    textCopy = textCopy.Replace("color2", ConvertToRGBA(infoYG.Templates.gradientBackgroundByAspectRatio.color2));

                    if (infoYG.Templates.gradientBackgroundByAspectRatio.radial == false)
                    {
                        textCopy = textCopy.Replace("radial-gradient", "linear-gradient");
                        textCopy = textCopy.Replace("circle", $"{infoYG.Templates.gradientBackgroundByAspectRatio.angleInclination}deg");
                    }

                    styleFile += $"\n\n\n{textCopy}";
                }
            }
        }
    }
}
#endif