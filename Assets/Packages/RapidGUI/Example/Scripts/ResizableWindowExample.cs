using UnityEngine;


namespace RapidGUI.Example
{

    public class ResizableWindowExample : MonoBehaviour
    {
        Rect rect = new Rect(Vector2.one * 100, Vector2.one * 100);

        private void OnGUI()
        {
            rect = RGUI.ResizableWindow(GetHashCode(), rect,
                (id) =>
                {
                    GUILayout.Label("ResizableWindow");
                    GUI.DragWindow();
                },
                "Title");
        }
    }
}