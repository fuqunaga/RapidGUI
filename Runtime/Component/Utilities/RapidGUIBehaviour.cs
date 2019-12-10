using System;
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

                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(instance);
                    }
                }

                return instance;
            }
        }

        #endregion

        public KeyCode closeFocusedWindowKey = KeyCode.Q;
        public int prefixLabelSlideButton = 1;
        public Action onGUI;

        public void OnGUI()
        {
            onGUI?.Invoke();
        }
    }
}