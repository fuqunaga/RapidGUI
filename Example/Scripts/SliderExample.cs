using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    /// RGUI.Slider() examples
    /// </summary>
    public class SliderExample : ExampleBase
    {
        public int intVal;
        public float floatVal;
        public Vector2 vector2Val;
        public Vector3 vector3Val;
        public Vector4 vector4Val;
        public Vector2Int vector2IntVal;
        public Vector3Int vector3IntVal;
        public Rect rectVal;
        public RectInt rectIntVal;
        public RectOffset rectOffsetVal;
        public Bounds boundsVal;
        public BoundsInt boundsIntVal;

        public bool isOpenVector2;
        public bool isOpenVector3;
        public bool isOpenVector4;
        public bool isOpenVector2Int;
        public bool isOpenVector3Int;
        public bool isOpenRect;
        public bool isOpenRectInt;
        public bool isOpenRectOffset;
        public bool isOpenBounds;
        public bool isOpenBoundsInt;

        protected override string title => "RGUI.Slider()";


        public override void DoGUI()
        {
            using (new GUILayout.HorizontalScope(GUILayout.MinWidth(1500f)))
            {
                using (new GUILayout.VerticalScope())
                {
                    intVal = RGUI.Slider(intVal, 100, "int");
                    floatVal = RGUI.Slider(floatVal, "float");

                    vector2Val = RGUI.Slider(vector2Val, Vector2.one, "vector2");
                    vector3Val = RGUI.Slider(vector3Val, Vector3.one, "vector3");
                    vector4Val = RGUI.Slider(vector4Val, Vector4.one, "vector4");

                    vector2IntVal = RGUI.Slider(vector2IntVal, Vector2Int.one * 100, "vector2Int");
                    vector3IntVal = RGUI.Slider(vector3IntVal, Vector3Int.one * 100, "vector3Int");

                    rectVal = RGUI.Slider(rectVal, new Rect(Vector2.one, Vector2.one), "rect");
                    rectIntVal = RGUI.Slider(rectIntVal, new RectInt(Vector2Int.one * 100, Vector2Int.one * 100), "rectInt");
                    rectOffsetVal = RGUI.Slider(rectOffsetVal, new RectOffset(100, 100, 100, 100), "rectOffset");
                }

                using (new GUILayout.VerticalScope())
                {
                    boundsVal = RGUI.Slider(boundsVal, new Bounds(Vector3.one, Vector3.one), "bounds");
                    boundsIntVal = RGUI.Slider(boundsIntVal, new BoundsInt(Vector3Int.one * 100, Vector3Int.one * 100), "boundsInt");
                }


                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Label("<b>with isOpen</b>");

                    vector2Val = RGUI.Slider(vector2Val, Vector2.one, "vector2", ref isOpenVector2);
                    vector3Val = RGUI.Slider(vector3Val, Vector3.one, "vector3", ref isOpenVector3);
                    vector4Val = RGUI.Slider(vector4Val, Vector4.one, "vector4", ref isOpenVector4);

                    vector2IntVal = RGUI.Slider(vector2IntVal, Vector2Int.one * 100, "vector2Int", ref isOpenVector2Int);
                    vector3IntVal = RGUI.Slider(vector3IntVal, Vector3Int.one * 100, "vector3Int", ref isOpenVector3Int);

                    rectVal = RGUI.Slider(rectVal, new Rect(Vector2.one, Vector2.one), "rect", ref isOpenRect);
                    rectIntVal = RGUI.Slider(rectIntVal, new RectInt(Vector2Int.one * 100, Vector2Int.one * 100), "rectInt", ref isOpenRectInt);
                    rectOffsetVal = RGUI.Slider(rectOffsetVal, new RectOffset(100, 100, 100, 100), "rectOffset", ref isOpenRectOffset);

                    boundsVal = RGUI.Slider(boundsVal, new Bounds(Vector3.one, Vector3.one), "bounds", ref isOpenBounds);
                    boundsIntVal = RGUI.Slider(boundsIntVal, new BoundsInt(Vector3Int.one * 100, Vector3Int.one * 100), "boundsInt", ref isOpenBoundsInt);
                }
            }
        }
    }
}