<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          body {
          font-size: 20px;
          }
          th{
          border: 0px ;
          text-align: left;
          }
          td {
          border: 1px solid black;
          text-align: left;
          }
          table {
          width: 1200px;
          border-spacing: 0px;
          border-collapse: separate;
          }
          .rowheader {
          text-align: left;
          }
          @media print {
          table{
          page-break-inside: avoid;
          page-break-after: always;
          }
          thead {display: table-header-group;}
          }
         
          
        </style>
      </head>
      <body>
        <xsl:for-each select="INCIDENTS/INCIDENT">
          <xsl:variable name="iposition" select="position()" />
          <table>
            <colgroup span="3"></colgroup>
            <thead>
              <tr>
                <th colspan="3" scope="colgroup" style="text-align:center;">
                  <p>
                    Supplementary Homicide Report
                  </p>
                </th>
              </tr>
              <tr>
                <th colspan="3" scope="colgroup" >
                  <xsl:if test="MANSLAUGTERNEGLIGENT='1'">
                    <p class="firstpage">Manslaughter by Negligence</p>
                    <p class="firstpagedesc">
                      Do not list traffic fatalities, accidental deaths, or death due to negligence of the victim. List below all other
                      negligent manslaughters, regardless of prosecutive action taken.
                    </p>
                  </xsl:if>
                  <xsl:if test="MANSLAUGTERNOTNEGLIGENT='1'">
                    <p class="firstpage">Murder and Nonnegligent Manslaughter</p>
                   <p class="firstpagedesc">
                      List below for each murder and nonnegligent homicide and / or justifiable homicide shown in item 1a of the monthly
                      Return A. In addition, for justifiable homicide list all justifiable killings of felons by a citizen or by a peace
                      officer in the line of duty. A brief explanation in the circumstances field regarding unfounded homicide offenses
                      will aid the natural Uniform Crime Reporting Program in editing the reports.
                    </p>
                  </xsl:if>
                </th>
              </tr>
              <tr>
                <th>
                  Incident #: <xsl:value-of select="position()" />
                  <br/>
                  <br/>
                </th>
              </tr>
              <tr>
                <th>
                  Situation : <xsl:value-of select="SITUATION" />
                </th>
              </tr>
              <xsl:for-each select="VICTIMS/VICTIM">
                <tr>
                  <th>
                    Victim # : <xsl:value-of select="position()" />
                  </th>
                </tr>
                <tr>
                  <th >
                    Age : <xsl:value-of select="AGE" />
                  </th>
                  <th>
                    Sex : <xsl:value-of select="SEX" />
                  </th>
                </tr>
                <tr>
                  <th>
                    Ethnicity : <xsl:value-of select="ETHNICITY" />
                  </th>
                  <th>
                    Race : <xsl:value-of select="RACE" />
                  </th>
                </tr>
                <tr>
                  <th colspan="3" style="text-align:center;">
                    Offenders for Victim #: <xsl:value-of select="$iposition" />
                  </th>
                </tr>
              </xsl:for-each>
            </thead>
            <tbody>
              <xsl:for-each select="OFFENDERS/OFFENDER">
                <tr>
                  <td colspan="3" style="text-align:left;border:0px;">
                    Offender # : <xsl:value-of select="position()" />
                  </td>
                </tr>
                <tr>
                  <td colspan="1">
                    *Age : <xsl:value-of select="AGE" />
                  </td>
                  <td colspan="1">
                    Sex : <xsl:value-of select="SEX" />
                  </td>
                  <td colspan="1">
                    Race : <xsl:value-of select="RACE" />
                  </td>
                </tr>
                <tr>
                  <td colspan="1">
                    Ethnicity : <xsl:value-of select="ETHNICITY" />
                  </td>
                  <td colspan="1">
                    Weapon Used : <xsl:value-of select="WEAPONUSED" />
                  </td>
                  <td colspan="1">
                    Relationship : <xsl:value-of select="RELATIONSHIP" />
                  </td>
                </tr>
                <tr>
                  <td colspan="1">
                    Circumstance : <xsl:value-of select="CIRCUMSTANCE" />
                  </td>
                  <td colspan="1">
                    Subcircumstance : <xsl:value-of select="SUBCIRCUMSTANCE" />
                  </td>
                  <td colspan="1">

                  </td>
                </tr>
              </xsl:for-each>
            </tbody>
          </table>
          </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>