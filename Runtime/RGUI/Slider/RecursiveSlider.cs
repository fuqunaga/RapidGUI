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
                var info = infos[i];
                if (CheckIgnoreField(info.Name)) continue;

                var elem = info.GetValue(obj);
                var elemMin = (min is float) ? min : info.GetValue(min);
                var elemMax = (max is float) ? max : info.GetValue(max);
                var elemLabel = CheckCustomLabel(info.Name) ?? info.label;

                elem = Slider(elem, elemMin, elemMax, info.MemberType, elemLabel);

                info.SetValue(obj, elem);
            }
        }
    }
}