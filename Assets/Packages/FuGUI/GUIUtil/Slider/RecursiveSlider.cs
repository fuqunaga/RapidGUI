using System;
using System.Collections.Generic;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        public static object RecursiveSlider(object obj, object min, object max, string label = "", Dictionary<string, string> labelReplaceTable = null)
        {
            if (obj == null)
            {
                using (new GUILayout.HorizontalScope())
                {
                    PrefixLabel(label);
                    GUILayout.Label("<color=grey>object is null</grey>", "box");
                }
            }
            else
            {
                min = min ?? Activator.CreateInstance(obj.GetType());

                var type = obj.GetType();

                PrefixLabel(label);


                isLabelRightAlign = true;

                var infos = GetMemberInfoList(type);
                for (var i = 0; i < infos.Count; ++i)
                {
                    var fi = infos[i];
                    var elem = fi.GetValue(obj);
                    var elemMin = fi.GetValue(min);
                    var elemMax = fi.GetValue(max);
                    var elemLabel = labelCheck(fi.Name, labelReplaceTable);

                    elem = Slider(elem, elemMin, elemMax, fi.MemberType, elemLabel);

                    fi.SetValue(obj, elem);
                }

                isLabelRightAlign = false;
            }

            return obj;
        }
    }
}