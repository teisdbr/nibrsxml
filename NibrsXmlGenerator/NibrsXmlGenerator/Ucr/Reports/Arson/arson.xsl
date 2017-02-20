<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:decimal-format name="us" decimal-separator="." grouping-separator=","/>
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
        </style>
      </head>
      <body>
        <table>
          <colgroup span="6"></colgroup>
          <thead>
            <tr>
              <th colspan="8" scope="colgroup">Monthly Arson Offenses Known to Law Enforcement</th>
            </tr>
            <tr>
              <th scope="col">1</th>
              <th scope="col">2</th>
              <th scope="col">3</th>
              <th scope="col">4</th>
              <th scope="col">5</th>
              <th scope="col">6</th>
              <th scope="col">7</th>
              <th scope="col">8</th>              
            </tr>
            <tr>
              <th scope="col">Property Classification</th>
              <th scope="col">Offenses Reported</th>
              <th scope="col">Unfounded, i.e., False or Baseless Complaints</th>
              <th scope="col">Number of Actual Offenses (Column 2 Minus Column 3) (Include Attempts)</th>
              <th scope="col">Total Offenses Clears by Arrest or Exceptional Means</th>
              <th scope="col">Number of Clearances Involving Only Persons Under 18 Years of Age</th>
              <th scope="col">Offenses Where Structures Uninhabited, Abandoned, or not Normally in Use</th>
              <th scope="col">Estimated Value of Property Damage</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="ArsonSummary/Classification">
              <tr>
                <!--The row header-->
                <th class="rowheader">
                  <xsl:choose>
                    <xsl:when test="@name='A'">
                      <xsl:value-of select="'A. Single Occupancy Residential: Houses, Townhouses, Duplexes, etc.'"/>
                    </xsl:when>
                    <xsl:when test="@name='B'">
                      <xsl:value-of select="'B. Other Residential: Apartments, Tenements, Flats, Hotels, Motels, Inns, Dormitories, Boarding Houses, etc.'"/>
                    </xsl:when>
                    <xsl:when test="@name='C'">
                      <xsl:value-of select="'C. Storage: Barns, Garages, Warehouses, etc.'"/>
                    </xsl:when>
                    <xsl:when test="@name='D'">
                      <xsl:value-of select="'D. Industrial/Manufacturing'"/>
                    </xsl:when>
                    <xsl:when test="@name='E'">
                      <xsl:value-of select="'E. Other Commercial Stores, Restaurants Offices, etc.'"/>
                    </xsl:when>
                    <xsl:when test="@name='F'">
                      <xsl:value-of select="'F. Community/Public: Churches, Jails, Schools, Colleges, Hospitals, etc.'"/>
                    </xsl:when>
                    <xsl:when test="@name='G'">
                      <xsl:value-of select="'G. All Other Structure: Out Buildings, Monuments, Buildings Under Construction, etc.'"/>
                    </xsl:when>
                    <xsl:when test="@name='H'">
                      <xsl:value-of select="'H. Motor Vehicles: Automobiles, Trucks, Buses, Motorcycles, etc.: UCR Definition'"/>
                    </xsl:when>
                    <xsl:when test="@name='I'">
                      <xsl:value-of select="'I. Other Mobile Property: Trailers, Recreational Vehicles, Airplanes, Boats, etc.'"/>
                    </xsl:when>
                    <xsl:when test="@name='J'">
                      <xsl:value-of select="'J. Total Other: Crops, Timber, Fences, Signs, etc.'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="@name"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </th>

                <!-- Offenses Reported is same as Actual -->
                <td>
                  <xsl:if test="not(Actual)">0</xsl:if>
                  <xsl:value-of select="Actual"/>
                </td>

                <!-- Unfounded Offenses will always be 0 -->
                <td>0</td>

                <td>
                  <xsl:if test="not(Actual)">0</xsl:if>
                  <xsl:value-of select="Actual"/>
                </td>
                <td>
                  <xsl:if test="not(ClearedByArrest)">0</xsl:if>
                  <xsl:value-of select="ClearedByArrest"/>
                </td>
                <td>
                  <xsl:if test="not(ClearedByJuvArrest)">0</xsl:if>
                  <xsl:value-of select="ClearedByJuvArrest"/>
                </td>
                
                <!-- This is column 7 which will always be zero for NIBRS -->
                <!-- Offenses Where Structures Uninhabited, Abandoned, or not Normally in Use -->
                <td>
                  0
                </td>

                <td>
                  <xsl:choose>
                    <xsl:when test="not(EstimatedValueOfDamage)">$0.00</xsl:when>
                    <xsl:otherwise><xsl:value-of select="format-number(EstimatedValueOfDamage, '$#,###.00')"/></xsl:otherwise>
                  </xsl:choose>
                </td>                
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>