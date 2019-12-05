using System.Collections.Generic;
using UnityEngine;

namespace RapidGUI
{
    //[CreateAssetMenu(menuName = "RapidGUI/Create CursorData")]
    public class CursorData : ScriptableObject
    {
        [System.Serializable]
        public class Data
        {
            public Texture2D tex;
            public Vector2Int hotspot;
        }

        public Data resizeHorizontal;
        public Data resizeVertical;
        public Data resizeUpLeft;
    }
}
