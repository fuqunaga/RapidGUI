using System;
using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI
{
    public static partial class RGUI
    {
        delegate object SliderFunc(object v, object min, object max);

        public static class SliderSetting
        {
            public static float minWidth = 200f;
            public static float fieldWidth = 80f;
        }


        public static float Slider(float v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, 1f, label, options);
        }


        #region Unity Primitives

        public static Vector2 Slider(Vector2 v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, Vector2.one, label, options);
        }

        public static Vector3 Slider(Vector3 v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, Vector3.one, label, options);
        }

        public static Vector4 Slider(Vector4 v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, Vector4.one, label, options);
        }

        public static Vector2Int Slider(Vector2Int v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, Vector2Int.one * 100, label, options);
        }

        public static Vector3Int Slider(Vector3Int v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, Vector3Int.one * 100, label, options);
        }

        public static Rect Slider(Rect v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, new Rect(Vector2.one, Vector2.one), label, options);
        }

        public static Bounds Slider(Bounds v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, new Bounds(Vector3.one, Vector3.one), label, options);
        }

        public static BoundsInt Slider(BoundsInt v, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, new BoundsInt(Vector3Int.one * 100, Vector3Int.one * 100), label, options);
        }

        #endregion



        public static T Slider<T>(T v, T max, string label = null, params GUILayoutOption[] options)
        {
            return Slider(v, default, max, label, options);
        }

        public static T Slider<T>(T v, T min, T max, string label = null, params GUILayoutOption[] options)
        {
            return (T)Slider(v, min, max, typeof(T), label, options);
        }


        public static float Slider(float v, string label, ref bool isOpen, params GUILayoutOption[] options)
        {
            return Slider(v, 1f, label, ref isOpen, options);
        }

        public static T Slider<T>(T v, T max, string label, ref bool isOpen, params GUILayoutOption[] options)
        {
            return Slider(v, default, max, label, ref isOpen, options);
        }

        public static T Slider<T>(T v, T min, T max, string label, ref bool isOpen, params GUILayoutOption[] options)
        {
            return (T)Slider(v, min, max, typeof(T), label, ref isOpen, options);
        }



        #region Slider() Implement

        public static object Slider(object obj, object min, object max, Type type, string label, ref bool isOpen, params GUILayoutOption[] options)
        {
            if (!TypeUtility.IsRecursive(type))
            {
                obj = Slider(obj, min, max, type, label, options);
            }
            else
            {
                using (new GUILayout.VerticalScope(options))
                {
                    if (isOpen)
                    {
                        isOpen = Fold.DoGUIHeader(isOpen, label);

                        using (new GUILayout.HorizontalScope())
                        {
                            obj = DispatchSliderFunc(type).Invoke(obj, min, max);
                        }
                    }
                    else
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            isOpen = Fold.DoGUIHeader(isOpen, label, GUILayout.Width(PrefixLabelSetting.width));
                            obj = DispatchFieldFunc(type).Invoke(obj, type);
                        }
                    }
                }
            }

            return obj;
        }

        /*
        public static object Slider(object obj, object min, object max, Type type, string label, params GUILayoutOption[] options)
        {
            using (new GUILayout.VerticalScope(options))
            using (new GUILayout.HorizontalScope())
            {
                obj = PrefixLabelDraggable(label, obj, type);
                obj = DicpatchSliderFunc(type).Invoke(obj, min, max);
            }

            return obj;
        }
        */

        public static object Slider(object obj, object min, object max, Type type, string label, params GUILayoutOption[] options)
        {
            var sliderFunc = DispatchSliderFunc(type);
            
            return DoField(obj, type, label, GUIStyle.none, FieldFunc, null, options);

            object FieldFunc(object o, Type _)
            {
                return sliderFunc(o, min, max);
            }
        }

        static readonly Dictionary<Type, SliderFunc> sliderFuncTable = new Dictionary<Type, SliderFunc>()
        {
            {typeof(int), SliderInt },
            {typeof(float), SliderFloat }
        };

        static SliderFunc DispatchSliderFunc(Type type)
        {
            if (!sliderFuncTable.TryGetValue(type, out var func))
            {
                if (TypeUtility.IsRecursive(type))
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
            if (min is float) min = (int)(float)min;
            if (max is float) max = (int)(float)max;
            var ret = (int)GUILayout.HorizontalSlider((int)v, (int)min, (int)max, GUILayout.MinWidth(SliderSetting.minWidth));
            ret = (int)StandardField(ret, v.GetType(), GUILayout.Width(SliderSetting.fieldWidth));

            return ret;
        }

        static object SliderFloat(object v, object min, object max)
        {
            var ret = GUILayout.HorizontalSlider((float)v, (float)min, (float)max, GUILayout.MinWidth(SliderSetting.minWidth));
            ret = (float)StandardField(ret, v.GetType(), GUILayout.Width(SliderSetting.fieldWidth));


            return ret;
        }

        #endregion
    }

}