using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI
{
    public static partial class RGUI
    {
        static Stack<Color> bacgroundColorScopeStack = new Stack<Color>();

        public static void BeginBackgroundColor(Color color)
        {
            bacgroundColorScopeStack.Push(GUI.backgroundColor);
            GUI.backgroundColor = color;
        }

        public static void EndBackgroundColor()
        {
            GUI.backgroundColor = bacgroundColorScopeStack.Pop();
        }


        public class BackgroundColorScope : GUI.Scope
        {
            public BackgroundColorScope(Color color) => BeginBackgroundColor(color);

            protected override void CloseScope() => EndBackgroundColor();
        }
    }
}