using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static readonly string[] listPopupButtonNames = new[] { "Add Element", "Delete Element" };

        static Rect rect_;


        static object ListField(object v, Type type)
        {
            var list = v as IList;
            var hasElem = (list != null) && list.Count > 0;
            var elemType = TypeUtility.GetListInterface(type).GetGenericArguments().First();

            using (var ver = new GUILayout.VerticalScope("box"))
            {
                if (v == null)
                {
                    WarningLabelNoStyle("List is null.");
                }
                else if (!hasElem)
                {
                    WarningLabelNoStyle("List is empty.");
                }
                else
                {
                    var ev = Event.current;

                    for (var i = 0; i < list.Count; ++i)
                    {
                        var label = TypeUtility.IsMultiLine(elemType) ? $"Element {i}" : null;

                        using (new IndentScope(20f))
                        {
                            list[i] = Field(list[i], elemType, label);
                        }

                        var result = PopupOnLastRect(listPopupButtonNames, 1);
                        switch (result)
                        {
                            case 0:
                                list = AddElement(list, elemType, list[i], i+1);
                                break;

                            case 1:
                                list = DeleteElement(list, elemType, i);
                                break;
                        }
                    }
                }

                // +/- button
                using (var h = new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    var width = GUILayout.Width(20f);
                    if (GUILayout.Button("+", width))
                    {
                        if (list == null)
                        {
                            list = (IList)Activator.CreateInstance(type, 0);
                        }

                        var baseElem = hasElem ? list[list.Count - 1] : null;

                        list = AddElement(list, elemType, baseElem, list.Count);
                    }

                    using (new EnabledScope(hasElem))
                    {
                        if (GUILayout.Button("-", width))
                        {
                            list = DeleteElement(list, elemType, list.Count - 1);
                        }
                    }
                }
            }

            return list;
        }


        static IList AddElement(IList list, Type elemType, object baseElem, int index)
        {
            index = Mathf.Clamp(index, 0, list.Count);
            var newElem = CreateNewElement(baseElem, elemType);

            var array = list as Array;
            if (array != null)
            {
                var newArray = Array.CreateInstance(elemType, array.Length + 1);
                Array.Copy(array, newArray, index);
                newArray.SetValue(newElem, index);
                Array.Copy(array, index, newArray, index + 1, array.Length - index);
                list = newArray;
            }
            else
            {
                list.Insert(index, newElem);
            }

            return list;
        }

        static IList DeleteElement(IList list, Type elemType, int index)
        {
            var array = list as Array;
            if (array != null)
            {
                var newArray = Array.CreateInstance(elemType, array.Length - 1);
                Array.Copy(array, newArray, index);
                Array.Copy(array, index + 1, newArray, index, array.Length - 1 - index);
                list = newArray;
            }
            else
            {
                list.RemoveAt(index);
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
                else if (elemType.IsValueType)
                {
                    ret = baseElem;
                }
                // has copy constructor
                else if (elemType.GetConstructor(new[] { elemType }) != null)
                {
                    ret = Activator.CreateInstance(elemType, baseElem);
                }
            }

            if (ret == null)
            {
                ret = Activator.CreateInstance(elemType);
            }

            return ret;
        }
    }
}
