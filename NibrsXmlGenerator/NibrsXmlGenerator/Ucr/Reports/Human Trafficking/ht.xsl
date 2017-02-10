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
        </style>
      </head>
      <body>
        <table>
          <colgroup span="6"></colgroup>
          <thead>
            <tr>
              <th colspan="6" scope="colgroup">Monthly Return of Human Trafficking Offenses Known to Law Enforcement</th>
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
              <th scope="col">Human Trafficking Classification</th>
              <th scope="col">Offenses Reported</th>
              <th scope="col">Unfounded, i.e., False or Baseless Complaints</th>
              <th scope="col">Number of Actual Offenses (Column 2 Minus Column 3) (Include Attempts)</th>
              <th scope="col">Total Offenses Clears by Arrest or Exceptional Means</th>
              <th scope="col">Number of Clearances Involving Only Persons Under 18 Years of Age</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="HumanTraffickingSummary/Classification">
              <xsl:sort select="./@name"/>
              <tr>
                <th>
                  <xsl:variable name="ucrCode" select="./@name"/>
                  <xsl:choose>
                    <xsl:when test="//UCRDescription[@value=$ucrCode]">
                      <xsl:value-of select="//UCRDescription[@value=$ucrCode]"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="@name"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </th>

                <!-- Offenses Reported is same as Actual -->
                <td><xsl:if test="not(Actual)">0</xsl:if><xsl:value-of select="Actual"/></td>

                <!-- Unfounded Offenses will always be 0 -->
                <td>0</td>

                <td><xsl:if test="not(Actual)">0</xsl:if><xsl:value-of select="Actual"/></td>
                <td><xsl:if test="not(ClearedByArrest)">0</xsl:if><xsl:value-of select="ClearedByArrest"/></td>
                <td><xsl:if test="not(ClearedByJuvArrest)">0</xsl:if><xsl:value-of select="ClearedByJuvArrest"/></td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>