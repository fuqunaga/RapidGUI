using System.Linq;
using UnityEngine;


namespace RapidGUI
{
    public class Fold : TitleContent<Fold>
    {
        public Fold() : base() { }
        public Fold(string name) : base(name) { }

        public bool DoGUI()
        {
            var ret = false;
            var guiFuncs = GetGUIFuncs();

            if (guiFuncs.Any())
            {
                using (new GUILayout.HorizontalScope())
                {
                    isOpen = DoGUIHeader(isOpen, name);
                    titleAction?.Invoke();
                }

                using (new RGUI.IndentScope())
                {
                    if (isOpen)
                    {
                        ret |= guiFuncs.Aggregate(false, (changed, drawFunc) => changed || drawFunc());
                    }
                }
            }

            return ret;
        }

        public static bool DoGUIHeader(bool isOpen, string name, params GUILayoutOption[] options)
        {
            var foldStr = isOpen ? "▼" : "▶";
            isOpen ^= GUILayout.Button(foldStr + name, Style.Fold, options);

            return isOpen;
        }


        public static class Style
        {
            public static readonly GUIStyle Fold;
            static Texture2D tex;

            static Style()
            {
                var style = new GUIStyle(GUI.skin.label);
                var toggle = GUI.skin.toggle;
                style.normal.textColor = toggle.normal.textColor;
                style.hover.textColor = toggle.hover.textColor;
                style.margin.left = 0;

                tex = new Texture2D(1, 1);
                tex.SetPixels(new[] { new Color(0.5f, 0.5f, 0.5f, 0.5f) });
                tex.Apply();
                style.hover.background = tex;

                Fold = style;
            }
        }
    }
}