using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml
{
    public static class FileLogger
    {
        public static void WriteException(Exception ex)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"C:\WinLibrs\IEPDReprocessor.log"), true))
            {
                outputFile.WriteLine("*** EXCEPTION - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffffff") + " ***");
                outputFile.WriteLine(ex.Message);
                outputFile.WriteLine(ex.Source);
                outputFile.WriteLine(ex.StackTrace);
                outputFile.WriteLine(ex.InnerException?.Message);
                outputFile.WriteLine(ex.InnerException?.StackTrace);
                outputFile.WriteLine("*** END EXCEPTION ***");
            }
        }

        public static void WriteInfo(string message)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"C:\WinLibrs\IEPDReprocessor.log"), true))
            {
                outputFile.WriteLine("INFO - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffffff") + ": " + message);
            }
        }
    }
}
