using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapidGUI
{
    /// <summary>
    /// FastScrollView: ignore items out of range.
    /// call SetNeedUpdateLayout() if height/count of items changed.
    /// </summary>
    public class FastScrollViewVertical
    {
        bool needUpdateLayout = true;

        public Vector2 scrollPosition;
        public float scrollViewHeight;

        List<float> yMaxList = new List<float>();

        public void SetNeedUpdateLayout() => needUpdateLayout = true;

        public void DoGUI<T>(IEnumerable<T> items, Action<T> doGUIItem)
        {
            var evType = Event.current.type;
            
            using (var sv = new GUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = sv.scrollPosition;

                using (new GUILayout.VerticalScope())
                {
                    if (needUpdateLayout)
                    {
                        var itemList = items.ToList();

                        if (evType == EventType.Repaint)
                        {
                            yMaxList.Clear();

                            itemList.ForEach(item =>
                            {
                                doGUIItem(item);
                                var rect = GUILayoutUtility.GetLastRect();
                                yMaxList.Add(rect.yMax);
                            });

                            needUpdateLayout = false;
                        }
                        else
                        {
                            itemList.ForEach(item => doGUIItem(item));
                        }
                    }
                    else
                    {
                        var startIdx = Mathf.Max(0, yMaxList.FindIndex(y => y > scrollPosition.y));
                        var endPos = scrollPosition.y + scrollViewHeight;
                        var endIdx = Mathf.Min(yMaxList.Count - 1, yMaxList.FindLastIndex(y => y < endPos) + 1);

                        if (startIdx > 0) GUILayout.Space(yMaxList[startIdx - 1]);

                        items
                            .Skip(startIdx)
                            .Take(endIdx - startIdx + 1)
                            .ToList()
                            .ForEach(item => doGUIItem(item));

                        if (endIdx < yMaxList.Count - 1) GUILayout.Space(yMaxList.Last() - yMaxList[endIdx]);
                    }
                }
            }



            if (evType == EventType.Repaint)
            {
                scrollViewHeight = GUILayoutUtility.GetLastRect().height;
            }
        }
    }
}