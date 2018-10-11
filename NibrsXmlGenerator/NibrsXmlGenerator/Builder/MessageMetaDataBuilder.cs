using System;
using System.Collections.Generic;
using NibrsXml.NibrsReport.Misc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMD_NameSpace  =  NibrsXml.NibrsReport.MessageMetadatas ;


namespace NibrsXml.Builder
{
   public   class MessageMetaDataBuilder
    {




       public static NibrsXml.NibrsReport.MessageMetadata ExtractNibrsMessageDateTime()
        {
           var MD = new NibrsXml.NibrsReport.MessageMetadata(); 
            string date, time, DateTime;
            try
            {
                var month = System.DateTime.Now.Month.ToString().PadLeft(2, '0');   //admin.IncidentDate.Substring(0, 2);
                var day = System.DateTime.Now.Day.ToString().PadLeft(2, '0');
                var year = System.DateTime.Now.Year;
                var hour = System.DateTime.Now.Hour.ToString().PadLeft(2, '0');
                date = string.Format("{0}-{1}-{2}", year, month, day);
                time = string.Format("{0}:00:00", hour == string.Empty ? "00" : hour.PadLeft(2, '0'));

               DateTime = date + "T" + time;
               if (DateTime == null || DateTime == string.Empty)
                   MD.MessageDateTime = "Error";
               MD.MessageDateTime = DateTime;
               MD.Version = (float) 4.2;
               MD.MessageIdentification = MessageIdentificationBuilder();

               MD.MessageSubmittingOrganization = new MMD_NameSpace.MessageSubmittingOrganization { OrganizationAugmentation = new OrganizationAugmentation(new OrganizationORIIdentification("LA0140000")) };
               
            }
            catch (Exception e)
            {
                throw new Exception("Errror occured while generating the MessageMetadata", e);
            }

            return MD;
       
       }


       //public static MMD_NameSpace.OrganizationORIIdentification OragnizationORIIdentificationBuilder()
      
       //{
       //    var OrganizationORIIdentification = new MMD_NameSpace.OrganizationORIIdentification();
           

       //   OrganizationORIIdentification.IdentificationID= "LA000000";
          
       //   return OrganizationORIIdentification;

       //}


       public static MMD_NameSpace.MessageIdentification MessageIdentificationBuilder()
       {
           var MessageID = new MMD_NameSpace.MessageIdentification();

           MessageID.IdentificationID = "123456";

           if (MessageID.IdentificationID != null)
               return MessageID;

           else
           {
               MessageID.IdentificationID = "Error"; 
               
               return MessageID;}

       }





    }
}
