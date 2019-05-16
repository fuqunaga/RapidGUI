using UnityEngine;


namespace FuGUI.Example
{
    public class GUIUtilExample : MonoBehaviour
    {
        Folds _fieldFolds = new Folds();
        Folds _sliderFolds = new Folds();
        Folds _miscFolds = new Folds();
        Folds _dynamicFolds = new Folds();


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
            _miscFolds.Add("Fold0", () => { GUILayout.Label("Fold0"); });
            _miscFolds.Add("Fold1", () => { GUILayout.Label("Fold1 FirstAdd"); });
            _miscFolds.Add("Fold1", () => { GUILayout.Label("Fold1 SecondAdd"); });
            _miscFolds.Add("TitleAction", () => { GUILayout.Label("TitleAction"); }).SetTitleAction(() => _bool = GUILayout.Toggle(_bool, "Custom Title Action"));
            _miscFolds.Add(-1, "FoldCustomOrder", () => { GUILayout.Label("FoldCustomOrder"); });
            _miscFolds.Add("IDebugMenu", typeof(IDebugMenuExample));
            _dynamicFolds.Add("DynamicFold", () => _dynamicFoldEnable, () => { GUILayout.Label("DynamicFold"); });

            _fieldFolds.Add("Field", () =>
            {
                _string = GUIUtil.Field(_string, "string");
                _bool = GUIUtil.Field(_bool, "bool");
                _int = GUIUtil.Field(_int, "int");
                _float = GUIUtil.Field(_float, "float");
                _vector2 = GUIUtil.Field(_vector2, "vector2");
                _vector3 = GUIUtil.Field(_vector3, "vector3");
                _vector4 = GUIUtil.Field(_vector4, "vector4");
                _vector2Int = GUIUtil.Field(_vector2Int, "vector2Int");
                _vector3Int = GUIUtil.Field(_vector3Int, "vector3Int");
                _rect = GUIUtil.Field(_rect, "rect");
            }, true);


            _sliderFolds.Add("Slider", () =>
            {
                _int = GUIUtil.Slider(_int, 0, 100, "int");
                _float = GUIUtil.Slider(_float, "float");

                _vector2 = GUIUtil.Slider(_vector2, Vector2.zero, Vector2.one, "Slider(Vector2)");
                _vector3 = GUIUtil.Slider(_vector3, Vector3.zero, Vector3.one, "Slider(Vector3)");
                _vector4 = GUIUtil.Slider(_vector4, Vector4.zero, Vector4.one, "Slider(Vector4)");
                _vector2Int = GUIUtil.Slider(_vector2Int, Vector2Int.zero, Vector2Int.one * 100, "Slider(Vector2Int)");
                _vector3Int = GUIUtil.Slider(_vector3Int, Vector3Int.zero, Vector3Int.one * 100, "Slider(Vector3Int)");
                _rect = GUIUtil.Slider(_rect, Rect.zero, new Rect(1f, 1f, 1f, 1f), "Slider(Rect)");
            }, true);
        }

        void OnGUI()
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                using (var v = new GUILayout.VerticalScope(GUILayout.MinWidth(300f)))
                {
                    _miscFolds.OnGUI();
                    _dynamicFoldEnable = GUILayout.Toggle(_dynamicFoldEnable, "DynamicFold");
                    _dynamicFolds.OnGUI();
                    _int = GUIUtil.IntButton(_int, "IntButton");

                    using (new GUIUtil.IndentScope())
                    {
                        GUILayout.Label("Indent");
                    }

                    using (var cs = new GUIUtil.ColorScope(Color.green))
                    {
                        GUILayout.Label("ColorScope");
                    }

                    GUILayout.Box("Popup");
                    var rect = GUILayoutUtility.GetLastRect();
                    var resultIdx = GUIUtil.Popup(rect, new[] { "Button One", "Button Two", "Button Three" });
                    if (resultIdx >= 0 )
                    {
                        Debug.Log($"Popup: Button{resultIdx + 1}");
                    }
                }

                _fieldFolds.OnGUI();
                _sliderFolds.OnGUI();
            }
        }
    }
}