using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace RapidGUI
{
    public static partial class RGUI
    {
        static IMColorPicker colorPicker = null;
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

            using (new BackgroundColorScope(new Color(color.r, color.g, color.b, 1f)))
            {
                const int height = 16;
                const int alphaHeight = 3;

                if (GUILayout.Button("", Style.whiteRect, GUILayout.Height(height)))
                {
                    colorPicker = new IMColorPicker(color);
                    colorPicker.SetWindowPosition(colorPickerLastPos ?? GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
                }

                var rect = GUILayoutUtility.GetLastRect();
                rect.y = rect.yMax - alphaHeight;
                rect.height = alphaHeight;
                GUI.backgroundColor = Color.black;
                GUI.Box(rect, "", Style.whiteRect);

                rect.width *= color.a;
                GUI.backgroundColor = Color.white;
                GUI.Box(rect, "", Style.whiteRect);
            }


            if (colorPicker != null)
            {
                WindowInvoker.Add(colorPicker);

                if (colorPicker.destroy)
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



        #region Style

        static class Style
        {
            public readonly static GUIStyle whiteRect;

            static Style()
            {
                whiteRect = new GUIStyle(GUIStyle.none);
                whiteRect.normal.background = Texture2D.whiteTexture;
                whiteRect.margin = new RectOffset(5, 5, 5, 5);
            }
        }

        #endregion

    }
}
