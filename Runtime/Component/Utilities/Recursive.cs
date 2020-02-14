using System;
using System.Collections.Generic;
using System.Linq;
using TupleObject = System.ValueTuple<object, object>;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static Stack<object> recursiveTypeLoopCheck = new Stack<object>();
        static bool isInRecursive => recursiveTypeLoopCheck.Count > 0;

        enum ObjStatus
        {
            Null,
            Loop,
            ValueType,
            Class,
            Tuple,
        }

        static object DoRecursiveSafe(object obj, Func<object> doFunc)
        {
            ObjStatus GetStatus(object o)
            {
                if (o == null) return ObjStatus.Null;

                var type = o.GetType();
                if (type.IsValueType)
                {
                    return (type == typeof(TupleObject)) ? ObjStatus.Tuple : ObjStatus.ValueType;
                }
                else if (recursiveTypeLoopCheck.Contains(o)) return ObjStatus.Loop;

                return ObjStatus.Class;
            }

            const string nullMsg = "is null.";
            const string loopMsg = "circular reference detected.";

            switch (GetStatus(obj))
            {
                case ObjStatus.Null:
                    WarningLabel("object " + nullMsg);
                    break;

                case ObjStatus.Loop:
                    WarningLabel($"[{obj.GetType()}]: " + loopMsg);
                    break;


                case ObjStatus.ValueType:
                    obj = doFunc();
                    break;

                case ObjStatus.Class:
                    {
                        recursiveTypeLoopCheck.Push(obj);
                        obj = doFunc();
                        recursiveTypeLoopCheck.Pop();
                    }
                    break;

                case ObjStatus.Tuple:
                    {
                        var (min, max) = (TupleObject)obj;
                        var stMin = GetStatus(min);
                        var stMax = GetStatus(max);

                        var str1 = (stMin == ObjStatus.Null) ? "min " + nullMsg : ((stMin == ObjStatus.Loop) ? "min: " + loopMsg : null);
                        var str2 = (stMax == ObjStatus.Null) ? "max " + nullMsg : ((stMax == ObjStatus.Loop) ? "max: " + loopMsg : null);

                        if ( str1 != null || str2 != null)
                        {
                            WarningLabel(string.Join("\n", new[] { str1, str2 }.Where(str => str != null).ToArray()));
                        }
                        else
                        {
                            if ( stMin == ObjStatus.Class)
                            {
                                recursiveTypeLoopCheck.Push(min);
                                recursiveTypeLoopCheck.Push(max);
                                obj = doFunc();
                                recursiveTypeLoopCheck.Pop();
                                recursiveTypeLoopCheck.Pop();
                            }
                            else
                            {
                                obj = doFunc();
                            }
                        }
                    }
                    break;
            }

            return obj;
        }
    }
}
