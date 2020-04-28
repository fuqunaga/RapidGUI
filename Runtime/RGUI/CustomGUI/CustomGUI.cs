using System.Linq;

namespace RapidGUI
{
    public static class CustomGUI
    {
        public static bool Label<T>(string memberName, string label)
        {
            var infos = TypeUtility.GetMemberInfoList(typeof(T));

            bool changed = false; ;
            foreach (var info in infos)
            {
                if (info.Name == memberName)
                {
                    info.label = label;
                    changed = true;
                }
            }

            return changed;
        }

        public static bool IgnoreMember<T>(string memberName)
        {
            var infos = TypeUtility.GetMemberInfoList(typeof(T));
            var removed = infos.RemoveAll(info => info.Name == memberName) > 0;
            return removed;
        }

        public static bool AddRange<T>(string memberName, float max) => AddRange<T>(memberName, 0f, max);

        public static bool AddRange<T>(string memberName, float min, float max) => AddRange<T>(memberName, new MinMaxFloat() { min = min, max = max });

        public static bool AddRange<T>(string memberName, MinMaxFloat range)
        {
            var infos = TypeUtility.GetMemberInfoList(typeof(T));
            var info = infos.FirstOrDefault(fi => fi.Name == memberName);
            var hasMember = (info != null);
            if (hasMember)
            {
                info.range = range;
            }

            return hasMember;
        }
    }
}