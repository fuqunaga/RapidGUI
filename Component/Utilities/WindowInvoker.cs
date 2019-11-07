using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public static class WindowInvoker
    {
        static HashSet<IDoGUIWindow> windows = new HashSet<IDoGUIWindow>();

        static WindowInvoker()
        {
            RapidGUIBehaviour.Instance.onGUI += DoGUI;
        }


        public static void Add(IDoGUIWindow window) => windows.Add(window);
        public static void Remove(IDoGUIWindow window) => windows.Remove(window);

        static IDoGUIWindow focusedWindow;

        public static void SetFocusedWindow(IDoGUIWindow window)
        {
            focusedWindow = window;
        }

        static void DoGUI()
        {
            windows.ToList().ForEach(l => l?.DoGUIWindow());

            var evt = Event.current;

            if ((evt.type == EventType.KeyUp) 
                && (evt.keyCode == RapidGUIBehaviour.Instance.closeFocusedWindowKey)
                && (GUIUtility.keyboardControl == 0)
                )
            {
                if (windows.Contains(focusedWindow))
                {
                    focusedWindow.CloseWindow();
                    focusedWindow = null;
                }
            }


            if (Event.current.type == EventType.Repaint)
            {
                windows.Clear();
            }
        }
    }
}