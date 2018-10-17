using System.Collections.Generic;
using UnityEngine;


namespace FuGUI.Example
{
    /// <summary>
    /// GUIUtil.Field() sample
    /// </summary>
    public class FieldExample : MonoBehaviour
    {
        public enum EnumSample
        {
            One,
            Two,
            Three,
        };

        [System.Flags]
        public enum EnumSmapleFlags
        {
            Flag001 = 1,
            Flag010 = 2,
            Flag100 = 4,
        }

        public class CustomClass
        {
            public int intVal;
            public float floatVal;
            public string stringVal;
            public List<Color> colos;
        }

        public string stringVal;
        public bool boolVal;
        public int intVal;
        public float floatVal;
        public Color colorVal;
        public EnumSample enumVal;
        public EnumSmapleFlags enumFlagsVal;

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

        public int[] arrayVal;
        public List<float> listVal;

        public CustomClass customClassVal = new CustomClass();
        public List<CustomClass> customClassListVal;

        Vector2 scrollPosition;

        private void OnGUI()
        {
            GUILayout.Label("<b>GUIUtil.Field()</b>");

            using (var sc = new GUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = sc.scrollPosition;

                stringVal = GUIUtil.Field(stringVal, "string");
                boolVal = GUIUtil.Field(boolVal, "bool");

                intVal = GUIUtil.Field(intVal, "int");
                floatVal = GUIUtil.Field(floatVal, "float");
                colorVal = GUIUtil.Field(colorVal, "color");
                enumVal = GUIUtil.Field(enumVal, "enum");
                enumFlagsVal = GUIUtil.Field(enumFlagsVal, "enumFlags");

                vector2Val = GUIUtil.Field(vector2Val, "vector2");
                vector3Val = GUIUtil.Field(vector3Val, "vector3");
                vector4Val = GUIUtil.Field(vector4Val, "vector4");

                vector2IntVal = GUIUtil.Field(vector2IntVal, "vector2Int");
                vector3IntVal = GUIUtil.Field(vector3IntVal, "vector3Int");

                rectVal = GUIUtil.Field(rectVal, "rect");
                rectIntVal = GUIUtil.Field(rectIntVal, "rectInt");
                rectOffsetVal = GUIUtil.Field(rectOffsetVal, "rectOffset");

                boundsVal = GUIUtil.Field(boundsVal, "bounds");
                boundsIntVal = GUIUtil.Field(boundsIntVal, "boundsInt");

                arrayVal = GUIUtil.Field(arrayVal, "array");
                listVal = GUIUtil.Field(listVal, "list");

                customClassVal = GUIUtil.Field(customClassVal, "customClass");
                customClassListVal = GUIUtil.Field(customClassListVal, "customClassList");
            }
        }
    }
}