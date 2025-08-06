namespace YG.EditorScr
{
    using UnityEngine;
    using UnityEditor;
    using YG.EditorScr.BuildModify;

    [InitializeOnLoad]
    public static class YGEditorStyles
    {
        private static double nextExecutionTime;

        private static GUIStyle _selectable;
        private static GUIStyle _deselectable;
        private static GUIStyle _box;
        private static GUIStyle _boxLight;
        private static GUIStyle _error;
        private static GUIStyle _button;
        private static GUIStyle _debutton;
        private static GUIStyle _warning;

        public static GUIStyle selectable
        {
            get
            {
                if (_selectable == null)
                    _selectable = Selectable();
                return _selectable;
            }
        }
        public static GUIStyle deselectable
        {
            get
            {
                if (_deselectable == null)
                    _deselectable = Deselectable();
                return _deselectable;
            }
        }

        public static GUIStyle box
        {
            get
            {
                if (_box == null)
                    _box = Box();
                return _box;
            }
        }

        public static GUIStyle boxLight
        {
            get
            {
                if (_boxLight == null)
                    _boxLight = BoxLight();
                return _boxLight;
            }
        }

        public static GUIStyle error
        {
            get
            {
                if (_error == null)
                    _error = Error();
                return _error;
            }
        }

        public static GUIStyle warning
        {
            get
            {
                if (_warning == null)
                    _warning = Warning();
                return _warning;
            }
        }

        public static GUIStyle button
        {
            get
            {
                if (_button == null)
                    _button = Button();
                return _button;
            }
        }

        public static GUIStyle debutton
        {
            get
            {
                if (_debutton == null)
                    _debutton = Debutton();
                return _debutton;
            }
        }

        static YGEditorStyles()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.update += UpdateStyles;
            ModifyBuild.onModifyComplete += ReinitializeStyles;
        }

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

        private static void UpdateStyles()
        {
            if (EditorApplication.isPlaying && Time.unscaledTime < 10)
            {
                ReinitializeStyles();
            }
            else if (EditorApplication.timeSinceStartup >= nextExecutionTime)
            {
                nextExecutionTime = EditorApplication.timeSinceStartup + 3;
                ReinitializeStyles();
            }
        }

        public static GUIStyle Selectable()
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);

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
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);

            Color normalColor = new Color(1f, 1f, 1f, 0.07f);
            Color hoverColor = new Color(1f, 0.5f, 0f, 0.3f);

            style.normal.background = MakeTexUnderlineLeft(normalColor);
            return style;
        }

        public static GUIStyle Box()
        {
            GUIStyle style;
            Color color;

            if (EditorGUIUtility.isProSkin)
            {
                style = new GUIStyle(EditorStyles.helpBox);
                color = new Color(0f, 0f, 0f, 0.2f);
            }
            else
            {
                style = new GUIStyle();
                color = new Color(1f, 1f, 1f, 0.5f);
            }

            style.normal.background = MakeTex(color);
            style.hover.background = MakeTex(color);
            style.active.background = MakeTex(color);
            style.focused.background = MakeTex(color);

            return style;
        }

        public static GUIStyle BoxLight()
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);

            if (EditorGUIUtility.isProSkin)
            {
                Color color = new Color(1f, 1f, 1f, 0.05f);

                style.normal.background = MakeTex(color);
                style.hover.background = MakeTex(color);
                style.active.background = MakeTex(color);
                style.focused.background = MakeTex(color);

                style.border = new RectOffset(23, 23, 23, 23);
            }
            else
            {
                style = new GUIStyle(EditorStyles.helpBox);
            }

            return style;
        }

        public static GUIStyle Error()
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);
            Color color = new Color(1f, 0f, 0f, 0.18f);

            style.normal.background = MakeTex(color);
            style.hover.background = MakeTex(color);
            style.active.background = MakeTex(color);
            style.focused.background = MakeTex(color);

            return style;
        }

        public static GUIStyle Warning()
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);
            Color color = new Color(1f, 0.6f, 0f, 0.25f);

            style.normal.background = MakeTex(color);
            style.hover.background = MakeTex(color);
            style.active.background = MakeTex(color);
            style.focused.background = MakeTex(color);

            return style;
        }

        public static GUIStyle Button()
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);

            if (EditorGUIUtility.isProSkin)
            {
                Color hoverColor = new Color(1f, 0.5f, 0f, 0.5f);

                style.normal.background = MakeTexUnderline(new Color(1f, 1f, 1f, 0.2f));
                style.hover.background = MakeTexUnderline(hoverColor);
                style.active.background = MakeTexUnderline(new Color(1f, 0.5f, 0f, 1f));
                style.focused.background = MakeTexUnderline(hoverColor);

                style.normal.textColor = Color.white;
                style.hover.textColor = Color.white;
                style.active.textColor = Color.white;
                style.focused.textColor = Color.white;

                style.fontSize = 12;
                style.alignment = TextAnchor.MiddleCenter;
            }
            else
            {
                style = new GUIStyle(GUI.skin.button);
            }

            return style;
        }

        public static GUIStyle Debutton()
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);

            if (EditorGUIUtility.isProSkin)
            {
                style.normal.background = MakeTexUnderline(new Color(1f, 1f, 1f, 0.2f));
                style.normal.textColor = Color.white;
                style.fontSize = 12;
                style.alignment = TextAnchor.MiddleCenter;
            }
            else
            {
                style = new GUIStyle(GUI.skin.button);
            }

            return style;
        }

        private static Texture2D MakeTex(Color col)
        {
            Color[] pix = new Color[1] { col };
            Texture2D result = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            result.SetPixels(pix);
            result.Apply(true);
            return result;
        }

        private static Texture2D MakeTexUnderline(Color color)
        {
            int squareSize = 16;
            int width = squareSize + 1;

            Texture2D result = new Texture2D(width, width, TextureFormat.ARGB32, false);

            Color[] pixels = new Color[width * width];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Color orange = new Color(1f, 0.5f, 0f, 1f);
            pixels[7] = orange;
            pixels[8] = orange;
            pixels[9] = orange;

            result.SetPixels(pixels);
            result.Apply(true);
            return result;
        }

        private static Texture2D MakeTexUnderlineLeft(Color color)
        {
            int squareSize = 16;
            int width = squareSize + 1;

            Texture2D result = new Texture2D(width, width, TextureFormat.ARGB32, false);

            Color[] pixels = new Color[width * width];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Color orange = new Color(1f, 0.5f, 0f, 1f);

            for (int i = 0; i < 6; i++)
                pixels[i] = orange;

            result.SetPixels(pixels);
            result.Apply(true);
            return result;
        }
    }
}