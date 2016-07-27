using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NIBRSXML.Utility;

/**
 * See the NIBRSCode and CodeDescription classes to see how to extract descriptions from the following enums
 */

namespace NIBRSXML.Constants
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
        [NIBRSCode("F")]
        [CodeDescription("Female")]
        FEMALE,

        /// <summary>
        /// Male
        /// </summary>
        [NIBRSCode("M")]
        [CodeDescription("Male")]
        MALE,

        /// <summary>
        /// Unknown - For Unidentified Only
        /// </summary>
        [NIBRSCode("U")]
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
        [NIBRSCode("ANTIAMERICAN INDIAN_ ALASKAN NATIVE")]
        [CodeDescription("Anti-American Indian or Alaskan Native_race ethnicity bias")]
        ANTIAMERICAN_INDIAN_ALASKAN_NATIVE,

        /// <summary>
        /// Anti-Arab_race ethnicity bias
        /// </summary>
        [NIBRSCode("ANTIARAB")]
        [CodeDescription("Anti-Arab_race ethnicity bias")]
        ANTIARAB,

        /// <summary>
        /// Anti-Asian_race ethnicity bias
        /// </summary>
        [NIBRSCode("ANTIASIAN")]
        [CodeDescription("Anti-Asian_race ethnicity bias")]
        ANTIASIAN,

        /// <summary>
        /// Anti-Atheist or Agnostic_religious bias
        /// </summary>
        [NIBRSCode("ANTIATHEIST_AGNOSTIC")]
        [CodeDescription("Anti-Atheist or Agnostic_religious bias")]
        ANTIATHEIST_AGNOSTIC,

        /// <summary>
        /// Anti-Bisexual
        /// </summary>
        [NIBRSCode("ANTIBISEXUAL")]
        [CodeDescription("Anti-Bisexual")]
        ANTIBISEXUAL,

        /// <summary>
        /// Anti-Black or African American_race ethnicity bias
        /// </summary>
        [NIBRSCode("ANTIBLACK_AFRICAN AMERICAN")]
        [CodeDescription("Anti-Black or African American_race ethnicity bias")]
        ANTIBLACK_AFRICAN_AMERICAN,

        /// <summary>
        /// Anti-Buddhist_religious bias
        /// </summary>
        [NIBRSCode("ANTIBUDDHIST")]
        [CodeDescription("Anti-Buddhist_religious bias")]
        ANTIBUDDHIST,

        /// <summary>
        /// Anti-Catholic religion_religious bias
        /// </summary>
        [NIBRSCode("ANTICATHOLIC")]
        [CodeDescription("Anti-Catholic religion_religious bias")]
        ANTICATHOLIC,

        /// <summary>
        /// AntiDisbled_disability bias
        /// </summary>
        [NIBRSCode("ANTIDISABLED")]
        [CodeDescription("AntiDisbled_disability bias")]
        ANTIDISABLED,

        /// <summary>
        /// Anti-Eastern Orthodox (Russian, Greek, Other)_religious bias
        /// </summary>
        [NIBRSCode("ANTIEASTERNORTHODOX")]
        [CodeDescription("Anti-Eastern Orthodox (Russian, Greek, Other)_religious bias")]
        ANTIEASTERNORTHODOX,

        /// <summary>
        /// Anti-Female_gender bias
        /// </summary>
        [NIBRSCode("ANTIFEMALE")]
        [CodeDescription("Anti-Female_gender bias")]
        ANTIFEMALE,

        /// <summary>
        /// Anti-Female Homosexual (Lesbian) _sexual orientation bias
        /// </summary>
        [NIBRSCode("ANTIFEMALE HOMOSEXUAL")]
        [CodeDescription("Anti-Female Homosexual (Lesbian) _sexual orientation bias")]
        ANTIFEMALE_HOMOSEXUAL,

        /// <summary>
        /// Anti-Gender Non-Conforming
        /// </summary>
        [NIBRSCode("ANTIGENDER_NONCONFORMING")]
        [CodeDescription("Anti-Gender Non-Conforming")]
        ANTIGENDER_NONCONFORMING,

        /// <summary>
        /// Anti-Heterosexual _sexual orientation bias
        /// </summary>
        [NIBRSCode("ANTIHETEROSEXUAL")]
        [CodeDescription("Anti-Heterosexual _sexual orientation bias")]
        ANTIHETEROSEXUAL,

        /// <summary>
        /// Anti-Hindu_religious bias
        /// </summary>
        [NIBRSCode("ANTIHINDU")]
        [CodeDescription("Anti-Hindu_religious bias")]
        ANTIHINDU,

        /// <summary>
        /// Anti-Hispanic or Latino_race ethnicity bias
        /// </summary>
        [NIBRSCode("ANTIHISPANIC_LATINO")]
        [CodeDescription("Anti-Hispanic or Latino_race ethnicity bias")]
        ANTIHISPANIC_LATINO,

        /// <summary>
        /// Anti-Homosexual, e.g, Lesbian, Gay, Bisexual, and transgender (mixed group), _sexual orientation bias
        /// </summary>
        [NIBRSCode("ANTIHOMOSEXUAL")]
        [CodeDescription("Anti-Homosexual, e.g., Lesbian, Gay, Bisexual, and transgender (mixed group), _sexual orientation bias")]
        ANTIHOMOSEXUAL,

        /// <summary>
        /// Anti-Islamic, includes muslim_religious bias
        /// </summary>
        [NIBRSCode("ANTIISLAMIC")]
        [CodeDescription("Anti-Islamic, includes muslim_religious bias")]
        ANTIISLAMIC,

        /// <summary>
        /// Anti-Jehovah's Witness_religious bias
        /// </summary>
        [NIBRSCode("ANTIJEHOVAHWITNESS")]
        [CodeDescription("Anti-Jehovah's Witness_religious bias")]
        ANTIJEHOVAHWITNESS,

        /// <summary>
        /// Anti-Jewish_religious bias
        /// </summary>
        [NIBRSCode("ANTIJEWISH")]
        [CodeDescription("Anti-Jewish_religious bias")]
        ANTIJEWISH,

        /// <summary>
        /// Anti-Male_gender bias
        /// </summary>
        [NIBRSCode("ANTIMALE")]
        [CodeDescription("Anti-Male_gender bias")]
        ANTIMALE,

        /// <summary>
        /// Anti-Male Homosexual (Gay) _sexual orientation bias
        /// </summary>
        [NIBRSCode("ANTIMALE HOMOSEXUAL")]
        [CodeDescription("Anti-Male Homosexual (Gay) _sexual orientation bias")]
        ANTIMALE_HOMOSEXUAL,

        /// <summary>
        /// Anti-Mental Disability_disability bias
        /// </summary>
        [NIBRSCode("ANTIMENTAL DISABILITY")]
        [CodeDescription("Anti-Mental Disability_disability bias")]
        ANTIMENTAL_DISABILITY,

        /// <summary>
        /// Anti-Mormon_religious bias
        /// </summary>
        [NIBRSCode("ANTIMORMON")]
        [CodeDescription("Anti-Mormon_religious bias")]
        ANTIMORMON,

        /// <summary>
        /// Anti-Multi-Racial Group, e.g., Arab and Asian and American Indian and White and etc_race ethnicity bias
        /// </summary>
        [NIBRSCode("ANTIMULTIRACIAL GROUP")]
        [CodeDescription("Anti-Multi-Racial Group, e.g., Arab and Asian and American Indian and White and etc._race ethnicity bias")]
        ANTIMULTIRACIAL_GROUP,

        /// <summary>
        /// Anti-Multi-Religious Group, e.g., Catholic and Mormon and Islamic and etc_religious bias
        /// </summary>
        [NIBRSCode("ANTIMULTIRELIGIOUS GROUP")]
        [CodeDescription("Anti-Multi-Religious Group, e.g., Catholic and Mormon and Islamic and etc._religious bias")]
        ANTIMULTIRELIGIOUS_GROUP,

        /// <summary>
        /// Anti-Native Hawaiian or Other Pacific Islander_race ethnicity bias
        /// </summary>
        [NIBRSCode("ANTINATIVEHAWAIIAN_OTHERPACIFICISLANDER")]
        [CodeDescription("Anti-Native Hawaiian or Other Pacific Islander_race ethnicity bias")]
        ANTINATIVEHAWAIIAN_OTHERPACIFICISLANDER,

        /// <summary>
        /// Anti-Other Christian_religious bias
        /// </summary>
        [NIBRSCode("ANTIOTHER CHRISTIAN")]
        [CodeDescription("Anti-Other Christian_religious bias")]
        ANTIOTHER_CHRISTIAN,

        /// <summary>
        /// Anti-Other Race, Ethnicity, Ancestry, or National Origin_race ethnicity bias
        /// </summary>
        [NIBRSCode("ANTIOTHER ETHNICITY_NATIONAL ORIGIN")]
        [CodeDescription("Anti-Other Race, Ethnicity, Ancestry, or National Origin_race ethnicity bias")]
        ANTIOTHER_ETHNICITY_NATIONAL_ORIGIN,

        /// <summary>
        /// Anti-Other Religion_religious bias
        /// </summary>
        [NIBRSCode("ANTIOTHER RELIGION")]
        [CodeDescription("Anti-Other Religion_religious bias")]
        ANTIOTHER_RELIGION,

        /// <summary>
        /// Anti-Physical Disability_disability bias
        /// </summary>
        [NIBRSCode("ANTIPHYSICAL DISABILITY")]
        [CodeDescription("Anti-Physical Disability_disability bias")]
        ANTIPHYSICAL_DISABILITY,

        /// <summary>
        /// Anti-Protestant_religious bias
        /// </summary>
        [NIBRSCode("ANTIPROTESTANT")]
        [CodeDescription("Anti-Protestant_religious bias")]
        ANTIPROTESTANT,

        /// <summary>
        /// Anti-Sikh_religious bias
        /// </summary>
        [NIBRSCode("ANTISIKH")]
        [CodeDescription("Anti-Sikh_religious bias")]
        ANTISIKH,

        /// <summary>
        /// Anti-Transgender_gender identity
        /// </summary>
        [NIBRSCode("ANTITRANSGENDER")]
        [CodeDescription("Anti-Transgender_gender identity")]
        ANTITRANSGENDER,

        /// <summary>
        /// Anti-White_race ethnicity bias
        /// </summary>
        [NIBRSCode("ANTIWHITE")]
        [CodeDescription("Anti-White_race ethnicity bias")]
        ANTIWHITE,

        /// <summary>
        /// Gender Bias_gender bias
        /// </summary>
        [NIBRSCode("GENDER BIAS")]
        [CodeDescription("Gender Bias_gender bias")]
        GENDER_BIAS,

        /// <summary>
        /// None (no bias)
        /// </summary>
        [NIBRSCode("NONE")]
        [CodeDescription("None (no bias)")]
        NONE,

        /// <summary>
        /// Political Affiliation Bias
        /// </summary>
        [NIBRSCode("POLITICAL AFFILIATION BIAS")]
        [CodeDescription("Political Affiliation Bias")]
        POLITICAL_AFFILIATION_BIAS,

        /// <summary>
        /// Unknown (offender's motivation not known)
        /// </summary>
        [NIBRSCode("UNKNOWN")]
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
        [NIBRSCode("G")]
        [CodeDescription("Other Gang")]
        OTHER,

        /// <summary>
        /// Juvenile Gang
        /// </summary>
        [NIBRSCode("J")]
        [CodeDescription("Juvenile Gang")]
        JUVENILE_GANG,

        /// <summary>
        /// None/Unknown
        /// </summary>
        [NIBRSCode("N")]
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
        [NIBRSCode("ACTIVE")]
        [CodeDescription("Active")]
        ACTIVE,

        /// <summary>
        /// Administratively Closed
        /// </summary>
        [NIBRSCode("ADMINISTRATIVELY CLOSED")]
        [CodeDescription("Administratively Closed")]
        ADMINISTRATIVELY_CLOSED,

        /// <summary>
        /// Cleared by Arrest
        /// </summary>
        [NIBRSCode("CLEARED BY ARREST")]
        [CodeDescription("Cleared by Arrest")]
        CLEARED_BY_ARREST,

        /// <summary>
        /// Cleared by Exceptional Means
        /// </summary>
        [NIBRSCode("CLEARED BY EXCEPTIONAL MEANS")]
        [CodeDescription("Cleared by Exceptional Means")]
        CLEAREDBY_EXCEPTIONAL_MEANS,

        /// <summary>
        /// Closed
        /// </summary>
        [NIBRSCode("CLOSED")]
        [CodeDescription("Closed")]
        CLOSED,

        /// <summary>
        /// Cold
        /// </summary>
        [NIBRSCode("COLD")]
        [CodeDescription("Cold")]
        COLD,

        /// <summary>
        /// Inactive
        /// </summary>
        [NIBRSCode("INACTIVE")]
        [CodeDescription("Inactive")]
        INACTIVE,

        /// <summary>
        /// Open
        /// </summary>
        [NIBRSCode("OPEN")]
        [CodeDescription("Open")]
        OPEN,

        /// <summary>
        /// Pending
        /// </summary>
        [NIBRSCode("PENDING")]
        [CodeDescription("Pending")]
        PENDING,

        /// <summary>
        /// Re_opened
        /// </summary>
        [NIBRSCode("RE_OPENED")]
        [CodeDescription("Re_opened")]
        RE_OPENED,

        /// <summary>
        /// Unfounded
        /// </summary>
        [NIBRSCode("UNFOUNDED")]
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
        [NIBRSCode("C")]
        [CodeDescription("Referred to Criminal (Adult) Court")]
        CRIMINAL_COURT,

        /// <summary>
        /// Handled Within Department 
        /// </summary>
        [NIBRSCode("H")]
        [CodeDescription("Handled Within Department ")]
        HANDLED_WITHIN_DEPARTMENT,

        /// <summary>
        /// Referred to Juvenile Court
        /// </summary>
        [NIBRSCode("J")]
        [CodeDescription("Referred to Juvenile Court")]
        JUVENILE_COURT,

        /// <summary>
        /// Referred to Other Authorities
        /// </summary>
        [NIBRSCode("R")]
        [CodeDescription("Referred to Other Authorities")]
        OTHER_AUTHORITIES,

        /// <summary>
        /// Referred to Welfare Agency
        /// </summary>
        [NIBRSCode("w")]
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
        [NIBRSCode("A")]
        [CodeDescription("ASIAN:  A person having origins in any of the original peoples of the Far East, Southeast Asia, or the Indian subcontinent including, for example, Cambodia, China, India, Japan, Korea, Malaysia, Pakistan, the Philippine Islands, Thailand, and Vietnam.")]
        ASIAN,

        /// <summary>
        /// BLACK:  A person having origins in any of the black racial groups of Africa
        /// </summary>
        [NIBRSCode("B")]
        [CodeDescription("BLACK:  A person having origins in any of the black racial groups of Africa.")]
        BLACK,

        /// <summary>
        /// AMERICAN INDIAN or ALASKAN NATIVE:  A person having origins in any of the original peoples of the Americas and maintaining cultural identification through tribal affiliations or community recognition
        /// </summary>
        [NIBRSCode("I")]
        [CodeDescription("AMERICAN INDIAN or ALASKAN NATIVE:  A person having origins in any of the original peoples of the Americas and maintaining cultural identification through tribal affiliations or community recognition.")]
        AMERICAN_INDIAN_OR_ALASKAN_NATIVE,

        [NIBRSCode("P")]
        [CodeDescription(@"NATIVE HAWAIIAN or OTHER PACIFIC ISLANDER:  A person having origins in any of the original peoples of Hawaii, Guam, Samoa, or other Pacific Islands.  The term ""Native Hawaiian"" does not include individuals who are native to the State of Hawaii by virtue of being born there.  However, the following Pacific Islander groups are included:  Carolinian, Fijian, Kosraean, Melanesian, Micronesian, Northern Mariana Islander, Palauan, Papua New Guinean, Ponapean (Pohnpelan), Polynesian, Solomon Islander, Tahitian, Tarawa Islander, Tokelauan, Tongan, Trukese (Chuukese), and Yapese.")]
        HAWAIIAN_OR_PACIFIC_ISLANDER,

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [NIBRSCode("U")]
        [CodeDescription("UNKNOWN")]
        UNKNOWN,

        /// <summary>
        /// WHITE:  A person having origins in any of the original peoples of Europe, North Africa, or Middle East
        /// </summary>
        [NIBRSCode("W")]
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
        [NIBRSCode("Accomplice")]
        [CodeDescription("Accomplice")]
        Accomplice,

        /// <summary>
        /// Acquaintance
        /// </summary>
        [NIBRSCode("Acquaintance")]
        [CodeDescription("Acquaintance")]
        Acquaintance,

        /// <summary>
        /// Authority Figure
        /// </summary>
        [NIBRSCode("Authority Figure")]
        [CodeDescription("Authority Figure")]
        Authority_Figure,

        /// <summary>
        /// Babysittee
        /// </summary>
        [NIBRSCode("Babysittee")]
        [CodeDescription("Babysittee")]
        Babysittee,

        /// <summary>
        /// Babysitter
        /// </summary>
        [NIBRSCode("Babysitter")]
        [CodeDescription("Babysitter")]
        Babysitter,

        /// <summary>
        /// Boyfriend
        /// </summary>
        [NIBRSCode("Boyfriend")]
        [CodeDescription("Boyfriend")]
        Boyfriend,

        /// <summary>
        /// Caregiver
        /// </summary>
        [NIBRSCode("Caregiver")]
        [CodeDescription("Caregiver")]
        Caregiver,

        /// <summary>
        /// Child of Boyfriend_Girlfriend
        /// </summary>
        [NIBRSCode("Child of Boyfriend_Girlfriend")]
        [CodeDescription("Child of Boyfriend_Girlfriend")]
        Child_of_Boyfriend_Girlfriend,

        /// <summary>
        /// Client
        /// </summary>
        [NIBRSCode("Client")]
        [CodeDescription("Client")]
        Client,

        /// <summary>
        /// Cohabitant
        /// </summary>
        [NIBRSCode("Cohabitant")]
        [CodeDescription("Cohabitant")]
        Cohabitant,

        /// <summary>
        /// Delivery Person
        /// </summary>
        [NIBRSCode("Delivery Person")]
        [CodeDescription("Delivery Person")]
        Delivery_Person,

        /// <summary>
        /// Employee
        /// </summary>
        [NIBRSCode("Employee")]
        [CodeDescription("Employee")]
        Employee,

        /// <summary>
        /// Employer
        /// </summary>
        [NIBRSCode("Employer")]
        [CodeDescription("Employer")]
        Employer,

        /// <summary>
        /// Ex_Spouse
        /// </summary>
        [NIBRSCode("Ex_Spouse")]
        [CodeDescription("Ex_Spouse")]
        Ex_Spouse,

        /// <summary>
        /// Family Member
        /// </summary>
        [NIBRSCode("Family Member")]
        [CodeDescription("Family Member")]
        Family_Member,

        /// <summary>
        /// Aunt
        /// </summary>
        [NIBRSCode("Family Member_Aunt")]
        [CodeDescription("Aunt")]
        Family_Member_Aunt,

        /// <summary>
        /// Child
        /// </summary>
        [NIBRSCode("Family Member_Child")]
        [CodeDescription("Child")]
        Family_Member_Child,

        /// <summary>
        /// Cousin
        /// </summary>
        [NIBRSCode("Family Member_Cousin")]
        [CodeDescription("Cousin")]
        Family_Member_Cousin,

        /// <summary>
        /// Family Member_Foster Child
        /// </summary>
        [NIBRSCode("Family Member_Foster Child")]
        [CodeDescription("Family Member_Foster Child")]
        Family_Member_Foster_Child,

        /// <summary>
        /// Family Member_Foster Parent
        /// </summary>
        [NIBRSCode("Family Member_Foster Parent")]
        [CodeDescription("Family Member_Foster Parent")]
        Family_Member_Foster_Parent,

        /// <summary>
        /// Grandchild
        /// </summary>
        [NIBRSCode("Family Member_Grandchild")]
        [CodeDescription("Grandchild")]
        Family_Member_Grandchild,

        /// <summary>
        /// Grandparent
        /// </summary>
        [NIBRSCode("Family Member_Grandparent")]
        [CodeDescription("Grandparent")]
        Family_Member_Grandparent,

        /// <summary>
        /// In-Law
        /// </summary>
        [NIBRSCode("Family Member_In-Law")]
        [CodeDescription("In-Law")]
        Family_Member_In_Law,

        /// <summary>
        /// Nephew
        /// </summary>
        [NIBRSCode("Family Member_Nephew")]
        [CodeDescription("Nephew")]
        Family_Member_Nephew,

        /// <summary>
        /// Niece
        /// </summary>
        [NIBRSCode("Family Member_Niece")]
        [CodeDescription("Niece")]
        Family_Member_Niece,

        /// <summary>
        /// Parent
        /// </summary>
        [NIBRSCode("Family Member_Parent")]
        [CodeDescription("Parent")]
        Family_Member_Parent,

        /// <summary>
        /// Sibling
        /// </summary>
        [NIBRSCode("Family Member_Sibling")]
        [CodeDescription("Sibling")]
        Family_Member_Sibling,

        /// <summary>
        /// Family Member_Spouse 
        /// </summary>
        [NIBRSCode("Family Member_Spouse")]
        [CodeDescription("Family Member_Spouse ")]
        Family_Member_Spouse,

        /// <summary>
        /// Spouse_Common Law
        /// </summary>
        [NIBRSCode("Family Member_Spouse_Common Law")]
        [CodeDescription("Spouse_Common Law")]
        Family_Member_Spouse_Common_Law,

        /// <summary>
        /// Stepchild
        /// </summary>
        [NIBRSCode("Family Member_Stepchild")]
        [CodeDescription("Stepchild")]
        Family_Member_Stepchild,

        /// <summary>
        /// Stepparent
        /// </summary>
        [NIBRSCode("Family Member_Stepparent")]
        [CodeDescription("Stepparent")]
        Family_Member_Stepparent,

        /// <summary>
        /// Stepsibling
        /// </summary>
        [NIBRSCode("Family Member_Stepsibling")]
        [CodeDescription("Stepsibling")]
        Family_Member_Stepsibling,

        /// <summary>
        /// Uncle
        /// </summary>
        [NIBRSCode("Family Member_Uncle")]
        [CodeDescription("Uncle")]
        Family_Member_Uncle,

        /// <summary>
        /// Former Employee
        /// </summary>
        [NIBRSCode("Former Employee")]
        [CodeDescription("Former Employee")]
        Former_Employee,

        /// <summary>
        /// Former Employer
        /// </summary>
        [NIBRSCode("Former Employer")]
        [CodeDescription("Former Employer")]
        Former_Employer,

        /// <summary>
        /// Friend
        /// </summary>
        [NIBRSCode("Friend")]
        [CodeDescription("Friend")]
        Friend,

        /// <summary>
        /// Girlfriend
        /// </summary>
        [NIBRSCode("Girlfriend")]
        [CodeDescription("Girlfriend")]
        Girlfriend,

        /// <summary>
        /// Guardian
        /// </summary>
        [NIBRSCode("Guardian")]
        [CodeDescription("Guardian")]
        Guardian,

        /// <summary>
        /// Homosexual relationship
        /// </summary>
        [NIBRSCode("Homosexual relationship")]
        [CodeDescription("Homosexual relationship")]
        Homosexual_relationship,

        /// <summary>
        /// Neighbor
        /// </summary>
        [NIBRSCode("Neighbor")]
        [CodeDescription("Neighbor")]
        Neighbor,

        /// <summary>
        /// NonFamily_Otherwise Known
        /// </summary>
        [NIBRSCode("NonFamily_Otherwise Known")]
        [CodeDescription("NonFamily_Otherwise Known")]
        NonFamily_Otherwise_Known,

        /// <summary>
        /// Patient
        /// </summary>
        [NIBRSCode("Patient")]
        [CodeDescription("Patient")]
        Patient,

        /// <summary>
        /// Relationship Unknown 
        /// </summary>
        [NIBRSCode("Relationship Unknown")]
        [CodeDescription("Relationship Unknown ")]
        Relationship_Unknown,

        /// <summary>
        /// Stranger
        /// </summary>
        [NIBRSCode("Stranger")]
        [CodeDescription("Stranger")]
        Stranger,

        /// <summary>
        /// Student
        /// </summary>
        [NIBRSCode("Student")]
        [CodeDescription("Student")]
        Student,

        /// <summary>
        /// Teacher
        /// </summary>
        [NIBRSCode("Teacher")]
        [CodeDescription("Teacher")]
        Teacher,

        /// <summary>
        /// Victim Was Offender
        /// </summary>
        [NIBRSCode("Victim Was Offender")]
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
        [NIBRSCode("O")]
        [CodeDescription("On-View Arrest")]
        ON_VIEW_ARREST,

        /// <summary>
        /// Summoned/ Cited
        /// </summary>
        [NIBRSCode("S")]
        [CodeDescription("Summoned/ Cited")]
        SUMMONED_CITED,

        /// <summary>
        /// Taken Into Custody
        /// </summary>
        [NIBRSCode("T")]
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
        [NIBRSCode("01")]
        [CodeDescription("Unarmed")]
        UNARMED,

        /// <summary>
        /// Firearm (type not stated)
        /// </summary>
        [NIBRSCode("11")]
        [CodeDescription("Firearm (type not stated)")]
        FIREARM,

        /// <summary>
        /// Firearm (type not stated) - Automatic
        /// </summary>
        [NIBRSCode("11A")]
        [CodeDescription("Firearm (type not stated) - Automatic")]
        AUTOMATIC_FIREARM,

        /// <summary>
        /// Handgun
        /// </summary>
        [NIBRSCode("12")]
        [CodeDescription("Handgun")]
        HANDGUN,

        /// <summary>
        /// Handgun - Automatic
        /// </summary>
        [NIBRSCode("12A")]
        [CodeDescription("Handgun - Automatic")]
        AUTOMATIC_HANDGUN,

        /// <summary>
        /// Rifle
        /// </summary>
        [NIBRSCode("13")]
        [CodeDescription("Rifle")]
        RIFLE,

        /// <summary>
        /// Rifle - Automatic
        /// </summary>
        [NIBRSCode("13A")]
        [CodeDescription("Rifle - Automatic")]
        AUTOMATIC_RIFLE,

        /// <summary>
        /// Shotgun
        /// </summary>
        [NIBRSCode("14")]
        [CodeDescription("Shotgun")]
        SHOTGUN,

        /// <summary>
        /// Shotgun - Automatic
        /// </summary>
        [NIBRSCode("14A")]
        [CodeDescription("Shotgun - Automatic")]
        AUTOMATIC_SHOTGUN,

        /// <summary>
        /// Other Firearm
        /// </summary>
        [NIBRSCode("15")]
        [CodeDescription("Other Firearm")]
        OTHER_FIREARM,

        /// <summary>
        /// Other Firearm - Automatic
        /// </summary>
        [NIBRSCode("15A")]
        [CodeDescription("Other Firearm - Automatic")]
        OTHER_AUTOMATIC_FIREARM,

        /// <summary>
        /// Lethal Cutting Instrument
        /// </summary>
        [NIBRSCode("16")]
        [CodeDescription("Lethal Cutting Instrument")]
        LETHAL_CUTTING_INSTRUMENT,

        /// <summary>
        /// Club/ Blackjack/ Brass Knuckles
        /// </summary>
        [NIBRSCode("17")]
        [CodeDescription("Club/ Blackjack/ Brass Knuckles")]
        CLUB_BLACKJACK_BRASS_KNUCKLES
    }

    /// <summary>
    /// A data type for kinds of incidents
    /// </summary>
    [Description("A data type for kinds of incidents.")]
    enum CriminalActivityCategoryCode
    {
        /// <summary>
        /// buying/ receiving
        /// </summary>
        [NIBRSCode("B")]
        [CodeDescription("buying/ receiving")]
        BUYING_RECEIVING,

        /// <summary>
        /// cultivate/ manufacture/ publish (i.e,production of any type)
        /// </summary>
        [NIBRSCode("C")]
        [CodeDescription("cultivate/ manufacture/ publish (i.e.,production of any type)")]
        CULTIVATE_MANUFACTURE_PUBLISH,

        /// <summary>
        /// distribute/ selling
        /// </summary>
        [NIBRSCode("D")]
        [CodeDescription("distribute/ selling")]
        DISTRIBUTE_SELLING,

        /// <summary>
        /// exploiting children
        /// </summary>
        [NIBRSCode("E")]
        [CodeDescription("exploiting children")]
        EXPLOITING_CHILDREN,

        /// <summary>
        /// other gang
        /// </summary>
        [NIBRSCode("G")]
        [CodeDescription("other gang")]
        OTHER_GANG,

        /// <summary>
        /// juvenile gang
        /// </summary>
        [NIBRSCode("J")]
        [CodeDescription("juvenile gang")]
        JUVENILE_GANG,

        /// <summary>
        /// none/ unknown
        /// </summary>
        [NIBRSCode("N")]
        [CodeDescription("none/ unknown")]
        NONE_UNKNOWN,

        /// <summary>
        /// operate/ promote/ assist
        /// </summary>
        [NIBRSCode("O")]
        [CodeDescription("operate/ promote/ assist")]
        OOPERATE_PROMOTE_ASSIST,

        /// <summary>
        /// possession/ conceal
        /// </summary>
        [NIBRSCode("P")]
        [CodeDescription("possession/ conceal")]
        POSSESS_CONCEAL,

        /// <summary>
        /// transport/ transmitting
        /// </summary>
        [NIBRSCode("T")]
        [CodeDescription("transport/ transmitting")]
        TRANSPORT_TRANSMIT,

        /// <summary>
        /// using/ consuming
        /// </summary>
        [NIBRSCode("U")]
        [CodeDescription("using/ consuming")]
        USING_CONSUMING
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
        [NIBRSCode("A")]
        [CodeDescription("crack cocaine")]
        CRACK_COCAINE,

        /// <summary>
        /// cocaine
        /// </summary>
        [NIBRSCode("B")]
        [CodeDescription("cocaine")]
        COCAINE,

        /// <summary>
        /// hashish
        /// </summary>
        [NIBRSCode("C")]
        [CodeDescription("hashish")]
        HASHISH,

        /// <summary>
        /// heroin
        /// </summary>
        [NIBRSCode("D")]
        [CodeDescription("heroin")]
        HEROIN,

        /// <summary>
        /// marijuana
        /// </summary>
        [NIBRSCode("E")]
        [CodeDescription("marijuana")]
        MARIJUANA,

        /// <summary>
        /// morphine
        /// </summary>
        [NIBRSCode("F")]
        [CodeDescription("morphine")]
        MORPHINE,

        /// <summary>
        /// opium
        /// </summary>
        [NIBRSCode("G")]
        [CodeDescription("opium")]
        OPIUM,

        /// <summary>
        /// other narcotics
        /// </summary>
        [NIBRSCode("H")]
        [CodeDescription("other narcotics")]
        OTHER_NARCOTIC,

        /// <summary>
        /// LSD
        /// </summary>
        [NIBRSCode("I")]
        [CodeDescription("LSD")]
        LSD,

        /// <summary>
        /// PCP
        /// </summary>
        [NIBRSCode("J")]
        [CodeDescription("PCP")]
        PCP,

        /// <summary>
        /// other hallucinogens
        /// </summary>
        [NIBRSCode("K")]
        [CodeDescription("other hallucinogens")]
        OTHER_HALLUCINOGEN,

        /// <summary>
        /// amphetamines/ methamphetamines
        /// </summary>
        [NIBRSCode("L")]
        [CodeDescription("amphetamines/ methamphetamines")]
        AMPHETAMINES_METHAPHETAMINES,

        /// <summary>
        /// other stimulants
        /// </summary>
        [NIBRSCode("M")]
        [CodeDescription("other stimulants")]
        OTHER_STIMULANT,

        /// <summary>
        /// barbiturates
        /// </summary>
        [NIBRSCode("N")]
        [CodeDescription("barbiturates")]
        BARBITURATES,

        /// <summary>
        /// other depressants
        /// </summary>
        [NIBRSCode("O")]
        [CodeDescription("other depressants")]
        OTHER_DEPRESSANT,

        /// <summary>
        /// other drugs
        /// </summary>
        [NIBRSCode("P")]
        [CodeDescription("other drugs")]
        OTHER_DRUG,

        /// <summary>
        /// unknown drug type
        /// </summary>
        [NIBRSCode("U")]
        [CodeDescription("unknown drug type")]
        UNKNOWN_DRUG_TYPE,

        /// <summary>
        /// over 3 drug types
        /// </summary>
        [NIBRSCode("X")]
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
        [NIBRSCode("DU")]
        [CodeDescription("Dosage Units/ Items")]
        DOSAGE_UNITS_OR_ITEMS,

        /// <summary>
        /// Fluid Ounce
        /// </summary>
        [NIBRSCode("FO")]
        [CodeDescription("Fluid Ounce")]
        FLUID_OUNCE,

        /// <summary>
        /// Gallon
        /// </summary>
        [NIBRSCode("GL")]
        [CodeDescription("Gallon")]
        GALLON,

        /// <summary>
        /// Gram
        /// </summary>
        [NIBRSCode("GM")]
        [CodeDescription("Gram")]
        GRAM,

        /// <summary>
        /// Kilogram
        /// </summary>
        [NIBRSCode("KG")]
        [CodeDescription("Kilogram")]
        KILOGRAM,

        /// <summary>
        /// Pound
        /// </summary>
        [NIBRSCode("LB")]
        [CodeDescription("Pound")]
        POUND,

        /// <summary>
        /// Liter
        /// </summary>
        [NIBRSCode("LT")]
        [CodeDescription("Liter")]
        LITER,

        /// <summary>
        /// Milliliter
        /// </summary>
        [NIBRSCode("ML")]
        [CodeDescription("Milliliter")]
        MILLILITER,

        /// <summary>
        /// Number of Plants
        /// </summary>
        [NIBRSCode("NP")]
        [CodeDescription("Number of Plants")]
        NUMBER_OF_PLANTS,

        /// <summary>
        /// Ounce
        /// </summary>
        [NIBRSCode("OZ")]
        [CodeDescription("Ounce")]
        OUNCE,

        /// <summary>
        /// Not Reported
        /// </summary>
        [NIBRSCode("XX")]
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
        [NIBRSCode("H")]
        [CodeDescription("Hispanic or Latino")]
        HISPANIC_OR_LATINO,

        /// <summary>
        /// Not Hispanic or Latino
        /// </summary>
        [NIBRSCode("N")]
        [CodeDescription("Not Hispanic or Latino")]
        NOT_HISPANIC_OR_LATINO,

        /// <summary>
        /// Unknown
        /// </summary>
        [NIBRSCode("U")]
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
        [NIBRSCode("11")]
        [CodeDescription("Firearm (type not stated)")]
        FIREARM,

        /// <summary>
        /// Automatic Firearm (type not stated)
        /// </summary>
        [NIBRSCode("11A")]
        [CodeDescription("Automatic Firearm (type not stated)")]
        AUTOMATIC_FIREARM,

        /// <summary>
        /// Handgun
        /// </summary>
        [NIBRSCode("12")]
        [CodeDescription("Handgun")]
        HANDGUN,

        /// <summary>
        /// Automatic Handgun
        /// </summary>
        [NIBRSCode("12A")]
        [CodeDescription("Automatic Handgun")]
        AUTOMATIC_HANDGUN,

        /// <summary>
        /// Rifle
        /// </summary>
        [NIBRSCode("13")]
        [CodeDescription("Rifle")]
        RIFLE,

        /// <summary>
        /// Automatic Rifle
        /// </summary>
        [NIBRSCode("13A")]
        [CodeDescription("Automatic Rifle")]
        AUTOMATIC_RIFLE,

        /// <summary>
        /// Shotgun
        /// </summary>
        [NIBRSCode("14")]
        [CodeDescription("Shotgun")]
        SHOTGUN,

        /// <summary>
        /// Automatic Shotgun
        /// </summary>
        [NIBRSCode("14A")]
        [CodeDescription("Automatic Shotgun")]
        AUTOMATIC_SHOTGUN,

        /// <summary>
        /// Other Firearm
        /// </summary>
        [NIBRSCode("15")]
        [CodeDescription("Other Firearm")]
        OTHER_FIREARM,

        /// <summary>
        /// Other Automatic Firearm
        /// </summary>
        [NIBRSCode("15A")]
        [CodeDescription("Other Automatic Firearm")]
        OTHER_AUTOMATIC_FIREARM,

        /// <summary>
        /// Knife/Cutting Instrument
        /// </summary>
        [NIBRSCode("20")]
        [CodeDescription("Knife/Cutting Instrument")]
        LETHAL_CUTTING_INSTRUMENT,

        /// <summary>
        /// Blunt Object
        /// </summary>
        [NIBRSCode("30")]
        [CodeDescription("Blunt Object")]
        BLUNT_OBJECT,

        /// <summary>
        /// Motor Vehicle
        /// </summary>
        [NIBRSCode("35")]
        [CodeDescription("Motor Vehicle")]
        MOTOR_VEHICLE,

        /// <summary>
        /// Personal Weapons
        /// </summary>
        [NIBRSCode("40")]
        [CodeDescription("Personal Weapons")]
        PERSONAL_WEAPONS,

        /// <summary>
        /// Poison
        /// </summary>
        [NIBRSCode("50")]
        [CodeDescription("Poison")]
        POISON,

        /// <summary>
        /// Explosives
        /// </summary>
        [NIBRSCode("60")]
        [CodeDescription("Explosives")]
        EXPLOSIVES,

        /// <summary>
        /// Fire/ Incendiary Device
        /// </summary>
        [NIBRSCode("65")]
        [CodeDescription("Fire/ Incendiary Device")]
        FIRE_INCENDIARY_DEVICE,

        /// <summary>
        /// Drugs/ Narcotics/ Sleeping Pills
        /// </summary>
        [NIBRSCode("70")]
        [CodeDescription("Drugs/ Narcotics/ Sleeping Pills")]
        DRUGS_NARCOTICS_SLEEPING_PILLS,

        /// <summary>
        /// Asphyxiation
        /// </summary>
        [NIBRSCode("85")]
        [CodeDescription("Asphyxiation")]
        ASPHYXIATION,

        /// <summary>
        /// Other
        /// </summary>
        [NIBRSCode("90")]
        [CodeDescription("Other")]
        OTHER,

        /// <summary>
        /// Unknown
        /// </summary>
        [NIBRSCode("95")]
        [CodeDescription("Unknown")]
        UNKNOWN,

        /// <summary>
        /// None
        /// </summary>
        [NIBRSCode("99")]
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
        [NIBRSCode("A")]
        [CodeDescription("Death of Offender")]
        DEATH_OF_OFFENDER,

        /// <summary>
        /// Prosecution Declined (by the prosecutor for other than lack of probable cause)
        /// </summary>
        [NIBRSCode("B")]
        [CodeDescription("Prosecution Declined (by the prosecutor for other than lack of probable cause)")]
        PROSECUTION_DECLINED,

        /// <summary>
        /// In Custody of Other Jurisdiction
        /// </summary>
        [NIBRSCode("C")]
        [CodeDescription("In Custody of Other Jurisdiction")]
        IN_CUSTODY,

        /// <summary>
        /// Victim Refused to Cooperate (in the prosecution)
        /// </summary>
        [NIBRSCode("D")]
        [CodeDescription("Victim Refused to Cooperate (in the prosecution)")]
        VICTIM_REFUSED_TO_COOPERATE,

        /// <summary>
        /// Juvenile/No Custody (the handling of a juvenile without taking him/her into custody, but rather by oral or written notice given to the parents or legal guardian in a case involving a minor offense, such as a petty larceny)
        /// </summary>
        [NIBRSCode("E")]
        [CodeDescription("Juvenile/No Custody (the handling of a juvenile without taking him/her into custody, but rather by oral or written notice given to the parents or legal guardian in a case involving a minor offense, such as a petty larceny)")]
        JUVENILE_NO_CUSTODY,

        /// <summary>
        /// Not Applicable (not cleared exceptionally)
        /// </summary>
        [NIBRSCode("N")]
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
        [NIBRSCode("A")]
        [CodeDescription("Alcohol")]
        ALCOHOL,

        /// <summary>
        /// Computer Equipment
        /// </summary>
        [NIBRSCode("C")]
        [CodeDescription("Computer Equipment")]
        COMPUTER_EQUIPMENT,

        /// <summary>
        /// Drugs/ Narcotics
        /// </summary>
        [NIBRSCode("D")]
        [CodeDescription("Drugs/ Narcotics")]
        DRUGS_NARCOTICS,

        /// <summary>
        /// Not Applicable
        /// </summary>
        [NIBRSCode("N")]
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
        [NIBRSCode("B")]
        [CodeDescription("apparent broken bones")]
        BROKEN_BONES,

        /// <summary>
        /// possible internal injury
        /// </summary>
        [NIBRSCode("I")]
        [CodeDescription("possible internal injury")]
        INTERNAL_INJURY,

        /// <summary>
        /// severe laceration
        /// </summary>
        [NIBRSCode("L")]
        [CodeDescription("severe laceration")]
        SEVERE_LACERATION,

        /// <summary>
        /// apparent minor injury
        /// </summary>
        [NIBRSCode("M")]
        [CodeDescription("apparent minor injury")]
        MINOR_INJURY,

        /// <summary>
        /// none
        /// </summary>
        [NIBRSCode("N")]
        [CodeDescription("none")]
        NONE,

        /// <summary>
        /// other major injury
        /// </summary>
        [NIBRSCode("O")]
        [CodeDescription("other major injury")]
        OTHER_MAJOR_INJURY,

        /// <summary>
        /// loss of teeth
        /// </summary>
        [NIBRSCode("T")]
        [CodeDescription("loss of teeth")]
        LOSS_OF_TEETH,

        /// <summary>
        /// unconsciousness
        /// </summary>
        [NIBRSCode("U")]
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
        [NIBRSCode("A")]
        [CodeDescription("Criminal Attacked Police Officer and That Officer Killed Criminal")]
        CRIMINAL_ATTACKED_OFFICER_AND_THAT_OFFICER_KILLED_CRIMINAL,

        /// <summary>
        /// Criminal Attacked Police Officer and Criminal Killed by Another Police Officer
        /// </summary>
        [NIBRSCode("B")]
        [CodeDescription("Criminal Attacked Police Officer and Criminal Killed by Another Police Officer")]
        CRIMINAL_ATTACKED_OFFICER_AND_OTHER_OFFICER_KILLED_CRIMINAL,

        /// <summary>
        /// Criminal Attacked a Civilian
        /// </summary>
        [NIBRSCode("C")]
        [CodeDescription("Criminal Attacked a Civilian")]
        CRIMINAL_ATTACKED_CIVILIAN,

        /// <summary>
        /// Criminal Attempted Flight From a Crime
        /// </summary>
        [NIBRSCode("D")]
        [CodeDescription("Criminal Attempted Flight From a Crime")]
        CRIMINAL_ATTEMPTED_FLIGHT,

        /// <summary>
        /// Criminal Killed in Commission of a Crime
        /// </summary>
        [NIBRSCode("E")]
        [CodeDescription("Criminal Killed in Commission of a Crime")]
        CRIMINAL_KILLED_IN_COMMISSION_OF_CRIME,

        /// <summary>
        /// Criminal Resisted Arrest
        /// </summary>
        [NIBRSCode("F")]
        [CodeDescription("Criminal Resisted Arrest")]
        CRIMINAL_RESISTED_ARREST,

        /// <summary>
        /// Unable to Determine/Not Enough Information
        /// </summary>
        [NIBRSCode("G")]
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
        [NIBRSCode("01")]
        [CodeDescription("Responding to Disturbance Call (Family Quarrels, Person with Firearm, Etc.)")]
        RESPONDING_TO_DISTURBANCE_CALL,

        /// <summary>
        /// Burglaries in Progress or Pursuing Burglary Suspects
        /// </summary>
        [NIBRSCode("02")]
        [CodeDescription("Burglaries in Progress or Pursuing Burglary Suspects")]
        BURGLARIES_IN_PROGRESS_OR_PURSUING_BURGLARY_SUSPECTS,

        /// <summary>
        /// Robberies in Progress or Pursuing Robbery Suspects
        /// </summary>
        [NIBRSCode("03")]
        [CodeDescription("Robberies in Progress or Pursuing Robbery Suspects")]
        ROBBERIES_IN_PROGRESS_OR_PURSUING_ROBBERY_SUSPECTS,

        /// <summary>
        /// Attempting Other Arrests
        /// </summary>
        [NIBRSCode("04")]
        [CodeDescription("Attempting Other Arrests")]
        ATTEMPTING_OTHER_ARRESTS,

        /// <summary>
        /// Civil Disorder (Riot, Mass Disobedience)
        /// </summary>
        [NIBRSCode("05")]
        [CodeDescription("Civil Disorder (Riot, Mass Disobedience)")]
        CIVIL_DISORDER,

        /// <summary>
        /// Handling, Transporting, Custody of Prisoners
        /// </summary>
        [NIBRSCode("06")]
        [CodeDescription("Handling, Transporting, Custody of Prisoners")]
        HANDLING_OR_TRANSPORTING_CUSTODY_OF_PRISONERS,

        /// <summary>
        /// Investigating Suspicious Persons or Circumstances
        /// </summary>
        [NIBRSCode("07")]
        [CodeDescription("Investigating Suspicious Persons or Circumstances")]
        INVESTIGATING_SUSPICIOUS_PERSONS_OR_CIRCUMSTANCES,

        /// <summary>
        /// Ambush-No Warning
        /// </summary>
        [NIBRSCode("08")]
        [CodeDescription("Ambush-No Warning")]
        AMBUSH,

        /// <summary>
        /// Handling Persons with Mental Illness
        /// </summary>
        [NIBRSCode("09")]
        [CodeDescription("Handling Persons with Mental Illness")]
        HANDLING_PERSONS_WITH_MENTAL_ILLNESS,

        /// <summary>
        /// Traffic Pursuits and Stops
        /// </summary>
        [NIBRSCode("10")]
        [CodeDescription("Traffic Pursuits and Stops")]
        TRAFFIC_PURSUITS_AND_STOPS,

        /// <summary>
        /// All Other
        /// </summary>
        [NIBRSCode("11")]
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
        [NIBRSCode("F")]
        [CodeDescription("Two-Officer Vehicle - uniformed law enforcement officers")]
        TWO_OFFICER_VEHICLE,

        /// <summary>
        /// One-Officer Vehicle (Alone) - uniformed law enforcement officers
        /// </summary>
        [NIBRSCode("G")]
        [CodeDescription("One-Officer Vehicle (Alone) - uniformed law enforcement officers")]
        ONE_OFFICER_VEHICLE_ALONE,

        /// <summary>
        /// One-Officer Vehicle (Assisted) - uniformed law enforcement officers
        /// </summary>
        [NIBRSCode("H")]
        [CodeDescription("One-Officer Vehicle (Assisted) - uniformed law enforcement officers")]
        ONE_OFFICER_VEHICLE_ASSISTED,

        /// <summary>
        /// Detective or Special Assignment (Alone) - non-uniformed officers
        /// </summary>
        [NIBRSCode("I")]
        [CodeDescription("Detective or Special Assignment (Alone) - non-uniformed officers")]
        DETECTIVE_OR_SPECIAL_ASSIGNLENT_ALONE,

        /// <summary>
        /// Detective or Special Assignment (Assisted) - non-uniformed officers
        /// </summary>
        [NIBRSCode("J")]
        [CodeDescription("Detective or Special Assignment (Assisted) - non-uniformed officers")]
        DETECTIVE_OR_SPECIAL_ASSIGNLENT_ASSISTED,

        /// <summary>
        /// Other (Alone) - law enforcement officers serving in other capacities (foot patrol, off duty, etc)
        /// </summary>
        [NIBRSCode("K")]
        [CodeDescription("Other (Alone) - law enforcement officers serving in other capacities (foot patrol, off duty, etc.)")]
        OTHER_ALONE,

        /// <summary>
        /// Other (Assisted) - law enforcement officers serving in other capacities (foot patrol, off duty, etc)
        /// </summary>
        [NIBRSCode("L")]
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
        [NIBRSCode("F")]
        [CodeDescription("Force")]
        FORCE,

        /// <summary>
        /// No Force
        /// </summary>
        [NIBRSCode("N")]
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
        [NIBRSCode("C")]
        [CodeDescription("Count Arrestee")]
        COUNT,

        /// <summary>
        /// Multiple
        /// </summary>
        [NIBRSCode("M")]
        [CodeDescription("Multiple")]
        MULTIPLE,

        /// <summary>
        /// Not Applicable
        /// </summary>
        [NIBRSCode("N")]
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
        [NIBRSCode("01")]
        [CodeDescription("aircraft")]
        AIRCRAFT,

        /// <summary>
        /// alcohol
        /// </summary>
        [NIBRSCode("02")]
        [CodeDescription("alcohol")]
        ALCOHOL,

        /// <summary>
        /// automobile
        /// </summary>
        [NIBRSCode("03")]
        [CodeDescription("automobile")]
        AUTOMOBILE,

        /// <summary>
        /// bicycles
        /// </summary>
        [NIBRSCode("04")]
        [CodeDescription("bicycles")]
        BICYCLES,

        /// <summary>
        /// buses
        /// </summary>
        [NIBRSCode("05")]
        [CodeDescription("buses")]
        BUSES,

        /// <summary>
        /// clothes/ furs
        /// </summary>
        [NIBRSCode("06")]
        [CodeDescription("clothes/ furs")]
        CLOTHING,

        /// <summary>
        /// computer hardware/ software
        /// </summary>
        [NIBRSCode("07")]
        [CodeDescription("computer hardware/ software")]
        COMPUTER_HARDWARE_SOFTWARE,

        /// <summary>
        /// consumable goods
        /// </summary>
        [NIBRSCode("08")]
        [CodeDescription("consumable goods")]
        CONSUMABLES,

        /// <summary>
        /// credit/ debit cards
        /// </summary>
        [NIBRSCode("09")]
        [CodeDescription("credit/ debit cards")]
        CREDIT_DEBIT_CARDS,

        /// <summary>
        /// drugs/ narcotics
        /// </summary>
        [NIBRSCode("10")]
        [CodeDescription("drugs/ narcotics")]
        DRUGS_NARCOTICS,

        /// <summary>
        /// drug/ narcotic equipment
        /// </summary>
        [NIBRSCode("11")]
        [CodeDescription("drug/ narcotic equipment")]
        DRUG_NARCOTIC_EQUIPMENT,

        /// <summary>
        /// farm equipment
        /// </summary>
        [NIBRSCode("12")]
        [CodeDescription("farm equipment")]
        FARM_EQUIPMENT,

        /// <summary>
        /// firearms
        /// </summary>
        [NIBRSCode("13")]
        [CodeDescription("firearms")]
        FIREARMS,

        /// <summary>
        /// gambling equipment
        /// </summary>
        [NIBRSCode("14")]
        [CodeDescription("gambling equipment")]
        GAMBLING_EQUIPMENT,

        /// <summary>
        /// heavy construction/ industrial equipment
        /// </summary>
        [NIBRSCode("15")]
        [CodeDescription("heavy construction/ industrial equipment")]
        CONSTRUCTION_INDUSTRIAL_EQUIPMENT,

        /// <summary>
        /// household goods
        /// </summary>
        [NIBRSCode("16")]
        [CodeDescription("household goods")]
        HOUSEHOLD_GOODS,

        /// <summary>
        /// jewelry/ precious metals/ gems
        /// </summary>
        [NIBRSCode("17")]
        [CodeDescription("jewelry/ precious metals/ gems")]
        JEWELRY_PRECIOUS_METALS_GEMS,

        /// <summary>
        /// livestock
        /// </summary>
        [NIBRSCode("18")]
        [CodeDescription("livestock")]
        LIVESTOCK,

        /// <summary>
        /// merchandise
        /// </summary>
        [NIBRSCode("19")]
        [CodeDescription("merchandise")]
        MERCHANDISE,

        /// <summary>
        /// money
        /// </summary>
        [NIBRSCode("20")]
        [CodeDescription("money")]
        MONEY,

        /// <summary>
        /// negotiable instruments
        /// </summary>
        [NIBRSCode("21")]
        [CodeDescription("negotiable instruments")]
        NEGOTIABLE_INSTRUMENTS,

        /// <summary>
        /// nonnegotiable instruments
        /// </summary>
        [NIBRSCode("22")]
        [CodeDescription("nonnegotiable instruments")]
        NONNEGOTIABLE_INSTRUMENTS,

        /// <summary>
        /// office-type equipment
        /// </summary>
        [NIBRSCode("23")]
        [CodeDescription("office-type equipment")]
        OFFICE_TYPE_EQUIPMENT,

        /// <summary>
        /// other motor vehicles
        /// </summary>
        [NIBRSCode("24")]
        [CodeDescription("other motor vehicles")]
        OTHER_MOTOR_VEHICLES,

        /// <summary>
        /// purses/ handbags/ wallets
        /// </summary>
        [NIBRSCode("25")]
        [CodeDescription("purses/ handbags/ wallets")]
        PURSES_HANDBAGS_WALLETS,

        /// <summary>
        /// radios/ tvs/ vcrs/ dvd players
        /// </summary>
        [NIBRSCode("26")]
        [CodeDescription("radios/ tvs/ vcrs/ dvd players")]
        RADIOS_TVS_VCRS_DVD_PLAYERS,

        /// <summary>
        /// recordings - audio/ visual
        /// </summary>
        [NIBRSCode("27")]
        [CodeDescription("recordings - audio/ visual")]
        AUDIO_VISUAL_RECORDINGS,

        /// <summary>
        /// recreational vehicles
        /// </summary>
        [NIBRSCode("28")]
        [CodeDescription("recreational vehicles")]
        RECREATIONAL_VEHICLES,

        /// <summary>
        /// structures - single occupancy dwellings
        /// </summary>
        [NIBRSCode("29")]
        [CodeDescription("structures - single occupancy dwellings")]
        SINGLE_OCCUPANCY_DWELLING_STRUCTURE,

        /// <summary>
        /// structures - other dwellings
        /// </summary>
        [NIBRSCode("30")]
        [CodeDescription("structures - other dwellings")]
        OTHER_DWELLING_STRUCTURE,

        /// <summary>
        /// structures - other commercial/ business
        /// </summary>
        [NIBRSCode("31")]
        [CodeDescription("structures - other commercial/ business")]
        OTHER_COMMERCIAL_BUSINESS_STRUCTURE,

        /// <summary>
        /// structures - industrial/ manufacturing
        /// </summary>
        [NIBRSCode("32")]
        [CodeDescription("structures - industrial/ manufacturing")]
        INDUSTRIAL_MANUFACTURING_STRUCTURE,

        /// <summary>
        /// structures - public/ community
        /// </summary>
        [NIBRSCode("33")]
        [CodeDescription("structures - public/ community")]
        PUBLIC_COMMUNITY_STRUCTURE,

        /// <summary>
        /// structures - storage
        /// </summary>
        [NIBRSCode("34")]
        [CodeDescription("structures - storage")]
        STORAGE_STRUCTURE,

        /// <summary>
        /// structures - other
        /// </summary>
        [NIBRSCode("35")]
        [CodeDescription("structures - other")]
        OTHER_STRUCTURE,

        /// <summary>
        /// tools
        /// </summary>
        [NIBRSCode("36")]
        [CodeDescription("tools")]
        TOOLS,

        /// <summary>
        /// trucks
        /// </summary>
        [NIBRSCode("37")]
        [CodeDescription("trucks")]
        TRUCKS,

        /// <summary>
        /// vehicle parts/ accessories
        /// </summary>
        [NIBRSCode("38")]
        [CodeDescription("vehicle parts/ accessories")]
        VEHICLE_PARTS_ACCESSORIES,

        /// <summary>
        /// watercraft
        /// </summary>
        [NIBRSCode("39")]
        [CodeDescription("watercraft")]
        WATERCRAFT,

        /// <summary>
        /// aircraft parts/ accessories
        /// </summary>
        [NIBRSCode("41")]
        [CodeDescription("aircraft parts/ accessories")]
        AIRCRAFT_PARTS_ACCESSORIES,

        /// <summary>
        /// artistic supplies/ accessories
        /// </summary>
        [NIBRSCode("42")]
        [CodeDescription("artistic supplies/ accessories")]
        ARTISTIC_SUPPLIES_ACCESSORIES,

        /// <summary>
        /// building materials
        /// </summary>
        [NIBRSCode("43")]
        [CodeDescription("building materials")]
        BUILDING_MATERIALS,

        /// <summary>
        /// Camping/ Hunting/ Fishing Equipment/ Supplies
        /// </summary>
        [NIBRSCode("44")]
        [CodeDescription("Camping/ Hunting/ Fishing Equipment/ Supplies")]
        CAMPING_HUNTING_FISHING_EQUIPMENT_SUPPLIES,

        /// <summary>
        /// Chemicals
        /// </summary>
        [NIBRSCode("45")]
        [CodeDescription("Chemicals")]
        CHEMICALS,

        /// <summary>
        /// Collections/ Collectibles
        /// </summary>
        [NIBRSCode("46")]
        [CodeDescription("Collections/ Collectibles")]
        COLLECTIONS_COLLECTIBLES,

        /// <summary>
        /// Crops
        /// </summary>
        [NIBRSCode("47")]
        [CodeDescription("Crops")]
        CROPS,

        /// <summary>
        /// Documents/ Personal or Business
        /// </summary>
        [NIBRSCode("48")]
        [CodeDescription("Documents/ Personal or Business")]
        PERSONAL_BUSINESS_DOCUMENTS,

        /// <summary>
        /// Explosives
        /// </summary>
        [NIBRSCode("49")]
        [CodeDescription("Explosives")]
        EXPLOSIVES,

        /// <summary>
        /// Firearm Accessories
        /// </summary>
        [NIBRSCode("59")]
        [CodeDescription("Firearm Accessories")]
        FIREARM_ACCESSORIES,

        /// <summary>
        /// Fuel
        /// </summary>
        [NIBRSCode("64")]
        [CodeDescription("Fuel")]
        FUEL,

        /// <summary>
        /// Identity Documents
        /// </summary>
        [NIBRSCode("65")]
        [CodeDescription("Identity Documents")]
        IDENTITY_DOCUMENTS,

        /// <summary>
        /// Identity - Intangible
        /// </summary>
        [NIBRSCode("66")]
        [CodeDescription("Identity - Intangible")]
        IDENTITY,

        /// <summary>
        /// Law Enforcement Equipment
        /// </summary>
        [NIBRSCode("67")]
        [CodeDescription("Law Enforcement Equipment")]
        LAW_ENFORCEMENT_EQUIPMENT,

        /// <summary>
        /// Lawn/ Yard/ Garden Equipment
        /// </summary>
        [NIBRSCode("68")]
        [CodeDescription("Lawn/ Yard/ Garden Equipment")]
        LAWN_YARD_GARDEN_EQUIPMENT,

        /// <summary>
        /// Logging Equipment
        /// </summary>
        [NIBRSCode("69")]
        [CodeDescription("Logging Equipment")]
        LOGGING_EQUIPMENT,

        /// <summary>
        /// Medical/ Medical Lab Equipment
        /// </summary>
        [NIBRSCode("70")]
        [CodeDescription("Medical/ Medical Lab Equipment")]
        MEDICAL_OR_MEDICAL_LAB_EQUIPMENT,

        /// <summary>
        /// Metals, Non-Precious
        /// </summary>
        [NIBRSCode("71")]
        [CodeDescription("Metals, Non-Precious")]
        METALS,

        /// <summary>
        /// Musical Instruments
        /// </summary>
        [NIBRSCode("72")]
        [CodeDescription("Musical Instruments")]
        MUSICAL_INSTRUMENTS,

        /// <summary>
        /// Pets
        /// </summary>
        [NIBRSCode("73")]
        [CodeDescription("Pets")]
        PETS,

        /// <summary>
        /// Photographic/ Optical Equipment
        /// </summary>
        [NIBRSCode("74")]
        [CodeDescription("Photographic/ Optical Equipment")]
        PHOTOGRAPHIC_OPTICAL_EQUIPMENT,

        /// <summary>
        /// Portable Electronic Communications
        /// </summary>
        [NIBRSCode("75")]
        [CodeDescription("Portable Electronic Communications")]
        PORTABLE_ELECTRONIC_COMMUNICATIONS,

        /// <summary>
        /// Recreational/ Sports Equipment
        /// </summary>
        [NIBRSCode("76")]
        [CodeDescription("Recreational/ Sports Equipment")]
        RECREATIONAL_SPORTS_EQUIPMENT,

        /// <summary>
        /// Other
        /// </summary>
        [NIBRSCode("77")]
        [CodeDescription("Other")]
        OTHER,

        /// <summary>
        /// Trailers
        /// </summary>
        [NIBRSCode("78")]
        [CodeDescription("Trailers")]
        TRAILERS,

        /// <summary>
        /// Watercraft Equipment/Parts/Accessories
        /// </summary>
        [NIBRSCode("79")]
        [CodeDescription("Watercraft Equipment/Parts/Accessories")]
        WATERCRAFT_EQUIPMENT_PARTS_ACCESSORIES,

        /// <summary>
        /// Weapons - Other
        /// </summary>
        [NIBRSCode("80")]
        [CodeDescription("Weapons - Other")]
        OTHER_WEAPONS,

        /// <summary>
        /// Pending Inventory
        /// </summary>
        [NIBRSCode("88")]
        [CodeDescription("Pending Inventory")]
        PENDING_INVENTORY,

        /// <summary>
        /// (blank) - this data value is not currently used by the FBI
        /// </summary>
        [NIBRSCode("99")]
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
        [NIBRSCode("N")]
        [CodeDescription("Nonresident")]
        NONRESIDENT,

        /// <summary>
        /// Resident
        /// </summary>
        [NIBRSCode("R")]
        [CodeDescription("Resident")]
        RESIDENT,

        /// <summary>
        /// Unknown
        /// </summary>
        [NIBRSCode("U")]
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
        [NIBRSCode("B")]
        [CodeDescription("Business")]
        BUSINESS,

        /// <summary>
        /// Financial Institution
        /// </summary>
        [NIBRSCode("F")]
        [CodeDescription("Financial Institution")]
        FINANCIAL_INSTITUTION,

        /// <summary>
        /// Government
        /// </summary>
        [NIBRSCode("G")]
        [CodeDescription("Government")]
        GOVERNMENT,

        /// <summary>
        /// Individual
        /// </summary>
        [NIBRSCode("I")]
        [CodeDescription("Individual")]
        INDIVIDUAL,

        /// <summary>
        /// Law Enforcement Officer
        /// </summary>
        [NIBRSCode("L")]
        [CodeDescription("Law Enforcement Officer")]
        LAW_ENFORCEMENT_OFFICER_LEO,

        /// <summary>
        /// Other
        /// </summary>
        [NIBRSCode("O")]
        [CodeDescription("Other")]
        OTHER,

        /// <summary>
        /// Religious Organization
        /// </summary>
        [NIBRSCode("R")]
        [CodeDescription("Religious Organization")]
        RELIGIOUS_ORGANIZATION,

        /// <summary>
        /// Society/ Public
        /// </summary>
        [NIBRSCode("S")]
        [CodeDescription("Society/ Public")]
        SOCIETY_PUBLIC,

        /// <summary>
        /// Unknown
        /// </summary>
        [NIBRSCode("U")]
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
        [NIBRSCode("ANN")]
        [CodeDescription("year")]
        YEAR,

        /// <summary>
        /// kilosecond
        /// </summary>
        [NIBRSCode("B52")]
        [CodeDescription("kilosecond")]
        KILOSECOND,

        /// <summary>
        /// microsecond
        /// </summary>
        [NIBRSCode("B98")]
        [CodeDescription("microsecond")]
        MICROSECOND,

        /// <summary>
        /// millisecond
        /// </summary>
        [NIBRSCode("C26")]
        [CodeDescription("millisecond")]
        MILLISECOND,

        /// <summary>
        /// nanosecond
        /// </summary>
        [NIBRSCode("C47")]
        [CodeDescription("nanosecond")]
        NANOSECOND,

        /// <summary>
        /// tropical year
        /// </summary>
        [NIBRSCode("D42")]
        [CodeDescription("tropical year")]
        TROPICAL_YEAR,

        /// <summary>
        /// day
        /// </summary>
        [NIBRSCode("DAY")]
        [CodeDescription("day")]
        DAY,

        /// <summary>
        /// hour
        /// </summary>
        [NIBRSCode("HUR")]
        [CodeDescription("hour")]
        HOUR,

        /// <summary>
        /// minute [unit of time]
        /// </summary>
        [NIBRSCode("MIN")]
        [CodeDescription("minute [unit of time]")]
        MINUTE,

        /// <summary>
        /// month
        /// </summary>
        [NIBRSCode("MON")]
        [CodeDescription("month")]
        MONTH,

        /// <summary>
        /// second [unit of time]
        /// </summary>
        [NIBRSCode("SEC")]
        [CodeDescription("second [unit of time]")]
        SECOND,

        /// <summary>
        /// week
        /// </summary>
        [NIBRSCode("WEE")]
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
        [NIBRSCode("BAIT")]
        [CodeDescription("Bait")]
        BAIT,

        /// <summary>
        /// Burned
        /// </summary>
        [NIBRSCode("BURNED")]
        [CodeDescription("Burned")]
        BURNED,

        /// <summary>
        /// Cargo
        /// </summary>
        [NIBRSCode("CARGO")]
        [CodeDescription("Cargo")]
        CARGO,

        /// <summary>
        /// Contraband
        /// </summary>
        [NIBRSCode("CONTRABAND")]
        [CodeDescription("Contraband")]
        CONTRABAND,

        /// <summary>
        /// Counterfeited, includes forged
        /// </summary>
        [NIBRSCode("COUNTERFEITED")]
        [CodeDescription("Counterfeited, includes forged")]
        COUNTERFEITED,

        /// <summary>
        /// Cultivated
        /// </summary>
        [NIBRSCode("CULTIVATED")]
        [CodeDescription("Cultivated")]
        CULTIVATED,

        /// <summary>
        /// Damaged
        /// </summary>
        [NIBRSCode("DAMAGED")]
        [CodeDescription("Damaged")]
        DAMAGED,

        /// <summary>
        /// Destroyed
        /// </summary>
        [NIBRSCode("DESTROYED")]
        [CodeDescription("Destroyed")]
        DESTROYED,

        /// <summary>
        /// Destroyed_Damaged_Vandalized
        /// </summary>
        [NIBRSCode("DESTROYED_DAMAGED_VANDALIZED")]
        [CodeDescription("Destroyed_Damaged_Vandalized")]
        DESTROYED_DAMAGED_VANDALIZED,

        /// <summary>
        /// Found
        /// </summary>
        [NIBRSCode("FOUND")]
        [CodeDescription("Found")]
        FOUND,

        /// <summary>
        /// Lost
        /// </summary>
        [NIBRSCode("LOST")]
        [CodeDescription("Lost")]
        LOST,

        /// <summary>
        /// Recovered (To impound property previously stolen)
        /// </summary>
        [NIBRSCode("RECOVERED")]
        [CodeDescription("Recovered (To impound property previously stolen)")]
        RECOVERED,

        /// <summary>
        /// Returned
        /// </summary>
        [NIBRSCode("RETURNED")]
        [CodeDescription("Returned")]
        RETURNED,

        /// <summary>
        /// Seized (To impound property not previously stolen)
        /// </summary>
        [NIBRSCode("SEIZED")]
        [CodeDescription("Seized (To impound property not previously stolen)")]
        SEIZED,

        /// <summary>
        /// Stolen
        /// </summary>
        [NIBRSCode("STOLEN")]
        [CodeDescription("Stolen")]
        STOLEN,

        /// <summary>
        /// Stolen_Bribed
        /// </summary>
        [NIBRSCode("STOLEN_BRIBED")]
        [CodeDescription("Stolen_Bribed")]
        STOLEN_BRIBED,

        /// <summary>
        /// Stolen_Defrauded
        /// </summary>
        [NIBRSCode("STOLEN_DEFRAUDED")]
        [CodeDescription("Stolen_Defrauded")]
        STOLEN_DEFRAUDED,

        /// <summary>
        /// Stolen_Embezzled
        /// </summary>
        [NIBRSCode("STOLEN_EMBEZZLED")]
        [CodeDescription("Stolen_Embezzled")]
        STOLEN_EMBEZZLED,

        /// <summary>
        /// Stolen_Extorted
        /// </summary>
        [NIBRSCode("STOLEN_EXTORTED")]
        [CodeDescription("Stolen_Extorted")]
        STOLEN_EXTORTED,

        /// <summary>
        /// Stolen_Ransomed
        /// </summary>
        [NIBRSCode("STOLEN_RANSOMED")]
        [CodeDescription("Stolen_Ransomed")]
        STOLEN_RANSOMED,

        /// <summary>
        /// Stolen_Robbed
        /// </summary>
        [NIBRSCode("STOLEN_ROBBED")]
        [CodeDescription("Stolen_Robbed")]
        STOLEN_ROBBED,

        /// <summary>
        /// Vandalized
        /// </summary>
        [NIBRSCode("VANDALIZED")]
        [CodeDescription("Vandalized")]
        VANDALIZED,

        /// <summary>
        /// None
        /// </summary>
        [NIBRSCode("NONE")]
        [CodeDescription("None")]
        NONE,

        /// <summary>
        /// Unknown
        /// </summary>
        [NIBRSCode("UNKNOWN")]
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
		[NIBRSCode("01")]
		[CodeDescription("Argument")]
		ARGUMENT,

		/// <summary>
		/// Assault on Law Enforcement Officer(s)
		/// </summary>
		[NIBRSCode("02")]
		[CodeDescription("Assault on Law Enforcement Officer(s)")]
		ASSAULT_ON_LAW_ENFORCEMENT_OFFIER,

		/// <summary>
		/// Drug Dealing
		/// </summary>
		[NIBRSCode("03")]
		[CodeDescription("Drug Dealing")]
		DRUG_DEALING,

		/// <summary>
		/// Gangland (Organized Crime Involvement)
		/// </summary>
		[NIBRSCode("04")]
		[CodeDescription("Gangland (Organized Crime Involvement)")]
		GANGLANG_ORGANIZED_CRIME,

		/// <summary>
		/// Juvenile Gang
		/// </summary>
		[NIBRSCode("05")]
		[CodeDescription("Juvenile Gang")]
		JUVENILE_GANG,

		/// <summary>
		/// Lovers' Quarrel
		/// </summary>
		[NIBRSCode("06")]
		[CodeDescription("Lovers' Quarrel")]
		LOVERS_QUARREL,

		/// <summary>
		/// Mercy Killing (Not applicable to Aggravated Assault)
		/// </summary>
		[NIBRSCode("07")]
		[CodeDescription("Mercy Killing (Not applicable to Aggravated Assault)")]
		MERCY_KILLING,

		/// <summary>
		/// Other Felony Involved
		/// </summary>
		[NIBRSCode("08")]
		[CodeDescription("Other Felony Involved")]
		OTHER_FELONY,

		/// <summary>
		/// Other Circumstances
		/// </summary>
		[NIBRSCode("09")]
		[CodeDescription("Other Circumstances")]
		OTHER_CIRCUMSTANCES,

		/// <summary>
		/// Murder and Non-negligent Manslaughter
		/// </summary>
		[NIBRSCode("09A")]
		[CodeDescription("Murder and Non-negligent Manslaughter")]
		MURDER_NONNEGLIGENT_MANSLAUGHTER,

		/// <summary>
		/// Negligent Manslaughter
		/// </summary>
		[NIBRSCode("09B")]
		[CodeDescription("Negligent Manslaughter")]
		NEGLIGENT_MANSLAUGHTER,

		/// <summary>
		/// Justifiable Homicide
		/// </summary>
		[NIBRSCode("09C")]
		[CodeDescription("Justifiable Homicide")]
		JUSTIFIABLE_HOMICIDE,

		/// <summary>
		/// Unknown Circumstances
		/// </summary>
		[NIBRSCode("10")]
		[CodeDescription("Unknown Circumstances")]
		UNKNOWN_CIRCUMSTANCES,

		/// <summary>
		/// Aggravated Assault
		/// </summary>
		[NIBRSCode("13A")]
		[CodeDescription("Aggravated Assault")]
		AGGRAVATED_ASSAULT,

		/// <summary>
		/// Criminal Killed by Private Citizen
		/// </summary>
		[NIBRSCode("20")]
		[CodeDescription("Criminal Killed by Private Citizen")]
		CRIMINAL_KILLED_BY_PRIVATE_CITIZEN,

		/// <summary>
		/// Criminal Killed by Police Officer
		/// </summary>
		[NIBRSCode("21")]
		[CodeDescription("Criminal Killed by Police Officer")]
		CRIMINAL_KILLED_BY_POLICE_OFFICER,

		/// <summary>
		/// Child Playing With Weapon
		/// </summary>
		[NIBRSCode("30")]
		[CodeDescription("Child Playing With Weapon")]
		CHILD_PLAYING_WITH_WEAPON,

		/// <summary>
		/// Gun-Cleaning Accident
		/// </summary>
		[NIBRSCode("31")]
		[CodeDescription("Gun-Cleaning Accident")]
		GUN_CLEANING_ACCIDENT,

		/// <summary>
		/// Hunting Accident
		/// </summary>
		[NIBRSCode("32")]
		[CodeDescription("Hunting Accident")]
		HUNTING_ACCIDENT,

		/// <summary>
		/// Other Negligent Weapon Handling
		/// </summary>
		[NIBRSCode("33")]
		[CodeDescription("Other Negligent Weapon Handling")]
		OTHER_NEGLIGENT_WEAPON_HANDLING,

		/// <summary>
		/// Other Negligent Killings
		/// </summary>
		[NIBRSCode("34")]
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
		[NIBRSCode("I")]
		[CodeDescription("Incident Report")]
		I,

		/// <summary>
		/// Delete
		/// </summary>
		[NIBRSCode("D")]
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
		[NIBRSCode("A")]
		[CodeDescription("Simple/Gross Neglect")]
		A,

		/// <summary>
		/// Buying/Receiving
		/// </summary>
		[NIBRSCode("B")]
		[CodeDescription("Buying/Receiving")]
		B,

		/// <summary>
		/// Cultivating/Manufacturing/Publishing (i.e., production of any type)
		/// </summary>
		[NIBRSCode("C")]
		[CodeDescription("Cultivating/Manufacturing/Publishing (i.e., production of any type)")]
		C,

		/// <summary>
		/// Distributing/Selling
		/// </summary>
		[NIBRSCode("D")]
		[CodeDescription("Distributing/Selling")]
		D,

		/// <summary>
		/// Exploiting Children
		/// </summary>
		[NIBRSCode("E")]
		[CodeDescription("Exploiting Children")]
		E,

		/// <summary>
		/// Organized Abuse (Dog Fighting and Cock Fighting)
		/// </summary>
		[NIBRSCode("F")]
		[CodeDescription("Organized Abuse (Dog Fighting and Cock Fighting)")]
		F,

		/// <summary>
		/// Other Gang
		/// </summary>
		[NIBRSCode("G")]
		[CodeDescription("Other Gang")]
		G,

		/// <summary>
		/// Intentional Abuse and Torture (tormenting, mutilating, maiming,poisoning, or abandonment)
		/// </summary>
		[NIBRSCode("I")]
		[CodeDescription("Intentional Abuse and Torture (tormenting, mutilating, maiming,poisoning, or abandonment)")]
		I,

		/// <summary>
		/// Juvenile Gang
		/// </summary>
		[NIBRSCode("J")]
		[CodeDescription("Juvenile Gang")]
		J,

		/// <summary>
		/// None/Unknown
		/// </summary>
		[NIBRSCode("N")]
		[CodeDescription("None/Unknown")]
		N,

		/// <summary>
		/// Operating/Promoting/Assisting
		/// </summary>
		[NIBRSCode("O")]
		[CodeDescription("Operating/Promoting/Assisting")]
		O,

		/// <summary>
		/// Possessing/Concealing
		/// </summary>
		[NIBRSCode("P")]
		[CodeDescription("Possessing/Concealing")]
		P,

		/// <summary>
		/// Animal Sexual Abuse (Bestiality)
		/// </summary>
		[NIBRSCode("S")]
		[CodeDescription("Animal Sexual Abuse (Bestiality)")]
		S,

		/// <summary>
		/// Transporting/Transmitting/Importing
		/// </summary>
		[NIBRSCode("T")]
		[CodeDescription("Transporting/Transmitting/Importing")]
		T,

		/// <summary>
		/// Using/Consuming
		/// </summary>
		[NIBRSCode("U")]
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
        [NIBRSCode("01")]
        [CodeDescription("air/ bus/ train terminal")]
        AIR_BUS_TRAIN,

        /// <summary>
        /// bank/ savings and loan
        /// </summary>
        [NIBRSCode("02")]
        [CodeDescription("bank/ savings and loan")]
        BANK,

        /// <summary>
        /// bar/ nightclub
        /// </summary>
        [NIBRSCode("03")]
        [CodeDescription("bar/ nightclub")]
        BAR_NIGHTCLUB,

        /// <summary>
        /// church/ synagogue/ temple/ mosque
        /// </summary>
        [NIBRSCode("04")]
        [CodeDescription("church/ synagogue/ temple/ mosque")]
        CHURCH_SYNAGOGUE_TEMPLE_MOSQUE,

        /// <summary>
        /// commercial/ office building
        /// </summary>
        [NIBRSCode("05")]
        [CodeDescription("commercial/ office building")]
        COMMERCIAL_OFFICE_BUILDING,

        /// <summary>
        /// construction site
        /// </summary>
        [NIBRSCode("06")]
        [CodeDescription("construction site")]
        CONSTRUCTION_SITE,

        /// <summary>
        /// convenience store
        /// </summary>
        [NIBRSCode("07")]
        [CodeDescription("convenience store")]
        CONVENIENCE_STORE,

        /// <summary>
        /// department/ discount/ store
        /// </summary>
        [NIBRSCode("08")]
        [CodeDescription("department/ discount/ store")]
        DEPARTMENT_DISCOUNT_STORE,

        /// <summary>
        /// drug store/ doctor's office/ hospital
        /// </summary>
        [NIBRSCode("09")]
        [CodeDescription("drug store/ doctor's office/ hospital")]
        PHARMACY_DOCTOR_OFFICE_HOSPITAL,

        /// <summary>
        /// field/ woods
        /// </summary>
        [NIBRSCode("10")]
        [CodeDescription("field/ woods")]
        FIELD_WOODS,

        /// <summary>
        /// government/ public building
        /// </summary>
        [NIBRSCode("11")]
        [CodeDescription("government/ public building")]
        GOVERNMENT_PUBLIC_BUILDING,

        /// <summary>
        /// grocery/ supermarket
        /// </summary>
        [NIBRSCode("12")]
        [CodeDescription("grocery/ supermarket")]
        GROCERY_SUPERMARKET,

        /// <summary>
        /// highway/ road/ alley/ street/ sidewalk
        /// </summary>
        [NIBRSCode("13")]
        [CodeDescription("highway/ road/ alley/ street/ sidewalk")]
        HIGHWAY_ROAD_ALLEY_STREET_SIDEWALK,

        /// <summary>
        /// hotel/ motel/ etc
        /// </summary>
        [NIBRSCode("14")]
        [CodeDescription("hotel/ motel/ etc.")]
        HOTEL_MOTEL_LODGING,

        /// <summary>
        /// jail/ prison/ penetentiary/ corrections facility
        /// </summary>
        [NIBRSCode("15")]
        [CodeDescription("jail/ prison/ penetentiary/ corrections facility")]
        JAIL_PRISON_PENETENTIARY_CORRECTIONS_FACILITY,

        /// <summary>
        /// lake/ waterway/ beach
        /// </summary>
        [NIBRSCode("16")]
        [CodeDescription("lake/ waterway/ beach")]
        LAKE_WATERWAY_BEACH,

        /// <summary>
        /// liquor store
        /// </summary>
        [NIBRSCode("17")]
        [CodeDescription("liquor store")]
        LIQUOR_STORE,

        /// <summary>
        /// parking/ drop lot/ garage
        /// </summary>
        [NIBRSCode("18")]
        [CodeDescription("parking/ drop lot/ garage")]
        PARKING_LOG_GARAGE,

        /// <summary>
        /// rental storage facility
        /// </summary>
        [NIBRSCode("19")]
        [CodeDescription("rental storage facility")]
        RENTAL_STORAGE_FACILITY,

        /// <summary>
        /// residence/ home
        /// </summary>
        [NIBRSCode("20")]
        [CodeDescription("residence/ home")]
        RESIDENCE_HOME,

        /// <summary>
        /// restaurant
        /// </summary>
        [NIBRSCode("21")]
        [CodeDescription("restaurant")]
        RESTAURANT,

        /// <summary>
        /// school/ college
        /// </summary>
        [NIBRSCode("22")]
        [CodeDescription("school/ college")]
        SCHOOL_COLLEGE,

        /// <summary>
        /// service/ gas station
        /// </summary>
        [NIBRSCode("23")]
        [CodeDescription("service/ gas station")]
        SERVICE_GAS_STATION,

        /// <summary>
        /// specialty store
        /// </summary>
        [NIBRSCode("24")]
        [CodeDescription("specialty store")]
        SPECIALTY_STORE,

        /// <summary>
        /// other/ unknown
        /// </summary>
        [NIBRSCode("25")]
        [CodeDescription("other/ unknown")]
        OTHER_UNKNOWN,

        /// <summary>
        /// Abandoned/ Condemned Structure
        /// </summary>
        [NIBRSCode("37")]
        [CodeDescription("Abandoned/ Condemned Structure")]
        ABANDONED_CONDEMNED_STRUCTURE,

        /// <summary>
        /// Amusement Park
        /// </summary>
        [NIBRSCode("38")]
        [CodeDescription("Amusement Park")]
        AMUSEMENT_PARK,

        /// <summary>
        /// Arena/ Stadium/ Fairgrounds/Coliseum
        /// </summary>
        [NIBRSCode("39")]
        [CodeDescription("Arena/ Stadium/ Fairgrounds/Coliseum")]
        ARENA_STADIUM_FAIRGROUNDS_COLISEUM,

        /// <summary>
        /// ATM Separate from Bank
        /// </summary>
        [NIBRSCode("40")]
        [CodeDescription("ATM Separate from Bank")]
        ATM_SEPARATE_FROM_BANK,

        /// <summary>
        /// Auto Dealership New/Used
        /// </summary>
        [NIBRSCode("41")]
        [CodeDescription("Auto Dealership New/Used")]
        AUTO_DEALERSHIP,

        /// <summary>
        /// Camp/ Campground
        /// </summary>
        [NIBRSCode("42")]
        [CodeDescription("Camp/ Campground")]
        CAMPGROUND,

        /// <summary>
        /// Daycare Facility
        /// </summary>
        [NIBRSCode("44")]
        [CodeDescription("Daycare Facility")]
        DAYCARE_FACILITY,

        /// <summary>
        /// Dock/ Wharf/ Freight/Modal Terminal
        /// </summary>
        [NIBRSCode("45")]
        [CodeDescription("Dock/ Wharf/ Freight/Modal Terminal")]
        DOCK_WHARF_FREIGHT_TERMINAL,

        /// <summary>
        /// Farm Facility
        /// </summary>
        [NIBRSCode("46")]
        [CodeDescription("Farm Facility")]
        FARM_FACILITY,

        /// <summary>
        /// Gambling Facility/ Casino/ Race Track
        /// </summary>
        [NIBRSCode("47")]
        [CodeDescription("Gambling Facility/ Casino/ Race Track")]
        GAMBLING_FACILITY_CASINO_RACE_TRACK,

        /// <summary>
        /// Industrial Site
        /// </summary>
        [NIBRSCode("48")]
        [CodeDescription("Industrial Site")]
        INDUSTRIAL_SITE,

        /// <summary>
        /// Military Installation
        /// </summary>
        [NIBRSCode("49")]
        [CodeDescription("Military Installation")]
        MILITARY_INSTALLATION,

        /// <summary>
        /// Park/ Playground
        /// </summary>
        [NIBRSCode("50")]
        [CodeDescription("Park/ Playground")]
        PARK_PLAYGROUND,

        /// <summary>
        /// Rest Area
        /// </summary>
        [NIBRSCode("51")]
        [CodeDescription("Rest Area")]
        REST_AREA,

        /// <summary>
        /// School - College/ University
        /// </summary>
        [NIBRSCode("52")]
        [CodeDescription("School - College/ University")]
        SCHOOL_COLLEGE_UNIVERSITY,

        /// <summary>
        /// School - Elementary/ Secondary
        /// </summary>
        [NIBRSCode("53")]
        [CodeDescription("School - Elementary/ Secondary")]
        SCHOOL_ELEMENTARY_SECONDARY,

        /// <summary>
        /// Shelter - Mission/ Homeless
        /// </summary>
        [NIBRSCode("54")]
        [CodeDescription("Shelter - Mission/ Homeless")]
        SHELTER,

        /// <summary>
        /// Shopping Mall
        /// </summary>
        [NIBRSCode("55")]
        [CodeDescription("Shopping Mall")]
        SHOPPING_MALL,

        /// <summary>
        /// Tribal Lands
        /// </summary>
        [NIBRSCode("56")]
        [CodeDescription("Tribal Lands")]
        TRIBAL_LANDS,

        /// <summary>
        /// Community Center
        /// </summary>
        [NIBRSCode("57")]
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
		[NIBRSCode("GROUP A INCIDENT REPORT")]
		[CodeDescription("Group A Incident Report")]
		A,

		/// <summary>
		/// Group A Incident Report - LEOKA
		/// </summary>
		[NIBRSCode("GROUP A INCIDENT REPORT_LEOKA")]
		[CodeDescription("Group A Incident Report - LEOKA")]
		A_LEOKA,

		/// <summary>
		/// Group A Incident Report - Time Window
		/// </summary>
		[NIBRSCode("GROUP A INCIDENT REPORT_TIME WINDOW")]
		[CodeDescription("Group A Incident Report - Time Window")]
		A_TIME_WINDOW,

		/// <summary>
		/// Group B Arrest Report
		/// </summary>
		[NIBRSCode("GROUP B ARREST REPORT")]
		[CodeDescription("Group B Arrest Report")]
		B,

		/// <summary>
		/// Zero Report
		/// </summary>
		[NIBRSCode("ZERO REPORT")]
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
		[NIBRSCode("NN")]
		[CodeDescription("Under 24 Hours")]
		NEONATAL,

		/// <summary>
		/// 1-6 Days Old
		/// </summary>
		[NIBRSCode("NB")]
		[CodeDescription("1-6 Days Old")]
		NEWBORN,

		/// <summary>
		/// 7-364 Days Old
		/// </summary>
		[NIBRSCode("BB")]
		[CodeDescription("7-364 Days Old")]
		BABY,

		/// <summary>
		/// Unknown
		/// </summary>
		[NIBRSCode("00")]
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
        [NIBRSCode("09A")]
        [CodeDescription("Murder &amp; Nonnegligent Manslaughter")]
        MURDER_NONNEGLIGENT,

        /// <summary>
        /// Negligent Manslaughter
        /// </summary>
        [NIBRSCode("09B")]
        [CodeDescription("Negligent Manslaughter")]
        NEGLIGENT_MANSLAUGHTER,

        /// <summary>
        /// Justifiable Homicide
        /// </summary>
        [NIBRSCode("09C")]
        [CodeDescription("Justifiable Homicide")]
        JUSTIFIABLE_HOMICIDE,

        /// <summary>
        /// Kidnapping/Abduction
        /// </summary>
        [NIBRSCode("100")]
        [CodeDescription("Kidnapping/Abduction")]
        KIDNAPPING_ABDUCTION,

        /// <summary>
        /// Rape
        /// </summary>
        [NIBRSCode("11A")]
        [CodeDescription("Rape")]
        RAPE,

        /// <summary>
        /// Sodomy
        /// </summary>
        [NIBRSCode("11B")]
        [CodeDescription("Sodomy")]
        SODOMY,

        /// <summary>
        /// Sexual Assault With An Object
        /// </summary>
        [NIBRSCode("11C")]
        [CodeDescription("Sexual Assault With An Object")]
        SEXUAL_ASSAULT_WITH_OBJECT,

        /// <summary>
        /// Fondling
        /// </summary>
        [NIBRSCode("11D")]
        [CodeDescription("Fondling")]
        FONDLING,

        /// <summary>
        /// Robbery
        /// </summary>
        [NIBRSCode("120")]
        [CodeDescription("Robbery")]
        ROBBERY,

        /// <summary>
        /// Aggravated Assault
        /// </summary>
        [NIBRSCode("13A")]
        [CodeDescription("Aggravated Assault")]
        AGGRAVATED_ASSAULT,

        /// <summary>
        /// Simple Assault
        /// </summary>
        [NIBRSCode("13B")]
        [CodeDescription("Simple Assault")]
        SIMPLE_ASSAULT,

        /// <summary>
        /// Intimidation
        /// </summary>
        [NIBRSCode("13C")]
        [CodeDescription("Intimidation")]
        INTIMIDATION,

        /// <summary>
        /// Arson
        /// </summary>
        [NIBRSCode("200")]
        [CodeDescription("Arson")]
        ARSON,

        /// <summary>
        /// Extortion/Blackmail
        /// </summary>
        [NIBRSCode("210")]
        [CodeDescription("Extortion/Blackmail")]
        EXTORTION_BLACKMAIL,

        /// <summary>
        /// Burglary/Breaking &amp; Entering
        /// </summary>
        [NIBRSCode("220")]
        [CodeDescription("Burglary/Breaking &amp; Entering")]
        BURGLARY_BREAKING_AND_ENTERING,

        /// <summary>
        /// Pocket-picking
        /// </summary>
        [NIBRSCode("23A")]
        [CodeDescription("Pocket-picking")]
        PICKPOCKETING,

        /// <summary>
        /// Purse-snatching
        /// </summary>
        [NIBRSCode("23B")]
        [CodeDescription("Purse-snatching")]
        PURSE_SNATCHING,

        /// <summary>
        /// Shoplifting
        /// </summary>
        [NIBRSCode("23C")]
        [CodeDescription("Shoplifting")]
        SHOPLIFTING,

        /// <summary>
        /// Theft From Building
        /// </summary>
        [NIBRSCode("23D")]
        [CodeDescription("Theft From Building")]
        THEFT_FROM_BUILDING,

        /// <summary>
        /// Theft From Coin-Operated Machine or Device
        /// </summary>
        [NIBRSCode("23E")]
        [CodeDescription("Theft From Coin-Operated Machine or Device")]
        THEFT_FROM_COIN_OPERATED_MACHINE,

        /// <summary>
        /// Theft From Motor Vehicle
        /// </summary>
        [NIBRSCode("23F")]
        [CodeDescription("Theft From Motor Vehicle")]
        THEFT_FROM_MOTOR_VEHICLE,

        /// <summary>
        /// Theft of Motor Vehicle Parts or Accessories
        /// </summary>
        [NIBRSCode("23G")]
        [CodeDescription("Theft of Motor Vehicle Parts or Accessories")]
        THEFT_OF_MOTOR_VEHICLE_PARTS_OR_ACCESSORIES,

        /// <summary>
        /// All Other Larceny
        /// </summary>
        [NIBRSCode("23H")]
        [CodeDescription("All Other Larceny")]
        OTHER_LARCENY,

        /// <summary>
        /// Motor Vehicle Theft
        /// </summary>
        [NIBRSCode("240")]
        [CodeDescription("Motor Vehicle Theft")]
        MOTOR_VEHICLE_THEFT,

        /// <summary>
        /// Counterfeiting/Forgery
        /// </summary>
        [NIBRSCode("250")]
        [CodeDescription("Counterfeiting/Forgery")]
        COINTERFEITING_FORGERY,

        /// <summary>
        /// False Pretenses/Swindle/Confidence Game
        /// </summary>
        [NIBRSCode("26A")]
        [CodeDescription("False Pretenses/Swindle/Confidence Game")]
        FALSE_PRETENSES_SWINDLE_CONFIDENCE_GAME,

        /// <summary>
        /// Credit Card/Automated Teller Machine Fraud
        /// </summary>
        [NIBRSCode("26B")]
        [CodeDescription("Credit Card/Automated Teller Machine Fraud")]
        CREDIT_CARD_FRAUD,

        /// <summary>
        /// Impersonation
        /// </summary>
        [NIBRSCode("26C")]
        [CodeDescription("Impersonation")]
        IMPERSONATION,

        /// <summary>
        /// Welfare Fraud
        /// </summary>
        [NIBRSCode("26D")]
        [CodeDescription("Welfare Fraud")]
        WELFARE_FRAUD,

        /// <summary>
        /// Wire Fraud
        /// </summary>
        [NIBRSCode("26E")]
        [CodeDescription("Wire Fraud")]
        WIRE_FRAUD,

        /// <summary>
        /// Identity Theft
        /// </summary>
        [NIBRSCode("26F")]
        [CodeDescription("Identity Theft")]
        IDENTITY_THEFT,

        /// <summary>
        /// Hacking/Computer Invasion
        /// </summary>
        [NIBRSCode("26G")]
        [CodeDescription("Hacking/Computer Invasion")]
        HACKING_COMPUTER_INVASION,

        /// <summary>
        /// Embezzlement
        /// </summary>
        [NIBRSCode("270")]
        [CodeDescription("Embezzlement")]
        EMBEZZLEMENT,

        /// <summary>
        /// Stolen Offenses
        /// </summary>
        [NIBRSCode("280")]
        [CodeDescription("Stolen Offenses")]
        STOLEN_OFFENSES,

        /// <summary>
        /// Destruction/Damage/Vandalism of Property
        /// </summary>
        [NIBRSCode("290")]
        [CodeDescription("Destruction/Damage/Vandalism of Property")]
        DESTRUCTION_DAMAGE_VANDALISM_OR_PROPERTY,

        /// <summary>
        /// Drug/Narcotic Violations
        /// </summary>
        [NIBRSCode("35A")]
        [CodeDescription("Drug/Narcotic Violations")]
        DRUGS_NARCOTICS,

        /// <summary>
        /// Drug Equipment Violations
        /// </summary>
        [NIBRSCode("35B")]
        [CodeDescription("Drug Equipment Violations")]
        DRUG_EQUIPMENT,

        /// <summary>
        /// Incest
        /// </summary>
        [NIBRSCode("36A")]
        [CodeDescription("Incest")]
        INCEST,

        /// <summary>
        /// Statutory Rape
        /// </summary>
        [NIBRSCode("36B")]
        [CodeDescription("Statutory Rape")]
        STATUTORY_RAPE,

        /// <summary>
        /// Pornography/Obscene Material
        /// </summary>
        [NIBRSCode("370")]
        [CodeDescription("Pornography/Obscene Material")]
        PORNOGRAPHY,

        /// <summary>
        /// Betting/Wagering
        /// </summary>
        [NIBRSCode("39A")]
        [CodeDescription("Betting/Wagering")]
        BETTING_WAGERING,

        /// <summary>
        /// Operating/Promoting/Assisting Gambling
        /// </summary>
        [NIBRSCode("39B")]
        [CodeDescription("Operating/Promoting/Assisting Gambling")]
        OPERATING_PROMOTING_ASSISTING_GAMBLING,

        /// <summary>
        /// Gambling Equipment Violation
        /// </summary>
        [NIBRSCode("39C")]
        [CodeDescription("Gambling Equipment Violation")]
        GAMBLING_EQUIPMENT,

        /// <summary>
        /// Sports Tampering
        /// </summary>
        [NIBRSCode("39D")]
        [CodeDescription("Sports Tampering")]
        SPORTS_TAMPERING,

        /// <summary>
        /// Prostitution
        /// </summary>
        [NIBRSCode("40A")]
        [CodeDescription("Prostitution")]
        PROSTITUTION,

        /// <summary>
        /// Assisting or Promoting Prostitution
        /// </summary>
        [NIBRSCode("40B")]
        [CodeDescription("Assisting or Promoting Prostitution")]
        ASSISTING_PROMOTING_PROSTITUTION,

        /// <summary>
        /// Purchasing Prostitution
        /// </summary>
        [NIBRSCode("40C")]
        [CodeDescription("Purchasing Prostitution")]
        PURCHASING_PROSTITUTION,

        /// <summary>
        /// Bribery
        /// </summary>
        [NIBRSCode("510")]
        [CodeDescription("Bribery")]
        BRIBERY,

        /// <summary>
        /// Weapon Law Violations
        /// </summary>
        [NIBRSCode("520")]
        [CodeDescription("Weapon Law Violations")]
        WEAPON_LAW,

        /// <summary>
        /// Human Trafficking, Commercial Sex Acts
        /// </summary>
        [NIBRSCode("64A")]
        [CodeDescription("Human Trafficking, Commercial Sex Acts")]
        HUMAN_TRAFFICKING_COMMERCIAL_SEX_ACTS,

        /// <summary>
        /// Human Trafficking, Involuntary Servitude
        /// </summary>
        [NIBRSCode("64B")]
        [CodeDescription("Human Trafficking, Involuntary Servitude")]
        HUMAN_TRAFFICKING_INVOLUNTARY_SERVITUDE,

        /// <summary>
        /// Animal Cruelty
        /// </summary>
        [NIBRSCode("720")]
        [CodeDescription("Animal Cruelty")]
        ANIMAL_CRUELTY,

        /// <summary>
        /// Bad Checks
        /// </summary>
        [NIBRSCode("90A")]
        [CodeDescription("Bad Checks")]
        BAD_CHECKS,

        /// <summary>
        /// Curfew/Loitering/Vagrancy Violations
        /// </summary>
        [NIBRSCode("90B")]
        [CodeDescription("Curfew/Loitering/Vagrancy Violations")]
        CURFEW_LOITERING_VAGRANCY,

        /// <summary>
        /// Disorderly Conduct
        /// </summary>
        [NIBRSCode("90C")]
        [CodeDescription("Disorderly Conduct")]
        DISORDERLY_CONDUCT,

        /// <summary>
        /// Driving Under the Influence
        /// </summary>
        [NIBRSCode("90D")]
        [CodeDescription("Driving Under the Influence")]
        DUI,

        /// <summary>
        /// Drunkenness
        /// </summary>
        [NIBRSCode("90E")]
        [CodeDescription("Drunkenness")]
        DRUNKENNESS,

        /// <summary>
        /// Family Offenses, Nonviolent
        /// </summary>
        [NIBRSCode("90F")]
        [CodeDescription("Family Offenses, Nonviolent")]
        FAMILY,

        /// <summary>
        /// Liquor Law Violations
        /// </summary>
        [NIBRSCode("90G")]
        [CodeDescription("Liquor Law Violations")]
        LIQUOR_LAW,

        /// <summary>
        /// Peeping Tom
        /// </summary>
        [NIBRSCode("90H")]
        [CodeDescription("Peeping Tom")]
        PEEPING_TOM,

        /// <summary>
        /// Runaway
        /// </summary>
        [NIBRSCode("90I")]
        [CodeDescription("Runaway")]
        RUNAWAY,

        /// <summary>
        /// Trespass of Real
        /// </summary>
        [NIBRSCode("90J")]
        [CodeDescription("Trespass of Real")]
        TRESPASS,

        /// <summary>
        /// All Other Offenses
        /// </summary>
        [NIBRSCode("90Z")]
        [CodeDescription("All Other Offenses")]
        OTHER_OFFENSE
    }
}
