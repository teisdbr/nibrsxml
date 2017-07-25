<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          body {
          font-size: 20px;
          }
          .head{
          border: 0px;
          text-align: left;
          font-weight: bold;
          padding:0px;
          }
          .small{
          border: 0px;
          text-align: left;
          font-weight: bold;
          font-size: 15px;
          padding:0px;
          }
          td {
          border: 1px solid black;
          text-align: left;
          padding:10px;
          }
          table {
          width: 100%;
          border-spacing: 0px;
          border-collapse: collapse;
          page-break-inside: avoid;
          }
          div{
          white-space:nowrap;
          }
          @media print {
          table{
          page-break-inside: avoid;
          page-break-after: always;
          }
          div.body {
          page-break-inside: avoid;
          <!--page-break-after: always;-->
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
                <th  colspan="3" style="text-align:center;">
                  Quarterly Hate Crime Report<br/>(Offenses Known to Law Enforcement)
                  </th>
              </tr>
              <tr >
                <td  class="small">
                  <xsl:value-of select="concat(../../@AGENCY,'  ',../../@ORI)" />
                </td>
                <td  class="small">
                  <xsl:value-of select="../../@QUARTER" />
                  <br />
                  <br />
                </td>          
              </tr>

             </thead>
            <tbody>
              <tr>
                <td colspan="3" class="head">
                  Incident Date : <xsl:value-of select="INCIDENTDATE" />

                </td>
              </tr>
              <tr>
                <td colspan="3" class="head">

                  Incident # : <xsl:value-of select="INCIDENTNUM" />

                </td>
              </tr>
              <tr>
                <td colspan="3" class="head">
                  Filing Type : <xsl:value-of select="FILINGTYPE" />
                </td>
              </tr>
              <tr>
                <td   class="head">
                  #Adult Offenders: <xsl:value-of select="ADULTOFFENDERSCOUNT" />
                </td>
                <td   class="head">
                  #Juvenile Offenders : <xsl:value-of select="JUVENILEOFFENDERSCOUNT" />
                </td>
              </tr>
              <tr>
                <td   class="head">
                  <div>
                  Offender Race :<xsl:value-of select="OFFENDERRACE" />
                  </div>
                </td>
                <td   class="head">
                  <div>
                  Offender Ethnicity :<xsl:value-of select="OFFENDERETHNICITY" />
                  </div>
                </td>
              </tr>
              <tr>
                <td colspan="3" class="head">
                  Offenses for this Incident # :
                </td>
              </tr>
              <xsl:for-each select="OFFENSES/OFFENSE">
                <div class="body">
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
                  <td  >
                    # Juvenile Victims : <xsl:value-of select="JUVENILEVICTIMSCOUNT" />
                  </td>
                </tr>
                <tr>
                  <td  style="text-align:left;border-right:1px;">
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
                    Victim Type(s): 
                    <xsl:if test="VICTIMTYPE/INDIVIDUAL='1'">Individual</xsl:if>
                    <xsl:if test="VICTIMTYPE/BUSINESS='1'"> Business</xsl:if>
                    <xsl:if test="VICTIMTYPE/FINANCIAL='1'"> Financial-Institution</xsl:if>
                    <xsl:if test="VICTIMTYPE/GOVERNMENT='1'"> Government</xsl:if>
                    <xsl:if test="VICTIMTYPE/RELIGIOUS='1'"> Religious-Organization</xsl:if>
                    <xsl:if test="VICTIMTYPE/OTHER='1'"> Other</xsl:if>
                    <xsl:if test="VICTIMTYPE/UNKNOWN='1'"> Unknown</xsl:if>
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