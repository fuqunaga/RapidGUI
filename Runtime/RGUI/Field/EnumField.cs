using System;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
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
                var idx = enumValues.IndexOf(v);
                var valueNames = enumValues.Select(value => value.ToString()).ToArray();

#if UNITY_EDITOR
                if (RGUILayoutUtility.IsInEditorWindow())
                {
                    idx = UnityEditor.EditorGUILayout.Popup(idx, valueNames);
                }
                else
#endif
                {
                    idx = SelectionPopup(idx, valueNames);
                }

                v = enumValues.ElementAtOrDefault(idx);
            }
            return v;
        }
    }
}