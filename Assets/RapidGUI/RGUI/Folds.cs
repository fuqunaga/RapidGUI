using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RapidGUI
{
    public class Folds : TitleContents<Fold>
    {
        List<Fold> folds = new List<Fold>();

        public bool DoGUI()
        {
            var ret = false;

            if (dicChanged)
            {
                folds = dic.Values.ToList();
                dicChanged = false;
            }

            using (new GUILayout.VerticalScope())
            {
                ret = folds.Aggregate(false, (changed, fold) => changed || fold.DoGUI());
            }
            return ret;
        }
    }
}