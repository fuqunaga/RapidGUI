using UnityEngine;

namespace FuGUI
{
    public static partial class GUIUtil
    {
        public class PrefixLabelIndentScope : GUI.Scope
        {
            float width;

            public PrefixLabelIndentScope(float width=20f)
            {
                BeginIndent(width);
                PrefixLabelSetting.width -= width;
                this.width = width;
            }

            protected override void CloseScope()
            {
                PrefixLabelSetting.width += width;
                EndIndent();
            }
        }
    }
}