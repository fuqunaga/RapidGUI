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


        public void Add(string name, Func<bool> checkEnableFunc, Action drawFunc)
        {
            if (!launcherDic.TryGetValue(name, out var launcher))
            {
                launcherDic[name] = launcher = new WindowLauncher(name);
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

        List<WindowLauncher> openLaunchers = new List<WindowLauncher>();

        void OnOpen(WindowLauncher launcher)
        {
            var idx = openLaunchers.FindIndex(l => l == launcher || !l.isOpen || l.isMoved);
            if (idx >= 0)
            {
                openLaunchers.RemoveRange(idx, openLaunchers.Count - idx);
            }

            var last = openLaunchers.LastOrDefault();
            var pos = new Vector2(rect.xMax + 28f, (last?.rect.yMax + 16f) ?? rect.yMin);
            launcher.rect.position = pos;

            openLaunchers.Add(launcher);
        }
    }
}