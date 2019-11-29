using UnityEngine;


namespace RapidGUI
{
    public class MinMaxSliderCore
    {
        public class Style
        {
            public static GUIStyle minMaxSliderThumb;

            static Style()
            {
                InitStyle();
            }

            static void InitStyle()
            {
                minMaxSliderThumb = new GUIStyle()
                {
                    border = new RectOffset(7, 7, 0, 0),
                    clipping = TextClipping.Clip,
                    fixedHeight = 12f,
                    imagePosition = ImagePosition.ImageOnly,
                    name = "MinMaxHorizontalSliderThumb",
                    //overflow = new RectOffset(2, 2, 2, 2),
                    padding = new RectOffset(7, 7, 0, 0),
                    richText = false,
                };

                var normalTex = Resources.Load<Texture2D>("minmax slider thumb");
                var activeTex = Resources.Load<Texture2D>("minmax slider thumb act");

                minMaxSliderThumb.normal.background = normalTex;
                minMaxSliderThumb.active.background = activeTex;
            }
        }



        public static void MinMaxSlider(Rect position, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
        {
            int id = GUIUtility.GetControlID(s_MinMaxSliderHash, FocusType.Passive);
            DoMinMaxSlider(position, id, ref minValue, ref maxValue, minLimit, maxLimit);
        }


        static void DoMinMaxSlider(Rect position, int id, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
        {
            float size = maxValue - minValue;

            DoMinMaxSlider(position, id, ref minValue, ref size, minLimit, maxLimit, minLimit, maxLimit, GUI.skin.horizontalSlider, Style.minMaxSliderThumb, true);
            maxValue = minValue + size;
        }



        #region based on EditorGUIExt.cs


        // State for when we're dragging a MinMax slider.
        class MinMaxSliderState
        {
            public float dragStartPos = 0;      // Start of the drag (mousePosition)
            public float dragStartValue = 0;        // Value at start of drag.
            public float dragStartSize = 0;     // Size at start of drag.
            public float dragStartValuesPerPixel = 0;
            public float dragStartLimit = 0;        // start limit at start of drag
            public float dragEndLimit = 0;      // end limit at start of drag
            public int whereWeDrag = -1;        // which part are we dragging? 0 = middle, 1 = min, 2 = max, 3 = min trough, 4 = max trough
        }

        static MinMaxSliderState s_MinMaxSliderState;
        static int kFirstScrollWait = 250; // ms
        static int kScrollWait = 30; // ms
        static System.DateTime s_NextScrollStepTime = System.DateTime.Now; // whatever but null

        // Mouse down position for
        private static Vector2 s_MouseDownPos = Vector2.zero;
        // Are we doing a drag selection (as opposed to when the mousedown was over a selection rect)
        enum DragSelectionState
        {
            None, DragSelecting, Dragging
        }
        //static DragSelectionState s_MultiSelectDragSelection = DragSelectionState.None;
        static Vector2 s_StartSelectPos = Vector2.zero;
        //static List<bool> s_SelectionBackup = null;
        //static List<bool> s_LastFrameSelections = null;
        internal static int s_MinMaxSliderHash = "MinMaxSliderCore".GetHashCode();
        /// Make a double-draggable slider that will let you specify a range of values.
        /// @param position where to draw it
        /// @param value the current start position
        /// @param size the size of the covered range
        /// @param visualStart what is displayed as the start of the range. The user can drag beyond this, but the displays shows this as the limit. Set this to be the start of the relevant data.
        /// @param visualEnd what is displayed as the end of the range. The user can drag beyond this, but the displays shows this as the limit. Set this to be the end of the relevant data.
        /// @param startLimit what is the lowest possible value? The user can never slide beyond this in the minimum direction. If you don't want a limit, set it to -Mathf.Infinity
        /// @param endLimit what is the highes possible value? The user can never slide beyond this in the maximum direction. If you don't want a limit, set it to Mathf.Infinity
        public static void MinMaxSlider(Rect position, ref float value, ref float size, float visualStart, float visualEnd, float startLimit, float endLimit, GUIStyle slider, GUIStyle thumb, bool horiz)
        {
            DoMinMaxSlider(position, GUIUtility.GetControlID(s_MinMaxSliderHash, FocusType.Passive), ref value, ref size, visualStart, visualEnd, startLimit, endLimit, slider, thumb, horiz);
        }

        internal static void DoMinMaxSlider(Rect position, int id, ref float value, ref float size, float visualStart, float visualEnd, float startLimit, float endLimit, GUIStyle slider, GUIStyle thumb, bool horiz)
        {
            Event evt = Event.current;
            bool usePageScrollbars = size == 0;

            float minVisual = Mathf.Min(visualStart, visualEnd);
            float maxVisual = Mathf.Max(visualStart, visualEnd);
            float minLimit = Mathf.Min(startLimit, endLimit);
            float maxLimit = Mathf.Max(startLimit, endLimit);

            MinMaxSliderState state = s_MinMaxSliderState;

            if (GUIUtility.hotControl == id && state != null)
            {
                minVisual = state.dragStartLimit;
                minLimit = state.dragStartLimit;
                maxVisual = state.dragEndLimit;
                maxLimit = state.dragEndLimit;
            }

            float minSize = 0;

            float displayValue = Mathf.Clamp(value, minVisual, maxVisual);
            float displaySize = Mathf.Clamp(value + size, minVisual, maxVisual) - displayValue;

            float sign = visualStart > visualEnd ? -1 : 1;


            if (slider == null || thumb == null)
                return;

            // Figure out the rects
            float pixelsPerValue;
            float mousePosition;
            Rect thumbRect;
            Rect thumbMinRect, thumbMaxRect;
            if (horiz)
            {
                float thumbSize = thumb.fixedWidth != 0 ? thumb.fixedWidth : thumb.padding.horizontal;
                pixelsPerValue = (position.width - slider.padding.horizontal - thumbSize) / (maxVisual - minVisual);
                thumbRect = new Rect(
                    (displayValue - minVisual) * pixelsPerValue + position.x + slider.padding.left,
                    position.y + slider.padding.top,
                    displaySize * pixelsPerValue + thumbSize,
                    position.height - slider.padding.vertical);
                thumbMinRect = new Rect(thumbRect.x, thumbRect.y, thumb.padding.left, thumbRect.height);
                thumbMaxRect = new Rect(thumbRect.xMax - thumb.padding.right, thumbRect.y, thumb.padding.right, thumbRect.height);
                mousePosition = evt.mousePosition.x - position.x;
            }
            else
            {
                float thumbSize = thumb.fixedHeight != 0 ? thumb.fixedHeight : thumb.padding.vertical;
                pixelsPerValue = (position.height - slider.padding.vertical - thumbSize) / (maxVisual - minVisual);
                thumbRect = new Rect(
                    position.x + slider.padding.left,
                    (displayValue - minVisual) * pixelsPerValue + position.y + slider.padding.top,
                    position.width - slider.padding.horizontal,
                    displaySize * pixelsPerValue + thumbSize);
                thumbMinRect = new Rect(thumbRect.x, thumbRect.y, thumbRect.width, thumb.padding.top);
                thumbMaxRect = new Rect(thumbRect.x, thumbRect.yMax - thumb.padding.bottom, thumbRect.width, thumb.padding.bottom);
                mousePosition = evt.mousePosition.y - position.y;
            }

            float mousePos;
            float thumbPos;
            switch (evt.GetTypeForControl(id))
            {
                case EventType.MouseDown:
                    // if the click is outside this control, just bail out...
                    if (evt.button != 0 || !position.Contains(evt.mousePosition) || minVisual - maxVisual == 0)
                        return;
                    if (state == null)
                        state = s_MinMaxSliderState = new MinMaxSliderState();

                    // These are required to be set whenever we grab hotcontrol, regardless of if we actually drag or not. (case 585577)
                    state.dragStartLimit = startLimit;
                    state.dragEndLimit = endLimit;

                    if (thumbRect.Contains(evt.mousePosition))
                    {
                        // We have a mousedown on the thumb
                        // Record where we're draging from, so the user can get back.
                        state.dragStartPos = mousePosition;
                        state.dragStartValue = value;
                        state.dragStartSize = size;
                        state.dragStartValuesPerPixel = pixelsPerValue;
                        if (thumbMinRect.Contains(evt.mousePosition))
                            state.whereWeDrag = 1;
                        else if (thumbMaxRect.Contains(evt.mousePosition))
                            state.whereWeDrag = 2;
                        else
                            state.whereWeDrag = 0;

                        GUIUtility.hotControl = id;
                        evt.Use();
                        return;
                    }
                    else
                    {
                        // We're outside the thumb, but inside the trough.
                        // If we have no background, we just bail out.
                        if (slider == GUIStyle.none)
                            return;

                        // If we have a scrollSize, we do pgup/pgdn style movements
                        // if not, we just snap to the current position and begin tracking
                        if (size != 0 && usePageScrollbars)
                        {
                            if (horiz)
                            {
                                if (mousePosition > thumbRect.xMax - position.x)
                                    value += size * sign * .9f;
                                else
                                    value -= size * sign * .9f;
                            }
                            else
                            {
                                if (mousePosition > thumbRect.yMax - position.y)
                                    value += size * sign * .9f;
                                else
                                    value -= size * sign * .9f;
                            }
                            state.whereWeDrag = 0;
                            GUI.changed = true;
                            s_NextScrollStepTime = System.DateTime.Now.AddMilliseconds(kFirstScrollWait);

                            mousePos = horiz ? evt.mousePosition.x : evt.mousePosition.y;
                            thumbPos = horiz ? thumbRect.x : thumbRect.y;

                            state.whereWeDrag = mousePos > thumbPos ? 4 : 3;
                        }
                        else
                        {
                            if (horiz)
                                value = ((float)mousePosition - thumbRect.width * .5f) / pixelsPerValue + minVisual - size * .5f;
                            else
                                value = ((float)mousePosition - thumbRect.height * .5f) / pixelsPerValue + minVisual - size * .5f;
                            state.dragStartPos = mousePosition;
                            state.dragStartValue = value;
                            state.dragStartSize = size;
                            state.dragStartValuesPerPixel = pixelsPerValue;
                            state.whereWeDrag = 0;
                            GUI.changed = true;
                        }
                        GUIUtility.hotControl = id;
                        value = Mathf.Clamp(value, minLimit, maxLimit - size);
                        evt.Use();
                        return;
                    }
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl != id)
                        return;

                    // Recalculate the value from the mouse position. this has the side effect that values are relative to the
                    // click point - no matter where inside the trough the original value was. Also means user can get back original value
                    // if he drags back to start position.
                    float deltaVal = (mousePosition - state.dragStartPos) / state.dragStartValuesPerPixel;
                    switch (state.whereWeDrag)
                    {
                        case 0: // normal drag
                            value = Mathf.Clamp(state.dragStartValue + deltaVal, minLimit, maxLimit - size);
                            break;
                        case 1:// min size drag
                            value = state.dragStartValue + deltaVal;
                            size = state.dragStartSize - deltaVal;
                            if (value < minLimit)
                            {
                                size -= minLimit - value;
                                value = minLimit;
                            }
                            if (size < minSize)
                            {
                                value -= minSize - size;
                                size = minSize;
                            }
                            break;
                        case 2:// max size drag
                            size = state.dragStartSize + deltaVal;
                            if (value + size > maxLimit)
                                size = maxLimit - value;
                            if (size < minSize)
                                size = minSize;
                            break;
                    }
                    GUI.changed = true;
                    evt.Use();
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id)
                    {
                        evt.Use();
                        GUIUtility.hotControl = 0;
                    }
                    break;
                case EventType.Repaint:
                    slider.Draw(position, GUIContent.none, id);
                    thumb.Draw(thumbRect, GUIContent.none, id);

#if false
                    EditorGUIUtility.AddCursorRect(thumbMinRect, horiz ? MouseCursor.ResizeHorizontal : MouseCursor.ResizeVertical, state != null && state.whereWeDrag == 1 ? id : -1);
                    EditorGUIUtility.AddCursorRect(thumbMaxRect, horiz ? MouseCursor.ResizeHorizontal : MouseCursor.ResizeVertical, state != null && state.whereWeDrag == 2 ? id : -1);
#else
                    var hasControl = (GUIUtility.hotControl == id) && (state != null);
                    var draggingThumb = hasControl && (state.whereWeDrag == 1 || state.whereWeDrag == 2);
                    if (draggingThumb ||
                        (!hasControl && (thumbMinRect.Contains(evt.mousePosition) || thumbMaxRect.Contains(evt.mousePosition)))
                        )
                    {
                        RGUIUtility.SetCursor(horiz ? MouseCursor.ResizeHorizontal : MouseCursor.ResizeVertical);
                    }
#endif

                    // if the mouse is outside this control, just bail out...
                    if (GUIUtility.hotControl != id ||
                        !position.Contains(evt.mousePosition) || minVisual - maxVisual == 0)
                    {
                        return;
                    }

                    if (thumbRect.Contains(evt.mousePosition))
                    {
                        if (state != null && (state.whereWeDrag == 3 || state.whereWeDrag == 4)) // if was scrolling with "through" and the thumb reached mouse - sliding action over
                            GUIUtility.hotControl = 0;
                        return;
                    }


                    if (System.DateTime.Now < s_NextScrollStepTime)
                        return;

                    mousePos = horiz ? evt.mousePosition.x : evt.mousePosition.y;
                    thumbPos = horiz ? thumbRect.x : thumbRect.y;

                    int currentSide = mousePos > thumbPos ? 4 : 3;
                    if (state != null && currentSide != state.whereWeDrag)
                        return;

                    // If we have a scrollSize, we do pgup/pgdn style movements
                    if (size != 0 && usePageScrollbars)
                    {
                        if (horiz)
                        {
                            if (mousePosition > thumbRect.xMax - position.x)
                                value += size * sign * .9f;
                            else
                                value -= size * sign * .9f;
                        }
                        else
                        {
                            if (mousePosition > thumbRect.yMax - position.y)
                                value += size * sign * .9f;
                            else
                                value -= size * sign * .9f;
                        }
                        if (state != null)
                            state.whereWeDrag = -1;
                        GUI.changed = true;
                    }
                    value = Mathf.Clamp(value, minLimit, maxLimit - size);

                    s_NextScrollStepTime = System.DateTime.Now.AddMilliseconds(kScrollWait);
                    break;
            }
        }
        #endregion
    }

}