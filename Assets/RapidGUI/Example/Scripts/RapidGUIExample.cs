using UnityEngine;


namespace RapidGUI.Example
{
    public class RapidGUIExample : MonoBehaviour
    {
        Folds fieldFolds = new Folds();
        Folds sliderFolds = new Folds();
        Folds miscFolds = new Folds();
        Folds dynamicFolds = new Folds();

        WindowLauncher windowLauncher = new WindowLauncher("windowLauncher");
        WindowLaunchers launchers = new WindowLaunchers();


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
            miscFolds.Add(-1, "FoldCustomOrder", () => { GUILayout.Label("FoldCustomOrder"); });
            miscFolds.Add("IDebugMenu", typeof(IDebugMenuExample));
            dynamicFolds.Add("DynamicFold", () => _dynamicFoldEnable, () => { GUILayout.Label("DynamicFold"); });

            fieldFolds.Add("Field", () =>
            {
                _string = RGUI.Field(_string, "string");
                _bool = RGUI.Field(_bool, "bool");
                _int = RGUI.Field(_int, "int");
                _float = RGUI.Field(_float, "float");
                _vector2 = RGUI.Field(_vector2, "vector2");
                _vector3 = RGUI.Field(_vector3, "vector3");
                _vector4 = RGUI.Field(_vector4, "vector4");
                _vector2Int = RGUI.Field(_vector2Int, "vector2Int");
                _vector3Int = RGUI.Field(_vector3Int, "vector3Int");
                _rect = RGUI.Field(_rect, "rect");
            }, true);


            sliderFolds.Add("Slider", () =>
            {
                _int = RGUI.Slider(_int, 0, 100, "int");
                _float = RGUI.Slider(_float, "float");

                _vector2 = RGUI.Slider(_vector2, Vector2.zero, Vector2.one, "Slider(Vector2)");
                _vector3 = RGUI.Slider(_vector3, Vector3.zero, Vector3.one, "Slider(Vector3)");
                _vector4 = RGUI.Slider(_vector4, Vector4.zero, Vector4.one, "Slider(Vector4)");
                _vector2Int = RGUI.Slider(_vector2Int, Vector2Int.zero, Vector2Int.one * 100, "Slider(Vector2Int)");
                _vector3Int = RGUI.Slider(_vector3Int, Vector3Int.zero, Vector3Int.one * 100, "Slider(Vector3Int)");
                _rect = RGUI.Slider(_rect, Rect.zero, new Rect(1f, 1f, 1f, 1f), "Slider(Rect)");
            }, true);

            windowLauncher.Add(() => GUILayout.Label("In WindowLauncher"));

            launchers.name = "WindowLaunchers";
            launchers.rect = new Rect(Vector2.up * 500f, Vector2.zero);
            launchers.Add("LauncherItem0", () => GUILayout.Label("In LauncherItem0"));
            launchers.Add("LauncherItem1", () => GUILayout.Label("In LauncherItem1"));
        }

        void OnGUI()
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                using (var v = new GUILayout.VerticalScope(GUILayout.MinWidth(300f)))
                {
                    miscFolds.OnGUI();
                    _dynamicFoldEnable = GUILayout.Toggle(_dynamicFoldEnable, "DynamicFold");
                    dynamicFolds.OnGUI();

                    using (new RGUI.IndentScope())
                    {
                        GUILayout.Label("IndentScope");
                    }

                    using (new RGUI.ColorScope(Color.green))
                    {
                        GUILayout.Label("ColorScope");
                    }

                    using (new RGUI.EnabledScope(false))
                    {
                        GUILayout.Label("EnabledScope");
                    }


                    GUILayout.Box("Popup");
                    var resultIdx = RGUI.PopupOnLastRect(new[] { "Button One", "Button Two", "Button Three" });
                    if (resultIdx >= 0 )
                    {
                        Debug.Log($"Popup: Button{resultIdx + 1}");
                    }

                    windowLauncher.OnGUI();
                }

                fieldFolds.OnGUI();
                sliderFolds.OnGUI();

                launchers.OnGUI();
            }
        }
    }
}