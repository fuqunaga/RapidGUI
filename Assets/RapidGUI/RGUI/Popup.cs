using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static int popupControlID;
        static PopsupWindow popupWindow = new PopsupWindow();

        public static int PopupOnLastRect(string[] displayOptions, string label = "") => PopupOnLastRect(-1, displayOptions, -1, label);
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
                    popupWindow.pos = GUIUtility.GUIToScreenPoint(pos);
                    popupControlID = controlID;
                    ev.Use();
                }
            }
            // Active
            else if (popupControlID == controlID)
            {
                var result = popupWindow.result;
                if (result.HasValue)
                {
                    ret = result.Value;
                    popupWindow.result = null;
                    popupControlID = 0;
                }
                else
                {
                    var type = Event.current.type;
                    if ((type == EventType.Layout) || (type == EventType.Repaint))
                    {
                        var buttonStyle = RGUIStyle.popupButton;
                        var contentSize = Vector2.zero;
                        for (var i = 0; i < displayOptions.Length; ++i)
                        {
                            var textSize = buttonStyle.CalcSize(RGUIUtility.TempContent(displayOptions[i]));
                            contentSize.x = Mathf.Max(contentSize.x, textSize.x);
                            contentSize.y += textSize.y;
                        }

                        var margin = buttonStyle.margin;
                        contentSize.y += Mathf.Max(0, displayOptions.Length - 1) * Mathf.Max(margin.top, margin.bottom); // is this right?

                        var size = RGUIStyle.popup.CalcScreenSize(contentSize);

                        popupWindow.size = size;
                    }

                    popupWindow.label = label;
                    popupWindow.displayOptions = displayOptions;
                    WindowInvoker.Add(popupWindow);
                }
            }

            return ret;
        }


        class PopsupWindow : IDoGUIWindow
        {
            public string label;
            public Vector2 pos;
            public Vector2 size;
            public int? result;
            public string[] displayOptions;

            static readonly int popupWindowID = "Popup".GetHashCode();

            public void DoGUIWindow()
            {
                GUI.ModalWindow(popupWindowID, new Rect(pos, size), (id) =>
                {
                    using (new GUILayout.VerticalScope())
                    {
                        for (var j = 0; j < displayOptions.Length; ++j)
                        {
                            if (GUILayout.Button(displayOptions[j], RGUIStyle.popupButton))
                            {
                                result = j;
                            }
                        }
                    }

                    var ev = Event.current;
                    if ((ev.rawType == EventType.MouseDown) && !(new Rect(Vector2.zero, size).Contains(ev.mousePosition)))
                    {
                        result = -1; ;
                    }
                }
                , label, RGUIStyle.popup);
            }
        }
    }
}