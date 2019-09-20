using UnityEngine;


namespace RapidGUI.Example
{
    public class RapidGUIExample : MonoBehaviour
    {
        Folds miscFolds = new Folds();
        Folds dynamicFolds = new Folds();

        WindowLaunchers launchers;


        public string _string;
        public bool _bool;
        public int _int;
        public float _float;
        public Vector2 _vector2;
        public Vector3 _vector3;
        public Vector4 _vector4;
        public Vector2Int _vector2Int;
        public Vector3Int _vector3Int;
        public Rect _rect;

        bool _dynamicFoldEnable = true;

        public void Start()
        {
            miscFolds.Add("Fold0", () => { GUILayout.Label("Fold0"); });
            miscFolds.Add("Fold1", () => { GUILayout.Label("Fold1 FirstAdd"); });
            miscFolds.Add("Fold1", () => { GUILayout.Label("Fold1 SecondAdd"); });
            miscFolds.Add("TitleAction", () => { GUILayout.Label("TitleAction"); }).SetTitleAction(() => _bool = GUILayout.Toggle(_bool, "Custom Title Action"));
            //miscFolds.Add(-1, "FoldCustomOrder", () => { GUILayout.Label("FoldCustomOrder"); });
            miscFolds.Add("IDebugMenu", typeof(IDebugMenuExample));
            dynamicFolds.Add("DynamicFold", () => _dynamicFoldEnable, () => { GUILayout.Label("DynamicFold"); });


            // if WindowLaunchers.isWindow == true(default)
            // WindowLaunchers will be wrapped in window.
            // child windows automaticaly aligned.
            launchers = new WindowLaunchers();
            launchers.name = "WindowLaunchers";
            launchers.Add("RGUI.Field()", typeof(FieldExample));
            launchers.Add("RGUI.Slider()", typeof(SliderExample));
            launchers.Add("RGUI.MinMaxSlider()", typeof(MinMaxSliderExample));
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