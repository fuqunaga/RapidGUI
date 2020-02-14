using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RapidGUI
{
    public static partial class RGUI
    {
        static bool CheckIgnoreField(string label) => ignoreFieldStack.Any(set => set.Contains(label));


        static Stack<HashSet<string>> ignoreFieldStack = new Stack<HashSet<string>>();

        public static void BeginIgnoreField(params string[] fieldNames)
        {
            ignoreFieldStack.Push(new HashSet<string>(fieldNames));
        }

        public static void EndIgnoreField()
        {
            ignoreFieldStack.Pop();
        }


        public class IgnoreFieldScope : GUI.Scope
        {
            public IgnoreFieldScope(params string[] fieldNames) => BeginIgnoreField(fieldNames);

            protected override void CloseScope() => EndIgnoreField();
        }
    }
}
