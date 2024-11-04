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
def cpare(olddata1, newdata1):
	data1 = ""
	olddata1 = olddata1.split (" ")
	newdata1 = newdata1.split(" ")
	currentold = 0
	currentnew = 0
	while currentold < len(olddata1):
		currentcheck = 0
		found = False
		while currentnew < len(newdata1) and currentcheck < 75:
			if (currentnew + currentcheck) < len(newdata1):
				if olddata1[currentold].lower().strip() == newdata1[currentnew + currentcheck].lower().strip():
					found = True
					break
			else:
				currentnew = len(newdata1)
				break
			currentcheck = currentcheck + 1
		if (currentnew + currentcheck) < len(newdata1):
			if found == False:
				data1 = data1 + newdata1[currentnew + currentcheck] + "\n"
				#print("data foumd")
				currentnew = currentnew + currentcheck
			else:
				currentnew = currentnew + 1
		else:
			currentnew = len(newdata1) + 1
		currentold = currentold + 1
	return data1
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
		#print(checkword1)
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
	linesnewpage1 = newpage.splitlines()
	linesoldpage1 = olddownload.splitlines()
	i = 0
	for line1 in linesnewpage1:
		i += 1
		found1 = False
		for line2 in linesoldpage1:
			if line1.casefold().strip() == line2.casefold().strip():
				found1 = True
				break
		if found1 == False:
			data1 = data1 + line1 + "\n"	
	return data1
cursor1 = cursorset()
cursor1.execute("SELECT companies.companies, companies.NewsLink, companies.NewsLink2, companies.Name, companies.Bad_word_length from companies")
results = cursor1.fetchall()
sqlstring = ""
for result1 in results:
	page1 = None
	data1 = None
	try:
		start = time.time()
		sys.settrace(trace_function)
		page1 = requests.get(result1[1], headers=headers1, timeout=(10,20))
	except:
		print(str(result1[3]) + " - timed out")
	finally:
		sys.settrace(None)
	try:
		page1 = page1.text
		print(str(result1[3]) + " - start")
		file1 = open("/tmp/Downloads/company" + str(result1[0]) + ".txt", "r", encoding="utf-8")
		filecontent1 = file1.read()
		data1 = comparedata(page1, filecontent1) 
		file1.close()
	except:
		print(str(result1[3]) + " - website issue")
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
				#print("here!!!")
				if wordcount1 > result1[4]:
					break
			#print("counting")					
		print(wordcount1)
		if wordcount1  > result1[4]:
			download1 = open("/tmp/Downloads/company" + str(result1[0]) + ".txt", "r", encoding="utf-8")                
			olddata1 = download1.read()
			download1.close()
			oldsplit1 = olddata1.split(" ")
			matchcount = 0
			for checkword in oldsplit1:
				if ismatchin(splitresponse1, checkword):
					matchcount = matchcount + 1
					if (matchcount > 10 and len(splitresponse1) < 30) or (matchcount > 20 and len(splitresponse1) < 60) or  matchcount > 30:
						break	
			print(matchcount)
			if (matchcount < 10 and len(splitresponse1) < 30) or (matchcount < 20 and len(splitresponse1) < 60) or matchcount < 30:
				print(matchcount)
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