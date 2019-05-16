using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
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
        public class IndentScope : GUI.Scope
        {
            public IndentScope(float width = 20f) => BeginIndent(width);

            protected override void CloseScope() => EndIndent();
        }
    }
}