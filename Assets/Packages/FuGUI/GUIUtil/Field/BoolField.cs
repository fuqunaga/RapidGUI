using System;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static object BoolField(object v)
        {
            return GUILayout.Toggle(Convert.ToBoolean(v), "");
        }
    }
}
