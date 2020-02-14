using UnityEngine;


namespace RapidGUI.Example
{
    public class RapidGUIExample : MonoBehaviour
    {
        WindowLaunchers launchers;

        public void Start()
        {
            // if WindowLaunchers.isWindow == true(default)
            // WindowLaunchers will be wrapped in window.
            // child windows automaticaly aligned.
            launchers = new WindowLaunchers
            {
                name = "WindowLaunchers"
            };
            
            launchers.Add("RGUI.Field() Part1", typeof(FieldExample_Part1));
            launchers.Add("RGUI.Field() Part2", typeof(FieldExample_Part2));
            launchers.Add("RGUI.Slider()", typeof(SliderExample));
            launchers.Add("RGUI.MinMaxSlider()", typeof(MinMaxSliderExample));
            launchers.Add("Scope", typeof(ScopeExample));
            launchers.Add("Fold / Folds", typeof(FoldExample));
            launchers.Add("WindowLauncher / WindowLaunchers", typeof(WindowLauncherExample));
            launchers.Add("Misc", typeof(MiscExample)).SetWidth(600f);
        }

        void OnGUI()
        {
            launchers.DoGUI();
        }
    }
}