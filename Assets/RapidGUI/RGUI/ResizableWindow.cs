using System.Collections.Generic;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        public static Rect ResizableWindow(int id, Rect rect, GUI.WindowFunction func, string text, GUIStyle style = null, params GUILayoutOption[] options)
        {
            return ResizableWindow_.DoWindow(id, rect, func, text, style, options);
        }

        class ResizableWindow_
        {
            #region static

            const int detectionRange = 8;
            static readonly RectOffset overflow = new RectOffset(detectionRange, detectionRange, 0, detectionRange);
            static GUIStyle defaultStyle;

            static Dictionary<GUIStyle, GUIStyle> customStyleDic = new Dictionary<GUIStyle, GUIStyle>();

            static ResizableWindow_()
            {
                defaultStyle = new GUIStyle(GUI.skin.window);
                defaultStyle.overflow = overflow;
            }

            protected static GUIStyle CheckStyle(GUIStyle style)
            {
                GUIStyle ret = null;
                if (style == null)
                {
                    ret = defaultStyle;
                }
                else if (style.overflow != overflow)
                {
                    if (customStyleDic.TryGetValue(style, out var customStyle))
                    {
                        ret = customStyle;
                    }
                    else
                    {
                        ret = new GUIStyle(style);
                        ret.overflow = overflow;
                        customStyleDic[style] = ret;
                    }
                }

                return ret;
            }

            protected static Dictionary<int, ResizableWindow_> table = new Dictionary<int, ResizableWindow_>();

            public static Rect DoWindow(int id, Rect rect, GUI.WindowFunction func, string text, GUIStyle style = null, params GUILayoutOption[] options)
            {
                if (!table.TryGetValue(id, out var window))
                {
                    table[id] = window = new ResizableWindow_();
                }
                return window.Do(id, rect, func, text, style, options);
            }

            #endregion


            int draggingLR;
            int draggingTB;

            protected Rect Do(int id, Rect rect, GUI.WindowFunction func, string text, GUIStyle style = null, params GUILayoutOption[] options)
            {
                rect = ResizeRect(rect, detectionRange);
                return GUILayout.Window(id, rect, func, text, CheckStyle(style), options);
            }

            Rect ResizeRect(Rect window, float detectionRange)
            {
                Event current = Event.current;

                if (current.type == EventType.MouseUp)
                {
                    draggingLR = draggingTB = 0;
                }
                else if (current.type == EventType.MouseDown)
                {
                    Rect resizer = window;
                    var pos = current.mousePosition;

                    if (draggingLR == 0 && draggingTB == 0)
                    {
                        resizer.xMin -= detectionRange;
                        resizer.yMin -= detectionRange;
                        resizer.xMax += detectionRange;
                        resizer.yMax += detectionRange;

                        if (resizer.Contains(current.mousePosition))
                        {
                            draggingLR = (pos.x < window.xMin) ? -1 : ((window.xMax < pos.x) ? 1 : 0);
                            draggingTB = (pos.y < window.yMin) ? -1 : ((window.yMax < pos.y) ? 1 : 0);
                        }
                    }
                }
                else if (current.type == EventType.MouseDrag && current.button == 0)
                {
                    var pos = current.mousePosition;

                    if (draggingLR == -1) window.xMin = pos.x;
                    if (draggingLR == 1) window.xMax = pos.x;
                    if (draggingTB == -1) window.yMin = pos.y;
                    if (draggingTB == 1) window.yMax = pos.y;
                }

                return window;
            }
        }
    }
}