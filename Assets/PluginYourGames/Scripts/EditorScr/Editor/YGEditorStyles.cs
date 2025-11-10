// YGEditorStyles.cs
// Ѕезопасна€ работа со стил€ми во врем€ перекомпил€ции/без активного GUI skin

namespace YG.EditorScr
{
    using UnityEngine;
    using UnityEditor;
    using YG.EditorScr.BuildModify;

    [InitializeOnLoad]
    public static class YGEditorStyles
    {
        // кэш
        private static GUIStyle _selectable;
        private static GUIStyle _deselectable;
        private static GUIStyle _box;
        private static GUIStyle _boxLight;
        private static GUIStyle _error;
        private static GUIStyle _button;
        private static GUIStyle _debutton;
        private static GUIStyle _warning;

        // безопасна€ проверка, можно ли строить стили на базе EditorStyles/GUIskin
        static bool CanBuildGUI =>
            GUI.skin != null &&
            Event.current != null &&
            !EditorApplication.isCompiling &&
            !EditorApplication.isUpdating;

        static YGEditorStyles()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            ModifyBuild.onModifyComplete += ReinitializeStyles;
        }

        [InitializeOnLoadMethod]
        static void AfterReload() => ReinitializeStyles();

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            ReinitializeStyles();
        }

        public static void ReinitializeStyles()
        {
            _selectable = null;
            _deselectable = null;
            _box = null;
            _boxLight = null;
            _error = null;
            _button = null;
            _debutton = null;
            _warning = null;
        }

        // универсальный безопасный геттер
        static GUIStyle GetOrMake(ref GUIStyle cache, System.Func<GUIStyle> factory)
        {
            if (cache != null) return cache;
            cache = CanBuildGUI ? factory() : new GUIStyle();
            return cache;
        }

        // публичные стили
        public static GUIStyle selectable => GetOrMake(ref _selectable, Selectable);
        public static GUIStyle deselectable => GetOrMake(ref _deselectable, Deselectable);
        public static GUIStyle box => GetOrMake(ref _box, Box);
        public static GUIStyle boxLight => GetOrMake(ref _boxLight, BoxLight);
        public static GUIStyle error => GetOrMake(ref _error, Error);
        public static GUIStyle warning => GetOrMake(ref _warning, Warning);
        public static GUIStyle button => GetOrMake(ref _button, Button);
        public static GUIStyle debutton => GetOrMake(ref _debutton, Debutton);

        // фабрика безопасного базового helpBox
        static GUIStyle BaseHelpBox()
        {
            return (GUI.skin != null ? new GUIStyle(EditorStyles.helpBox) : new GUIStyle());
        }

        // фабрики стилей
        public static GUIStyle Selectable()
        {
            var style = BaseHelpBox();

            Color normalColor = new Color(1f, 1f, 1f, 0.07f);
            Color hoverColor = new Color(1f, 0.5f, 0f, 0.3f);

            style.normal.background = MakeTexUnderlineLeft(normalColor);
            style.hover.background = MakeTexUnderlineLeft(hoverColor);
            style.active.background = MakeTexUnderlineLeft(hoverColor);
            style.focused.background = MakeTexUnderlineLeft(hoverColor);
            return style;
        }

        public static GUIStyle Deselectable()
        {
            var style = BaseHelpBox();
            Color normalColor = new Color(1f, 1f, 1f, 0.07f);
            style.normal.background = MakeTexUnderlineLeft(normalColor);
            return style;
        }

        public static GUIStyle Box()
        {
            GUIStyle style;
            Color color;

            if (GUI.skin != null && EditorGUIUtility.isProSkin)
            {
                style = BaseHelpBox();
                color = new Color(0f, 0f, 0f, 0.2f);
            }
            else
            {
                style = new GUIStyle();
                color = new Color(1f, 1f, 1f, 0.5f);
            }

            var tex = MakeTex(color);
            style.normal.background = tex;
            style.hover.background = tex;
            style.active.background = tex;
            style.focused.background = tex;
            return style;
        }

        public static GUIStyle BoxLight()
        {
            var style = BaseHelpBox();

            if (GUI.skin != null && EditorGUIUtility.isProSkin)
            {
                Color color = new Color(1f, 1f, 1f, 0.05f);

                var tex = MakeTex(color);
                style.normal.background = tex;
                style.hover.background = tex;
                style.active.background = tex;
                style.focused.background = tex;

                style.border = new RectOffset(23, 23, 23, 23);
            }

            return style;
        }

        public static GUIStyle Error()
        {
            var style = BaseHelpBox();
            Color color = new Color(1f, 0f, 0f, 0.18f);

            var tex = MakeTex(color);
            style.normal.background = tex;
            style.hover.background = tex;
            style.active.background = tex;
            style.focused.background = tex;

            return style;
        }

        public static GUIStyle Warning()
        {
            var style = BaseHelpBox();
            Color color = new Color(1f, 0.6f, 0f, 0.25f);

            var tex = MakeTex(color);
            style.normal.background = tex;
            style.hover.background = tex;
            style.active.background = tex;
            style.focused.background = tex;

            return style;
        }

        public static GUIStyle Button()
        {
            // дл€ лайт-скина отдаЄм системную кнопку, дл€ про Ч кастом
            if (!(GUI.skin != null && EditorGUIUtility.isProSkin))
                return new GUIStyle(GUI.skin != null ? GUI.skin.button : new GUIStyle());

            var style = BaseHelpBox();
            var hover = new Color(1f, 0.5f, 0f, 0.5f);

            style.normal.background = MakeTexUnderline(new Color(1f, 1f, 1f, 0.2f));
            style.hover.background = MakeTexUnderline(hover);
            style.active.background = MakeTexUnderline(new Color(1f, 0.5f, 0f, 1f));
            style.focused.background = MakeTexUnderline(hover);

            style.normal.textColor = Color.white;
            style.hover.textColor = Color.white;
            style.active.textColor = Color.white;
            style.focused.textColor = Color.white;

            style.fontSize = 12;
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }

        public static GUIStyle Debutton()
        {
            if (!(GUI.skin != null && EditorGUIUtility.isProSkin))
                return new GUIStyle(GUI.skin != null ? GUI.skin.button : new GUIStyle());

            var style = BaseHelpBox();
            style.normal.background = MakeTexUnderline(new Color(1f, 1f, 1f, 0.2f));
            style.normal.textColor = Color.white;
            style.fontSize = 12;
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }

        // генераци€ текстур (с флагами, чтобы не плодить ресурсы между reload)
        private static Texture2D MakeTex(Color col)
        {
            var result = new Texture2D(1, 1, TextureFormat.ARGB32, false)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            result.SetPixel(0, 0, col);
            result.Apply(true);
            return result;
        }

        private static Texture2D MakeTexUnderline(Color color)
        {
            int w = 17;
            var result = new Texture2D(w, w, TextureFormat.ARGB32, false)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            var pixels = new Color[w * w];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = color;

            var orange = new Color(1f, 0.5f, 0f, 1f);
            pixels[7] = orange; pixels[8] = orange; pixels[9] = orange;

            result.SetPixels(pixels);
            result.Apply(true);
            return result;
        }

        private static Texture2D MakeTexUnderlineLeft(Color color)
        {
            int w = 17;
            var result = new Texture2D(w, w, TextureFormat.ARGB32, false)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            var pixels = new Color[w * w];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = color;

            var orange = new Color(1f, 0.5f, 0f, 1f);
            for (int i = 0; i < 6; i++) pixels[i] = orange;

            result.SetPixels(pixels);
            result.Apply(true);
            return result;
        }
    }
}
