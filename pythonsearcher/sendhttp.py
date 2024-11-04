import socket
sever_socket1 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
def StartServer:
	server_socket1.bind(('localhost', 7000))
	server_socket1.listen(1)
	(connection1, client_ip) = server_socket.accept()
	data1 = connection1.recv(1024)
	datatr = data1.decode('utf-8')
	connection1.sendall('this notification')
	connection1.close()
	
