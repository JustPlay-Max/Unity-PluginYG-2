#if EmptyWebGLPlatform_yg
using System.IO;
using UnityEngine;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void SetBackgroundFormat()
        {
            string pathImagesFolder = Path.Combine(BUILD_PATCH, "Images");
            string sourceFilePath = Path.Combine(InfoYG.PATCH_PC_WEBGLTEMPLATES, "YandexGames", "Images", "background.");
            string searchCode = @"loadingCover.style.background = ""url('Images/background.png') center / cover"";";

            if (!indexFile.Contains(searchCode))
            {
                Debug.LogWarning("Search string not found in index.html");
                return;
            }

            if (infoYG.Templates.logoImageFormat != InfoYG.TemplatesSettings.LogoImgFormat.No)
            {
                if (!Directory.Exists(pathImagesFolder))
                    Directory.CreateDirectory(pathImagesFolder);
            }

            if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.PNG)
            {
                DeleteImage("jpg");
                DeleteImage("gif");

                sourceFilePath += "png";
                string destinationFilePath = Path.Combine(pathImagesFolder, "background.png");

                if (File.Exists(sourceFilePath))
                    File.Copy(sourceFilePath, destinationFilePath, true);
            }
            else if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.JPG)
            {
                indexFile = indexFile.Replace(searchCode, searchCode.Replace("png", "jpg"));
                DeleteImage("png");
                DeleteImage("gif");

                sourceFilePath += "jpg";
                string destinationFilePath = Path.Combine(pathImagesFolder, "background.jpg");

                if (File.Exists(sourceFilePath))
                    File.Copy(sourceFilePath, destinationFilePath, true);
            }
            else if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.GIF)
            {
                indexFile = indexFile.Replace(searchCode, searchCode.Replace("png", "gif"));
                DeleteImage("png");
                DeleteImage("jpg");

                sourceFilePath += "gif";
                string destinationFilePath = Path.Combine(pathImagesFolder, "background.gif");

                if (File.Exists(sourceFilePath))
                    File.Copy(sourceFilePath, destinationFilePath, true);
            }
            else if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.Unity)
            {
                if (indexFile.Contains("var backgroundUnity = "))
                    indexFile = indexFile.Replace(searchCode, "canvas.style.background = backgroundUnity;");
                else
                    indexFile = indexFile.Replace(searchCode, string.Empty);

                DeleteImage("png");
                DeleteImage("jpg");
                DeleteImage("gif");
            }
            else
            {
                indexFile = indexFile.Replace(searchCode, string.Empty);
                DeleteImage("png");
                DeleteImage("jpg");
                DeleteImage("gif");
            }

            void DeleteImage(string format)
            {
                string pathImage = BUILD_PATCH + "/Images/background." + format;

                if (File.Exists(pathImage))
                {
                    File.Delete(pathImage);
                }
            }
        }

    }
}
#endif