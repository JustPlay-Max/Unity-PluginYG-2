using UnityEngine;
#if YG_TEXT_MESH_PRO
using TMPro;
#endif

namespace YG.Utils.Lang
{
    public class UtilsLang : MonoBehaviour
    {
        public static string UnauthorizedTextTranslate()
        {
            return UnauthorizedTextTranslate(YG2.lang);
        }

        public static string UnauthorizedTextTranslate(string languageTranslate)
        {
            string name;

            switch (languageTranslate)
            {
                case "ru":
                    name = "неавторизованный";
                    break;
                case "en":
                    name = "unauthorized";
                    break;
                case "tr":
                    name = "yetkisiz";
                    break;
                case "az":
                    name = "icazəsiz";
                    break;
                case "be":
                    name = "неаўтарызаваны";
                    break;
                case "et":
                    name = "loata";
                    break;
                case "fr":
                    name = "non autorisé";
                    break;
                case "kk":
                    name = "рұқсат етілмеген";
                    break;
                case "ky":
                    name = "уруксатсыз";
                    break;
                case "lt":
                    name = "neleistinas";
                    break;
                case "lv":
                    name = "neleistinas";
                    break;
                case "ro":
                    name = "neautorizat";
                    break;
                case "tg":
                    name = "беиҷозат";
                    break;
                case "tk":
                    name = "yetkisiz";
                    break;
                case "uk":
                    name = "несанкціонований";
                    break;
                case "uz":
                    name = "ruxsatsiz";
                    break;
                case "es":
                    name = "autorizado";
                    break;
                case "pt":
                    name = "autorizado";
                    break;
                case "id":
                    name = "tidak sah";
                    break;
                case "it":
                    name = "autorizzato";
                    break;
                case "de":
                    name = "unerlaubter";
                    break;
                default:
                    name = "---";
                    break;

            }
            return name;
        }

        public static string IsHiddenTextTranslate()
        {
            return IsHiddenTextTranslate(YG2.lang);
        }

        public static string IsHiddenTextTranslate(string languageTranslate)
        {
            string name;

            switch (languageTranslate)
            {
                case "ru":
                    name = "скрыт";
                    break;
                case "en":
                    name = "is hidden";
                    break;
                case "tr":
                    name = "gizli";
                    break;
                case "az":
                    name = "gizlidir";
                    break;
                case "be":
                    name = "скрыты";
                    break;
                case "et":
                    name = "on peidetud";
                    break;
                case "fr":
                    name = "est caché";
                    break;
                case "kk":
                    name = "жасырылған";
                    break;
                case "ky":
                    name = "жашыруун";
                    break;
                case "lt":
                    name = "yra paslėpta";
                    break;
                case "lv":
                    name = "ir paslēpts";
                    break;
                case "ro":
                    name = "este ascuns";
                    break;
                case "tg":
                    name = "пинҳон аст";
                    break;
                case "tk":
                    name = "gizlenendir";
                    break;
                case "uk":
                    name = "прихований";
                    break;
                case "uz":
                    name = "yashiringan";
                    break;
                case "es":
                    name = "Está oculto";
                    break;
                case "pt":
                    name = "está escondido";
                    break;
                case "id":
                    name = "tersembunyi";
                    break;
                case "it":
                    name = "è nascosto";
                    break;
                case "de":
                    name = "ist versteckt";
                    break;
                default:
                    name = "---";
                    break;
            }
            return name;
        }
    }
}