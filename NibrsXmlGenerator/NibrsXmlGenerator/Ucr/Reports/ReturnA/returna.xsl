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
        </style>
      </head>
      <body>
        <table>
          <colgroup span="6"></colgroup>
          <thead>
            <tr>
              <th colspan="6" scope="colgroup">Return A - Monthly Return of Offenses Known to the Police</th>
            </tr>
            <tr>
              <th scope="col">1</th>
              <th scope="col">2</th>
              <th scope="col">3</th>
              <th scope="col">4</th>
              <th scope="col">5</th>
              <th scope="col">6</th>
            </tr>
            <tr>
              <th scope="col">Classification of Offenses</th>
              <th scope="col">Offenses Reported</th>
              <th scope="col">Unfounded, i.e., False or Baseless Complaints</th>
              <th scope="col">Number of Actual Offenses (Column 2 Minus Column 3) (Include Attempts)</th>
              <th scope="col">Total Offenses Clears by Arrest or Exceptional Means</th>
              <th scope="col">Number of Clearances Involving Only Persons Under 18 Years of Age</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="ReturnASummary/Classification">
              <tr>
                <!--The row header-->
                <th class="rowheader">
                  <xsl:choose>
                    <xsl:when test="@name='1'">
                      <xsl:value-of select="'1. CRIMINAL HOMICIDE TOTAL'"/>
                    </xsl:when>
                    <xsl:when test="@name='1a'">
                      <xsl:value-of select="'a. Murder &amp; Nonnegligent Homicide'"/>
                    </xsl:when>
                    <xsl:when test="@name='1b'">
                      <xsl:value-of select="'b. Manslaughter by Negligence'"/>
                    </xsl:when>
                    <xsl:when test="@name='2'">
                      <xsl:value-of select="'RAPE TOTAL'"/>
                    </xsl:when>
                    <xsl:when test="@name='2a'">
                      <xsl:value-of select="'a. Rape'"/>
                    </xsl:when>
                    <xsl:when test="@name='2b'">
                      <xsl:value-of select="'b. Attempted Rape'"/>
                    </xsl:when>
                    <xsl:when test="@name='3'">
                      <xsl:value-of select="'ROBBERY TOTAL'"/>
                    </xsl:when>
                    <xsl:when test="@name='3a'">
                      <xsl:value-of select="'a. Firearm'"/>
                    </xsl:when>
                    <xsl:when test="@name='3b'">
                      <xsl:value-of select="'b. Knife or Cutting Instrument'"/>
                    </xsl:when>
                    <xsl:when test="@name='3c'">
                      <xsl:value-of select="'c. Other Dangerous Weapon'"/>
                    </xsl:when>
                    <xsl:when test="@name='3d'">
                      <xsl:value-of select="'d. Strong-Arm (Hands, Fists, Feet, Etc.)'"/>
                    </xsl:when>
                    <xsl:when test="@name='4'">
                      <xsl:value-of select="'ASSAULT TOTAL'"/>
                    </xsl:when>
                    <xsl:when test="@name='4a'">
                      <xsl:value-of select="'a. Firearm'"/>
                    </xsl:when>
                    <xsl:when test="@name='4b'">
                      <xsl:value-of select="'b. Knife or Cutting Instrument'"/>
                    </xsl:when>
                    <xsl:when test="@name='4c'">
                      <xsl:value-of select="'c. Other Dangerous Weapon'"/>
                    </xsl:when>
                    <xsl:when test="@name='4d'">
                      <xsl:value-of select="'d. Strong-Arm (Hands, Fists, Feet, Etc.)'"/>
                    </xsl:when>
                    <xsl:when test="@name='4e'">
                      <xsl:value-of select="'e. Other Assaults - Simple, Not Aggravated'"/>
                    </xsl:when>
                    <xsl:when test="@name='5'">
                      <xsl:value-of select="'BURGLARY TOTAL'"/>
                    </xsl:when>
                    <xsl:when test="@name='5a'">
                      <xsl:value-of select="'a. Forcible Entry'"/>
                    </xsl:when>
                    <xsl:when test="@name='5b'">
                      <xsl:value-of select="'b. Unlawful Entry - No Force'"/>
                    </xsl:when>
                    <xsl:when test="@name='5c'">
                      <xsl:value-of select="'c. Attempted Forcible Entry'"/>
                    </xsl:when>
                    <xsl:when test="@name='6'">
                      <xsl:value-of select="'LARCENY TOTAL'"/>
                    </xsl:when>
                    <xsl:when test="@name='7'">
                      <xsl:value-of select="'MOTOR VEHICLE THEFT TOTAL'"/>
                    </xsl:when>
                    <xsl:when test="@name='7a'">
                      <xsl:value-of select="'a. Autos'"/>
                    </xsl:when>
                    <xsl:when test="@name='7b'">
                      <xsl:value-of select="'b. Trucks and Buses'"/>
                    </xsl:when>
                    <xsl:when test="@name='7c'">
                      <xsl:value-of select="'c. Other Vehicles'"/>
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
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>