using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NibrsModels.Constants;
using NibrsModels.Utility;
using NibrsXml.Utility;
using NibrsXml.Constants;
using TeUtil.Extensions;


namespace NibrsXml.Ucr.DataCollections
{
    public class Leoka
    {
        /// <summary>
        ///     Initializes all counters
        /// </summary>
        public Leoka()
        {
            //Officers Killed
            OfficersKilled = new LeokaOfficersKilledCounts();

            //Assault Counts
            ActivityCounts = new Dictionary<string, LeokaCounts>
            {
                {"1", new LeokaCounts()},
                {"2", new LeokaCounts()},
                {"3", new LeokaCounts()},
                {"4", new LeokaCounts()},
                {"5", new LeokaCounts()},
                {"6", new LeokaCounts()},
                {"7", new LeokaCounts()},
                {"8", new LeokaCounts()},
                {"9", new LeokaCounts()},
                {"10", new LeokaCounts()},
                {"11", new LeokaCounts()}
            };

            //Assault Times
            AssaultTimesCountsDictionary = new Dictionary<string, int>();
        }

        public static Dictionary<string, string> ActivityTranslatorDictionary =new Dictionary<string, string>
        {
            {LEOKAActivityCategoryCode.RESPONDING_TO_DISTURBANCE_CALL.NibrsCode(), "1"},
            {LEOKAActivityCategoryCode.BURGLARIES_IN_PROGRESS_OR_PURSUING_BURGLARY_SUSPECTS.NibrsCode(), "2"},
            {LEOKAActivityCategoryCode.ROBBERIES_IN_PROGRESS_OR_PURSUING_ROBBERY_SUSPECTS.NibrsCode(), "3"},
            {LEOKAActivityCategoryCode.ATTEMPTING_OTHER_ARRESTS.NibrsCode(), "4"},
            {LEOKAActivityCategoryCode.CIVIL_DISORDER.NibrsCode(), "5"},
            {LEOKAActivityCategoryCode.HANDLING_OR_TRANSPORTING_CUSTODY_OF_PRISONERS.NibrsCode(), "6"},
            {LEOKAActivityCategoryCode.INVESTIGATING_SUSPICIOUS_PERSONS_OR_CIRCUMSTANCES.NibrsCode(), "7"},
            {LEOKAActivityCategoryCode.AMBUSH.NibrsCode(), "8"},
            {LEOKAActivityCategoryCode.HANDLING_PERSONS_WITH_MENTAL_ILLNESS.NibrsCode(), "9"},
            {LEOKAActivityCategoryCode.TRAFFIC_PURSUITS_AND_STOPS.NibrsCode(), "10"},
            {LEOKAActivityCategoryCode.OTHER.NibrsCode(), "11"},
        };

        /// <summary>
        ///     Increment either 09A or 09B
        /// </summary>
        /// <param name="key"></param>
        /// <param name="byValue"></param>
        public void IncrementKilled(string key, int byValue = 1)
        {
            OfficersKilled.IncrementCount(key, byValue);
        }

        #region Serialization Properties

        public XDocument Serialize()
        {
            return new XDocument(
                new XProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"leoka.xsl\""),
                new XElement("LeokaSummary",
                    new XElement("Killed", new XElement("Feloneously", OfficersKilled.Feloniously), new XElement("ByAccident", OfficersKilled.ByAccident)),
                    FetchClassifications(),
                    new XElement("AssaultsTime", AssaultTimesCountsDictionary.OrderBy(a => a.Key).Select(a => new XElement(a.Key, a.Value)).ToArray()))
            );
        }

        private XElement FetchClassifications()
        {
            //Classification Element
            var classificationXElement = new List<XElement>();

            foreach (var classificationCount in ActivityCounts)
                classificationXElement.Add(new XElement("Classification", new XAttribute("name", classificationCount.Key),
                    classificationCount.Value.CountsDictionary.OrderBy(c => c.Key).Select(counts => new XElement(counts.Key, counts.Value)).ToArray()));

            return new XElement("Assaults", classificationXElement.ToArray());
        }

        #endregion

        #region Base Overriden Incrementers

        #endregion

        #region Report Specific Data Structure

        public LeokaOfficersKilledCounts OfficersKilled { get; set; }

        public Dictionary<string, LeokaCounts> ActivityCounts { get; set; }

        #endregion

        #region Specific Incrementers

        public void ScoreActivityCounts(string classificationKey, string weaponKey, string assignmentKey = null, int byValue = 1)
        {
            //Get activity counter.
            var activity = ActivityCounts.TryAdd(classificationKey);
            var grandTotalActivity = ActivityCounts.TryAdd("12");

            //Increment Weapons
            activity.IncrementClassificationCounters(weaponKey, byValue);
            //--Only increment grand total for classifications 1 - 11
            if (ActivityTranslatorDictionary.ContainsKey(classificationKey.PadLeft(2,'0')))
            {
                grandTotalActivity.IncrementClassificationCounters(weaponKey, byValue);
                grandTotalActivity.IncrementClassificationCounters("A", byValue);
            }

            //Increment Assignments
            if (assignmentKey != null)
            {
                activity.IncrementClassificationCounters(assignmentKey, byValue);
                grandTotalActivity.IncrementClassificationCounters(assignmentKey, byValue);
            }

            //Increment Total Assaults by Weapon
            activity.IncrementClassificationCounters("A", byValue);
        }

        public void ScoreClearanceCounts(string activity)
        {
            if (activity == null)
                return;

            var activityKey = ActivityTranslatorDictionary[activity];

            //Create the dictionary entry if it doesn't exist, then increment the clearance counter for the appropriate row's column M
            var leokaCounts = ActivityCounts.TryAdd(activityKey);
            leokaCounts.IncrementClassificationCounters("M");

            //Create the dictionary entry if it doesn't exist, then increment the clearance counter for grand totals
            var grandTotalActivity = ActivityCounts.TryAdd("12");
            grandTotalActivity.IncrementClassificationCounters("M");
        }

        #endregion

        #region Officer Assault Timing

        public void ScoreAssaultTime(string assaultDateTime)
        {
            //Make sure this is a valid date and time object that was passed. Do nothing if it is not valid.
            DateTime convertedDateTime;
            if (!DateTime.TryParse(assaultDateTime, out convertedDateTime))
                return;

            //Gets the time from the date object.
            var time = convertedDateTime.TimeOfDay;

            //Determine key for scoring
            var assaultTimeKey = AssaultTimeRanges.Where(range => range.Item1 <= time && time <= range.Item2).FirstOrDefault();

            //Score it
            AssaultTimesCountsDictionary.TryIncrement(assaultTimeKey.Item3);
        }

        /// <summary>
        ///     The first item contains the lower time from the range.
        ///     The second item contains the highest time from the range.
        ///     The third item contains the corresponding key for the range.
        /// </summary>
        public List<Tuple<TimeSpan, TimeSpan, string>> AssaultTimeRanges
        {
            get
            {
                return new List<Tuple<TimeSpan, TimeSpan, string>>
                {
                    Tuple.Create(new TimeSpan(0, 1, 0), new TimeSpan(2, 0, 0), "H00-01"),
                    Tuple.Create(new TimeSpan(2, 1, 0), new TimeSpan(4, 0, 0), "H02-03"),
                    Tuple.Create(new TimeSpan(4, 1, 0), new TimeSpan(6, 0, 0), "H04-05"),
                    Tuple.Create(new TimeSpan(6, 1, 0), new TimeSpan(8, 0, 0), "H06-07"),
                    Tuple.Create(new TimeSpan(8, 1, 0), new TimeSpan(10, 0, 0), "H08-09"),
                    Tuple.Create(new TimeSpan(10, 1, 0), new TimeSpan(12, 0, 0), "H10-11"),
                    Tuple.Create(new TimeSpan(12, 1, 0), new TimeSpan(14, 0, 0), "H12-13"),
                    Tuple.Create(new TimeSpan(14, 1, 0), new TimeSpan(16, 0, 0), "H14-15"),
                    Tuple.Create(new TimeSpan(16, 1, 0), new TimeSpan(18, 0, 0), "H16-17"),
                    Tuple.Create(new TimeSpan(18, 1, 0), new TimeSpan(20, 0, 0), "H18-19"),
                    Tuple.Create(new TimeSpan(20, 1, 0), new TimeSpan(22, 0, 0), "H20-21"),
                    Tuple.Create(new TimeSpan(22, 1, 0), new TimeSpan(23, 59, 59), "H22-23"),
                    Tuple.Create(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), "H22-23")
                };   
            }
        }

        public Dictionary<string, int> AssaultTimesCountsDictionary { get; set; }

        #endregion
    }
}