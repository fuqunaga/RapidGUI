using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static object RecursiveField(object obj, Dictionary<string, string> labelReplaceTable = null)
        {
            if (obj == null)
            {
                GUILayout.Label("<color=grey>object is null</color>", "box");
            }
            else
            {
                var type = obj.GetType();

                var tmp = prefixLabelWidth;
                prefixLabelWidth = 0f;

                 var multiLine = IsMultiLine(type);
                if (multiLine)
                {
                    GUILayout.EndHorizontal();
                    prefixLabelWidth = tmp;
                    isLabelRightAlign = true;
                }

                var infos = GetMemberInfoList(type);
                for(var i=0; i<infos.Count; ++i)
                {
                    var info = infos[i];
                    var v = info.GetValue(obj);
                    var label = labelCheck(info.Name, labelReplaceTable);
                    v = Field(v, info.MemberType, label);
                    info.SetValue(obj, v);
                };

                if (multiLine)
                {
                    isLabelRightAlign = false;
                    GUILayout.BeginHorizontal();
                }
                else
                {
                    GUILayout.FlexibleSpace();
                }
                prefixLabelWidth = tmp;

            }
            return obj;
        }


        static Dictionary<Type, bool> multiLineTable = new Dictionary<Type, bool>();
        static bool IsMultiLine(Type type)
        {
            bool ret;
            if ( !multiLineTable.TryGetValue(type, out ret) )
            {
                var elemtTypes = GetMemberInfoList(type).Select(info => info.MemberType);

                ret = elemtTypes.Any(t => IsRecursive(t) || IsList(t))
                    || (elemtTypes.Count() > 4);

                multiLineTable[type] = ret;
            }

            return ret;
        }

        static string labelCheck(string label, Dictionary<string,string> table)
        {
            string ret;
            return ((table != null) && table.TryGetValue(label, out ret))
                ? ret
                : label;            
        }


        static Dictionary<Type, bool> isRecursiveTable = new Dictionary<Type, bool>();

        static bool IsRecursive(Type type)
        {
            bool ret;
            if (!isRecursiveTable.TryGetValue(type, out ret))
            {
                ret = GetMemberInfoList(type).Any();
                isRecursiveTable[type] = ret;
            }
            return ret;
        }
    }
}