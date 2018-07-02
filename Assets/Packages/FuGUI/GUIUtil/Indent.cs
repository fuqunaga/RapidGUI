using System;
using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        #region Static Method Style

        public static void Indent(Action action) { Indent(1, action); }
        public static void Indent(int level, Action action)
        {
            using (var i = new IndentScope(level))
            {
                action();
            }
        }

        #endregion


        #region Scope Style

        public class IndentScope : GUI.Scope
        {
            public IndentScope(int level = 1)
            {
                const int TAB = 20;
                GUILayout.BeginHorizontal();
                GUILayout.Space(TAB * level);
                GUILayout.BeginVertical();
            }

            protected override void CloseScope()
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        #endregion
    }
}