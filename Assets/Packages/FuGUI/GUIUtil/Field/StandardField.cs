﻿using System;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static GUILayoutOption fieldWidthMin = GUILayout.MinWidth(80f);

        static object StandardField(object v, Type type)
        {
            object ret = v;

            var unparsedStr = UnparsedStr.Create();
            var color = (unparsedStr.hasStr && !unparsedStr.CanParse(type)) ? UnityEngine.Color.red : GUI.color;

            using (var cs = new ColorScope(color))
            {
                var text = unparsedStr.Get() ?? ((v != null) ? v.ToString() : "");
                var displayStr = GUILayout.TextField(text, fieldWidthMin);
                if (displayStr != text)
                {
                    try
                    {
                        ret = Convert.ChangeType(displayStr, type);
                        if (ret.ToString() == displayStr)
                        {
                            displayStr = null;
                        }
                    }
                    catch { }

                    unparsedStr.Set(displayStr);
                }
            }
            return ret;
        }
    }
}