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


        public string GetErroredLocation()
        {

            var failedToUploadLocation = Convert.ToString(AppSettingsReader.GetValue("FailedFilesFolderLocation", typeof(string)));
            return NibrsXmlFolderPath + "\\" + failedToUploadLocation;
        }

        public string GetTempFolderLocation()
        {
            var failedToUploadLocation = Convert.ToString(AppSettingsReader.GetValue("FailedFilesFolderLocation", typeof(string)));
            return NibrsXmlFolderPath + "\\" + failedToUploadLocation + "\\" + "Temp";
        }


        public DirectoryInfo GetTempFolderDirectoryInfo()
        {
            var tempLocation = GetTempFolderLocation();
            DirectoryInfo directory = new DirectoryInfo(tempLocation);
            if (!directory.Exists)
                directory.Create();
            return directory;
        }


        public DirectoryInfo GetErroredDirectory()        
        {

            var location = Convert.ToString(AppSettingsReader.GetValue("ErroredNibrsXmlFilesFolderLocation", typeof(string)));
            DirectoryInfo directory = new DirectoryInfo(NibrsXmlFolderPath + "\\" + location);
            if (!directory.Exists)
                directory.Create();
            return directory;
        }


        public string GetFailedToSaveLocation()
        {
            var failedToSaveLocation = Convert.ToString(AppSettingsReader.GetValue("FailedToSaveNibrsXmlFilesFolderLocation", typeof(string)));
            return NibrsXmlFolderPath + "\\" + failedToSaveLocation;
        }


        public string GetDataFolderLocation()
        {
            var dataFolderName = Convert.ToString(AppSettingsReader.GetValue("IncomingFilesFolderLocation", typeof(string)));
            return NibrsXmlFolderPath + "\\" + dataFolderName;
        }

        public DirectoryInfo GetDataDirectoryInfo()
        {
            var failedToSaveLocation = GetDataFolderLocation();
            DirectoryInfo directory = new DirectoryInfo(failedToSaveLocation);
            if (!directory.Exists)
                directory.Create();
            return directory;
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
