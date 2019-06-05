using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        public static class Style
        {
            public static GUIStyle popupButton;
            public static GUIStyle popup;
            public static GUIStyle darkWindow;

            static Style()
            {
                CreateFlatButton();
                CreatePopup();
                CreateDarkWindow();
            }

            static void CreateFlatButton()
            {
                var style = new GUIStyle("label");                
                style.padding = new RectOffset(24, 48, 2, 2);

                var toggle = GUI.skin.toggle;
                style.normal.textColor = toggle.normal.textColor;
                style.hover.textColor = toggle.hover.textColor;

                var tex = new Texture2D(1, 1);
                tex.SetPixels(new[] { new Color(0.5f, 0.5f, 0.5f, 0.5f) });
                tex.Apply();
                style.hover.background = tex;

                style.name = nameof(popupButton);
                popupButton = style;
            }

            static void CreatePopup()
            {
                var style = new GUIStyle("box");
                style.border = new RectOffset();

                var tex = new Texture2D(1, 1);
                var brightness = 0.2f;
                var alpha = 0.9f;
                tex.SetPixels(new[] { new Color(brightness, brightness, brightness, alpha) });
                tex.Apply();

                style.normal.background =
                style.hover.background = tex;

                style.name = nameof(popup);
                popup = style;
            }


            static void CreateDarkWindow()
            {
                var style = new GUIStyle(GUI.skin.window);

                style.normal.background = CreateTexDark(style.normal.background, 0.6f, 1.1f);
                style.onNormal.background = CreateTexDark(style.onNormal.background, 0.6f, 1.4f);

                style.name = nameof(darkWindow);
                darkWindow = style;
            }

            static Texture2D CreateTexDark(Texture2D src, float colorRate, float alphaRate)
            {
                var dst = new Texture2D(src.width, src.height, src.format, false);
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
}