import smtplib
import base64
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
server1 = "daytradingsignaler.com"
from1 = "noreply@smtp.daytradingsignaler.com"
email1 = "noreply@smtp.daytradingsignaler.com"
to1 = email1.lower()
email_message =MIMEMultipart('alternative')
email_message['From'] = from1
email_message['To'] = email1
subject1 = "Password change for Day Trading Signaler\n\n"
email_message['Subject'] = subject1

html1 = '''
<HTML>
<BODY>
<img src="https://daytradingsignaler.com/investmentnews.jpg"><BR>
'''
html1 = html1 + "Hello,<br><Br>Your key for registering is fake. Please proceed using this key on the registration website."
html1 = html1 + '''
<hr>
This message was produced by Aqesda Software Solutions Inc. All rights reserved.
</BODY>
</HTML>
'''
part1 = MIMEText(html1, 'html')
email_message.attach(part1)

mail1 = smtplib.SMTP('192.168.100.112', 587)
mail1.ehlo()
mail1.starttls()
mail1.ehlo()
password_bytes = "Kungfu01!"
secret_key = password_bytes.decode('ascii')
mail1.login(from1, secret_key)
mail1.sendmail(from1,mail1.lower(), email_message.as_string())
mail1.quit()
