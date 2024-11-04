import smtplib
import base64
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
email1 = "plourdemario@yahoo.ca"
pin = "123556"
server1 = "daytradingsignaler.com"
from1 = "noreply@aqesda.com"
to1 = email1.lower()
email_message =MIMEMultipart('alternative')
email_message['From'] = from1
email_message['To'] = email1
subject1 = "Email verification for Day Trading Signaler\n\n"


html1 = '''
<HTML>
<BODY>
<img src="https://daytradingsignaler.com/investmentnews.jpg"><BR>
'''
html1 = html1 + "Hello,<br><Br>Your key for registering is " + str(''.join(pin)) + ". Please proceed using this key on the registration website."
html1 = html1 + '''
<hr>
This message was produced by Aqesda Software Solutions Inc. All rights reserved.
</BODY>
</HTML>
'''
email_message = MIMEMultipart('alternative')
email_message['From'] = "reply@aqesda.com"
email_message['To'] = "plourdemario@yahoo.ca"
email_message['Subject'] = subject1
email_message.attach(MIMEText("test", "plain"))
email_message.attach(MIMEText(html1, 'html'))
email_string = email_message.as_string()
#context = ssl.create_default_context()
#with smtplib.SMTP_SSL('smtp.ionos.com', 587, context=context) as server:
#	server.login("reply@aqesda.com", "Cantstoptheparty2021!")
#	server.sendmail("reply@aqesda.com", "reply@aqesda.com", email_string)
mail1 = smtplib.SMTP('smtp.ionos.com', 587)
mail1.set_debuglevel(1)
mail1.ehlo()
mail1.starttls()
mail1.ehlo()
#encrypted1 = "VGVtcHBhc3N3b3JkMSE="
#base64_bytes = encrypted1.encode('ascii')
#password_bytes = base64.b64decode(base64_bytes)
#secret_key = password_bytes.decode('ascii')
mail1.login(from1, "Cantstoptheparty2021!")
mail1.sendmail(from1,to1, email_string)
mail1.quit()
