using System;
using System.Collections.Generic;
using UnityEngine;
using TupleObject = System.ValueTuple<object, object>;


namespace RapidGUI
{
    public static partial class RGUI
    {
        delegate object MinMaxSliderFunc(TupleObject tuple, object min, object max);


        public static void MinMaxSlider(MinMax<float> val, string label = null, params GUILayoutOption[] options)
        {
            MinMaxSlider(ref val.min, ref val.max, label, options);
        }

        public static void MinMaxSlider(ref float valMin, ref float valMax, string label = null, params GUILayoutOption[] options)
        {
            MinMaxSlider(ref valMin, ref valMax, 1f, label, options);
        }


        public static void MinMaxSlider<T>(MinMax<T> val, T max, string label = null, params GUILayoutOption[] options)
        {
            MinMaxSlider(ref val.min, ref val.max, max, label, options);
        }

        public static void MinMaxSlider<T>(ref T valMin, ref T valMax, T max, string label = null, params GUILayoutOption[] options)
        {
            MinMaxSlider(ref valMin, ref valMax, default, max, label, options);
        }


        public static void MinMaxSlider<T>(MinMax<T> val, MinMax<T> range, string label = null, params GUILayoutOption[] options)
        {
            MinMaxSlider(ref val.min, ref val.max, range.min, range.max, label, options);
        }

        public static void MinMaxSlider<T>(ref T valMin, ref T valMax, T min, T max, string label = null, params GUILayoutOption[] options)
        {
            var obj = MinMaxSlider((valMin, valMax), min, max, typeof(T), label, options);
            var to = (TupleObject)obj;
            valMin = (T)to.Item1;
            valMax = (T)to.Item2;
        }


        #region MinMaxSlider() Implement

        public static object MinMaxSlider(TupleObject to, object min, object max, Type type, string label, params GUILayoutOption[] options)
        {
            using (new GUILayout.VerticalScope(options))
            using (new GUILayout.HorizontalScope())
            {
                PrefixLabel(label);
                return DicpatchMinMaxSliderFunc(type).Invoke(to, min, max);
            }
        }

        static Dictionary<Type, MinMaxSliderFunc> MinMaxSliderFuncTable = new Dictionary<Type, MinMaxSliderFunc>()
        {
            {typeof(int), MinMaxSliderInt },
            {typeof(float), MinMaxSliderFloat }
        };

        static MinMaxSliderFunc DicpatchMinMaxSliderFunc(Type type)
        {
            if (!MinMaxSliderFuncTable.TryGetValue(type, out var func))
            {
                if (TypeUtility.IsRecursive(type))
                {
                    func = RecursiveMinMaxSlider;
                }
                else
                {
                    // 不明なものはFieldに流す
                    var fieldFunc = DispatchFieldFunc(type);
                    func = (v, min, max) => fieldFunc(v, type);
                }

                MinMaxSliderFuncTable[type] = func;
            }

            return func;
        }


        static object MinMaxSliderInt(TupleObject v, object min, object max)
        {
            var f1 = (float)(int)v.Item1;
            var f2 = (float)(int)v.Item2;

            var rect = GUILayoutUtility.GetRect(RGUIUtility.TempContent(null), GUI.skin.horizontalSlider, GUILayout.MinWidth(SliderSetting.minWidth));
            MinMaxSliderCore.MinMaxSlider(rect, ref f1, ref f2, (int)min, (int)max);
            v = ((int)f1, (int)f2);

            v.Item1 = StandardField(v.Item1,typeof(int), GUILayout.Width(SliderSetting.fieldWidth));
            v.Item2 = StandardField(v.Item2,typeof(int), GUILayout.Width(SliderSetting.fieldWidth));

            return v;
        }

        static object MinMaxSliderFloat(TupleObject v, object min, object max)
        {
            var f1 = (float)v.Item1;
            var f2 = (float)v.Item2;


            var rect = GUILayoutUtility.GetRect(RGUIUtility.TempContent(null), GUI.skin.horizontalSlider, GUILayout.MinWidth(SliderSetting.minWidth));
            MinMaxSliderCore.MinMaxSlider(rect, ref f1, ref f2, (float)min, (float)max);
            v = (f1, f2);

            v.Item1 = (float)StandardField(v.Item1, typeof(float), GUILayout.Width(SliderSetting.fieldWidth));
            v.Item2 = (float)StandardField(v.Item2, typeof(float), GUILayout.Width(SliderSetting.fieldWidth));

            return v;
        }

        #endregion
    }
}