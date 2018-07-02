using System;
using System.Linq;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static object EnumField(object v)
        {
            var type = v.GetType();
            var enumValues = Enum.GetValues(type).Cast<object>().ToList();

            var isFlag = type.GetCustomAttributes(typeof(FlagsAttribute), true).Any();
            if (isFlag)
            {
                var flagV = Convert.ToUInt64(Convert.ChangeType(v, type));
                enumValues.ForEach(value =>
                {
                    var flag = Convert.ToUInt64(value);
                    if (flag > 0)
                    {
                        var has = (flag & flagV) == flag;
                        has = GUILayout.Toggle(has, value.ToString());

                        flagV = has ? (flagV | flag) : (flagV & ~flag);
                    }
                });

                v = Enum.ToObject(type, flagV);
            }
            else
            {
                var valueNames = enumValues.Select(value => value.ToString()).ToArray();
                var idx = enumValues.IndexOf(v);
                idx = GUILayout.SelectionGrid(
                    idx,
                    valueNames,
                    valueNames.Length);
                v = enumValues.ElementAtOrDefault(idx);
            }
            return v;
        }
    }
}