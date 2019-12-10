using UnityEngine;

namespace RapidGUI.imColorPicker.Test
{

    public class TestOnGUI : MonoBehaviour {

        public IMColorPreset preset;
        public Color color = Color.red;

        IMColorPicker colorPicker;
        [SerializeField] bool window;

        void OnGUI()
        {
            if(colorPicker == null)
            {
                colorPicker = new IMColorPicker(color, preset);
            }

            using(new GUILayout.HorizontalScope())
            {
                window = GUILayout.Toggle(window, "Window");
            }

            if(window)
            {
                colorPicker.DrawWindow();
            } else
            {
                using(new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(10f);
                    using(new GUILayout.VerticalScope())
                    {
                        GUILayout.Space(10f);
                        GUILayout.Label("IMColorPicker");
                        colorPicker.DrawColorPicker();
                    }
                }
            }

            color = colorPicker.color;
        } 

    }

}


