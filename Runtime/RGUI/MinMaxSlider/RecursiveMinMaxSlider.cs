using System;
using UnityEngine;
using TupleObject = System.ValueTuple<object, object>;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static object RecursiveMinMaxSlider(TupleObject to, object min, object max)
        {
            return DoRecursiveSafe(to, () => DoRecursiveMinMaxSlider(to, min, max));
        }

        static object DoRecursiveMinMaxSlider(TupleObject to, object min, object max)
        {
            var type = to.Item1.GetType();
            min = min ?? Activator.CreateInstance(type);

            GUILayout.EndHorizontal();

            using (new PrefixLabelIndentScope())
            {
                DoMinMaxSlider(to, min, max, type);
            }

            GUILayout.BeginHorizontal();

            return to;
        }

        static void DoMinMaxSlider(TupleObject to, object min, object max, Type type)
        {
            var infos = TypeUtility.GetMemberInfoList(type);
            for (var i = 0; i < infos.Count; ++i)
            {
                var info = infos[i];
                if (CheckIgnoreField(info.Name)) continue;

                var elemValMin = info.GetValue(to.Item1);
                var elemValMax = info.GetValue(to.Item2);
                var elemMin = info.GetValue(min);
                var elemMax = info.GetValue(max);
                var elemLabel = CheckCustomLabel(info.Name) ?? info.label;

                var tuple = (TupleObject)MinMaxSlider((elemValMin, elemValMax), elemMin, elemMax, info.MemberType, elemLabel);


                info.SetValue(to.Item1, tuple.Item1);
                info.SetValue(to.Item2, tuple.Item2);
            }
        }
    }
}