using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    /// Scope examples
    /// </summary>
    public class ScopeExample : ExampleBase
    {
        public class CustomClass
        {
            public int value0;
            public int value1;
        }

        CustomClass customClass = new CustomClass();


        protected override string title => "Scope";

        static readonly Dictionary<string, string> customLabelTable = new Dictionary<string, string>() { { "value1", "value1 is replaced" } };

        public override void DoGUI()
        {
            using (new RGUI.IndentScope())
            {
                GUILayout.Label("IndentScope");
            }

            using (new RGUI.EnabledScope(false))
            {
                GUILayout.Label("EnabledScope");
            }

            using (new RGUI.ColorScope(Color.green))
            {
                GUILayout.Label("ColorScope");
            }

            using (new RGUI.BackgroundColorScope(Color.red))
            {
                GUILayout.Button("BackgroundColorScope");
            }

            using (new RGUI.CustomLabelScope(customLabelTable))
            {
                RGUI.Field(customClass, "CustomLabelScope - replace value1 to custom label");
            }

            using(new RGUI.IgnoreFieldScope("value1"))
            {
                RGUI.Field(customClass, "IgnoreFieldScope - ignore value1 field");
            }
        }
    }
}