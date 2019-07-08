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


        static void DoGUI()
        {
            windows.ToList().ForEach(l => l?.DoGUIWindow());

            if (Event.current.type == EventType.Repaint)
            {
                windows.Clear();
            }
        }
    }
}