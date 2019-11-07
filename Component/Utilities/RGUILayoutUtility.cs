using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public static class RGUILayoutUtility
{

    public static bool IsInEditorWindow()
    {
#if UNITY_EDITOR

        var pos = GUIUtility.GUIToScreenRect(GUILayoutUtility.GetRect(0f, 0f, GUILayout.ExpandWidth(false))).position;

        return Resources.FindObjectsOfTypeAll<EditorWindow>()
            .Where(ew => ew.position.Contains(pos))
            .Where(ew => ew.GetType().Name != "GameView")
            .Any();
#else
        return false;
#endif
    }
}