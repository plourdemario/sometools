import requests
import json

apilToken = "o.QhbicYH3lrxH4OUE4hogFWZlPOoRjx7U"
headers1 = {"Accept": "application/json", 
					"Content-Type": "application/json",
					"User-Agent": "pyPushBullet",
					"Access-Token": apilToken 
					}

#get authentication
#url1 = "https://api.pushbullet.com/v2/users/me"
#response1  = requests.get(url1, headers=headers1)

#get devices
#url1 = "https://api.pushbullet.com/v2/devices"
#https://api.pushbullet.com/v2/devices

#ephemeral method
#url1 = "https://api.pushbullet.com/v2/ephemerals"
#data4 = { "push": { "conversation_iden": "+1 204 509 9199", "message": "Hello!",    "package_name": "com.pushbullet.android",    "source_user_iden": "ujx9le26Frw", "target_device_iden": "ujx9le26FrwsjAh334EQoe",    "type": "messaging_extension_reply"},  "type": "push"}

#send text message
data4= {"data":{"addresses":["+12045099199"],"file_type":"image/jpeg","guid":"993aaa48567d91068e96c75a74644159","message":"Text message number 1.","target_device_iden":"ujx9le26FrwsjAh334EQoe"},"file_url":"https://dl.pushbulletusercontent.com/foGfub1jtC6yYcOMACk1AbHwTrTKvrDc/john.jpg"}
url1 = "https://api.pushbullet.com/v2/texts"


response2  = requests.get(url1, headers=headers1, data=data4)
#response2  = requests.get(url1, headers=headers1)

#response2 = json.loads(response1.text)
print(response2.content)

#print(response1.content)