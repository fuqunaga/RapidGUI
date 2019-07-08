using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace RapidGUI
{
    /// <summary>
    /// TypeUtility.GetmemberInfoList() implements
    /// </summary>
    public static partial class TypeUtility
    {
        #region Type Define

        public interface IMemberWrapper
        {
            string Name { get; }
            Type MemberType { get; }

            object GetValue(object value);
            void SetValue(object obj, object value);
        }

        public class MemberFieldInfo : IMemberWrapper
        {
            FieldInfo info;
            public MemberFieldInfo(FieldInfo info)
            {
                this.info = info;
            }

            public string Name => info.Name;

            public Type MemberType => info.FieldType;

            public object GetValue(object obj) => info.GetValue(obj);

            public void SetValue(object obj, object value) => info.SetValue(obj, value);
        }

        public class MemberPropertyInfo : IMemberWrapper
        {
            PropertyInfo info;
            public MemberPropertyInfo(PropertyInfo info)
            {
                this.info = info;
            }

            public string Name => info.Name;

            public Type MemberType => info.PropertyType;

            public object GetValue(object obj) => info.GetValue(obj);

            public void SetValue(object obj, object value) => info.SetValue(obj, value);
        }

        #endregion

        static Dictionary<Type, List<IMemberWrapper>> memberInfoTable = new Dictionary<Type, List<IMemberWrapper>>();
        public static List<IMemberWrapper> GetMemberInfoList(Type type)
        {
            List<IMemberWrapper> list;
            if (!memberInfoTable.TryGetValue(type, out list))
            {
                list = new List<IMemberWrapper>();
                list.AddRange(GetPropertyInfoList(type).Select(info => new MemberPropertyInfo(info)));
                list.AddRange(GetFieldInfoList(type).Select(info => new MemberFieldInfo(info)));

                memberInfoTable[type] = list;
            }

            return list;
        }


        #region Property Info List

        static Dictionary<Type, List<PropertyInfo>> propertyInfoTable;
        static List<PropertyInfo> GetPropertyInfoList(Type type)
        {
            if (propertyInfoTable == null)
            {
                var list = new[]
                {
                    new {type = typeof(Vector2Int), names = new[]{"x", "y"} },
                    new {type = typeof(Vector3Int), names = new[]{"x", "y", "z"} },
                    new {type = typeof(Rect), names = new[]{"x", "y", "width", "height"} },
                    new {type = typeof(RectInt), names = new[]{"x", "y", "width", "height"} },
                    new {type = typeof(RectOffset), names = new []{"left", "right", "top", "bottom"} },
                    new {type = typeof(Bounds), names = new[]{"center", "extents"} },
                    new {type = typeof(BoundsInt), names = new[]{ "position", "size"} },
                };

                propertyInfoTable = list.ToDictionary(tn => tn.type, tn => tn.names.Select(name => tn.type.GetProperty(name)).ToList());
            };

            propertyInfoTable.TryGetValue(type, out var piList);

            return piList ?? new List<PropertyInfo>();
        }

        #endregion


        #region Field Info List

        static Dictionary<Type, List<FieldInfo>> fieldInfoTable = new Dictionary<Type, List<FieldInfo>>();

        static List<FieldInfo> GetFieldInfoList(Type type)
        {
            if (!fieldInfoTable.TryGetValue(type, out var fiList))
            {
                fiList = type
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(fi => fi.GetCustomAttribute<NonSerializedAttribute>() == null)
                    .ToList();

                fiList.AddRange(type
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(fi => fi.GetCustomAttribute<SerializeField>() != null)
                    );

                fieldInfoTable[type] = fiList;
            }

            return fiList;
        }

        #endregion
    }
}
