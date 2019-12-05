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

        static object ColorField(object obj)
        {
            var color = (Color)obj;

#if UNITY_EDITOR
            if (RGUILayoutUtility.IsInEditorWindow())
            {
                var ret = EditorGUILayout.ColorField(color);
                return ret;
            }
#endif
            var id = GUIUtility.GetControlID(FocusType.Passive);

            using (new BackgroundColorScope(new Color(color.r, color.g, color.b, 1f)))
            {
                const int height = 20;
                const int alphaHeight = 3;

                // color bar
                if (GUILayout.Button("", Style.whiteRect, GUILayout.Height(height)))
                {
                    colorPicker = new IMColorPicker(color);
                    colorPicker.SetWindowPosition(colorPickerLastPos ?? GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
                    colorPickerControlId = id;
                }

                var rect = GUILayoutUtility.GetLastRect();
                
                // label
                Color.RGBToHSV(color, out var h, out var s, out var v);
                var yuv_y = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
                using (new ColorScope(yuv_y > 0.7f ? Color.black : Color.white))
                {
                    GUI.Label(rect, $" HSV {h:0.00} {s:0.00} {v:0.00}");
                }

                // alpha bar background(black)
                rect.y = rect.yMax - alphaHeight;
                rect.height = alphaHeight;
                GUI.backgroundColor = Color.black;
                GUI.Box(rect, "", Style.whiteRect);

                // alpha bar
                rect.width *= color.a;
                GUI.backgroundColor = Color.white;
                GUI.Box(rect, "", Style.whiteRect);
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
            public static readonly GUIStyle whiteRect;

            static Style()
            {
                whiteRect = new GUIStyle(GUIStyle.none)
                {
                    normal = {background = Texture2D.whiteTexture},
                    margin = new RectOffset(0, 5, 0, 0)
                };
            }
        }

        #endregion

    }
}
