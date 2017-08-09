<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          body {
          font-size: 10px;
          }
          th, td {
          border: 1px solid black;
          }
          th {
          text-align:center;
          }
          table {
          border-spacing: 0px;
          border-collapse: separate;
          }
          td {
          text-align:right;
          }
          .rowheader {
          text-align: left;
          }
          .title{
          border:0px;
          font-size:20px;
          }
        </style>
      </head>
      <body>
        <table class="page-wrapper">
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
                <!--Legend-->
                <!--Age
                Circumstance
                Ethnicity
                Race
                Relationship
                Sex
                Subcircumstance
                Weapon Used-->

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
                          <xsl:for-each select="UcrReports/SHR/INCIDENTS/INCIDENT[MANSLAUGHTERNEGLIGENT=1]">
                            <div class="incident-wrapper">
                              <div class="incident-header">
                                <span class="incident-number">
                                  Incident #<xsl:value-of select="SEQUENCENUMBER" />
                                </span>
                                <span class="incident-situation">
                                  <xsl:choose>
                                    <xsl:when test="SITUATION='A'">Situation A - Single Victim/Single Offender</xsl:when>
                                    <xsl:when test="SITUATION='B'">Situation B - Single Victim/Unknown Offender or Offenders</xsl:when>
                                    <xsl:when test="SITUATION='C'">Situation C - Single Victim/Multiple Offenders</xsl:when>
                                    <xsl:when test="SITUATION='D'">Situation D - Multiple Victims/Single Offender</xsl:when>
                                    <xsl:when test="SITUATION='E'">Situation E - Multiple Victims/Multiple Offenders</xsl:when>
                                    <xsl:when test="SITUATION='F'">Situation F - Multiple Victims/Unknown Offender or Offenders</xsl:when>
                                  </xsl:choose>
                                </span>
                              </div>
                              <xsl:for-each select="VICTIMS/VICTIM">
                                <div class="related-people-wrapper">
                                  <div class="victim-panel">
                                    <div class="victim-card">
                                      <span class="victim-sequence-number">
                                        Victim #<xsl:value-of select="position()" />
                                      </span>
                                      <span class="label">Age:</span>
                                      <span class="code">
                                        <xsl:value-of select="AGE" />
                                      </span>

                                      <span class="label">Sex:</span>
                                      <span class="code">
                                        <xsl:value-of select="SEX" />
                                      </span>

                                      <span class="label">Race:</span>
                                      <span class="code">
                                        <xsl:value-of select="RACE" />
                                      </span>

                                      <span class="label">Ethnicity:</span>
                                      <span class="code">
                                        <xsl:value-of select="ETHNICITY" />
                                      </span>
                                    </div>
                                  </div>
                                  <div class="people-panel-separator" />
                                  <div class="offender-panel">
                                    <xsl:for-each select="OFFENDERS/OFFENDER">
                                      <div class="offender-card">
                                        <span class="victim-sequence-number">
                                          Offender #<xsl:value-of select="position()" />
                                        </span>
                                        <div class="offender-details-wrapper">
                                          <div class="offender-details">
                                            <span class="label">Age:</span>
                                            <span class="code">
                                              <xsl:value-of select="AGE" />
                                            </span>

                                            <span class="label">Sex:</span>
                                            <span class="code">
                                              <xsl:value-of select="SEX" />
                                            </span>

                                            <span class="label">Race:</span>
                                            <span class="code">
                                              <xsl:value-of select="RACE" />
                                            </span>

                                            <span class="label">Ethnicity:</span>
                                            <span class="code">
                                              <xsl:value-of select="ETHNICITY" />
                                            </span>
                                          </div>
                                          <div class="crime-details">
                                            <span class="label">Relationship:</span>
                                            <span class="code">
                                              <xsl:value-of select="RELATIONSHIP" />
                                            </span>

                                            <span class="label">Weapon Used:</span>
                                            <span class="code">
                                              <xsl:value-of select="WEAPONUSED" />
                                            </span>

                                            <span class="label">Circumstance:</span>
                                            <span class="code">
                                              <xsl:value-of select="CIRCUMSTANCE" />
                                            </span>

                                            <xsl:if test="CIRCUMSTANCE=80 or CIRCUMSTNACE=81">
                                              <span class="label">Subcircumstance:</span>
                                              <span class="code">
                                                <xsl:value-of select="SUBCIRCUMSTANCE" />
                                              </span>
                                            </xsl:if>
                                          </div>
                                        </div>
                                      </div>
                                    </xsl:for-each>
                                  </div>
                                </div>
                              </xsl:for-each>
                            </div>
                          </xsl:for-each>
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
                          <xsl:for-each select="UcrReports/SHR/INCIDENTS/INCIDENT[MANSLAUGHTERNEGLIGENT=0]">
                            <div class="incident-wrapper">
                              <div class="incident-header">
                                <span class="incident-number">
                                  Incident #<xsl:value-of select="SEQUENCENUMBER" />
                                </span>
                                <span class="incident-situation">
                                  <xsl:choose>
                                    <xsl:when test="SITUATION='A'">Situation A - Single Victim/Single Offender</xsl:when>
                                    <xsl:when test="SITUATION='B'">Situation B - Single Victim/Unknown Offender or Offenders</xsl:when>
                                    <xsl:when test="SITUATION='C'">Situation C - Single Victim/Multiple Offenders</xsl:when>
                                    <xsl:when test="SITUATION='D'">Situation D - Multiple Victims/Single Offender</xsl:when>
                                    <xsl:when test="SITUATION='E'">Situation E - Multiple Victims/Multiple Offenders</xsl:when>
                                    <xsl:when test="SITUATION='F'">Situation F - Multiple Victims/Unknown Offender or Offenders</xsl:when>
                                  </xsl:choose>
                                </span>
                              </div>
                              <xsl:for-each select="VICTIMS/VICTIM">
                                <div class="related-people-wrapper">
                                  <div class="victim-panel">
                                    <div class="victim-card">
                                      <span class="victim-sequence-number">
                                        Victim #<xsl:value-of select="position()" />
                                      </span>
                                      <span class="label">Age:</span>
                                      <span class="code">
                                        <xsl:value-of select="AGE" />
                                      </span>

                                      <span class="label">Sex:</span>
                                      <span class="code">
                                        <xsl:value-of select="SEX" />
                                      </span>

                                      <span class="label">Race:</span>
                                      <span class="code">
                                        <xsl:value-of select="RACE" />
                                      </span>

                                      <span class="label">Ethnicity:</span>
                                      <span class="code">
                                        <xsl:value-of select="ETHNICITY" />
                                      </span>
                                    </div>
                                  </div>
                                  <div class="people-panel-separator" />
                                  <div class="offender-panel">
                                    <xsl:for-each select="OFFENDERS/OFFENDER">
                                      <div class="offender-card">
                                        <span class="victim-sequence-number">
                                          Offender #<xsl:value-of select="position()" />
                                        </span>
                                        <div class="offender-details-wrapper">
                                          <div class="offender-details">
                                            <span class="label">Age:</span>
                                            <span class="code">
                                              <xsl:value-of select="AGE" />
                                            </span>

                                            <span class="label">Sex:</span>
                                            <span class="code">
                                              <xsl:value-of select="SEX" />
                                            </span>

                                            <span class="label">Race:</span>
                                            <span class="code">
                                              <xsl:value-of select="RACE" />
                                            </span>

                                            <span class="label">Ethnicity:</span>
                                            <span class="code">
                                              <xsl:value-of select="ETHNICITY" />
                                            </span>
                                          </div>
                                          <div class="crime-details">
                                            <span class="label">Relationship:</span>
                                            <span class="code">
                                              <xsl:value-of select="RELATIONSHIP" />
                                            </span>

                                            <span class="label">Weapon Used:</span>
                                            <span class="code">
                                              <xsl:value-of select="WEAPONUSED" />
                                            </span>

                                            <span class="label">Circumstance:</span>
                                            <span class="code">
                                              <xsl:value-of select="CIRCUMSTANCE" />
                                            </span>

                                            <xsl:if test="CIRCUMSTANCE=80 or CIRCUMSTNACE=81">
                                              <span class="label">Subcircumstance:</span>
                                              <span class="code">
                                                <xsl:value-of select="SUBCIRCUMSTANCE" />
                                              </span>
                                            </xsl:if>
                                          </div>
                                        </div>
                                      </div>
                                    </xsl:for-each>
                                  </div>
                                </div>
                              </xsl:for-each>
                            </div>
                          </xsl:for-each>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </xsl:if>
              </td>
            </tr>
          </tbody>
          <!--Page footer-->
          <tfoot>
          </tfoot>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>