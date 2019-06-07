using UnityEngine;

namespace RapidGUI
{
    public static class RGUIStyle
    {
        public static GUIStyle popupButton;
        public static GUIStyle popup;
        public static GUIStyle darkWindow;

        // GUIStyleState.background will be null 
        // if it set after secound scene load and don't use a few frame
        // to keep textures, set it to other member. at unity2019
        public static Texture2D popupButtonTex;
        public static Texture2D popupTex;
        public static Texture2D darkWindowTexNormal;
        public static Texture2D darkWindowTexOnNormal;

        static RGUIStyle()
        {
            CreateStyles();
        }

        public static void CreateStyles()
        {
            CreateFlatButton();
            CreatePopup();
            darkWindow = CreateDarkWindow();
        }

        static void CreateFlatButton()
        {
            var style = new GUIStyle(GUI.skin.label);
            style.padding = new RectOffset(24, 48, 2, 2);

            var toggle = GUI.skin.toggle;
            style.normal.textColor = toggle.normal.textColor;
            style.hover.textColor = toggle.hover.textColor;

            popupButtonTex = new Texture2D(1, 1);
            popupButtonTex.SetPixels(new[] { new Color(0.5f, 0.5f, 0.5f, 0.5f) });
            popupButtonTex.Apply();
            style.hover.background = popupButtonTex;

            style.name = nameof(popupButton);
            popupButton = style;
        }

        static void CreatePopup()
        {
            var style = new GUIStyle(GUI.skin.box);
            style.border = new RectOffset();

            popupTex = new Texture2D(1, 1);
            var brightness = 0.2f;
            var alpha = 0.9f;
            popupTex.SetPixels(new[] { new Color(brightness, brightness, brightness, alpha) });
            popupTex.Apply();

            style.normal.background =
            style.hover.background = popupTex;

            style.name = nameof(popup);
            popup = style;
        }


        public static GUIStyle CreateDarkWindow()
        {
            var style = new GUIStyle(GUI.skin.window);

            style.normal.background = darkWindowTexNormal = CreateTexDark(style.normal.background, 0.5f, 1.4f);
            style.onNormal.background = darkWindowTexOnNormal = CreateTexDark(style.onNormal.background, 0.6f, 1.5f);

            style.name = nameof(darkWindow);

            return style;
        }

        public static Texture2D CreateTexDark(Texture2D src, float colorRate, float alphaRate)
        {
            var dst = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false);
            Graphics.CopyTexture(src, dst);


            var pixels = dst.GetPixels();
            for (var i = 0; i < pixels.Length; ++i)
            {
                var col = pixels[i];
                col.r *= colorRate;
                col.g *= colorRate;
                col.b *= colorRate;
                col.a *= alphaRate;

                pixels[i] = col;
            }

            dst.SetPixels(pixels);
            dst.Apply();

            return dst;
        }
    }
}
