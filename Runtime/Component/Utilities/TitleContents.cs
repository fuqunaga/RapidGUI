using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace RapidGUI
{
    public abstract class TitleContents<T> where T : TitleContent<T>, new()
    {
        protected readonly Dictionary<string, T> dic = new Dictionary<string, T>();
        protected bool dicChanged = true;


        public T Add(string title, Action guiAction) => Add(title, null, guiAction);

        public T Add(string title, Func<bool> checkEnableFunc, Action guiAction) => Add(title, checkEnableFunc, () => { guiAction(); return false; });

        public T Add(string title, Func<bool> guiFunc) => Add(title, null, guiFunc);

        public virtual T Add(string title, Func<bool> checkEnableFunc, Func<bool> guiFunc)
        {
            if (dic.TryGetValue(title, out var element))
            {
                element.Add(checkEnableFunc, guiFunc);
            }
            else
            {
                element = new T() { name = title }.Add(checkEnableFunc, guiFunc);
                dic.Add(title, element);
            }

            dicChanged = true;

            return element;
        }

        public T Add(string title, params Type[] iDoGUITypes)
        {
            Assert.IsTrue(iDoGUITypes.All(type => type.GetInterfaces().Contains(typeof(IDoGUI))));

            var iDoGUIs = iDoGUITypes.Select(t => new LazyFindObject(t)).ToList() // exec once.
                .Select(lfo => lfo.GetObject()).Where(o => o != null).Cast<IDoGUI>();   // exec every call.

            return Add(title, () => iDoGUIs.Any(), () => iDoGUIs.ToList().ForEach(idm => idm.DoGUI()));
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