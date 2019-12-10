using System.Collections.Generic;
using UnityEngine;
using Table = System.Collections.Generic.Dictionary<string, string>;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static Table recursiveCustomLabel;

        static string CheckCustomLabel(string label)
        {
            if (recursiveCustomLabel != null)
            {
                if (recursiveCustomLabel.TryGetValue(label, out var modified))
                {
                    label = modified;
                }
            }

            return label;
        }

        static Stack<Table> customLabelScopeStack = new Stack<Table>();

        public static void BeginCustomLabel(Table table)
        {
            customLabelScopeStack.Push(recursiveCustomLabel);
            recursiveCustomLabel = table;
        }

        public static void EndCustomLabel()
        {
            recursiveCustomLabel = customLabelScopeStack.Pop();
        }


        public class CustomLabelScope : GUI.Scope
        {
            public CustomLabelScope(Table table) => BeginCustomLabel(table);

            protected override void CloseScope() => EndCustomLabel();
        }
    }
}
