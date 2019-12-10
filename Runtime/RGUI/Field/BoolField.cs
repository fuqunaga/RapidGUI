using System;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static object BoolField(object v)
        {
            return GUILayout.Toggle(Convert.ToBoolean(v), "");
        }
    }
}
