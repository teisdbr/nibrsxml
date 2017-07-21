<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          body {
          font-size: 20px;
          }
          th{border: 0px ;
          text-align: left;}
          td {
          border: 1px solid black;
          text-align: left;
          }
          table {
          width: 1200px;
          border-spacing: 0px;
          border-collapse: separate;
          }
          .rowheader {
          text-align: left;
          }
          @media print {
          table{
          page-break-inside: avoid;
          page-break-after: always;
          }
          thead {
          display: table-header-group;
          vertical-align: middle;
          border-color: inherit;
          }
          @page{
          <!--letter=1700px by 2200px@200 DPI-->
          size: letter portrait;
          margin: 5px ;
          }
          div {
          page-break-inside: avoid;
          <!--page-break-after: always;-->
          }
          }
          }
        </style>
      </head>
      <body>
        <xsl:for-each select="HCR/INCIDENTS/INCIDENT">
          <table>
            <colgroup span="3"></colgroup>
            <thead>
              <tr>
                <th colspan="3" scope="colgroup" style="text-align:center;">
                  Quarterly Hate Crime Report<br/>(Offenses Known to Law Enforcement)
                  </th>
              </tr>

              <tr>
                <th >
                  Incident Date : <xsl:value-of select="INCIDENTDATE" />

                </th>
              </tr>
              <tr>
                <th >

                  Incident # : <xsl:value-of select="INCIDENTNUM" />

                </th>
              </tr>
              <tr>
                <th>
                  Filing Type : <xsl:value-of select="FILINGTYPE" />
                </th>
              </tr>
              <tr>
                <th  colspan="2">
                  #Adult Offenders: <xsl:value-of select="ADULTOFFENDERSCOUNT" />
                </th>
                <th>
                  #Juvenile Offenders : <xsl:value-of select="JUVENILEOFFENDERSCOUNT" />
                </th>
              </tr>
              <tr>
                <th  colspan="2">
                  Offender Race :<xsl:value-of select="OFFENDERRACE" />
                </th>
                <th>
                  Offender Ethnicity :<xsl:value-of select="OFFENDERETHNICITY" />
                </th>
              </tr>
              <tr>
                <th>
                  Offenses for this Incident # :
                </th>
              </tr>
            </thead>
            <tbody>
              <xsl:for-each select="OFFENSES/OFFENSE">
                <div>
                <tr>
                  <td colspan="3" style="text-align:left;border:0px;" >
                    <!--Not sure if offense # is total count or the occurence-->
                    Offense #: <xsl:value-of select="position()" />
                  </td>
                </tr>
                <tr>
                  <td colspan="1" >
                    Offense Code :<xsl:value-of select="OFFENSECODE" />
                  </td>
                  <td colspan="2" >
                    Location Code :<xsl:value-of select="LOCATIONCODE" />
                  </td>
                </tr>

                <tr>
                  <td  >
                    # Adult Victims : <xsl:value-of select="ADULTVICTIMSCOUNT" />
                  </td >
                  <td colspan="2" >
                    # Juvenile Victims : <xsl:value-of select="JUVENILEVICTIMSCOUNT" />
                  </td>
                </tr>
                <tr>
                  <td colspan="1" style="text-align:left;border-right:1px;">
                    Bias Motive (s) :
                  </td>
                  <td colspan="2" style="text-align:left;border-left:1px;">
                    <xsl:for-each select="BIASMOTIVES/BIASMOTIVE">
                      <xsl:value-of select="DESCRIPTION" />
                      <xsl:if test="position() != last()">
                        <br />
                      </xsl:if>
                    </xsl:for-each>
                  </td>
                </tr>
                <tr >
                  <td colspan="3">
                    Victim Type(s): <xsl:value-of select="VICTIMTYPE"></xsl:value-of>
                  </td>
                </tr>
                </div>
              </xsl:for-each>
            </tbody>
          </table>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>