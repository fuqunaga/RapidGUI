using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public class WindowLauncherManager : MonoBehaviour
    {
        #region static 

        static WindowLauncherManager instance;
        public static WindowLauncherManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<WindowLauncherManager>();
                    if (instance == null)
                    {
                        var ga = new GameObject("WindowLauncherManager");
                        instance = ga.AddComponent<WindowLauncherManager>();
                    }

                    DontDestroyOnLoad(instance);
                }

                return instance;
            }
        }

        #endregion


        HashSet<WindowLauncher> launchers = new HashSet<WindowLauncher>();

        public void Add(WindowLauncher launcher) => launchers.Add(launcher);
        public void Remove(WindowLauncher launcher) => launchers.Remove(launcher);
        
        public void OnGUI()
        {
            launchers.ToList().ForEach(l => l.OnGUIWindow());
        }
    }
}