#if EmptyWebGLPlatform_yg
using System.IO;
using UnityEngine;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void SetLogoImageFormat()
        {
            string pathImagesFolder = Path.Combine(BUILD_PATCH, "Images");
            string sourceFilePath = Path.Combine(InfoYG.PATCH_PC_WEBGLTEMPLATES, "YandexGames", "Images", "logo.");
            string searchCode = "Images/logo.png";

            if (!indexFile.Contains(searchCode))
            {
                Debug.LogError($"Search string '{searchCode}' not found in index.html");
                return;
            }

            if (infoYG.Templates.logoImageFormat != InfoYG.TemplatesSettings.LogoImgFormat.No)
            {
                if (!Directory.Exists(pathImagesFolder))
                    Directory.CreateDirectory(pathImagesFolder);
            }

            if (infoYG.Templates.logoImageFormat == InfoYG.TemplatesSettings.LogoImgFormat.PNG)
            {
                DeleteLogo("jpg");
                DeleteLogo("gif");

                sourceFilePath += "png";
                string destinationFilePath = Path.Combine(pathImagesFolder, "logo.png");

                if (File.Exists(sourceFilePath))
                    File.Copy(sourceFilePath, destinationFilePath, true);
            }
            else if (infoYG.Templates.logoImageFormat == InfoYG.TemplatesSettings.LogoImgFormat.JPG)
            {
                indexFile = indexFile.Replace(searchCode, searchCode.Replace("png", "jpg"));
                DeleteLogo("png");
                DeleteLogo("gif");

                sourceFilePath += "jpg";
                string destinationFilePath = Path.Combine(pathImagesFolder, "logo.jpg");

                if (File.Exists(sourceFilePath))
                    File.Copy(sourceFilePath, destinationFilePath, true);
            }
            else if (infoYG.Templates.logoImageFormat == InfoYG.TemplatesSettings.LogoImgFormat.GIF)
            {
                indexFile = indexFile.Replace(searchCode, searchCode.Replace("png", "gif"));
                DeleteLogo("png");
                DeleteLogo("jpg");

                sourceFilePath += "gif";
                string destinationFilePath = Path.Combine(pathImagesFolder, "logo.gif");

                if (File.Exists(sourceFilePath))
                    File.Copy(sourceFilePath, destinationFilePath, true);
            }
            else if (infoYG.Templates.logoImageFormat == InfoYG.TemplatesSettings.LogoImgFormat.No)
            {
                indexFile = indexFile.Replace(@"<div id=""unity-logo""><img src=""Images/logo.png""></div>", string.Empty);
                DeleteLogo("png");
                DeleteLogo("jpg");
                DeleteLogo("gif");
            }

            void DeleteLogo(string format)
            {
                string pathImage = BUILD_PATCH + "/Images/logo." + format;

                if (File.Exists(pathImage))
                {
                    File.Delete(pathImage);
                }
            }
        }

    }
}
#endif