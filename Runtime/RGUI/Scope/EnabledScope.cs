using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI
{
    public static partial class RGUI
    {
        static Stack<bool> enabledScopeStack = new Stack<bool>();

        public static void BeginEnabled(bool enabled)
        {
            enabledScopeStack.Push(GUI.enabled);
            GUI.enabled = enabled;
        }

        public static void EndEnabled()
        {
            GUI.enabled = enabledScopeStack.Pop();
        }


        public class EnabledScope : GUI.Scope
        {
            public EnabledScope(bool enabled) => BeginEnabled(enabled);

            protected override void CloseScope() => EndEnabled();
        }
    }
}