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
                var fi = infos[i];
                if (CheckIgnoreField(fi.Name)) continue;

                var elemValMin = fi.GetValue(to.Item1);
                var elemValMax = fi.GetValue(to.Item2);
                var elemMin = fi.GetValue(min);
                var elemMax = fi.GetValue(max);
                var elemLabel = CheckCustomLabel(fi.Name);

                var tuple = (TupleObject)MinMaxSlider((elemValMin, elemValMax), elemMin, elemMax, fi.MemberType, elemLabel);


                fi.SetValue(to.Item1, tuple.Item1);
                fi.SetValue(to.Item2, tuple.Item2);
            }
        }
    }
}