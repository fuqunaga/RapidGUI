using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        public static class Style
        {
            public static GUIStyle flatButton;
            public static GUIStyle popup;

            static Style()
            {
                CreateFlatButton();
                CreatePopup();
            }

            static void CreateFlatButton()
            {
                var style = new GUIStyle("label");
                style.padding = new RectOffset(24, 6, 2, 2);

                var toggle = GUI.skin.toggle;
                style.normal.textColor = toggle.normal.textColor;
                style.hover.textColor = toggle.hover.textColor;

                var tex = new Texture2D(1, 1);
                tex.SetPixels(new[] { new Color(0.5f, 0.5f, 0.5f, 0.5f) });
                tex.Apply();
                style.hover.background = tex;

                flatButton = style;
            }

            static void CreatePopup()
            {
                var style = new GUIStyle("box");
                style.stretchHeight = true;

                var tex = new Texture2D(1, 1);
                var brightness = 0.2f;
                var alpha = 0.9f;
                tex.SetPixels(new[] { new Color(brightness, brightness, brightness, alpha) });
                tex.Apply();

                style.normal.background =
                style.hover.background = tex;

                popup = style;
            }
        }
    }
}