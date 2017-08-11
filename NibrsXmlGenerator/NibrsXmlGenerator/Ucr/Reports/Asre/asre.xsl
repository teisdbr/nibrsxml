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
                  Age, Sex, Race, and Ethnicity of Persons Arrested, All Age Groups<br/>
                  (includes those released without having been charged formally)
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
                  <div class="data-table-container">
                    <table class="table-pad-bot">
                      <thead>
                        <tr>
                          <th rowspan="3" colspan="2">Classification of Offenses</th>
                          <th rowspan="3">Sex</th>
                          <th colspan="23">Ages</th>
                          <th colspan="10">Race</th>
                          <th colspan="4">Ethnicity</th>
                        </tr>
                        <tr>
                          <th rowspan="2">
                            Under<br />10
                          </th>
                          <th rowspan="2">10-12</th>
                          <th rowspan="2">13-14</th>
                          <th rowspan="2">15</th>
                          <th rowspan="2">16</th>
                          <th rowspan="2">17</th>
                          <th rowspan="2">18</th>
                          <th rowspan="2">19</th>
                          <th rowspan="2">20</th>
                          <th rowspan="2">21</th>
                          <th rowspan="2">22</th>
                          <th rowspan="2">23</th>
                          <th rowspan="2">24</th>
                          <th rowspan="2">25-29</th>
                          <th rowspan="2">30-34</th>
                          <th rowspan="2">35-39</th>
                          <th rowspan="2">40-44</th>
                          <th rowspan="2">45-49</th>
                          <th rowspan="2">50-54</th>
                          <th rowspan="2">55-59</th>
                          <th rowspan="2">60-64</th>
                          <th rowspan="2">65+</th>
                          <th rowspan="2">Total</th>
                          <th colspan="2">White</th>
                          <th colspan="2">Black</th>
                          <th colspan="2">
                            American<br />Indian
                          </th>
                          <th colspan="2">Asian</th>
                          <th colspan="2">
                            Native Hawaiian<br />Or<br />Pacific Islander
                          </th>
                          <th colspan="2">
                            Hispanic Or<br />Latino
                          </th>
                          <th colspan="2">
                            Not Hispanic<br />or<br />Latino
                          </th>
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
                            <xsl:variable name="ucrCode" select="./@value" />
                            <xsl:choose>
                              <xsl:when test="@value='01A'">
                                <th class="rowheader" rowspan="2">
                                  Murder and Nonnegligent Manslaughter
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='01B'">
                                <th class="rowheader" rowspan="2">
                                  Manslaughter by Negligence
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='02b'">
                                <th class="rowheader" rowspan="2">
                                  Rape
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='03b'">
                                <th class="rowheader" rowspan="2">
                                  Robbery
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='04b'">
                                <th class="rowheader" rowspan="2">
                                  Aggravated Assault (Return A: 4a-d)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='05b'">
                                <th class="rowheader" rowspan="2">
                                  Burglary - Breaking or Entering
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='06b'">
                                <th class="rowheader" rowspan="2">
                                  Larceny - Theft (Except Motor Vehicle Theft)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='07b'">
                                <th class="rowheader" rowspan="2">
                                  Motor Vehicle Theft
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='08b'">
                                <th class="rowheader" rowspan="2">
                                  Other Assaults (Return A: 4e)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='09b'">
                                <th class="rowheader" rowspan="2">
                                  Arson
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='10b'">
                                <th class="rowheader" rowspan="2">
                                  Forgery and Counterfeiting
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='11b'">
                                <th class="rowheader" rowspan="2">
                                  Fraud
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='12b'">
                                <th class="rowheader" rowspan="2">
                                  Embezzlement
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='13b'">
                                <th class="rowheader" rowspan="2">
                                  Stolen Property: Buying, Receiving, Possessing
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='14b'">
                                <th class="rowheader" rowspan="2">
                                  Vandalism
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='15b'">
                                <th class="rowheader" rowspan="2">
                                  Weapons: Carrying, Possessing, etc.
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='16b'">
                                <th class="rowheader" rowspan="2">
                                  Prostitution and Commercialized Vice
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='16A'">
                                <th class="rowheader" rowspan="2">
                                  Prostitution
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='16B'">
                                <th class="rowheader" rowspan="2">
                                  Assisting or Promoting Prostitution
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='16C'">
                                <th class="rowheader" rowspan="2">
                                  Purchasing Prostitution
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='17b'">
                                <th class="rowheader" rowspan="2">
                                  Sex Offenses (Except Rape and Prostitution)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18b'">
                                <th class="rowheader" rowspan="2">
                                  Drug Abuse Violations<br/>Grand Total
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='180'">
                                <th class="rowheader" rowspan="2">
                                  (1) Sale/Manufacturing<br/>Subtotal
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18A'">
                                <th class="rowheader" rowspan="2">
                                  Opium or Cocaine and their Derivatives (Morphine, Heroin, Codeine)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18B'">
                                <th class="rowheader" rowspan="2">
                                  Marijuana
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18C'">
                                <th class="rowheader" rowspan="2">
                                  Synthetic Narcotics -<br/>Manufactured Narcotics which can cause true drug addiction (Demerol, Methadones)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18D'">
                                <th class="rowheader" rowspan="2">
                                  Other - Dangerous Non-narcotic Drugs (Barbiturates, Benzedrine)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='185'">
                                <th class="rowheader" rowspan="2">
                                  (2) Possession<br/>Subtotal
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18E'">
                                <th class="rowheader" rowspan="2">
                                  Opium or Cocaine and their Derivatives (Morphine and Codeine)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18F'">
                                <th class="rowheader" rowspan="2">
                                  Marijuana
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18G'">
                                <th class="rowheader" rowspan="2">
                                  Synthetic Narcotics -<br/>Manufactured Narcotics Which Can Cause True Drug Addiction (Demerol, Methadones)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='18H'">
                                <th class="rowheader" rowspan="2">
                                  Other - Dangerous Non-narcotic Drugs (Barbiturates, Benzedrine)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='19b'">
                                <th class="rowheader" rowspan="2">
                                  Gambling<br/>Total
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='19A'">
                                <th class="rowheader" rowspan="2">
                                  Bookmaking (Horse and Sport Book)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='19B'">
                                <th class="rowheader" rowspan="2">
                                  Numbers and Lottery
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='19C'">
                                <th class="rowheader" rowspan="2">
                                  All Other Gambling
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='20b'">
                                <th class="rowheader" rowspan="2">
                                  Offenses Against the Family and Children
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='21b'">
                                <th class="rowheader" rowspan="2">
                                  Driving Under the Influence
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='22b'">
                                <th class="rowheader" rowspan="2">
                                  Liquor Laws
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='23b'">
                                <th class="rowheader" rowspan="2">
                                  Drunkenness
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='24b'">
                                <th class="rowheader" rowspan="2">
                                  Disorderly Conduct
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='25b'">
                                <th class="rowheader" rowspan="2">
                                  Vagrancy
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='26b'">
                                <th class="rowheader" rowspan="2">
                                  All Other Offenses (Except Traffic)
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='27b'">
                                <th class="rowheader" rowspan="2">
                                  Suspicion
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='28b'">
                                <th class="rowheader" rowspan="2">
                                  Curfew and Loitering Law Violations
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='29b'">
                                <th class="rowheader" rowspan="2">
                                  Runaways
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='30b'">
                                <th class="rowheader" rowspan="2">
                                  Human Trafficking/<br/>Commercial Sex Acts
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:when test="@value='31b'">
                                <th class="rowheader" rowspan="2">
                                  Human Trafficking/<br/>Involuntary Servitude
                                </th>
                                <th rowspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:when>

                              <xsl:otherwise>
                                <th class="rowheader" colspan="2">
                                  <xsl:value-of select="@value" />
                                </th>
                              </xsl:otherwise>
                            </xsl:choose>
                            <td class="centered">M</td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='Under 10']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='Under 10']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='10-12']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='10-12']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='13-14']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='13-14']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='15']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='15']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='16']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='16']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='17']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='17']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='18']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='18']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='19']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='19']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='20']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='20']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='21']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='21']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='22']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='22']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='23']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='23']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='24']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='24']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='25-29']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='25-29']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='30-34']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='30-34']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='35-39']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='35-39']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='40-44']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='40-44']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='45-49']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='45-49']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='50-54']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='50-54']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='55-59']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='55-59']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='60-64']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='60-64']/M" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='65+']/M)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='65+']/M" />
                            </td>
                            <td>
                              <xsl:value-of select="sum(Ages/Age/M)" />
                            </td>
                            <td rowspan="2">
                              <xsl:if test="not(Adult/Races/White)">0</xsl:if>
                              <xsl:value-of select="Adult/Races/White" />
                            </td>
                            <td rowspan="2">
                              <xsl:if test="not(Juvenile/Races/White)">0</xsl:if>
                              <xsl:value-of select="Juvenile/Races/White" />
                            </td>
                            <td rowspan="2">
                              <xsl:if test="not(Adult/Races/Black)">0</xsl:if>
                              <xsl:value-of select="Adult/Races/Black" />
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
                              <xsl:if test="not(Adult/Races/Asian)">0</xsl:if>
                              <xsl:value-of select="Adult/Races/Asian" />
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
                              <xsl:if test="not(Ages/Age[@value='Under 10']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='Under 10']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='10-12']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='10-12']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='13-14']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='13-14']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='15']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='15']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='16']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='16']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='17']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='17']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='18']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='18']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='19']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='19']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='20']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='20']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='21']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='21']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='22']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='22']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='23']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='23']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='24']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='24']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='25-29']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='25-29']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='30-34']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='30-34']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='35-39']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='35-39']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='40-44']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='40-44']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='45-49']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='45-49']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='50-54']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='50-54']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='55-59']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='55-59']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='60-64']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='60-64']/F" />
                            </td>
                            <td>
                              <xsl:if test="not(Ages/Age[@value='65+']/F)">0</xsl:if>
                              <xsl:value-of select="Ages/Age[@value='65+']/F" />
                            </td>
                            <td>
                              <xsl:value-of select="sum(Ages/Age/F)" />
                            </td>
                          </tr>
                        </xsl:for-each>
                        <tr>
                          <th class="rowheader" scope="row" colspan="3">Totals</th>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='Under 10']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='10-12']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='13-14']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='15']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='16']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='17']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='18']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='19']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='20']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='21']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='22']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='23']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='24']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='25-29']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='30-34']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='35-39']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='40-44']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='45-49']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='50-54']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='55-59']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='60-64']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age[@value='65+']/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//Ages/Age/*)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Adult/Races/White)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Juvenile/Races/White)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Adult/Races/Black)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Juvenile/Races/Black)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Adult/Races/AmericanIndian)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Juvenile/Races/AmericanIndian)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Adult/Races/Asian)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Juvenile/Races/Asian)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Adult/Races/NativeHawaiianOrOther)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Juvenile/Races/NativeHawaiianOrOther)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Adult/Ethnicities/Hispanic)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Juvenile/Ethnicities/Hispanic)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Adult/Ethnicities/Non-Hispanic)" />
                          </td>
                          <td>
                            <xsl:value-of select="sum(//ASRSummary/UCR/Juvenile/Ethnicities/Non-Hispanic)" />
                          </td>
                        </tr>
                      </tbody>
                    </table>
                    <table>
                      <thead>
                        <tr>
                          <th colspan="2">Police Disposition of Juveniles - Not to Include Neglect or Traffic Cases</th>
                        </tr>
                        <tr>
                          <th colspan="2">(Follow your state age definition for juveniles)</th>
                        </tr>
                        <tr>
                          <th>Disposition Category</th>
                          <th>Number Handled/Referred</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <th class="rowheader">Handled Within Department and Released. (Warning, released to parents, etc.)</th>
                          <td>
                            <xsl:if test="not(//JuvenileDispositions/Department[1])">0</xsl:if>
                            <xsl:value-of select="//JuvenileDispositions/Department[1]" />
                          </td>
                        </tr>
                        <tr>
                          <th class="rowheader">Referred to Juvenile Court or Probation Department</th>
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
                          <th class="rowheader">Referred to Criminal or Adult Court</th>
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
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="wrapper">
    <xsl:apply-templates select="document(./@Source)" />
  </xsl:template>
</xsl:stylesheet>