using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static object RecursiveField(object obj)
        {
            return RecursiveFlow(obj, () => DoRecursiveField(obj));
        }

        static object DoRecursiveField(object obj)
        {
            var type = obj.GetType();

            var multiLine = IsMultiLine(type);
            if (multiLine)
            {
                GUILayout.EndHorizontal();

                using (new PrefixLabelIndentScope())
                {
                    DoFields(obj, type);
                }

                GUILayout.BeginHorizontal();
            }
            else
            {
                var tmp = PrefixLabelSetting.width;
                PrefixLabelSetting.width = 0f;

                DoFields(obj, type);

                GUILayout.FlexibleSpace();
                PrefixLabelSetting.width = tmp;
            }

            return obj;
        }


        static void DoFields(object obj, Type type)
        {
            var infos = GetMemberInfoList(type);
            for (var i = 0; i < infos.Count; ++i)
            {
                var info = infos[i];
                var v = info.GetValue(obj);
                v = Field(v, info.MemberType, info.Name);
                info.SetValue(obj, v);
            };
        }




        static Dictionary<Type, bool> multiLineTable = new Dictionary<Type, bool>();
        static bool IsMultiLine(Type type)
        {
            bool ret;
            if (!multiLineTable.TryGetValue(type, out ret))
            {
                var elemtTypes = GetMemberInfoList(type).Select(info => info.MemberType);

                ret = elemtTypes.Any(t => IsRecursive(t) || IsList(t))
                    || (elemtTypes.Count() > 4);

                multiLineTable[type] = ret;
            }

            return ret;
        }

        static string labelCheck(string label, Dictionary<string, string> table)
        {
            string ret;
            return ((table != null) && table.TryGetValue(label, out ret))
                ? ret
                : label;
        }


        static Dictionary<Type, bool> isRecursiveTable = new Dictionary<Type, bool>();

        static bool IsRecursive(Type type)
        {
            if (!isRecursiveTable.TryGetValue(type, out var ret))
            {
                ret = GetMemberInfoList(type).Any();
                isRecursiveTable[type] = ret;
            }
            return ret;
        }
    }
}