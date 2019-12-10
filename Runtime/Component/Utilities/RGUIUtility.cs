using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUIUtility
    {
        static GUIContent tempContent = new GUIContent();
    

        public static GUIContent TempContent(string text)
        {
            tempContent.text = text;
            tempContent.tooltip = null;
            tempContent.image = null;

            return tempContent;
        }
    }
}