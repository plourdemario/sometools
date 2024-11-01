import datetime

def check_date(date1):
    correctDate = None
    try:
        newDate = strptime(date1, "%Y/%m/%d")
        correctDate = True
    except ValueError:
        correctDate = False
    return correctDate

def check_datetime(date1):
	correctDate = None
	try:
		newDatTime = strptime(date1, "%Y/%m/%d %X")
		correctDate = True
	except ValueError:
		correctDate = False
	return correctDate

def check_int(number1):
	correctnumber = None
	try:
		val = int(number1)
		correctnumber = True
	except ValueError:
		correctnumber = False
	return correctnumber

def check_float(decimal1):
	correctdecimal = None
	try:
		val = float(decimal1)
		correctdecimal = True
	except ValueError:
		correctdecimal = False
	return correctdecimal
