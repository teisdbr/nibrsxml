<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          body {
          font-size: 20px;
          }
          td.head{
          border: 0px ;
          text-align: left;
          font-weight: bold;
          }

          td {
          border: 1px solid black;
          text-align: left;
          }
          table {
          width: 1200px;
          border-spacing: 0px;
          border-collapse: collapse;
          page-break-inside: avoid;
          }
          .rowheader {
          text-align: left;
          }
          p.desc{
          font-size: 18px;
          }
          @media print {
          table{
          page-break-inside: avoid;
          page-break-after: always;
          }

          div {
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
            <colgroup span="3"></colgroup>
            <thead>

              <tr>
                <th colspan="3" scope="colgroup" style="text-align:center;">
                  Supplementary Homicide Report
                </th>
              </tr>


            </thead>
            <tbody>
              <tr>
                <td colspan="3" scope="colgroup" class="head" >
                  <xsl:if test="MANSLAUGTERNEGLIGENT='1'">
                   
                      
                      <p>Manslaughter by Negligence</p>
                      <p class="desc">
                    
                        Do not list traffic fatalities, accidental deaths, or death due to negligence of the victim. List below all other
                        negligent manslaughters, regardless of prosecutive action taken.
                      </p>
                    
                  </xsl:if>
                  <xsl:if test="MANSLAUGTERNOTNEGLIGENT='1'">
                   
                      <p >Murder and Nonnegligent Manslaughter</p>
                      <p class="desc">    
                        List below for each murder and nonnegligent homicide and / or justifiable homicide shown in item 1a of the monthly
                        Return A. In addition, for justifiable homicide list all justifiable killings of felons by a citizen or by a peace
                        officer in the line of duty. A brief explanation in the circumstances field regarding unfounded homicide offenses
                        will aid the natural Uniform Crime Reporting Program in editing the reports.
                      </p>
                    </xsl:if>
                </td>
              </tr>
              <tr>
                <td colspan="3" class="head">
                  Incident #: <xsl:value-of select="position()" />
                  <br/>
                  <br/>
                </td>
              </tr>
              <tr>
                <td colspan="3" class="head">
                  Situation : <xsl:value-of select="SITUATION" />
                </td>
              </tr>
              <xsl:for-each select="VICTIMS/VICTIM">
                <tr>
                  <td colspan="3" class="head">
                    Victim # : <xsl:value-of select="position()" />
                  </td>
                </tr>
                <tr>
                  <td colspan="1" class="head">
                    Age : <xsl:value-of select="AGE" />
                  </td>
                  <td colspan="2" class="head">
                    Sex : <xsl:value-of select="SEX" />
                  </td>
                </tr>
                <tr>
                  <td colspan="1" class="head">
                    Etdnicity : <xsl:value-of select="EtdNICITY" />
                  </td>
                  <td colspan="2" class="head">
                    Race : <xsl:value-of select="RACE" />
                  </td>
                </tr>
                <tr>
                  <td colspan="3" style="text-align:center;border:0px">
                    Offenders for Victim #: <xsl:value-of select="$iposition" />
                  </td>
                </tr>
             

              <xsl:for-each select="OFFENDERS/OFFENDER">
                <div>
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
                  <br/>
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