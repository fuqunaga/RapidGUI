using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static readonly string[] ListPopupButtonNames = new[] { "Add Element", "Delete Element" };

        public static T ListField<T>(T list, Func<T, int, string, object> customElementGUI = null, Func<T, object> customLabelRightFunc = null)
            where T: IList
        {
            return ListField(list, null, customElementGUI, customLabelRightFunc);
        }

        public static T ListField<T>(T list, string label, Func<T, int, string, object> customElementGUI = null, Func<T, object> customLabelRightFunc = null)
            where T : IList
        {
            Func<object, Type, object> labelRightFunc = ListLabelRightFunc;
            if (customLabelRightFunc != null)
            {
                labelRightFunc = (obj, type) => customLabelRightFunc((T)obj);
            }

            return (T)DoField(list, typeof(T), label, styleNone,
                fieldFunc: (v, t) => ListField(v, t, customElementGUI),
                labelRightFunc: labelRightFunc,
                options: null
                );
        }

        public static T ListLabelRightFunc<T>(T v) where T : IList => (T)ListLabelRightFunc(v, typeof(T));

        static object ListLabelRightFunc(object v, Type type)
        {
            var list = v as IList;
            var count = list?.Count ?? 0;
            var elemType = TypeUtility.GetListInterface(type).GetGenericArguments().First();

            GUILayout.FlexibleSpace();

            var newCount = Field(count, null, GUILayout.Width(20f));
            while (newCount > count)
            {
                list = AddElementAtLast(list, type, elemType);
                count = list.Count;
            }

            while (newCount < count)
            {
                list = DeleteElementAtLast(list, elemType);
                count = list.Count;
            }

            return list;
        }

        static object ListField(object v, Type type) => ListField<object>(v, type, null);

        static object ListField<T>(object v, Type type, Func<T, int, string, object> customElementGUI)
        {
            var list = v as IList;
            var hasElem = (list != null) && list.Count > 0;
            var elemType = TypeUtility.GetListInterface(type).GetGenericArguments().First();

            var addIdx = -1;
            var deleteIdx = -1;

            using (new GUILayout.VerticalScope())
            {
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
                                list[i] = (customElementGUI != null)
                                    ? customElementGUI((T)list, i, label)
                                    : Field(list[i], elemType, label);
                            }

                            var result = PopupOnLastRect(ListPopupButtonNames, 1);

                            switch (result)
                            {
                                case 0:
                                    addIdx = i + 1;
                                    break;
                                case 1:
                                    deleteIdx = i;
                                    break;
                            }
                        }
                    }

                    if (addIdx >= 0) list = AddElement(list, elemType, list[addIdx - 1], addIdx);
                    if (deleteIdx >= 0) list = DeleteElement(list, elemType, deleteIdx);

                    // +/- button
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        var width = GUILayout.Width(20f);
                        if (GUILayout.Button("+", width))
                        {
                            list = AddElementAtLast(list, type, elemType);
                        }

                        using (new EnabledScope(hasElem))
                        {
                            if (GUILayout.Button("-", width))
                            {
                                list = DeleteElementAtLast(list, elemType);
                            }
                        }
                    }
                }
            }

            return list;
        }


        static IList AddElementAtLast(IList list, Type type, Type elemType)
        {
            if (list == null)
            {
                list = (IList)Activator.CreateInstance(type, 0);
            }

            var baseElem = list.Count > 0  ? list[list.Count - 1] : null;

            return AddElement(list, elemType, baseElem, list.Count);
        }

        static IList DeleteElementAtLast(IList target, Type elemType)
        {
            return DeleteElement(target, elemType, target.Count - 1);
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
