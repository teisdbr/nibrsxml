using System;

namespace NibrsXml.Utility
{
    public static class Extensions
    {
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

        public static string NibrsTextDescription(this Enum e)
        {
            return TextDescriptionAttribute.GetDescription(e);
        }

        public static string UcrReportHeader(this Enum e)
        {
            return UcrElementName.GetDescription(e);
        }

        #endregion
    }
}