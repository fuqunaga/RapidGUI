using System;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static GUILayoutOption fieldWidthMin = GUILayout.MinWidth(80f);

        static object StandardField(object v, Type type) => StandardField(v, type, null);
        
        static object StandardField(object v, Type type, GUILayoutOption option)
        {
            object ret = v;

            var unparsedStr = UnparsedStr.Create();
            var color = (unparsedStr.hasStr && !unparsedStr.CanParse(type)) ? Color.red : GUI.color;

            using (new ColorScope(color))
            {
                var text = unparsedStr.Get() ?? ((v != null) ? v.ToString() : "");
                var displayStr = GUILayout.TextField(text, option ?? fieldWidthMin);
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