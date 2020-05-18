using System;
using MongoDB.Bson;
using NibrsXml.NibrsReport;
using NibrsXml.NibrsReport.MessageMetadatas;
using NibrsXml.NibrsReport.Misc;

namespace NibrsXml.Builder
{
    public class MessageMetaDataBuilder
    {
        public static MessageMetadata Build(ObjectId submissionId, string agencyOri)
        {
            var md = new MessageMetadata();
            try
            {
                var month = DateTime.Now.Month.ToString().PadLeft(2, '0');
                var day = DateTime.Now.Day.ToString().PadLeft(2, '0');
                var year = DateTime.Now.Year;
                var hour = DateTime.Now.Hour.ToString().PadLeft(2, '0');
                var date = string.Format("{0}-{1}-{2}", year, month, day);
                var time = string.Format("{0}:00:00", hour == string.Empty ? "00" : hour.PadLeft(2, '0'));

                var dateTime = date + "T" + time;
                if (string.IsNullOrEmpty(dateTime))
                    md.MessageDateTime = "Error";
                md.MessageDateTime = dateTime;
                md.Version = (float) 4.2;
                md.MessageIdentification = new MessageIdentification {IdentificationId = submissionId.ToString()};

                md.MessageSubmittingOrganization = new MessageSubmittingOrganization
                {
                    OrganizationAugmentation =
                        new OrganizationAugmentation(new OrganizationORIIdentification(agencyOri))
                };
            }
            catch (Exception e)
            {
                throw new Exception("Errror occured while generating the MessageMetadata", e);
            }

            return md;
        }
    }
}