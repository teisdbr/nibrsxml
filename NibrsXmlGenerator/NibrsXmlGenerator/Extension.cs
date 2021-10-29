using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml
{
   public static class Extension
    {

        public static string ToFalttenString( this AggregateException aex)
        {
            return String.Join("Exception: ", aex.Flatten().InnerExceptions.Select(ine => $"Exception Type: { ine.GetType().ToString()} , Message : {ine.Message} , Inner Exception: {ine.InnerException}, ").AsEnumerable());
        }

        public static string ToReableString(this Exception ex)
        {
            return $" Exception Type: {ex.GetType().ToString()}  Message: { ex.Message} , Inner Exception:  { ex.InnerException}";
        }
    }
}

