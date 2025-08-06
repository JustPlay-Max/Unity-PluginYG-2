using System;
using UnityEngine;
#if TMP_YG2
using TMPro;
#endif

namespace YG
{
    public partial class InfoYG : ScriptableObject
    {
        public AutoTranslateLangsSettings AutoTranslateLangs = new AutoTranslateLangsSettings();

        [Serializable]
        public partial class AutoTranslateLangsSettings
        {
            [Tooltip("Домен с которого будет скачиваться перевод. Если у вас возникли проблемы с авто-переводом, попробуйте поменять домен.")]
            public string domainAutoLocalization = "com";

            #region LanguagesEnumeration
            [Serializable]
            public class Languages
            {
                [Tooltip("RUSSIAN")] public bool ru = true;
                [Tooltip("ENGLISH")] public bool en = true;
                [Tooltip("TURKISH")] public bool tr = true;
                [Tooltip("AZERBAIJANIAN")] public bool az;
                [Tooltip("BELARUSIAN")] public bool be;
                [Tooltip("HEBREW")] public bool he;
                [Tooltip("ARMENIAN")] public bool hy;
                [Tooltip("GEORGIAN")] public bool ka;
                [Tooltip("ESTONIAN")] public bool et;
                [Tooltip("FRENCH")] public bool fr;
                [Tooltip("KAZAKH")] public bool kk;
                [Tooltip("KYRGYZ")] public bool ky;
                [Tooltip("LITHUANIAN")] public bool lt;
                [Tooltip("LATVIAN")] public bool lv;
                [Tooltip("ROMANIAN")] public bool ro;
                [Tooltip("TAJICK")] public bool tg;
                [Tooltip("TURKMEN")] public bool tk;
                [Tooltip("UKRAINIAN")] public bool uk;
                [Tooltip("UZBEK")] public bool uz;
                [Tooltip("SPANISH")] public bool es;
                [Tooltip("PORTUGUESE")] public bool pt;
                [Tooltip("ARABIAN")] public bool ar;
                [Tooltip("INDONESIAN")] public bool id;
                [Tooltip("JAPANESE")] public bool ja;
                [Tooltip("ITALIAN")] public bool it;
                [Tooltip("GERMAN")] public bool de;
                [Tooltip("HINDI")] public bool hi;
            }

            [Tooltip("Выберите языки, на которые будет переведена Ваша игра.")]
            public Languages languages = new Languages();

            [Serializable]
            public class Fonts
            {
                [Tooltip("Стандартный шрифт")] public Font[] defaultFont = new Font[0];
                [Tooltip("RUSSIAN")] public Font[] ru = new Font[0];
                [Tooltip("ENGLISH")] public Font[] en = new Font[0];
                [Tooltip("TURKISH")] public Font[] tr = new Font[0];
                [Tooltip("AZERBAIJANIAN")] public Font[] az = new Font[0];
                [Tooltip("BELARUSIAN")] public Font[] be = new Font[0];
                [Tooltip("HEBREW")] public Font[] he = new Font[0];
                [Tooltip("ARMENIAN")] public Font[] hy = new Font[0];
                [Tooltip("GEORGIAN")] public Font[] ka = new Font[0];
                [Tooltip("ESTONIAN")] public Font[] et = new Font[0];
                [Tooltip("FRENCH")] public Font[] fr = new Font[0];
                [Tooltip("KAZAKH")] public Font[] kk = new Font[0];
                [Tooltip("KYRGYZ")] public Font[] ky = new Font[0];
                [Tooltip("LITHUANIAN")] public Font[] lt = new Font[0];
                [Tooltip("LATVIAN")] public Font[] lv = new Font[0];
                [Tooltip("ROMANIAN")] public Font[] ro = new Font[0];
                [Tooltip("TAJICK")] public Font[] tg = new Font[0];
                [Tooltip("TURKMEN")] public Font[] tk = new Font[0];
                [Tooltip("UKRAINIAN")] public Font[] uk = new Font[0];
                [Tooltip("UZBEK")] public Font[] uz = new Font[0];
                [Tooltip("SPANISH")] public Font[] es = new Font[0];
                [Tooltip("PORTUGUESE")] public Font[] pt = new Font[0];
                [Tooltip("ARABIAN")] public Font[] ar = new Font[0];
                [Tooltip("INDONESIAN")] public Font[] id = new Font[0];
                [Tooltip("JAPANESE")] public Font[] ja = new Font[0];
                [Tooltip("ITALIAN")] public Font[] it = new Font[0];
                [Tooltip("GERMAN")] public Font[] de = new Font[0];
                [Tooltip("HINDI")] public Font[] hi = new Font[0];
            }

            [Tooltip("Здесь вы можете выбрать одельные шрифты для каждого языка.")]
            public Fonts fonts = new Fonts();

#if TMP_YG2
            [Serializable]
            public class FontsTMP
            {
                [Tooltip("Стандартный шрифт")] public TMP_FontAsset[] defaultFont = new TMP_FontAsset[0];
                [Tooltip("RUSSIAN")] public TMP_FontAsset[] ru = new TMP_FontAsset[0];
                [Tooltip("ENGLISH")] public TMP_FontAsset[] en = new TMP_FontAsset[0];
                [Tooltip("TURKISH")] public TMP_FontAsset[] tr = new TMP_FontAsset[0];
                [Tooltip("AZERBAIJANIAN")] public TMP_FontAsset[] az = new TMP_FontAsset[0];
                [Tooltip("BELARUSIAN")] public TMP_FontAsset[] be = new TMP_FontAsset[0];
                [Tooltip("HEBREW")] public TMP_FontAsset[] he = new TMP_FontAsset[0];
                [Tooltip("ARMENIAN")] public TMP_FontAsset[] hy = new TMP_FontAsset[0];
                [Tooltip("GEORGIAN")] public TMP_FontAsset[] ka = new TMP_FontAsset[0];
                [Tooltip("ESTONIAN")] public TMP_FontAsset[] et = new TMP_FontAsset[0];
                [Tooltip("FRENCH")] public TMP_FontAsset[] fr = new TMP_FontAsset[0];
                [Tooltip("KAZAKH")] public TMP_FontAsset[] kk = new TMP_FontAsset[0];
                [Tooltip("KYRGYZ")] public TMP_FontAsset[] ky = new TMP_FontAsset[0];
                [Tooltip("LITHUANIAN")] public TMP_FontAsset[] lt = new TMP_FontAsset[0];
                [Tooltip("LATVIAN")] public TMP_FontAsset[] lv = new TMP_FontAsset[0];
                [Tooltip("ROMANIAN")] public TMP_FontAsset[] ro = new TMP_FontAsset[0];
                [Tooltip("TAJICK")] public TMP_FontAsset[] tg = new TMP_FontAsset[0];
                [Tooltip("TURKMEN")] public TMP_FontAsset[] tk = new TMP_FontAsset[0];
                [Tooltip("UKRAINIAN")] public TMP_FontAsset[] uk = new TMP_FontAsset[0];
                [Tooltip("UZBEK")] public TMP_FontAsset[] uz = new TMP_FontAsset[0];
                [Tooltip("SPANISH")] public TMP_FontAsset[] es = new TMP_FontAsset[0];
                [Tooltip("PORTUGUESE")] public TMP_FontAsset[] pt = new TMP_FontAsset[0];
                [Tooltip("ARABIAN")] public TMP_FontAsset[] ar = new TMP_FontAsset[0];
                [Tooltip("INDONESIAN")] public TMP_FontAsset[] id = new TMP_FontAsset[0];
                [Tooltip("JAPANESE")] public TMP_FontAsset[] ja = new TMP_FontAsset[0];
                [Tooltip("ITALIAN")] public TMP_FontAsset[] it = new TMP_FontAsset[0];
                [Tooltip("GERMAN")] public TMP_FontAsset[] de = new TMP_FontAsset[0];
                [Tooltip("HINDI")] public TMP_FontAsset[] hi = new TMP_FontAsset[0];
            }

            [Tooltip("Здесь вы можете выбрать одельные шрифты TextMeshPro для каждого языка.")]
            public FontsTMP fontsTMP = new FontsTMP();
#endif

            [Serializable]
            public class FontsSizeCorrect
            {
                [Tooltip("RUSSIAN")] public int[] ru = new int[0];
                [Tooltip("ENGLISH")] public int[] en = new int[0];
                [Tooltip("TURKISH")] public int[] tr = new int[0];
                [Tooltip("AZERBAIJANIAN")] public int[] az = new int[0];
                [Tooltip("BELARUSIAN")] public int[] be = new int[0];
                [Tooltip("HEBREW")] public int[] he = new int[0];
                [Tooltip("ARMENIAN")] public int[] hy = new int[0];
                [Tooltip("GEORGIAN")] public int[] ka = new int[0];
                [Tooltip("ESTONIAN")] public int[] et = new int[0];
                [Tooltip("FRENCH")] public int[] fr = new int[0];
                [Tooltip("KAZAKH")] public int[] kk = new int[0];
                [Tooltip("KYRGYZ")] public int[] ky = new int[0];
                [Tooltip("LITHUANIAN")] public int[] lt = new int[0];
                [Tooltip("LATVIAN")] public int[] lv = new int[0];
                [Tooltip("ROMANIAN")] public int[] ro = new int[0];
                [Tooltip("TAJICK")] public int[] tg = new int[0];
                [Tooltip("TURKMEN")] public int[] tk = new int[0];
                [Tooltip("UKRAINIAN")] public int[] uk = new int[0];
                [Tooltip("UZBEK")] public int[] uz = new int[0];
                [Tooltip("SPANISH")] public int[] es = new int[0];
                [Tooltip("PORTUGUESE")] public int[] pt = new int[0];
                [Tooltip("ARABIAN")] public int[] ar = new int[0];
                [Tooltip("INDONESIAN")] public int[] id = new int[0];
                [Tooltip("JAPANESE")] public int[] ja = new int[0];
                [Tooltip("ITALIAN")] public int[] it = new int[0];
                [Tooltip("GERMAN")] public int[] de = new int[0];
                [Tooltip("HINDI")] public int[] hi = new int[0];
            }

            [Tooltip("Вы можете скорректировать размер шрифта для каждого языка. Допустим, для Японского языка вы можете указать -3. В таком случае, если бы базовый размер был бы, например, 10, то для японского языка он бы стал равен 7.")]
            public FontsSizeCorrect fontsSizeCorrect = new FontsSizeCorrect();
            #endregion LanguagesEnumeration
        }
    }
}
