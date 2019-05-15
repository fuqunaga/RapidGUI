using System;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static int popupControlID;
        static Rect popupRect;

        static readonly int popupWindowID = "Popup".GetHashCode();

        /// <summary>
        /// Popup
        /// func returns finish flag
        /// </summary>
        public static void Popup(Rect controlRect, int button, Vector2 size, Func<bool> func)
        {
            var controlID = GUIUtility.GetControlID(FocusType.Passive);

            // Don't Exist Active
            if (popupControlID == 0)
            {
                var ev = Event.current;
                var pos = ev.mousePosition;

                if ((ev.type == EventType.MouseUp) &&(ev.button == button) && controlRect.Contains(pos))
                {
                    pos.y += 15 + 6; //window will appear a little upper
                    popupRect = new Rect(pos, size);
                    popupControlID = controlID;
                    ev.Use();
                }
            }


            // Active
            else if (popupControlID == controlID)
            {
                GUI.WindowFunction funcWithCheckFinish = (id) =>
                {
                    var finish = func();

                    var ev = Event.current;

                    if (
                        finish
                        || ((ev.rawType == EventType.MouseDown) && !(new Rect(Vector2.zero, popupRect.size).Contains(ev.mousePosition)))
                    )
                    {
                        popupControlID = 0;
                        ev.Use();
                    }
                };

                GUI.ModalWindow(popupWindowID, popupRect, funcWithCheckFinish, "", Style.popup);
            }
        }
    }
}