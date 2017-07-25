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
          table {
          width: 100%;
          border-spacing: 0px;
          border-collapse: collapse;
          page-break-inside: avoid;
          }
          div{
          white-space:nowrap;
          }
          @media print {
          table{
          page-break-inside: avoid;
          page-break-after: always;
          }
          div.body {
          page-break-inside: avoid;
          <!--page-break-after: always;-->
          }
          }
        </style>
      </head>
      <body>
        <table>
          <xsl:for-each select="SHR/INCIDENTS/INCIDENT">
            <xsl:variable name="iposition" select="position()" />
            <thead>
              <tr colspan="3">
                <th style="text-align:center;">
                  Supplementary Homicide Report
                </th>
              </tr>
              <tr>
                <td  class="small">
                  <xsl:value-of select="concat(../../@AGENCY,'  ',../../@ORI)" />
                </td>
                <td  class="small">
                  <xsl:value-of select="../../@PERIOD" />
                  <br />
                  <br />
                </td>
              </tr>
            </thead>
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
              <tr >
                <td class="head">
                  Incident #: <xsl:value-of select="SEQUENCENUMBER" />
                </td>
              </tr>
              <tr >
                <td class="head">
                  Situation : <xsl:value-of select="SITUATION" />
                </td>
              </tr>
              <xsl:for-each select="VICTIMS/VICTIM">
                <tr >
                  <td class="head">
                    Victim # : <xsl:value-of select="position()" />
                  </td>
                </tr>
                <tr >
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
                    Ethnicity : <xsl:value-of select="ETHNICITY" />
                  </td>
                  <td class="head">
                    <div>
                    Race : <xsl:value-of select="RACE" />
                    </div>
                  </td>
                </tr>
                <tr >
                  <td style="text-align:center;border:0px">
                    Offenders for Victim #: <xsl:value-of select="position()" />
                  </td>
                </tr>

                <xsl:for-each select="OFFENDERS/OFFENDER">
                  <div class="body">
                    <tr >
                      <td style="text-align:left;border:0px;">
                        Offender # : <xsl:value-of select="position()" />
                      </td>
                    </tr>
                    <tr>

                      <td>
                        *Age : <xsl:value-of select="AGE" />
                      </td>

                      <td>
                        Sex : <xsl:value-of select="SEX" />
                      </td>

                      <td>
                          Race : <xsl:value-of select="RACE" />
                      </td>
                    </tr>
                    <tr >
                      <td>
                        Ethnicity : <xsl:value-of select="ETHNICITY" />
                      </td>
                      <td>
                        Weapon Used : <xsl:value-of select="WEAPONUSED" />
                      </td>
                      <td>
                        Relationship : <xsl:value-of select="RELATIONSHIP" />
                      </td>
                    </tr>
                    <tr>
                      <td>
                        Circumstance : <xsl:value-of select="CIRCUMSTANCE" />
                      </td>
                      <td>
                        Subcircumstance : <xsl:value-of select="SUBCIRCUMSTANCE" />
                      </td>
                      <td>
                      </td>
                    </tr>
                    <br />
                  </div>
                </xsl:for-each>
              </xsl:for-each>
            </tbody>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>