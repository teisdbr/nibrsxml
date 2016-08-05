using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml.Utility
{
    public static class Extensions
    {
        public static void TryAdd<T>(this List<T> list, T item)
        {
            if (item != null)
            {
                list.Add(item);
            }
        }

        public static void TryAdd<T>(this List<T> list, params T[] items)
        {
            foreach (T item in items)
            {
                list.TryAdd(item);
            }
        }

        public static T TryBuild<T>(this String input)
        {
            if (input.Trim() != String.Empty)
                return (T)Activator.CreateInstance(typeof(T), input);
            return default(T);
        }

        public static T TryBuild<T>(this String input, params string[] args)
        {
            if (input.Trim() != String.Empty)
                return (T)Activator.CreateInstance(typeof(T), input, args);
            return default(T);
        }
    }
}
