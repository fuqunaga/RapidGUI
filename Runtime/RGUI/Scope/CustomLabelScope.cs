using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RapidGUI
{
    public static partial class RGUI
    {
        static string CheckCustomLabel(string label)
        {
            return customLabelScopeStack
                .Select(t =>
                {
                    t.TryGetValue(label, out var l);
                    return l;
                })
                .FirstOrDefault(l => l != null);
        }

        static readonly Stack<Dictionary<string, string>> customLabelScopeStack = new ();

        public static void BeginCustomLabel(Dictionary<string, string> table)
        {
            customLabelScopeStack.Push(table);
        }

        public static void EndCustomLabel()
        {
            customLabelScopeStack.Pop();
        }


        public class CustomLabelScope : GUI.Scope
        {
            public CustomLabelScope(Dictionary<string, string> table) => BeginCustomLabel(table);

            protected override void CloseScope() => EndCustomLabel();
        }
    }
}
