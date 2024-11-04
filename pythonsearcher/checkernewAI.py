import requests
import sys
import time
import pymysql
import filecmp
from datetime import datetime
from connection import cursorset
from connection import commit1
import subprocess
import urllib.request
headers1 = {'user-agent':'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36'} 
def ismatchin(oldwords, newword):
	for oldword in oldwords:
		if oldword.lower() == newword.lower():
			return True
			break
	return False
def trace_function(frame, event, arg):
	if time.time() - start > TOTAL_TIMEOUT:
		raise Exception("website timeout")
	return trace_function
TOTAL_TIMEOUT = 10

words1 = ['word']
file1 = open("wordslist.txt", "r", encoding="utf-8")
words1 = file1.readlines()
print(len(words1))
index1 = ['a','b','c','d','e','f','g','h','i','j','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z']
startindex1 = [0,5582,9472,16366,20291,23046,25713,27942,30539,33391,33965,34573,37013,40930,42576,44328,50066,50436,53385,60652,64230,66759,68005,69414,69495,69688]
endindex1 = [5581,9471,16365,20290,23045,25712,27941,30538,33390,33964,34572,37012,40929,42575,44327,50065,50435,53384,60651,64229,66758,68004,69413,69494,69687,69882]
file1.close()
def isword(word1):
	word1 = word1.lower()
	startsearch1 = 0
	endsearch1 = 0
	for x in range(0,25):
		if index1[x] == word1[0:1]:
			startsearch1 = startindex1[x]
			endsearch1 = endindex1[x]
			break
	for x in range(startsearch1, endsearch1):
		checkword1 = words1[x].lower().strip('\n').strip('\r') 
		if checkword1 == word1:
			return True
			break
	return False
def findnew(data1, searchtext1):
	count1 = 0
	data1 = data1.lower()
	searchtext1 = searchtext1.lower()
	#print(len(searchtext1))
	while count1 < len(data1):
		#print(data1[count1:len(searchtext1) + count1])
		if data1[count1:len(searchtext1) + count1] == searchtext1:
			return count1
		count1 = count1 + 1
	return -1
def removetext(data1, starttext1, endtext1):
	startpoint1 = findnew(data1, starttext1)
	endpoint1 = findnew(data1, endtext1)
	print(startpoint1)
	print(endpoint1)
	if startpoint1 > -1 and endpoint1 > -1:
		if startpoint1 < endpoint1:
			data1 = data1.replace(data1[startpoint1:endpoint1 + len(endtext1)], "")
		else:
			data1 = data1.replace(data1[endpoint1 -len(endtext1):endpoint1], "")
	return data1
def extracttext(data1, starttext1, endtext1):
	startpoint1 = findnew(data1, starttext1)
	endpoint1 = findnew(data1, endtext1)
	print(startpoint1)
	print(endpoint1)
	if startpoint1 > -1 and endpoint1 > -1:
		if startpoint1 < endpoint1:
			data1 = data1[startpoint1:endpoint1 + len(endtext1)]
		else:
			data1 = data1[endpoint1 -len(endtext1):endpoint1]
	return data1
def removespaces(data1):
	count = 0
	for dataitem1 in data1:
		if dataitem1 == "":
			del data1[count]
		count = count + 1
	return data1
def removealltags(data1):
	startcount = 0

	while data1.lower().find("<") > -1 and data1.lower().find(">") > -1:
		data1 = removetext(data1, "<", ">")
	return data1
def removestuff(data1, companyID):
	#cursor2 = cursorset()
	#cursor2.execute("SELECT CompanyID, starttext, endtext FROM companyremotedata WHERE companyID = " + companyID)
	#results2 = cursor2.fetchall()
	#for result2 in results2:
		#data1 = removetext(data1, result2[1], result2[2])
	if findnew(data1, "<body") > -1 and findnew(data1, "/body>") > -1:
		data1 = extracttext(data1, "<body", "/body>")
	while findnew(data1, "<script") > -1 and findnew(data1, "/script>") > -1:
		data1 = removetext(data1, "<script", "/script>") 
	return data1
def comparedata(newpage, olddownload):
	data1 = ""
	linenewpage1 = newpage.splitlines()
	linesoldpage1 = olddownload.splitlines()
	for line1 in linenewpage1:
		line1  = line1.lower().strip()
		for line2 in linesoldpage1:
			if line1 == line2.lower().strip():
				data1 = data1 + line1 + "\n"
				break
				#print(line2)
	return data1
cursor1 = cursorset()
cursor1.execute("SELECT companies.companies, companies.NewsLink, companies.NewsLink2, companies.Name, companies.Bad_word_length from companies WHERE slowwebsite = 0")
results = cursor1.fetchall()
sqlstring = ""
timeoutfile1 = open("errors.txt", "a")
scriptpage1 = open("script1.sh", "w")
scriptpage1.write("rm wget*\n")
for result1 in results:
	scriptpage1.write("timeout -s KILL 15 wget " + result1[1] + " -O /tmp/starter/Downloading" + str(result1[0]) + ".txt --restrict-file-names=nocontrol\n")
	print("Company " + result1[3])
scriptpage1.close()
process1 = subprocess.Popen("./script1.sh", shell=True, stdout=subprocess.PIPE)
process1.wait()

for result1 in results:
	page1 = None
	data1 = None
	page1 = open("/tmp/starter/Downloading" + str(result1[0]) + ".txt")
	page1 = page1.read()
	print(str(result1[3]) + " - start")
	file1 = open("/tmp/Downloads/company" + str(result1[0]) + ".txt", "r", encoding="utf-8")
	filecontent1 = file1.read()
	data1 = comparedata(page1, filecontent1) 
	file1.close()
	#print(result1[3] + " timed out")
    
	if data1 != None and data1 != "":
		#data1 = removestuff(data1, str(result1[0]))
		cleandata1 = data1.replace("\t", "").replace("\r", "").replace("\n", "")
		#splitdata1 = removealltags(cleandata1)
		splitdata1 = cleandata1

		
		print(result1[3] + "got here! - 3")
		while splitdata1.find("  ") > -1:
			splitdata1 = splitdata1.replace("  ", " ")
		splitresponse1 = splitdata1.split(" ")
		wordcount1 = 0
		for reponse1 in splitresponse1:
			if isword(reponse1) == True:
				wordcount1 = wordcount1 + 1
				if wordcount1 > result1[4]:
					break
			print("counting")					
		print(wordcount1)
		if len(splitresponse1)  > result1[4]:
			download1 = open("/tmp/Downloads/company" + str(result1[0]) + ".txt", "r")                
			olddata1 = download1.read()
			download1.close()
			oldsplit1 = olddata1.split(" ")
			matchcount = 0
			for checkword in oldsplit1:
				if ismatchin(splitresponse1, checkword):
					matchcount = matchcount + 1
					if (matchcount > 10 and len(splitdata1) < 30) or (matchcount > 20 and len(splitdata1) < 60) or matchcount > 30:
						break	
			print(wordcount1)
			if (matchcount > 10 and len(splitdata1) < 30) or (matchcount > 20 and len(splitdata1) < 60) or matchcount > 30:
				print(wordcount1)
				date1 = datetime.now().strftime('%Y-%m-%d %H:%M')
				filename1 = "/srv/www/htdocs/main/Exports/company" + str(result1[0]) + str(date1).replace(" ", "").replace("-", "").replace(":", "") + ".txt"
				file1 = open(filename1, "w", encoding="utf-8")
				file1.write(data1)
				file1.close()
				file2 = open("/tmp/Downloads/company" + str(result1[0]) + ".txt", "w", encoding="utf-8")
				file2.write(page1)
				file2.close()
				print(result1[3] + " Download updated")
				sqlstring = "INSERT INTO InvestorRelations (companyID, Datetime1, filename1, Url) VALUES (" + str(result1[0]) + ",'" + str(date1) + "','" + filename1 + "','" + str(result1[1]) + "');"
				#print(sqlstring)
				cursor1.execute(sqlstring)
				commit1()
