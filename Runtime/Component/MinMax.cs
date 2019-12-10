using System;
using UnityEngine;


namespace RapidGUI
{
    [Serializable]
    public class MinMaxInt : MinMax<int>, IMinMaxLerp<int>
    {
        public int LerpUnclamped(float t) => Mathf.FloorToInt(Mathf.LerpUnclamped(min, max, t));
    }

    [Serializable]
    public class MinMaxFloat : MinMax<float>, IMinMaxLerp<float>
    {
        public float LerpUnclamped(float t) => Mathf.LerpUnclamped(min, max, t);
    }

    [Serializable]
    public class MinMaxVector2 : MinMax<Vector2>, IMinMaxLerp<Vector2>
    {
        public Vector2 LerpUnclamped(float t) => Vector2.LerpUnclamped(min, max, t);
    }

    [Serializable]
    public class MinMaxVector3 : MinMax<Vector3>, IMinMaxLerp<Vector3>
    {
        public Vector3 LerpUnclamped(float t) => Vector3.LerpUnclamped(min, max, t);
    }

    [Serializable]
    public class MinMaxVector4 : MinMax<Vector4>, IMinMaxLerp<Vector4>
    {
        public Vector4 LerpUnclamped(float t) => Vector4.LerpUnclamped(min, max, t);
    }

    [Serializable]
    public class MinMaxVector2Int : MinMax<Vector2Int>, IMinMaxLerp<Vector2Int>
    {
        public Vector2Int LerpUnclamped(float t) => Vector2Int.FloorToInt(Vector2.LerpUnclamped(min, max, t));
    }

    [Serializable]
    public class MinMaxVector3Int : MinMax<Vector3Int>, IMinMaxLerp<Vector3Int>
    {
        public Vector3Int LerpUnclamped(float t) => Vector3Int.FloorToInt(Vector3.LerpUnclamped(min, max, t));
    }



    #region Internal

    public class MinMax<T>
    {
        public T min;
        public T max;
    }

    public interface IMinMaxLerp<T>
    {
        T LerpUnclamped(float t);
    }

    public static class IMinMaxLerp
    {
        public static T Lerp<T>(this IMinMaxLerp<T> me, float t)
        {
            return me.LerpUnclamped(Mathf.Clamp01(t));
        }
    }

    #endregion
}