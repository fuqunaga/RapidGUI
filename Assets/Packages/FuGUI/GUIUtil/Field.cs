using System;
using System.Collections.Generic;
using UnityEngine;


namespace FuGUI
{
    using FieldFunc = Func<object, Type, object>;

    public static partial class GUIUtil
    {
        public static T Field<T>(T v, string label = null, params GUILayoutOption[] options)
        {
            var type = typeof(T);
            var obj = Field(v, type, label, options);
            return (T)Convert.ChangeType(obj, type);
        }


        public static object Field(object obj, Type type, string label = null, params GUILayoutOption[] options)
        {
            using (var h = new GUILayout.HorizontalScope(options))
            {
                PrefixLabel(label);
                obj = DicpatchFieldFunc(type).Invoke(obj, type);
            }

            return obj;
        }

        static Dictionary<Type, FieldFunc> fieldFuncTable = new Dictionary<Type, FieldFunc>();

        static FieldFunc DicpatchFieldFunc(Type type)
        {
            FieldFunc func;
            if ( !fieldFuncTable.TryGetValue(type, out func))
            {
                if (type.IsEnum)
                {
                    func = new FieldFunc((obj, t) => EnumField(obj));
                }
                else if (type == typeof(bool))
                {
                    func = new FieldFunc((obj, t) => BoolField(obj));
                }
                else if (type == typeof(Color))
                {
                    func = new FieldFunc((obj, t) => ColorField(obj));
                }
                else if (IsList(type))
                {
                    func = ListField;
                }
                else if (IsRecursive(type))
                {
                    func = new FieldFunc((obj, t) => RecursiveField(obj));
                }
                else
                {
                    func = StandardField;
                }

                fieldFuncTable[type] = func;
            }

            return func;
        }

        static bool IsList(Type type) => type.GetInterface(ListInterfaceStr) != null;

        #region Style

        static GUIStyle _labelRight;
        public static GUIStyle labelRight
        {
            get
            {
                if (_labelRight == null)
                {
                    _labelRight = new GUIStyle(GUI.skin.label);
                    _labelRight.alignment = TextAnchor.UpperRight;
                }
                return _labelRight;
            }
        }

        #endregion
    }
}