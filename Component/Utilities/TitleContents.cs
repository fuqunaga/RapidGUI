using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace RapidGUI
{
    public abstract class TitleContents<T> where T : TitleContent<T>, new()
    {
        protected Dictionary<string, T> dic = new Dictionary<string, T>();
        protected bool dicChanged = true;


        public T Add(string name, Action guiAction) => Add(name, null, guiAction);

        public T Add(string name, Func<bool> checkEnableFunc, Action guiAction) => Add(name, checkEnableFunc, () => { guiAction(); return false; });

        public T Add(string name, Func<bool> guiFunc) => Add(name, null, guiFunc);

        public virtual T Add(string name, Func<bool> checkEnableFunc, Func<bool> guiFunc)
        {
            if (dic.TryGetValue(name, out var element))
            {
                element.Add(checkEnableFunc, guiFunc);
            }
            else
            {
                element = new T() { name = name }.Add(checkEnableFunc, guiFunc);
                dic.Add(name, element);
            }

            dicChanged = true;

            return element;
        }

        public T Add(string name, params Type[] iDoGUITypes)
        {
            Assert.IsTrue(iDoGUITypes.All(type => type.GetInterfaces().Contains(typeof(IDoGUI))));

            var iDoGUIs = iDoGUITypes.Select(t => new LazyFindObject(t)).ToList() // exec once.
                .Select(lfo => lfo.GetObject()).Where(o => o != null).Cast<IDoGUI>();   // exec every call.

            return Add(name, () => iDoGUIs.Any(), () => iDoGUIs.ToList().ForEach(idm => idm.DoGUI()));
        }


        public bool Contains(string name)
        {
            return dic.ContainsKey(name);
        }

        public void Remove(string name)
        {
            if (dic.ContainsKey(name))
            {
                dic.Remove(name);
            }

            dicChanged = true;
        }
    }
}