using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FuGUI
{
    public static partial class GUIUtil
    {
        delegate object SliderFunc(object v, object min, object max);

        public static float sliderMinWidth = 200f;
        public static float sliderFieldWidth = 80f;


        public static float Slider(float v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, 1f, label, options);
        }

        public static T Slider<T>(T v, T max, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, max, label, options);
        }

        public static T Slider<T>(T v, T min, T max, string label = null, params GUILayoutOption[] options)
        {
            return (T)Slider(v, min, max, typeof(T), label, options);
        }


        #region Slider() Implement



        public static object Slider(object obj, object min, object max, Type type, string label = null, params GUILayoutOption[] options)
        {
            using (var h = new GUILayout.HorizontalScope(options))
            {
                obj = PrefixLabelDraggable(label, obj, type);
                obj = DicpatchSliderFunc(type).Invoke(obj, min, max);
            }

            return obj;
        }

        static Dictionary<Type, SliderFunc> sliderFuncTable = new Dictionary<Type, SliderFunc>()
        {
            {typeof(int), SliderInt },
            {typeof(float), SliderFloat }
        };

        static SliderFunc DicpatchSliderFunc(Type type)
        {
            if (!sliderFuncTable.TryGetValue(type, out var func))
            {
                if (IsRecursive(type))
                {
                    func = RecursiveSlider;
                }
                else
                {
                    // 不明なものはFieldに流す
                    var fieldFunc = DispatchFieldFunc(type);
                    func = (v, min, max) => fieldFunc(v, type);
                }

                sliderFuncTable[type] = func;
            }

            return func;
        }


        static object SliderInt(object v, object min, object max)
        {
            var ret = (int)GUILayout.HorizontalSlider((int)v, (int)min, (int)max, GUILayout.MinWidth(sliderMinWidth));
            ret = (int)StandardField(ret, v.GetType(), GUILayout.Width(sliderFieldWidth));

            return ret;
        }

        static object SliderFloat(object v, object min, object max)
        {
            var ret = GUILayout.HorizontalSlider((float)v, (float)min, (float)max, GUILayout.MinWidth(sliderMinWidth));
            ret = (float)StandardField(ret, v.GetType(), GUILayout.Width(sliderFieldWidth));


            return ret;
        }

        #endregion
    }

}