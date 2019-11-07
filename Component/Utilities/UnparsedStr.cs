using System;
using UnityEngine;


namespace RapidGUI
{
    /// <summary>
    /// UnparsedStr
    /// temporary display string that could not be parsed
    /// </summary>
    public class UnparsedStr
    {
        #region static

        static string lastStr;
        static int lastControlID;

        public static UnparsedStr Create()
        {
            if (ForcusChecker.IsChanged())
            {
                Reset();
            }

            return new UnparsedStr();
        }

        protected static void Reset()
        {
            lastStr = null;
            lastControlID = 0;
        }

        #endregion


        int controlID;

        protected UnparsedStr()
        {
            controlID = GUIUtility.GetControlID(FocusType.Passive);
        }

        public string Get()
        {
            return hasStr ? lastStr : null;
        }

        public void Set(string str)
        {
            if (str == null)
            {
                if ( hasStr )
                {
                    Reset();
                }
            }
            else
            {
                lastStr = str;
                lastControlID = controlID;
            }
        }

        public bool hasStr => (controlID == lastControlID);

        public bool CanParse(Type type)
        {
            var ret = false;
            var str = Get();

            if ( str != null)
            try
            {
                ret = Convert.ChangeType(str, type).ToString() == str;
            }
            catch { }

            return ret;
        }
    }
}