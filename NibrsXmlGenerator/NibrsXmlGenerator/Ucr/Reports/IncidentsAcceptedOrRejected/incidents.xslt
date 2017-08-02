<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
      <body>
        <table class="wrapper">
          <thead>
            <tr>
              <th style="text-align:center;" colspan="3">
                Supplementary Homicide Report
              </th>
            </tr>
            <tr>
              <td class="small">
                <xsl:value-of select="UcrReports/@agency" />
              </td>
              <td class="small">
                <xsl:value-of select="UcrReports/@ori" />
              </td>
              <td class="small">
                <xsl:value-of select="concat(UcrReports/@year, ' ', UcrReports/@month)" />
              </td>
            </tr>
            <tr>
              <th>Incident ID</th>
              <th>Accepted</th>
              <th>Rejected</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="UcrReports/IncidentsAcceptedOrRejected/Incident">
              <tr>
                <td>
                  <xsl:value-of select="@id"/>
                </td>
                <td>
                  <xsl:if test="@accepted = 1">X</xsl:if>
                </td>
                <td>
                  <xsl:if test="@accepted = 0">X</xsl:if>
                </td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>