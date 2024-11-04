import smtplib
import ssl
ctx = ssl.create_default_context(cafile='/etc/postfix/cert.pem')
s = smtplib.SMTP("gmail.com", 587)

