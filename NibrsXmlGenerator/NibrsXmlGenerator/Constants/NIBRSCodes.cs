using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NibrsXml.Utility;

/**
 * See the NIBRSCode and CodeDescription classes to see how to extract descriptions from the following enums
 */

namespace NibrsXml.Constants
{
    /// <summary>
    /// A data type for Sex
    /// </summary>
    [Description("A data type for Sex.")]
    enum SexCode
    {
        /// <summary>
        /// Female
        /// </summary>
        [NibrsCode("F")]
        [CodeDescription("Female")]
        FEMALE,

        /// <summary>
        /// Male
        /// </summary>
        [NibrsCode("M")]
        [CodeDescription("Male")]
        MALE,

        /// <summary>
        /// Unknown - For Unidentified Only
        /// </summary>
        [NibrsCode("U")]
        [CodeDescription("Unknown - For Unidentified Only")]
        UNKNOWN
    }

    /// <summary>
    /// A data type for a code that identifies the type of bias that motivated the offense, if any. Includes all NIBRS codes, plus additional codes
    /// </summary>
    [Description("A data type for a code that identifies the type of bias that motivated the offense, if any. Includes all NIBRS codes, plus additional codes.")]
    enum BiasMotivationCode
    {
        /// <summary>
        /// Anti-American Indian or Alaskan Native_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTIAMERICAN INDIAN_ ALASKAN NATIVE")]
        [CodeDescription("Anti-American Indian or Alaskan Native_race ethnicity bias")]
        ANTIAMERICAN_INDIAN_ALASKAN_NATIVE,

        /// <summary>
        /// Anti-Arab_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTIARAB")]
        [CodeDescription("Anti-Arab_race ethnicity bias")]
        ANTIARAB,

        /// <summary>
        /// Anti-Asian_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTIASIAN")]
        [CodeDescription("Anti-Asian_race ethnicity bias")]
        ANTIASIAN,

        /// <summary>
        /// Anti-Atheist or Agnostic_religious bias
        /// </summary>
        [NibrsCode("ANTIATHEIST_AGNOSTIC")]
        [CodeDescription("Anti-Atheist or Agnostic_religious bias")]
        ANTIATHEIST_AGNOSTIC,

        /// <summary>
        /// Anti-Bisexual
        /// </summary>
        [NibrsCode("ANTIBISEXUAL")]
        [CodeDescription("Anti-Bisexual")]
        ANTIBISEXUAL,

        /// <summary>
        /// Anti-Black or African American_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTIBLACK_AFRICAN AMERICAN")]
        [CodeDescription("Anti-Black or African American_race ethnicity bias")]
        ANTIBLACK_AFRICAN_AMERICAN,

        /// <summary>
        /// Anti-Buddhist_religious bias
        /// </summary>
        [NibrsCode("ANTIBUDDHIST")]
        [CodeDescription("Anti-Buddhist_religious bias")]
        ANTIBUDDHIST,

        /// <summary>
        /// Anti-Catholic religion_religious bias
        /// </summary>
        [NibrsCode("ANTICATHOLIC")]
        [CodeDescription("Anti-Catholic religion_religious bias")]
        ANTICATHOLIC,

        /// <summary>
        /// AntiDisbled_disability bias
        /// </summary>
        [NibrsCode("ANTIDISABLED")]
        [CodeDescription("AntiDisbled_disability bias")]
        ANTIDISABLED,

        /// <summary>
        /// Anti-Eastern Orthodox (Russian, Greek, Other)_religious bias
        /// </summary>
        [NibrsCode("ANTIEASTERNORTHODOX")]
        [CodeDescription("Anti-Eastern Orthodox (Russian, Greek, Other)_religious bias")]
        ANTIEASTERNORTHODOX,

        /// <summary>
        /// Anti-Female_gender bias
        /// </summary>
        [NibrsCode("ANTIFEMALE")]
        [CodeDescription("Anti-Female_gender bias")]
        ANTIFEMALE,

        /// <summary>
        /// Anti-Female Homosexual (Lesbian) _sexual orientation bias
        /// </summary>
        [NibrsCode("ANTIFEMALE HOMOSEXUAL")]
        [CodeDescription("Anti-Female Homosexual (Lesbian) _sexual orientation bias")]
        ANTIFEMALE_HOMOSEXUAL,

        /// <summary>
        /// Anti-Gender Non-Conforming
        /// </summary>
        [NibrsCode("ANTIGENDER_NONCONFORMING")]
        [CodeDescription("Anti-Gender Non-Conforming")]
        ANTIGENDER_NONCONFORMING,

        /// <summary>
        /// Anti-Heterosexual _sexual orientation bias
        /// </summary>
        [NibrsCode("ANTIHETEROSEXUAL")]
        [CodeDescription("Anti-Heterosexual _sexual orientation bias")]
        ANTIHETEROSEXUAL,

        /// <summary>
        /// Anti-Hindu_religious bias
        /// </summary>
        [NibrsCode("ANTIHINDU")]
        [CodeDescription("Anti-Hindu_religious bias")]
        ANTIHINDU,

        /// <summary>
        /// Anti-Hispanic or Latino_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTIHISPANIC_LATINO")]
        [CodeDescription("Anti-Hispanic or Latino_race ethnicity bias")]
        ANTIHISPANIC_LATINO,

        /// <summary>
        /// Anti-Homosexual, e.g, Lesbian, Gay, Bisexual, and transgender (mixed group), _sexual orientation bias
        /// </summary>
        [NibrsCode("ANTIHOMOSEXUAL")]
        [CodeDescription("Anti-Homosexual, e.g., Lesbian, Gay, Bisexual, and transgender (mixed group), _sexual orientation bias")]
        ANTIHOMOSEXUAL,

        /// <summary>
        /// Anti-Islamic, includes muslim_religious bias
        /// </summary>
        [NibrsCode("ANTIISLAMIC")]
        [CodeDescription("Anti-Islamic, includes muslim_religious bias")]
        ANTIISLAMIC,

        /// <summary>
        /// Anti-Jehovah's Witness_religious bias
        /// </summary>
        [NibrsCode("ANTIJEHOVAHWITNESS")]
        [CodeDescription("Anti-Jehovah's Witness_religious bias")]
        ANTIJEHOVAHWITNESS,

        /// <summary>
        /// Anti-Jewish_religious bias
        /// </summary>
        [NibrsCode("ANTIJEWISH")]
        [CodeDescription("Anti-Jewish_religious bias")]
        ANTIJEWISH,

        /// <summary>
        /// Anti-Male_gender bias
        /// </summary>
        [NibrsCode("ANTIMALE")]
        [CodeDescription("Anti-Male_gender bias")]
        ANTIMALE,

        /// <summary>
        /// Anti-Male Homosexual (Gay) _sexual orientation bias
        /// </summary>
        [NibrsCode("ANTIMALE HOMOSEXUAL")]
        [CodeDescription("Anti-Male Homosexual (Gay) _sexual orientation bias")]
        ANTIMALE_HOMOSEXUAL,

        /// <summary>
        /// Anti-Mental Disability_disability bias
        /// </summary>
        [NibrsCode("ANTIMENTAL DISABILITY")]
        [CodeDescription("Anti-Mental Disability_disability bias")]
        ANTIMENTAL_DISABILITY,

        /// <summary>
        /// Anti-Mormon_religious bias
        /// </summary>
        [NibrsCode("ANTIMORMON")]
        [CodeDescription("Anti-Mormon_religious bias")]
        ANTIMORMON,

        /// <summary>
        /// Anti-Multi-Racial Group, e.g., Arab and Asian and American Indian and White and etc_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTIMULTIRACIAL GROUP")]
        [CodeDescription("Anti-Multi-Racial Group, e.g., Arab and Asian and American Indian and White and etc._race ethnicity bias")]
        ANTIMULTIRACIAL_GROUP,

        /// <summary>
        /// Anti-Multi-Religious Group, e.g., Catholic and Mormon and Islamic and etc_religious bias
        /// </summary>
        [NibrsCode("ANTIMULTIRELIGIOUS GROUP")]
        [CodeDescription("Anti-Multi-Religious Group, e.g., Catholic and Mormon and Islamic and etc._religious bias")]
        ANTIMULTIRELIGIOUS_GROUP,

        /// <summary>
        /// Anti-Native Hawaiian or Other Pacific Islander_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTINATIVEHAWAIIAN_OTHERPACIFICISLANDER")]
        [CodeDescription("Anti-Native Hawaiian or Other Pacific Islander_race ethnicity bias")]
        ANTINATIVEHAWAIIAN_OTHERPACIFICISLANDER,

        /// <summary>
        /// Anti-Other Christian_religious bias
        /// </summary>
        [NibrsCode("ANTIOTHER CHRISTIAN")]
        [CodeDescription("Anti-Other Christian_religious bias")]
        ANTIOTHER_CHRISTIAN,

        /// <summary>
        /// Anti-Other Race, Ethnicity, Ancestry, or National Origin_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTIOTHER ETHNICITY_NATIONAL ORIGIN")]
        [CodeDescription("Anti-Other Race, Ethnicity, Ancestry, or National Origin_race ethnicity bias")]
        ANTIOTHER_ETHNICITY_NATIONAL_ORIGIN,

        /// <summary>
        /// Anti-Other Religion_religious bias
        /// </summary>
        [NibrsCode("ANTIOTHER RELIGION")]
        [CodeDescription("Anti-Other Religion_religious bias")]
        ANTIOTHER_RELIGION,

        /// <summary>
        /// Anti-Physical Disability_disability bias
        /// </summary>
        [NibrsCode("ANTIPHYSICAL DISABILITY")]
        [CodeDescription("Anti-Physical Disability_disability bias")]
        ANTIPHYSICAL_DISABILITY,

        /// <summary>
        /// Anti-Protestant_religious bias
        /// </summary>
        [NibrsCode("ANTIPROTESTANT")]
        [CodeDescription("Anti-Protestant_religious bias")]
        ANTIPROTESTANT,

        /// <summary>
        /// Anti-Sikh_religious bias
        /// </summary>
        [NibrsCode("ANTISIKH")]
        [CodeDescription("Anti-Sikh_religious bias")]
        ANTISIKH,

        /// <summary>
        /// Anti-Transgender_gender identity
        /// </summary>
        [NibrsCode("ANTITRANSGENDER")]
        [CodeDescription("Anti-Transgender_gender identity")]
        ANTITRANSGENDER,

        /// <summary>
        /// Anti-White_race ethnicity bias
        /// </summary>
        [NibrsCode("ANTIWHITE")]
        [CodeDescription("Anti-White_race ethnicity bias")]
        ANTIWHITE,

        /// <summary>
        /// Gender Bias_gender bias
        /// </summary>
        [NibrsCode("GENDER BIAS")]
        [CodeDescription("Gender Bias_gender bias")]
        GENDER_BIAS,

        /// <summary>
        /// None (no bias)
        /// </summary>
        [NibrsCode("NONE")]
        [CodeDescription("None (no bias)")]
        NONE,

        /// <summary>
        /// Political Affiliation Bias
        /// </summary>
        [NibrsCode("POLITICAL AFFILIATION BIAS")]
        [CodeDescription("Political Affiliation Bias")]
        POLITICAL_AFFILIATION_BIAS,

        /// <summary>
        /// Unknown (offender's motivation not known)
        /// </summary>
        [NibrsCode("UNKNOWN")]
        [CodeDescription("Unknown (offender's motivation not known)")]
        UNKNOWN
    }

    /// <summary>
    /// A data type for a code that identifies gang involvement of offenders in an offense
    /// </summary>
    [Description("A data type for a code that identifies gang involvement of offenders in an offense.")]
    enum GangInvolvementCategoryCode
    {
        /// <summary>
        /// Other Gang
        /// </summary>
        [NibrsCode("G")]
        [CodeDescription("Other Gang")]
        OTHER,

        /// <summary>
        /// Juvenile Gang
        /// </summary>
        [NibrsCode("J")]
        [CodeDescription("Juvenile Gang")]
        JUVENILE_GANG,

        /// <summary>
        /// None/Unknown
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("None/Unknown")]
        NONE_UNKNOWN
    }

    /// <summary>
    /// A data type for a code that identifies the current disposition of the incident
    /// </summary>
    [Description("A data type for a code that identifies the current disposition of the incident.")]
    enum IncidentStatusCode
    {
        /// <summary>
        /// Active
        /// </summary>
        [NibrsCode("ACTIVE")]
        [CodeDescription("Active")]
        ACTIVE,

        /// <summary>
        /// Administratively Closed
        /// </summary>
        [NibrsCode("ADMINISTRATIVELY CLOSED")]
        [CodeDescription("Administratively Closed")]
        ADMINISTRATIVELY_CLOSED,

        /// <summary>
        /// Cleared by Arrest
        /// </summary>
        [NibrsCode("CLEARED BY ARREST")]
        [CodeDescription("Cleared by Arrest")]
        CLEARED_BY_ARREST,

        /// <summary>
        /// Cleared by Exceptional Means
        /// </summary>
        [NibrsCode("CLEARED BY EXCEPTIONAL MEANS")]
        [CodeDescription("Cleared by Exceptional Means")]
        CLEAREDBY_EXCEPTIONAL_MEANS,

        /// <summary>
        /// Closed
        /// </summary>
        [NibrsCode("CLOSED")]
        [CodeDescription("Closed")]
        CLOSED,

        /// <summary>
        /// Cold
        /// </summary>
        [NibrsCode("COLD")]
        [CodeDescription("Cold")]
        COLD,

        /// <summary>
        /// Inactive
        /// </summary>
        [NibrsCode("INACTIVE")]
        [CodeDescription("Inactive")]
        INACTIVE,

        /// <summary>
        /// Open
        /// </summary>
        [NibrsCode("OPEN")]
        [CodeDescription("Open")]
        OPEN,

        /// <summary>
        /// Pending
        /// </summary>
        [NibrsCode("PENDING")]
        [CodeDescription("Pending")]
        PENDING,

        /// <summary>
        /// Re_opened
        /// </summary>
        [NibrsCode("RE_OPENED")]
        [CodeDescription("Re_opened")]
        RE_OPENED,

        /// <summary>
        /// Unfounded
        /// </summary>
        [NibrsCode("UNFOUNDED")]
        [CodeDescription("Unfounded")]
        UNFOUNDED
    }

    /// <summary>
    /// A data type for a code that identifies the disposition of the arrest if the arrestee was a juvenile at the time of arrest
    /// </summary>
    [Description("A data type for a code that identifies the disposition of the arrest if the arrestee was a juvenile at the time of arrest.")]
    enum JuvenileDispositionCode
    {
        /// <summary>
        /// Referred to Criminal (Adult) Court
        /// </summary>
        [NibrsCode("C")]
        [CodeDescription("Referred to Criminal (Adult) Court")]
        CRIMINAL_COURT,

        /// <summary>
        /// Handled Within Department 
        /// </summary>
        [NibrsCode("H")]
        [CodeDescription("Handled Within Department ")]
        HANDLED_WITHIN_DEPARTMENT,

        /// <summary>
        /// Referred to Juvenile Court
        /// </summary>
        [NibrsCode("J")]
        [CodeDescription("Referred to Juvenile Court")]
        JUVENILE_COURT,

        /// <summary>
        /// Referred to Other Authorities
        /// </summary>
        [NibrsCode("R")]
        [CodeDescription("Referred to Other Authorities")]
        OTHER_AUTHORITIES,

        /// <summary>
        /// Referred to Welfare Agency
        /// </summary>
        [NibrsCode("w")]
        [CodeDescription("Referred to Welfare Agency")]
        WELFARE
    }

    /// <summary>
    /// A data type for a code that identifies the race of the person
    /// </summary>
    [Description("A data type for a code that identifies the race of the person.")]
    enum RACCode
    {
        /// <summary>
        /// ASIAN:  A person having origins in any of the original peoples of the Far East, Southeast Asia, or the Indian subcontinent including, for example, Cambodia, China, India, Japan, Korea, Malaysia, Pakistan, the Philippine Islands, Thailand, and Vietnam
        /// </summary>
        [NibrsCode("A")]
        [CodeDescription("ASIAN:  A person having origins in any of the original peoples of the Far East, Southeast Asia, or the Indian subcontinent including, for example, Cambodia, China, India, Japan, Korea, Malaysia, Pakistan, the Philippine Islands, Thailand, and Vietnam.")]
        ASIAN,

        /// <summary>
        /// BLACK:  A person having origins in any of the black racial groups of Africa
        /// </summary>
        [NibrsCode("B")]
        [CodeDescription("BLACK:  A person having origins in any of the black racial groups of Africa.")]
        BLACK,

        /// <summary>
        /// AMERICAN INDIAN or ALASKAN NATIVE:  A person having origins in any of the original peoples of the Americas and maintaining cultural identification through tribal affiliations or community recognition
        /// </summary>
        [NibrsCode("I")]
        [CodeDescription("AMERICAN INDIAN or ALASKAN NATIVE:  A person having origins in any of the original peoples of the Americas and maintaining cultural identification through tribal affiliations or community recognition.")]
        AMERICAN_INDIAN_OR_ALASKAN_NATIVE,

        [NibrsCode("P")]
        [CodeDescription(@"NATIVE HAWAIIAN or OTHER PACIFIC ISLANDER:  A person having origins in any of the original peoples of Hawaii, Guam, Samoa, or other Pacific Islands.  The term ""Native Hawaiian"" does not include individuals who are native to the State of Hawaii by virtue of being born there.  However, the following Pacific Islander groups are included:  Carolinian, Fijian, Kosraean, Melanesian, Micronesian, Northern Mariana Islander, Palauan, Papua New Guinean, Ponapean (Pohnpelan), Polynesian, Solomon Islander, Tahitian, Tarawa Islander, Tokelauan, Tongan, Trukese (Chuukese), and Yapese.")]
        HAWAIIAN_OR_PACIFIC_ISLANDER,

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [NibrsCode("U")]
        [CodeDescription("UNKNOWN")]
        UNKNOWN,

        /// <summary>
        /// WHITE:  A person having origins in any of the original peoples of Europe, North Africa, or Middle East
        /// </summary>
        [NibrsCode("W")]
        [CodeDescription("WHITE:  A person having origins in any of the original peoples of Europe, North Africa, or Middle East.")]
        WHITE
    }

    /// <summary>
    /// A data type for a code that identifies the victim's relationship to subject who perpetrated a crime against them, depicting who the victim was to the offender
    /// </summary>
    [Description("A data type for a code that identifies the victim's relationship to subject who perpetrated a crime against them, depicting who the victim was to the offender.")]
    enum VictimToSubjectRelationshipCode
    {
        /// <summary>
        /// Accomplice
        /// </summary>
        [NibrsCode("Accomplice")]
        [CodeDescription("Accomplice")]
        Accomplice,

        /// <summary>
        /// Acquaintance
        /// </summary>
        [NibrsCode("Acquaintance")]
        [CodeDescription("Acquaintance")]
        Acquaintance,

        /// <summary>
        /// Authority Figure
        /// </summary>
        [NibrsCode("Authority Figure")]
        [CodeDescription("Authority Figure")]
        Authority_Figure,

        /// <summary>
        /// Babysittee
        /// </summary>
        [NibrsCode("Babysittee")]
        [CodeDescription("Babysittee")]
        Babysittee,

        /// <summary>
        /// Babysitter
        /// </summary>
        [NibrsCode("Babysitter")]
        [CodeDescription("Babysitter")]
        Babysitter,

        /// <summary>
        /// Boyfriend
        /// </summary>
        [NibrsCode("Boyfriend")]
        [CodeDescription("Boyfriend")]
        Boyfriend,

        /// <summary>
        /// Caregiver
        /// </summary>
        [NibrsCode("Caregiver")]
        [CodeDescription("Caregiver")]
        Caregiver,

        /// <summary>
        /// Child of Boyfriend_Girlfriend
        /// </summary>
        [NibrsCode("Child of Boyfriend_Girlfriend")]
        [CodeDescription("Child of Boyfriend_Girlfriend")]
        Child_of_Boyfriend_Girlfriend,

        /// <summary>
        /// Client
        /// </summary>
        [NibrsCode("Client")]
        [CodeDescription("Client")]
        Client,

        /// <summary>
        /// Cohabitant
        /// </summary>
        [NibrsCode("Cohabitant")]
        [CodeDescription("Cohabitant")]
        Cohabitant,

        /// <summary>
        /// Delivery Person
        /// </summary>
        [NibrsCode("Delivery Person")]
        [CodeDescription("Delivery Person")]
        Delivery_Person,

        /// <summary>
        /// Employee
        /// </summary>
        [NibrsCode("Employee")]
        [CodeDescription("Employee")]
        Employee,

        /// <summary>
        /// Employer
        /// </summary>
        [NibrsCode("Employer")]
        [CodeDescription("Employer")]
        Employer,

        /// <summary>
        /// Ex_Spouse
        /// </summary>
        [NibrsCode("Ex_Spouse")]
        [CodeDescription("Ex_Spouse")]
        Ex_Spouse,

        /// <summary>
        /// Family Member
        /// </summary>
        [NibrsCode("Family Member")]
        [CodeDescription("Family Member")]
        Family_Member,

        /// <summary>
        /// Aunt
        /// </summary>
        [NibrsCode("Family Member_Aunt")]
        [CodeDescription("Aunt")]
        Family_Member_Aunt,

        /// <summary>
        /// Child
        /// </summary>
        [NibrsCode("Family Member_Child")]
        [CodeDescription("Child")]
        Family_Member_Child,

        /// <summary>
        /// Cousin
        /// </summary>
        [NibrsCode("Family Member_Cousin")]
        [CodeDescription("Cousin")]
        Family_Member_Cousin,

        /// <summary>
        /// Family Member_Foster Child
        /// </summary>
        [NibrsCode("Family Member_Foster Child")]
        [CodeDescription("Family Member_Foster Child")]
        Family_Member_Foster_Child,

        /// <summary>
        /// Family Member_Foster Parent
        /// </summary>
        [NibrsCode("Family Member_Foster Parent")]
        [CodeDescription("Family Member_Foster Parent")]
        Family_Member_Foster_Parent,

        /// <summary>
        /// Grandchild
        /// </summary>
        [NibrsCode("Family Member_Grandchild")]
        [CodeDescription("Grandchild")]
        Family_Member_Grandchild,

        /// <summary>
        /// Grandparent
        /// </summary>
        [NibrsCode("Family Member_Grandparent")]
        [CodeDescription("Grandparent")]
        Family_Member_Grandparent,

        /// <summary>
        /// In-Law
        /// </summary>
        [NibrsCode("Family Member_In-Law")]
        [CodeDescription("In-Law")]
        Family_Member_In_Law,

        /// <summary>
        /// Nephew
        /// </summary>
        [NibrsCode("Family Member_Nephew")]
        [CodeDescription("Nephew")]
        Family_Member_Nephew,

        /// <summary>
        /// Niece
        /// </summary>
        [NibrsCode("Family Member_Niece")]
        [CodeDescription("Niece")]
        Family_Member_Niece,

        /// <summary>
        /// Parent
        /// </summary>
        [NibrsCode("Family Member_Parent")]
        [CodeDescription("Parent")]
        Family_Member_Parent,

        /// <summary>
        /// Sibling
        /// </summary>
        [NibrsCode("Family Member_Sibling")]
        [CodeDescription("Sibling")]
        Family_Member_Sibling,

        /// <summary>
        /// Family Member_Spouse 
        /// </summary>
        [NibrsCode("Family Member_Spouse")]
        [CodeDescription("Family Member_Spouse ")]
        Family_Member_Spouse,

        /// <summary>
        /// Spouse_Common Law
        /// </summary>
        [NibrsCode("Family Member_Spouse_Common Law")]
        [CodeDescription("Spouse_Common Law")]
        Family_Member_Spouse_Common_Law,

        /// <summary>
        /// Stepchild
        /// </summary>
        [NibrsCode("Family Member_Stepchild")]
        [CodeDescription("Stepchild")]
        Family_Member_Stepchild,

        /// <summary>
        /// Stepparent
        /// </summary>
        [NibrsCode("Family Member_Stepparent")]
        [CodeDescription("Stepparent")]
        Family_Member_Stepparent,

        /// <summary>
        /// Stepsibling
        /// </summary>
        [NibrsCode("Family Member_Stepsibling")]
        [CodeDescription("Stepsibling")]
        Family_Member_Stepsibling,

        /// <summary>
        /// Uncle
        /// </summary>
        [NibrsCode("Family Member_Uncle")]
        [CodeDescription("Uncle")]
        Family_Member_Uncle,

        /// <summary>
        /// Former Employee
        /// </summary>
        [NibrsCode("Former Employee")]
        [CodeDescription("Former Employee")]
        Former_Employee,

        /// <summary>
        /// Former Employer
        /// </summary>
        [NibrsCode("Former Employer")]
        [CodeDescription("Former Employer")]
        Former_Employer,

        /// <summary>
        /// Friend
        /// </summary>
        [NibrsCode("Friend")]
        [CodeDescription("Friend")]
        Friend,

        /// <summary>
        /// Girlfriend
        /// </summary>
        [NibrsCode("Girlfriend")]
        [CodeDescription("Girlfriend")]
        Girlfriend,

        /// <summary>
        /// Guardian
        /// </summary>
        [NibrsCode("Guardian")]
        [CodeDescription("Guardian")]
        Guardian,

        /// <summary>
        /// Homosexual relationship
        /// </summary>
        [NibrsCode("Homosexual relationship")]
        [CodeDescription("Homosexual relationship")]
        Homosexual_relationship,

        /// <summary>
        /// Neighbor
        /// </summary>
        [NibrsCode("Neighbor")]
        [CodeDescription("Neighbor")]
        Neighbor,

        /// <summary>
        /// NonFamily_Otherwise Known
        /// </summary>
        [NibrsCode("NonFamily_Otherwise Known")]
        [CodeDescription("NonFamily_Otherwise Known")]
        NonFamily_Otherwise_Known,

        /// <summary>
        /// Patient
        /// </summary>
        [NibrsCode("Patient")]
        [CodeDescription("Patient")]
        Patient,

        /// <summary>
        /// Relationship Unknown 
        /// </summary>
        [NibrsCode("Relationship Unknown")]
        [CodeDescription("Relationship Unknown ")]
        Relationship_Unknown,

        /// <summary>
        /// Stranger
        /// </summary>
        [NibrsCode("Stranger")]
        [CodeDescription("Stranger")]
        Stranger,

        /// <summary>
        /// Student
        /// </summary>
        [NibrsCode("Student")]
        [CodeDescription("Student")]
        Student,

        /// <summary>
        /// Teacher
        /// </summary>
        [NibrsCode("Teacher")]
        [CodeDescription("Teacher")]
        Teacher,

        /// <summary>
        /// Victim Was Offender
        /// </summary>
        [NibrsCode("Victim Was Offender")]
        [CodeDescription("Victim Was Offender")]
        Victim_Was_Offender
    }  

    /// <summary>
    /// A data type for kinds of arrests that can occur
    /// </summary>
    [Description("A data type for kinds of arrests that can occur.")]
    enum ArrestCategoryCode
    {
        /// <summary>
        /// On-View Arrest
        /// </summary>
        [NibrsCode("O")]
        [CodeDescription("On-View Arrest")]
        ON_VIEW_ARREST,

        /// <summary>
        /// Summoned/ Cited
        /// </summary>
        [NibrsCode("S")]
        [CodeDescription("Summoned/ Cited")]
        SUMMONED_CITED,

        /// <summary>
        /// Taken Into Custody
        /// </summary>
        [NibrsCode("T")]
        [CodeDescription("Taken Into Custody")]
        TAKEN_INTO_CUSTODY
    }

    /// <summary>
    /// A data type for weapons with which a subject may be armed with upon apprehension
    /// </summary>
    [Description("A data type for weapons with which a subject may be armed with upon apprehension.")]
    enum ArresteeWeaponCode
    {
        /// <summary>
        /// Unarmed
        /// </summary>
        [NibrsCode("01")]
        [CodeDescription("Unarmed")]
        UNARMED,

        /// <summary>
        /// Firearm (type not stated)
        /// </summary>
        [NibrsCode("11")]
        [CodeDescription("Firearm (type not stated)")]
        FIREARM,

        /// <summary>
        /// Firearm (type not stated) - Automatic
        /// </summary>
        [NibrsCode("11A")]
        [CodeDescription("Firearm (type not stated) - Automatic")]
        AUTOMATIC_FIREARM,

        /// <summary>
        /// Handgun
        /// </summary>
        [NibrsCode("12")]
        [CodeDescription("Handgun")]
        HANDGUN,

        /// <summary>
        /// Handgun - Automatic
        /// </summary>
        [NibrsCode("12A")]
        [CodeDescription("Handgun - Automatic")]
        AUTOMATIC_HANDGUN,

        /// <summary>
        /// Rifle
        /// </summary>
        [NibrsCode("13")]
        [CodeDescription("Rifle")]
        RIFLE,

        /// <summary>
        /// Rifle - Automatic
        /// </summary>
        [NibrsCode("13A")]
        [CodeDescription("Rifle - Automatic")]
        AUTOMATIC_RIFLE,

        /// <summary>
        /// Shotgun
        /// </summary>
        [NibrsCode("14")]
        [CodeDescription("Shotgun")]
        SHOTGUN,

        /// <summary>
        /// Shotgun - Automatic
        /// </summary>
        [NibrsCode("14A")]
        [CodeDescription("Shotgun - Automatic")]
        AUTOMATIC_SHOTGUN,

        /// <summary>
        /// Other Firearm
        /// </summary>
        [NibrsCode("15")]
        [CodeDescription("Other Firearm")]
        OTHER_FIREARM,

        /// <summary>
        /// Other Firearm - Automatic
        /// </summary>
        [NibrsCode("15A")]
        [CodeDescription("Other Firearm - Automatic")]
        OTHER_AUTOMATIC_FIREARM,

        /// <summary>
        /// Lethal Cutting Instrument
        /// </summary>
        [NibrsCode("16")]
        [CodeDescription("Lethal Cutting Instrument")]
        LETHAL_CUTTING_INSTRUMENT,

        /// <summary>
        /// Club/ Blackjack/ Brass Knuckles
        /// </summary>
        [NibrsCode("17")]
        [CodeDescription("Club/ Blackjack/ Brass Knuckles")]
        CLUB_BLACKJACK_BRASS_KNUCKLES
    }

    /// <summary>
    /// A data type for kinds of drugs
    /// </summary>
    [Description("A data type for kinds of drugs.")]
    enum DrugCategoryCode
    {
        /// <summary>
        /// crack cocaine
        /// </summary>
        [NibrsCode("A")]
        [CodeDescription("crack cocaine")]
        CRACK_COCAINE,

        /// <summary>
        /// cocaine
        /// </summary>
        [NibrsCode("B")]
        [CodeDescription("cocaine")]
        COCAINE,

        /// <summary>
        /// hashish
        /// </summary>
        [NibrsCode("C")]
        [CodeDescription("hashish")]
        HASHISH,

        /// <summary>
        /// heroin
        /// </summary>
        [NibrsCode("D")]
        [CodeDescription("heroin")]
        HEROIN,

        /// <summary>
        /// marijuana
        /// </summary>
        [NibrsCode("E")]
        [CodeDescription("marijuana")]
        MARIJUANA,

        /// <summary>
        /// morphine
        /// </summary>
        [NibrsCode("F")]
        [CodeDescription("morphine")]
        MORPHINE,

        /// <summary>
        /// opium
        /// </summary>
        [NibrsCode("G")]
        [CodeDescription("opium")]
        OPIUM,

        /// <summary>
        /// other narcotics
        /// </summary>
        [NibrsCode("H")]
        [CodeDescription("other narcotics")]
        OTHER_NARCOTIC,

        /// <summary>
        /// LSD
        /// </summary>
        [NibrsCode("I")]
        [CodeDescription("LSD")]
        LSD,

        /// <summary>
        /// PCP
        /// </summary>
        [NibrsCode("J")]
        [CodeDescription("PCP")]
        PCP,

        /// <summary>
        /// other hallucinogens
        /// </summary>
        [NibrsCode("K")]
        [CodeDescription("other hallucinogens")]
        OTHER_HALLUCINOGEN,

        /// <summary>
        /// amphetamines/ methamphetamines
        /// </summary>
        [NibrsCode("L")]
        [CodeDescription("amphetamines/ methamphetamines")]
        AMPHETAMINES_METHAPHETAMINES,

        /// <summary>
        /// other stimulants
        /// </summary>
        [NibrsCode("M")]
        [CodeDescription("other stimulants")]
        OTHER_STIMULANT,

        /// <summary>
        /// barbiturates
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("barbiturates")]
        BARBITURATES,

        /// <summary>
        /// other depressants
        /// </summary>
        [NibrsCode("O")]
        [CodeDescription("other depressants")]
        OTHER_DEPRESSANT,

        /// <summary>
        /// other drugs
        /// </summary>
        [NibrsCode("P")]
        [CodeDescription("other drugs")]
        OTHER_DRUG,

        /// <summary>
        /// unknown drug type
        /// </summary>
        [NibrsCode("U")]
        [CodeDescription("unknown drug type")]
        UNKNOWN_DRUG_TYPE,

        /// <summary>
        /// over 3 drug types
        /// </summary>
        [NibrsCode("X")]
        [CodeDescription("over 3 drug types")]
        OVER_THREE_DRUG_TYPES
    }

    /// <summary>
    /// A data type for the kinds of measurements used to specify a quantity of drugs or narcotics
    /// </summary>
    [Description("A data type for the kinds of measurements used to specify a quantity of drugs or narcotics.")]
    enum DrugMeasurementCode
    {
        /// <summary>
        /// Dosage Units/ Items
        /// </summary>
        [NibrsCode("DU")]
        [CodeDescription("Dosage Units/ Items")]
        DOSAGE_UNITS_OR_ITEMS,

        /// <summary>
        /// Fluid Ounce
        /// </summary>
        [NibrsCode("FO")]
        [CodeDescription("Fluid Ounce")]
        FLUID_OUNCE,

        /// <summary>
        /// Gallon
        /// </summary>
        [NibrsCode("GL")]
        [CodeDescription("Gallon")]
        GALLON,

        /// <summary>
        /// Gram
        /// </summary>
        [NibrsCode("GM")]
        [CodeDescription("Gram")]
        GRAM,

        /// <summary>
        /// Kilogram
        /// </summary>
        [NibrsCode("KG")]
        [CodeDescription("Kilogram")]
        KILOGRAM,

        /// <summary>
        /// Pound
        /// </summary>
        [NibrsCode("LB")]
        [CodeDescription("Pound")]
        POUND,

        /// <summary>
        /// Liter
        /// </summary>
        [NibrsCode("LT")]
        [CodeDescription("Liter")]
        LITER,

        /// <summary>
        /// Milliliter
        /// </summary>
        [NibrsCode("ML")]
        [CodeDescription("Milliliter")]
        MILLILITER,

        /// <summary>
        /// Number of Plants
        /// </summary>
        [NibrsCode("NP")]
        [CodeDescription("Number of Plants")]
        NUMBER_OF_PLANTS,

        /// <summary>
        /// Ounce
        /// </summary>
        [NibrsCode("OZ")]
        [CodeDescription("Ounce")]
        OUNCE,

        /// <summary>
        /// Not Reported
        /// </summary>
        [NibrsCode("XX")]
        [CodeDescription("Not Reported")]
        NOT_REPORTED
    }

    /// <summary>
    /// A data type for kinds of cultural lineages of a person
    /// </summary>
    [Description("A data type for kinds of cultural lineages of a person.")]
    enum EthnicityCode
    {
        /// <summary>
        /// Hispanic or Latino
        /// </summary>
        [NibrsCode("H")]
        [CodeDescription("Hispanic or Latino")]
        HISPANIC_OR_LATINO,

        /// <summary>
        /// Not Hispanic or Latino
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("Not Hispanic or Latino")]
        NOT_HISPANIC_OR_LATINO,

        /// <summary>
        /// Unknown
        /// </summary>
        [NibrsCode("U")]
        [CodeDescription("Unknown")]
        UNKNOWN
    }

    /// <summary>
    /// A data type for kinds of weapons or forces used by a subject in committing an offense
    /// </summary>
    [Description("A data type for kinds of weapons or forces used by a subject in committing an offense.")]
    enum ForceCategoryCode
    {
        /// <summary>
        /// Firearm (type not stated)
        /// </summary>
        [NibrsCode("11")]
        [CodeDescription("Firearm (type not stated)")]
        FIREARM,

        /// <summary>
        /// Automatic Firearm (type not stated)
        /// </summary>
        [NibrsCode("11A")]
        [CodeDescription("Automatic Firearm (type not stated)")]
        AUTOMATIC_FIREARM,

        /// <summary>
        /// Handgun
        /// </summary>
        [NibrsCode("12")]
        [CodeDescription("Handgun")]
        HANDGUN,

        /// <summary>
        /// Automatic Handgun
        /// </summary>
        [NibrsCode("12A")]
        [CodeDescription("Automatic Handgun")]
        AUTOMATIC_HANDGUN,

        /// <summary>
        /// Rifle
        /// </summary>
        [NibrsCode("13")]
        [CodeDescription("Rifle")]
        RIFLE,

        /// <summary>
        /// Automatic Rifle
        /// </summary>
        [NibrsCode("13A")]
        [CodeDescription("Automatic Rifle")]
        AUTOMATIC_RIFLE,

        /// <summary>
        /// Shotgun
        /// </summary>
        [NibrsCode("14")]
        [CodeDescription("Shotgun")]
        SHOTGUN,

        /// <summary>
        /// Automatic Shotgun
        /// </summary>
        [NibrsCode("14A")]
        [CodeDescription("Automatic Shotgun")]
        AUTOMATIC_SHOTGUN,

        /// <summary>
        /// Other Firearm
        /// </summary>
        [NibrsCode("15")]
        [CodeDescription("Other Firearm")]
        OTHER_FIREARM,

        /// <summary>
        /// Other Automatic Firearm
        /// </summary>
        [NibrsCode("15A")]
        [CodeDescription("Other Automatic Firearm")]
        OTHER_AUTOMATIC_FIREARM,

        /// <summary>
        /// Knife/Cutting Instrument
        /// </summary>
        [NibrsCode("20")]
        [CodeDescription("Knife/Cutting Instrument")]
        LETHAL_CUTTING_INSTRUMENT,

        /// <summary>
        /// Blunt Object
        /// </summary>
        [NibrsCode("30")]
        [CodeDescription("Blunt Object")]
        BLUNT_OBJECT,

        /// <summary>
        /// Motor Vehicle
        /// </summary>
        [NibrsCode("35")]
        [CodeDescription("Motor Vehicle")]
        MOTOR_VEHICLE,

        /// <summary>
        /// Personal Weapons
        /// </summary>
        [NibrsCode("40")]
        [CodeDescription("Personal Weapons")]
        PERSONAL_WEAPONS,

        /// <summary>
        /// Poison
        /// </summary>
        [NibrsCode("50")]
        [CodeDescription("Poison")]
        POISON,

        /// <summary>
        /// Explosives
        /// </summary>
        [NibrsCode("60")]
        [CodeDescription("Explosives")]
        EXPLOSIVES,

        /// <summary>
        /// Fire/ Incendiary Device
        /// </summary>
        [NibrsCode("65")]
        [CodeDescription("Fire/ Incendiary Device")]
        FIRE_INCENDIARY_DEVICE,

        /// <summary>
        /// Drugs/ Narcotics/ Sleeping Pills
        /// </summary>
        [NibrsCode("70")]
        [CodeDescription("Drugs/ Narcotics/ Sleeping Pills")]
        DRUGS_NARCOTICS_SLEEPING_PILLS,

        /// <summary>
        /// Asphyxiation
        /// </summary>
        [NibrsCode("85")]
        [CodeDescription("Asphyxiation")]
        ASPHYXIATION,

        /// <summary>
        /// Other
        /// </summary>
        [NibrsCode("90")]
        [CodeDescription("Other")]
        OTHER,

        /// <summary>
        /// Unknown
        /// </summary>
        [NibrsCode("95")]
        [CodeDescription("Unknown")]
        UNKNOWN,

        /// <summary>
        /// None
        /// </summary>
        [NibrsCode("99")]
        [CodeDescription("None")]
        NONE
    }

    /// <summary>
    /// A data type for ways in which an incident may be cleared exceptionally
    /// </summary>
    [Description("A data type for ways in which an incident may be cleared exceptionally.")]
    enum IncidentExceptionalClearanceCode
    {
        /// <summary>
        /// Death of Offender
        /// </summary>
        [NibrsCode("A")]
        [CodeDescription("Death of Offender")]
        DEATH_OF_OFFENDER,

        /// <summary>
        /// Prosecution Declined (by the prosecutor for other than lack of probable cause)
        /// </summary>
        [NibrsCode("B")]
        [CodeDescription("Prosecution Declined (by the prosecutor for other than lack of probable cause)")]
        PROSECUTION_DECLINED,

        /// <summary>
        /// In Custody of Other Jurisdiction
        /// </summary>
        [NibrsCode("C")]
        [CodeDescription("In Custody of Other Jurisdiction")]
        IN_CUSTODY,

        /// <summary>
        /// Victim Refused to Cooperate (in the prosecution)
        /// </summary>
        [NibrsCode("D")]
        [CodeDescription("Victim Refused to Cooperate (in the prosecution)")]
        VICTIM_REFUSED_TO_COOPERATE,

        /// <summary>
        /// Juvenile/No Custody (the handling of a juvenile without taking him/her into custody, but rather by oral or written notice given to the parents or legal guardian in a case involving a minor offense, such as a petty larceny)
        /// </summary>
        [NibrsCode("E")]
        [CodeDescription("Juvenile/No Custody (the handling of a juvenile without taking him/her into custody, but rather by oral or written notice given to the parents or legal guardian in a case involving a minor offense, such as a petty larceny)")]
        JUVENILE_NO_CUSTODY,

        /// <summary>
        /// Not Applicable (not cleared exceptionally)
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("Not Applicable (not cleared exceptionally)")]
        NOT_APPLICABLE
    }

    /// <summary>
    /// A data type for factors that may have been involved in or contributed to a subject committing an offense
    /// </summary>
    [Description("A data type for factors that may have been involved in or contributed to a subject committing an offense.")]
    enum IncidentFactorCode
    {
        /// <summary>
        /// Alcohol
        /// </summary>
        [NibrsCode("A")]
        [CodeDescription("Alcohol")]
        ALCOHOL,

        /// <summary>
        /// Computer Equipment
        /// </summary>
        [NibrsCode("C")]
        [CodeDescription("Computer Equipment")]
        COMPUTER_EQUIPMENT,

        /// <summary>
        /// Drugs/ Narcotics
        /// </summary>
        [NibrsCode("D")]
        [CodeDescription("Drugs/ Narcotics")]
        DRUGS_NARCOTICS,

        /// <summary>
        /// Not Applicable
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("Not Applicable")]
        NOT_APPLICABLE
    }

    /// <summary>
    /// A data type for a general category of harm or injury
    /// </summary>
    [Description("A data type for a general category of harm or injury.")]
    enum InjuryCategoryCode
    {
        /// <summary>
        /// apparent broken bones
        /// </summary>
        [NibrsCode("B")]
        [CodeDescription("apparent broken bones")]
        BROKEN_BONES,

        /// <summary>
        /// possible internal injury
        /// </summary>
        [NibrsCode("I")]
        [CodeDescription("possible internal injury")]
        INTERNAL_INJURY,

        /// <summary>
        /// severe laceration
        /// </summary>
        [NibrsCode("L")]
        [CodeDescription("severe laceration")]
        SEVERE_LACERATION,

        /// <summary>
        /// apparent minor injury
        /// </summary>
        [NibrsCode("M")]
        [CodeDescription("apparent minor injury")]
        MINOR_INJURY,

        /// <summary>
        /// none
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("none")]
        NONE,

        /// <summary>
        /// other major injury
        /// </summary>
        [NibrsCode("O")]
        [CodeDescription("other major injury")]
        OTHER_MAJOR_INJURY,

        /// <summary>
        /// loss of teeth
        /// </summary>
        [NibrsCode("T")]
        [CodeDescription("loss of teeth")]
        LOSS_OF_TEETH,

        /// <summary>
        /// unconsciousness
        /// </summary>
        [NibrsCode("U")]
        [CodeDescription("unconsciousness")]
        UNCONSCIOUSNESS
    }

    /// <summary>
    /// A data type for circumstances of a justifiable homicide
    /// </summary>
    [Description("A data type for circumstances of a justifiable homicide.")]
    enum JustifiableHomicideFactorsCode
    {
        /// <summary>
        /// Criminal Attacked Police Officer and That Officer Killed Criminal
        /// </summary>
        [NibrsCode("A")]
        [CodeDescription("Criminal Attacked Police Officer and That Officer Killed Criminal")]
        CRIMINAL_ATTACKED_OFFICER_AND_THAT_OFFICER_KILLED_CRIMINAL,

        /// <summary>
        /// Criminal Attacked Police Officer and Criminal Killed by Another Police Officer
        /// </summary>
        [NibrsCode("B")]
        [CodeDescription("Criminal Attacked Police Officer and Criminal Killed by Another Police Officer")]
        CRIMINAL_ATTACKED_OFFICER_AND_OTHER_OFFICER_KILLED_CRIMINAL,

        /// <summary>
        /// Criminal Attacked a Civilian
        /// </summary>
        [NibrsCode("C")]
        [CodeDescription("Criminal Attacked a Civilian")]
        CRIMINAL_ATTACKED_CIVILIAN,

        /// <summary>
        /// Criminal Attempted Flight From a Crime
        /// </summary>
        [NibrsCode("D")]
        [CodeDescription("Criminal Attempted Flight From a Crime")]
        CRIMINAL_ATTEMPTED_FLIGHT,

        /// <summary>
        /// Criminal Killed in Commission of a Crime
        /// </summary>
        [NibrsCode("E")]
        [CodeDescription("Criminal Killed in Commission of a Crime")]
        CRIMINAL_KILLED_IN_COMMISSION_OF_CRIME,

        /// <summary>
        /// Criminal Resisted Arrest
        /// </summary>
        [NibrsCode("F")]
        [CodeDescription("Criminal Resisted Arrest")]
        CRIMINAL_RESISTED_ARREST,

        /// <summary>
        /// Unable to Determine/Not Enough Information
        /// </summary>
        [NibrsCode("G")]
        [CodeDescription("Unable to Determine/Not Enough Information")]
        NONDETERMINABLE_LACK_OF_INFORMATION
    }

    /// <summary>
    /// A data type for a kind of activity or circumstance in which a law enforcement officer was engaged at the time of assault
    /// </summary>
    [Description("A data type for a kind of activity or circumstance in which a law enforcement officer was engaged at the time of assault.")]
    enum LEOKAActivityCategoryCode
    {
        /// <summary>
        /// Responding to Disturbance Call (Family Quarrels, Person with Firearm, Etc)
        /// </summary>
        [NibrsCode("01")]
        [CodeDescription("Responding to Disturbance Call (Family Quarrels, Person with Firearm, Etc.)")]
        RESPONDING_TO_DISTURBANCE_CALL,

        /// <summary>
        /// Burglaries in Progress or Pursuing Burglary Suspects
        /// </summary>
        [NibrsCode("02")]
        [CodeDescription("Burglaries in Progress or Pursuing Burglary Suspects")]
        BURGLARIES_IN_PROGRESS_OR_PURSUING_BURGLARY_SUSPECTS,

        /// <summary>
        /// Robberies in Progress or Pursuing Robbery Suspects
        /// </summary>
        [NibrsCode("03")]
        [CodeDescription("Robberies in Progress or Pursuing Robbery Suspects")]
        ROBBERIES_IN_PROGRESS_OR_PURSUING_ROBBERY_SUSPECTS,

        /// <summary>
        /// Attempting Other Arrests
        /// </summary>
        [NibrsCode("04")]
        [CodeDescription("Attempting Other Arrests")]
        ATTEMPTING_OTHER_ARRESTS,

        /// <summary>
        /// Civil Disorder (Riot, Mass Disobedience)
        /// </summary>
        [NibrsCode("05")]
        [CodeDescription("Civil Disorder (Riot, Mass Disobedience)")]
        CIVIL_DISORDER,

        /// <summary>
        /// Handling, Transporting, Custody of Prisoners
        /// </summary>
        [NibrsCode("06")]
        [CodeDescription("Handling, Transporting, Custody of Prisoners")]
        HANDLING_OR_TRANSPORTING_CUSTODY_OF_PRISONERS,

        /// <summary>
        /// Investigating Suspicious Persons or Circumstances
        /// </summary>
        [NibrsCode("07")]
        [CodeDescription("Investigating Suspicious Persons or Circumstances")]
        INVESTIGATING_SUSPICIOUS_PERSONS_OR_CIRCUMSTANCES,

        /// <summary>
        /// Ambush-No Warning
        /// </summary>
        [NibrsCode("08")]
        [CodeDescription("Ambush-No Warning")]
        AMBUSH,

        /// <summary>
        /// Handling Persons with Mental Illness
        /// </summary>
        [NibrsCode("09")]
        [CodeDescription("Handling Persons with Mental Illness")]
        HANDLING_PERSONS_WITH_MENTAL_ILLNESS,

        /// <summary>
        /// Traffic Pursuits and Stops
        /// </summary>
        [NibrsCode("10")]
        [CodeDescription("Traffic Pursuits and Stops")]
        TRAFFIC_PURSUITS_AND_STOPS,

        /// <summary>
        /// All Other
        /// </summary>
        [NibrsCode("11")]
        [CodeDescription("All Other")]
        OTHER
    }

    /// <summary>
    /// A data type for an assignment a law enforcement officer was on when assaulted
    /// </summary>
    [Description("A data type for an assignment a law enforcement officer was on when assaulted.")]
    enum LEOKAOfficerAssignmentCategoryCode
    {
        /// <summary>
        /// Two-Officer Vehicle - uniformed law enforcement officers
        /// </summary>
        [NibrsCode("F")]
        [CodeDescription("Two-Officer Vehicle - uniformed law enforcement officers")]
        TWO_OFFICER_VEHICLE,

        /// <summary>
        /// One-Officer Vehicle (Alone) - uniformed law enforcement officers
        /// </summary>
        [NibrsCode("G")]
        [CodeDescription("One-Officer Vehicle (Alone) - uniformed law enforcement officers")]
        ONE_OFFICER_VEHICLE_ALONE,

        /// <summary>
        /// One-Officer Vehicle (Assisted) - uniformed law enforcement officers
        /// </summary>
        [NibrsCode("H")]
        [CodeDescription("One-Officer Vehicle (Assisted) - uniformed law enforcement officers")]
        ONE_OFFICER_VEHICLE_ASSISTED,

        /// <summary>
        /// Detective or Special Assignment (Alone) - non-uniformed officers
        /// </summary>
        [NibrsCode("I")]
        [CodeDescription("Detective or Special Assignment (Alone) - non-uniformed officers")]
        DETECTIVE_OR_SPECIAL_ASSIGNLENT_ALONE,

        /// <summary>
        /// Detective or Special Assignment (Assisted) - non-uniformed officers
        /// </summary>
        [NibrsCode("J")]
        [CodeDescription("Detective or Special Assignment (Assisted) - non-uniformed officers")]
        DETECTIVE_OR_SPECIAL_ASSIGNLENT_ASSISTED,

        /// <summary>
        /// Other (Alone) - law enforcement officers serving in other capacities (foot patrol, off duty, etc)
        /// </summary>
        [NibrsCode("K")]
        [CodeDescription("Other (Alone) - law enforcement officers serving in other capacities (foot patrol, off duty, etc.)")]
        OTHER_ALONE,

        /// <summary>
        /// Other (Assisted) - law enforcement officers serving in other capacities (foot patrol, off duty, etc)
        /// </summary>
        [NibrsCode("L")]
        [CodeDescription("Other (Assisted) - law enforcement officers serving in other capacities (foot patrol, off duty, etc.)")]
        OTHER_ASSISTED
    }

    /// <summary>
    /// A data type for methods of entry into a structure or premises
    /// </summary>
    [Description("A data type for methods of entry into a structure or premises.")]
    enum MethodOfEntryCode
    {
        /// <summary>
        /// Force
        /// </summary>
        [NibrsCode("F")]
        [CodeDescription("Force")]
        FORCE,

        /// <summary>
        /// No Force
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("No Force")]
        NO_FORCE
    }

    /// <summary>
    /// A data type for the ways in which an arrested subject is counted or scored in a system so that a subject is counted only once despite potentially multiple arrests at a time
    /// </summary>
    [Description("A data type for the ways in which an arrested subject is counted or scored in a system so that a subject is counted only once despite potentially multiple arrests at a time.")]
    enum MultipleArresteeSegmentsCode
    {

        /// <summary>
        /// Count Arrestee
        /// </summary>
        [NibrsCode("C")]
        [CodeDescription("Count Arrestee")]
        COUNT,

        /// <summary>
        /// Multiple
        /// </summary>
        [NibrsCode("M")]
        [CodeDescription("Multiple")]
        MULTIPLE,

        /// <summary>
        /// Not Applicable
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("Not Applicable")]
        NOT_APPLICABLE
    }

    /// <summary>
    /// A data type for categories or classifications of a property item
    /// </summary>
    [Description("A data type for categories or classifications of a property item.")]
    enum PropertyCategoryCode
    {
        /// <summary>
        /// aircraft
        /// </summary>
        [NibrsCode("01")]
        [CodeDescription("aircraft")]
        AIRCRAFT,

        /// <summary>
        /// alcohol
        /// </summary>
        [NibrsCode("02")]
        [CodeDescription("alcohol")]
        ALCOHOL,

        /// <summary>
        /// automobile
        /// </summary>
        [NibrsCode("03")]
        [CodeDescription("automobile")]
        AUTOMOBILE,

        /// <summary>
        /// bicycles
        /// </summary>
        [NibrsCode("04")]
        [CodeDescription("bicycles")]
        BICYCLES,

        /// <summary>
        /// buses
        /// </summary>
        [NibrsCode("05")]
        [CodeDescription("buses")]
        BUSES,

        /// <summary>
        /// clothes/ furs
        /// </summary>
        [NibrsCode("06")]
        [CodeDescription("clothes/ furs")]
        CLOTHING,

        /// <summary>
        /// computer hardware/ software
        /// </summary>
        [NibrsCode("07")]
        [CodeDescription("computer hardware/ software")]
        COMPUTER_HARDWARE_SOFTWARE,

        /// <summary>
        /// consumable goods
        /// </summary>
        [NibrsCode("08")]
        [CodeDescription("consumable goods")]
        CONSUMABLES,

        /// <summary>
        /// credit/ debit cards
        /// </summary>
        [NibrsCode("09")]
        [CodeDescription("credit/ debit cards")]
        CREDIT_DEBIT_CARDS,

        /// <summary>
        /// drugs/ narcotics
        /// </summary>
        [NibrsCode("10")]
        [CodeDescription("drugs/ narcotics")]
        DRUGS_NARCOTICS,

        /// <summary>
        /// drug/ narcotic equipment
        /// </summary>
        [NibrsCode("11")]
        [CodeDescription("drug/ narcotic equipment")]
        DRUG_NARCOTIC_EQUIPMENT,

        /// <summary>
        /// farm equipment
        /// </summary>
        [NibrsCode("12")]
        [CodeDescription("farm equipment")]
        FARM_EQUIPMENT,

        /// <summary>
        /// firearms
        /// </summary>
        [NibrsCode("13")]
        [CodeDescription("firearms")]
        FIREARMS,

        /// <summary>
        /// gambling equipment
        /// </summary>
        [NibrsCode("14")]
        [CodeDescription("gambling equipment")]
        GAMBLING_EQUIPMENT,

        /// <summary>
        /// heavy construction/ industrial equipment
        /// </summary>
        [NibrsCode("15")]
        [CodeDescription("heavy construction/ industrial equipment")]
        CONSTRUCTION_INDUSTRIAL_EQUIPMENT,

        /// <summary>
        /// household goods
        /// </summary>
        [NibrsCode("16")]
        [CodeDescription("household goods")]
        HOUSEHOLD_GOODS,

        /// <summary>
        /// jewelry/ precious metals/ gems
        /// </summary>
        [NibrsCode("17")]
        [CodeDescription("jewelry/ precious metals/ gems")]
        JEWELRY_PRECIOUS_METALS_GEMS,

        /// <summary>
        /// livestock
        /// </summary>
        [NibrsCode("18")]
        [CodeDescription("livestock")]
        LIVESTOCK,

        /// <summary>
        /// merchandise
        /// </summary>
        [NibrsCode("19")]
        [CodeDescription("merchandise")]
        MERCHANDISE,

        /// <summary>
        /// money
        /// </summary>
        [NibrsCode("20")]
        [CodeDescription("money")]
        MONEY,

        /// <summary>
        /// negotiable instruments
        /// </summary>
        [NibrsCode("21")]
        [CodeDescription("negotiable instruments")]
        NEGOTIABLE_INSTRUMENTS,

        /// <summary>
        /// nonnegotiable instruments
        /// </summary>
        [NibrsCode("22")]
        [CodeDescription("nonnegotiable instruments")]
        NONNEGOTIABLE_INSTRUMENTS,

        /// <summary>
        /// office-type equipment
        /// </summary>
        [NibrsCode("23")]
        [CodeDescription("office-type equipment")]
        OFFICE_TYPE_EQUIPMENT,

        /// <summary>
        /// other motor vehicles
        /// </summary>
        [NibrsCode("24")]
        [CodeDescription("other motor vehicles")]
        OTHER_MOTOR_VEHICLES,

        /// <summary>
        /// purses/ handbags/ wallets
        /// </summary>
        [NibrsCode("25")]
        [CodeDescription("purses/ handbags/ wallets")]
        PURSES_HANDBAGS_WALLETS,

        /// <summary>
        /// radios/ tvs/ vcrs/ dvd players
        /// </summary>
        [NibrsCode("26")]
        [CodeDescription("radios/ tvs/ vcrs/ dvd players")]
        RADIOS_TVS_VCRS_DVD_PLAYERS,

        /// <summary>
        /// recordings - audio/ visual
        /// </summary>
        [NibrsCode("27")]
        [CodeDescription("recordings - audio/ visual")]
        AUDIO_VISUAL_RECORDINGS,

        /// <summary>
        /// recreational vehicles
        /// </summary>
        [NibrsCode("28")]
        [CodeDescription("recreational vehicles")]
        RECREATIONAL_VEHICLES,

        /// <summary>
        /// structures - single occupancy dwellings
        /// </summary>
        [NibrsCode("29")]
        [CodeDescription("structures - single occupancy dwellings")]
        SINGLE_OCCUPANCY_DWELLING_STRUCTURE,

        /// <summary>
        /// structures - other dwellings
        /// </summary>
        [NibrsCode("30")]
        [CodeDescription("structures - other dwellings")]
        OTHER_DWELLING_STRUCTURE,

        /// <summary>
        /// structures - other commercial/ business
        /// </summary>
        [NibrsCode("31")]
        [CodeDescription("structures - other commercial/ business")]
        OTHER_COMMERCIAL_BUSINESS_STRUCTURE,

        /// <summary>
        /// structures - industrial/ manufacturing
        /// </summary>
        [NibrsCode("32")]
        [CodeDescription("structures - industrial/ manufacturing")]
        INDUSTRIAL_MANUFACTURING_STRUCTURE,

        /// <summary>
        /// structures - public/ community
        /// </summary>
        [NibrsCode("33")]
        [CodeDescription("structures - public/ community")]
        PUBLIC_COMMUNITY_STRUCTURE,

        /// <summary>
        /// structures - storage
        /// </summary>
        [NibrsCode("34")]
        [CodeDescription("structures - storage")]
        STORAGE_STRUCTURE,

        /// <summary>
        /// structures - other
        /// </summary>
        [NibrsCode("35")]
        [CodeDescription("structures - other")]
        OTHER_STRUCTURE,

        /// <summary>
        /// tools
        /// </summary>
        [NibrsCode("36")]
        [CodeDescription("tools")]
        TOOLS,

        /// <summary>
        /// trucks
        /// </summary>
        [NibrsCode("37")]
        [CodeDescription("trucks")]
        TRUCKS,

        /// <summary>
        /// vehicle parts/ accessories
        /// </summary>
        [NibrsCode("38")]
        [CodeDescription("vehicle parts/ accessories")]
        VEHICLE_PARTS_ACCESSORIES,

        /// <summary>
        /// watercraft
        /// </summary>
        [NibrsCode("39")]
        [CodeDescription("watercraft")]
        WATERCRAFT,

        /// <summary>
        /// aircraft parts/ accessories
        /// </summary>
        [NibrsCode("41")]
        [CodeDescription("aircraft parts/ accessories")]
        AIRCRAFT_PARTS_ACCESSORIES,

        /// <summary>
        /// artistic supplies/ accessories
        /// </summary>
        [NibrsCode("42")]
        [CodeDescription("artistic supplies/ accessories")]
        ARTISTIC_SUPPLIES_ACCESSORIES,

        /// <summary>
        /// building materials
        /// </summary>
        [NibrsCode("43")]
        [CodeDescription("building materials")]
        BUILDING_MATERIALS,

        /// <summary>
        /// Camping/ Hunting/ Fishing Equipment/ Supplies
        /// </summary>
        [NibrsCode("44")]
        [CodeDescription("Camping/ Hunting/ Fishing Equipment/ Supplies")]
        CAMPING_HUNTING_FISHING_EQUIPMENT_SUPPLIES,

        /// <summary>
        /// Chemicals
        /// </summary>
        [NibrsCode("45")]
        [CodeDescription("Chemicals")]
        CHEMICALS,

        /// <summary>
        /// Collections/ Collectibles
        /// </summary>
        [NibrsCode("46")]
        [CodeDescription("Collections/ Collectibles")]
        COLLECTIONS_COLLECTIBLES,

        /// <summary>
        /// Crops
        /// </summary>
        [NibrsCode("47")]
        [CodeDescription("Crops")]
        CROPS,

        /// <summary>
        /// Documents/ Personal or Business
        /// </summary>
        [NibrsCode("48")]
        [CodeDescription("Documents/ Personal or Business")]
        PERSONAL_BUSINESS_DOCUMENTS,

        /// <summary>
        /// Explosives
        /// </summary>
        [NibrsCode("49")]
        [CodeDescription("Explosives")]
        EXPLOSIVES,

        /// <summary>
        /// Firearm Accessories
        /// </summary>
        [NibrsCode("59")]
        [CodeDescription("Firearm Accessories")]
        FIREARM_ACCESSORIES,

        /// <summary>
        /// Fuel
        /// </summary>
        [NibrsCode("64")]
        [CodeDescription("Fuel")]
        FUEL,

        /// <summary>
        /// Identity Documents
        /// </summary>
        [NibrsCode("65")]
        [CodeDescription("Identity Documents")]
        IDENTITY_DOCUMENTS,

        /// <summary>
        /// Identity - Intangible
        /// </summary>
        [NibrsCode("66")]
        [CodeDescription("Identity - Intangible")]
        IDENTITY,

        /// <summary>
        /// Law Enforcement Equipment
        /// </summary>
        [NibrsCode("67")]
        [CodeDescription("Law Enforcement Equipment")]
        LAW_ENFORCEMENT_EQUIPMENT,

        /// <summary>
        /// Lawn/ Yard/ Garden Equipment
        /// </summary>
        [NibrsCode("68")]
        [CodeDescription("Lawn/ Yard/ Garden Equipment")]
        LAWN_YARD_GARDEN_EQUIPMENT,

        /// <summary>
        /// Logging Equipment
        /// </summary>
        [NibrsCode("69")]
        [CodeDescription("Logging Equipment")]
        LOGGING_EQUIPMENT,

        /// <summary>
        /// Medical/ Medical Lab Equipment
        /// </summary>
        [NibrsCode("70")]
        [CodeDescription("Medical/ Medical Lab Equipment")]
        MEDICAL_OR_MEDICAL_LAB_EQUIPMENT,

        /// <summary>
        /// Metals, Non-Precious
        /// </summary>
        [NibrsCode("71")]
        [CodeDescription("Metals, Non-Precious")]
        METALS,

        /// <summary>
        /// Musical Instruments
        /// </summary>
        [NibrsCode("72")]
        [CodeDescription("Musical Instruments")]
        MUSICAL_INSTRUMENTS,

        /// <summary>
        /// Pets
        /// </summary>
        [NibrsCode("73")]
        [CodeDescription("Pets")]
        PETS,

        /// <summary>
        /// Photographic/ Optical Equipment
        /// </summary>
        [NibrsCode("74")]
        [CodeDescription("Photographic/ Optical Equipment")]
        PHOTOGRAPHIC_OPTICAL_EQUIPMENT,

        /// <summary>
        /// Portable Electronic Communications
        /// </summary>
        [NibrsCode("75")]
        [CodeDescription("Portable Electronic Communications")]
        PORTABLE_ELECTRONIC_COMMUNICATIONS,

        /// <summary>
        /// Recreational/ Sports Equipment
        /// </summary>
        [NibrsCode("76")]
        [CodeDescription("Recreational/ Sports Equipment")]
        RECREATIONAL_SPORTS_EQUIPMENT,

        /// <summary>
        /// Other
        /// </summary>
        [NibrsCode("77")]
        [CodeDescription("Other")]
        OTHER,

        /// <summary>
        /// Trailers
        /// </summary>
        [NibrsCode("78")]
        [CodeDescription("Trailers")]
        TRAILERS,

        /// <summary>
        /// Watercraft Equipment/Parts/Accessories
        /// </summary>
        [NibrsCode("79")]
        [CodeDescription("Watercraft Equipment/Parts/Accessories")]
        WATERCRAFT_EQUIPMENT_PARTS_ACCESSORIES,

        /// <summary>
        /// Weapons - Other
        /// </summary>
        [NibrsCode("80")]
        [CodeDescription("Weapons - Other")]
        OTHER_WEAPONS,

        /// <summary>
        /// Pending Inventory
        /// </summary>
        [NibrsCode("88")]
        [CodeDescription("Pending Inventory")]
        PENDING_INVENTORY,

        /// <summary>
        /// (blank) - this data value is not currently used by the FBI
        /// </summary>
        [NibrsCode("99")]
        [CodeDescription("(blank) - this data value is not currently used by the FBI")]
        BLANK
    }

    /// <summary>
    /// A data type for whether or not a person was a resident of a town, city, or community in relation to some activity
    /// </summary>
    [Description("A data type for whether or not a person was a resident of a town, city, or community in relation to some activity.")]
    enum ResidentCode
    {
        /// <summary>
        /// Nonresident
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("Nonresident")]
        NONRESIDENT,

        /// <summary>
        /// Resident
        /// </summary>
        [NibrsCode("R")]
        [CodeDescription("Resident")]
        RESIDENT,

        /// <summary>
        /// Unknown
        /// </summary>
        [NibrsCode("U")]
        [CodeDescription("Unknown")]
        UNKNOWN
    }

    /// <summary>
    /// A data type for kinds of victims in an incident
    /// </summary>
    [Description("A data type for kinds of victims in an incident.")]
    enum VictimCategoryCode
    {
        /// <summary>
        /// Business
        /// </summary>
        [NibrsCode("B")]
        [CodeDescription("Business")]
        BUSINESS,

        /// <summary>
        /// Financial Institution
        /// </summary>
        [NibrsCode("F")]
        [CodeDescription("Financial Institution")]
        FINANCIAL_INSTITUTION,

        /// <summary>
        /// Government
        /// </summary>
        [NibrsCode("G")]
        [CodeDescription("Government")]
        GOVERNMENT,

        /// <summary>
        /// Individual
        /// </summary>
        [NibrsCode("I")]
        [CodeDescription("Individual")]
        INDIVIDUAL,

        /// <summary>
        /// Law Enforcement Officer
        /// </summary>
        [NibrsCode("L")]
        [CodeDescription("Law Enforcement Officer")]
        LAW_ENFORCEMENT_OFFICER_LEO,

        /// <summary>
        /// Other
        /// </summary>
        [NibrsCode("O")]
        [CodeDescription("Other")]
        OTHER,

        /// <summary>
        /// Religious Organization
        /// </summary>
        [NibrsCode("R")]
        [CodeDescription("Religious Organization")]
        RELIGIOUS_ORGANIZATION,

        /// <summary>
        /// Society/ Public
        /// </summary>
        [NibrsCode("S")]
        [CodeDescription("Society/ Public")]
        SOCIETY_PUBLIC,

        /// <summary>
        /// Unknown
        /// </summary>
        [NibrsCode("U")]
        [CodeDescription("Unknown")]
        UNKNOWN
    }

    /// <summary>
    /// A data type for units of time
    /// </summary>
    [Description("A data type for units of time.")]
    enum TimeCode
    {
        /// <summary>
        /// year
        /// </summary>
        [NibrsCode("ANN")]
        [CodeDescription("year")]
        YEAR,

        /// <summary>
        /// kilosecond
        /// </summary>
        [NibrsCode("B52")]
        [CodeDescription("kilosecond")]
        KILOSECOND,

        /// <summary>
        /// microsecond
        /// </summary>
        [NibrsCode("B98")]
        [CodeDescription("microsecond")]
        MICROSECOND,

        /// <summary>
        /// millisecond
        /// </summary>
        [NibrsCode("C26")]
        [CodeDescription("millisecond")]
        MILLISECOND,

        /// <summary>
        /// nanosecond
        /// </summary>
        [NibrsCode("C47")]
        [CodeDescription("nanosecond")]
        NANOSECOND,

        /// <summary>
        /// tropical year
        /// </summary>
        [NibrsCode("D42")]
        [CodeDescription("tropical year")]
        TROPICAL_YEAR,

        /// <summary>
        /// day
        /// </summary>
        [NibrsCode("DAY")]
        [CodeDescription("day")]
        DAY,

        /// <summary>
        /// hour
        /// </summary>
        [NibrsCode("HUR")]
        [CodeDescription("hour")]
        HOUR,

        /// <summary>
        /// minute [unit of time]
        /// </summary>
        [NibrsCode("MIN")]
        [CodeDescription("minute [unit of time]")]
        MINUTE,

        /// <summary>
        /// month
        /// </summary>
        [NibrsCode("MON")]
        [CodeDescription("month")]
        MONTH,

        /// <summary>
        /// second [unit of time]
        /// </summary>
        [NibrsCode("SEC")]
        [CodeDescription("second [unit of time]")]
        SECOND,

        /// <summary>
        /// week
        /// </summary>
        [NibrsCode("WEE")]
        [CodeDescription("week")]
        WEEK
    }

    /// <summary>
    /// A data type for a code that identifies the status of property.  Expands the NIBRS PropertyLoss code list
    /// </summary>
    [Description("A data type for a code that identifies the status of property.  Expands the NIBRS PropertyLoss code list.")]
    enum ItemStatusCode
    {
        /// <summary>
        /// Bait
        /// </summary>
        [NibrsCode("BAIT")]
        [CodeDescription("Bait")]
        BAIT,

        /// <summary>
        /// Burned
        /// </summary>
        [NibrsCode("BURNED")]
        [CodeDescription("Burned")]
        BURNED,

        /// <summary>
        /// Cargo
        /// </summary>
        [NibrsCode("CARGO")]
        [CodeDescription("Cargo")]
        CARGO,

        /// <summary>
        /// Contraband
        /// </summary>
        [NibrsCode("CONTRABAND")]
        [CodeDescription("Contraband")]
        CONTRABAND,

        /// <summary>
        /// Counterfeited, includes forged
        /// </summary>
        [NibrsCode("COUNTERFEITED")]
        [CodeDescription("Counterfeited, includes forged")]
        COUNTERFEITED,

        /// <summary>
        /// Cultivated
        /// </summary>
        [NibrsCode("CULTIVATED")]
        [CodeDescription("Cultivated")]
        CULTIVATED,

        /// <summary>
        /// Damaged
        /// </summary>
        [NibrsCode("DAMAGED")]
        [CodeDescription("Damaged")]
        DAMAGED,

        /// <summary>
        /// Destroyed
        /// </summary>
        [NibrsCode("DESTROYED")]
        [CodeDescription("Destroyed")]
        DESTROYED,

        /// <summary>
        /// Destroyed_Damaged_Vandalized
        /// </summary>
        [NibrsCode("DESTROYED_DAMAGED_VANDALIZED")]
        [CodeDescription("Destroyed_Damaged_Vandalized")]
        DESTROYED_DAMAGED_VANDALIZED,

        /// <summary>
        /// Found
        /// </summary>
        [NibrsCode("FOUND")]
        [CodeDescription("Found")]
        FOUND,

        /// <summary>
        /// Lost
        /// </summary>
        [NibrsCode("LOST")]
        [CodeDescription("Lost")]
        LOST,

        /// <summary>
        /// Recovered (To impound property previously stolen)
        /// </summary>
        [NibrsCode("RECOVERED")]
        [CodeDescription("Recovered (To impound property previously stolen)")]
        RECOVERED,

        /// <summary>
        /// Returned
        /// </summary>
        [NibrsCode("RETURNED")]
        [CodeDescription("Returned")]
        RETURNED,

        /// <summary>
        /// Seized (To impound property not previously stolen)
        /// </summary>
        [NibrsCode("SEIZED")]
        [CodeDescription("Seized (To impound property not previously stolen)")]
        SEIZED,

        /// <summary>
        /// Stolen
        /// </summary>
        [NibrsCode("STOLEN")]
        [CodeDescription("Stolen")]
        STOLEN,

        /// <summary>
        /// Stolen_Bribed
        /// </summary>
        [NibrsCode("STOLEN_BRIBED")]
        [CodeDescription("Stolen_Bribed")]
        STOLEN_BRIBED,

        /// <summary>
        /// Stolen_Defrauded
        /// </summary>
        [NibrsCode("STOLEN_DEFRAUDED")]
        [CodeDescription("Stolen_Defrauded")]
        STOLEN_DEFRAUDED,

        /// <summary>
        /// Stolen_Embezzled
        /// </summary>
        [NibrsCode("STOLEN_EMBEZZLED")]
        [CodeDescription("Stolen_Embezzled")]
        STOLEN_EMBEZZLED,

        /// <summary>
        /// Stolen_Extorted
        /// </summary>
        [NibrsCode("STOLEN_EXTORTED")]
        [CodeDescription("Stolen_Extorted")]
        STOLEN_EXTORTED,

        /// <summary>
        /// Stolen_Ransomed
        /// </summary>
        [NibrsCode("STOLEN_RANSOMED")]
        [CodeDescription("Stolen_Ransomed")]
        STOLEN_RANSOMED,

        /// <summary>
        /// Stolen_Robbed
        /// </summary>
        [NibrsCode("STOLEN_ROBBED")]
        [CodeDescription("Stolen_Robbed")]
        STOLEN_ROBBED,

        /// <summary>
        /// Vandalized
        /// </summary>
        [NibrsCode("VANDALIZED")]
        [CodeDescription("Vandalized")]
        VANDALIZED,

        /// <summary>
        /// None
        /// </summary>
        [NibrsCode("NONE")]
        [CodeDescription("None")]
        NONE,

        /// <summary>
        /// Unknown
        /// </summary>
        [NibrsCode("UNKNOWN")]
        [CodeDescription("Unknown")]
        UNKNOWN
    }

    	/// <summary>
	 /// A data type for circumstances of either an aggravated assault or homicide.
	/// </summary>
	[Description("A data type for circumstances of either an aggravated assault or homicide.")]
	enum AggravatedAssaultHomicideFactorsCode
	{
		/// <summary>
		/// Argument
		/// </summary>
		[NibrsCode("01")]
		[CodeDescription("Argument")]
		ARGUMENT,

		/// <summary>
		/// Assault on Law Enforcement Officer(s)
		/// </summary>
		[NibrsCode("02")]
		[CodeDescription("Assault on Law Enforcement Officer(s)")]
		ASSAULT_ON_LAW_ENFORCEMENT_OFFIER,

		/// <summary>
		/// Drug Dealing
		/// </summary>
		[NibrsCode("03")]
		[CodeDescription("Drug Dealing")]
		DRUG_DEALING,

		/// <summary>
		/// Gangland (Organized Crime Involvement)
		/// </summary>
		[NibrsCode("04")]
		[CodeDescription("Gangland (Organized Crime Involvement)")]
		GANGLANG_ORGANIZED_CRIME,

		/// <summary>
		/// Juvenile Gang
		/// </summary>
		[NibrsCode("05")]
		[CodeDescription("Juvenile Gang")]
		JUVENILE_GANG,

		/// <summary>
		/// Lovers' Quarrel
		/// </summary>
		[NibrsCode("06")]
		[CodeDescription("Lovers' Quarrel")]
		LOVERS_QUARREL,

		/// <summary>
		/// Mercy Killing (Not applicable to Aggravated Assault)
		/// </summary>
		[NibrsCode("07")]
		[CodeDescription("Mercy Killing (Not applicable to Aggravated Assault)")]
		MERCY_KILLING,

		/// <summary>
		/// Other Felony Involved
		/// </summary>
		[NibrsCode("08")]
		[CodeDescription("Other Felony Involved")]
		OTHER_FELONY,

		/// <summary>
		/// Other Circumstances
		/// </summary>
		[NibrsCode("09")]
		[CodeDescription("Other Circumstances")]
		OTHER_CIRCUMSTANCES,

		/// <summary>
		/// Murder and Non-negligent Manslaughter
		/// </summary>
		[NibrsCode("09A")]
		[CodeDescription("Murder and Non-negligent Manslaughter")]
		MURDER_NONNEGLIGENT_MANSLAUGHTER,

		/// <summary>
		/// Negligent Manslaughter
		/// </summary>
		[NibrsCode("09B")]
		[CodeDescription("Negligent Manslaughter")]
		NEGLIGENT_MANSLAUGHTER,

		/// <summary>
		/// Justifiable Homicide
		/// </summary>
		[NibrsCode("09C")]
		[CodeDescription("Justifiable Homicide")]
		JUSTIFIABLE_HOMICIDE,

		/// <summary>
		/// Unknown Circumstances
		/// </summary>
		[NibrsCode("10")]
		[CodeDescription("Unknown Circumstances")]
		UNKNOWN_CIRCUMSTANCES,

		/// <summary>
		/// Aggravated Assault
		/// </summary>
		[NibrsCode("13A")]
		[CodeDescription("Aggravated Assault")]
		AGGRAVATED_ASSAULT,

		/// <summary>
		/// Criminal Killed by Private Citizen
		/// </summary>
		[NibrsCode("20")]
		[CodeDescription("Criminal Killed by Private Citizen")]
		CRIMINAL_KILLED_BY_PRIVATE_CITIZEN,

		/// <summary>
		/// Criminal Killed by Police Officer
		/// </summary>
		[NibrsCode("21")]
		[CodeDescription("Criminal Killed by Police Officer")]
		CRIMINAL_KILLED_BY_POLICE_OFFICER,

		/// <summary>
		/// Child Playing With Weapon
		/// </summary>
		[NibrsCode("30")]
		[CodeDescription("Child Playing With Weapon")]
		CHILD_PLAYING_WITH_WEAPON,

		/// <summary>
		/// Gun-Cleaning Accident
		/// </summary>
		[NibrsCode("31")]
		[CodeDescription("Gun-Cleaning Accident")]
		GUN_CLEANING_ACCIDENT,

		/// <summary>
		/// Hunting Accident
		/// </summary>
		[NibrsCode("32")]
		[CodeDescription("Hunting Accident")]
		HUNTING_ACCIDENT,

		/// <summary>
		/// Other Negligent Weapon Handling
		/// </summary>
		[NibrsCode("33")]
		[CodeDescription("Other Negligent Weapon Handling")]
		OTHER_NEGLIGENT_WEAPON_HANDLING,

		/// <summary>
		/// Other Negligent Killings
		/// </summary>
		[NibrsCode("34")]
		[CodeDescription("Other Negligent Killings")]
		OTHER_NEGLIGENT_KILLINGS
	}

	/// <summary>
	 /// A data type for a kind of action to be taken on the report
	/// </summary>
	[Description("A data type for a kind of action to be taken on the report")]
	enum ReportActionCategoryCode
	{
		/// <summary>
		/// Incident Report
		/// </summary>
		[NibrsCode("I")]
		[CodeDescription("Incident Report")]
		I,

		/// <summary>
		/// Delete
		/// </summary>
		[NibrsCode("D")]
		[CodeDescription("Delete")]
		D
	}

	/// <summary>
	 /// A data type for a code that identifies additional information on criminal activity of offenders in the offense.
	/// </summary>
    [Description("A data type for a code that identifies additional information on criminal activity of offenders in the offense.")]
    enum CriminalActivityCategoryCode
    {
        /// <summary>
        /// Simple/Gross Neglect
        /// </summary>
        [NibrsCode("A")]
        [CodeDescription("Simple/Gross Neglect")]
        A,

        /// <summary>
        /// Buying/Receiving
        /// </summary>
        [NibrsCode("B")]
        [CodeDescription("Buying/Receiving")]
        B,

        /// <summary>
        /// Cultivating/Manufacturing/Publishing (i.e., production of any type)
        /// </summary>
        [NibrsCode("C")]
        [CodeDescription("Cultivating/Manufacturing/Publishing (i.e., production of any type)")]
        C,

        /// <summary>
        /// Distributing/Selling
        /// </summary>
        [NibrsCode("D")]
        [CodeDescription("Distributing/Selling")]
        D,

        /// <summary>
        /// Exploiting Children
        /// </summary>
        [NibrsCode("E")]
        [CodeDescription("Exploiting Children")]
        E,

        /// <summary>
        /// Organized Abuse (Dog Fighting and Cock Fighting)
        /// </summary>
        [NibrsCode("F")]
        [CodeDescription("Organized Abuse (Dog Fighting and Cock Fighting)")]
        F,

        /// <summary>
        /// Other Gang
        /// </summary>
        [NibrsCode("G")]
        [CodeDescription("Other Gang")]
        G,

        /// <summary>
        /// Intentional Abuse and Torture (tormenting, mutilating, maiming,poisoning, or abandonment)
        /// </summary>
        [NibrsCode("I")]
        [CodeDescription("Intentional Abuse and Torture (tormenting, mutilating, maiming,poisoning, or abandonment)")]
        I,

        /// <summary>
        /// Juvenile Gang
        /// </summary>
        [NibrsCode("J")]
        [CodeDescription("Juvenile Gang")]
        J,

        /// <summary>
        /// None/Unknown
        /// </summary>
        [NibrsCode("N")]
        [CodeDescription("None/Unknown")]
        N,

        /// <summary>
        /// Operating/Promoting/Assisting
        /// </summary>
        [NibrsCode("O")]
        [CodeDescription("Operating/Promoting/Assisting")]
        O,

        /// <summary>
        /// Possessing/Concealing
        /// </summary>
        [NibrsCode("P")]
        [CodeDescription("Possessing/Concealing")]
        P,

        /// <summary>
        /// Animal Sexual Abuse (Bestiality)
        /// </summary>
        [NibrsCode("S")]
        [CodeDescription("Animal Sexual Abuse (Bestiality)")]
        S,

        /// <summary>
        /// Transporting/Transmitting/Importing
        /// </summary>
        [NibrsCode("T")]
        [CodeDescription("Transporting/Transmitting/Importing")]
        T,

        /// <summary>
        /// Using/Consuming
        /// </summary>
        [NibrsCode("U")]
        [CodeDescription("Using/Consuming")]
        U
    }

    /// <summary>
    /// A data type for kinds or functional descriptions of a location
    /// </summary>
    [Description("A data type for kinds or functional descriptions of a location.")]
    enum LocationCategoryCode
    {
        /// <summary>
        /// air/ bus/ train terminal
        /// </summary>
        [NibrsCode("01")]
        [CodeDescription("air/ bus/ train terminal")]
        AIR_BUS_TRAIN,

        /// <summary>
        /// bank/ savings and loan
        /// </summary>
        [NibrsCode("02")]
        [CodeDescription("bank/ savings and loan")]
        BANK,

        /// <summary>
        /// bar/ nightclub
        /// </summary>
        [NibrsCode("03")]
        [CodeDescription("bar/ nightclub")]
        BAR_NIGHTCLUB,

        /// <summary>
        /// church/ synagogue/ temple/ mosque
        /// </summary>
        [NibrsCode("04")]
        [CodeDescription("church/ synagogue/ temple/ mosque")]
        CHURCH_SYNAGOGUE_TEMPLE_MOSQUE,

        /// <summary>
        /// commercial/ office building
        /// </summary>
        [NibrsCode("05")]
        [CodeDescription("commercial/ office building")]
        COMMERCIAL_OFFICE_BUILDING,

        /// <summary>
        /// construction site
        /// </summary>
        [NibrsCode("06")]
        [CodeDescription("construction site")]
        CONSTRUCTION_SITE,

        /// <summary>
        /// convenience store
        /// </summary>
        [NibrsCode("07")]
        [CodeDescription("convenience store")]
        CONVENIENCE_STORE,

        /// <summary>
        /// department/ discount/ store
        /// </summary>
        [NibrsCode("08")]
        [CodeDescription("department/ discount/ store")]
        DEPARTMENT_DISCOUNT_STORE,

        /// <summary>
        /// drug store/ doctor's office/ hospital
        /// </summary>
        [NibrsCode("09")]
        [CodeDescription("drug store/ doctor's office/ hospital")]
        PHARMACY_DOCTOR_OFFICE_HOSPITAL,

        /// <summary>
        /// field/ woods
        /// </summary>
        [NibrsCode("10")]
        [CodeDescription("field/ woods")]
        FIELD_WOODS,

        /// <summary>
        /// government/ public building
        /// </summary>
        [NibrsCode("11")]
        [CodeDescription("government/ public building")]
        GOVERNMENT_PUBLIC_BUILDING,

        /// <summary>
        /// grocery/ supermarket
        /// </summary>
        [NibrsCode("12")]
        [CodeDescription("grocery/ supermarket")]
        GROCERY_SUPERMARKET,

        /// <summary>
        /// highway/ road/ alley/ street/ sidewalk
        /// </summary>
        [NibrsCode("13")]
        [CodeDescription("highway/ road/ alley/ street/ sidewalk")]
        HIGHWAY_ROAD_ALLEY_STREET_SIDEWALK,

        /// <summary>
        /// hotel/ motel/ etc
        /// </summary>
        [NibrsCode("14")]
        [CodeDescription("hotel/ motel/ etc.")]
        HOTEL_MOTEL_LODGING,

        /// <summary>
        /// jail/ prison/ penetentiary/ corrections facility
        /// </summary>
        [NibrsCode("15")]
        [CodeDescription("jail/ prison/ penetentiary/ corrections facility")]
        JAIL_PRISON_PENETENTIARY_CORRECTIONS_FACILITY,

        /// <summary>
        /// lake/ waterway/ beach
        /// </summary>
        [NibrsCode("16")]
        [CodeDescription("lake/ waterway/ beach")]
        LAKE_WATERWAY_BEACH,

        /// <summary>
        /// liquor store
        /// </summary>
        [NibrsCode("17")]
        [CodeDescription("liquor store")]
        LIQUOR_STORE,

        /// <summary>
        /// parking/ drop lot/ garage
        /// </summary>
        [NibrsCode("18")]
        [CodeDescription("parking/ drop lot/ garage")]
        PARKING_LOG_GARAGE,

        /// <summary>
        /// rental storage facility
        /// </summary>
        [NibrsCode("19")]
        [CodeDescription("rental storage facility")]
        RENTAL_STORAGE_FACILITY,

        /// <summary>
        /// residence/ home
        /// </summary>
        [NibrsCode("20")]
        [CodeDescription("residence/ home")]
        RESIDENCE_HOME,

        /// <summary>
        /// restaurant
        /// </summary>
        [NibrsCode("21")]
        [CodeDescription("restaurant")]
        RESTAURANT,

        /// <summary>
        /// school/ college
        /// </summary>
        [NibrsCode("22")]
        [CodeDescription("school/ college")]
        SCHOOL_COLLEGE,

        /// <summary>
        /// service/ gas station
        /// </summary>
        [NibrsCode("23")]
        [CodeDescription("service/ gas station")]
        SERVICE_GAS_STATION,

        /// <summary>
        /// specialty store
        /// </summary>
        [NibrsCode("24")]
        [CodeDescription("specialty store")]
        SPECIALTY_STORE,

        /// <summary>
        /// other/ unknown
        /// </summary>
        [NibrsCode("25")]
        [CodeDescription("other/ unknown")]
        OTHER_UNKNOWN,

        /// <summary>
        /// Abandoned/ Condemned Structure
        /// </summary>
        [NibrsCode("37")]
        [CodeDescription("Abandoned/ Condemned Structure")]
        ABANDONED_CONDEMNED_STRUCTURE,

        /// <summary>
        /// Amusement Park
        /// </summary>
        [NibrsCode("38")]
        [CodeDescription("Amusement Park")]
        AMUSEMENT_PARK,

        /// <summary>
        /// Arena/ Stadium/ Fairgrounds/Coliseum
        /// </summary>
        [NibrsCode("39")]
        [CodeDescription("Arena/ Stadium/ Fairgrounds/Coliseum")]
        ARENA_STADIUM_FAIRGROUNDS_COLISEUM,

        /// <summary>
        /// ATM Separate from Bank
        /// </summary>
        [NibrsCode("40")]
        [CodeDescription("ATM Separate from Bank")]
        ATM_SEPARATE_FROM_BANK,

        /// <summary>
        /// Auto Dealership New/Used
        /// </summary>
        [NibrsCode("41")]
        [CodeDescription("Auto Dealership New/Used")]
        AUTO_DEALERSHIP,

        /// <summary>
        /// Camp/ Campground
        /// </summary>
        [NibrsCode("42")]
        [CodeDescription("Camp/ Campground")]
        CAMPGROUND,

        /// <summary>
        /// Daycare Facility
        /// </summary>
        [NibrsCode("44")]
        [CodeDescription("Daycare Facility")]
        DAYCARE_FACILITY,

        /// <summary>
        /// Dock/ Wharf/ Freight/Modal Terminal
        /// </summary>
        [NibrsCode("45")]
        [CodeDescription("Dock/ Wharf/ Freight/Modal Terminal")]
        DOCK_WHARF_FREIGHT_TERMINAL,

        /// <summary>
        /// Farm Facility
        /// </summary>
        [NibrsCode("46")]
        [CodeDescription("Farm Facility")]
        FARM_FACILITY,

        /// <summary>
        /// Gambling Facility/ Casino/ Race Track
        /// </summary>
        [NibrsCode("47")]
        [CodeDescription("Gambling Facility/ Casino/ Race Track")]
        GAMBLING_FACILITY_CASINO_RACE_TRACK,

        /// <summary>
        /// Industrial Site
        /// </summary>
        [NibrsCode("48")]
        [CodeDescription("Industrial Site")]
        INDUSTRIAL_SITE,

        /// <summary>
        /// Military Installation
        /// </summary>
        [NibrsCode("49")]
        [CodeDescription("Military Installation")]
        MILITARY_INSTALLATION,

        /// <summary>
        /// Park/ Playground
        /// </summary>
        [NibrsCode("50")]
        [CodeDescription("Park/ Playground")]
        PARK_PLAYGROUND,

        /// <summary>
        /// Rest Area
        /// </summary>
        [NibrsCode("51")]
        [CodeDescription("Rest Area")]
        REST_AREA,

        /// <summary>
        /// School - College/ University
        /// </summary>
        [NibrsCode("52")]
        [CodeDescription("School - College/ University")]
        SCHOOL_COLLEGE_UNIVERSITY,

        /// <summary>
        /// School - Elementary/ Secondary
        /// </summary>
        [NibrsCode("53")]
        [CodeDescription("School - Elementary/ Secondary")]
        SCHOOL_ELEMENTARY_SECONDARY,

        /// <summary>
        /// Shelter - Mission/ Homeless
        /// </summary>
        [NibrsCode("54")]
        [CodeDescription("Shelter - Mission/ Homeless")]
        SHELTER,

        /// <summary>
        /// Shopping Mall
        /// </summary>
        [NibrsCode("55")]
        [CodeDescription("Shopping Mall")]
        SHOPPING_MALL,

        /// <summary>
        /// Tribal Lands
        /// </summary>
        [NibrsCode("56")]
        [CodeDescription("Tribal Lands")]
        TRIBAL_LANDS,

        /// <summary>
        /// Community Center
        /// </summary>
        [NibrsCode("57")]
        [CodeDescription("Community Center")]
        COMMUNITY_CENTER
    }

	/// <summary>
	 /// A data type for a kind of report contained in the NIBRS submission
	/// </summary>
	[Description("A data type for a kind of report contained in the NIBRS submission")]
	enum NIBRSReportCategoryCode
	{
		/// <summary>
		/// Group A Incident Report
		/// </summary>
		[NibrsCode("GROUP A INCIDENT REPORT")]
		[CodeDescription("Group A Incident Report")]
		A,

		/// <summary>
		/// Group A Incident Report - LEOKA
		/// </summary>
		[NibrsCode("GROUP A INCIDENT REPORT_LEOKA")]
		[CodeDescription("Group A Incident Report - LEOKA")]
		A_LEOKA,

		/// <summary>
		/// Group A Incident Report - Time Window
		/// </summary>
		[NibrsCode("GROUP A INCIDENT REPORT_TIME WINDOW")]
		[CodeDescription("Group A Incident Report - Time Window")]
		A_TIME_WINDOW,

		/// <summary>
		/// Group B Arrest Report
		/// </summary>
		[NibrsCode("GROUP B ARREST REPORT")]
		[CodeDescription("Group B Arrest Report")]
		B,

		/// <summary>
		/// Zero Report
		/// </summary>
		[NibrsCode("ZERO REPORT")]
		[CodeDescription("Zero Report")]
		ZERO
	}

	/// <summary>
	 /// A data type for the NIBRS code for an age of a person
	/// </summary>
	[Description("A data type for the NIBRS code for an age of a person")]
	enum PersonAgeCode
	{
		/// <summary>
		/// Under 24 Hours
		/// </summary>
		[NibrsCode("NN")]
		[CodeDescription("Under 24 Hours")]
		NEONATAL,

		/// <summary>
		/// 1-6 Days Old
		/// </summary>
		[NibrsCode("NB")]
		[CodeDescription("1-6 Days Old")]
		NEWBORN,

		/// <summary>
		/// 7-364 Days Old
		/// </summary>
		[NibrsCode("BB")]
		[CodeDescription("7-364 Days Old")]
		BABY,

		/// <summary>
		/// Unknown
		/// </summary>
		[NibrsCode("00")]
		[CodeDescription("Unknown")]
		UNKNOWN
	}

    /// <summary>
    /// A data type for Uniform Crime Reporting (UCR) offense codes
    /// </summary>
    [Description("A data type for Uniform Crime Reporting (UCR) offense codes.")]
    enum OffenseCode
    {
        /// <summary>
        /// Murder &amp; Nonnegligent Manslaughter
        /// </summary>
        [NibrsCode("09A")]
        [CodeDescription("Murder &amp; Nonnegligent Manslaughter")]
        MURDER_NONNEGLIGENT,

        /// <summary>
        /// Negligent Manslaughter
        /// </summary>
        [NibrsCode("09B")]
        [CodeDescription("Negligent Manslaughter")]
        NEGLIGENT_MANSLAUGHTER,

        /// <summary>
        /// Justifiable Homicide
        /// </summary>
        [NibrsCode("09C")]
        [CodeDescription("Justifiable Homicide")]
        JUSTIFIABLE_HOMICIDE,

        /// <summary>
        /// Kidnapping/Abduction
        /// </summary>
        [NibrsCode("100")]
        [CodeDescription("Kidnapping/Abduction")]
        KIDNAPPING_ABDUCTION,

        /// <summary>
        /// Rape
        /// </summary>
        [NibrsCode("11A")]
        [CodeDescription("Rape")]
        RAPE,

        /// <summary>
        /// Sodomy
        /// </summary>
        [NibrsCode("11B")]
        [CodeDescription("Sodomy")]
        SODOMY,

        /// <summary>
        /// Sexual Assault With An Object
        /// </summary>
        [NibrsCode("11C")]
        [CodeDescription("Sexual Assault With An Object")]
        SEXUAL_ASSAULT_WITH_OBJECT,

        /// <summary>
        /// Fondling
        /// </summary>
        [NibrsCode("11D")]
        [CodeDescription("Fondling")]
        FONDLING,

        /// <summary>
        /// Robbery
        /// </summary>
        [NibrsCode("120")]
        [CodeDescription("Robbery")]
        ROBBERY,

        /// <summary>
        /// Aggravated Assault
        /// </summary>
        [NibrsCode("13A")]
        [CodeDescription("Aggravated Assault")]
        AGGRAVATED_ASSAULT,

        /// <summary>
        /// Simple Assault
        /// </summary>
        [NibrsCode("13B")]
        [CodeDescription("Simple Assault")]
        SIMPLE_ASSAULT,

        /// <summary>
        /// Intimidation
        /// </summary>
        [NibrsCode("13C")]
        [CodeDescription("Intimidation")]
        INTIMIDATION,

        /// <summary>
        /// Arson
        /// </summary>
        [NibrsCode("200")]
        [CodeDescription("Arson")]
        ARSON,

        /// <summary>
        /// Extortion/Blackmail
        /// </summary>
        [NibrsCode("210")]
        [CodeDescription("Extortion/Blackmail")]
        EXTORTION_BLACKMAIL,

        /// <summary>
        /// Burglary/Breaking &amp; Entering
        /// </summary>
        [NibrsCode("220")]
        [CodeDescription("Burglary/Breaking &amp; Entering")]
        BURGLARY_BREAKING_AND_ENTERING,

        /// <summary>
        /// Pocket-picking
        /// </summary>
        [NibrsCode("23A")]
        [CodeDescription("Pocket-picking")]
        PICKPOCKETING,

        /// <summary>
        /// Purse-snatching
        /// </summary>
        [NibrsCode("23B")]
        [CodeDescription("Purse-snatching")]
        PURSE_SNATCHING,

        /// <summary>
        /// Shoplifting
        /// </summary>
        [NibrsCode("23C")]
        [CodeDescription("Shoplifting")]
        SHOPLIFTING,

        /// <summary>
        /// Theft From Building
        /// </summary>
        [NibrsCode("23D")]
        [CodeDescription("Theft From Building")]
        THEFT_FROM_BUILDING,

        /// <summary>
        /// Theft From Coin-Operated Machine or Device
        /// </summary>
        [NibrsCode("23E")]
        [CodeDescription("Theft From Coin-Operated Machine or Device")]
        THEFT_FROM_COIN_OPERATED_MACHINE,

        /// <summary>
        /// Theft From Motor Vehicle
        /// </summary>
        [NibrsCode("23F")]
        [CodeDescription("Theft From Motor Vehicle")]
        THEFT_FROM_MOTOR_VEHICLE,

        /// <summary>
        /// Theft of Motor Vehicle Parts or Accessories
        /// </summary>
        [NibrsCode("23G")]
        [CodeDescription("Theft of Motor Vehicle Parts or Accessories")]
        THEFT_OF_MOTOR_VEHICLE_PARTS_OR_ACCESSORIES,

        /// <summary>
        /// All Other Larceny
        /// </summary>
        [NibrsCode("23H")]
        [CodeDescription("All Other Larceny")]
        OTHER_LARCENY,

        /// <summary>
        /// Motor Vehicle Theft
        /// </summary>
        [NibrsCode("240")]
        [CodeDescription("Motor Vehicle Theft")]
        MOTOR_VEHICLE_THEFT,

        /// <summary>
        /// Counterfeiting/Forgery
        /// </summary>
        [NibrsCode("250")]
        [CodeDescription("Counterfeiting/Forgery")]
        COINTERFEITING_FORGERY,

        /// <summary>
        /// False Pretenses/Swindle/Confidence Game
        /// </summary>
        [NibrsCode("26A")]
        [CodeDescription("False Pretenses/Swindle/Confidence Game")]
        FALSE_PRETENSES_SWINDLE_CONFIDENCE_GAME,

        /// <summary>
        /// Credit Card/Automated Teller Machine Fraud
        /// </summary>
        [NibrsCode("26B")]
        [CodeDescription("Credit Card/Automated Teller Machine Fraud")]
        CREDIT_CARD_FRAUD,

        /// <summary>
        /// Impersonation
        /// </summary>
        [NibrsCode("26C")]
        [CodeDescription("Impersonation")]
        IMPERSONATION,

        /// <summary>
        /// Welfare Fraud
        /// </summary>
        [NibrsCode("26D")]
        [CodeDescription("Welfare Fraud")]
        WELFARE_FRAUD,

        /// <summary>
        /// Wire Fraud
        /// </summary>
        [NibrsCode("26E")]
        [CodeDescription("Wire Fraud")]
        WIRE_FRAUD,

        /// <summary>
        /// Identity Theft
        /// </summary>
        [NibrsCode("26F")]
        [CodeDescription("Identity Theft")]
        IDENTITY_THEFT,

        /// <summary>
        /// Hacking/Computer Invasion
        /// </summary>
        [NibrsCode("26G")]
        [CodeDescription("Hacking/Computer Invasion")]
        HACKING_COMPUTER_INVASION,

        /// <summary>
        /// Embezzlement
        /// </summary>
        [NibrsCode("270")]
        [CodeDescription("Embezzlement")]
        EMBEZZLEMENT,

        /// <summary>
        /// Stolen Offenses
        /// </summary>
        [NibrsCode("280")]
        [CodeDescription("Stolen Offenses")]
        STOLEN_OFFENSES,

        /// <summary>
        /// Destruction/Damage/Vandalism of Property
        /// </summary>
        [NibrsCode("290")]
        [CodeDescription("Destruction/Damage/Vandalism of Property")]
        DESTRUCTION_DAMAGE_VANDALISM_OR_PROPERTY,

        /// <summary>
        /// Drug/Narcotic Violations
        /// </summary>
        [NibrsCode("35A")]
        [CodeDescription("Drug/Narcotic Violations")]
        DRUGS_NARCOTICS,

        /// <summary>
        /// Drug Equipment Violations
        /// </summary>
        [NibrsCode("35B")]
        [CodeDescription("Drug Equipment Violations")]
        DRUG_EQUIPMENT,

        /// <summary>
        /// Incest
        /// </summary>
        [NibrsCode("36A")]
        [CodeDescription("Incest")]
        INCEST,

        /// <summary>
        /// Statutory Rape
        /// </summary>
        [NibrsCode("36B")]
        [CodeDescription("Statutory Rape")]
        STATUTORY_RAPE,

        /// <summary>
        /// Pornography/Obscene Material
        /// </summary>
        [NibrsCode("370")]
        [CodeDescription("Pornography/Obscene Material")]
        PORNOGRAPHY,

        /// <summary>
        /// Betting/Wagering
        /// </summary>
        [NibrsCode("39A")]
        [CodeDescription("Betting/Wagering")]
        BETTING_WAGERING,

        /// <summary>
        /// Operating/Promoting/Assisting Gambling
        /// </summary>
        [NibrsCode("39B")]
        [CodeDescription("Operating/Promoting/Assisting Gambling")]
        OPERATING_PROMOTING_ASSISTING_GAMBLING,

        /// <summary>
        /// Gambling Equipment Violation
        /// </summary>
        [NibrsCode("39C")]
        [CodeDescription("Gambling Equipment Violation")]
        GAMBLING_EQUIPMENT,

        /// <summary>
        /// Sports Tampering
        /// </summary>
        [NibrsCode("39D")]
        [CodeDescription("Sports Tampering")]
        SPORTS_TAMPERING,

        /// <summary>
        /// Prostitution
        /// </summary>
        [NibrsCode("40A")]
        [CodeDescription("Prostitution")]
        PROSTITUTION,

        /// <summary>
        /// Assisting or Promoting Prostitution
        /// </summary>
        [NibrsCode("40B")]
        [CodeDescription("Assisting or Promoting Prostitution")]
        ASSISTING_PROMOTING_PROSTITUTION,

        /// <summary>
        /// Purchasing Prostitution
        /// </summary>
        [NibrsCode("40C")]
        [CodeDescription("Purchasing Prostitution")]
        PURCHASING_PROSTITUTION,

        /// <summary>
        /// Bribery
        /// </summary>
        [NibrsCode("510")]
        [CodeDescription("Bribery")]
        BRIBERY,

        /// <summary>
        /// Weapon Law Violations
        /// </summary>
        [NibrsCode("520")]
        [CodeDescription("Weapon Law Violations")]
        WEAPON_LAW,

        /// <summary>
        /// Human Trafficking, Commercial Sex Acts
        /// </summary>
        [NibrsCode("64A")]
        [CodeDescription("Human Trafficking, Commercial Sex Acts")]
        HUMAN_TRAFFICKING_COMMERCIAL_SEX_ACTS,

        /// <summary>
        /// Human Trafficking, Involuntary Servitude
        /// </summary>
        [NibrsCode("64B")]
        [CodeDescription("Human Trafficking, Involuntary Servitude")]
        HUMAN_TRAFFICKING_INVOLUNTARY_SERVITUDE,

        /// <summary>
        /// Animal Cruelty
        /// </summary>
        [NibrsCode("720")]
        [CodeDescription("Animal Cruelty")]
        ANIMAL_CRUELTY,

        /// <summary>
        /// Bad Checks
        /// </summary>
        [NibrsCode("90A")]
        [CodeDescription("Bad Checks")]
        BAD_CHECKS,

        /// <summary>
        /// Curfew/Loitering/Vagrancy Violations
        /// </summary>
        [NibrsCode("90B")]
        [CodeDescription("Curfew/Loitering/Vagrancy Violations")]
        CURFEW_LOITERING_VAGRANCY,

        /// <summary>
        /// Disorderly Conduct
        /// </summary>
        [NibrsCode("90C")]
        [CodeDescription("Disorderly Conduct")]
        DISORDERLY_CONDUCT,

        /// <summary>
        /// Driving Under the Influence
        /// </summary>
        [NibrsCode("90D")]
        [CodeDescription("Driving Under the Influence")]
        DUI,

        /// <summary>
        /// Drunkenness
        /// </summary>
        [NibrsCode("90E")]
        [CodeDescription("Drunkenness")]
        DRUNKENNESS,

        /// <summary>
        /// Family Offenses, Nonviolent
        /// </summary>
        [NibrsCode("90F")]
        [CodeDescription("Family Offenses, Nonviolent")]
        FAMILY,

        /// <summary>
        /// Liquor Law Violations
        /// </summary>
        [NibrsCode("90G")]
        [CodeDescription("Liquor Law Violations")]
        LIQUOR_LAW,

        /// <summary>
        /// Peeping Tom
        /// </summary>
        [NibrsCode("90H")]
        [CodeDescription("Peeping Tom")]
        PEEPING_TOM,

        /// <summary>
        /// Runaway
        /// </summary>
        [NibrsCode("90I")]
        [CodeDescription("Runaway")]
        RUNAWAY,

        /// <summary>
        /// Trespass of Real
        /// </summary>
        [NibrsCode("90J")]
        [CodeDescription("Trespass of Real")]
        TRESPASS,

        /// <summary>
        /// All Other Offenses
        /// </summary>
        [NibrsCode("90Z")]
        [CodeDescription("All Other Offenses")]
        OTHER_OFFENSE
    }
}
