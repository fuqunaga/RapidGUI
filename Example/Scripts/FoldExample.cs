using UnityEngine;


namespace RapidGUI.Example
{
    public class FoldExample : ExampleBase
    {
        Fold fold;
        Folds folds;

        public bool isEnable = true;

        private void Start()
        {
            InitFold();
            InitFolds();
        }

        void InitFold()
        {
            fold = new Fold("Fold");

            // add funcion
            fold.Add(() => GUILayout.Label("Added function"));

            // add function with checkEnableFunc
            // Called only when checkEnableFunc returns true.
            fold.Add(
                () => isEnable,
                () => GUILayout.Label("With checkEnableFunc.")
                );

            // add title label action
            fold.SetTitleAction(() => GUILayout.Label("Title Action"));

            // set open first
            fold.Open();
        }


        void InitFolds()
        {
            folds = new Folds();

            // Add() returns Fold. so you can method chains.
            folds.Add("Simple Add()", () => GUILayout.Label("This is Folds."))
                .SetTitleAction(() => GUILayout.Label("Title Action"))
                .Open();

            // if name used already, it will margined.
            folds.Add("Simple Add()", () => GUILayout.Label("added by same name"));

            folds.Add("With checkEnableFunc.",
                () => isEnable,
                () => GUILayout.Label("Displayed only when checkEnableFunc return true.")
            );

            folds.Add("finds the type of IDoGUI in the scene.", typeof(FieldExample));
        }



        protected override string title => "Fold, Folds";

        public override void DoGUI()
        {
            isEnable = GUILayout.Toggle(isEnable, "checkEnableFunc return value");

            using (new GUILayout.VerticalScope(GUILayout.MinWidth(500f)))
            {
                fold.DoGUI();

                GUILayout.Space(10f);
                GUILayout.Label("<b>Folds</b>");
                folds.DoGUI();
            }
        }
    }
}