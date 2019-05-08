using UnityEngine;

namespace FuGUI.Example
{
    public class IDebugMenuExample : MonoBehaviour, IDebugMenu
    {
        public void DebugMenu()
        {
            GUILayout.Label("IDebugMenu");
        }
    }
}