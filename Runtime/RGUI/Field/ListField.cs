using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static readonly string[] ListPopupButtonNames = new[] { "Add Element", "Delete Element" };

        static object ListField(object v, Type type)
        {
            var list = v as IList;
            var hasElem = (list != null) && list.Count > 0;
            var elemType = TypeUtility.GetListInterface(type).GetGenericArguments().First();

            var addIdx = -1;
            var deleteIdx = -1;
            using (new GUILayout.VerticalScope("box"))
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
                    for (var i = 0; i < list.Count; ++i)
                    {
                        var label = TypeUtility.IsMultiLine(elemType) ? $"Element {i}" : null;

                        using (new IndentScope(20f))
                        {
                            list[i] = Field(list[i], elemType, label);
                        }

                        var result = PopupOnLastRect(ListPopupButtonNames, 1);
#if true
                        switch (result)
                        {
                            case 0: addIdx = i+1;
                                break;
                            case 1: deleteIdx = i;
                                break;
                        }
#else
                        list = result switch
                        {
                            0 => AddElement(list, elemType, list[i], i + 1),
                            1 => DeleteElement(list, elemType, i),
                            _ => list
                        };
#endif
                    }
                }
                
                if (addIdx >= 0) list = AddElement(list, elemType, list[addIdx-1], addIdx);
                if (deleteIdx >= 0) list = DeleteElement(list, elemType, deleteIdx);
                
                // +/- button
                using (new GUILayout.HorizontalScope())
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

            if (list is Array array)
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
            if (list is Array array)
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
                ret = (elemType == typeof(string))
                    ? ""
                    : Activator.CreateInstance(elemType);
            }

            return ret;
        }
    }
}
