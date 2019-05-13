using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        #region Style

        static GUIStyle _warningLabelStyle;
        public static GUIStyle warningLabelStyle
        {
            get
            {
                if (_warningLabelStyle == null)
                {
                    _warningLabelStyle = new GUIStyle(GUI.skin.box);
                    _warningLabelStyle.alignment = GUI.skin.label.alignment;
                }
                return _warningLabelStyle;
            }
        }

        #endregion

        public static string warningLabelPrefix = "<color=grey>";
        public static string warningLabelPostfix = "</color>";

        static string WarningLabelModifyLabel(string label) => warningLabelPrefix + label + warningLabelPostfix;

        public static void WarningLabel(string label)
        {
            GUILayout.Label(WarningLabelModifyLabel(label), warningLabelStyle);
        }

        public static void WarningLabelNoStyle(string label)
        {
            GUILayout.Label(WarningLabelModifyLabel(label));
        }
    }
}