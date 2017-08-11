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
                                  <td rowspan="{1 + count(VICTIMS/VICTIM) + count(VICTIMS/VICTIM/OFFENDERS/OFFENDER)}">
                                    <xsl:value-of select="SEQUENCENUMBER"/>
                                  </td>
                                  <td rowspan="{1 + count(VICTIMS/VICTIM) + count(VICTIMS/VICTIM/OFFENDERS/OFFENDER)}">
                                    <xsl:value-of select="SITUATION"/>
                                  </td>
                                </tr>
                                <xsl:for-each select="VICTIMS/VICTIM">
                                  <tr>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="position()"/>
                                    </td>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="AGE"/>
                                    </td>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="SEX"/>
                                    </td>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="RACE"/>
                                    </td>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="ETHNICITY"/>
                                    </td>
                                  </tr>
                                  <xsl:for-each select="OFFENDERS/OFFENDER">
                                    <tr>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="position()"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="AGE"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="SEX"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="RACE"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="ETHNICITY"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="WEAPONUSED"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="RELATIONSHIP"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="CIRCUMSTANCE"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="SUBCIRCUMSTANCE"/>
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
                                  <td rowspan="{1 + count(VICTIMS/VICTIM) + count(VICTIMS/VICTIM/OFFENDERS/OFFENDER)}">
                                    <xsl:value-of select="SEQUENCENUMBER"/>
                                  </td>
                                  <td rowspan="{1 + count(VICTIMS/VICTIM) + count(VICTIMS/VICTIM/OFFENDERS/OFFENDER)}">
                                    <xsl:value-of select="SITUATION"/>
                                  </td>
                                </tr>
                                <xsl:for-each select="VICTIMS/VICTIM">
                                  <tr>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="position()"/>
                                    </td>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="AGE"/>
                                    </td>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="SEX"/>
                                    </td>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="RACE"/>
                                    </td>
                                    <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                      <xsl:value-of select="ETHNICITY"/>
                                    </td>
                                  </tr>
                                  <xsl:for-each select="OFFENDERS/OFFENDER">
                                    <tr>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="position()"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="AGE"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="SEX"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="RACE"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="ETHNICITY"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="WEAPONUSED"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="RELATIONSHIP"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="CIRCUMSTANCE"/>
                                      </td>
                                      <td rowspan="{1 + count(OFFENDERS/OFFENDER)}">
                                        <xsl:value-of select="SUBCIRCUMSTANCE"/>
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