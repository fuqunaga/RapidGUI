using System;
using System.Text;
using UnityEngine;

namespace RapidGUI
{
    public static partial class RGUI
    {
        static object RecursiveField(object obj)
        {
            return DoRecursiveSafe(obj, () => DoRecursiveField(obj));
        }

        static object DoRecursiveField(object obj)
        {
            var doGuiObj = obj as IDoGUI;
            if (doGuiObj != null)
            {
                GUILayout.EndHorizontal();

                using (new PrefixLabelIndentScope())
                {
                    doGuiObj.DoGUI();
                }

                GUILayout.BeginHorizontal();
            }
            else
            {
                var type = obj.GetType();

                var multiLine = TypeUtility.IsMultiLine(type);
                if (multiLine)
                {
                    GUILayout.EndHorizontal();

                    using (new PrefixLabelIndentScope())
                    {
                        DoFields(obj, type);
                    }

                    GUILayout.BeginHorizontal();
                }
                else
                {
                    var tmp = PrefixLabelSetting.width;
                    PrefixLabelSetting.width = 0f;

                    DoFields(obj, type);

                    GUILayout.FlexibleSpace();
                    PrefixLabelSetting.width = tmp;
                }
            }

            return obj;
        }

        static StringBuilder tmpStringBuilder = new StringBuilder();
        static void DoFields(object obj, Type type)
        {
            var infos = TypeUtility.GetMemberInfoList(type);
            for (var i = 0; i < infos.Count; ++i)
            {
                var info = infos[i];
                if (CheckIgnoreField(info.Name)) continue;

                var v = info.GetValue(obj);
                var range = info.range;
                var memberType = info.MemberType;
                var elemName = CheckCustomLabel(info.Name) ?? info.label;


                if (range != null)
                {
                    v = Slider(v, range.min, range.max, memberType, elemName);
                }
                else
                {
                    // for the bug that short label will be strange word wrap at unity2019
                    tmpStringBuilder.Clear();
                    tmpStringBuilder.Append(elemName);
                    tmpStringBuilder.Append(" ");

                    v = Field(v, memberType, tmpStringBuilder.ToString());
                }
                info.SetValue(obj, v);
            };
        }
    }
}