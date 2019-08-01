using System;
using UnityEngine;


namespace RapidGUI
{
    /// <summary>
    /// FindObjectOfTypeを呼びまくるのは重いので適度に散らす
    /// </summary>
    public class LazyFindObject
    {
        protected UnityEngine.Object _obj;
        protected Type _type;
        protected int _delayCount;
        const int _delayCountMax = 60;

        public LazyFindObject(Type type)
        {
            _type = type;
        }

        public UnityEngine.Object GetObject()
        {
            if ((Event.current.type == EventType.Layout) && _obj == null)
            {
                if (--_delayCount <= 0)
                {
                    _obj = UnityEngine.Object.FindObjectOfType(_type);
                    _delayCount = UnityEngine.Random.Range(0, _delayCountMax);
                }
            }
            return _obj;
        }
    }
}