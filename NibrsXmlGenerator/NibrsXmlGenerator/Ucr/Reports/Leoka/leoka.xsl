<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ucr="http://whatever" version="2.0">
<xsl:function name="ucr:TranslateActivity" as="xs:string">
<xsl:param name="activityNumber" as="xs:string"/>
<xsl:choose>
<xsl:when test="$activityNumber='1'">
<xsl:value-of select = "fdfd"/>
</xsl:when>
<xsl:otherwise>
<xsl:value-of select="UNKNOWN"/>
</xsl:otherwise>
</xsl:choose>
</xsl:function>
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
			#report-header {
				text-align: center;
				font-size: 20px;
			}
		</style>
	</head>
	<body>
		<div id="report-header">Law Enforcement Officers Killed or Assaulted</div>
		<br/>
		<table id="killed-table">
			<thead>
				<tr>
					<th colspan="2">Officers Killed</th>
				</tr>
			</thead>
			<tbody>
				<tr>
					<th class="rowheader">By felonious act</th>
					<td>
						<xsl:if test="not(//Feloneously[1] != '')">0</xsl:if>
						<xsl:value-of select="//Feloneously[1]"/>
					</td>
				</tr>
				<tr>
					<th class="rowheader">By accident or negligence</th>
					<td>
						<xsl:if test="not(Classification/C)">0</xsl:if>
						<xsl:value-of select="Classification/C"/>
					</td>
				</tr>
			</tbody>
		</table>
		<br/>
		<table id="assaults-table">
			<colgroup></colgroup>
			<colgroup></colgroup>
			<colgroup span="4"></colgroup>
			<colgroup span="7"></colgroup>
			<colgroup></colgroup>
			<thead>
				<tr>
					<th colspan="14">Officers Assaulted</th>
				</tr>
				<tr>
					<th rowspan="4">Type of Activity</th>
					<th rowspan="3">Total Assaults by Weapon</th>
					<th colspan="4">Type of Weapon</th>
					<th colspan="7">Type of Assignment</th>
					<th rowspan="3">Officer Assaults Cleared</th>
				</tr>
				<tr>
					<!-- Weapons -->
					<th rowspan="2">Firearm</th>
					<th rowspan="2">Knife or Cutting Instrument</th>
					<th rowspan="2">Other Dangerous Weapons</th>
					<th rowspan="2">Hands, Fists, Feet, etc.</th>
					<!-- Assignments -->
					<th rowspan="2">Two-Officer Vehicle</th>
					<th colspan="2">One-Officer Vehicle</th>
					<th colspan="2">Detective or Special Assignment</th>
					<th colspan="2">Other</th>
				</tr>
				<tr>
					<!-- Assignments From Column G -->
					<th>Alone</th>
					<th>Assisted</th>
					<th>Alone</th>
					<th>Assisted</th>
					<th>Alone</th>
					<th>Assisted</th>
				</tr>
				<tr>
					<th>A</th>
					<!-- Weapons -->
					<th>B</th>
					<th>C</th>
					<th>D</th>
					<th>E</th>
					<th>F</th>
					<th>G</th>
					<th>H</th>
					<th>I</th>
					<th>J</th>
					<th>K</th>
					<th>L</th>
					<th>M</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="LeokaSummary/Assaults/Classification">
				<tr>
					<th class="rowheader">
						<xsl:choose>
						<xsl:when test="@name='1'">
						<xsl:value-of select="'Responding to Disturbance Call (Family Quarrels, Person with Firearm, Etc.)'"/>
						</xsl:when>
						<xsl:when test="@name='2'">
						<xsl:value-of select="'Burglaries in Progress or Pursuing Burglary Suspects'"/>
						</xsl:when>
						<xsl:when test="@name='3'">
						<xsl:value-of select="'Robberies in Progress or Pursuing Robbery Suspects'"/>
						</xsl:when>
						<xsl:when test="@name='4'">
						<xsl:value-of select="'Attempting Other Arrests'"/>
						</xsl:when>
						<xsl:when test="@name='5'">
						<xsl:value-of select="'Civil Disorder (Riot, Mass Disobedience)'"/>
						</xsl:when>
						<xsl:when test="@name='6'">
						<xsl:value-of select="'Handling, Transporting, Custody of Prisoners'"/>
						</xsl:when>
						<xsl:when test="@name='7'">
						<xsl:value-of select="'Investigating Suspicious Persons or Circumstances'"/>
						</xsl:when>
						<xsl:when test="@name='8'">
						<xsl:value-of select="'Ambush-No Warning'"/>
						</xsl:when>
						<xsl:when test="@name='9'">
						<xsl:value-of select="'Handling Persons with Mental Illness'"/>
						</xsl:when>
						<xsl:when test="@name='10'">
						<xsl:value-of select="'Traffic Pursuits and Stops'"/>
						</xsl:when>
						<xsl:when test="@name='11'">
						<xsl:value-of select="'All Other'"/>
						</xsl:when>
						<xsl:when test="@name='12'">
						<xsl:value-of select="'Totals'"/>
						</xsl:when>
						<xsl:when test="@name='13'">
						<xsl:value-of select="'Numbers with Personal Injuries'"/>
						</xsl:when>
						<xsl:when test="@name='14'">
						<xsl:value-of select="'Numbers with Personal Without Injuries'"/>
						</xsl:when>
						</xsl:choose>
					</th>
					<td>
						<xsl:if test="not(A)">0</xsl:if>
						<xsl:value-of select="A"/>
					</td>
					<td>
						<xsl:if test="not(B)">0</xsl:if>
						<xsl:value-of select="B"/>
					</td>
					<td>
						<xsl:if test="not(C)">0</xsl:if>
						<xsl:value-of select="C"/>
					</td>
					<td>
						<xsl:if test="not(D)">0</xsl:if>
						<xsl:value-of select="D"/>
					</td>
					<td>
						<xsl:if test="not(E)">0</xsl:if>
						<xsl:value-of select="E"/>
					</td>
					<td>
						<xsl:if test="not(F)">0</xsl:if>
						<xsl:value-of select="F"/>
					</td>
					<td>
						<xsl:if test="not(G)">0</xsl:if>
						<xsl:value-of select="G"/>
					</td>
					<td>
						<xsl:if test="not(H)">0</xsl:if>
						<xsl:value-of select="H"/>
					</td>
					<td>
						<xsl:if test="not(I)">0</xsl:if>
						<xsl:value-of select="I"/>
					</td>
					<td>
						<xsl:if test="not(J)">0</xsl:if>
						<xsl:value-of select="J"/>
					</td>
					<td>
						<xsl:if test="not(K)">0</xsl:if>
						<xsl:value-of select="K"/>
					</td>
					<td>
						<xsl:if test="not(L)">0</xsl:if>
						<xsl:value-of select="L"/>
					</td>
					<td>
						<xsl:if test="not(M)">0</xsl:if>
						<xsl:value-of select="M"/>
					</td>
				</tr>
				</xsl:for-each>
			</tbody>
		</table>
		<br/>
		<table id="assaults-time-table">
			<thead>
				<tr><th colspan="7">Time of Assaults</th></tr>
			</thead>
			<tbody>
				<tr>
					<td></td>
					<td>12:01-02:00</td>
					<td>2:01-04:00</td>
					<td>4:01-06:00</td>
					<td>6:01-08:00</td>
					<td>8:01-10:00</td>
					<td>10:01-12:00</td>
				</tr>
				<tr>
					<td>AM</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H00-01)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H00-01"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H02-03)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H02-03"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H04-05)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H04-05"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H06-07)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H06-07"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H08-09)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H08-09"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H10-11)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H10-11"/>
					</td>
				</tr>
				<tr>
					<td>PM</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H12-13)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H12-13"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H14-15)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H14-15"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H16-17)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H16-17"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H18-19)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H18-19"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H20-21)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H20-21"/>
					</td>
					<td>
						<xsl:if test="not(LeokaSummary/AssaultsTime/H22-23)">0</xsl:if>
						<xsl:value-of select="LeokaSummary/AssaultsTime/H22-23"/>
					</td>
				</tr>
			</tbody>
		</table>
	</body>
</html>
</xsl:template>
</xsl:stylesheet>