using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    /// GUIUtil.Slider() examples
    /// </summary>
    public class MinMaxSliderExample : ExampleBase
    {
        public int intValMin;
        public int intValMax;
        public float floatValMin;
        public float floatValMax;
        public Vector2 vector2ValMin;
        public Vector2 vector2ValMax;
        public Vector3 vector3ValMin;
        public Vector3 vector3ValMax;
        public Vector4 vector4ValMin;
        public Vector4 vector4ValMax;
        public Vector2Int vector2IntValMin;
        public Vector2Int vector2IntValMax;
        public Vector3Int vector3IntValMin;
        public Vector3Int vector3IntValMax;
        public Rect rectValMin;
        public Rect rectValMax;
        public RectInt rectIntValMin;
        public RectInt rectIntValMax;
        public RectOffset rectOffsetValMin;
        public RectOffset rectOffsetValMax;
        public Bounds boundsValMin;
        public Bounds boundsValMax;
        public BoundsInt boundsIntValMin;
        public BoundsInt boundsIntValMax;

        protected override string title => "RGUI.MinMaxSlider()";

        public override void DoGUI()
        {
            using (new GUILayout.HorizontalScope(GUILayout.MinWidth(1500f)))
            {
                using (new GUILayout.VerticalScope())
                {
                    RGUI.MinMaxSlider(ref intValMin, ref intValMax, 100, "int");
                    RGUI.MinMaxSlider(ref floatValMin, ref floatValMax, "float");


                    RGUI.MinMaxSlider(ref vector2ValMin, ref vector2ValMax, Vector2.one, "vector2");

                    RGUI.MinMaxSlider(ref vector3ValMin, ref vector3ValMax, Vector3.one, "vector3");
                    RGUI.MinMaxSlider(ref vector4ValMin, ref vector4ValMax, Vector4.one, "vector4");

                    RGUI.MinMaxSlider(ref vector2IntValMin, ref vector2IntValMax, Vector2Int.one * 100, "vector2Int");
                    RGUI.MinMaxSlider(ref vector3IntValMin, ref vector3IntValMax, Vector3Int.one * 100, "vector3Int");

                    RGUI.MinMaxSlider(ref rectValMin, ref rectValMax, new Rect(Vector2.one, Vector2.one), "rect");
                    RGUI.MinMaxSlider(ref rectIntValMin, ref rectIntValMax, new RectInt(Vector2Int.one * 100, Vector2Int.one * 100), "rectInt");

                    RGUI.MinMaxSlider(ref rectOffsetValMin, ref rectOffsetValMax, new RectOffset(100, 100, 100, 100), "rectOffset");
                }

                using (new GUILayout.VerticalScope())
                {
                    RGUI.MinMaxSlider(ref boundsValMin, ref boundsValMax, new Bounds(Vector3.one, Vector3.one), "bounds");
                    RGUI.MinMaxSlider(ref boundsIntValMin, ref boundsIntValMax, new BoundsInt(Vector3Int.one * 100, Vector3Int.one * 100), "boundsInt");
                }
            }
        }
    }
}