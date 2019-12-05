using System;
using System.Collections.Generic;
using System.Linq;


namespace RapidGUI
{
    /// <summary>
    /// Title and content that opens and closes
    /// </summary>
    public abstract class TitleContent<T> where T : TitleContent<T>
    {
        protected class FuncData
        {
            public Func<bool> checkEnableFunc;
            public Func<bool> guiFunc;
        }

        public string name;

        public bool isOpen { get; protected set; }
        protected Action titleAction;
        protected List<FuncData> funcDatas = new List<FuncData>();

        public TitleContent() { }

        public TitleContent(string name)
        {
            this.name = name;
        }


        public T Add(Action guiAction) => Add(null, guiAction);

        public T Add(Func<bool> checkEnableFunc, Action guiAction) => Add(checkEnableFunc, () => { guiAction(); return false; });

        public T Add(Func<bool> guiFunc) => Add(null, guiFunc);

        public T Add(Func<bool> checkEnableFunc, Func<bool> guiFunc)
        {
            funcDatas.Add(new FuncData()
            {
                checkEnableFunc = checkEnableFunc,
                guiFunc = guiFunc
            });

            return (T)this;
        }

        public T Open() { isOpen = true; return (T)this; }
        public T Close() { isOpen = false; return (T)this; }

        public T SetTitleAction(Action titleAction) { this.titleAction = titleAction; return (T)this; }


        protected IEnumerable<Func<bool>> GetGUIFuncs() => funcDatas.Where(fd => fd.checkEnableFunc?.Invoke() ?? true).Select(fd => fd.guiFunc);
    }
}