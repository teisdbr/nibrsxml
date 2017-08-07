<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:decimal-format name="us" decimal-separator="." grouping-separator="," />
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
          .rowheader {
          text-align: left;
          }
          table {
          border-spacing: 0px;
          border-collapse: separate;
          }
          td {
          text-align:right;
          }
          .small{
          border: 0px;
          text-align: left;
          font-weight: bold;
          font-size: 10px;
          padding:0px;
          }
        </style>
      </head>
      <body>
        <table>
          <!--Page header-->
          <thead>
            <tr>
              <th colspan="4">Monthly Arson Offenses Known to Law Enforcement</th>
            </tr>
            <tr>
              <th>
                <xsl:value-of select="concat(UcrReports/@ori, ' ', UcrReports/@agency)" />
              </th>
              <th>City: </th>
              <th>Parish: </th>
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
              <td colspan="4">
                <!--Page content-->
                <table>
                  <colgroup span="6"></colgroup>
                  <thead>
                    <tr>
                      <th colspan="2">1</th>
                      <th>2</th>
                      <th>3</th>
                      <th>4</th>
                      <th>5</th>
                      <th>6</th>
                      <th>7</th>
                      <th>8</th>
                    </tr>
                    <tr>
                      <th colspan="2">Property Classification</th>
                      <th>Offenses Reported or Known to Police (Include "Unfounded" and Attempts)</th>
                      <th>Unfounded, i.e., False or Baseless Complaints</th>
                      <th>Number of Actual Offenses (Column 2 Minus Column 3) (Include Attempts)</th>
                      <th>Total Offenses Clears by Arrest or Exceptional Means (Includes Column 6)</th>
                      <th>Number of Clearances Involving Only Persons Under 18 Years of Age</th>
                      <th>Offenses Where Structures Uninhabited, Abandoned, or not Normally in Use</th>
                      <th>Estimated Value of Property Damage</th>
                    </tr>
                  </thead>
                  <tbody>
                    <xsl:for-each select="UcrReports/ArsonSummary/Classification">
                      <tr>
                        <!--The row header-->
                        <xsl:choose>
                          <xsl:when test="@name='A'">
                            <th rowspan="8">STRUCTURAL</th>
                            <th class="rowheader">A. Single Occupancy Residential: Houses, Townhouses, Duplexes, etc.</th>
                          </xsl:when>
                          <xsl:when test="@name='B'">
                            <th class="rowheader">B. Other Residential: Apartments, Tenements, Flats, Hotels, Motels, Inns, Dormitories, Boarding Houses, etc.</th>
                          </xsl:when>
                          <xsl:when test="@name='C'">
                            <th class="rowheader">C. Storage: Barns, Garages, Warehouses, etc.</th>
                          </xsl:when>
                          <xsl:when test="@name='D'">
                            <th class="rowheader">D. Industrial/Manufacturing</th>
                          </xsl:when>
                          <xsl:when test="@name='E'">
                            <th class="rowheader">E. Other Commercial Stores, Restaurants Offices, etc.</th>
                          </xsl:when>
                          <xsl:when test="@name='F'">
                            <th class="rowheader">F. Community/Public: Churches, Jails, Schools, Colleges, Hospitals, etc.</th>
                          </xsl:when>
                          <xsl:when test="@name='G'">
                            <th class="rowheader">G. All Other Structure: Out Buildings, Monuments, Buildings Under Construction, etc.</th>
                          </xsl:when>
                          <xsl:when test="@name='Total Structure'">
                            <th class="rowheader">TOTAL STRUCTURE</th>
                          </xsl:when>
                          <xsl:when test="@name='H'">
                            <th rowspan="3">MOBILE</th>
                            <th class="rowheader">H. Motor Vehicles: Automobiles, Trucks, Buses, Motorcycles, etc.: UCR Definition</th>
                          </xsl:when>
                          <xsl:when test="@name='I'">
                            <th class="rowheader">I. Other Mobile Property: Trailers, Recreational Vehicles, Airplanes, Boats, etc.</th>
                          </xsl:when>
                          <xsl:when test="@name='Total Mobile'">
                            <th class="rowheader">TOTAL MOBILE</th>
                          </xsl:when>
                          <xsl:when test="@name='J'">
                            <th class="rowheader">OTHER</th>
                            <th class="rowheader">J. Total Other: Crops, Timber, Fences, Signs, etc.</th>
                          </xsl:when>
                          <xsl:when test="@name='Grand Total'">
                            <th class="rowheader" colspan="2">GRAND TOTAL</th>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="@name" />
                          </xsl:otherwise>
                        </xsl:choose>

                        <!-- Offenses Reported is same as Actual -->
                        <xsl:choose>
                          <xsl:when test="@name='Grand Total' or @name='Total Structure' or @name='Total Mobile'">
                            <th>
                              <xsl:if test="not(Actual)">0</xsl:if>
                              <xsl:value-of select="Actual" />
                            </th>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:if test="not(Actual)">0</xsl:if>
                              <xsl:value-of select="Actual" />
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>

                        <!-- Unfounded Offenses will always be 0 -->
                        <xsl:choose>
                          <xsl:when test="@name='Grand Total' or @name='Total Structure' or @name='Total Mobile'">
                            <th>0</th>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>0</td>
                          </xsl:otherwise>
                        </xsl:choose>

                        <xsl:choose>
                          <xsl:when test="@name='Grand Total' or @name='Total Structure' or @name='Total Mobile'">
                            <th>
                              <xsl:if test="not(Actual)">0</xsl:if>
                              <xsl:value-of select="Actual" />
                            </th>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:if test="not(Actual)">0</xsl:if>
                              <xsl:value-of select="Actual" />
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>

                        <xsl:choose>
                          <xsl:when test="@name='Grand Total' or @name='Total Structure' or @name='Total Mobile'">
                            <th>
                              <xsl:if test="not(ClearedByArrest)">0</xsl:if>
                              <xsl:value-of select="ClearedByArrest" />
                            </th>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:if test="not(ClearedByArrest)">0</xsl:if>
                              <xsl:value-of select="ClearedByArrest" />
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>

                        <xsl:choose>
                          <xsl:when test="@name='Grand Total' or @name='Total Structure' or @name='Total Mobile'">
                            <th>
                              <xsl:if test="not(ClearedByJuvArrest)">0</xsl:if>
                              <xsl:value-of select="ClearedByJuvArrest" />
                            </th>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:if test="not(ClearedByJuvArrest)">0</xsl:if>
                              <xsl:value-of select="ClearedByJuvArrest" />
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>

                        <!-- This is column 7 which will always be zero for NIBRS -->
                        <!-- Offenses Where Structures Uninhabited, Abandoned, or not Normally in Use -->
                        <xsl:choose>
                          <xsl:when test="@name='Grand Total' or @name='Total Structure' or @name='Total Mobile'">
                            <th>0</th>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>0</td>
                          </xsl:otherwise>
                        </xsl:choose>

                        <xsl:choose>
                          <xsl:when test="@name='Grand Total' or @name='Total Structure' or @name='Total Mobile'">
                            <th>
                              <xsl:choose>
                                <xsl:when test="not(EstimatedValueOfDamage)">$0.00</xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="format-number(EstimatedValueOfDamage, '$#,###.00')" />
                                </xsl:otherwise>
                              </xsl:choose>
                            </th>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:choose>
                                <xsl:when test="not(EstimatedValueOfDamage)">$0.00</xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="format-number(EstimatedValueOfDamage, '$#,###.00')" />
                                </xsl:otherwise>
                              </xsl:choose>
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>
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