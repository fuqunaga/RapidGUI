using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    /// RGUI.MinMaxSlider() examples
    /// </summary>
    public class MinMaxSliderExample : ExampleBase
    {
        public MinMaxInt intVal;
        public MinMaxFloat floatVal;
        public MinMaxVector2 vector2Val;
        public MinMaxVector3 vector3Val;
        public MinMaxVector4 vector4Val;
        public MinMaxVector2Int vector2IntVal;
        public MinMaxVector3Int vector3IntVal;

        public float floatMin;
        public float floatMax;

        protected override string title => "RGUI.MinMaxSlider()";

        public override void DoGUI()
        {
            RGUI.MinMaxSlider(floatVal, "float"); // default range max=1.0f
            RGUI.MinMaxSlider(ref floatMin, ref floatMax, "ref float"); // You can also call it without using MinMax class

            RGUI.MinMaxSlider(intVal, 100, "int");
            RGUI.MinMaxSlider(vector2Val, Vector2.one, "vector2");
            RGUI.MinMaxSlider(vector3Val, Vector3.one, "vector3");
            RGUI.MinMaxSlider(vector4Val, Vector4.one, "vector4");
            RGUI.MinMaxSlider(vector2IntVal, Vector2Int.one * 100, "vector2Int");
            RGUI.MinMaxSlider(vector3IntVal, Vector3Int.one * 100, "vector3Int");
        }
    }
}