using System.Collections.Generic;
using UnityEngine;


namespace FuGUI.Example
{
    /// <summary>
    /// GUIUtil.Slider() examples
    /// </summary>
    public class SliderExample : MonoBehaviour
    {
        public int intVal;
        [Range(0,1)]
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

        public List<float> listVal;


        private void OnGUI()
        {
            GUILayout.Label("<b>GUIUtil.Slider()</b>");

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope())
                {
                    intVal = GUIUtil.Slider(intVal, 100, "int");
                    floatVal = GUIUtil.Slider(floatVal, "float");
                    vector2Val = GUIUtil.Slider(vector2Val, Vector2.one, "vector2");
                    vector3Val = GUIUtil.Slider(vector3Val, Vector3.one, "vector3");
                    vector4Val = GUIUtil.Slider(vector4Val, Vector4.one, "vector4");

                    vector2IntVal = GUIUtil.Slider(vector2IntVal, Vector2Int.one * 100, "vector2Int");
                    vector3IntVal = GUIUtil.Slider(vector3IntVal, Vector3Int.one * 100, "vector3Int");

                    rectVal = GUIUtil.Slider(rectVal, new Rect(Vector2.one, Vector2.one), "rect");
                    rectIntVal = GUIUtil.Slider(rectIntVal, new RectInt(Vector2Int.one * 100, Vector2Int.one * 100), "rectInt");
                    rectOffsetVal = GUIUtil.Slider(rectOffsetVal, new RectOffset(100, 100, 100, 100), "rectOffset");
                }

                using (new GUILayout.VerticalScope())
                {
                    boundsVal = GUIUtil.Slider(boundsVal, new Bounds(Vector3.one, Vector3.one), "bounds");
                    boundsIntVal = GUIUtil.Slider(boundsIntVal, new BoundsInt(Vector3Int.one * 100, Vector3Int.one * 100), "boundsInt");
                }
            }
        }
    }
}