<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          body {
          font-size: 20px;
          }

          .head{
          border: 0px;
          text-align: left;
          font-weight: bold;
          padding:0px;
          }

          .small{
          border: 0px;
          text-align: left;
          font-weight: bold;
          font-size: 15px;
          padding:0px;
          }

          td {
          border: 1px solid black;
          text-align: left;
          padding:10px;
          }

          .incident {
          border-spacing: 0px;
          border-collapse: collapse;
          page-break-inside: avoid;
          }

          div{
          white-space:nowrap;
          }

          .wrapper {
          text-align: center;
          display: block;
          padding: 0 10%;
          }

          @media print {
          .incident{
          page-break-inside: avoid;
          page-break-after: always;
          }
          div.body {
          page-break-inside: avoid;
          }
          }
        </style>
      </head>
      <body>
        <table class="wrapper">
          <thead>
            <tr>
              <th style="text-align:center;" colspan="3">
                Supplementary Homicide Report
              </th>
            </tr>
            <tr>
              <td class="small">
                <xsl:value-of select="UcrReports/@agency" />
              </td>
              <td class="small">
                <xsl:value-of select="UcrReports/@ori" />
              </td>
              <td class="small">
                <xsl:value-of select="concat(UcrReports/@year, ' ', UcrReports/@month)" />
              </td>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td colspan="3">
                <xsl:for-each select="UcrReports/SHR/INCIDENTS/INCIDENT">
                  <table class="incident">
                    <xsl:variable name="iposition" select="position()" />
                    <tbody>
                      <tr>
                        <td class="head">
                          <xsl:if test="MANSLAUGTERNEGLIGENT='1'">
                            <p>Manslaughter by Negligence</p>
                            <p class="small">
                              Do not list traffic fatalities, accidental deaths, or death due to negligence of the victim. List below all other
                              negligent manslaughters, regardless of prosecutive action taken.
                            </p>
                          </xsl:if>
                          <xsl:if test="MANSLAUGTERNOTNEGLIGENT='1'">
                            <p>Murder and Nonnegligent Manslaughter</p>
                            <p class="small">
                              List below for each murder and nonnegligent homicide and / or justifiable homicide shown in item 1a of the monthly
                              Return A. In addition, for justifiable homicide list all justifiable killings of felons by a citizen or by a peace
                              officer in the line of duty. A brief explanation in the circumstances field regarding unfounded homicide offenses
                              will aid the natural Uniform Crime Reporting Program in editing the reports.
                            </p>
                          </xsl:if>
                        </td>
                      </tr>
                      <tr>
                        <td class="head">
                          Incident #: <xsl:value-of select="SEQUENCENUMBER" />
                        </td>
                      </tr>
                      <tr>
                        <td class="head">
                          Situation :
                          <xsl:choose>
                            <xsl:when test="SITUATION='A'">A (Single Victim/Single Offender)</xsl:when>
                            <xsl:when test="SITUATION='B'">B (Single Victim/Unknown Offender or Offenders)</xsl:when>
                            <xsl:when test="SITUATION='C'">C (Single Victim/Multiple Offenders)</xsl:when>
                            <xsl:when test="SITUATION='D'">D (Multiple Victims/Single Offender)</xsl:when>
                            <xsl:when test="SITUATION='E'">E (Multiple Victims/Multiple Offenders)</xsl:when>
                            <xsl:when test="SITUATION='F'">F (Multiple Victims/Unknown Offender or Offenders)</xsl:when>
                          </xsl:choose>
                        </td>
                      </tr>
                      <xsl:for-each select="VICTIMS/VICTIM">
                        <tr>
                          <td class="head">
                            Victim # : <xsl:value-of select="position()" />
                          </td>
                        </tr>
                        <tr>
                          <td class="head">
                            <div>
                              Age : <xsl:value-of select="AGE" />
                            </div>
                          </td>
                          <td class="head">
                            <div>
                              Sex : <xsl:value-of select="SEX" />
                            </div>
                          </td>
                        </tr>
                        <tr>
                          <td class="head">
                            Ethnicity :
                            <xsl:choose>
                              <xsl:when test="ETHNICITY='H'">H (Hispanic or Latino)</xsl:when>
                              <xsl:when test="ETHNICITY='N'">N (Non-Hispanic or Non-Latino)</xsl:when>
                              <xsl:when test="ETHNICITY='U'">U (Unknown)</xsl:when>
                            </xsl:choose>
                          </td>
                          <td class="head">
                            <div>
                              Race :
                              <xsl:choose>
                                <xsl:when test="RACE='W'">W (White)</xsl:when>
                                <xsl:when test="RACE='B'">B (Black)</xsl:when>
                                <xsl:when test="RACE='I'">I (American Indian or Alaskan Native)</xsl:when>
                                <xsl:when test="RACE='A'">A (Asian)</xsl:when>
                                <xsl:when test="RACE='U'">U (Unknown)</xsl:when>
                                <xsl:when test="RACE='P'">P (Native Hawaiian or Other Pacific Islander)</xsl:when>
                              </xsl:choose>
                            </div>
                          </td>
                        </tr>
                        <tr>
                          <td style="text-align:center;border:0px">
                            Offenders for Victim #: <xsl:value-of select="position()" />
                          </td>
                        </tr>

                        <xsl:for-each select="OFFENDERS/OFFENDER">
                          <div class="body">
                            <tr>
                              <td style="text-align:left;border:0px;">
                                Offender # : <xsl:value-of select="position()" />
                              </td>
                            </tr>
                            <tr>

                              <td>
                                *Age : <xsl:value-of select="AGE" />
                              </td>

                              <td>
                                Sex :
                                <xsl:choose>
                                  <xsl:when test="SEX='M'">M (Male)</xsl:when>
                                  <xsl:when test="SEX='F'">F (Female)</xsl:when>
                                  <xsl:when test="SEX='U'">U (Unknown)</xsl:when>
                                </xsl:choose>
                              </td>

                              <td>
                                Race :
                                <xsl:choose>
                                  <xsl:when test="RACE='W'">W (White)</xsl:when>
                                  <xsl:when test="RACE='B'">B (Black)</xsl:when>
                                  <xsl:when test="RACE='I'">I (American Indian or Alaskan Native)</xsl:when>
                                  <xsl:when test="RACE='A'">A (Asian)</xsl:when>
                                  <xsl:when test="RACE='U'">U (Unknown)</xsl:when>
                                  <xsl:when test="RACE='P'">P (Native Hawaiian or Other Pacific Islander)</xsl:when>
                                </xsl:choose>
                              </td>
                            </tr>
                            <tr>
                              <td>
                                Ethnicity :
                                <xsl:choose>
                                  <xsl:when test="ETHNICITY='H'">H (Hispanic or Latino)</xsl:when>
                                  <xsl:when test="ETHNICITY='N'">N (Non-Hispanic or Non-Latino)</xsl:when>
                                  <xsl:when test="ETHNICITY='U'">U (Unknown)</xsl:when>
                                </xsl:choose>
                              </td>
                              <td>
                                Weapon Used :
                                <xsl:choose>
                                  <xsl:when test="WEAPONUSED='11'">11 (Firearm)</xsl:when>
                                  <xsl:when test="WEAPONUSED='12'">12 (Handgun-pistol, revolver)</xsl:when>
                                  <xsl:when test="WEAPONUSED='13'">13 (Rifle)</xsl:when>
                                  <xsl:when test="WEAPONUSED='14'">14 (Shotgun)</xsl:when>
                                  <xsl:when test="WEAPONUSED='15'">15 (Other gun)</xsl:when>
                                  <xsl:when test="WEAPONUSED='20'">20 (Knife or cutting instrument)</xsl:when>
                                  <xsl:when test="WEAPONUSED='30'">30 (Blunt object-hammer, club)</xsl:when>
                                  <xsl:when test="WEAPONUSED='40'">40 (Personal weapons)</xsl:when>
                                  <xsl:when test="WEAPONUSED='50'">50 (Poison)</xsl:when>
                                  <xsl:when test="WEAPONUSED='55'">55 (Pushed or thrown out window)</xsl:when>
                                  <xsl:when test="WEAPONUSED='60'">60 (Explosives)</xsl:when>
                                  <xsl:when test="WEAPONUSED='65'">65 (Fire)</xsl:when>
                                  <xsl:when test="WEAPONUSED='70'">70 (Narcotics and drugs)</xsl:when>
                                  <xsl:when test="WEAPONUSED='75'">75 (Drowning)</xsl:when>
                                  <xsl:when test="WEAPONUSED='80'">80 (Strangulation-include hanging)</xsl:when>
                                  <xsl:when test="WEAPONUSED='85'">85 (Asphyxiation)</xsl:when>
                                  <xsl:when test="WEAPONUSED='90'">90 (Other-type of weapon)</xsl:when>
                                </xsl:choose>
                              </td>
                              <td>
                                Relationship :
                                <xsl:choose>
                                  <xsl:when test="RELATIONSHIP='HU'">HU (Husband)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='WI'">WI (Wife)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='CH'">CH (Common-Law Husband)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='CW'">CW (Common-Law Wife)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='MO'">MO (Mother)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='FA'">FA (Father)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='SO'">SO (Son)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='DA'">DA (Daughter)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='BR'">BR (Brother)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='SI'">SI (Sister)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='IL'">IL (In-Law)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='SF'">SF (Stepfather)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='SM'">SM (Stepmother)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='SS'">SS (Stepson)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='SD'">SD (Stepdaughter)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='OF'">OF (Other family)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='NE'">NE (Neighbor)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='AQ'">AQ (Acquaintance)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='BF'">BF (Boyfriend)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='GF'">GF (Girlfriend)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='XH'">XH (Ex-Husband)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='XW'">XW (Ex-Wife)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='EE'">EE (Employee)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='ER'">ER (Employer)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='FR'">FR (Friend)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='HO'">HO (Homosexual Relationship)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='OK'">OK (Other-known to victim)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='ST'">ST (Stranger)</xsl:when>
                                  <xsl:when test="RELATIONSHIP='UN'">UN (Unknown Relationship)</xsl:when>
                                </xsl:choose>
                              </td>
                            </tr>
                            <tr>
                              <td>
                                Circumstance :
                                <xsl:choose>
                                  <xsl:when test="CIRCUMSTANCE='01'">01 (Rape)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='02'">02 (Robbery)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='05'">05 (Burglary)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='06'">06 (Larceny-theft)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='07'">07 (Motor Vehicle Theft)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='09'">09 (Arson)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='10'">10 (Prostitution and Commercialized Vice)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='17'">17 (Other Sex Offense)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='32'">32 (Abortion)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='18'">18 (Narcotic Drug Laws)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='19'">19 (Gambling)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='26'">26 (Other-not specified)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='30'">30 (Human Trafficking/Commercial Sex Acts)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='31'">31 (Human Trafficking/Involuntary Serviture)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='40'">40 (Lover's Triangle)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='41'">41 (Child Killed by Babysitter)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='42'">42 (Brawl Due to Influence of Alcohol)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='43'">43 (Brawl Due to Influence of Narcotics)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='44'">44 (Argument Over Money or Property)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='45'">45 (Other Arguments)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='46'">46 (Gangland Killings)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='47'">47 (Juvenile Gang Killings)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='48'">48 (Institutional Killings)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='49'">49 (Sniper Attack)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='60'">60 (Other)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='70'">70 (All suspected felony types)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='80'">80 (Felon killed by private citizen)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='81'">81 (Felon killed by police)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='99'">99 (All instances where facts provided do not permit determination of circumstances)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='50'">50 (Victim shot in hunting accident)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='51'">51 (Gun-cleaning death-other than self-inflicted)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='52'">52 (Children playing with gun)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='53'">53 (Other negligent handling of gun which results in death of another)</xsl:when>
                                  <xsl:when test="CIRCUMSTANCE='59'">59 (All other manslaughter by negligence except traffic deaths)</xsl:when>
                                </xsl:choose>
                              </td>
                              <td>
                                Subcircumstance :
                                <xsl:choose>
                                  <xsl:when test="SUBCIRCUMSTANCE='A'">A (Felon attacked police officer)</xsl:when>
                                  <xsl:when test="SUBCIRCUMSTANCE='B'">B (Felon attacked fellow police officer)</xsl:when>
                                  <xsl:when test="SUBCIRCUMSTANCE='C'">C (Felon attacked a civilian)</xsl:when>
                                  <xsl:when test="SUBCIRCUMSTANCE='D'">D (Felon attempted flight from a crime)</xsl:when>
                                  <xsl:when test="SUBCIRCUMSTANCE='E'">E (Felon killed in commission of a crime)</xsl:when>
                                  <xsl:when test="SUBCIRCUMSTANCE='F'">F (Felon resisted arrest)</xsl:when>
                                  <xsl:when test="SUBCIRCUMSTANCE='G'">G (Not enough information to determine)</xsl:when>
                                </xsl:choose>
                              </td>
                              <td>
                              </td>
                            </tr>
                            <br />
                          </div>
                        </xsl:for-each>
                      </xsl:for-each>
                    </tbody>
                  </table>
                </xsl:for-each>
              </td>
            </tr>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>