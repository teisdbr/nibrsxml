using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibrsXml
{
  public  class AgencyXmlDirectoryInfo
    {

        public readonly AppSettingsReader AppSettingsReader = new AppSettingsReader();

        public string NibrsXmlFolderPath { get; private set; }

        public AgencyXmlDirectoryInfo(string ori)
        {
            var appSettingsReader = new AppSettingsReader();
            var nibrsXmlFilesFolderLocation = Convert.ToString(appSettingsReader.GetValue("IncomingNibrsXmlFilesFolderLocation", typeof(string)));
            var strIncomingFolderLocation = Convert.ToString(appSettingsReader.GetValue("ReadDirectoryPath", typeof(string)));
         

            NibrsXmlFolderPath = strIncomingFolderLocation + ori + "\\" + nibrsXmlFilesFolderLocation;
        }


        public string GetFailedLocation()
        {

            var failedToUploadLocation = Convert.ToString(AppSettingsReader.GetValue("FailedFilesFolderLocation", typeof(string)));
            return NibrsXmlFolderPath + "\\" + failedToUploadLocation;
        }


        public DirectoryInfo GetFailedDirectory()        
        {

            var failedToUploadLocation = Convert.ToString(AppSettingsReader.GetValue("FailedFilesFolderLocation", typeof(string)));
            DirectoryInfo directory = new DirectoryInfo(NibrsXmlFolderPath + "\\" + failedToUploadLocation);
            if (!directory.Exists)
                directory.Create();
            return directory;
        }


        public string GetFailedToSaveLocation()
        {
            var failedToSaveLocation = Convert.ToString(AppSettingsReader.GetValue("FailedToSaveNibrsXmlFilesFolderLocation", typeof(string)));
            return NibrsXmlFolderPath + "\\" + failedToSaveLocation;
        }



        public DirectoryInfo GetFailedToSaveDirectory()
        {

            var failedToSaveLocation = Convert.ToString(AppSettingsReader.GetValue("FailedToSaveNibrsXmlFilesFolderLocation", typeof(string)));
            DirectoryInfo directory = new DirectoryInfo(NibrsXmlFolderPath + "\\" + failedToSaveLocation);
            if (!directory.Exists)
                directory.Create();
            return directory;
        }


        public string GetArchiveLocation()
        {
            var archiveXmlLocation = Convert.ToString(AppSettingsReader.GetValue("ArchiveNibrsXmlFilesFolderLocation", typeof(string)));
            return NibrsXmlFolderPath + "\\" + archiveXmlLocation;
        }
    }
}
