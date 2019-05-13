using System;
using System.Collections.Generic;
using UnityEngine;

namespace FuGUI
{

    public static partial class GUIUtil
    {
        static Stack<int> recursiveTypeLoopCheck = new Stack<int>();
        static bool isInRecursive => recursiveTypeLoopCheck.Count > 1;

        static object RecursiveFlow(object obj, Func<object> doFunc)
        {
            if (obj == null)
            {
                GUILayout.Label("<color=grey>object is null</color>", "box");
            }
            else
            {
                var type = obj.GetType();

                if (type.IsValueType)
                {
                    obj = doFunc();
                }
                else
                {
                    var hash = obj.GetHashCode();

                    if (recursiveTypeLoopCheck.Contains(hash))
                    {
                        GUILayout.Label($"<color=grey>[{type}]: circular reference detected.</color>", "box");
                    }
                    else
                    {
                        recursiveTypeLoopCheck.Push(hash);
                        obj = doFunc();
                        recursiveTypeLoopCheck.Pop();
                    }
                }
            }
            return obj;
        }
    }
}
