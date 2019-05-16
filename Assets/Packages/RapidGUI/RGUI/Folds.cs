using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


namespace RapidGUI
{
    public class Folds
    {
        public class FoldData
        {
            public int _order;
            public Fold _fold;
        }

        Dictionary<string, FoldData> _dic = new Dictionary<string, FoldData>();
        bool _needUpdate = true;


        #region Add 

        public Fold Add(string name, Func<bool> drawFunc, bool enableFirst = false) => Add(name, null, drawFunc, enableFirst);

        public Fold Add(string name, Func<bool> checkEnableFunc, Func<bool> drawFunc, bool enableFirst = false) => Add(0, name, checkEnableFunc, drawFunc, enableFirst);

        public Fold Add(int order, string name, Func<bool> drawFunc, bool enableFirst = false) => Add(order, name, null, drawFunc, enableFirst);

        public Fold Add(int order, string name, Func<bool> checkEnableFunc, Func<bool> drawFunc, bool enableFirst = false)
        {
            Fold ret;
            FoldData foldData;
            if (_dic.TryGetValue(name, out foldData))
            {
                foldData._order = order;
                foldData._fold.Add(checkEnableFunc, drawFunc);
                ret = foldData._fold;
            }
            else
            {
                ret = new Fold(name, checkEnableFunc, drawFunc, enableFirst);
                _dic.Add(name, new FoldData
                {
                    _order = order,
                    _fold = ret,
                });
            }

            _needUpdate = true;

            return ret;
        }

        public Fold Add(string name, Action drawFunc, bool enableFirst = false) => Add(name, null, drawFunc, enableFirst);

        public Fold Add(string name, Func<bool> checkEnableFunc, Action drawFunc, bool enableFirst = false) => Add(0, name, checkEnableFunc, drawFunc, enableFirst);

        public Fold Add(int order, string name, Action drawFunc, bool enableFirst = false) => Add(order, name, null, drawFunc, enableFirst);

        public Fold Add(int order, string name, Func<bool> checkEnableFunc, Action drawFunc, bool enableFirst = false) => Add(order, name, checkEnableFunc, () => { drawFunc(); return false; }, enableFirst);

        #endregion


        public bool Contains(string name)
        {
            return _dic.ContainsKey(name);
        }

        public void Remove(string name)
        {
            if (_dic.ContainsKey(name))
            {
                _dic.Remove(name);
            }

            _needUpdate = true;
        }


        List<Fold> _folds = new List<Fold>();
        public bool OnGUI()
        {
            var ret = false;

            if (_needUpdate)
            {
                _folds = _dic.Values.OrderBy(of => of._order).Select(of => of._fold).ToList();
                _needUpdate = false;
            }

            using (var v = new GUILayout.VerticalScope())
            {
                ret = _folds.Aggregate(false, (changed, fold) => changed || fold.OnGUI());
            }
            return ret;
        }
    }

    public static class FoldsExtention
    {
        public static void Add(this Folds folds, string name, params Type[] iDebugMenuTypes)
        {
            folds.Add(name, false, iDebugMenuTypes);
        }

        public static void Add(this Folds folds, string name, bool enableFirst, params Type[] iDebugMenuTypes)
        {
            folds.Add(0, name, enableFirst, iDebugMenuTypes);
        }

        public static void Add(this Folds folds, int order, string name, params Type[] iDebugMenuTypes)
        {
            folds.Add(order, name, false, iDebugMenuTypes);
        }

        public static void Add(this Folds folds, int order, string name, bool enableFirst, params Type[] iDebugMenuTypes)
        {
            Assert.IsTrue(iDebugMenuTypes.All(type => type.GetInterfaces().Contains(typeof(IDebugMenu))));

            var iDebugMenus = iDebugMenuTypes.Select(t => new LazyFindObject(t)).ToList() // exec once.
                .Select(lfo => lfo.GetObject()).Where(o => o != null).Cast<IDebugMenu>();   // exec every call.

            folds.Add(order, name, () => iDebugMenus.Any(), () => iDebugMenus.ToList().ForEach(idm => idm.DebugMenu()), enableFirst);
        }
    }
}