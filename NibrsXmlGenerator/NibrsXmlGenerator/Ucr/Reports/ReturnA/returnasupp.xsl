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
          .table-pad-bot {
            padding-bottom: 10px;
          }
        </style>
      </head>
      <body>
        <table class='table-pad-bot'>
          <colgroup span="3"></colgroup>
          <thead>
            <tr>
              <th colspan="3" scope="colgroup">Supplement to Return A - Property by Type and Value</th>
            </tr>
            <tr>
              <th scope="col">Property Type</th>
              <th scope="col">Stolen Value</th>
              <th scope="col">Recovered Value</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="ReturnASupplement/StolenAndRecoveredProperties/Property">
              <tr>
                <!--Stolen and Recovered Properties by Type and Value - row header-->
                <th class="rowheader">
                  <xsl:choose>
                    <xsl:when test="@entry='01'">
                      <xsl:value-of select="'Currency, Notes, Etc.'"/>
                    </xsl:when>
                    <xsl:when test="@entry='02'">
                      <xsl:value-of select="'Jewelry and Precious Metals'"/>
                    </xsl:when>
                    <xsl:when test="@entry='03'">
                      <xsl:value-of select="'Clothing and Furs'"/>
                    </xsl:when>
                    <xsl:when test="@entry='04'">
                      <xsl:value-of select="'Locally Stolen Motor Vehicles'"/>
                    </xsl:when>
                    <xsl:when test="@entry='05'">
                      <xsl:value-of select="'Office Equipment'"/>
                    </xsl:when>
                    <xsl:when test="@entry='06'">
                      <xsl:value-of select="'Televisions, Radios, Stereos, Etc.'"/>
                    </xsl:when>
                    <xsl:when test="@entry='07'">
                      <xsl:value-of select="'Firearms'"/>
                    </xsl:when>
                    <xsl:when test="@entry='08'">
                      <xsl:value-of select="'Household Goods'"/>
                    </xsl:when>
                    <xsl:when test="@entry='09'">
                      <xsl:value-of select="'Consumable Goods'"/>
                    </xsl:when>
                    <xsl:when test="@entry='10'">
                      <xsl:value-of select="'Livestock'"/>
                    </xsl:when>
                    <xsl:when test="@entry='11'">
                      <xsl:value-of select="'Miscellaneous'"/>
                    </xsl:when>
                    <xsl:when test="@entry='00'">
                      <xsl:value-of select="'TOTAL'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="@entry"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </th>
                <td>
                  <xsl:choose>
                    <xsl:when test="not(Stolen)">$0.00</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="format-number(Stolen, '$#,###.00')"/>
                    </xsl:otherwise>
                </xsl:choose>
                </td>
                <td>
                  <xsl:choose>
                    <xsl:when test="not(Recovered)">$0.00</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="format-number(Recovered, '$#,###.00')"/>
                    </xsl:otherwise>
                </xsl:choose>
                </td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
        <table>
          <colgroup span="3"></colgroup>
          <thead>
            <tr>
              <th colspan="3" scope="colgroup">Supplement to Return A - Property Stolen by Classification</th>
            </tr>
            <tr>
              <th scope="col">Classification</th>
              <th scope="col">Number of Actual Offenses</th>
              <th scope="col">Stolen Value</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="ReturnASupplement/StolenPropertiesByClassification/OffenseClassification">
              <tr>
                <!--Properties Stolen by Classification - row header-->
                <th class="rowheader">
                  <xsl:choose>
                    <xsl:when test="@entry='12'">
                      <xsl:value-of select="'Murder and Nonnegligent Manslaughter'"/>
                    </xsl:when>
                    <xsl:when test="@entry='20'">
                      <xsl:value-of select="'Rape'"/>
                    </xsl:when>
                    <xsl:when test="@entry='31'">
                      <xsl:value-of select="'Robbery - Highway (Streets, Alleys, Etc.)'"/>
                    </xsl:when>
                    <xsl:when test="@entry='32'">
                      <xsl:value-of select="'Robbery - Comercial House'"/>
                    </xsl:when>
                    <xsl:when test="@entry='33'">
                      <xsl:value-of select="'Robbery - Gas or Service Station'"/>
                    </xsl:when>
                    <xsl:when test="@entry='34'">
                      <xsl:value-of select="'Robbery - Convenience Store'"/>
                    </xsl:when>
                    <xsl:when test="@entry='35'">
                      <xsl:value-of select="'Robbery - Residence'"/>
                    </xsl:when>
                    <xsl:when test="@entry='36'">
                      <xsl:value-of select="'Robbery - Bank'"/>
                    </xsl:when>
                    <xsl:when test="@entry='37'">
                      <xsl:value-of select="'Robbery - Miscellaneous'"/>
                    </xsl:when>
                    <xsl:when test="@entry='30'">
                      <xsl:value-of select="'Total Robbery'"/>
                    </xsl:when>
                    <xsl:when test="@entry='51'">
                      <xsl:value-of select="'Burglary/Breaking or Entering - Residence, Night (6 p.m. to 6 a.m.)'"/>
                    </xsl:when>
                    <xsl:when test="@entry='52'">
                      <xsl:value-of select="'Burglary/Breaking or Entering - Residence, Day (6 a.m. to 6 p.m.)'"/>
                    </xsl:when>
                    <xsl:when test="@entry='53'">
                      <xsl:value-of select="'Burglary/Breaking or Entering - Residence, Unknown'"/>
                    </xsl:when>
                    <xsl:when test="@entry='54'">
                      <xsl:value-of select="'Burglary/Breaking or Entering - Nonresidence, Night (6 p.m. to 6 a.m.)'"/>
                    </xsl:when>
                    <xsl:when test="@entry='55'">
                      <xsl:value-of select="'Burglary/Breaking or Entering - Nonresidence, Day (6 a.m. to 6 p.m.)'"/>
                    </xsl:when>
                    <xsl:when test="@entry='56'">
                      <xsl:value-of select="'Burglary/Breaking or Entering - Nonresidence, Unknown'"/>
                    </xsl:when>
                    <xsl:when test="@entry='50'">
                      <xsl:value-of select="'Total Burglary'"/>
                    </xsl:when>
                    <xsl:when test="@entry='61'">
                      <xsl:value-of select="'Larceny - $200+'"/>
                    </xsl:when>
                    <xsl:when test="@entry='62'">
                      <xsl:value-of select="'Larceny - $50 - $199'"/>
                    </xsl:when>
                    <xsl:when test="@entry='63'">
                      <xsl:value-of select="'Larceny - Below $50'"/>
                    </xsl:when>
                    <xsl:when test="@entry='60'">
                      <xsl:value-of select="'Total Larceny'"/>
                    </xsl:when>
                    <xsl:when test="@entry='70'">
                      <xsl:value-of select="'Motor Vehicle Theft'"/>
                    </xsl:when>
                    <xsl:when test="@entry='77'">
                      <xsl:value-of select="'GRAND TOTAL'"/>
                    </xsl:when>
                    <xsl:when test="@entry='81'">
                      <xsl:value-of select="'Pocket-Picking'"/>
                    </xsl:when>
                    <xsl:when test="@entry='82'">
                      <xsl:value-of select="'Purse-Snatching'"/>
                    </xsl:when>
                    <xsl:when test="@entry='83'">
                      <xsl:value-of select="'Shoplifting'"/>
                    </xsl:when>
                    <xsl:when test="@entry='84'">
                      <xsl:value-of select="'From Motor Vehicles'"/>
                    </xsl:when>
                    <xsl:when test="@entry='85'">
                      <xsl:value-of select="'Motor Vehicle Parts and Accessories'"/>
                    </xsl:when>
                    <xsl:when test="@entry='86'">
                      <xsl:value-of select="'Bicycles'"/>
                    </xsl:when>
                    <xsl:when test="@entry='87'">
                      <xsl:value-of select="'From Building'"/>
                    </xsl:when>
                    <xsl:when test="@entry='88'">
                      <xsl:value-of select="'From Coin-Operated Machine'"/>
                    </xsl:when>
                    <xsl:when test="@entry='89'">
                      <xsl:value-of select="'All Other'"/>
                    </xsl:when>
                    <xsl:when test="@entry='80'">
                      <xsl:value-of select="'Total Larceny'"/>
                    </xsl:when>
                    <xsl:when test="@entry='91'">
                      <xsl:value-of select="'Motor Vehicles Recovered - Stolen Locally and Recovered Locally'"/>
                    </xsl:when>
                    <xsl:when test="@entry='92'">
                      <xsl:value-of select="'Motor Vehicles Recovered - Stolen Locally and Recovered by Other Jurisdictions'"/>
                    </xsl:when>
                    <xsl:when test="@entry='90'">
                      <xsl:value-of select="'Motor Vehicles Recovered - Total Locally Stolen Motor Vehicles Recovered'"/>
                    </xsl:when>
                    <xsl:when test="@entry='93'">
                      <xsl:value-of select="'Motor Vehicles Recovered - Stolen in Other Jurisdictions and Recovered Locally'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="@entry"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </th>
                <td>
                  <xsl:choose>
                    <xsl:when test="not(Actual)">0</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="format-number(Actual, '#,###')"/>
                    </xsl:otherwise>
                </xsl:choose>
                </td>
                <td>
                  <xsl:choose>
                    <xsl:when test="not(Stolen)">$0.00</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="format-number(Stolen, '$#,###.00')"/>
                    </xsl:otherwise>
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