import requests
import pymysql
from connection import cursorset
from connection import commit1
import datetime
import os
def removescript(data1):
    while data1.index("<script") > -1 or data1.index("<script") != None:
        data1 = data1.substring(data1.indexof("<script"), data1.indexof("/script>"))
    return data1
def comparedata(newpage, olddownload):
	data1 = ""
	mismatchcount = 0
	#newpage = removescript(newpage)
	#olddownload = removescript(olddownload)
	olddownloaddata = olddownload.split(" ")
	newpagedata = newpage.split(" ")
	i = 0
	for word in newpagedata:
		if word != olddownloaddata[i]:
			mismatchcount = mismatchcount + 1
			data1 = data1 + word + " "
		else:
			i = i + 1
	if mismatchcount  > 5:
		return data1
	else:
		return None
cursor1 = cursorset()
sqlstring = "DELETE FROM investorrelations"
cursor1.execute(sqlstring)
commit1()
cursor1.execute("SELECT companies, NewsLink, NewsLink2, Name FROM Companies")
results = cursor1.fetchall()
sqlstring = ""
for result1 in results:
	try:
		page1 = requests.get(result1[1], timeout=(10, 10))
	except:
		print("download timed out.")
	try:
		file1 = open("/tmp/Downloads/company" + str(result1[0]) + ".txt", "w", encoding="utf-8")
		file1.write(page1.text)
		file1.close()
		print(str(result1[3]) + " downloaded")
	except:
		print("Error saving" +  str(result1[3]))
#EOD cleanup
dir = '/srv/www/htdocs/main/Exports'
for f in os.listdir(dir):
    os.remove(os.path.join(dir, f))

dir = '/srv/www/htdocs/main/sessions'
for f in os.listdir(dir):
    os.remove(os.path.join(dir, f))
#delete from investorrelations where InvestorRelations.datetime1 <= CURDATE();