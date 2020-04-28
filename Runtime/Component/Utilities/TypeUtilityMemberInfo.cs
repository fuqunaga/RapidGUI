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

        public abstract class MemberWrapper
        {
            public abstract string Name { get; }
            public abstract Type MemberType { get; }

            public abstract object GetValue(object value);
            public abstract void SetValue(object obj, object value);

            string label_;

            public string label
            {
                get
                {
                    return label_ ?? Name;
                }

                set
                {
                    label_ = value;
                }
            }

            public MinMaxFloat range { get; set; }
        }

        public class MemberFieldInfo : MemberWrapper
        {
            FieldInfo info;

            public MemberFieldInfo(FieldInfo info)
            {
                this.info = info;
                var rangeAttr = info.GetCustomAttribute<RangeAttribute>();
                if ( rangeAttr != null)
                {
                    range = new MinMaxFloat()
                    {
                        min = rangeAttr.min,
                        max = rangeAttr.max
                    };
                }
            }

            public override string Name => info.Name;

            public override Type MemberType => info.FieldType;

            public override object GetValue(object obj) => info.GetValue(obj);

            public override void SetValue(object obj, object value) => info.SetValue(obj, value);
        }

        public class MemberPropertyInfo : MemberWrapper
        {
            PropertyInfo info;

            public MemberPropertyInfo(PropertyInfo info)
            {
                this.info = info;
            }

            public override string Name => info.Name;

            public override Type MemberType => info.PropertyType;

            public override object GetValue(object obj) => info.GetValue(obj);

            public override void SetValue(object obj, object value) => info.SetValue(obj, value);
        }

        #endregion

        static Dictionary<Type, List<MemberWrapper>> memberInfoTable = new Dictionary<Type, List<MemberWrapper>>();
        public static List<MemberWrapper> GetMemberInfoList(Type type)
        {
            List<MemberWrapper> list;
            if (!memberInfoTable.TryGetValue(type, out list))
            {
                list = new List<MemberWrapper>();
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
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(fi => !typeof(Delegate).IsAssignableFrom(fi.FieldType))
                    .Where(fi => fi.IsPublic ? fi.GetCustomAttribute<NonSerializedAttribute>() == null : fi.GetCustomAttribute<SerializeField>() != null)
                    .ToList();
                fieldInfoTable[type] = fiList;
            }

            return fiList;
        }

        #endregion
    }
}
