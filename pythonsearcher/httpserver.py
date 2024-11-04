import socket
server_socket1 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket1.bind(('localhost', 7000))
connection1 = ""
while True:
	server_socket1.listen()
	(connection1, client_ip) = server_socket1.accept()
	try:
		data1 = connection1.recv(1024)
		datastr = data1.decode('utf-8')
		if datastr == "A2AH@1jkl2asht3Ashe":
			connection1.sendall("atqHAwhdahwMah12sh3")
			data2 = connection.recv(1024)
			newdata = data2.split(',')
			email1 = newdata[0]
			record = newdata[1]
			cursor1 = cursorset()
			#execute the search for the user
			cursor1.execute("SELECT UserInfo.Email, UserInfo.UserInfo, UserInfo.Verified, Subscriptions.SubscriptionID FROM UserInfo LEFT JOIN Subscriptions ON UserInfo.Userid = Subscriptions.Userid WHERE UserInfo.Email = '" + email1.lower() + "' ORDER BY Subscriptions.EndDate DESC;")
			#pull the entry
			result = cursor1.fetchone()
			if result != None:
				cursor1.execute("SELECT Companies.Name investorrelations.datetime1 FROM UserMonitorSignup, companies, investorrelations WHERE InvestorRelations.CompanyID = UserMonitorSignup.companies AND UserMonitorSignup.companies = Companies.Companies AND InvestorRelations.datetime1 >= CURDATE() AND usermonitorsignup.UserID = PWAW_Users.UserID and PWAW_Users.UserID = UserInfo.UserID And UserInfo.Email =  '" + email1 + "';")
				result2 = cursor1.fetchone()
				if result2 != None:	
					connection1.sendall(result2[0] - " news post " + result2[1])
			cursor1.close()
	except:
		print("Error occured during transfer")
connection1.close()
	
