<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
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
                <th colspan="2">Monthly Return of Human Trafficking Offenses Known to Law Enforcement</th>
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
                    <xsl:for-each select="UcrReports/HumanTraffickingSummary">
                      <colgroup span="6"></colgroup>
                      <thead>
                        <tr>
                          <th>1</th>
                          <th>2</th>
                          <th>3</th>
                          <th>4</th>
                          <th>5</th>
                          <th>6</th>
                        </tr>
                        <tr>
                          <th>Human Trafficking Classification</th>
                          <th>Offenses Reported</th>
                          <th>Unfounded, i.e., False or Baseless Complaints</th>
                          <th>Number of Actual Offenses (Column 2 Minus Column 3) (Include Attempts)</th>
                          <th>Total Offenses Clears by Arrest or Exceptional Means</th>
                          <th>Number of Clearances Involving Only Persons Under 18 Years of Age</th>
                        </tr>
                      </thead>
                      <tbody>
                        <xsl:for-each select="Classification">
                          <xsl:sort select="@name" />
                          <tr>
                            <!--The row header-->
                            <th class="rowheader">
                              <xsl:choose>
                                <xsl:when test="@name='A'">
                                  <xsl:value-of select="'A. Commercial Sex Acts'" />
                                </xsl:when>
                                <xsl:when test="@name='B'">
                                  <xsl:value-of select="'B. Involuntary Servitude'" />
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="@name" />
                                </xsl:otherwise>
                              </xsl:choose>
                            </th>

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
                    </xsl:for-each>
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