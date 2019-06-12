using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public class WindowInvoker : MonoBehaviour
    {
        #region static 

        static WindowInvoker instance;
        public static WindowInvoker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<WindowInvoker>();
                    if (instance == null)
                    {
                        var ga = new GameObject("WindowLauncherManager");
                        instance = ga.AddComponent<WindowInvoker>();
                    }

                    DontDestroyOnLoad(instance);
                }

                return instance;
            }
        }

        #endregion


        HashSet<IDoGUIWindow> windows = new HashSet<IDoGUIWindow>();

        public void Add(IDoGUIWindow window) => windows.Add(window);
        public void Remove(IDoGUIWindow window) => windows.Remove(window);
        
        public void OnGUI()
        {
            windows.ToList().ForEach(l => l?.DoGUIWindow());

            if (Event.current.type == EventType.Repaint)
            {
                windows.Clear();
            }
        }
    }
}