using System;
using System.ComponentModel;

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
            var desc = "";

            try
            {
                var t = nc.GetType();
                var mi = t.GetMember(nc.ToString());
                var attributes = (DescriptionAttribute[])mi[0].GetCustomAttributes(descriptionType, false);
                desc = attributes[0].Description;
            }
            catch (Exception e)
            {
                throw new InvalidEnumArgumentException("An error occurred while trying to translate a NIBRS Code.", e);
            }

            return desc;
        }
    }

    public class NibrsCodeAttribute : NibrsDescriptionAttribute
    {
        public NibrsCodeAttribute(string description) : base(description) { }

        public static string GetDescription(Enum nc)
        {
            return GetDescription(nc, typeof(NibrsCodeAttribute));
        }
    }

    public class CodeDescriptionAttribute : NibrsDescriptionAttribute
    {
        public CodeDescriptionAttribute(string description) : base(description) { }

        public static string GetDescription(Enum nc)
        {
            return GetDescription(nc, typeof(CodeDescriptionAttribute));
        }
    }

    public class TextDescriptionAttribute : NibrsDescriptionAttribute
    {
        public TextDescriptionAttribute(string description) : base(description) { }

        public static string GetDescription(Enum nc)
        {
            return GetDescription(nc, typeof(TextDescriptionAttribute));
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
