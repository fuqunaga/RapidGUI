using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif


namespace RapidGUI
{
    public static partial class RGUI
    {
        static IMColorPicker colorPicker = null;
        static int colorPickerControlId;
        static Vector2? colorPickerLastPos;

        public static class ColorFieldSetting
        {
            public static Color labelColorLight = new Vector4(0.9f, 0.9f, 0.9f, 1.0f);
            public static Color labelColorDark = new Vector4(0.1f, 0.1f, 0.1f, 1.0f);
            public static int alphaBarHeight = 3;
        }


        static object ColorField(object obj)
        {
            void ColorBox(Rect r, Color col)
            {
                using (new BackgroundColorScope(col))
                {
                    GUI.Box(r, "", Style.whiteRect);
                }
            }
            
            var color = (Color) obj;
            
#if UNITY_EDITOR
            if (RGUILayoutUtility.IsInEditorWindow())
            {
                var ret = EditorGUILayout.ColorField(color);
                return ret;
            }
#endif

            var id = GUIUtility.GetControlID(FocusType.Passive);

            var rect = GUILayoutUtility.GetRect(new GUIContent(), GUI.skin.textField);

            // color bar
            ColorBox(rect, new Color(color.r, color.g, color.b, 1f));

            var alphaRect = rect;

            // alpha bar background(black)
            alphaRect.y = alphaRect.yMax - ColorFieldSetting.alphaBarHeight;
            alphaRect.height = ColorFieldSetting.alphaBarHeight;
            ColorBox(alphaRect, Color.black);

            // alpha bar
            alphaRect.width *= color.a;
            ColorBox(alphaRect, Color.white);

            // label
            Color.RGBToHSV(color, out var h, out var s, out var v);
            var yuvY = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
            var fontColor = yuvY >= 0.4f ? ColorFieldSetting.labelColorDark : ColorFieldSetting.labelColorLight;
            using (new ColorScope(fontColor))
            {
                GUI.Label(rect, $" HSV {h:0.00} {s:0.00} {v:0.00}");
            }

            // button
            using (new ColorScope(Color.clear))
            {
                if (GUI.Button(rect, "", Style.whiteRect))
                {
                    colorPicker = new IMColorPicker(color);
                    colorPicker.SetWindowPosition(colorPickerLastPos ?? RGUIUtility.GetMouseScreenPos());
                    colorPickerControlId = id;
                }
            }

            
            if ((colorPicker != null) && (colorPickerControlId == id))
            {
                WindowInvoker.Add(colorPicker);

                if (colorPicker.destroy)
                {
                    colorPicker = null;
                    colorPickerControlId = 0;
                }
                else
                {
                    color = colorPicker.color;
                    colorPickerLastPos = colorPicker.windowRect.position;
                }
            }

            return color;
        }


        #region Style

        static class Style
        {
            public static readonly GUIStyle colorField;
            public static readonly GUIStyle whiteRect;

            static Style()
            {
                var whiteTex = Texture2D.whiteTexture;
                colorField = new GUIStyle(GUIStyle.none)
                {
                    normal = {background = whiteTex},
                    //margin = new RectOffset(0, 4, 4, 4),
                    //padding = new RectOffset(6, 6, 4, 4)
                };

                whiteRect = new GUIStyle(GUIStyle.none)
                {
                    normal = {background = whiteTex},
                };
            }
        }

        #endregion
    }
}