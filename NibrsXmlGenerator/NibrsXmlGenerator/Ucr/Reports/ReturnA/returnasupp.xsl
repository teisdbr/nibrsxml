<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:decimal-format name="us" decimal-separator="." grouping-separator="," />
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
                <th colspan="2">
                  Supplement to Return A <br />Monthly Return Of Offenses Known to Police
                </th>
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
                  <p>This form deals with the nature of crime and the monetary value of property stole and recovered. The total offenses recorded on this form should be the same as the number of actual offenses listed in Column 4 of the Return A for each crime class. Include attempted crimes on this form, but do not include unfounded offenses. If you cannot complete the report in all areas, please record as much information as is available.</p>
                  <table style="width:100%;">
                    <thead>
                      <xsl:for-each select="UcrReports/ReturnASupplement">
                        <tr>
                          <th colspan="4">PROPERTY BY TYPE AND VALUE</th>
                        </tr>
                        <tr>
                          <th>Type of Property</th>
                          <th rowspan="2">Data Entry</th>
                          <th colspan="2">Monetary Value of Property Stolen in Your Jurisdiction</th>
                        </tr>
                        <tr>
                          <th>(1)</th>
                          <th>
                            Stolen<br />(2) *
                          </th>
                          <th>
                            Recovered<br />(3) **
                          </th>
                        </tr>
                      </xsl:for-each>
                    </thead>
                    <tbody>
                      <xsl:for-each select="UcrReports/ReturnASupplement/StolenAndRecoveredProperties/Property">
                        <tr>
                          <!--Stolen and Recovered Properties by Type and Value - row header-->
                          <th class="rowheader">
                            <xsl:choose>
                              <xsl:when test="@entry='01'">
                                <xsl:value-of select="'(A) Currency, Notes, Etc.'" />
                              </xsl:when>
                              <xsl:when test="@entry='02'">
                                <xsl:value-of select="'(B) Jewelry and Precious Metals'" />
                              </xsl:when>
                              <xsl:when test="@entry='03'">
                                <xsl:value-of select="'(C) Clothing and Furs'" />
                              </xsl:when>
                              <xsl:when test="@entry='04'">
                                <xsl:value-of select="'(D) Locally Stolen Motor Vehicles'" />
                              </xsl:when>
                              <xsl:when test="@entry='05'">
                                <xsl:value-of select="'(E) Office Equipment'" />
                              </xsl:when>
                              <xsl:when test="@entry='06'">
                                <xsl:value-of select="'(F) Televisions, Radios, Stereos, Etc.'" />
                              </xsl:when>
                              <xsl:when test="@entry='07'">
                                <xsl:value-of select="'(G) Firearms'" />
                              </xsl:when>
                              <xsl:when test="@entry='08'">
                                <xsl:value-of select="'(H) Household Goods'" />
                              </xsl:when>
                              <xsl:when test="@entry='09'">
                                <xsl:value-of select="'(I) Consumable Goods'" />
                              </xsl:when>
                              <xsl:when test="@entry='10'">
                                <xsl:value-of select="'(J) Livestock'" />
                              </xsl:when>
                              <xsl:when test="@entry='11'">
                                <xsl:value-of select="'(K) Miscellaneous'" />
                              </xsl:when>
                              <xsl:when test="@entry='00'">
                                <xsl:value-of select="'TOTAL'" />
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="@entry" />
                              </xsl:otherwise>
                            </xsl:choose>
                          </th>
                          <td style="text-align:center;">
                            <xsl:value-of select="@entry" />
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="not(Stolen)">$0.00</xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="format-number(Stolen, '$#,###.00')" />
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="not(Recovered)">$0.00</xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="format-number(Recovered, '$#,###.00')" />
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </tbody>
                    <tfoot>
                      <tr>
                        <td colspan="4">
                          <p>* The total of this column should agree with the Grand Total (DATA ENTRY 77) in the Property Stolen By Classification table.</p>
                          <p>** Include in this column all property recovered even though stolen in prior months. The above is an accounting for only that property stolen in your jurisdiction. This will include property recovered for you by other jurisdictions, but not property you recover for them.</p>
                        </td>
                      </tr>
                    </tfoot>
                  </table>

                  <div style="display:inline-block;"></div>

                  <table style="width:100%;">
                    <thead>
                      <tr>
                        <th colspan="6">PROPERTY STOLEN BY CLASSIFICATION</th>
                      </tr>
                      <tr>
                        <th rowspan="2" colspan="3">Classification</th>
                        <th rowspan="2">Data Entry</th>
                        <th rowspan="2">Number of Actual Offenses (Column 4 Return A)</th>
                        <th rowspan="2">Monetary Value of Property Stolen</th>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:for-each
                        select="UcrReports/ReturnASupplement/StolenPropertiesByClassification/OffenseClassification">
                        <xsl:if test="@entry &lt; 80">
                          <tr>
                            <!--Properties Stolen by Classification - row header-->
                            <xsl:choose>
                              <xsl:when test="@entry='12'">
                                <th class="rowheader" colspan="3">
                                  <xsl:value-of select="'MURDER AND NONNEGLIGENT MANSLAUGHTER'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='20'">
                                <th class="rowheader" colspan="3">
                                  <xsl:value-of select="'RAPE'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='31'">
                                <th class="rowheader" rowspan="8">ROBBERY</th>
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Robbery - Highway (Streets, Alleys, Etc.)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='32'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Robbery - Comercial House (Except c, d, and f)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='33'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Robbery - Gas or Service Station'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='34'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Robbery - Convenience Store'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='35'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Robbery - Residence (anywhere on premises)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='36'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Robbery - Bank'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='37'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Robbery - Miscellaneous'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='30'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'TOTAL ROBBERY'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='51'">
                                <th class="rowheader" rowspan="7">BURGLARY - BREAKING OR ENTERING</th>
                                <th class="rowheader" rowspan="3">Residence (Dwelling)</th>
                                <th class="rowheader">
                                  <xsl:value-of select="'Night (6 p.m. to 6 a.m.)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='52'">
                                <th class="rowheader">
                                  <xsl:value-of select="'Day (6 a.m. to 6 p.m.)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='53'">
                                <th class="rowheader">
                                  <xsl:value-of select="'Unknown'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='54'">
                                <th class="rowheader" rowspan="3">Nonresidence</th>
                                <th class="rowheader">
                                  <xsl:value-of select="'Night (6 p.m. to 6 a.m.)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='55'">
                                <th class="rowheader">
                                  <xsl:value-of select="'Day (6 a.m. to 6 p.m.)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='56'">
                                <th class="rowheader">
                                  <xsl:value-of select="'Unknown'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='50'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'TOTAL BURGLARY'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='61'">
                                <th class="rowheader" rowspan="4">LARCENY/THEFT (EXCEPT MOTOR VEHICLE THEFT)</th>
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Larceny - $200+'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='62'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Larceny - $50 - $199'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='63'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'Larceny - Below $50'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='60'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'TOTAL LARCENY (SAME AS DATA ENTRY 80)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='70'">
                                <th class="rowheader" colspan="3">
                                  <xsl:value-of select="'MOTOR VEHICLE THEFT (INCLUDE ALLEGED JOY RIDE)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='77'">
                                <th class="rowheader" colspan="3">
                                  <xsl:value-of select="'GRAND TOTAL'" />
                                </th>
                              </xsl:when>
                            </xsl:choose>
                            <td style="text-align:center;">
                              <xsl:value-of select="@entry" />
                            </td>
                            <td>
                              <xsl:choose>
                                <xsl:when test="not(Actual)">0</xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="format-number(Actual, '#,###')" />
                                </xsl:otherwise>
                              </xsl:choose>
                            </td>
                            <td>
                              <xsl:choose>
                                <xsl:when test="not(Stolen)">$0.00</xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="format-number(Stolen, '$#,###.00')" />
                                </xsl:otherwise>
                              </xsl:choose>
                            </td>
                          </tr>
                        </xsl:if>
                      </xsl:for-each>
                    </tbody>
                  </table>

                  <div style="display:inline-block;"></div>


                  <table style="width:100%;">
                    <thead>
                      <tr>
                        <th colspan="6">ADDITIONAL ANALYSIS OF LARCENY AND MOTOR VEHICLE THEFT</th>
                      </tr>
                      <tr>
                        <th rowspan="2" colspan="3">Classification</th>
                        <th rowspan="2">Data Entry</th>
                        <th rowspan="2">Number of Actual Offenses (Column 4 Return A)</th>
                        <th rowspan="2">Monetary Value of Property Stolen</th>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:for-each
                        select="UcrReports/ReturnASupplement/StolenPropertiesByClassification/OffenseClassification">
                        <xsl:if test="@entry &gt;= 80">
                          <tr>
                            <!--Properties Stolen by Classification - row header-->
                            <xsl:choose>
                              <xsl:when test="@entry='81'">
                                <th class="rowheader" rowspan="10">6X. NATURE OF LARCENIES UNDER ITEM 6</th>
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'a. Pocket-Picking'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='82'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'b. Purse-Snatching'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='83'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'c. Shoplifting'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='84'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'d. From Motor Vehicles (Except e)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='85'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'e. Motor Vehicle Parts and Accessories'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='86'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'f. Bicycles'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='87'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'g. From Building'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='88'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'h. From Coin-Operated Machine (Parking Meters, Etc.)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='89'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'i. All Other'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='80'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="'TOTAL LARCENY (SAME AS DATA ENTRY 60)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='91'">
                                <th class="rowheader" rowspan="4">7X. MOTOR VEHICLES RECOVERED</th>
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of
                                    select="'a. Stolen Locally and Recovered Locally'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='92'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of
                                    select="'b. Stolen Locally and Recovered by Other Jurisdictions'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='90'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of
                                    select="'c. Total Locally Stolen Motor Vehicles Recovered (a &amp; b)'" />
                                </th>
                              </xsl:when>
                              <xsl:when test="@entry='93'">
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of
                                    select="'d. Stolen in Other Jurisdictions and Recovered Locally'" />
                                </th>
                              </xsl:when>
                            </xsl:choose>
                            <td style="text-align:center;">
                              <xsl:value-of select="@entry" />
                            </td>
                            <td>
                              <xsl:choose>
                                <xsl:when test="not(Actual)">0</xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="format-number(Actual, '#,###')" />
                                </xsl:otherwise>
                              </xsl:choose>
                            </td>
                            <td>
                              <xsl:choose>
                                <xsl:when test="not(Stolen)">$0.00</xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="format-number(Stolen, '$#,###.00')" />
                                </xsl:otherwise>
                              </xsl:choose>
                            </td>
                          </tr>
                        </xsl:if>
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
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>