using UnityEngine;


namespace FuGUI
{
    public static partial class GUIUtil
    {
        public static float prefixLabelWidth
        {
            get { return _prefisLabelWidth; }
            set
            {
                _prefisLabelWidth = value;
                labelWidthLayout = GUILayout.Width(_prefisLabelWidth);
            }
        }

        public static bool isLabelRightAlign;

        static float _prefisLabelWidth = 128f;
        static GUILayoutOption labelWidthLayout = GUILayout.Width(_prefisLabelWidth);


        public static void PrefixLabel(string label)
        {
            if (!string.IsNullOrEmpty(label))
            {
                var style = isLabelRightAlign ? labelRight : GUI.skin.label;

                GUILayout.Label(label, style, labelWidthLayout);
            }
        }

    }
}