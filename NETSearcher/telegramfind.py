import requests;

apilToken = "enterhere"
url1 = "https://api.telegram.org/bot" + apilToken + "/getUpdates"
response1  = requests.get(url1)

#response2  = requests.get(url1, paramr={"chatid", ""})
print(response1.content)