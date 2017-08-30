<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:ucr="http://whatever" version="2.0">
  <xsl:function name="ucr:TranslateActivity" as="xs:string">
    <xsl:param name="activityNumber" as="xs:string" />
    <xsl:choose>
      <xsl:when test="$activityNumber='1'">
        <xsl:value-of select="fdfd" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="UNKNOWN" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>
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
          max-height:161mm;
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
                <th colspan="2">Law Enforcement Officers Killed or Assaulted</th>
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
                  <p>
                    Additional information on officers who were assaulted and injured with a firearm or a knife or other cutting instrument will be requested on a separate questionnaire,
                    <span>Analysis of Law Enforcement Officers Killed or Assaulted.</span>
                  </p>
                  <table id="assaults-table">
                    <thead>
                      <tr>
                        <th colspan="14">Officers Assaulted (Exclude officers killed)</th>
                      </tr>
                      <tr>
                        <th rowspan="4">Type of Activity</th>
                        <th rowspan="3">Total Assaults by Weapon</th>
                        <th colspan="4">Type of Weapon</th>
                        <th colspan="7">Type of Assignment</th>
                        <th rowspan="3">Officer Assaults Cleared</th>
                      </tr>
                      <tr>
                        <!-- Weapons -->
                        <th rowspan="2">Firearm</th>
                        <th rowspan="2">Knife or Cutting Instrument</th>
                        <th rowspan="2">Other Dangerous Weapons</th>
                        <th rowspan="2">Hands, Fists, Feet, etc.</th>
                        <!-- Assignments -->
                        <th rowspan="2">Two-Officer Vehicle</th>
                        <th colspan="2">One-Officer Vehicle</th>
                        <th colspan="2">Detective or Special Assignment</th>
                        <th colspan="2">Other</th>
                      </tr>
                      <tr>
                        <!-- Assignments From Column G -->
                        <th>Alone</th>
                        <th>Assisted</th>
                        <th>Alone</th>
                        <th>Assisted</th>
                        <th>Alone</th>
                        <th>Assisted</th>
                      </tr>
                      <tr>
                        <th>A</th>
                        <!-- Weapons -->
                        <th>B</th>
                        <th>C</th>
                        <th>D</th>
                        <th>E</th>
                        <th>F</th>
                        <th>G</th>
                        <th>H</th>
                        <th>I</th>
                        <th>J</th>
                        <th>K</th>
                        <th>L</th>
                        <th>M</th>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:for-each select="UcrReports/LeokaSummary/Assaults/Classification">
                        <tr>
                          <th class="rowheader">
                            <xsl:choose>
                              <xsl:when test="@name='1'">
                                <xsl:value-of
                                  select="'1. Responding to Disturbance Call (Family Quarrels, Person with Firearm, Etc.)'" />
                              </xsl:when>
                              <xsl:when test="@name='2'">
                                <xsl:value-of select="'2. Burglaries in Progress or Pursuing Burglary Suspects'" />
                              </xsl:when>
                              <xsl:when test="@name='3'">
                                <xsl:value-of select="'3. Robberies in Progress or Pursuing Robbery Suspects'" />
                              </xsl:when>
                              <xsl:when test="@name='4'">
                                <xsl:value-of select="'4. Attempting Other Arrests'" />
                              </xsl:when>
                              <xsl:when test="@name='5'">
                                <xsl:value-of select="'5. Civil Disorder (Riot, Mass Disobedience)'" />
                              </xsl:when>
                              <xsl:when test="@name='6'">
                                <xsl:value-of select="'6. Handling, Transporting, Custody of Prisoners'" />
                              </xsl:when>
                              <xsl:when test="@name='7'">
                                <xsl:value-of select="'7. Investigating Suspicious Persons or Circumstances'" />
                              </xsl:when>
                              <xsl:when test="@name='8'">
                                <xsl:value-of select="'8. Ambush-No Warning'" />
                              </xsl:when>
                              <xsl:when test="@name='9'">
                                <xsl:value-of select="'9. Handling Persons with Mental Illness'" />
                              </xsl:when>
                              <xsl:when test="@name='10'">
                                <xsl:value-of select="'10. Traffic Pursuits and Stops'" />
                              </xsl:when>
                              <xsl:when test="@name='11'">
                                <xsl:value-of select="'11. All Other'" />
                              </xsl:when>
                              <xsl:when test="@name='12'">
                                <xsl:value-of select="'12. TOTAL (1-11)'" />
                              </xsl:when>
                              <xsl:when test="@name='13'">
                                <xsl:value-of select="'13. Number with Personal Injuries *'" />
                              </xsl:when>
                              <xsl:when test="@name='14'">
                                <xsl:value-of select="'14. Number without Personal Injuries'" />
                              </xsl:when>
                            </xsl:choose>
                          </th>
                          <td>
                            <xsl:if test="not(A)">0</xsl:if>
                            <xsl:value-of select="A" />
                          </td>
                          <td>
                            <xsl:if test="not(B)">0</xsl:if>
                            <xsl:value-of select="B" />
                          </td>
                          <td>
                            <xsl:if test="not(C)">0</xsl:if>
                            <xsl:value-of select="C" />
                          </td>
                          <td>
                            <xsl:if test="not(D)">0</xsl:if>
                            <xsl:value-of select="D" />
                          </td>
                          <td>
                            <xsl:if test="not(E)">0</xsl:if>
                            <xsl:value-of select="E" />
                          </td>
                          <xsl:if test="@name!=13 and @name!=14">
                            <td>
                              <xsl:if test="not(F)">0</xsl:if>
                              <xsl:value-of select="F" />
                            </td>
                            <td>
                              <xsl:if test="not(G)">0</xsl:if>
                              <xsl:value-of select="G" />
                            </td>
                            <td>
                              <xsl:if test="not(H)">0</xsl:if>
                              <xsl:value-of select="H" />
                            </td>
                            <td>
                              <xsl:if test="not(I)">0</xsl:if>
                              <xsl:value-of select="I" />
                            </td>
                            <td>
                              <xsl:if test="not(J)">0</xsl:if>
                              <xsl:value-of select="J" />
                            </td>
                            <td>
                              <xsl:if test="not(K)">0</xsl:if>
                              <xsl:value-of select="K" />
                            </td>
                            <td>
                              <xsl:if test="not(L)">0</xsl:if>
                              <xsl:value-of select="L" />
                            </td>
                            <td>
                              <xsl:if test="not(M)">0</xsl:if>
                              <xsl:value-of select="M" />
                            </td>
                          </xsl:if>
                        </tr>
                      </xsl:for-each>
                      <tr>
                        <th class="rowheader" rowspan="4">15. Time of Assaults</th>
                        <td rowspan="2">Time Period</td>
                        <td rowspan="2">12:01-02:00</td>
                        <td rowspan="2">2:01-04:00</td>
                        <td rowspan="2">4:01-06:00</td>
                        <td rowspan="2">6:01-08:00</td>
                        <td rowspan="2">8:01-10:00</td>
                        <td rowspan="2">10:01-12:00</td>
                        <th colspan="3" rowspan="4">
                          <span>OFFICERS KILLED</span>
                          <span>Number of your law enforcement officers killed in the line of duty this month.</span>
                        </th>
                        <td colspan="2" rowspan="2">By felonious act</td>
                        <td rowspan="2">
                          <xsl:if test="not(//Feloneously[1] != '')">0</xsl:if>
                          <xsl:value-of select="//Feloneously[1]" />
                        </td>
                        <td style="visibility:hidden;">.</td>
                      </tr>
                      <tr>
                        <td style="visibility:hidden;">.</td>
                      </tr>
                      <tr>
                        <td>AM</td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H00-01)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H00-01" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H02-03)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H02-03" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H04-05)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H04-05" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H06-07)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H06-07" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H08-09)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H08-09" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H10-11)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H10-11" />
                        </td>
                        <td rowspan="2" colspan="2">By accident or negligence</td>
                        <td rowspan="2">
                          <xsl:if test="not(//ByAccident[1] != '')">0</xsl:if>
                          <xsl:value-of select="//ByAccident[1]" />
                        </td>
                      </tr>
                      <tr>
                        <td>PM</td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H12-13)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H12-13" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H14-15)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H14-15" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H16-17)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H16-17" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H18-19)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H18-19" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H20-21)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H20-21" />
                        </td>
                        <td>
                          <xsl:if test="not(UcrReports/LeokaSummary/AssaultsTime/H22-23)">0</xsl:if>
                          <xsl:value-of select="UcrReports/LeokaSummary/AssaultsTime/H22-23" />
                        </td>
                      </tr>
                    </tbody>
                  </table>
                  <p>* If the officer was injured with a firearm (13B) or a knife or other cutting instrument (13C), please complete the block on page 2 and include your agency's incident or case number(s).</p>
                  <p>This information is only for your agency's use to assist in referencing the incident once the above-mention questionnaire is forwarded to you for completion.</p>
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