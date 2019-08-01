﻿using System.Linq;
using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    /// Misc examples
    /// </summary>
    public class MiscExample : ExampleBase
    {
        protected override string title => "Misc";

        public bool useFastScrollView = true;
        FastScrollView fastScrollView = new FastScrollView();
        Vector2 scPos;

        public int scrollViewItemCount = 1000;

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

            using (new RGUI.BackgroundColorScope(Color.red))
            {
                GUILayout.Button("BackgroundColorScope");
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


            GUILayout.Space(8f);
            

            GUILayout.Label("FastScrollView (doesn't slow down even if there are many items.)");
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("ItemNum");
                if ( int.TryParse(GUILayout.TextField(scrollViewItemCount.ToString()), out var count))
                {
                    if (scrollViewItemCount != count)
                    {
                        scrollViewItemCount = count;
                        fastScrollView.SetNeedUpdateLayout();
                    }
                }
            }

            using (new RGUI.IndentScope())
            {
                useFastScrollView = GUILayout.Toggle(useFastScrollView, nameof(useFastScrollView));
                var items = Enumerable.Range(0, scrollViewItemCount);

                using (new GUILayout.VerticalScope(GUILayout.Height(500)))
                {
                    if (useFastScrollView)
                    {
                        fastScrollView.DoGUI(items, (item) => GUILayout.Label($"FastScrollView item: {item}"));
                    }
                    else
                    {
                        using (var sv = new GUILayout.ScrollViewScope(scPos))
                        {
                            scPos = sv.scrollPosition;
                            items.ToList().ForEach(i => GUILayout.Label($"ScrollView item: {i}"));
                        }
                    }
                }
            }
        }
    }
}