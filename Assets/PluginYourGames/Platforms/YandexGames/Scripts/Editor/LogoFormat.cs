#if YandexGamesPlatform_yg
using System.IO;
using UnityEngine;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void SetLogoImageFormat()
        {
            string searchCode = "Images/logo.png";

            if (!indexFile.Contains(searchCode))
            {
                Debug.LogError($"Search string '{searchCode}' not found in index.html");
                return;
            }

            if (infoYG.Templates.logoImageFormat == InfoYG.TemplatesSettings.LogoImgFormat.PNG)
            {
                DeleteLogo("jpg");
                DeleteLogo("gif");
            }
            else if (infoYG.Templates.logoImageFormat == InfoYG.TemplatesSettings.LogoImgFormat.JPG)
            {
                indexFile = indexFile.Replace(searchCode, searchCode.Replace("png", "jpg"));
                DeleteLogo("png");
                DeleteLogo("gif");
            }
            else if (infoYG.Templates.logoImageFormat == InfoYG.TemplatesSettings.LogoImgFormat.GIF)
            {
                indexFile = indexFile.Replace(searchCode, searchCode.Replace("png", "gif"));
                DeleteLogo("png");
                DeleteLogo("jpg");
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