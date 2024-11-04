import os
import subprocess
from email.message import EmailMessage


def sendMail():
	msg = EmailMessage()
	msg.set_content("this is a test")
	msg['From'] = "plourdemario@yahoo.ca"
	msg['To'] = "plourdemario@yahoo.ca"
	msg['Subject'] = "test sucess!!"   
	sendmail_location = "/usr/sbin/sendmail"
	subprocess.run([sendmail_location, "-t", "-oi"], input=msg.as_bytes())
sendMail()
