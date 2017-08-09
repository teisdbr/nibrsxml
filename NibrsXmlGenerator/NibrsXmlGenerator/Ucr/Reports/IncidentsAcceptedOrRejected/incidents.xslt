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
        <table class="page-wrapper">
          <!--Page header-->
          <thead>
            <tr>
              <th style="text-align:center;" colspan="3">
                Incidents Accepted or Rejected
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
                <table>
                  <thead>
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