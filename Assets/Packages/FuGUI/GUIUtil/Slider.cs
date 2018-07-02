using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FuGUI
{

    public static partial class GUIUtil
    {
        public static float Slider(float v, string label = "", Dictionary<string, string> labelReplaceTable = null)
        {
            return Slider(v, 0f, 1f, label, labelReplaceTable);
        }

        public static T Slider<T>(T v, T min, T max, string label = "", Dictionary<string, string> labelReplaceTable = null)
        {
            return (T)Slider(v, min, max, typeof(T), label, labelReplaceTable);
        }


        #region Slider() Implement

        delegate object SliderFunc(object v, object min, object max, string label);

        public static object Slider(object obj, object min, object max, Type type, string label = "", Dictionary<string, string> labelReplaceTable = null)
        {
            object ret;

            SliderFunc func;
            if (typeSliderFuncTable.TryGetValue(type, out func))
            {
                ret = func(obj, min, max, label);
            }
            else if (GetMemberInfoList(type).Any())
            {
                ret = RecursiveSlider(obj, min, max, label, labelReplaceTable);
            }
            else
            {
                ret = Field(obj, type, label);
            }


            return ret;
        }


        public static object SliderInt(object v, object min, object max, string label = "")
        {
            return Mathf.FloorToInt((float)SliderFloat((float)(int)v, (float)(int)min, (float)(int)max, label));
        }

        public static object SliderFloat(object v, object min, object max, string label = "")
        {
            float ret = default(float);

            using (var h = new GUILayout.HorizontalScope())
            {
                PrefixLabel(label);
                ret = GUILayout.HorizontalSlider((float)v, (float)min, (float)max, GUILayout.MinWidth(200));
                ret = (float)StandardField(ret, v.GetType());
            }

            return ret;
        }



        static readonly Dictionary<Type, SliderFunc> typeSliderFuncTable = new Dictionary<Type, SliderFunc>()
        {
            { typeof(int), SliderInt },
            { typeof(float), SliderFloat },
        };

        #endregion
    }

}