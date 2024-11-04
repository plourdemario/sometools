#!c:/Python3/python.exe



print("content-type: text/html\r\n\r\n")
import codecs
from datetime import datetime
import pymysql
import os
abspath = os.path.abspath(__file__)
dname = os.path.dirname(abspath)
os.chdir(dname)
import cgi
form = cgi.FieldStorage()

headerfile1=codecs.open("header.html", 'r')

from connection import cursorset
cursor = cursorset()
script = "<script>\n"
script = script + "function timedRefresh() {\n"
script = script + "\tlocation.reload(true);\n"
script = script + "}\n"
script = script + "setTimeout(timedRefresh, 60000);\n"
script = script + "</script>\n"
headerfile2 = headerfile1.read().replace("<!--Script-->", script) 
print(headerfile2)
print("Warning! Not all news is an investment opportunity and not all news are added when announced to the public. Make sure to validate the information.")
print("<form action=\"displayallbusinessnews.py\" type-\"POST\">")

sqlstring = ""
checked1 = ""
checked2 = ""
#if form.getvalue('newstype1') == "other":
#	sqlstring = "SELECT Companies.Name, Companies.NewsLink2, Companies.YahooLink, businessnews.datetime1, businessnews.DataID FROM UserMonitorSignup, companies, businessnews WHERE businessnews.CompanyID = UserMonitorSignup.companies AND UserMonitorSignup.companies = Companies.Companies AND businessnews.datetime1 >= CURDATE() AND usermonitorsignup.UserID = '" + account + "';"
#	checked2 = "CHECKED"
#else:
sqlstring = "SELECT Companies.Name, Companies.NewsLink, Companies.YahooLink, investorrelations.datetime1, investorrelations.DataID FROM UserMonitorSignup, companies, investorrelations WHERE InvestorRelations.CompanyID = UserMonitorSignup.companies AND UserMonitorSignup.companies = Companies.Companies AND InvestorRelations.datetime1 >= CURDATE() AND usermonitorsignup.UserID = '" + account + "';"
#checked1 = "CHECKED"
#print("<input type=\"radio\" name=\"newstype1\" value=\"investorrelations\" onchange=\"this.form.submit();\" " + checked1 + "> Investor relations sites<BR>")
#print("<input type=\"radio\" name=\"newstype1\" value=\"other\" onchange=\"this.form.submit();\" " + checked2 + "> Business news sites<BR>")
print("</form>")

cursor.execute(sqlstring)
result = cursor.fetchall()
if result is not None:
	print("<Table border=1>")
	print("<TR>")
	print("<TD>Company</TD>")
	print("<TD>Source</TD>")
	print("<TD>Yahoo Data</TD>")
	print("<TD>Time detected</TD>")
	print("<TD>Data</TD>")
	print("</TR>")
	for row in result:
		print("<TR>")
		print("<TD>" + str(row[0]) + "</a></TD>")
		print("<TD><A href=\"" + str(row[1]) + "\">News Link</A></TD>")
		print("<TD><A href=\"" + str(row[2]) + "\">Yahoo</A></TD>")
		print("<TD>" + str(row[3]) + "</TD>")
		print("<TD><A href=\"selected_showdata.py?dailynewsid=" + str(row[4]) + "&newstype1=investorrelations\">See data</TD>")
		print("</TR>")
	print("</Table>")	
else:
	print("<BR>No news was found for today.<BR><BR>")
print("<BR><BR>This page refreshes every minute. <A href=\"selectcompanies.py\">Register</a> to modify your list of oompanies or to get email notifications.<BR><BR>")
footerfile1=codecs.open("footer.html", 'r')
print(footerfile1.read())
