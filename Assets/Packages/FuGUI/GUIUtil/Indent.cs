using System;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        #region Static Method Style

        public static void BeginIndent(float width = 20f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(width);
            GUILayout.BeginVertical();
        }

        public static void EndIndent()
        {
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }

        #endregion


        #region Scope Style

        public class IndentScope : GUI.Scope
        {
            public IndentScope(float width = 20f)
            {
                BeginIndent(width);
            }

            protected override void CloseScope()
            {
                EndIndent();
            }
        }

        #endregion
    }
}