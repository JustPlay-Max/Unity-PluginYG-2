using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Net;
#if NJSON_YG2
using Newtonsoft.Json.Linq;
#endif
#if TMP_YG2
using TMPro;
#endif

namespace YG.LanguageLegacy
{
    public class LanguageYG : MonoBehaviour
    {
#if TMP_YG2
        public TMP_Text textMPComponent;
        public TMP_FontAsset uniqueFontTMP;
#endif
        public Text textLComponent;
        public InfoYG.AutoTranslateLangsSettings info;
        [Space(10)]
        public string text;
        public string ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz, es, pt, ar, id, ja, it, de, hi;
        public bool changeOnlyFont;
        public int fontNumber;
        public Font uniqueFont;
        public LangYGAdditionalText additionalText;
        int baseFontSize;

        private void Awake()
        {
            // Uncomment the bottom line if you get any errors related to infoYG. In some cases, it may help.
            //Serialize();

            info = YG2.infoYG.AutoTranslateLangs;

            if (textLComponent)
                baseFontSize = textLComponent.fontSize;
#if TMP_YG2
            else if (textMPComponent)
                baseFontSize = Mathf.RoundToInt(textMPComponent.fontSize);
#endif
        }

        [ContextMenu("Reserialize")]
        public void Serialize()
        {
            info = YG2.infoYG.AutoTranslateLangs;
            textLComponent = GetComponent<Text>();
#if TMP_YG2
            textMPComponent = GetComponent<TMP_Text>();
#endif
            additionalText = GetComponent<LangYGAdditionalText>();
        }

        private void OnEnable()
        {
            SwitchLanguage();
            YG2.onSwitchLang += SwitchLanguage;
        }

        private void OnDisable() => YG2.onSwitchLang -= SwitchLanguage;

        public void SwitchLanguage(string lang)
        {
            for (int i = 0; i < languages.Length; i++)
            {
                if (lang == UtilsLang.LangName(i))
                {
                    if (!changeOnlyFont)
                        AssignTranslate(languages[i]);

                    if (textLComponent)
                        ChangeFont(UtilsLang.GetFont(i, info));
#if TMP_YG2
                    else if (textMPComponent)
                        ChangeFont(UtilsLang.GetFontTMP(i, info));
#endif

                    FontSizeCorrect(UtilsLang.GetFontSize(i, info));

                    if (additionalText != null)
                        additionalText.AssignAdditionalText(this);

                }
            }
        }

        public void SwitchLanguage()
        {
            SwitchLanguage(YG2.lang);
        }

        void AssignTranslate(string translation)
        {
            if (textLComponent)
                textLComponent.text = translation;
#if TMP_YG2
            else if (textMPComponent)
                textMPComponent.text = translation;
#endif
        }

        public void AssignTranslate()
        {
            for (int i = 0; i < languages.Length; i++)
            {
                if (YG2.lang == UtilsLang.LangName(i))
                {
                    AssignTranslate(languages[i]);
                    break;
                }
            }
        }

        public void ChangeFont(Font[] fontArray)
        {
            Font font;

            if (fontArray.Length >= fontNumber + 1 && fontArray[fontNumber])
            {
                font = fontArray[fontNumber];
            }
            else font = null;

            if (uniqueFont)
            {
                font = uniqueFont;
            }
            else if (font == null)
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

            if (font != null)
            {
                textLComponent.font = font;
            }
        }

#if TMP_YG2
        public void ChangeFont(TMP_FontAsset[] fontArray)
        {
            TMP_FontAsset font;

            if (fontArray.Length >= fontNumber + 1 && fontArray[fontNumber])
            {
                font = fontArray[fontNumber];
            }
            else font = null;

            if (uniqueFontTMP)
            {
                font = uniqueFontTMP;
            }
            else if (font == null)
            {
                if (info.fontsTMP.defaultFont.Length >= fontNumber + 1 && info.fontsTMP.defaultFont[fontNumber])
                {
                    font = info.fontsTMP.defaultFont[fontNumber];
                }
                else if (info.fontsTMP.defaultFont.Length >= 1 && info.fontsTMP.defaultFont[0])
                {
                    font = info.fontsTMP.defaultFont[0];
                }
            }

            if (font != null)
            {
                textMPComponent.font = font;
            }
        }
#endif

        void FontSizeCorrect(int[] fontSizeArray)
        {
            if (textLComponent)
                textLComponent.fontSize = baseFontSize;
#if TMP_YG2
            else if (textMPComponent)
                textMPComponent.fontSize = baseFontSize;
#endif
            if (fontSizeArray.Length != 0 && fontSizeArray.Length - 1 >= fontNumber)
            {
                if (textLComponent)
                    textLComponent.fontSize += fontSizeArray[fontNumber];
#if TMP_YG2
                else if (textMPComponent)
                    textMPComponent.fontSize += fontSizeArray[fontNumber];
#endif
            }
        }

        public string[] languages
        {
            get
            {
                string[] s = new string[27];

                s[0] = ru;
                s[1] = en;
                s[2] = tr;
                s[3] = az;
                s[4] = be;
                s[5] = he;
                s[6] = hy;
                s[7] = ka;
                s[8] = et;
                s[9] = fr;
                s[10] = kk;
                s[11] = ky;
                s[12] = lt;
                s[13] = lv;
                s[14] = ro;
                s[15] = tg;
                s[16] = tk;
                s[17] = uk;
                s[18] = uz;
                s[19] = es;
                s[20] = pt;
                s[21] = ar;
                s[22] = id;
                s[23] = ja;
                s[24] = it;
                s[25] = de;
                s[26] = hi;

                return s;
            }
            set
            {
                ru = value[0];
                en = value[1];
                tr = value[2];
                az = value[3];
                be = value[4];
                he = value[5];
                hy = value[6];
                ka = value[7];
                et = value[8];
                fr = value[9];
                kk = value[10];
                ky = value[11];
                lt = value[12];
                lv = value[13];
                ro = value[14];
                tg = value[15];
                tk = value[16];
                uk = value[17];
                uz = value[18];
                es = value[19];
                pt = value[20];
                ar = value[21];
                id = value[22];
                ja = value[23];
                it = value[24];
                de = value[25];
                hi = value[26];
            }
        }

#if UNITY_EDITOR
        [HideInInspector] public float textHeight = 20f;
        [HideInInspector] public string processTranslateLabel;
        [HideInInspector] public bool componentTextField;

        public void SetLang(int index, string text)
        {
            string[] str = languages;
            str[index] = text;

            languages = str;
        }

        public void Translate(int countLangAvailable)
        {
            RunTranslateEmptyFields(countLangAvailable);
        }

        string TranslateGoogle(string translationTo = "en")
        {
            string text;

            if (!componentTextField)
                text = this.text;
            else if (textLComponent)
                text = textLComponent.text;
#if TMP_YG2
            else if (textMPComponent)
                text = textMPComponent.text;
#endif
            else
            {
                Debug.LogError("(The text for translation was not found!");
                return null;
            }
#if NJSON_YG2
            var url = String.Format("https://translate.google." + info.domainAutoLocalization + "/translate_a/single?client=gtx&dt=t&sl={0}&tl={1}&q={2}",
                "auto", translationTo, WebUtility.UrlEncode(text));
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SendWebRequest();
            while (!www.isDone)
            {

            }
            string response = www.downloadHandler.text;

            try
            {
                JArray jsonArray = JArray.Parse(response);
                response = jsonArray[0][0][0].ToString();
            }
            catch
            {
                response = "process error";
                StopAllCoroutines();
                processTranslateLabel = processTranslateLabel + " error";

                Debug.LogError("The process is not completed! Most likely, you made too many requests. In this case, the Google Translate API blocks access to the translation for a while.  Please try again later. Do not translate the text too often, so that Google does not consider your actions as spam");
            }

            return response;
#else
#if RU_YG2
            Debug.LogError($"Для авто локализации требуется импортировать пакет Newtonsoft JSON. Сделать это можно в настройках {InfoYG.NAME_PLUGIN}.");
#else
            Debug.LogError($"For auto localization, you need to import the Newtonsoft JSON package. You can do this in the settings {InfoYG.NAME_PLUGIN}.");
#endif
            return text;
#endif
        }

        [HideInInspector] public int countLang = 0;

        private void RunTranslateEmptyFields(int countLangAvailable)
        {
            countLang = 0;
            processTranslateLabel = "processing... 0/" + countLangAvailable;

            for (int i = 0; i < languages.Length; i++)
            {
                if (UtilsLang.LangIsActive()[i] && (languages[i] == null || languages[i] == ""))
                {
                    string translate = TranslateGoogle(UtilsLang.LangName(i));

                    if (translate == null)
                        return;

                    SetLang(i, translate);

                    if (processTranslateLabel.Contains("error"))
                        processTranslateLabel = countLang + "/" + countLangAvailable + " error";
                    else
                    {
                        processTranslateLabel = countLang + "/" + countLangAvailable;
                    }

                    countLang++;
                }
            }

            processTranslateLabel = countLang + "/" + countLangAvailable + " completed";
        }
#endif
    }
}
