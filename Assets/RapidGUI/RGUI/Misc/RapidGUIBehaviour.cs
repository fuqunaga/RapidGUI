using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    public class RapidGUIBehaviour : MonoBehaviour
    {
        #region static 

        static RapidGUIBehaviour instance;
        public static RapidGUIBehaviour Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<RapidGUIBehaviour>();
                    if (instance == null)
                    {
                        var ga = new GameObject("RapidGUI");
                        instance = ga.AddComponent<RapidGUIBehaviour>();
                    }

                    DontDestroyOnLoad(instance);
                }

                return instance;
            }
        }

        #endregion

        public Action onGUI;

        public void OnGUI()
        {
            onGUI?.Invoke();
        }
    }
}