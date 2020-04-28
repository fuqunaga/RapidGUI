using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidGUI
{
    public static partial class TypeUtility
    {
        static readonly string ListInterfaceStr = "IList`1";

        public static Type GetListInterface(Type type) => type.GetInterface(ListInterfaceStr);

        public static bool IsList(Type type) => GetListInterface(type) != null;

        static Dictionary<Type, bool> multiLineTable = new Dictionary<Type, bool>();
        public static bool IsMultiLine(Type type)
        {
            bool ret;
            if (!multiLineTable.TryGetValue(type, out ret))
            {
                var infoList = GetMemberInfoList(type);

                ret = infoList.Any(info => info.range != null);
                if (!ret)
                {
                    var elemtTypes = infoList.Select(info => info.MemberType);

                    ret = elemtTypes.Any(t => IsRecursive(t) || IsList(t))
                        || (elemtTypes.Count() > 4);
                }

                multiLineTable[type] = ret;
            }

            return ret;
        }

        static Dictionary<Type, bool> isRecursiveTable = new Dictionary<Type, bool>();

        public static bool IsRecursive(Type type)
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