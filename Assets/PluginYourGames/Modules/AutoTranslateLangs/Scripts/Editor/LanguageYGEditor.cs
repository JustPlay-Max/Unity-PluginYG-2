using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
#if TMP_YG2
using TMPro;
#endif

namespace YG.LanguageLegacy
{
    [CustomEditor(typeof(LanguageYG))]
    public class LanguageYGEditor : Editor
    {
        LanguageYG scr;

        GUIStyle red;
        GUIStyle green;

        int processTranslateLabel;

        private void OnEnable()
        {
            scr = (LanguageYG)target;
            scr.Serialize();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            scr = (LanguageYG)target;
            Undo.RecordObject(scr, "Undo LanguageYG");

            red = new GUIStyle(EditorStyles.label);
            red.normal.textColor = Color.red;
            green = new GUIStyle(EditorStyles.label);
            green.normal.textColor = Color.green;

            bool isNullTextComponent = false;

#if TMP_YG2
            if (scr.textLComponent == null && scr.textMPComponent == null)
                isNullTextComponent = true;
#else
            if (scr.textLComponent == null)
                isNullTextComponent = true;
#endif

            if (isNullTextComponent)
            {
                if (GUILayout.Button("Add component - Text Legasy", GUILayout.Height(23)))
                {
                    scr.textLComponent = scr.gameObject.AddComponent<Text>();
                    Undo.RecordObject(scr.textLComponent, "Undo textUIComponent");
                }
#if TMP_YG2
                if (GUILayout.Button("Add component - Text Mesh Pro UGUI", GUILayout.Height(23)))
                {
                    scr.textMPComponent = scr.gameObject.AddComponent<TextMeshProUGUI>();
                }
#endif
                return;
            }


            GUILayout.BeginVertical("HelpBox");

            scr.componentTextField = EditorGUILayout.ToggleLeft("Component Text/TextMeshPro Translate", scr.componentTextField);
            scr.textHeight = EditorGUILayout.Slider("Text Height", scr.textHeight, 20f, 400f);

            if (!scr.componentTextField)
                scr.text = EditorGUILayout.TextArea(scr.text, GUILayout.Height(scr.textHeight));
            else
            {
                if (scr.textLComponent)
                    GUILayout.Label(scr.textLComponent.text);
#if TMP_YG2
                else if (scr.textMPComponent)
                    GUILayout.Label(scr.textMPComponent.text);
#endif
            }

            GUILayout.BeginHorizontal();

            if (scr.componentTextField)
            {
                if (scr.textLComponent)
                {
                    if (scr.textLComponent.text.Length > 0)
                    {
                        GUILayout.Label("Text Component", green);

                        if (GUILayout.Button("TRANSLATE"))
                            TranslateButton();
                    }
                    else
                        GUILayout.Label("Text Component", red);
                }
#if TMP_YG2
                else if (scr.textMPComponent)
                {
                    if (scr.textMPComponent.text != null && scr.textMPComponent.text.Length > 0)
                    {
                        GUILayout.Label("TextMeshPro Component", green);

                        if (GUILayout.Button("TRANSLATE"))
                            TranslateButton();
                    }
                    else
                        GUILayout.Label("TextMeshPro Component", red);
                }
#endif
            }
            else
            {
                if (scr.componentTextField || (scr.text == null || scr.text.Length == 0))
                {
                    GUILayout.Label("FILL IN THE FIELD", red);
                }
                else if (GUILayout.Button("TRANSLATE"))
                    TranslateButton();
            }

            if (GUILayout.Button("CLEAR"))
            {
                scr.ru = "";
                scr.en = "";
                scr.tr = "";
                scr.az = "";
                scr.be = "";
                scr.he = "";
                scr.hy = "";
                scr.ka = "";
                scr.et = "";
                scr.fr = "";
                scr.kk = "";
                scr.ky = "";
                scr.lt = "";
                scr.lv = "";
                scr.ro = "";
                scr.tg = "";
                scr.tk = "";
                scr.uk = "";
                scr.uz = "";
                scr.es = "";
                scr.pt = "";
                scr.ar = "";
                scr.id = "";
                scr.ja = "";
                scr.it = "";
                scr.de = "";
                scr.hi = "";

                scr.processTranslateLabel = "";
                scr.countLang = processTranslateLabel;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();

            bool labelProcess = false;

            if (scr.processTranslateLabel != "")
            {
                if (scr.countLang == processTranslateLabel)
                {
                    GUILayout.Label(scr.processTranslateLabel, green, GUILayout.Height(20));
                    labelProcess = true;
                }
                else if (scr.processTranslateLabel == "")
                {
                    labelProcess = true;
                }
                else
                {
                    GUILayout.Label(scr.processTranslateLabel, GUILayout.Height(20));
                    labelProcess = true;
                }

                try
                {
                    if (scr.processTranslateLabel.Contains("error"))
                    {
                        GUILayout.Label(scr.processTranslateLabel, red, GUILayout.Height(20));
                        labelProcess = true;
                    }
                }
                catch
                {
                }
            }

            if (labelProcess == false)
                GUILayout.Label(processTranslateLabel + " Languages", GUILayout.Height(20));

            GUILayout.EndHorizontal();

            UpdateLanguages(false);
            GUILayout.EndVertical();

            if (scr.textLComponent
#if TMP_YG2
                || scr.textMPComponent
#endif
                )
            {
                GUILayout.Space(10);
                GUILayout.BeginVertical("box");

                if (scr.textLComponent)
                {
                    scr.uniqueFont = (Font)EditorGUILayout.ObjectField("Unique Font", scr.uniqueFont, typeof(Font), false);
                    FontSettingsDraw();
                }
#if TMP_YG2
                else if (scr.textMPComponent)
                {
                    scr.uniqueFontTMP = (TMP_FontAsset)EditorGUILayout.ObjectField("Unique Font", scr.uniqueFontTMP, typeof(TMP_FontAsset), false);
                    FontTMPSettingsDraw();
                }
#endif
                GUILayout.EndVertical();
            }

            if (scr.additionalText != null)
                scr.additionalText = (LangYGAdditionalText)EditorGUILayout.ObjectField("Additional Text", scr.additionalText, typeof(LangYGAdditionalText), false);


            if (GUI.changed)
            {
                EditorUtility.SetDirty(scr.gameObject);
                EditorSceneManager.MarkSceneDirty(scr.gameObject.scene);
            }
        }

        readonly string buttonText_ReplaseFont = "Replace the font with the standard one";

        void FontSettingsDraw()
        {
            if (scr.info.fonts.defaultFont.Length == 0)
                return;

            scr.fontNumber = Mathf.Clamp(scr.fontNumber, 0, scr.info.fonts.defaultFont.Length - 1);
            if (scr.info.fonts.defaultFont.Length > 1)
                scr.fontNumber = EditorGUILayout.IntField("Font Index (in array default fonts)", scr.fontNumber);

            if (scr.textLComponent.font == scr.info.fonts.defaultFont[scr.fontNumber])
                return;

            if (GUILayout.Button(buttonText_ReplaseFont))
            {
                Undo.RecordObject(scr.textLComponent, "Undo TextLComponent.font");
                scr.textLComponent.font = scr.info.fonts.defaultFont[scr.fontNumber];
            }
        }

#if TMP_YG2
        void FontTMPSettingsDraw()
        {
            if (scr.info.fontsTMP.defaultFont.Length == 0)
                return;

            scr.fontNumber = Mathf.Clamp(scr.fontNumber, 0, scr.info.fontsTMP.defaultFont.Length - 1);
            if (scr.info.fontsTMP.defaultFont.Length > 1)
                scr.fontNumber = EditorGUILayout.IntField("Font Index (in array default fonts)", scr.fontNumber);

            if (scr.textMPComponent.font == scr.info.fontsTMP.defaultFont[scr.fontNumber])
                return;

            if (GUILayout.Button(buttonText_ReplaseFont))
            {
                Undo.RecordObject(scr.textMPComponent, "Undo TextMPComponent.font");
                scr.textMPComponent.font = scr.info.fontsTMP.defaultFont[scr.fontNumber];
            }
        }
#endif
        void TranslateButton()
        {
            scr.processTranslateLabel = "";
            scr.Translate(processTranslateLabel);
        }

        void UpdateLanguages(bool CSVFile)
        {
            processTranslateLabel = 0;
            bool[] langArr = UtilsLang.LangIsActive();

            for (int i = 0; i < langArr.Length; i++)
            {
                if (langArr[i])
                {
                    processTranslateLabel++;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent(UtilsLang.LangName(i), FullNameLanguages()[i]), GUILayout.Width(20), GUILayout.Height(20));

                    if (i == 0) scr.ru = EditorGUILayout.TextArea(scr.ru, GUILayout.Height(scr.textHeight));
                    else if (i == 1) scr.en = EditorGUILayout.TextArea(scr.en, GUILayout.Height(scr.textHeight));
                    else if (i == 2) scr.tr = EditorGUILayout.TextArea(scr.tr, GUILayout.Height(scr.textHeight));
                    else if (i == 3) scr.az = EditorGUILayout.TextArea(scr.az, GUILayout.Height(scr.textHeight));
                    else if (i == 4) scr.be = EditorGUILayout.TextArea(scr.be, GUILayout.Height(scr.textHeight));
                    else if (i == 5) scr.he = EditorGUILayout.TextArea(scr.he, GUILayout.Height(scr.textHeight));
                    else if (i == 6) scr.hy = EditorGUILayout.TextArea(scr.hy, GUILayout.Height(scr.textHeight));
                    else if (i == 7) scr.ka = EditorGUILayout.TextArea(scr.ka, GUILayout.Height(scr.textHeight));
                    else if (i == 8) scr.et = EditorGUILayout.TextArea(scr.et, GUILayout.Height(scr.textHeight));
                    else if (i == 9) scr.fr = EditorGUILayout.TextArea(scr.fr, GUILayout.Height(scr.textHeight));
                    else if (i == 10) scr.kk = EditorGUILayout.TextArea(scr.kk, GUILayout.Height(scr.textHeight));
                    else if (i == 11) scr.ky = EditorGUILayout.TextArea(scr.ky, GUILayout.Height(scr.textHeight));
                    else if (i == 12) scr.lt = EditorGUILayout.TextArea(scr.lt, GUILayout.Height(scr.textHeight));
                    else if (i == 13) scr.lv = EditorGUILayout.TextArea(scr.lv, GUILayout.Height(scr.textHeight));
                    else if (i == 14) scr.ro = EditorGUILayout.TextArea(scr.ro, GUILayout.Height(scr.textHeight));
                    else if (i == 15) scr.tg = EditorGUILayout.TextArea(scr.tg, GUILayout.Height(scr.textHeight));
                    else if (i == 16) scr.tk = EditorGUILayout.TextArea(scr.tk, GUILayout.Height(scr.textHeight));
                    else if (i == 17) scr.uk = EditorGUILayout.TextArea(scr.uk, GUILayout.Height(scr.textHeight));
                    else if (i == 18) scr.uz = EditorGUILayout.TextArea(scr.uz, GUILayout.Height(scr.textHeight));
                    else if (i == 19) scr.es = EditorGUILayout.TextArea(scr.es, GUILayout.Height(scr.textHeight));
                    else if (i == 20) scr.pt = EditorGUILayout.TextArea(scr.pt, GUILayout.Height(scr.textHeight));
                    else if (i == 21) scr.ar = EditorGUILayout.TextArea(scr.ar, GUILayout.Height(scr.textHeight));
                    else if (i == 22) scr.id = EditorGUILayout.TextArea(scr.id, GUILayout.Height(scr.textHeight));
                    else if (i == 23) scr.ja = EditorGUILayout.TextArea(scr.ja, GUILayout.Height(scr.textHeight));
                    else if (i == 24) scr.it = EditorGUILayout.TextArea(scr.it, GUILayout.Height(scr.textHeight));
                    else if (i == 25) scr.de = EditorGUILayout.TextArea(scr.de, GUILayout.Height(scr.textHeight));
                    else if (i == 26) scr.hi = EditorGUILayout.TextArea(scr.hi, GUILayout.Height(scr.textHeight));

                    GUILayout.EndHorizontal();
                }
            }

            string[] FullNameLanguages()
            {
                string[] s = new string[27];

                s[0] = "RUSSIAN";
                s[1] = "ENGLISH";
                s[2] = "TURKISH";
                s[3] = "AZERBAIJANIAN";
                s[4] = "BELARUSIAN";
                s[5] = "HEBREW";
                s[6] = "ARMENIAN";
                s[7] = "GEORGIAN";
                s[8] = "ESTONIAN";
                s[9] = "FRENCH";
                s[10] = "KAZAKH";
                s[11] = "KYRGYZ";
                s[12] = "LITHUANIAN";
                s[13] = "LATVIAN";
                s[14] = "ROMANIAN";
                s[15] = "TAJICK";
                s[16] = "TURKMEN";
                s[17] = "UKRAINIAN";
                s[18] = "UZBEK";
                s[19] = "SPANISH";
                s[20] = "PORTUGUESE";
                s[21] = "ARABIAN";
                s[22] = "INDONESIAN";
                s[23] = "JAPANESE";
                s[24] = "ITALIAN";
                s[25] = "GERMAN";
                s[26] = "HINDI";

                return s;
            }
        }
    }
}
