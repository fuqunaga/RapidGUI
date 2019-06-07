using UnityEngine;

namespace RapidGUI.Example
{
    public class IDebugMenuExample : MonoBehaviour, IDoGUI
    {
        public void DoGUI()
        {
            GUILayout.Label("IDoGUI");
        }
    }
}