using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        public class PrefixLabelIndentScope : GUI.Scope
        {
            float width;

            public PrefixLabelIndentScope(float width=32f)
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