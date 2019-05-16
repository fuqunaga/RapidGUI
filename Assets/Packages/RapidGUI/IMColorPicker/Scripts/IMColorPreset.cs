using UnityEngine;
using System.Collections.Generic;

namespace RapidGUI
{
    public class IMColorPreset : ScriptableObject {

        public List<Color> Colors
        {
            get
            {
                return colors;
            }
        }

        [SerializeField] List<Color> colors = new List<Color>();

        public void Save(Color color)
        {
            colors.Add(color);
        }

        public void Remove(int index)
        {
            colors.RemoveAt(index);
        }

    }

}

