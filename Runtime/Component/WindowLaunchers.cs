using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public class WindowLaunchers : TitleContents<WindowLauncher>
    {
        public string name = "";
        public bool isWindow = true;
        public bool isDraggable = true;
        public Rect rect = new Rect(Vector2.one * 10f, Vector2.zero);

        const float DefaultWidth = 300f;

        public override WindowLauncher Add(string title, Func<bool> checkEnableFunc, Func<bool> drawFunc)
        {
            var launcher = base.Add(title, checkEnableFunc, drawFunc)
                .SetWidth(DefaultWidth);

            launcher.onOpen -= OnOpen;
            launcher.onOpen += OnOpen;

            return launcher;
        }


        static readonly GUIContent tmpContent = new GUIContent();
        List<WindowLauncher> list;
        public void DoGUI()
        {
            if ( dicChanged)
            {
                list = dic.Values.ToList();
                dicChanged = false;
            }

            if (isWindow)
            {
                var style = RGUIStyle.darkWindow;
                var minWidth = 0f;
                if (!string.IsNullOrEmpty(name))
                {
                    tmpContent.text = name;
                    minWidth = style.CalcSize(tmpContent).x;
                }

                rect = RGUI.ResizableWindow(GetHashCode(), rect, (id) =>
                {
                    list.ForEach(l => l.DoGUI());
                    if (isDraggable) GUI.DragWindow();
                },
                name, RGUIStyle.darkWindow, GUILayout.MinWidth(minWidth));
            }
            else
            {
                list.ForEach(l => l.DoGUI());
            }
        }


        #region Auto Layout Windows

        readonly List<WindowLauncher> openLaunchers = new List<WindowLauncher>();

        void OnOpen(WindowLauncher launcher)
        {
            if (isWindow)
            {
                const float xOffset = 28f;
                const float yOffset = 16f;
                var x = rect.xMax + xOffset;
                var y = rect.yMin;

                var removeIdx = openLaunchers.FindIndex(l => l == launcher || !l.isOpen || l.isMoved);
                var last = (removeIdx >= 0)
                    ? openLaunchers.ElementAtOrDefault(removeIdx - 1)
                    : openLaunchers.LastOrDefault();

                if (last != null)
                {
                    x = last.rect.xMin;
                    y = last.rect.yMax + yOffset;
                    if (y > Screen.height - 100f)
                    {
                        var maxX = openLaunchers.Max(l => l.rect.xMin);
                        var top = openLaunchers.Find(l => l.rect.xMin == maxX);
                        x = top.rect.xMax + xOffset;
                        y = top.rect.yMin;
                    }
                }


                launcher.rect.position = new Vector2(x, y);

                if (removeIdx >= 0)
                {
                    openLaunchers.RemoveRange(removeIdx, openLaunchers.Count - removeIdx);
                }
                openLaunchers.Add(launcher);
            }
        }

        #endregion
    }
}