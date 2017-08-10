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
          .title{
          border:0px;
          font-size:20px;
          }
        </style>
      </head>
      <body>
        <table class="page-wrapper">
          <!--Page header-->
          <thead>
            <tr>
              <th colspan="2">
                Quarterly Hate Crime Report<br />
                (Offenses Known to Law Enforcement)
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
            <!--<tr>
              <td>
                <div class="legend-wrapper">
                  -->
            <!--Race
                  Ethnicity
                  Offense Code
                  Location Code
                  Bias Motive
                  Victim Type-->
            <!--
                </div>
              </td>
            </tr>-->
          </thead>
          <tbody>
            <tr>
              <td colspan="2">
                <!--Page content-->
                <table>
                  <thead>
                    <tr>
                      <th rowspan="3">Incident Date</th>
                      <th rowspan="3">Incident ID</th>
                      <th rowspan="2" colspan="2">Number of Offenders</th>
                      <th colspan="17">Related Offenses</th>
                    </tr>
                    <tr>
                      <th rowspan="2">Offense Sequence Number</th>
                      <th rowspan="2">Offense Code</th>
                      <th rowspan="2">Location Code</th>
                      <th colspan="2">Number of Victims</th>
                      <th colspan="5">Bias Motivation Codes</th>
                      <th colspan="8">Victim Types</th>
                    </tr>
                    <tr>
                      <th>Adult</th>
                      <th>Juvenile</th>
                      <th>Adult</th>
                      <th>Juvenile</th>
                      <th>1</th>
                      <th>2</th>
                      <th>3</th>
                      <th>4</th>
                      <th>5</th>
                      <th>I</th>
                      <th>B</th>
                      <th>F</th>
                      <th>G</th>
                      <th>R</th>
                      <th>O</th>
                      <th>U</th>
                    </tr>
                  </thead>
                  <tbody>
                    <xsl:for-each select="UcrReports/HCR/INCIDENTS/INCIDENT">
                      <tr>
                        <td rowspan="{count(OFFENSES/OFFENSE) + 1}">
                          <xsl:value-of select="INCIDENTDATE"/>
                        </td>
                        <td rowspan="{count(OFFENSES/OFFENSE) + 1}">
                          <xsl:value-of select="INCIDENTNUM"/>
                        </td>
                        <td rowspan="{count(OFFENSES/OFFENSE) + 1}">
                          <xsl:value-of select="ADULTOFFENDERSCOUNT"/>
                        </td>
                        <td rowspan="{count(OFFENSES/OFFENSE) + 1}">
                          <xsl:value-of select="JUVENILEOFFENDERSCOUNT"/>
                        </td>
                      </tr>
                      <xsl:for-each select="OFFENSES/OFFENSE">
                        <tr>
                          <td>
                            <xsl:value-of select="position()"/>
                          </td>
                          <td>
                            <xsl:value-of select="OFFENSECODE"/>
                          </td>
                          <td>
                            <xsl:value-of select="LOCATIONCODE"/>
                          </td>
                          <td>
                            <xsl:value-of select="ADULTVICTIMSCOUNT"/>
                          </td>
                          <td>
                            <xsl:value-of select="JUVENILEVICTIMSCOUNT"/>
                          </td>
                          <td>
                            <xsl:value-of select="BIASMOTIVES/BIASMOTIVE[position() = 1]/@CODE"/>
                          </td>
                          <td>
                            <xsl:value-of select="BIASMOTIVES/BIASMOTIVE[position() = 2]/@CODE"/>
                          </td>
                          <td>
                            <xsl:value-of select="BIASMOTIVES/BIASMOTIVE[position() = 3]/@CODE"/>
                          </td>
                          <td>
                            <xsl:value-of select="BIASMOTIVES/BIASMOTIVE[position() = 4]/@CODE"/>
                          </td>
                          <td>
                            <xsl:value-of select="BIASMOTIVES/BIASMOTIVE[position() = 5]/@CODE"/>
                          </td>
                          <td>
                            <input type="checkbox">
                              <xsl:if test="VICTIMTYPE/INDIVIDUAL = 1">
                                <xsl:attribute name="checked"></xsl:attribute>
                              </xsl:if>
                              <xsl:attribute name="disabled"></xsl:attribute>
                            </input>
                          </td>
                          <td>
                            <input type="checkbox">
                              <xsl:if test="VICTIMTYPE/BUSINESS = 1">
                                <xsl:attribute name="checked"></xsl:attribute>
                              </xsl:if>
                              <xsl:attribute name="disabled"></xsl:attribute>
                            </input>
                          </td>
                          <td>
                            <input type="checkbox">
                              <xsl:if test="VICTIMTYPE/FINANCIAL = 1">
                                <xsl:attribute name="checked"></xsl:attribute>
                              </xsl:if>
                              <xsl:attribute name="disabled"></xsl:attribute>
                            </input>
                          </td>
                          <td>
                            <input type="checkbox">
                              <xsl:if test="VICTIMTYPE/GOVERNMENT = 1">
                                <xsl:attribute name="checked"></xsl:attribute>
                              </xsl:if>
                              <xsl:attribute name="disabled"></xsl:attribute>
                            </input>
                          </td>
                          <td>
                            <input type="checkbox">
                              <xsl:if test="VICTIMTYPE/RELIGIOUS = 1">
                                <xsl:attribute name="checked"></xsl:attribute>
                              </xsl:if>
                              <xsl:attribute name="disabled"></xsl:attribute>
                            </input>
                          </td>
                          <td>
                            <input type="checkbox">
                              <xsl:if test="VICTIMTYPE/OTHER = 1">
                                <xsl:attribute name="checked"></xsl:attribute>
                              </xsl:if>
                              <xsl:attribute name="disabled"></xsl:attribute>
                            </input>
                          </td>
                          <td>
                            <input type="checkbox">
                              <xsl:if test="VICTIMTYPE/UNKNOWN = 1">
                                <xsl:attribute name="checked"></xsl:attribute>
                              </xsl:if>
                              <xsl:attribute name="disabled"></xsl:attribute>
                            </input>
                          </td>
                        </tr>
                      </xsl:for-each>
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