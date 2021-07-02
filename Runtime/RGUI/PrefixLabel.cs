using System;
using UnityEngine;


namespace RapidGUI
{
    public static partial class RGUI
    {
        public static class PrefixLabelSetting
        {
            public static float width = 130f;
        }

        public static bool PrefixLabel(string label)
        {
            var isLong = false;

            if (!string.IsNullOrEmpty(label))
            {
                var style = GUI.skin.label;
                isLong = PrefixLabelSetting.width > 0f && style.CalcSize(RGUIUtility.TempContent(label)).x > PrefixLabelSetting.width;

                if (isLong)
                {
                    GUILayout.Label(label);
                }
                else
                {
                    GUILayout.Label(label, GUILayout.Width(PrefixLabelSetting.width));
                }
            }

            return isLong;
        }

        //public static object PrefixLabelDraggable(string label, object obj, Type type) => PrefixLabelDraggable(label, obj, type, out var _);

        public static object PrefixLabelDraggable(string label, object obj, Type type, out bool isLong)
        {
            isLong = false;
            if (!string.IsNullOrEmpty(label))
            {
                isLong = PrefixLabel(label);
                if (IsDraggable(type))
                {
                    obj = DoDrag(obj, type);
                }
            }

            return obj;
        }


        #region implement drag

        static Vector2 lastMousePos;
        static readonly int DoDragHash = "DoDrag".GetHashCode();

        static object DoDrag(object obj, Type type)
        {
            var controlId = GUIUtility.GetControlID(DoDragHash, FocusType.Passive);

            var rect = GUILayoutUtility.GetLastRect();

            var ev = Event.current;
            var evType = ev.GetTypeForControl(controlId);

            switch (evType)
            {
                case EventType.MouseDown:
                {
                    if ((ev.button == RapidGUIBehaviour.Instance.prefixLabelSlideButton) &&
                        rect.Contains(ev.mousePosition))
                    {
                        GUIUtility.hotControl = controlId;
                        lastMousePos = ev.mousePosition;

                        RGUIUtility.SetCursor(MouseCursor.ResizeHorizontal);
                        ev.Use();
                    }
                }
                break;

                case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                        ev.Use();
                    }
                }
                break;

                case EventType.MouseDrag:
                {
                    if ((ev.button == RapidGUIBehaviour.Instance.prefixLabelSlideButton) &&
                        (GUIUtility.hotControl == controlId))
                    {
                        var diff = ev.mousePosition - lastMousePos;
                        var add = (Mathf.Abs(diff.x) > Mathf.Abs(diff.y)) ? diff.x : diff.y;
                        add = Math.Sign(add);

                        lastMousePos = ev.mousePosition;
                        if (typeof(int) == type)
                        {
                            var v = (int) obj;
                            v += (int) (add);
                            obj = v;
                        }
                        else if (typeof(float) == type)
                        {
                            var scale = 0.03f;
                            var v = (float) obj;
                            v += add * scale;
                            v = Mathf.Floor(v * 100f) * 0.01f; // chop
                            obj = v;
                        }

                        ev.Use();
                    }
                }
                break;
                
                case EventType.Repaint:
                {
                    if (GUIUtility.hotControl == controlId)
                    {
                        RGUIUtility.SetCursor(MouseCursor.ResizeHorizontal);
                    }
                }
                break;
            }

            return obj;
        }


        public static bool IsDraggable(Type type)
        {
            return (
                (typeof(int) == type) ||
                (typeof(float)) == type
            );
        }
    }

    #endregion
}