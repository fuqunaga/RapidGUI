using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        public static class Style
        {
            public static GUIStyle popupButton;
            public static GUIStyle popup;

            static Style()
            {
                CreateFlatButton();
                CreatePopup();
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
        }
    }
}