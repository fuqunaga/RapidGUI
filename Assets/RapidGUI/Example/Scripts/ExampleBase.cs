using System;
using System.Collections.Generic;
using UnityEngine;


namespace RapidGUI.Example
{
    public abstract class ExampleBase: MonoBehaviour, IDoGUI
    {
        private void OnGUI()
        {
            if ( transform.parent == null)
            {
                GUILayout.Label($"<b>{title}</b>");
                DoGUI();
            }
        }

        protected abstract string title { get; }

        public abstract void DoGUI();
    }
}