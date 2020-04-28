using System;
using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    /// RGUI.Field() examples part2
    /// </summary>
    public class FieldWithClassExample : ExampleBase
    {
        /// <summary>
        /// Simple Class
        /// - RapidGUI displays member fields with the same rules as unity serialize(public, SerializeField, NonSerialized).
        /// - Field with RangAttribute display a slider
        /// - If field name is long, name and value are displayed in multiple lines
        /// </summary>
        [Serializable]
        public class MyClass
        {
            public int publicField;

            [SerializeField]
            protected int serializeField;

            [NonSerialized]
            public int nonSerializedField;

            [Range(0f, 10f)]
            public float rangeVal;

            [Range(0f, 10f)] // The Range attribute is applied to a variety of numeric types 
            public Vector3Int rangeVec3Int;

            public string longNameFieldWillBeMultiLine;
        }

        /// <summary>
        /// CustomGUIClass
        /// - If you can't add an attribute to a class, you can specify a GUI from outside by using CustomGUI.
        /// - See also Start().
        /// </summary>
        public class CustomGUIClass
        {
            public int value0;
            public int value1;
            public int value2;
        }


        /// <summary>
        /// RobustTestClass
        /// </summary>
        public class RobustTestClass
        {
            public RobustTestClass nullReference;
            public RobustTestClass selfReference; // set self reference at Start()
        }

        /// <summary>
        /// ClassWithIDoGUI
        /// - Class with IDoGUI will automatically call DoGUI within an Array/List and another class
        /// </summary>
        public class ClassWithIDoGUI : IDoGUI
        {
            public int myParam;

            public void DoGUI()
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("In DoGUI");
                    using (new GUILayout.HorizontalScope())
                    {
                        myParam = RGUI.Field(myParam);
                        if (GUILayout.Button("Add10")) myParam += 10;
                    }
                }
            }
        }

        /// <summary>
        /// ClassWithICloneable
        /// - If class is ICloneable or has Copy Constructor then Array/List element will be duplicate when add new element.
        /// </summary>
        public class ClassWithICloneable : ICloneable
        {
            public int myParam;

            public object Clone()
            {
                return new ClassWithICloneable()
                {
                    myParam = myParam
                };
            }
        }




        public MyClass myClass = new MyClass();
        public CustomGUIClass customGUIClass = new CustomGUIClass();
        public RobustTestClass robustTestClass = new RobustTestClass();
        public List<ClassWithIDoGUI> classWithIDoGUIList = new List<ClassWithIDoGUI>();
        public List<ClassWithICloneable> classWithICloneableList = new List<ClassWithICloneable>();

        private void Start()
        {
            robustTestClass.selfReference = robustTestClass; // circular reference

            CustomGUI.Label<CustomGUIClass>("value0", "label changed");
            CustomGUI.IgnoreMember<CustomGUIClass>("value1");
            CustomGUI.AddRange<CustomGUIClass>("value2", new MinMaxFloat() { max = 100f });
        }

        public override void DoGUI()
        {
            myClass = RGUI.Field(myClass, nameof(myClass));
            customGUIClass = RGUI.Field(customGUIClass, nameof(customGUIClass));
            robustTestClass = RGUI.Field(robustTestClass, nameof(robustTestClass));
            
            GUILayout.Label("ClassWithIDoGUI - automatically call DoGUI within an Array/List and another class.");
            classWithIDoGUIList = RGUI.Field(classWithIDoGUIList, nameof(classWithIDoGUIList));

            GUILayout.Label("ClassWithICloneable - element will be duplicated when add new element.");
            classWithICloneableList = RGUI.Field(classWithICloneableList, nameof(classWithICloneableList));
        }
        
        protected override string title => "RGUI.Field() with class";
    }
}