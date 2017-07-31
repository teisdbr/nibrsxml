<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <head>
        <style>
          .no-border {
          border: none;
          }
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
        <table class="table-pad-bot">
          <col />
          <col />
          <colgroup span="23"></colgroup>
          <colgroup span="10"></colgroup>
          <colgroup span="4"></colgroup>
          <thead>
            <tr>
              <th colspan="39" class="title">Age, Sex, Race, and Ethnicity of Persons Arrested</th>
            </tr>
            <tr>
              <th colspan="20" style="text-align:left;border:0px;">
                <xsl:value-of select="concat(UcrReports/@agency, ' ', UcrReports/@ori)" />
              </th>
              <th colspan="19" style="text-align:right;border:0px;">
                <xsl:value-of select="concat(/UcrReports/@year,' ',/UcrReports/@month)" />
              </th>
            </tr>
            <tr>
              <th rowspan="3" scope="col">Classification of Offenses</th>
              <th rowspan="3" scope="col">Sex</th>
              <th colspan="23" scope="colgroup">Ages</th>
              <th colspan="10" scope="colgroup">Race</th>
              <th colspan="4" scope="colgroup">Ethnicity</th>
            </tr>
            <tr>
              <th rowspan="2" scope="col">Under<br />10</th>
              <th rowspan="2" scope="col">10-12</th>
              <th rowspan="2" scope="col">13-14</th>
              <th rowspan="2" scope="col">15</th>
              <th rowspan="2" scope="col">16</th>
              <th rowspan="2" scope="col">17</th>
              <th rowspan="2" scope="col">18</th>
              <th rowspan="2" scope="col">19</th>
              <th rowspan="2" scope="col">20</th>
              <th rowspan="2" scope="col">21</th>
              <th rowspan="2" scope="col">22</th>
              <th rowspan="2" scope="col">23</th>
              <th rowspan="2" scope="col">24</th>
              <th rowspan="2" scope="col">25-29</th>
              <th rowspan="2" scope="col">30-34</th>
              <th rowspan="2" scope="col">35-39</th>
              <th rowspan="2" scope="col">40-44</th>
              <th rowspan="2" scope="col">45-49</th>
              <th rowspan="2" scope="col">50-54</th>
              <th rowspan="2" scope="col">55-59</th>
              <th rowspan="2" scope="col">60-64</th>
              <th rowspan="2" scope="col">65+</th>
              <th rowspan="2" scope="col">Total</th>
              <th colspan="2" scope="col">White</th>
              <th colspan="2" scope="col">Black</th>
              <th colspan="2" scope="col">American<br />Indian</th>
              <th colspan="2" scope="col">Asian</th>
              <th colspan="2" scope="col">Native Hawaiian<br />Or<br />Pacific Islander</th>
              <th colspan="2" scope="col">Hispanic Or<br />Latino</th>
              <th colspan="2" scope="col">Not Hispanic<br />or<br />Latino</th>
            </tr>
            <tr>
              <th>A</th>
              <th>J</th>
              <th>A</th>
              <th>J</th>
              <th>A</th>
              <th>J</th>
              <th>A</th>
              <th>J</th>
              <th>A</th>
              <th>J</th>
              <th>A</th>
              <th>J</th>
              <th>A</th>
              <th>J</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="UcrReports/ASRSummary/UCR">
              <tr>
                <th class="rowheader" rowspan="2" scope="rowgroup">
                  <xsl:variable name="ucrCode" select="./@value" />
                  <xsl:choose>
                    <xsl:when test="@value='01A'">
                      <xsl:value-of select="'Murder and Nonnegligent Manslaughter'" />
                    </xsl:when>

                    <xsl:when test="@value='01B'">
                      <xsl:value-of select="'Manslaughter by Negligence'" />
                    </xsl:when>

                    <xsl:when test="@value='02b'">
                      <xsl:value-of select="'Rape'" />
                    </xsl:when>

                    <xsl:when test="@value='03b'">
                      <xsl:value-of select="'Robbery'" />
                    </xsl:when>

                    <xsl:when test="@value='04b'">
                      <xsl:value-of select="'Aggravated Assault'" />
                    </xsl:when>

                    <xsl:when test="@value='05b'">
                      <xsl:value-of select="'Burglary'" />
                    </xsl:when>

                    <xsl:when test="@value='06b'">
                      <xsl:value-of select="'Larceny-theft (Except Motor Vehicle Theft)'" />
                    </xsl:when>

                    <xsl:when test="@value='07b'">
                      <xsl:value-of select="'Motor Vehicle Theft'" />
                    </xsl:when>

                    <xsl:when test="@value='08b'">
                      <xsl:value-of select="'Other Assaults'" />
                    </xsl:when>

                    <xsl:when test="@value='09b'">
                      <xsl:value-of select="'Arson'" />
                    </xsl:when>

                    <xsl:when test="@value='10b'">
                      <xsl:value-of select="'Forgery and Counterfeiting'" />
                    </xsl:when>

                    <xsl:when test="@value='11b'">
                      <xsl:value-of select="'Fraud'" />
                    </xsl:when>

                    <xsl:when test="@value='12b'">
                      <xsl:value-of select="'Embezzlement'" />
                    </xsl:when>

                    <xsl:when test="@value='13b'">
                      <xsl:value-of select="'Stolen Property: Buying, Receiving, Possessing'" />
                    </xsl:when>

                    <xsl:when test="@value='14b'">
                      <xsl:value-of select="'Vandalism'" />
                    </xsl:when>

                    <xsl:when test="@value='15b'">
                      <xsl:value-of select="'Weapons: Carrying, Possessing, etc.'" />
                    </xsl:when>

                    <xsl:when test="@value='16b'">
                      <xsl:value-of select="'Prostitution and Commercialized Vice'" />
                    </xsl:when>

                    <xsl:when test="@value='16A'">
                      <xsl:value-of select="'Prostitution'" />
                    </xsl:when>

                    <xsl:when test="@value='16B'">
                      <xsl:value-of select="'Assisting or Promoting Prostitution'" />
                    </xsl:when>

                    <xsl:when test="@value='16C'">
                      <xsl:value-of select="'Purchasing Prostitution'" />
                    </xsl:when>

                    <xsl:when test="@value='17b'">
                      <xsl:value-of select="'Sex Offenses (Except Rape and Prostitution)'" />
                    </xsl:when>

                    <xsl:when test="@value='18b'">
                      <xsl:value-of select="'Drug Abuse Violations (Grand Total)'" />
                    </xsl:when>

                    <xsl:when test="@value='180'">
                      <xsl:value-of select="'Sale/Manufacturing (Subtotal)'" />
                    </xsl:when>

                    <xsl:when test="@value='18A'">
                      <xsl:value-of select="'Opium or Cocaine and their Derivatives (Morphine and Heroin)'" />
                    </xsl:when>

                    <xsl:when test="@value='18B'">
                      <xsl:value-of select="'Marijuana'" />
                    </xsl:when>

                    <xsl:when test="@value='18C'">
                      <xsl:value-of
                        select="'Synthetic Narcotics-Manufactured Narcotics which can cause true drug addiction (Demerol, Methadones)'" />
                    </xsl:when>

                    <xsl:when test="@value='18D'">
                      <xsl:value-of select="'Other-Dangerous Nonnarcotic Drugs (Barbiturates, Benzedrine)'" />
                    </xsl:when>

                    <xsl:when test="@value='185'">
                      <xsl:value-of select="'Possession (Subtotal)'" />
                    </xsl:when>

                    <xsl:when test="@value='18E'">
                      <xsl:value-of select="'Opium or Cocaine and their Derivatives (Morphine and Codeine)'" />
                    </xsl:when>

                    <xsl:when test="@value='18F'">
                      <xsl:value-of select="'Marijuana'" />
                    </xsl:when>

                    <xsl:when test="@value='18G'">
                      <xsl:value-of
                        select="'Synthetic Narcotics-Manufactured Narcotics which can cause true drug addiction (Demerol, Methadones)'" />
                    </xsl:when>

                    <xsl:when test="@value='18H'">
                      <xsl:value-of select="'Other-Dangerous Nonnarcotic Drugs (Barbiturates, Benzedrine)'" />
                    </xsl:when>

                    <xsl:when test="@value='19b'">
                      <xsl:value-of select="'Gambling (Total)'" />
                    </xsl:when>

                    <xsl:when test="@value='19A'">
                      <xsl:value-of select="'Bookmaking (Horse and Sport Book)'" />
                    </xsl:when>

                    <xsl:when test="@value='19B'">
                      <xsl:value-of select="'Numbers and Lottery'" />
                    </xsl:when>

                    <xsl:when test="@value='19C'">
                      <xsl:value-of select="'All Other Gambling'" />
                    </xsl:when>

                    <xsl:when test="@value='20b'">
                      <xsl:value-of select="'Offenses Against the Family and Children'" />
                    </xsl:when>

                    <xsl:when test="@value='21b'">
                      <xsl:value-of select="'Driving Under the Influence'" />
                    </xsl:when>

                    <xsl:when test="@value='22b'">
                      <xsl:value-of select="'Liquor Laws'" />
                    </xsl:when>

                    <xsl:when test="@value='23b'">
                      <xsl:value-of select="'Drunkenness'" />
                    </xsl:when>

                    <xsl:when test="@value='24b'">
                      <xsl:value-of select="'Disorderly Conduct'" />
                    </xsl:when>

                    <xsl:when test="@value='25b'">
                      <xsl:value-of select="'Vagrancy'" />
                    </xsl:when>

                    <xsl:when test="@value='26b'">
                      <xsl:value-of select="'All Other Offenses (Except Traffic)'" />
                    </xsl:when>

                    <xsl:when test="@value='27b'">
                      <xsl:value-of select="'Suspicion'" />
                    </xsl:when>

                    <xsl:when test="@value='28b'">
                      <xsl:value-of select="'Curfew and Loitering Law Violations'" />
                    </xsl:when>

                    <xsl:when test="@value='29b'">
                      <xsl:value-of select="'Runaways'" />
                    </xsl:when>

                    <xsl:when test="@value='30b'">
                      <xsl:value-of select="'Human Trafficking/Commercial Sex Acts'" />
                    </xsl:when>

                    <xsl:when test="@value='31b'">
                      <xsl:value-of select="'Human Trafficking/Involuntary Servitude'" />
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="@value" />
                    </xsl:otherwise>
                  </xsl:choose>
                </th>
                <td class="centered">M</td>
                <td>
                  <xsl:if test="not(Age[@value='Under 10']/M)">0</xsl:if>
                  <xsl:value-of select="Age[@value='Under 10']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='10-12']/M)">0</xsl:if><xsl:value-of select="Age[@value='10-12']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='13-14']/M)">0</xsl:if><xsl:value-of select="Age[@value='13-14']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='15']/M)">0</xsl:if><xsl:value-of select="Age[@value='15']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='16']/M)">0</xsl:if><xsl:value-of select="Age[@value='16']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='17']/M)">0</xsl:if><xsl:value-of select="Age[@value='17']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='18']/M)">0</xsl:if><xsl:value-of select="Age[@value='18']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='19']/M)">0</xsl:if><xsl:value-of select="Age[@value='19']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='20']/M)">0</xsl:if><xsl:value-of select="Age[@value='20']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='21']/M)">0</xsl:if><xsl:value-of select="Age[@value='21']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='22']/M)">0</xsl:if><xsl:value-of select="Age[@value='22']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='23']/M)">0</xsl:if><xsl:value-of select="Age[@value='23']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='24']/M)">0</xsl:if><xsl:value-of select="Age[@value='24']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='25-29']/M)">0</xsl:if><xsl:value-of select="Age[@value='25-29']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='30-34']/M)">0</xsl:if><xsl:value-of select="Age[@value='30-34']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='35-39']/M)">0</xsl:if><xsl:value-of select="Age[@value='35-39']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='40-44']/M)">0</xsl:if><xsl:value-of select="Age[@value='40-44']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='45-49']/M)">0</xsl:if><xsl:value-of select="Age[@value='45-49']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='50-54']/M)">0</xsl:if><xsl:value-of select="Age[@value='50-54']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='55-59']/M)">0</xsl:if><xsl:value-of select="Age[@value='55-59']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='60-64']/M)">0</xsl:if><xsl:value-of select="Age[@value='60-64']/M" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='65+']/M)">0</xsl:if><xsl:value-of select="Age[@value='65+']/M" />
                </td>
                <td>
                  <xsl:value-of select="sum(Age/M)" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Adult/Races/White)">0</xsl:if><xsl:value-of select="Adult/Races/White" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Juvenile/Races/White)">0</xsl:if>
                  <xsl:value-of select="Juvenile/Races/White" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Adult/Races/Black)">0</xsl:if><xsl:value-of select="Adult/Races/Black" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Juvenile/Races/Black)">0</xsl:if>
                  <xsl:value-of select="Juvenile/Races/Black" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Adult/Races/AmericanIndian)">0</xsl:if>
                  <xsl:value-of select="Adult/Races/AmericanIndian" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Juvenile/Races/AmericanIndian)">0</xsl:if>
                  <xsl:value-of select="Juvenile/Races/AmericanIndian" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Adult/Races/Asian)">0</xsl:if><xsl:value-of select="Adult/Races/Asian" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Juvenile/Races/Asian)">0</xsl:if>
                  <xsl:value-of select="Juvenile/Races/Asian" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Adult/Races/NativeHawaiianOrOther)">0</xsl:if>
                  <xsl:value-of select="Adult/Races/NativeHawaiianOrOther" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Juvenile/Races/NativeHawaiianOrOther)">0</xsl:if>
                  <xsl:value-of select="Juvenile/Races/NativeHawaiianOrOther" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Adult/Ethnicities/Hispanic)">0</xsl:if>
                  <xsl:value-of select="Adult/Ethnicities/Hispanic" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Juvenile/Ethnicities/Hispanic)">0</xsl:if>
                  <xsl:value-of select="Juvenile/Ethnicities/Hispanic" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Adult/Ethnicities/Non-Hispanic)">0</xsl:if>
                  <xsl:value-of select="Adult/Ethnicities/Non-Hispanic" />
                </td>
                <td rowspan="2">
                  <xsl:if test="not(Juvenile/Ethnicities/Non-Hispanic)">0</xsl:if>
                  <xsl:value-of select="Juvenile/Ethnicities/Non-Hispanic" />
                </td>
              </tr>
              <tr>
                <td class="centered">F</td>
                <td>
                  <xsl:if test="not(Age[@value='Under 10']/F)">0</xsl:if>
                  <xsl:value-of select="Age[@value='Under 10']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='10-12']/F)">0</xsl:if><xsl:value-of select="Age[@value='10-12']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='13-14']/F)">0</xsl:if><xsl:value-of select="Age[@value='13-14']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='15']/F)">0</xsl:if><xsl:value-of select="Age[@value='15']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='16']/F)">0</xsl:if><xsl:value-of select="Age[@value='16']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='17']/F)">0</xsl:if><xsl:value-of select="Age[@value='17']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='18']/F)">0</xsl:if><xsl:value-of select="Age[@value='18']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='19']/F)">0</xsl:if><xsl:value-of select="Age[@value='19']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='20']/F)">0</xsl:if><xsl:value-of select="Age[@value='20']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='21']/F)">0</xsl:if><xsl:value-of select="Age[@value='21']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='22']/F)">0</xsl:if><xsl:value-of select="Age[@value='22']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='23']/F)">0</xsl:if><xsl:value-of select="Age[@value='23']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='24']/F)">0</xsl:if><xsl:value-of select="Age[@value='24']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='25-29']/F)">0</xsl:if><xsl:value-of select="Age[@value='25-29']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='30-34']/F)">0</xsl:if><xsl:value-of select="Age[@value='30-34']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='35-39']/F)">0</xsl:if><xsl:value-of select="Age[@value='35-39']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='40-44']/F)">0</xsl:if><xsl:value-of select="Age[@value='40-44']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='45-49']/F)">0</xsl:if><xsl:value-of select="Age[@value='45-49']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='50-54']/F)">0</xsl:if><xsl:value-of select="Age[@value='50-54']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='55-59']/F)">0</xsl:if><xsl:value-of select="Age[@value='55-59']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='60-64']/F)">0</xsl:if><xsl:value-of select="Age[@value='60-64']/F" />
                </td>
                <td>
                  <xsl:if test="not(Age[@value='65+']/F)">0</xsl:if><xsl:value-of select="Age[@value='65+']/F" />
                </td>
                <td>
                  <xsl:value-of select="sum(Age/F)" />
                </td>
              </tr>
            </xsl:for-each>
            <tr>
              <th class="rowheader" scope="row" colspan="2">Totals</th>
              <td>
                <xsl:value-of select="sum(//Age[@value='Under 10']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='10-12']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='13-14']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='15']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='16']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='17']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='18']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='19']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='20']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='21']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='22']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='23']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='24']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='25-29']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='30-34']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='35-39']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='40-44']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='45-49']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='50-54']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='55-59']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='60-64']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age[@value='65+']/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Age/*)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Adult/Races/White)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Juvenile/Races/White)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Adult/Races/Black)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Juvenile/Races/Black)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Adult/Races/AmericanIndian)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Juvenile/Races/AmericanIndian)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Adult/Races/Asian)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Juvenile/Races/Asian)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Adult/Races/NativeHawaiianOrOther)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Juvenile/Races/NativeHawaiianOrOther)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Adult/Ethnicities/Hispanic)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Juvenile/Ethnicities/Hispanic)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Adult/Ethnicities/Non-Hispanic)" />
              </td>
              <td>
                <xsl:value-of select="sum(//Juvenile/Ethnicities/Non-Hispanic)" />
              </td>
            </tr>
          </tbody>
        </table>
        <table>
          <thead>
            <tr>
              <th colspan="2">Police Disposition of Juveniles</th>
            </tr>
            <tr>
              <th>Disposition Category</th>
              <th>Number Handled/Referred</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <th class="rowheader">Handled Within Department</th>
              <td>
                <xsl:if test="not(//JuvenileDispositions/Department[1])">0</xsl:if>
                <xsl:value-of select="//JuvenileDispositions/Department[1]" />
              </td>
            </tr>
            <tr>
              <th class="rowheader">Referred to Juvenile Court</th>
              <td>
                <xsl:if test="not(//JuvenileDispositions/Court[1])">0</xsl:if>
                <xsl:value-of select="//JuvenileDispositions/Court[1]" />
              </td>
            </tr>
            <tr>
              <th class="rowheader">Referred to Welfare Agency</th>
              <td>
                <xsl:if test="not(//JuvenileDispositions/WelfareAgency[1])">0</xsl:if>
                <xsl:value-of select="//JuvenileDispositions/WelfareAgency[1]" />
              </td>
            </tr>
            <tr>
              <th class="rowheader">Referred to Other Police Agency</th>
              <td>
                <xsl:if test="not(//JuvenileDispositions/OtherPoliceAgency[1])">0</xsl:if>
                <xsl:value-of select="//JuvenileDispositions/OtherPoliceAgency[1]" />
              </td>
            </tr>
            <tr>
              <th class="rowheader">Referred to Criminal (Adult) Court</th>
              <td>
                <xsl:if test="not(//JuvenileDispositions/CriminalOrAdultCourt[1])">0</xsl:if>
                <xsl:value-of select="//JuvenileDispositions/CriminalOrAdultCourt[1]" />
              </td>
            </tr>
            <tr>
              <th class="rowheader">Total</th>
              <td>
                <xsl:if test="not(//JuvenileDispositions/Total[1])">0</xsl:if>
                <xsl:value-of select="//JuvenileDispositions/Total[1]" />
              </td>
            </tr>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="wrapper">
    <xsl:apply-templates select="document(./@Source)" />
  </xsl:template>
</xsl:stylesheet>