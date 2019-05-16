using System;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static object RecursiveSlider(object obj, object min, object max)
        {
            return RecursiveFlow(obj, () => DoRecursiveSlider(obj, min, max));
        }

        static object DoRecursiveSlider(object obj, object min, object max)
        {
            min = min ?? Activator.CreateInstance(obj.GetType());

            GUILayout.EndHorizontal();
            {
                using (new PrefixLabelIndentScope())
                {
                    var type = obj.GetType();
                    //if (IsMultiLine(type))
                    {
                        DoSlider(obj, min, max, type);
                    }
                    /*
                    else
                    {
                        PrefixLabelSetting.alignRight = isInRecursive;
                        DoSlider(obj, min, max, type);
                        PrefixLabelSetting.alignRight = false;
                    }
                    */
                }
            }
            GUILayout.BeginHorizontal();

            return obj;
        }

        static void DoSlider(object obj, object min, object max, Type type)
        {
            var infos = GetMemberInfoList(type);
            for (var i = 0; i < infos.Count; ++i)
            {
                var fi = infos[i];
                var elem = fi.GetValue(obj);
                var elemMin = fi.GetValue(min);
                var elemMax = fi.GetValue(max);
                var elemLabel = fi.Name;

                elem = Slider(elem, elemMin, elemMax, fi.MemberType, elemLabel);

                fi.SetValue(obj, elem);
            }
        }
    }
}