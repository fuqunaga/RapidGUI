using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        static readonly string ListInterfaceStr = "IList`1";

        static object ListField(object v, Type type)
        {
            var list = v as IList;
            var hasElem = (list != null) && list.Count > 0;
            var elemType = type.GetInterface(ListInterfaceStr).GetGenericArguments().First();

            using (var ver = new GUILayout.VerticalScope("box"))
            {
                if (v == null)
                {
                    GUILayout.Label("<color=grey>List is null</color>");
                }
                else if (!hasElem)
                {
                    GUILayout.Label("<color=grey>List is Empty</color>");
                }
                else
                {
                    for (var i = 0; i < list.Count; ++i)
                    {
                        list[i] = Field(list[i], elemType);
                    }
                }


                using (var h = new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    var width = GUILayout.Width(20f);
                    if (GUILayout.Button("+", width))
                    {
                        if (list == null) list = (IList)Activator.CreateInstance(type);
                        var newElem = CreateNewElement(hasElem ? list[list.Count - 1] : null, elemType);

                        // list.Add() is Not implemented if type is array. so it rise exception.
                        var array = list as Array;
                        if (array != null)
                        {
                            var newArray = Array.CreateInstance(elemType, array.Length + 1);
                            Array.Copy(array, newArray, array.Length);
                            newArray.SetValue(newElem, array.Length);
                            list = newArray;
                        }
                        else
                        {
                            list.Add(newElem);
                        }
                    }

                    var tmp = GUI.enabled;
                    GUI.enabled = hasElem;
                    if (GUILayout.Button("-", width))
                    {
                        // list.Add() is Not implemented if type is array. so it rise exception.
                        var array = list as Array;
                        if (array != null)
                        {
                            var newArray = Array.CreateInstance(elemType, array.Length - 1);
                            Array.Copy(array, newArray, array.Length - 1);
                            list = newArray;
                        }
                        else
                        {
                            list.RemoveAt(list.Count - 1);
                        }
                    }
                    GUI.enabled = tmp;
                }
            }

            return list;
        }



        static object CreateNewElement(object baseElem, Type elemType)
        {
            object ret = null;

            if (baseElem != null)
            {
                // is cloneable
                var cloneable = baseElem as ICloneable;
                if (cloneable != null)
                {
                    ret = cloneable.Clone();
                }
                else if ( elemType.IsValueType)
                {
                    ret = baseElem;
                }
                // has copy constructor
                else if (elemType.GetConstructor(new[] { elemType }) != null)
                {
                    ret = Activator.CreateInstance(elemType, baseElem);
                }
            }

            if ( ret == null )
            {
                ret = Activator.CreateInstance(elemType);
            }

            return ret;
        }
    }
}
