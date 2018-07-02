using System.Collections.Generic;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        public static object RecursiveSlider(object obj, object min, object max, string label = "", Dictionary<string, string> labelReplaceTable = null)
        {
            PrefixLabel(label);

            if (obj == null)
            {
                GUILayout.Label("object is null", "box");
            }
            else
            {
                var type = obj.GetType();
                var infos = GetMemberInfoList(type);


                isLabelRightAlign = true;

                for(var i=0; i<infos.Count; ++i)
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