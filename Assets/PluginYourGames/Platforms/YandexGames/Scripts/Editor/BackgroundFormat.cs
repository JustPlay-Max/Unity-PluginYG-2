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

            if (!indexFile.Contains(searchCode))
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
                indexFile = indexFile.Replace(searchCode, searchCode.Replace("png", "jpg"));
                DeleteImage("png");
                DeleteImage("gif");
            }
            else if (infoYG.Templates.backgroundImgFormat == InfoYG.TemplatesSettings.BackgroundImageFormat.GIF)
            {
                indexFile = indexFile.Replace(searchCode, searchCode.Replace("png", "gif"));
                DeleteImage("png");
                DeleteImage("jpg");
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
                string pathImage = $"{ProcessBuild.BuildPath}/Images/background.{format}";

                if (File.Exists(pathImage))
                    FileYG.Delete(pathImage);
            }
        }

    }
}
#endif