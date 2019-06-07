using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public class WindowLaunchers
    {
        #region Type Define

        public class FuncData
        {
            public Func<bool> checkEnableFunc;
            public Action drawFunc;

            public void OnGUI()
            {
                if (checkEnableFunc())
                {
                    drawFunc();
                    GUI.DragWindow();
                }
            }
        }

        #endregion

        Dictionary<string, WindowLauncher> launcherDic = new Dictionary<string, WindowLauncher>();

        public string name = "";
        public bool isWindow = true;
        public bool isDraggable = true;
        public Rect rect = new Rect(Vector2.one * 10f, Vector2.zero);


        public void Add(string name, Action drawFunc) => Add(name, () => true, drawFunc);


        public void Add(string name, Func<bool> checkEnableFunc, Action drawFunc, float width = 300f)
        {
            if (!launcherDic.TryGetValue(name, out var launcher))
            {
                launcherDic[name] = launcher = new WindowLauncher(name, width);
                launcher.onOpen += OnOpen;
            }

            launcher.Add(checkEnableFunc, drawFunc);
        }

        public bool Remove(string name) => launcherDic.Remove(name);


        static GUIContent tmpContent = new GUIContent();
        public void DoGUI()
        {
            var list = launcherDic.Values.ToList();

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

        List<WindowLauncher> openLaunchers = new List<WindowLauncher>();

        void OnOpen(WindowLauncher launcher)
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
                Debug.Log($"{rect.yMax} {Screen.height}");
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

        #endregion
    }
}