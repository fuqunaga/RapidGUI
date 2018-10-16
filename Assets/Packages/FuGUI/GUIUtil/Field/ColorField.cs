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

        static IMColorPicker colorPicker;

        static object ColorField(object obj)
        {
            var color = (Color)obj;

            var bgColor = GUI.backgroundColor;
            {
                var controlID = GUIUtility.GetControlID(FocusType.Passive);

                const int height = 20;
                const int alphaHeight = 3;
                GUI.backgroundColor = new Color(color.r, color.g, color.b, 1f);
                if (GUILayout.Button("", colorStyle, GUILayout.Height(height)))
                {
                    GUIUtility.hotControl = controlID;
                    colorPicker = new IMColorPicker(color);
                }

                var rect = GUILayoutUtility.GetLastRect();
                rect.y = rect.yMax - alphaHeight;
                rect.height = alphaHeight;
                GUI.backgroundColor = UnityEngine.Color.black;
                GUI.Box(rect, "", colorStyle);

                rect.width *= color.a;
                GUI.backgroundColor = UnityEngine.Color.white;
                GUI.Box(rect, "", colorStyle);
            }
            GUI.backgroundColor = bgColor;


            if (colorPicker != null)
            {
                var destroy = colorPicker.DrawWindow();
                if (destroy) colorPicker = null;
                else color = colorPicker.color;
            }

            return color;
        }
    }
}
