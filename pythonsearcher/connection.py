import base64
password1 = "JWFjazFiYXIxaVA="
base64_bytes = password1.encode('ascii')
password_bytes = base64.b64decode(base64_bytes)
password1 = password_bytes.decode('ascii')
import pymysql
#VM is 34.130.250.127
connection1 = pymysql.connect(host="207.161.181.26", user="portaluser", passwd=password1, db="Investment_Leads", port=3306)
def cursorset():
	cursor1 = connection1.cursor()
	return cursor1
def commit1():
	connection1.commit()
def closeconnection():
	connection1.close()
