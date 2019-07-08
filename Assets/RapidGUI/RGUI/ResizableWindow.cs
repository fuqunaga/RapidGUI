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
                Rect CalcDraggableRect(Rect r, float d)
                {
                    r.xMin -= d;
                    r.yMin -= d;
                    r.xMax += d;
                    r.yMax += d;
                    return r;
                }

                var evt = Event.current;
                var id = GUIUtility.GetControlID(FocusType.Passive);

                switch (evt.type)
                {
                    case EventType.MouseUp:
                        {
                            draggingLR = draggingTB = 0;
                            if (GUIUtility.hotControl == id) GUIUtility.hotControl = 0;
                        }
                        break;

                    case EventType.MouseDown:
                        {
                            if (GUIUtility.hotControl == 0)
                            {
                                var pos = evt.mousePosition;

                                var rect = CalcDraggableRect(window, detectionRange);

                                if (rect.Contains(pos))
                                {
                                    draggingLR = (pos.x < window.xMin) ? -1 : ((window.xMax < pos.x) ? 1 : 0);
                                    draggingTB = (pos.y < window.yMin) ? -1 : ((window.yMax < pos.y) ? 1 : 0);

                                    GUIUtility.hotControl = id;
                                }
                            }
                        }
                        break;

                    case EventType.MouseDrag:
                        if ((GUIUtility.hotControl == id) && (evt.button == 0))
                        {
                            var pos = evt.mousePosition;

                            if (draggingLR == -1) window.xMin = pos.x;
                            if (draggingLR == 1) window.xMax = pos.x;
                            if (draggingTB == -1) window.yMin = pos.y;
                            if (draggingTB == 1) window.yMax = pos.y;
                        }
                        break;

                    case EventType.Repaint:
                        {
                            var cursor = MouseCursor.Default;

                            if (GUIUtility.hotControl == 0)
                            {
                                var pos = evt.mousePosition;
                                var rect = CalcDraggableRect(window, detectionRange);
                                if (rect.Contains(pos) && !window.Contains(pos))
                                {
                                    var h = (pos.x < window.xMin) || (window.xMax < pos.x);
                                    var v = (pos.y < window.yMin) || (window.yMax < pos.y);

                                    cursor = h
                                         ? (v ? MouseCursor.ResizeUpLeft : MouseCursor.ResizeHorizontal)
                                         : (v ? MouseCursor.ResizeVertical : MouseCursor.Default);

                                }
                            }
                            else
                            {
                                cursor = (draggingLR != 0)
                                    ? ((draggingTB != 0) ? MouseCursor.ResizeUpLeft : MouseCursor.ResizeHorizontal)
                                    : ((draggingTB != 0) ? MouseCursor.ResizeVertical : MouseCursor.Default);
                            }

                            if (cursor != MouseCursor.Default)
                            {
                                RGUIUtility.SetCursor(cursor);
                            }
                        }
                        break;
                }

                return window;
            }
        }
    }
}