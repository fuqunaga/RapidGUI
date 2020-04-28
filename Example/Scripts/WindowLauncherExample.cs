using System;
using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI.Example
{
    public class WindowLauncherExample : ExampleBase
    {
        WindowLauncher launcher;
        WindowLaunchers launchers;

        public bool isEnable = true;

        private void Start()
        {
            InitFold();
            InitFolds();
        }

        void InitFold()
        {
            launcher = new WindowLauncher("WindowLauncher");

            // add funcion
            launcher.Add(() => GUILayout.Label("Added function"));

            // add function with checkEnableFunc
            // Called only when checkEnableFunc returns true.
            launcher.Add(
                () => isEnable,
                () => GUILayout.Label("With checkEnableFunc.")
                );

            // add title label action
            launcher.SetTitleAction(() => GUILayout.Label("Title Action"));
        }


        void InitFolds()
        {
            launchers = new WindowLaunchers()
            {
                isWindow = false
            };

            // Add() returns Fold. so you can method chains.
            // SetWidth() can set window width.
            launchers.Add("Simple Add()", () => GUILayout.Label("This is WindowLaunchers."))
                .SetTitleAction(() => GUILayout.Label("Title Action"))
                .SetWidth(500f);


            // if name used already, it will margined.
            launchers.Add("Simple Add()", () => GUILayout.Label("added by same name"));

            launchers.Add("With checkEnableFunc.",
                () => isEnable,
                () => GUILayout.Label("Displayed only when checkEnableFunc return true.")
            );

            launchers.Add("finds the type of IDoGUI in the scene.", typeof(FieldExample));
        }



        protected override string title => "WindowLancuer, WindowLancuers";

        public override void DoGUI()
        {
            isEnable = GUILayout.Toggle(isEnable, "checkEnableFunc return value");

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope(GUILayout.MinWidth(500f)))
                {
                    launcher.DoGUI();

                    GUILayout.Space(10f);

                    GUILayout.Label("<b>WindowLaunchers</b>");
                    launchers.DoGUI();
                }
            }
        }
    }
}