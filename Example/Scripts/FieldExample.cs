using System;
using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    ///  RGUI.Field() examples part1
    /// </summary>
    public class FieldExample : ExampleBase
    {
        public enum EnumSample
        {
            One,
            Two,
            Three,
        };

        [Flags]
        public enum EnumSampleFlags
        {
            Flag001 = 1,
            Flag010 = 2,
            Flag100 = 4,
        }

        public string stringVal;
        public bool boolVal;
        public int intVal;
        public float floatVal;
        public Color colorVal;
        public EnumSample enumVal;
        public EnumSampleFlags enumFlagsVal;

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

        public float[] arrayVal;
        public List<int> listVal;

        protected override string title => "RGUI.Field()s";

        public override void DoGUI()
        {
            stringVal = RGUI.Field(stringVal, "string");
            boolVal = RGUI.Field(boolVal, "bool");
            intVal = RGUI.Field(intVal, "int");
            floatVal = RGUI.Field(floatVal, "float");
            colorVal = RGUI.Field(colorVal, "color");
            enumVal = RGUI.Field(enumVal, "enum");
            enumFlagsVal = RGUI.Field(enumFlagsVal, "enumFlags");
            vector2Val = RGUI.Field(vector2Val, "vector2");
            vector3Val = RGUI.Field(vector3Val, "vector3");
            vector4Val = RGUI.Field(vector4Val, "vector4");
            vector2IntVal = RGUI.Field(vector2IntVal, "vector2Int");
            vector3IntVal = RGUI.Field(vector3IntVal, "vector3Int");
            rectVal = RGUI.Field(rectVal, "rect");
            rectIntVal = RGUI.Field(rectIntVal, "rectInt");
            rectOffsetVal = RGUI.Field(rectOffsetVal, "rectOffset");
            boundsVal = RGUI.Field(boundsVal, "bounds");
            boundsIntVal = RGUI.Field(boundsIntVal, "boundsInt");
            arrayVal = RGUI.Field(arrayVal, "array");
            listVal = RGUI.Field(listVal, "list");

            listVal = RGUI.ListField(listVal, "list with custom element GUI", (list, idx, label) =>
            {
                using (new GUILayout.HorizontalScope())
                {
                    var v = list[idx];
                    v = RGUI.Slider(v, 100, label);
                    if (GUILayout.Button("+")) v++;
                    if (GUILayout.Button("-")) v--;

                    return v;
                }
            });
        }
    }
}