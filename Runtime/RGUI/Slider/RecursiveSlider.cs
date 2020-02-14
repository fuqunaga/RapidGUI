using System;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static object RecursiveSlider(object obj, object min, object max)
        {
            return DoRecursiveSafe(obj, () => DoRecursiveSlider(obj, min, max));
        }

        static object DoRecursiveSlider(object obj, object min, object max)
        {
            min = min ?? Activator.CreateInstance(obj.GetType());

            GUILayout.EndHorizontal();

            using (new PrefixLabelIndentScope())
            {
                var type = obj.GetType();
                DoSlider(obj, min, max, type);
            }

            GUILayout.BeginHorizontal();

            return obj;
        }

        static void DoSlider(object obj, object min, object max, Type type)
        {
            var infos = TypeUtility.GetMemberInfoList(type);
            for (var i = 0; i < infos.Count; ++i)
            {
                var fi = infos[i];
                if (CheckIgnoreField(fi.Name)) continue;

                var elem = fi.GetValue(obj);
                var elemMin = fi.GetValue(min);
                var elemMax = fi.GetValue(max);
                var elemLabel = CheckCustomLabel(fi.Name);

                elem = Slider(elem, elemMin, elemMax, fi.MemberType, elemLabel);

                fi.SetValue(obj, elem);
            }
        }
    }
}