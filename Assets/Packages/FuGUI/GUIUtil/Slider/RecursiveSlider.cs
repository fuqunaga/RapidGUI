using System;
using System.Collections.Generic;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static object RecursiveSlider(object obj, object min, object max)
        {
            return RecursiveFlow(obj, () => DoRecursiveSlider(obj, min, max));
        }

        static object DoRecursiveSlider(object obj, object min, object max)
        {
            min = min ?? Activator.CreateInstance(obj.GetType());

            var type = obj.GetType();


            PrefixLabelSetting.alignRight = true;

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

            PrefixLabelSetting.alignRight = false;

            return obj;
        }
    }
}