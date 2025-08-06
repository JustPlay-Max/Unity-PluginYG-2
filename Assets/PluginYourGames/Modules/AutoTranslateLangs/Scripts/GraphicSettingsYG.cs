using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace YG.LanguageLegacy
{
    public class GraphicSettingsYG : MonoBehaviour
    {
        public Dropdown dropdown;
        public Text labelText;
        public Text itemText;
        public int fontNumber;
        [Space(5)]
        [Header("Translate")]
        public string[] ru = new string[6];
        public string[] en = new string[6];
        public string[] tr = new string[6];
        public string[] az = new string[6];
        public string[] be = new string[6];
        public string[] he = new string[6];
        public string[] hy = new string[6];
        public string[] ka = new string[6];
        public string[] et = new string[6];
        public string[] fr = new string[6];
        public string[] kk = new string[6];
        public string[] ky = new string[6];
        public string[] lt = new string[6];
        public string[] lv = new string[6];
        public string[] ro = new string[6];
        public string[] tg = new string[6];
        public string[] tk = new string[6];
        public string[] uk = new string[6];
        public string[] uz = new string[6];
        public string[] es = new string[6];
        public string[] pt = new string[6];
        public string[] ar = new string[6];
        public string[] id = new string[6];
        public string[] ja = new string[6];
        public string[] it = new string[6];
        public string[] de = new string[6];
        public string[] hi = new string[6];

        private int labelBaseFontSize, itemBaseFontSize;
        private InfoYG.AutoTranslateLangsSettings info;

        public static Action onQualityChange;

        void Awake()
        {
            info = YG2.infoYG.AutoTranslateLangs;
            labelBaseFontSize = labelText.fontSize;
            itemBaseFontSize = itemText.fontSize;

            dropdown.ClearOptions();
            dropdown.AddOptions(QualitySettings.names.ToList());
            dropdown.value = QualitySettings.GetQualityLevel();
        }

        private void OnEnable() => YG2.onSwitchLang += SwitchLanguage;
        private void OnDisable() => YG2.onSwitchLang -= SwitchLanguage;
        private void Start() => SwitchLanguage(YG2.lang);

        public void SetQuality()
        {
            QualitySettings.SetQualityLevel(dropdown.value);
            onQualityChange?.Invoke();
        }

        private void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    labelText.text = ru[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.ru);
                    FontSizeCorrect(info.fontsSizeCorrect.ru);
                    for (int i = 0; i < ru.Length; i++)
                        dropdown.options[i].text = ru[i];
                    break;
                case "tr":
                    labelText.text = tr[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.tr);
                    FontSizeCorrect(info.fontsSizeCorrect.tr);
                    for (int i = 0; i < tr.Length; i++)
                        dropdown.options[i].text = tr[i];
                    break;
                case "en":
                    labelText.text = en[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.en);
                    FontSizeCorrect(info.fontsSizeCorrect.en);
                    for (int i = 0; i < en.Length; i++)
                        dropdown.options[i].text = en[i];
                    break;
                case "az":
                    labelText.text = az[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.az);
                    FontSizeCorrect(info.fontsSizeCorrect.az);
                    for (int i = 0; i < az.Length; i++)
                        dropdown.options[i].text = az[i];
                    break;
                case "be":
                    labelText.text = be[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.be);
                    FontSizeCorrect(info.fontsSizeCorrect.be);
                    for (int i = 0; i < be.Length; i++)
                        dropdown.options[i].text = be[i];
                    break;
                case "he":
                    labelText.text = he[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.he);
                    FontSizeCorrect(info.fontsSizeCorrect.he);
                    for (int i = 0; i < he.Length; i++)
                        dropdown.options[i].text = he[i];
                    break;
                case "hy":
                    labelText.text = hy[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.hy);
                    FontSizeCorrect(info.fontsSizeCorrect.hy);
                    for (int i = 0; i < hy.Length; i++)
                        dropdown.options[i].text = hy[i];
                    break;
                case "ka":
                    labelText.text = ka[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.ka);
                    FontSizeCorrect(info.fontsSizeCorrect.ka);
                    for (int i = 0; i < ka.Length; i++)
                        dropdown.options[i].text = ka[i];
                    break;
                case "et":
                    labelText.text = et[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.et);
                    FontSizeCorrect(info.fontsSizeCorrect.et);
                    for (int i = 0; i < et.Length; i++)
                        dropdown.options[i].text = et[i];
                    break;
                case "fr":
                    labelText.text = fr[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.fr);
                    FontSizeCorrect(info.fontsSizeCorrect.fr);
                    for (int i = 0; i < fr.Length; i++)
                        dropdown.options[i].text = fr[i];
                    break;
                case "kk":
                    labelText.text = kk[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.kk);
                    FontSizeCorrect(info.fontsSizeCorrect.kk);
                    for (int i = 0; i < kk.Length; i++)
                        dropdown.options[i].text = kk[i];
                    break;
                case "ky":
                    labelText.text = ky[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.ky);
                    FontSizeCorrect(info.fontsSizeCorrect.ky);
                    for (int i = 0; i < ky.Length; i++)
                        dropdown.options[i].text = ky[i];
                    break;
                case "lt":
                    labelText.text = lt[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.lt);
                    FontSizeCorrect(info.fontsSizeCorrect.lt);
                    for (int i = 0; i < lt.Length; i++)
                        dropdown.options[i].text = lt[i];
                    break;
                case "lv":
                    labelText.text = lv[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.lv);
                    FontSizeCorrect(info.fontsSizeCorrect.lv);
                    for (int i = 0; i < lv.Length; i++)
                        dropdown.options[i].text = lv[i];
                    break;
                case "ro":
                    labelText.text = ro[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.ro);
                    FontSizeCorrect(info.fontsSizeCorrect.ro);
                    for (int i = 0; i < ro.Length; i++)
                        dropdown.options[i].text = ro[i];
                    break;
                case "tg":
                    labelText.text = tg[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.tg);
                    FontSizeCorrect(info.fontsSizeCorrect.tg);
                    for (int i = 0; i < tg.Length; i++)
                        dropdown.options[i].text = tg[i];
                    break;
                case "tk":
                    labelText.text = tk[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.tk);
                    FontSizeCorrect(info.fontsSizeCorrect.tk);
                    for (int i = 0; i < tk.Length; i++)
                        dropdown.options[i].text = tk[i];
                    break;
                case "uk":
                    labelText.text = uk[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.uk);
                    FontSizeCorrect(info.fontsSizeCorrect.uk);
                    for (int i = 0; i < uk.Length; i++)
                        dropdown.options[i].text = uk[i];
                    break;
                case "uz":
                    labelText.text = uz[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.uz);
                    FontSizeCorrect(info.fontsSizeCorrect.uz);
                    for (int i = 0; i < uz.Length; i++)
                        dropdown.options[i].text = uz[i];
                    break;
                case "es":
                    labelText.text = es[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.es);
                    FontSizeCorrect(info.fontsSizeCorrect.es);
                    for (int i = 0; i < es.Length; i++)
                        dropdown.options[i].text = es[i];
                    break;
                case "pt":
                    labelText.text = pt[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.pt);
                    FontSizeCorrect(info.fontsSizeCorrect.pt);
                    for (int i = 0; i < pt.Length; i++)
                        dropdown.options[i].text = pt[i];
                    break;
                case "ar":
                    labelText.text = ar[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.ar);
                    FontSizeCorrect(info.fontsSizeCorrect.ar);
                    for (int i = 0; i < ar.Length; i++)
                        dropdown.options[i].text = ar[i];
                    break;
                case "id":
                    labelText.text = id[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.id);
                    FontSizeCorrect(info.fontsSizeCorrect.id);
                    for (int i = 0; i < id.Length; i++)
                        dropdown.options[i].text = id[i];
                    break;
                case "ja":
                    labelText.text = ja[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.ja);
                    FontSizeCorrect(info.fontsSizeCorrect.ja);
                    for (int i = 0; i < ja.Length; i++)
                        dropdown.options[i].text = ja[i];
                    break;
                case "it":
                    labelText.text = it[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.it);
                    FontSizeCorrect(info.fontsSizeCorrect.it);
                    for (int i = 0; i < it.Length; i++)
                        dropdown.options[i].text = it[i];
                    break;
                case "de":
                    labelText.text = de[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.de);
                    FontSizeCorrect(info.fontsSizeCorrect.en);
                    for (int i = 0; i < de.Length; i++)
                        dropdown.options[i].text = de[i];
                    break;
                case "hi":
                    labelText.text = hi[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.hi);
                    FontSizeCorrect(info.fontsSizeCorrect.hi);
                    for (int i = 0; i < hi.Length; i++)
                        dropdown.options[i].text = hi[i];
                    break;
                default:
                    labelText.text = en[QualitySettings.GetQualityLevel()];
                    SwithFont(info.fonts.en);
                    FontSizeCorrect(info.fontsSizeCorrect.en);
                    for (int i = 0; i < en.Length; i++)
                        dropdown.options[i].text = en[i];
                    break;
            }
        }

        private void SwithFont(Font[] fontArray)
        {
            if (fontArray.Length == 0)
                return;

            Font font = labelText.font;

            if (fontArray.Length >= fontNumber + 1 && fontArray[fontNumber])
            {
                font = fontArray[fontNumber];
            }
            else
            {
                if (info.fonts.defaultFont.Length >= fontNumber + 1 && info.fonts.defaultFont[fontNumber])
                {
                    font = info.fonts.defaultFont[fontNumber];
                }
                else if (info.fonts.defaultFont.Length >= 1 && info.fonts.defaultFont[0])
                {
                    font = info.fonts.defaultFont[0];
                }
            }

            labelText.font = font;
            itemText.font = font;
        }

        private void FontSizeCorrect(int[] fontSizeArray)
        {
            if (fontSizeArray.Length == 0)
                return;

            labelText.fontSize = labelBaseFontSize;
            itemText.fontSize = itemBaseFontSize;

            if (fontSizeArray.Length >= fontNumber - 1)
            {
                labelText.fontSize += fontSizeArray[fontNumber];
                itemText.fontSize += fontSizeArray[fontNumber];
            }
        }
    }
}
