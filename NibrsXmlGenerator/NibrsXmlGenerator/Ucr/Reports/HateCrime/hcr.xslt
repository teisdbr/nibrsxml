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
                  Filing Type :
                  <xsl:choose>
                    <xsl:when test="FILINGTYPE='1'">Initial</xsl:when>
                    <xsl:when test="FILINGTYPE='2'">Adjustment</xsl:when>
                  </xsl:choose>
                </td>
              </tr>
              <tr>
                <td   class="head">
                  #Adult Offenders : <xsl:value-of select="ADULTOFFENDERSCOUNT" />
                </td>
                <td   class="head">
                  #Juvenile Offenders : <xsl:value-of select="JUVENILEOFFENDERSCOUNT" />
                </td>
              </tr>
              <tr>
                <td   class="head">
                  <div>
                    Offender Race :
                    <xsl:choose>
                      <xsl:when test="OFFENDERRACE='W'">W (White)</xsl:when>
                      <xsl:when test="OFFENDERRACE='B'">B (Black or African American)</xsl:when>
                      <xsl:when test="OFFENDERRACE='I'">I (American Indian or Alaska Native)</xsl:when>
                      <xsl:when test="OFFENDERRACE='A'">A (Asian)</xsl:when>
                      <xsl:when test="OFFENDERRACE='P'">P (Native Hawaiian or Other Pacific Islander)</xsl:when>
                      <xsl:when test="OFFENDERRACE='M'">M (Group of Multiple Races)</xsl:when>
                      <xsl:when test="OFFENDERRACE='U'">U (Unknown)</xsl:when>
                    </xsl:choose>
                  </div>
                </td>
                <td   class="head">
                  <div>
                    Offender Ethnicity :
                    <xsl:choose>
                      <xsl:when test="OFFENDERETHNICITY='H'">H (Hispanic or Latino)</xsl:when>
                      <xsl:when test="OFFENDERETHNICITY='N'">N (Not Hispanic or Latino)</xsl:when>
                      <xsl:when test="OFFENDERETHNICITY='M'">M (Group of Multiple Ethnicities)</xsl:when>
                      <xsl:when test="OFFENDERETHNICITY='U'">U (Unknown)</xsl:when>
                      <xsl:when test="OFFENDERETHNICITY='b'">b (BLANK)</xsl:when>
                    </xsl:choose>
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
                      Offense #: <xsl:value-of select="position()" />
                    </td>
                  </tr>
                  <tr>
                    <td colspan="1" >
                      Offense Code :
                      <xsl:choose>
                        <xsl:when test="OFFENSECODE='01'">01 (Murder)</xsl:when>
                        <xsl:when test="OFFENSECODE='02'">02 (Rape)</xsl:when>
                        <xsl:when test="OFFENSECODE='03'">03 (Robbery)</xsl:when>
                        <xsl:when test="OFFENSECODE='04'">04 (Aggravated Assault)</xsl:when>
                        <xsl:when test="OFFENSECODE='05'">05 (Burglary)</xsl:when>
                        <xsl:when test="OFFENSECODE='06'">06 (Larceny-Theft)</xsl:when>
                        <xsl:when test="OFFENSECODE='07'">07 (Motor Vehicle Theft)</xsl:when>
                        <xsl:when test="OFFENSECODE='08'">08 (Arson)</xsl:when>
                        <xsl:when test="OFFENSECODE='09'">09 (Simple Assault)</xsl:when>
                        <xsl:when test="OFFENSECODE='10'">10 (Intimidation)</xsl:when>
                        <xsl:when test="OFFENSECODE='11'">11 (Destruction/Damage/Vandalism)</xsl:when>
                        <xsl:when test="OFFENSECODE='12'">12 (Human Trafficking, Commercial Sex Acts)</xsl:when>
                        <xsl:when test="OFFENSECODE='13'">13 (Human Trafficking, Involuntary Servitude)</xsl:when>
                      </xsl:choose>
                    </td>
                    <td colspan="2" >
                      Location Code :
                      <xsl:choose>
                        <xsl:when test="LOCATIONCODE='1'">1 (Air/Bus/Train Terminal)</xsl:when>
                        <xsl:when test="LOCATIONCODE='2'">2 (Bank/Savings and Loan)</xsl:when>
                        <xsl:when test="LOCATIONCODE='3'">3 (Bar/Night Club)</xsl:when>
                        <xsl:when test="LOCATIONCODE='4'">4 (Church/Synagogue/Temple)</xsl:when>
                        <xsl:when test="LOCATIONCODE='5'">5 (Commercial/Office Building)</xsl:when>
                        <xsl:when test="LOCATIONCODE='6'">6 (Construction Site)</xsl:when>
                        <xsl:when test="LOCATIONCODE='7'">7 (Convenience Store)</xsl:when>
                        <xsl:when test="LOCATIONCODE='8'">8 (Department/Discount Store)</xsl:when>
                        <xsl:when test="LOCATIONCODE='9'">9 (Drug Store/Doctor Office/Hospital)</xsl:when>
                        <xsl:when test="LOCATIONCODE='10'">10 (Field/Woods)</xsl:when>
                        <xsl:when test="LOCATIONCODE='11'">11 (Government/Public Building)</xsl:when>
                        <xsl:when test="LOCATIONCODE='12'">12 (Grocery/Supermarket)</xsl:when>
                        <xsl:when test="LOCATIONCODE='13'">13 (Highway/Road/Alley/Street)</xsl:when>
                        <xsl:when test="LOCATIONCODE='14'">14 (Hotel/Motel/etc.)</xsl:when>
                        <xsl:when test="LOCATIONCODE='15'">15 (Jail/Prison)</xsl:when>
                        <xsl:when test="LOCATIONCODE='16'">16 (Lake/Waterway)</xsl:when>
                        <xsl:when test="LOCATIONCODE='17'">17 (Liquor Store)</xsl:when>
                        <xsl:when test="LOCATIONCODE='18'">18 (Parking Lot/Garage)</xsl:when>
                        <xsl:when test="LOCATIONCODE='19'">19 (Rental Storage Facility)</xsl:when>
                        <xsl:when test="LOCATIONCODE='20'">20 (Residence/Home)</xsl:when>
                        <xsl:when test="LOCATIONCODE='21'">21 (Restaurant)</xsl:when>
                        <xsl:when test="LOCATIONCODE='23'">23 (Service/Gas Station)</xsl:when>
                        <xsl:when test="LOCATIONCODE='24'">24 (Specialty Store (TV, Fur, etc.))</xsl:when>
                        <xsl:when test="LOCATIONCODE='25'">25 (Other/Unknown)</xsl:when>
                        <xsl:when test="LOCATIONCODE='37'">37 (Abandoned/Condemned Structure)</xsl:when>
                        <xsl:when test="LOCATIONCODE='38'">38 (Amusement Park)</xsl:when>
                        <xsl:when test="LOCATIONCODE='39'">39 (Arena/Stadium/Fairgrounds/Coliseum)</xsl:when>
                        <xsl:when test="LOCATIONCODE='40'">40 (ATM Separate from Bank)</xsl:when>
                        <xsl:when test="LOCATIONCODE='41'">41 (Auto Dealership New/Used)</xsl:when>
                        <xsl:when test="LOCATIONCODE='42'">42 (Camp/Campground)</xsl:when>
                        <xsl:when test="LOCATIONCODE='44'">44 (Daycare Facility)</xsl:when>
                        <xsl:when test="LOCATIONCODE='45'">45 (Dock/Wharf/Freight/Modal Terminal)</xsl:when>
                        <xsl:when test="LOCATIONCODE='46'">46 (Farm Facility)</xsl:when>
                        <xsl:when test="LOCATIONCODE='47'">47 (Gambling Facility/Casino)</xsl:when>
                        <xsl:when test="LOCATIONCODE='48'">48 (Industrial Site)</xsl:when>
                        <xsl:when test="LOCATIONCODE='49'">49 (Military Installation)</xsl:when>
                        <xsl:when test="LOCATIONCODE='50'">50 (Park/Playground)</xsl:when>
                        <xsl:when test="LOCATIONCODE='51'">51 (Rest Area)</xsl:when>
                        <xsl:when test="LOCATIONCODE='52'">52 (School-College/University)</xsl:when>
                        <xsl:when test="LOCATIONCODE='53'">53 (School-Elementary/Secondary)</xsl:when>
                        <xsl:when test="LOCATIONCODE='54'">54 (Shelter-Mission/Homeless)</xsl:when>
                        <xsl:when test="LOCATIONCODE='55'">55 (Shopping Mall)</xsl:when>
                        <xsl:when test="LOCATIONCODE='56'">56 (Tribal Lands)</xsl:when>
                        <xsl:when test="LOCATIONCODE='57'">57 (Community Center)</xsl:when>
                      </xsl:choose>
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
                      <xsl:for-each select="BIASMOTIVES">
                        <xsl:choose>
                          <xsl:when test="BIASMOTIVE='11'">11 (Anti-White)</xsl:when>
                          <xsl:when test="BIASMOTIVE='12'">12 (Anti-Black or African American)</xsl:when>
                          <xsl:when test="BIASMOTIVE='13'">13 (Anti-American Indian/Alaska Native)</xsl:when>
                          <xsl:when test="BIASMOTIVE='14'">14 (Anti-Asian)</xsl:when>
                          <xsl:when test="BIASMOTIVE='15'">15 (Anti-Multiple Races, Group)</xsl:when>
                          <xsl:when test="BIASMOTIVE='21'">21 (Anti-Jewish)</xsl:when>
                          <xsl:when test="BIASMOTIVE='22'">22 (Anti-Catholic)</xsl:when>
                          <xsl:when test="BIASMOTIVE='23'">23 (Anti-Protestant)</xsl:when>
                          <xsl:when test="BIASMOTIVE='24'">24 (Anti-Islamic (Muslim))</xsl:when>
                          <xsl:when test="BIASMOTIVE='25'">25 (Anti-Other Religion)</xsl:when>
                          <xsl:when test="BIASMOTIVE='26'">26 (Anti-Multiple Religions, Group)</xsl:when>
                          <xsl:when test="BIASMOTIVE='27'">27 (Anti-Atheism/Agnosticism)</xsl:when>
                          <xsl:when test="BIASMOTIVE='32'">32 (Anti-Hispanic or Latino)</xsl:when>
                          <xsl:when test="BIASMOTIVE='33'">33 (Anti-Other Race/Ethnicity/Ancestry)</xsl:when>
                          <xsl:when test="BIASMOTIVE='41'">41 (Anti-Gay (Male))</xsl:when>
                          <xsl:when test="BIASMOTIVE='42'">42 (Anti-Lesbian)</xsl:when>
                          <xsl:when test="BIASMOTIVE='43'">43 (Anti-Lesbian, Gay, Bisexual, or Transgender (Mixed Group))</xsl:when>
                          <xsl:when test="BIASMOTIVE='44'">44 (Anti-Heterosexual)</xsl:when>
                          <xsl:when test="BIASMOTIVE='45'">45 (Anti-Bisexual)</xsl:when>
                          <xsl:when test="BIASMOTIVE='16'">16 (Anti-Native Hawaiian or Other Pacific Islander)</xsl:when>
                          <xsl:when test="BIASMOTIVE='31'">31 (Anti-Arab)</xsl:when>
                          <xsl:when test="BIASMOTIVE='28'">28 (Anti-Mormon)</xsl:when>
                          <xsl:when test="BIASMOTIVE='29'">29 (Anti-Jehovah's Witness)</xsl:when>
                          <xsl:when test="BIASMOTIVE='81'">81 (Anti-Eastern Orthodox (Russian, Greek, Other))</xsl:when>
                          <xsl:when test="BIASMOTIVE='82'">82 (Anti-Other Christian)</xsl:when>
                          <xsl:when test="BIASMOTIVE='83'">83 (Anti-Buddhist)</xsl:when>
                          <xsl:when test="BIASMOTIVE='84'">84 (Anti-Hindu)</xsl:when>
                          <xsl:when test="BIASMOTIVE='85'">85 (Anti-Sikh)</xsl:when>
                          <xsl:when test="BIASMOTIVE='51'">51 (Anti-Physical Disability)</xsl:when>
                          <xsl:when test="BIASMOTIVE='52'">52 (Anti-Mental Disability)</xsl:when>
                          <xsl:when test="BIASMOTIVE='61'">61 (Anti-Male)</xsl:when>
                          <xsl:when test="BIASMOTIVE='62'">62 (Anti-Female)</xsl:when>
                          <xsl:when test="BIASMOTIVE='71'">71 (Anti-Transgender)</xsl:when>
                          <xsl:when test="BIASMOTIVE='72'">72 (Anti-Gender Nonconforming)</xsl:when>
                        </xsl:choose>
                        <xsl:if test="position() != last()">
                          <br />
                        </xsl:if>
                      </xsl:for-each>
                    </td>
                  </tr>
                  <tr >
                    <td colspan="3">
                      Victim Type(s) :
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