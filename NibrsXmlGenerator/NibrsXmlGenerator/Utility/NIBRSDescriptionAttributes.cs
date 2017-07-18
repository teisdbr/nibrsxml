using System;
using System.ComponentModel;
using System.Reflection;

/**
 * Creating custom description attributes for NIBRS constants
 */

namespace NibrsXml.Utility
{
    public abstract class NibrsDescriptionAttribute : DescriptionAttribute
    {
        public NibrsDescriptionAttribute(string description) : base(description) { }

        protected static string GetDescription(Enum nc, Type descriptionType)
        {
            string desc = "";

            try
            {
                Type t = nc.GetType();
                MemberInfo[] mi = t.GetMember(nc.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])mi[0].GetCustomAttributes(descriptionType, false);
                desc = attributes[0].Description;
            }
            catch (Exception e)
            {
                throw new InvalidEnumArgumentException("An error occurred while trying to translate a NIBRS Code.", e);
            }

            return desc;
        }
    }

    internal class NibrsCodeAttribute : NibrsDescriptionAttribute
    {
        public NibrsCodeAttribute(string description) : base(description) { }

        public static string GetDescription(Enum nc)
        {
            return GetDescription(nc, typeof(NibrsCodeAttribute));
        }
    }

    internal class CodeDescriptionAttribute : NibrsDescriptionAttribute
    {
        public CodeDescriptionAttribute(string description) : base(description) { }

        public static string GetDescription(Enum nc)
        {
            return GetDescription(nc, typeof(CodeDescriptionAttribute));
        }
    }

    internal class UcrElementName : NibrsDescriptionAttribute
    {
        public UcrElementName(string description) : base(description) { }

        public static string GetDescription(Enum nc)
        {
            return GetDescription(nc, typeof(UcrElementName));
        }
    }
}
