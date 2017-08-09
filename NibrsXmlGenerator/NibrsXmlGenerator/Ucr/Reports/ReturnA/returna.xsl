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
          border-collapse: collapse;
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
              <th colspan="2">Return A - Monthly Return of Offenses Known to the Police</th>
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
                <table>
                  <thead>
                    <tr>
                      <th colspan="2">1</th>
                      <th>2</th>
                      <th>3</th>
                      <th>4</th>
                      <th>5</th>
                      <th>6</th>
                    </tr>
                    <tr>
                      <th>Classification of Offenses</th>
                      <th>DATA ENTRY</th>
                      <th>Offenses Reported or Known to Police (Include "Unfounded" and Attempts)</th>
                      <th>Unfounded, i.e., False or Baseless Complaints</th>
                      <th>Number of Actual Offenses (Column 2 Minus Column 3) (Include Attempts)</th>
                      <th>Total Offenses Clears by Arrest or Exceptional Means (Includes Column 6)</th>
                      <th>Number of Clearances Involving Only Persons Under 18 Years of Age</th>
                    </tr>
                  </thead>
                  <tbody>
                    <xsl:for-each select="UcrReports/ReturnASummary/Classification">
                      <tr>
                        <!--The row header-->
                        <xsl:choose>
                          <xsl:when test="@name='1'">
                            <th class="rowheader">
                              <xsl:value-of select="'1. CRIMINAL HOMICIDE TOTAL'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='1a'">
                            <th class="rowheader">
                              <xsl:value-of select="'a. Murder &amp; Nonnegligent Homicide (Score atte,[ts as aggravated assault) If homicide reported, submit Supplementary Homicide Report'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='1b'">
                            <th class="rowheader">
                              <xsl:value-of select="'b. Manslaughter by Negligence'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='2'">
                            <th class="rowheader">
                              <xsl:value-of select="'2. RAPE TOTAL'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='2a'">
                            <th class="rowheader">
                              <xsl:value-of select="'a. Rape'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='2b'">
                            <th class="rowheader">
                              <xsl:value-of select="'b. Attempts to Commit Rape'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='3'">
                            <th class="rowheader">
                              <xsl:value-of select="'3. ROBBERY TOTAL'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='3a'">
                            <th class="rowheader">
                              <xsl:value-of select="'a. Firearm'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='3b'">
                            <th class="rowheader">
                              <xsl:value-of select="'b. Knife or Cutting Instrument'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='3c'">
                            <th class="rowheader">
                              <xsl:value-of select="'c. Other Dangerous Weapon'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='3d'">
                            <th class="rowheader">
                              <xsl:value-of select="'d. Hands, Fists, Feet, Etc. - Aggravated Injury'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='4'">
                            <th class="rowheader">
                              <xsl:value-of select="'4. ASSAULT TOTAL'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='4a'">
                            <th class="rowheader">
                              <xsl:value-of select="'a. Firearm'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='4b'">
                            <th class="rowheader">
                              <xsl:value-of select="'b. Knife or Cutting Instrument'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='4c'">
                            <th class="rowheader">
                              <xsl:value-of select="'c. Other Dangerous Weapon'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='4d'">
                            <th class="rowheader">
                              <xsl:value-of select="'d. Strong-Arm (Hands, Fists, Feet, Etc.)'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='4e'">
                            <th class="rowheader">
                              <xsl:value-of select="'e. Other Assaults - Simple, Not Aggravated'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='5'">
                            <th class="rowheader">
                              <xsl:value-of select="'5. BURGLARY TOTAL'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='5a'">
                            <th class="rowheader">
                              <xsl:value-of select="'a. Forcible Entry'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='5b'">
                            <th class="rowheader">
                              <xsl:value-of select="'b. Unlawful Entry - No Force'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='5c'">
                            <th class="rowheader">
                              <xsl:value-of select="'c. Attempted Forcible Entry'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='6'">
                            <th class="rowheader">
                              <xsl:value-of select="'6. LARCENY-THEFT TOTAL (Except Motor Vehicle Theft)'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='7'">
                            <th class="rowheader">
                              <xsl:value-of select="'7. MOTOR VEHICLE THEFT TOTAL'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='7a'">
                            <th class="rowheader">
                              <xsl:value-of select="'a. Autos'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='7b'">
                            <th class="rowheader">
                              <xsl:value-of select="'b. Trucks and Buses'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='7c'">
                            <th class="rowheader">
                              <xsl:value-of select="'c. Other Vehicles'" />
                            </th>
                          </xsl:when>
                          <xsl:when test="@name='Grand Total'">
                            <th class="rowheader" colspan="2">
                              GRAND TOTAL
                            </th>
                          </xsl:when>
                        </xsl:choose>
                        <xsl:choose>
                          <xsl:when test="@name='1'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'N/A'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='1a'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'11'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='1b'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'12'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='2'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'20'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='2a'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'21'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='2b'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'22'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='3'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'30'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='3a'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'31'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='3b'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'32'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='3c'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'33'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='3d'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'34'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='4'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'40'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='4a'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'41'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='4b'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'42'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='4c'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'43'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='4d'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'44'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='4e'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'45'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='5'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'50'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='5a'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'51'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='5b'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'52'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='5c'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'53'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='6'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'60'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='7'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'70'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='7a'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'71'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='7b'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'72'" />
                            </td>
                          </xsl:when>
                          <xsl:when test="@name='7c'">
                            <td style="text-align:center;">
                              <xsl:value-of select="'73'" />
                            </td>
                          </xsl:when>
                        </xsl:choose>

                        <!-- Offenses Reported is same as Actual -->
                        <td>
                          <xsl:if test="not(Actual)">0</xsl:if>
                          <xsl:value-of select="Actual" />
                        </td>

                        <!-- Unfounded Offenses will always be 0 -->
                        <td>0</td>

                        <td>
                          <xsl:if test="not(Actual)">0</xsl:if>
                          <xsl:value-of select="Actual" />
                        </td>
                        <td>
                          <xsl:if test="not(ClearedByArrest)">0</xsl:if>
                          <xsl:value-of select="ClearedByArrest" />
                        </td>
                        <td>
                          <xsl:if test="not(ClearedByJuvArrest)">0</xsl:if>
                          <xsl:value-of select="ClearedByJuvArrest" />
                        </td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
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