<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          .no-border {
          border: none;
          }
          body {
          display: -webkit-flex;
          display: flex;
          font-size: 10px;
          }
          .page-wrapper {
          width:210mm;
          margin:auto;
          }
          th, td {
          border: 1px solid black;
          }
          table {
          border-spacing: 0px;
          border-collapse: separate;
          }
          .rowheader {
          text-align: left;
          }
          .title{
          border:0px;
          font-size:20px;
          }
          .page-content {
          }
          .page-content > table {
          width: 100%;
          padding: 10px 0;
          }
          .data-table-container {
          display: -webkit-flex;
          display: flex;
          -webkit-flex-direction: column;
          flex-direction: column;
          }
          .data-table-container > table {
          margin:auto;
          padding: 10px 0;
          width:100%;
          }
          .dictionary-container {
          max-height:340mm;
          min-width: 190mm;
          display: -webkit-flex;
          display: flex;
          -webkit-flex-direction: column;
          flex-direction: column;
          -webkit-flex-wrap: wrap;
          flex-wrap: wrap;
          margin:10px;
          }
          .dictionary {
          display: -webkit-flex;
          display: flex;
          -webkit-justify-content: center;
          justify-content: center;
          -webkit-flex-direction: column;
          flex-direction: column;
          padding: 5px 0;
          }
          .dictionary-header {
          display: block;
          }
          .dictionary-key {
          padding: 0 10px 0 0;
          display: inline-block;
          width:30px;
          text-align:right;
          vertical-align:top;
          }
          .dictionary-value {
          display: inline-block;
          width:200px;
          }
          .checkbox-container {
          pointer-events:none;
          }
          .centered {
          text-align: center;
          }
          .rowheader {
          text-align: left;
          }
          .ages {
          background-color: yellow;
          }
          .table-pad-bot {
          padding-bottom: 10px;
          }
          .title{
          border:0px;
          font-size:20px;
          }
        </style>
      </head>
      <body>
        <div class="page-wrapper">
          <table>
            <!--Page header-->
            <thead>
              <tr>
                <th colspan="2">Supplementary Homicide Report</th>
              </tr>
              <tr>
                <th>
                  <xsl:value-of select="concat(UcrReports/@ori, ' ', UcrReports/@agency)" />
                </th>
                <th>
                  <xsl:choose>
                    <xsl:when test="UcrReports/@month=1">January </xsl:when>
                    <xsl:when test="UcrReports/@month=2">February </xsl:when>
                    <xsl:when test="UcrReports/@month=3">March </xsl:when>
                    <xsl:when test="UcrReports/@month=4">April </xsl:when>
                    <xsl:when test="UcrReports/@month=5">May </xsl:when>
                    <xsl:when test="UcrReports/@month=6">June </xsl:when>
                    <xsl:when test="UcrReports/@month=7">July </xsl:when>
                    <xsl:when test="UcrReports/@month=8">August </xsl:when>
                    <xsl:when test="UcrReports/@month=9">September </xsl:when>
                    <xsl:when test="UcrReports/@month=10">October </xsl:when>
                    <xsl:when test="UcrReports/@month=11">November </xsl:when>
                    <xsl:when test="UcrReports/@month=12">December </xsl:when>
                  </xsl:choose>
                  <xsl:value-of select="UcrReports/@year" />
                </th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td colspan="2">
                  <!--Page content-->
                  <div class="page-content">
                    <!--Negligent homicide table-->
                    <xsl:if test="boolean(UcrReports/SHR/INCIDENTS/INCIDENT[MANSLAUGHTERNEGLIGENT=1])">
                      <table>
                        <thead>
                          <tr>
                            <th>Manslaughter by Negligence</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr>
                            <td class="foreword">
                              Do not list traffic fatalities, accidental deaths, or death due to negligence of the victim. List below all other
                              negligent manslaughters, regardless of prosecutive action taken.
                            </td>
                          </tr>
                          <tr>
                            <td>
                              <div class="data-table-container">
                                <table>
                                  <thead>
                                    <tr>
                                      <th rowspan="2">Incident Sequence Number</th>
                                      <th rowspan="2">Situation</th>
                                      <th colspan="5">Victim</th>
                                      <th colspan="9">Offender</th>
                                    </tr>
                                    <tr>
                                      <th>#</th>
                                      <th>Age</th>
                                      <th>Sex</th>
                                      <th>Race</th>
                                      <th>Ethnicity</th>
                                      <th>#</th>
                                      <th>Age</th>
                                      <th>Sex</th>
                                      <th>Race</th>
                                      <th>Ethnicity</th>
                                      <th>Weapon Used</th>
                                      <th>Relationship</th>
                                      <th>Circumstance</th>
                                      <th>Subcircumstance</th>
                                    </tr>
                                  </thead>
                                  <tbody>
                                    <xsl:for-each select="UcrReports/SHR/INCIDENTS/INCIDENT[MANSLAUGHTERNEGLIGENT=1]">
                                      <tr>
                                        <td
                                          rowspan="{1 + count(VICTIMS/VICTIM) + count(VICTIMS/VICTIM/OFFENDERS/OFFENDER)}">
                                          <xsl:value-of select="SEQUENCENUMBER" />
                                        </td>
                                        <td
                                          rowspan="{1 + count(VICTIMS/VICTIM) + count(VICTIMS/VICTIM/OFFENDERS/OFFENDER)}">
                                          <xsl:value-of select="SITUATION" />
                                        </td>
                                      </tr>
                                      <xsl:for-each select="VICTIMS/VICTIM">
                                        <tr>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="position()" />
                                          </td>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="AGE" />
                                          </td>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="SEX" />
                                          </td>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="RACE" />
                                          </td>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="ETHNICITY" />
                                          </td>
                                        </tr>
                                        <xsl:for-each select="OFFENDERS/OFFENDER">
                                          <tr>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="position()" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="AGE" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="SEX" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="RACE" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="ETHNICITY" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="WEAPONUSED" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="RELATIONSHIP" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="CIRCUMSTANCE" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="SUBCIRCUMSTANCE" />
                                              <xsl:if test="not(SUBCIRCUMSTANCE)">
                                                N/A
                                              </xsl:if>
                                            </td>
                                          </tr>
                                        </xsl:for-each>
                                      </xsl:for-each>
                                    </xsl:for-each>
                                  </tbody>
                                </table>
                              </div>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </xsl:if>

                    <!--Nonnegligent homicide table-->
                    <xsl:if test="boolean(UcrReports/SHR/INCIDENTS/INCIDENT[MANSLAUGHTERNEGLIGENT=0])">
                      <table>
                        <thead>
                          <tr>
                            <th>Murder and Nonnegligent Manslaughter</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr>
                            <td class="foreword">
                              List below for each murder and nonnegligent homicide and / or justifiable homicide shown in item 1a of the monthly
                              Return A. In addition, for justifiable homicide list all justifiable killings of felons by a citizen or by a peace
                              officer in the line of duty. A brief explanation in the circumstances field regarding unfounded homicide offenses
                              will aid the natural Uniform Crime Reporting Program in editing the reports.
                            </td>
                          </tr>
                          <tr>
                            <td>
                              <div class="data-table-container">
                                <table>
                                  <thead>
                                    <tr>
                                      <th rowspan="2">Incident Sequence Number</th>
                                      <th rowspan="2">Situation</th>
                                      <th colspan="5">Victim</th>
                                      <th colspan="9">Offender</th>
                                    </tr>
                                    <tr>
                                      <th>#</th>
                                      <th>Age</th>
                                      <th>Sex</th>
                                      <th>Race</th>
                                      <th>Ethnicity</th>
                                      <th>#</th>
                                      <th>Age</th>
                                      <th>Sex</th>
                                      <th>Race</th>
                                      <th>Ethnicity</th>
                                      <th>Weapon Used</th>
                                      <th>Relationship</th>
                                      <th>Circumstance</th>
                                      <th>Subcircumstance</th>
                                    </tr>
                                  </thead>
                                  <tbody>
                                    <xsl:for-each select="UcrReports/SHR/INCIDENTS/INCIDENT[MANSLAUGHTERNEGLIGENT=0]">
                                      <tr>
                                        <td
                                          rowspan="{1 + count(VICTIMS/VICTIM) + count(VICTIMS/VICTIM/OFFENDERS/OFFENDER)}">
                                          <xsl:value-of select="SEQUENCENUMBER" />
                                        </td>
                                        <td
                                          rowspan="{1 + count(VICTIMS/VICTIM) + count(VICTIMS/VICTIM/OFFENDERS/OFFENDER)}">
                                          <xsl:value-of select="SITUATION" />
                                        </td>
                                      </tr>
                                      <xsl:for-each select="VICTIMS/VICTIM">
                                        <tr>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="position()" />
                                          </td>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="AGE" />
                                          </td>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="SEX" />
                                          </td>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="RACE" />
                                          </td>
                                          <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                            <xsl:value-of select="ETHNICITY" />
                                          </td>
                                        </tr>
                                        <xsl:for-each select="OFFENDERS/OFFENDER">
                                          <tr>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="position()" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="AGE" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="SEX" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="RACE" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="ETHNICITY" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="WEAPONUSED" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="RELATIONSHIP" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="CIRCUMSTANCE" />
                                            </td>
                                            <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                              <xsl:value-of select="SUBCIRCUMSTANCE" />
                                              <xsl:if test="not(SUBCIRCUMSTANCE)">
                                                N/A
                                              </xsl:if>
                                            </td>
                                          </tr>
                                        </xsl:for-each>
                                      </xsl:for-each>
                                    </xsl:for-each>
                                  </tbody>
                                </table>
                              </div>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </xsl:if>
                  </div>
                </td>
              </tr>
              <tr>
                <td colspan="2">
                  <!--Legend-->
                  <div class="dictionary-container">
                    <div class="dictionary">
                      <span class="dictionary-header">Age of Victim/Offender</span>

                      <div>
                        <span class="dictionary-key">00</span>
                        <span class="dictionary-value">Age unknown</span>
                      </div>

                      <div>
                        <span class="dictionary-key">01-98</span>
                        <span class="dictionary-value">Corresponding age of individual</span>
                      </div>

                      <div>
                        <span class="dictionary-key">99</span>
                        <span class="dictionary-value">Age 99 or older</span>
                      </div>

                      <div>
                        <span class="dictionary-key">BB</span>
                        <span class="dictionary-value">Baby - 1 week to 1 year of age</span>
                      </div>

                      <div>
                        <span class="dictionary-key">NB</span>
                        <span class="dictionary-value">Newborn - less than 1 week of age (include abandoned infant)</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Circumstance</span>

                      <div>
                        <span class="dictionary-key">02</span>
                        <span class="dictionary-value">Rape</span>
                      </div>

                      <div>
                        <span class="dictionary-key">03</span>
                        <span class="dictionary-value">Robbery</span>
                      </div>

                      <div>
                        <span class="dictionary-key">05</span>
                        <span class="dictionary-value">Burglary</span>
                      </div>

                      <div>
                        <span class="dictionary-key">06</span>
                        <span class="dictionary-value">Larceny-theft</span>
                      </div>

                      <div>
                        <span class="dictionary-key">07</span>
                        <span class="dictionary-value">Motor Vehicle Theft</span>
                      </div>

                      <div>
                        <span class="dictionary-key">09</span>
                        <span class="dictionary-value">Arson</span>
                      </div>

                      <div>
                        <span class="dictionary-key">10</span>
                        <span class="dictionary-value">Prostitution and Commercialized Vice</span>
                      </div>

                      <div>
                        <span class="dictionary-key">17</span>
                        <span class="dictionary-value">Other Sex Offense</span>
                      </div>

                      <div>
                        <span class="dictionary-key">18</span>
                        <span class="dictionary-value">Narcotic Drug Laws</span>
                      </div>

                      <div>
                        <span class="dictionary-key">19</span>
                        <span class="dictionary-value">Gambling</span>
                      </div>

                      <div>
                        <span class="dictionary-key">26</span>
                        <span class="dictionary-value">Other - Not Specified</span>
                      </div>

                      <div>
                        <span class="dictionary-key">30</span>
                        <span class="dictionary-value">Human Trafficking/Commercial Sex Acts</span>
                      </div>

                      <div>
                        <span class="dictionary-key">31</span>
                        <span class="dictionary-value">Human Trafficking/Involuntary Servitude</span>
                      </div>

                      <div>
                        <span class="dictionary-key">32</span>
                        <span class="dictionary-value">Abortion</span>
                      </div>

                      <div>
                        <span class="dictionary-key">40</span>
                        <span class="dictionary-value">Lover's Triangle</span>
                      </div>

                      <div>
                        <span class="dictionary-key">41</span>
                        <span class="dictionary-value">Child Killed by Babysitter</span>
                      </div>

                      <div>
                        <span class="dictionary-key">42</span>
                        <span class="dictionary-value">Brawl Due to Influence of Alcohol</span>
                      </div>

                      <div>
                        <span class="dictionary-key">43</span>
                        <span class="dictionary-value">Brawl Due to Influence of Narcotics</span>
                      </div>

                      <div>
                        <span class="dictionary-key">44</span>
                        <span class="dictionary-value">Argument Over Money or Property</span>
                      </div>

                      <div>
                        <span class="dictionary-key">45</span>
                        <span class="dictionary-value">Other Arguments</span>
                      </div>

                      <div>
                        <span class="dictionary-key">46</span>
                        <span class="dictionary-value">Gangland Killings</span>
                      </div>

                      <div>
                        <span class="dictionary-key">47</span>
                        <span class="dictionary-value">Juvenile Gang Killings</span>
                      </div>

                      <div>
                        <span class="dictionary-key">48</span>
                        <span class="dictionary-value">Institutional Killings</span>
                      </div>

                      <div>
                        <span class="dictionary-key">49</span>
                        <span class="dictionary-value">Sniper Attack</span>
                      </div>

                      <div>
                        <span class="dictionary-key">50</span>
                        <span class="dictionary-value">Victim Shot in Hunting Accident</span>
                      </div>

                      <div>
                        <span class="dictionary-key">51</span>
                        <span class="dictionary-value">Gun-Cleaning Death - Other Than Self-Inflicted</span>
                      </div>

                      <div>
                        <span class="dictionary-key">52</span>
                        <span class="dictionary-value">Children Playing With Gun</span>
                      </div>

                      <div>
                        <span class="dictionary-key">53</span>
                        <span class="dictionary-value">Other Negligent Handling of Gun Which Results in Death of Another</span>
                      </div>

                      <div>
                        <span class="dictionary-key">59</span>
                        <span class="dictionary-value">All Other Manslaughter by Negligence Except Traffic Deaths</span>
                      </div>

                      <div>
                        <span class="dictionary-key">60</span>
                        <span class="dictionary-value">Other</span>
                      </div>

                      <div>
                        <span class="dictionary-key">70</span>
                        <span class="dictionary-value">All Suspected Felony Types</span>
                      </div>

                      <div>
                        <span class="dictionary-key">80</span>
                        <span class="dictionary-value">Felon Killed by Private Citizen</span>
                      </div>

                      <div>
                        <span class="dictionary-key">81</span>
                        <span class="dictionary-value">Felon Killed by Police</span>
                      </div>

                      <div>
                        <span class="dictionary-key">99</span>
                        <span class="dictionary-value">All Instances Where Facts Provided Do Not Permit Determination of Circumstances</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Ethnicity</span>

                      <div>
                        <span class="dictionary-key">H</span>
                        <span class="dictionary-value">Hispanic Origin</span>
                      </div>

                      <div>
                        <span class="dictionary-key">N</span>
                        <span class="dictionary-value">Not of Hispanic Origin</span>
                      </div>

                      <div>
                        <span class="dictionary-key">U</span>
                        <span class="dictionary-value">Unknown</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Race</span>

                      <div>
                        <span class="dictionary-key">W</span>
                        <span class="dictionary-value">White</span>
                      </div>

                      <div>
                        <span class="dictionary-key">B</span>
                        <span class="dictionary-value">Black</span>
                      </div>

                      <div>
                        <span class="dictionary-key">I</span>
                        <span class="dictionary-value">American Indian or Alaskan Native</span>
                      </div>

                      <div>
                        <span class="dictionary-key">A</span>
                        <span class="dictionary-value">Asian or Pacific Islander</span>
                      </div>

                      <div>
                        <span class="dictionary-key">U</span>
                        <span class="dictionary-value">Unknown</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Relationship</span>

                      <div>
                        <span class="dictionary-key">AQ</span>
                        <span class="dictionary-value">Acquaintance</span>
                      </div>

                      <div>
                        <span class="dictionary-key">BF</span>
                        <span class="dictionary-value">Boyfriend</span>
                      </div>

                      <div>
                        <span class="dictionary-key">BR</span>
                        <span class="dictionary-value">Brother</span>
                      </div>

                      <div>
                        <span class="dictionary-key">CH</span>
                        <span class="dictionary-value">Common-Law Husband</span>
                      </div>

                      <div>
                        <span class="dictionary-key">CW</span>
                        <span class="dictionary-value">Common-Law Wife</span>
                      </div>

                      <div>
                        <span class="dictionary-key">DA</span>
                        <span class="dictionary-value">Daughter</span>
                      </div>

                      <div>
                        <span class="dictionary-key">EE</span>
                        <span class="dictionary-value">Employee</span>
                      </div>

                      <div>
                        <span class="dictionary-key">ER</span>
                        <span class="dictionary-value">Employer</span>
                      </div>

                      <div>
                        <span class="dictionary-key">FA</span>
                        <span class="dictionary-value">Father</span>
                      </div>

                      <div>
                        <span class="dictionary-key">FR</span>
                        <span class="dictionary-value">Friend</span>
                      </div>

                      <div>
                        <span class="dictionary-key">GF</span>
                        <span class="dictionary-value">Girlfriend</span>
                      </div>

                      <div>
                        <span class="dictionary-key">HO</span>
                        <span class="dictionary-value">Homosexual Relationship</span>
                      </div>

                      <div>
                        <span class="dictionary-key">HU</span>
                        <span class="dictionary-value">Husband</span>
                      </div>

                      <div>
                        <span class="dictionary-key">IL</span>
                        <span class="dictionary-value">In-Law</span>
                      </div>

                      <div>
                        <span class="dictionary-key">MO</span>
                        <span class="dictionary-value">Mother</span>
                      </div>

                      <div>
                        <span class="dictionary-key">NE</span>
                        <span class="dictionary-value">Neighbor</span>
                      </div>

                      <div>
                        <span class="dictionary-key">OF</span>
                        <span class="dictionary-value">Other Family</span>
                      </div>

                      <div>
                        <span class="dictionary-key">OK</span>
                        <span class="dictionary-value">Other Known to Victim</span>
                      </div>

                      <div>
                        <span class="dictionary-key">SD</span>
                        <span class="dictionary-value">Stepdaughter</span>
                      </div>

                      <div>
                        <span class="dictionary-key">SF</span>
                        <span class="dictionary-value">Stepfather</span>
                      </div>

                      <div>
                        <span class="dictionary-key">SI</span>
                        <span class="dictionary-value">Sister</span>
                      </div>

                      <div>
                        <span class="dictionary-key">SM</span>
                        <span class="dictionary-value">Stepmother</span>
                      </div>

                      <div>
                        <span class="dictionary-key">SO</span>
                        <span class="dictionary-value">Son</span>
                      </div>

                      <div>
                        <span class="dictionary-key">SS</span>
                        <span class="dictionary-value">Stepson</span>
                      </div>

                      <div>
                        <span class="dictionary-key">ST</span>
                        <span class="dictionary-value">Stranger</span>
                      </div>

                      <div>
                        <span class="dictionary-key">UN</span>
                        <span class="dictionary-value">Unknown Relationship</span>
                      </div>

                      <div>
                        <span class="dictionary-key">WI</span>
                        <span class="dictionary-value">Wife</span>
                      </div>

                      <div>
                        <span class="dictionary-key">XH</span>
                        <span class="dictionary-value">Ex-Husband</span>
                      </div>

                      <div>
                        <span class="dictionary-key">XW</span>
                        <span class="dictionary-value">Ex-Wife</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Sex</span>

                      <div>
                        <span class="dictionary-key">M</span>
                        <span class="dictionary-value">Male</span>
                      </div>

                      <div>
                        <span class="dictionary-key">F</span>
                        <span class="dictionary-value">Female</span>
                      </div>

                      <div>
                        <span class="dictionary-key">U</span>
                        <span class="dictionary-value">Unknown</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Situation</span>

                      <div>
                        <span class="dictionary-key">A</span>
                        <span class="dictionary-value">Single Victim/Single Offender</span>
                      </div>

                      <div>
                        <span class="dictionary-key">B</span>
                        <span class="dictionary-value">Single Victim/Unknown Offender(s)</span>
                      </div>

                      <div>
                        <span class="dictionary-key">C</span>
                        <span class="dictionary-value">Single Victim/Multiple Offenders</span>
                      </div>

                      <div>
                        <span class="dictionary-key">D</span>
                        <span class="dictionary-value">Multiple Victims/Single Offender</span>
                      </div>

                      <div>
                        <span class="dictionary-key">E</span>
                        <span class="dictionary-value">Multiple Victims/Multiple Offenders</span>
                      </div>

                      <div>
                        <span class="dictionary-key">F</span>
                        <span class="dictionary-value">Multiple Victims/Unknown Offender(s)</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Subcircumstance</span>

                      <div>
                        <span class="dictionary-key">A</span>
                        <span class="dictionary-value">Felon Attacked Police Officer</span>
                      </div>

                      <div>
                        <span class="dictionary-key">B</span>
                        <span class="dictionary-value">Felon Attacked Fellow Police Officer</span>
                      </div>

                      <div>
                        <span class="dictionary-key">C</span>
                        <span class="dictionary-value">Felon Attacked a Civilian</span>
                      </div>

                      <div>
                        <span class="dictionary-key">D</span>
                        <span class="dictionary-value">Felon Attempted Flight From a Crime</span>
                      </div>

                      <div>
                        <span class="dictionary-key">E</span>
                        <span class="dictionary-value">Felon Killed in Commission of a Crime</span>
                      </div>

                      <div>
                        <span class="dictionary-key">F</span>
                        <span class="dictionary-value">Felon Resisted Arrest</span>
                      </div>

                      <div>
                        <span class="dictionary-key">G</span>
                        <span class="dictionary-value">Not Enough Information to Determine</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Weapon</span>

                      <div>
                        <span class="dictionary-key">11</span>
                        <span class="dictionary-value">Firearm</span>
                      </div>

                      <div>
                        <span class="dictionary-key">12</span>
                        <span class="dictionary-value">Handgun</span>
                      </div>

                      <div>
                        <span class="dictionary-key">13</span>
                        <span class="dictionary-value">Rifle</span>
                      </div>

                      <div>
                        <span class="dictionary-key">14</span>
                        <span class="dictionary-value">Shotgun</span>
                      </div>

                      <div>
                        <span class="dictionary-key">15</span>
                        <span class="dictionary-value">Other gun</span>
                      </div>

                      <div>
                        <span class="dictionary-key">20</span>
                        <span class="dictionary-value">Knife or Cutting Instrument</span>
                      </div>

                      <div>
                        <span class="dictionary-key">30</span>
                        <span class="dictionary-value">Blunt Object</span>
                      </div>

                      <div>
                        <span class="dictionary-key">40</span>
                        <span class="dictionary-value">Personal Weapons</span>
                      </div>

                      <div>
                        <span class="dictionary-key">50</span>
                        <span class="dictionary-value">Poison</span>
                      </div>

                      <div>
                        <span class="dictionary-key">55</span>
                        <span class="dictionary-value">Pushed or Thrown Out Window</span>
                      </div>

                      <div>
                        <span class="dictionary-key">60</span>
                        <span class="dictionary-value">Explosives</span>
                      </div>

                      <div>
                        <span class="dictionary-key">65</span>
                        <span class="dictionary-value">Fire</span>
                      </div>

                      <div>
                        <span class="dictionary-key">70</span>
                        <span class="dictionary-value">Narcotics and Drugs</span>
                      </div>

                      <div>
                        <span class="dictionary-key">75</span>
                        <span class="dictionary-value">Drowning</span>
                      </div>

                      <div>
                        <span class="dictionary-key">80</span>
                        <span class="dictionary-value">Strangulation</span>
                      </div>

                      <div>
                        <span class="dictionary-key">85</span>
                        <span class="dictionary-value">Asphyxiation</span>
                      </div>

                      <div>
                        <span class="dictionary-key">90</span>
                        <span class="dictionary-value">Other Weapon</span>
                      </div>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
            <!--Page footer-->
            <tfoot>
            </tfoot>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>