using System;
using System.Collections.Generic;
using System.Linq;
using NibrsXml.NibrsReport;
using NibrsXml.Ucr.DataMining;

namespace NibrsXml.Utility
{
    public static class Extensions
    {
        #region Dictionary Extensions

        public static Dictionary<string, int> TryAdd(this Dictionary<string, Dictionary<string, int>> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new Dictionary<string, int>());
            return dictionary[key];
        }

        public static void TryIncrement(this Dictionary<string, int> dictionary, string key, bool force = true)
        {
            if (key == null || !force && !dictionary.ContainsKey(key)) return;
            if (dictionary.ContainsKey(key))
                dictionary[key] += 1;
            else if (force)
                dictionary.Add(key, 1);
        }

        #endregion

        #region String Extensions

        public static string TrimNullIfEmpty(this string input)
        {
            return input.Trim() == string.Empty ? null : input.Trim();
        }

        // String Extensions
        public static T TryBuild<T>(this string input)
        {
            if (input.Trim() != string.Empty)
                return (T) Activator.CreateInstance(typeof(T), input);
            return default(T);
        }

        public static T TryBuild<T>(this string input, params string[] args)
        {
            if (input.Trim() != string.Empty)
                return (T) Activator.CreateInstance(typeof(T), input, args);
            return default(T);
        }

        #endregion

        #region Enum Extensions

        public static string NibrsCode(this Enum e)
        {
            return NibrsCodeAttribute.GetDescription(e);
        }

        public static string NibrsCodeDescription(this Enum e)
        {
            return CodeDescriptionAttribute.GetDescription(e);
        }

        public static string UcrReportHeader(this Enum e)
        {
            return UcrElementName.GetDescription(e);
        }

        #endregion
    }
}