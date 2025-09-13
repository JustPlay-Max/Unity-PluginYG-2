#if YandexGamesPlatform_yg
using System.IO;
using UnityEngine;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void SetBackgroundFormat()
        {
            string searchCode = @"loadingCover.style.background = ""url('Images/background.png') center / cover"";";

            if (!indexFileContent.Contains(searchCode))
            {
                Debug.LogWarning("Search string not found in index.html");
                return;
            }

            if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.PNG)
            {
                DeleteImage("jpg");
                DeleteImage("gif");
            }
            else if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.JPG)
            {
                indexFileContent = indexFileContent.Replace(searchCode, searchCode.Replace("png", "jpg"));
                DeleteImage("png");
                DeleteImage("gif");
            }
            else if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.GIF)
            {
                indexFileContent = indexFileContent.Replace(searchCode, searchCode.Replace("png", "gif"));
                DeleteImage("png");
                DeleteImage("jpg");
            }
            else if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.Unity)
            {
                if (indexFileContent.Contains("var backgroundUnity = "))
                    indexFileContent = indexFileContent.Replace(searchCode, "canvas.style.background = backgroundUnity;");
                else
                    indexFileContent = indexFileContent.Replace(searchCode, string.Empty);

                DeleteImage("png");
                DeleteImage("jpg");
                DeleteImage("gif");
            }
            else
            {
                indexFileContent = indexFileContent.Replace(searchCode, string.Empty);
                DeleteImage("png");
                DeleteImage("jpg");
                DeleteImage("gif");
            }

            void DeleteImage(string format)
            {
                string pathImage = $"{ProcessBuild.BuildPath}/Images/background.{format}";

                if (File.Exists(pathImage))
                    File.Delete(pathImage);
            }
        }

    }
}
#endif