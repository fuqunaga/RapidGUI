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
        public Rect rect;


        public void Add(string name, Action drawFunc) => Add(name, () => true, drawFunc);


        public void Add(string name, Func<bool> checkEnableFunc, Action drawFunc)
        {
            if (!launcherDic.TryGetValue(name, out var launcher))
            {
                launcherDic[name] = launcher = new WindowLauncher(name);
            }

            launcher.Add(checkEnableFunc, drawFunc);
        }

        public bool Remove(string name) => launcherDic.Remove(name);


        public void OnGUI()
        {
            var launchers = launcherDic.Values.ToList();

            if (isWindow)
            {
                rect = RGUI.ResizableWindow(GetHashCode(), rect, (id) =>
                {
                    launchers.ForEach(l => l.OnGUI());
                    if (isDraggable) GUI.DragWindow();
                },
                name);
            }
            else
            {
                launchers.ForEach(l => l.OnGUI());
            }
        }
    }
}