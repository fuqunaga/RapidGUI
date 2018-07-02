using System.Collections.Generic;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        #region Style

        static GUIStyle _colorStyle;
        static GUIStyle colorStyle
        {
            get
            {
                if (_colorStyle == null)
                {
                    _colorStyle = new GUIStyle(GUIStyle.none);
                    _colorStyle.normal.background = Texture2D.whiteTexture;
                    _colorStyle.margin = new RectOffset(5, 5, 5, 5);
                }
                return _colorStyle;
            }
        }

        #endregion

        static readonly Dictionary<string, string> _colorLabelTable = new Dictionary<string, string>()
        {
            { "x","h" },
            { "y","s" },
            { "z","v" },
            { "w","a" },
        };

        static object ColorField(object obj)
        {
            var color = (Color)obj;
            var a = color.a;

            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(color.r, color.g, color.b, 1f);
            {
                var rgbSize = 20f;
                GUILayout.Box("", colorStyle, GUILayout.Width(rgbSize), GUILayout.Height(rgbSize));
                //if (Event.current.type == EventType.Repaint)
                {
                    var aSize = 7f;
                    var rect = GUILayoutUtility.GetLastRect();
                    rect.x += rect.width - aSize;
                    rect.y += rect.height - aSize;
                    rect.width = aSize;
                    rect.height = aSize;

                    GUI.backgroundColor = new Color(a, a, a, 1f);
                    GUI.Box(rect, "", colorStyle);
                }

            }
            GUI.backgroundColor = bgColor;

            GUILayout.Space(8f);

            float h, s, v;
            UnityEngine.Color.RGBToHSV(color, out h, out s, out v);

            var hsva = (Vector4)RecursiveField(new Vector4(h, s, v, color.a), _colorLabelTable);

            color = UnityEngine.Color.HSVToRGB(hsva.x, hsva.y, hsva.z);
            color.a = hsva.w;

            return color;
        }
    }
}
