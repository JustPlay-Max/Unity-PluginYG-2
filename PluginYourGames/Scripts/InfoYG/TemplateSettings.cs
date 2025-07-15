using System;
using UnityEngine;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        public TemplatesSettings Templates = new TemplatesSettings();

        [Serializable]
        public partial class TemplatesSettings
        {
#if UNITY_EDITOR
            public enum LogoImgFormat
            {
                [InspectorName("No Logo")] No,
                PNG, JPG, GIF
            }
            public enum BackgroundImageFormat
            {
                [InspectorName("Gradient")] Gradient,
                [InspectorName("Player Settings")] Unity,
                [InspectorName("No Background")] No,
                PNG, JPG, GIF
            }

            [Serializable]
            public class Gradient
            {
                [Tooltip(Langs.t_gradient_radial)]
                public bool radial;
                [Tooltip(Langs.t_gradient_angle)]
                [NestedYG(0, "!radial"), Range(0, 360)]
                public int angleInclination = 140;
                public Color color1 = new Color(1f, 0.831f, 0.149f, 1f);
                public Color color2 = new Color(0.36f, 0.09f, 1f, 1f);
            }

            [Serializable]
            public class ProgressBar
            {
                [Tooltip(Langs.t_progressBar_round)]
                public bool round = true;
                [Range(1, 20), NestedYG(nameof(round))]
                public int roundAngle = 10;
                [Range(1, 100), Tooltip(Langs.t_progressBar_width)]
                public int width = 30;
                [Range(1, 100)]
                public int height = 24;
                [Tooltip(Langs.t_progressBar_color1)]
                public Color color1 = new Color(0.25f, 0.6392f, 1f, 1f);
                [Tooltip(Langs.t_progressBar_color2)]
                public Color color2 = new Color(1f, 0.831f, 0.149f, 0.7f);
            }
#if RU_YG2
            [HeaderYG("Стандартный", 10)]
#else
            [HeaderYG("Default", 10)]
#endif
            [Tooltip(Langs.logoImageFormat)]
            public LogoImgFormat logoImageFormat;
            [Tooltip(Langs.backgroundImgFormat)]
            public BackgroundImageFormat backgroundImgFormat;

            [NestedYG(8, 12, "!backgroundImgFormat"), Tooltip(Langs.t_gradient)]
            public Gradient gradientBackgroundByLoadGame;

            [Space(10)]
            public bool customProgressBar = true;
            [Tooltip(Langs.t_customProgressBar), NestedYG(8, 12, nameof(customProgressBar))]
            public ProgressBar progressBarSettigs;

            [Tooltip(Langs.t_fixedAspectRatio)]
            public bool fixedAspectRatio;
            [NestedYG(nameof(fixedAspectRatio)), Tooltip(Langs.t_aspectRatio)]
            public string aspectRatio = "16/9";
            [NestedYG(nameof(fixedAspectRatio)), Tooltip(Langs.t_disableForMobile)]
            public bool disableForMobile = true;
            [NestedYG(nameof(fixedAspectRatio)), Tooltip(Langs.t_fillBackground)]
            public bool fillBackground;

            [Tooltip(Langs.t_imageBackground), NestedYG(nameof(fixedAspectRatio), nameof(fillBackground))]
            public bool imageBackground;
            [Tooltip(Langs.t_imageName), NestedYG(nameof(fixedAspectRatio), nameof(fillBackground), nameof(imageBackground))]
            public string imageName = "background.jpg";
            [Tooltip(Langs.t_gradient), NestedYG(8, 11, nameof(fixedAspectRatio), nameof(fillBackground), "!imageBackground")]
            public Gradient gradientBackgroundByAspectRatio;

            [Tooltip(Langs.t_pixelRatio)]
#endif
            public bool pixelRatioEnable;
#if UNITY_EDITOR
            [NestedYG(nameof(pixelRatioEnable)), Min(0)]
#endif
            public float pixelRatioValue = 1.3f;
#if UNITY_EDITOR
            [Tooltip(Langs.t_developerBuild)]
#endif
            public bool developerBuild;
        }
    }
}