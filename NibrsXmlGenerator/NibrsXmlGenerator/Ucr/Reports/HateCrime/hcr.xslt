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
                    <xsl:when test="UcrReports/@month=1 or UcrReports/@month=2 or UcrReports/@month=3">Quarter 1: January-March </xsl:when>
                    <xsl:when test="UcrReports/@month=4 or UcrReports/@month=5 or UcrReports/@month=6">Quarter 2: April-June </xsl:when>
                    <xsl:when test="UcrReports/@month=7 or UcrReports/@month=8 or UcrReports/@month=9">Quarter 3: July-September </xsl:when>
                    <xsl:when test="UcrReports/@month=10 or UcrReports/@month=11 or UcrReports/@month=12">Quarter 4: October-December </xsl:when>
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
                  <div class="page-content data-table-container">
                    <table>
                      <thead>
                        <tr>
                          <th rowspan="3">Incident Date</th>
                          <th rowspan="3">Incident ID</th>
                          <th rowspan="2" colspan="2">Number of Offenders</th>
                          <th rowspan="3">Offender(s) Race</th>
                          <th rowspan="3">Offender(s) Ethnicity</th>
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
                              <xsl:value-of select="substring-before(INCIDENTDATE, 'T')"/>
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
                            <td rowspan="{count(OFFENSES/OFFENSE) + 1}">
                              <xsl:value-of select="OFFENDERRACE"/>
                            </td>
                            <td rowspan="{count(OFFENSES/OFFENSE) + 1}">
                              <xsl:value-of select="OFFENDERETHNICITY"/>
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
                              <td class="checkbox-container">
                                <input type="checkbox">
                                  <xsl:if test="VICTIMTYPE/INDIVIDUAL = 1">
                                    <xsl:attribute name="checked"></xsl:attribute>
                                  </xsl:if>
                                </input>
                              </td>
                              <td class="checkbox-container">
                                <input type="checkbox">
                                  <xsl:if test="VICTIMTYPE/BUSINESS = 1">
                                    <xsl:attribute name="checked"></xsl:attribute>
                                  </xsl:if>
                                </input>
                              </td>
                              <td class="checkbox-container">
                                <input type="checkbox">
                                  <xsl:if test="VICTIMTYPE/FINANCIAL = 1">
                                    <xsl:attribute name="checked"></xsl:attribute>
                                  </xsl:if>
                                </input>
                              </td>
                              <td class="checkbox-container">
                                <input type="checkbox">
                                  <xsl:if test="VICTIMTYPE/GOVERNMENT = 1">
                                    <xsl:attribute name="checked"></xsl:attribute>
                                  </xsl:if>
                                </input>
                              </td>
                              <td class="checkbox-container">
                                <input type="checkbox">
                                  <xsl:if test="VICTIMTYPE/RELIGIOUS = 1">
                                    <xsl:attribute name="checked"></xsl:attribute>
                                  </xsl:if>
                                </input>
                              </td>
                              <td class="checkbox-container">
                                <input type="checkbox">
                                  <xsl:if test="VICTIMTYPE/OTHER = 1">
                                    <xsl:attribute name="checked"></xsl:attribute>
                                  </xsl:if>
                                </input>
                              </td>
                              <td class="checkbox-container">
                                <input type="checkbox">
                                  <xsl:if test="VICTIMTYPE/UNKNOWN = 1">
                                    <xsl:attribute name="checked"></xsl:attribute>
                                  </xsl:if>
                                </input>
                              </td>
                            </tr>
                          </xsl:for-each>
                        </xsl:for-each>
                      </tbody>
                    </table>
                  </div>
                </td>
              </tr>
              <tr>
                <td colspan="2">
                  <!--Legend-->
                  <div class="dictionary-container">
                    <div class="dictionary">
                      <span class="dictionary-header">Bias Motivation</span>

                      <div>
                        <span class="dictionary-key">11</span>
                        <span class="dictionary-value">Anti-White</span>
                      </div>

                      <div>
                        <span class="dictionary-key">12</span>
                        <span class="dictionary-value">Anti-Black or African American</span>
                      </div>

                      <div>
                        <span class="dictionary-key">13</span>
                        <span class="dictionary-value">Anti-American Indian/Alaska Native</span>
                      </div>

                      <div>
                        <span class="dictionary-key">14</span>
                        <span class="dictionary-value">Anti-Asian</span>
                      </div>

                      <div>
                        <span class="dictionary-key">15</span>
                        <span class="dictionary-value">Anti-Multiple Races, Group</span>
                      </div>

                      <div>
                        <span class="dictionary-key">16</span>
                        <span class="dictionary-value">Anti-Native Hawaiian or Other Pacific Islander</span>
                      </div>

                      <div>
                        <span class="dictionary-key">21</span>
                        <span class="dictionary-value">Anti-Jewish</span>
                      </div>

                      <div>
                        <span class="dictionary-key">22</span>
                        <span class="dictionary-value">Anti-Catholic</span>
                      </div>

                      <div>
                        <span class="dictionary-key">23</span>
                        <span class="dictionary-value">Anti-Protestant</span>
                      </div>

                      <div>
                        <span class="dictionary-key">24</span>
                        <span class="dictionary-value">Anti-Islamic (Muslim)</span>
                      </div>

                      <div>
                        <span class="dictionary-key">25</span>
                        <span class="dictionary-value">Anti-Other Religion</span>
                      </div>

                      <div>
                        <span class="dictionary-key">26</span>
                        <span class="dictionary-value">Anti-Multiple Religions, Group</span>
                      </div>

                      <div>
                        <span class="dictionary-key">27</span>
                        <span class="dictionary-value">Anti-Atheism/Agnosticism</span>
                      </div>

                      <div>
                        <span class="dictionary-key">28</span>
                        <span class="dictionary-value">Anti-Mormon</span>
                      </div>

                      <div>
                        <span class="dictionary-key">29</span>
                        <span class="dictionary-value">Anti-Jehovah's Witness</span>
                      </div>

                      <div>
                        <span class="dictionary-key">31</span>
                        <span class="dictionary-value">Anti-Arab</span>
                      </div>

                      <div>
                        <span class="dictionary-key">32</span>
                        <span class="dictionary-value">Anti-Hispanic or Latino</span>
                      </div>

                      <div>
                        <span class="dictionary-key">33</span>
                        <span class="dictionary-value">Anti-Other Race/Ethnicity/Ancestry</span>
                      </div>

                      <div>
                        <span class="dictionary-key">41</span>
                        <span class="dictionary-value">Anti-Gay (Male)</span>
                      </div>

                      <div>
                        <span class="dictionary-key">42</span>
                        <span class="dictionary-value">Anti-Lesbian</span>
                      </div>

                      <div>
                        <span class="dictionary-key">43</span>
                        <span class="dictionary-value">Anti-Lesbian, Gay, Bisexual, or Transgender (Mixed Group)</span>
                      </div>

                      <div>
                        <span class="dictionary-key">44</span>
                        <span class="dictionary-value">Anti-Heterosexual</span>
                      </div>

                      <div>
                        <span class="dictionary-key">45</span>
                        <span class="dictionary-value">Anti-Bisexual</span>
                      </div>

                      <div>
                        <span class="dictionary-key">51</span>
                        <span class="dictionary-value">Anti-Physical Disability</span>
                      </div>

                      <div>
                        <span class="dictionary-key">52</span>
                        <span class="dictionary-value">Anti-Mental Disability</span>
                      </div>

                      <div>
                        <span class="dictionary-key">61</span>
                        <span class="dictionary-value">Anti-Male</span>
                      </div>

                      <div>
                        <span class="dictionary-key">62</span>
                        <span class="dictionary-value">Anti-Female</span>
                      </div>

                      <div>
                        <span class="dictionary-key">71</span>
                        <span class="dictionary-value">Anti-Transgender</span>
                      </div>

                      <div>
                        <span class="dictionary-key">72</span>
                        <span class="dictionary-value">Anti-Gender Nonconforming</span>
                      </div>

                      <div>
                        <span class="dictionary-key">81</span>
                        <span class="dictionary-value">Anti-Eastern Orthodox (Russian, Greek, Other)</span>
                      </div>

                      <div>
                        <span class="dictionary-key">82</span>
                        <span class="dictionary-value">Anti-Other Christian</span>
                      </div>

                      <div>
                        <span class="dictionary-key">83</span>
                        <span class="dictionary-value">Anti-Buddhist</span>
                      </div>

                      <div>
                        <span class="dictionary-key">84</span>
                        <span class="dictionary-value">Anti-Hindu</span>
                      </div>

                      <div>
                        <span class="dictionary-key">85</span>
                        <span class="dictionary-value">Anti-Sikh</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Ethnicity</span>

                      <div>
                        <span class="dictionary-key">b</span>
                        <span class="dictionary-value">Blank</span>
                      </div>

                      <div>
                        <span class="dictionary-key">H</span>
                        <span class="dictionary-value">Hispanic or Latino</span>
                      </div>

                      <div>
                        <span class="dictionary-key">M</span>
                        <span class="dictionary-value">Group of Multiple Ethnicities</span>
                      </div>

                      <div>
                        <span class="dictionary-key">N</span>
                        <span class="dictionary-value">Not Hispanic or Latino</span>
                      </div>

                      <div>
                        <span class="dictionary-key">U</span>
                        <span class="dictionary-value">Unknown</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Location</span>

                      <div>
                        <span class="dictionary-key">01</span>
                        <span class="dictionary-value">Air/Bus/Train Terminal</span>
                      </div>

                      <div>
                        <span class="dictionary-key">02</span>
                        <span class="dictionary-value">Bank/Savings and Loan</span>
                      </div>

                      <div>
                        <span class="dictionary-key">03</span>
                        <span class="dictionary-value">Bar/Night Club</span>
                      </div>

                      <div>
                        <span class="dictionary-key">04</span>
                        <span class="dictionary-value">Church/Synagogue/Temple</span>
                      </div>

                      <div>
                        <span class="dictionary-key">05</span>
                        <span class="dictionary-value">Commercial/Office Building</span>
                      </div>

                      <div>
                        <span class="dictionary-key">06</span>
                        <span class="dictionary-value">Construction Site</span>
                      </div>

                      <div>
                        <span class="dictionary-key">07</span>
                        <span class="dictionary-value">Convenience Store</span>
                      </div>

                      <div>
                        <span class="dictionary-key">08</span>
                        <span class="dictionary-value">Department/Discount Store</span>
                      </div>

                      <div>
                        <span class="dictionary-key">09</span>
                        <span class="dictionary-value">Drug Store/Doctor Office/Hospital</span>
                      </div>

                      <div>
                        <span class="dictionary-key">10</span>
                        <span class="dictionary-value">Field/Woods</span>
                      </div>

                      <div>
                        <span class="dictionary-key">11</span>
                        <span class="dictionary-value">Government/Public Building</span>
                      </div>

                      <div>
                        <span class="dictionary-key">12</span>
                        <span class="dictionary-value">Grocery/Supermarket</span>
                      </div>

                      <div>
                        <span class="dictionary-key">13</span>
                        <span class="dictionary-value">Highway/Road/Alley/Street</span>
                      </div>

                      <div>
                        <span class="dictionary-key">14</span>
                        <span class="dictionary-value">Hotel/Motel/etc.</span>
                      </div>

                      <div>
                        <span class="dictionary-key">15</span>
                        <span class="dictionary-value">Jail/Prison</span>
                      </div>

                      <div>
                        <span class="dictionary-key">16</span>
                        <span class="dictionary-value">Lake/Waterway</span>
                      </div>

                      <div>
                        <span class="dictionary-key">17</span>
                        <span class="dictionary-value">Liquor Store</span>
                      </div>

                      <div>
                        <span class="dictionary-key">18</span>
                        <span class="dictionary-value">Parking Lot/Garage</span>
                      </div>

                      <div>
                        <span class="dictionary-key">19</span>
                        <span class="dictionary-value">Rental Storage Facility</span>
                      </div>

                      <div>
                        <span class="dictionary-key">20</span>
                        <span class="dictionary-value">Residence/Home</span>
                      </div>

                      <div>
                        <span class="dictionary-key">21</span>
                        <span class="dictionary-value">Restaurant</span>
                      </div>

                      <div>
                        <span class="dictionary-key">23</span>
                        <span class="dictionary-value">Service/Gas Station</span>
                      </div>

                      <div>
                        <span class="dictionary-key">24</span>
                        <span class="dictionary-value">Specialty Store (TV, Fur, etc.)</span>
                      </div>

                      <div>
                        <span class="dictionary-key">25</span>
                        <span class="dictionary-value">Other/Unknown</span>
                      </div>

                      <div>
                        <span class="dictionary-key">37</span>
                        <span class="dictionary-value">Abandoned/Condemned Structure</span>
                      </div>

                      <div>
                        <span class="dictionary-key">38</span>
                        <span class="dictionary-value">Amusement Park</span>
                      </div>

                      <div>
                        <span class="dictionary-key">39</span>
                        <span class="dictionary-value">Arena/Stadium/Fairgrounds/Coliseum</span>
                      </div>

                      <div>
                        <span class="dictionary-key">40</span>
                        <span class="dictionary-value">ATM Separate from Bank</span>
                      </div>

                      <div>
                        <span class="dictionary-key">41</span>
                        <span class="dictionary-value">Auto Dealership New/Used</span>
                      </div>

                      <div>
                        <span class="dictionary-key">42</span>
                        <span class="dictionary-value">Camp/Campground</span>
                      </div>

                      <div>
                        <span class="dictionary-key">44</span>
                        <span class="dictionary-value">Daycare Facility</span>
                      </div>

                      <div>
                        <span class="dictionary-key">45</span>
                        <span class="dictionary-value">Dock/Wharf/Freight/Modal Terminal</span>
                      </div>

                      <div>
                        <span class="dictionary-key">46</span>
                        <span class="dictionary-value">Farm Facility</span>
                      </div>

                      <div>
                        <span class="dictionary-key">47</span>
                        <span class="dictionary-value">Gambling Facility/Casino</span>
                      </div>

                      <div>
                        <span class="dictionary-key">48</span>
                        <span class="dictionary-value">Industrial Site</span>
                      </div>

                      <div>
                        <span class="dictionary-key">49</span>
                        <span class="dictionary-value">Military Installation</span>
                      </div>

                      <div>
                        <span class="dictionary-key">50</span>
                        <span class="dictionary-value">Park/Playground</span>
                      </div>

                      <div>
                        <span class="dictionary-key">51</span>
                        <span class="dictionary-value">Rest Area</span>
                      </div>

                      <div>
                        <span class="dictionary-key">52</span>
                        <span class="dictionary-value">School-College/University</span>
                      </div>

                      <div>
                        <span class="dictionary-key">53</span>
                        <span class="dictionary-value">School-Elementary/Secondary</span>
                      </div>

                      <div>
                        <span class="dictionary-key">54</span>
                        <span class="dictionary-value">Shelter-Mission/Homeless</span>
                      </div>

                      <div>
                        <span class="dictionary-key">55</span>
                        <span class="dictionary-value">Shopping Mall</span>
                      </div>

                      <div>
                        <span class="dictionary-key">56</span>
                        <span class="dictionary-value">Tribal Lands</span>
                      </div>

                      <div>
                        <span class="dictionary-key">57</span>
                        <span class="dictionary-value">Community Center</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Offense Code</span>

                      <div>
                        <span class="dictionary-key">01</span>
                        <span class="dictionary-value">Murder</span>
                      </div>

                      <div>
                        <span class="dictionary-key">02</span>
                        <span class="dictionary-value">Rape</span>
                      </div>

                      <div>
                        <span class="dictionary-key">03</span>
                        <span class="dictionary-value">Robbery</span>
                      </div>

                      <div>
                        <span class="dictionary-key">04</span>
                        <span class="dictionary-value">Aggravated Assault</span>
                      </div>

                      <div>
                        <span class="dictionary-key">05</span>
                        <span class="dictionary-value">Burglary</span>
                      </div>

                      <div>
                        <span class="dictionary-key">06</span>
                        <span class="dictionary-value">Larceny-Theft</span>
                      </div>

                      <div>
                        <span class="dictionary-key">07</span>
                        <span class="dictionary-value">Motor Vehicle Theft</span>
                      </div>

                      <div>
                        <span class="dictionary-key">08</span>
                        <span class="dictionary-value">Arson</span>
                      </div>

                      <div>
                        <span class="dictionary-key">09</span>
                        <span class="dictionary-value">Simple Assault</span>
                      </div>

                      <div>
                        <span class="dictionary-key">10</span>
                        <span class="dictionary-value">Intimidation</span>
                      </div>

                      <div>
                        <span class="dictionary-key">11</span>
                        <span class="dictionary-value">Destruction/Damage/Vandalism</span>
                      </div>

                      <div>
                        <span class="dictionary-key">12</span>
                        <span class="dictionary-value">Human Trafficking, Commercial Sex Acts</span>
                      </div>

                      <div>
                        <span class="dictionary-key">13</span>
                        <span class="dictionary-value">Human Trafficking, Involuntary Servitude</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Race</span>

                      <div>
                        <span class="dictionary-key">A</span>
                        <span class="dictionary-value">Asian or Pacific Islander</span>
                      </div>

                      <div>
                        <span class="dictionary-key">B</span>
                        <span class="dictionary-value">Black</span>
                      </div>

                      <div>
                        <span class="dictionary-key">I</span>
                        <span class="dictionary-value">American Indian or Alaskan Native</span>
                      </div>

                      <div>
                        <span class="dictionary-key">U</span>
                        <span class="dictionary-value">Unknown</span>
                      </div>

                      <div>
                        <span class="dictionary-key">W</span>
                        <span class="dictionary-value">White</span>
                      </div>
                    </div>

                    <div class="dictionary">
                      <span class="dictionary-header">Victim Type</span>

                      <div>
                        <span class="dictionary-key">B</span>
                        <span class="dictionary-value">Business</span>
                      </div>

                      <div>
                        <span class="dictionary-key">F</span>
                        <span class="dictionary-value">Financial Institution</span>
                      </div>

                      <div>
                        <span class="dictionary-key">G</span>
                        <span class="dictionary-value">Government</span>
                      </div>

                      <div>
                        <span class="dictionary-key">I</span>
                        <span class="dictionary-value">Individual</span>
                      </div>

                      <div>
                        <span class="dictionary-key">O</span>
                        <span class="dictionary-value">Other</span>
                      </div>

                      <div>
                        <span class="dictionary-key">R</span>
                        <span class="dictionary-value">Religious Organization</span>
                      </div>

                      <div>
                        <span class="dictionary-key">U</span>
                        <span class="dictionary-value">Unknown</span>
                      </div>
                    </div>
                  </div>
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