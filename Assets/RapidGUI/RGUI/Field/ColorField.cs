using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
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

        static int colorPickerControlID = -1;
        static IMColorPicker colorPicker;
        static Vector2? colorPickerLastPos;

        static object ColorField(object obj)
        {
            var color = (Color)obj;
            var controlID = GUIUtility.GetControlID(FocusType.Passive);

            var bgColor = GUI.backgroundColor;
            {
                const int height = 20;
                const int alphaHeight = 3;
                GUI.backgroundColor = new Color(color.r, color.g, color.b, 1f);
                if (GUILayout.Button("", colorStyle, GUILayout.Height(height)))
                {
                    colorPickerControlID = controlID;
                    colorPicker = new IMColorPicker(color);
                    colorPicker.SetWindowPosition(colorPickerLastPos ?? Event.current.mousePosition);
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


            if ((colorPickerControlID == controlID) && (colorPicker != null))
            {
                var destroy = colorPicker.DrawWindow();
                if (destroy)
                {
                    colorPicker = null;
                }
                else
                {
                    color = colorPicker.color;
                    colorPickerLastPos = colorPicker.windowRect.position;
                }
            }

            return color;
        }
    }
}
