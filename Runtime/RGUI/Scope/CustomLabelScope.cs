using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Table = System.Collections.Generic.Dictionary<string, string>;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static string CheckCustomLabel(string label)
        {
            var table = customLabelScopeStack.FirstOrDefault(t => t.ContainsKey(label));
            if (table != null)
            {
                label = table[label];
            }
     

            return label;
        }

        static Stack<Table> customLabelScopeStack = new Stack<Table>();

        public static void BeginCustomLabel(Table table)
        {
            customLabelScopeStack.Push(table);
        }

        public static void EndCustomLabel()
        {
            customLabelScopeStack.Pop();
        }


        public class CustomLabelScope : GUI.Scope
        {
            public CustomLabelScope(Table table) => BeginCustomLabel(table);

            protected override void CloseScope() => EndCustomLabel();
        }
    }
}
