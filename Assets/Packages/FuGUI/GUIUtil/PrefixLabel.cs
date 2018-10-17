using System;
using System.Collections.Generic;
using UnityEngine;


namespace FuGUI
{
    public static partial class GUIUtil
    {
        public static float prefixLabelWidth
        {
            get { return _prefisLabelWidth; }
            set
            {
                _prefisLabelWidth = value;
                labelWidthLayout = GUILayout.Width(_prefisLabelWidth);
            }
        }

        public static bool isLabelRightAlign;

        static float _prefisLabelWidth = 128f;
        static GUILayoutOption labelWidthLayout = GUILayout.Width(_prefisLabelWidth);


        public static void PrefixLabel(string label)
        {
            if (!string.IsNullOrEmpty(label))
            {
                var style = isLabelRightAlign ? labelRight : GUI.skin.label;

                GUILayout.Label(label, style, labelWidthLayout);
            }
        }

        public static object PrefixLabelDraggable(string label, object obj, Type type)
        {
            if (!string.IsNullOrEmpty(label))
            {
                PrefixLabel(label);
                if (IsDraggable(type))
                {
                    obj = DoDrag(obj, type);
                }
            }

            return obj;
        }


        static Vector2 lastMousePos;

        static object DoDrag(object obj, Type type)
        {
            var controlID = GUIUtility.GetControlID(FocusType.Passive);

            var rect = GUILayoutUtility.GetLastRect();

            var ev = Event.current;
            var etype = ev.GetTypeForControl(controlID);

            switch(etype)
            {
                case EventType.MouseDown:
                    {
                        if ((ev.button == 0) && rect.Contains(ev.mousePosition))
                        {
                            GUIUtility.hotControl = controlID;
                            lastMousePos = ev.mousePosition;
                            ev.Use();
                        }
                    }
                    break;

                case EventType.MouseUp:
                    {
                        if (GUIUtility.hotControl == controlID)
                            GUIUtility.hotControl = 0;
                    }
                    break;

                case EventType.MouseDrag:
                    {
                        if ((ev.button == 0) && (GUIUtility.hotControl == controlID))
                        {
                            var diff = ev.mousePosition - lastMousePos;
                            var add = (Mathf.Abs(diff.x) > Mathf.Abs(diff.y)) ? diff.x : diff.y;
                            add = Math.Sign(add);

                            lastMousePos = ev.mousePosition;
                            if (typeof(int) == type)
                            {
                                var v = (int)obj;
                                v += (int)(add);
                                obj = v;
                            }
                            else if (typeof(float) == type)
                            {
                                var scale = 0.03f;
                                var v = (float)obj;
                                v += add * scale;
                                v = Mathf.Floor(v * 100f) * 0.01f; // chop
                                obj = v;
                            }


                            ev.Use();
                        }
                        
                    }
                    break;
            }

            return obj;
        }


        

        public static bool IsDraggable(Type type)
        {
            return (
                (typeof(int) == type)  ||
                (typeof(float)) == type
                );
        }
    }
}