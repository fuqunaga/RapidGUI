using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    /// Misc examples
    /// </summary>
    public class MiscExample : ExampleBase
    {
        protected override string title => "Misc";


        public override void DoGUI()
        {
            using (new RGUI.IndentScope())
            {
                GUILayout.Label("IndentScope");
            }

            using (new RGUI.ColorScope(Color.green))
            {
                GUILayout.Label("ColorScope");
            }

            using (new RGUI.BackgroundColorScope(Color.blue))
            {
                GUILayout.Label("BackgroundColorScope");
            }

            using (new RGUI.EnabledScope(false))
            {
                GUILayout.Label("EnabledScope");
            }


            GUILayout.Box("Popup");
            var resultIdx = RGUI.PopupOnLastRect(new[] { "Button One", "Button Two", "Button Three" });
            if (resultIdx >= 0)
            {
                Debug.Log($"Popup: Button{resultIdx + 1}");
            }
        }
    }
}