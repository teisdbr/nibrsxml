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
    </style>
  </head>
  <body>
    <table>
      <col/>
      <col/>
      <colgroup span="23"></colgroup>
      <colgroup span="5"></colgroup>
      <colgroup span="2"></colgroup>
      <thead>
        <tr>
          <td class="no-border"></td>
          <td class="no-border"></td>
          <th colspan="23" scope="colgroup">Ages</th>
          <th colspan="5" scope="colgroup">Race</th>
          <th colspan="2" scope="colgroup">Ethnicity</th>
        </tr>
        <tr>
          <th scope="col">Classification of Offenses</th>
          <th scope="col">Sex</th>
          <th scope="col">Under<br/>10</th>
          <th scope="col">10-12</th>
          <th scope="col">13-14</th>
          <th scope="col">15</th>
          <th scope="col">16</th>
          <th scope="col">17</th>
          <th scope="col">18</th>
          <th scope="col">19</th>
          <th scope="col">20</th>
          <th scope="col">21</th>
          <th scope="col">22</th>
          <th scope="col">23</th>
          <th scope="col">24</th>
          <th scope="col">25-29</th>
          <th scope="col">30-34</th>
          <th scope="col">35-39</th>
          <th scope="col">40-44</th>
          <th scope="col">45-49</th>
          <th scope="col">50-54</th>
          <th scope="col">55-59</th>
          <th scope="col">60-64</th>
          <th scope="col">65+</th>
          <th scope="col">Total</th>
          <th scope="col">White</th>
          <th scope="col">Black</th>
          <th scope="col">American<br/>Indian</th>
          <th scope="col">Asian</th>
          <th scope="col">Native Hawaiian<br/>Or<br/>Pacific Islander</th>
          <th scope="col">Hispanic Or<br/>Latino</th>
          <th scope="col">Not Hispanic<br/>or<br/>Latino</th>
        </tr>
      </thead>
      <tbody>
        <xsl:for-each select="ASRSummary/UCR">
        <tr>
          <th rowspan="2" scope="rowgroup">
            <xsl:variable name="ucrCode" select="./@value"/>
            <xsl:choose>
              <xsl:when test="//UCRDescription[@value=$ucrCode]">
                <xsl:value-of select="//UCRDescription[@value=$ucrCode]"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="@value"/>
              </xsl:otherwise>
            </xsl:choose>
          </th>
          <td class="centered">M</td>
          <td><xsl:if test="not(Age[@value='Under 10']/M)">0</xsl:if><xsl:value-of select="Age[@value='Under 10']/M"/></td>
          <td><xsl:if test="not(Age[@value='10-12']/M)">0</xsl:if><xsl:value-of select="Age[@value='10-12']/M"/></td>
          <td><xsl:if test="not(Age[@value='13-14']/M)">0</xsl:if><xsl:value-of select="Age[@value='13-14']/M"/></td>
          <td><xsl:if test="not(Age[@value='15']/M)">0</xsl:if><xsl:value-of select="Age[@value='15']/M"/></td>
          <td><xsl:if test="not(Age[@value='16']/M)">0</xsl:if><xsl:value-of select="Age[@value='16']/M"/></td>
          <td><xsl:if test="not(Age[@value='17']/M)">0</xsl:if><xsl:value-of select="Age[@value='17']/M"/></td>
          <td><xsl:if test="not(Age[@value='18']/M)">0</xsl:if><xsl:value-of select="Age[@value='18']/M"/></td>
          <td><xsl:if test="not(Age[@value='19']/M)">0</xsl:if><xsl:value-of select="Age[@value='19']/M"/></td>
          <td><xsl:if test="not(Age[@value='20']/M)">0</xsl:if><xsl:value-of select="Age[@value='20']/M"/></td>
          <td><xsl:if test="not(Age[@value='21']/M)">0</xsl:if><xsl:value-of select="Age[@value='21']/M"/></td>
          <td><xsl:if test="not(Age[@value='22']/M)">0</xsl:if><xsl:value-of select="Age[@value='22']/M"/></td>
          <td><xsl:if test="not(Age[@value='23']/M)">0</xsl:if><xsl:value-of select="Age[@value='23']/M"/></td>
          <td><xsl:if test="not(Age[@value='24']/M)">0</xsl:if><xsl:value-of select="Age[@value='24']/M"/></td>
          <td><xsl:if test="not(Age[@value='25-29']/M)">0</xsl:if><xsl:value-of select="Age[@value='25-29']/M"/></td>
          <td><xsl:if test="not(Age[@value='30-34']/M)">0</xsl:if><xsl:value-of select="Age[@value='30-34']/M"/></td>
          <td><xsl:if test="not(Age[@value='35-39']/M)">0</xsl:if><xsl:value-of select="Age[@value='35-39']/M"/></td>
          <td><xsl:if test="not(Age[@value='40-44']/M)">0</xsl:if><xsl:value-of select="Age[@value='40-44']/M"/></td>
          <td><xsl:if test="not(Age[@value='45-49']/M)">0</xsl:if><xsl:value-of select="Age[@value='45-49']/M"/></td>
          <td><xsl:if test="not(Age[@value='50-54']/M)">0</xsl:if><xsl:value-of select="Age[@value='50-54']/M"/></td>
          <td><xsl:if test="not(Age[@value='55-59']/M)">0</xsl:if><xsl:value-of select="Age[@value='55-59']/M"/></td>
          <td><xsl:if test="not(Age[@value='60-64']/M)">0</xsl:if><xsl:value-of select="Age[@value='60-64']/M"/></td>
          <td><xsl:if test="not(Age[@value='65+']/M)">0</xsl:if><xsl:value-of select="Age[@value='65+']/M"/></td>
          <td><xsl:value-of select="sum(Age/M)"/></td>
          <td rowspan="2"><xsl:if test="not(Races/White)">0</xsl:if><xsl:value-of select="Races/White"/></td>
          <td rowspan="2"><xsl:if test="not(Races/Black)">0</xsl:if><xsl:value-of select="Races/Black"/></td>
          <td rowspan="2"><xsl:if test="not(Races/AmericanIndian)">0</xsl:if><xsl:value-of select="Races/AmericanIndian"/></td>
          <td rowspan="2"><xsl:if test="not(Races/Asian)">0</xsl:if><xsl:value-of select="Races/Asian"/></td>
          <td rowspan="2"><xsl:if test="not(Races/NativeHawaiianOrOther)">0</xsl:if><xsl:value-of select="Races/NativeHawaiianOrOther"/></td>
          <td rowspan="2"><xsl:if test="not(Ethnicities/Hispanic)">0</xsl:if><xsl:value-of select="Ethnicities/Hispanic"/></td>
          <td rowspan="2"><xsl:if test="not(Ethnicities/Non-Hispanic)">0</xsl:if><xsl:value-of select="Ethnicities/Non-Hispanic"/></td>
        </tr>
        <tr>
          <td class="centered">F</td>
          <td><xsl:if test="not(Age[@value='Under 10']/F)">0</xsl:if><xsl:value-of select="Age[@value='Under 10']/F"/></td>
          <td><xsl:if test="not(Age[@value='10-12']/F)">0</xsl:if><xsl:value-of select="Age[@value='10-12']/F"/></td>
          <td><xsl:if test="not(Age[@value='13-14']/F)">0</xsl:if><xsl:value-of select="Age[@value='13-14']/F"/></td>
          <td><xsl:if test="not(Age[@value='15']/F)">0</xsl:if><xsl:value-of select="Age[@value='15']/F"/></td>
          <td><xsl:if test="not(Age[@value='16']/F)">0</xsl:if><xsl:value-of select="Age[@value='16']/F"/></td>
          <td><xsl:if test="not(Age[@value='17']/F)">0</xsl:if><xsl:value-of select="Age[@value='17']/F"/></td>
          <td><xsl:if test="not(Age[@value='18']/F)">0</xsl:if><xsl:value-of select="Age[@value='18']/F"/></td>
          <td><xsl:if test="not(Age[@value='19']/F)">0</xsl:if><xsl:value-of select="Age[@value='19']/F"/></td>
          <td><xsl:if test="not(Age[@value='20']/F)">0</xsl:if><xsl:value-of select="Age[@value='20']/F"/></td>
          <td><xsl:if test="not(Age[@value='21']/F)">0</xsl:if><xsl:value-of select="Age[@value='21']/F"/></td>
          <td><xsl:if test="not(Age[@value='22']/F)">0</xsl:if><xsl:value-of select="Age[@value='22']/F"/></td>
          <td><xsl:if test="not(Age[@value='23']/F)">0</xsl:if><xsl:value-of select="Age[@value='23']/F"/></td>
          <td><xsl:if test="not(Age[@value='24']/F)">0</xsl:if><xsl:value-of select="Age[@value='24']/F"/></td>
          <td><xsl:if test="not(Age[@value='25-29']/F)">0</xsl:if><xsl:value-of select="Age[@value='25-29']/F"/></td>
          <td><xsl:if test="not(Age[@value='30-34']/F)">0</xsl:if><xsl:value-of select="Age[@value='30-34']/F"/></td>
          <td><xsl:if test="not(Age[@value='35-39']/F)">0</xsl:if><xsl:value-of select="Age[@value='35-39']/F"/></td>
          <td><xsl:if test="not(Age[@value='40-44']/F)">0</xsl:if><xsl:value-of select="Age[@value='40-44']/F"/></td>
          <td><xsl:if test="not(Age[@value='45-49']/F)">0</xsl:if><xsl:value-of select="Age[@value='45-49']/F"/></td>
          <td><xsl:if test="not(Age[@value='50-54']/F)">0</xsl:if><xsl:value-of select="Age[@value='50-54']/F"/></td>
          <td><xsl:if test="not(Age[@value='55-59']/F)">0</xsl:if><xsl:value-of select="Age[@value='55-59']/F"/></td>
          <td><xsl:if test="not(Age[@value='60-64']/F)">0</xsl:if><xsl:value-of select="Age[@value='60-64']/F"/></td>
          <td><xsl:if test="not(Age[@value='65+']/F)">0</xsl:if><xsl:value-of select="Age[@value='65+']/F"/></td>
          <td><xsl:value-of select="sum(Age/F)"/></td>
        </tr>
        </xsl:for-each>
        <tr>
          <th scope="row" colspan="2">Totals</th>
          <td><xsl:value-of select="sum(//Age[@value='Under 10']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='10-12']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='13-14']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='15']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='16']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='17']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='18']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='19']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='20']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='21']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='22']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='23']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='24']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='25-29']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='30-34']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='35-39']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='40-44']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='45-49']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='50-54']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='55-59']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='60-64']/*)"/></td>
          <td><xsl:value-of select="sum(//Age[@value='65+']/*)"/></td>
          <td><xsl:value-of select="sum(//Age/*)"/></td>
          <td><xsl:value-of select="sum(//White)"/></td>
          <td><xsl:value-of select="sum(//Black)"/></td>
          <td><xsl:value-of select="sum(//AmericanIndian)"/></td>
          <td><xsl:value-of select="sum(//Asian)"/></td>
          <td><xsl:value-of select="sum(//NativeHawaiianOrOther)"/></td>      
          <td><xsl:value-of select="sum(//Hispanic)"/></td>
          <td><xsl:value-of select="sum(//Non-Hispanic)"/></td> 

       <!--              <td rowspan="2"><xsl:if test="not(Races/White)">0</xsl:if><xsl:value-of select="Races/White"/></td>
          <td rowspan="2"><xsl:if test="not(Races/Black)">0</xsl:if><xsl:value-of select="Races/Black"/></td>
          <td rowspan="2"><xsl:if test="not(Races/AmericanIndian)">0</xsl:if><xsl:value-of select="Races/AmericanIndian"/></td>
          <td rowspan="2"><xsl:if test="not(Races/Asian)">0</xsl:if><xsl:value-of select="Races/Asian"/></td>
          <td rowspan="2"><xsl:if test="not(Races/NativeHawaiianOrOther)">0</xsl:if><xsl:value-of select="Races/NativeHawaiianOrOther"/></td>
          <td rowspan="2"><xsl:if test="not(Ethnicities/Hispanic)">0</xsl:if><xsl:value-of select="Ethnicities/Hispanic"/></td>
          <td rowspan="2"><xsl:if test="not(Ethnicities/Non-Hispanic)">0</xsl:if><xsl:value-of select="Ethnicities/Non-Hispanic"/></td>
        -->   
        </tr>
      </tbody>
    </table>
  </body>
</html>
</xsl:template>
<xsl:template match="wrapper">
  <xsl:apply-templates select="document(./@Source)"/>
</xsl:template>
</xsl:stylesheet>