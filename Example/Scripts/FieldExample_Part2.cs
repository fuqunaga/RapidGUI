using System;
using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI.Example
{
    /// <summary>
    /// RGUI.Field() examples part2
    /// </summary>
    public class FieldExample_Part2 : ExampleBase
    {
        /// <summary>
        /// Simple Class
        /// - RapidGUI displays member fields with the same rules as unity serialize(public, SerializeField, NonSerialized).
        /// - Field with RangAttribute display a slider
        /// - If field name is long, name and value are displayed in multiple lines
        /// </summary>
        [Serializable]
        public class CustomClass
        {
            public int publicField;

            [SerializeField]
            protected int serializeField;

            [NonSerialized]
            public int nonSerializedField;

            [Range(0f, 10f)]
            public float rangeVal;

            public string longNameFieldWillBeMultiLine;
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
        ///  Class with IDoGUI will automatically call DoGUI within an Array/List and another class
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
        /// - if class is ICloneable or has Copy Constructor then Array/List element will be duplicate when add new element.
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


        public CustomClass customClass = new CustomClass();
        public RobustTestClass robustTestClass = new RobustTestClass();
        public List<ClassWithIDoGUI> classWithIDoGUIList = new List<ClassWithIDoGUI>();
        public List<ClassWithICloneable> classWithICloneableList = new List<ClassWithICloneable>();

        private void Start()
        {
            robustTestClass.selfReference = robustTestClass; // circular reference
        }
        
        public override void DoGUI()
        {
            customClass = RGUI.Field(customClass, nameof(customClass));
            robustTestClass = RGUI.Field(robustTestClass, nameof(robustTestClass));
            
            GUILayout.Label("ClassWithIDoGUI - automatically call DoGUI within an Array/List and another class.");
            classWithIDoGUIList = RGUI.Field(classWithIDoGUIList, nameof(classWithIDoGUIList));
            
            GUILayout.Label("ClassWithICloneable - element will be duplicate when add new element.");
            classWithICloneableList = RGUI.Field(classWithICloneableList, nameof(classWithICloneableList));
        }
        
        protected override string title => "RGUI.Field() Part2";
    }
}