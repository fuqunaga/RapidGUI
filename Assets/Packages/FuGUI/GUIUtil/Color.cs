using System;
using System.Collections.Generic;
using UnityEngine;


namespace FuGUI
{
    public static partial class GUIUtil
    {
        #region Static Method Style

        public static void Color(Color color, Action action)
        {
            using (var c = new ColorScope(color))
            {
                action();
            }
        }

        #endregion

        #region Scope Style

        public class ColorScope : GUI.Scope
        {
            static Queue<Color> queue = new Queue<Color>();

            public ColorScope(Color color)
            {
                queue.Enqueue(GUI.color);
                GUI.color = color;
            }

            protected override void CloseScope()
            {
                GUI.color = queue.Dequeue();
            }
        }

        #endregion
    }
}