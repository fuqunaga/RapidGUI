using UnityEngine;
using UnityEditor;

namespace RapidGUI.imColorPicker
{

    public class IMColorPickerEditor {

        [MenuItem("Assets/Create/IMColorPreset")]
        public static void CreateAsset()
        {
            CreateAsset<IMColorPreset>();
        }

        static void CreateAsset<Type>() where Type : ScriptableObject
        {
            Type item = ScriptableObject.CreateInstance<Type>();

            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/IMColorPreset.asset");

            AssetDatabase.CreateAsset(item, path);
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = item;
        }
    }
}


