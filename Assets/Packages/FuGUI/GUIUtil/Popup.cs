using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static int popupControlID;
        static Vector2 popupPos;
        static int? popupResult;

        static readonly int popupWindowID = "Popup".GetHashCode();

        public static int PopupOnLastRect(string[] displayOptions, string label="") => PopupOnLastRect(-1, displayOptions, -1, label);
        public static int PopupOnLastRect(string[] displayOptions, int button, string label = "") => PopupOnLastRect(-1, displayOptions, button, label);

        public static int PopupOnLastRect(int selectionIndex, string[] displayOptions, int button, string label = "") => Popup(GUILayoutUtility.GetLastRect(), button, selectionIndex, displayOptions, label);

        public static int Popup(Rect launchRect, int button, int selectionIndex, string[] displayOptions, string label = "")
        {
            var ret = selectionIndex;
            var controlID = GUIUtility.GetControlID(FocusType.Passive);

            // There are No Active Popup
            if (popupControlID == 0)
            {
                var ev = Event.current;
                var pos = ev.mousePosition;

                if ((ev.type == EventType.MouseUp)
                    && ((button < 0) || (ev.button == button)) 
                    && launchRect.Contains(pos)
                    )
                {
                    popupPos = pos;
                    popupControlID = controlID;
                    ev.Use();
                }
            }
            // Active
            else if (popupControlID == controlID)
            {
                if (popupResult.HasValue)
                {
                    ret = popupResult.Value;
                    popupResult = null;
                    popupControlID = 0;
                }
                else
                {
                    var size = default(Vector2);
                    var type = Event.current.type;
                    if ((type == EventType.Layout) || (type== EventType.Repaint) )
                    {
                        var buttonStyle = Style.popupButton;
                        var contentSize = Vector2.zero;
                        for (var i = 0; i < displayOptions.Length; ++i)
                        {
                            tmpContent.text = displayOptions[i];
                            var textSize = buttonStyle.CalcSize(tmpContent);
                            contentSize.x = Mathf.Max(contentSize.x, textSize.x);
                            contentSize.y += textSize.y;
                        }

                        var margin = buttonStyle.margin;
                        contentSize.y += Mathf.Max(0, displayOptions.Length - 1) * Mathf.Max(margin.top, margin.bottom); // is this right?

                        size = Style.popup.CalcScreenSize(contentSize);
                    }
                    

                    GUI.ModalWindow(popupWindowID, new Rect(popupPos, size), (id) =>
                    {
                        using (new GUILayout.VerticalScope())
                        {
                            for (var j = 0; j < displayOptions.Length; ++j)
                            {
                                if (GUILayout.Button(displayOptions[j], Style.popupButton))
                                {
                                    popupResult = j;
                                }
                            }
                        }

                        var ev = Event.current;
                        if ((ev.rawType == EventType.MouseDown) && !(new Rect(Vector2.zero, size).Contains(ev.mousePosition)))
                        {
                            popupResult = -1; ;
                        }
                    }
                    , label, Style.popup);
                }
            }

            return ret;
        }
    }
}