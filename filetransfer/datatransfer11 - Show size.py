import os
import Tkinter as tk
import tkMessageBox
import csv
import tarfile
import copy
import _winreg
import win32file
import win32con
import win32api
import subprocess
from threading import Thread
import time
import logging.config

root = tk.Tk()
path = 'C:\\transferdata'
queue1 = ['Registry Settings']
queue2 = ['Skip PST Files']

#class that controls the get data functions
class MyDialog(tk.Frame):
    def __init__(self, parent):
        tk.Frame.__init__(self, parent)
        top = self.top = tk.Toplevel(parent)
        self.myLabel = tk.Label(top, text='Are you sure you want to get the data?')
        self.myLabel.pack()
        self.process_directory(top, 'c:\\')

    #show list of checkboxes to show
    def process_directory(self, parent, path):
        rows = ["Registry Settings"]
        global var1
        var1 = [tk.IntVar()]
        count1 = 1
        count2 = 1
        global folderCheckButton
        global logger
        logger = logging.getLogger('filetransfer_application')
        logger.setLevel(logging.DEBUG)
        # create file handler which logs even debug messages
        fh = logging.FileHandler('backup.log')
        fh.setLevel(logging.DEBUG)
        logger.addHandler(fh)


        folderCheckButton = [tk.Checkbutton(self.top, text=queue1[0], variable=var1[0], onvalue=1, offvalue=0)]
        folderCheckButton[0].pack()

        #get all folders in csv to avoid showing to the user
        ifile = open("cdriveavoid.csv", "rt") 
        reader = csv.reader(ifile)
        for row in reader:
            row[0] = row[0].replace('\\\\', '\\')
            row[0] = row[0].lower()        
            logger.info(row[0])
            rows.append(row[0])

        #Check all folders on C: drive and create a button for each that aren't exceptions
        #rows.sort()
        rowcount1 = 1
        for p in os.listdir(path):            
            abspath = os.path.join(path, p)
            isdir = os.path.isdir(abspath)
            
            if isdir:
                try:
                    str1 = ''.join(rows[count2])
                except IndexError:
                    logger.info('bad row')
                    
                if abspath.lower() in str1.lower():
                    count2 = count2 + 1
                else:
                    if abspath.lower() not in rows:
                        logger.info('paths:'  + abspath.lower())
                        var1.append(tk.IntVar())
                        folderCheckButton.append(tk.Checkbutton(self.top, text=abspath, variable=var1[count1], onvalue=1, offvalue=0))
                        queue1.append(abspath)
                        folderCheckButton[count1].pack()
                        count1 = count1 + 1
                    
        #button to start the data retreival operations
        self.mySubmitButton = tk.Button(self.top, text='Proceed', command=profileslist)
        self.mySubmitButton.pack()

class ProfilesDialog(tk.Frame):
    def __init__(self, parent):
        tk.Frame.__init__(self, parent)
        top = self.top = tk.Toplevel(parent)
        self.profileslist()

    #allow user to pick the user profiles to transfer
    def profileslist(self):
        global queue1
        global queue2
        #global var2
        #var2 = [tk.IntVar()]
        count1 = 1
        #path = 'c:\\Users'
        #folderCheckButton2 = [tk.Checkbutton(self.top, text=queue2[0], variable=var2[0], onvalue=1, offvalue=0)]

        #Get a list of profiles
        queue2.append(os.environ['USERPROFILE'])
        queue2.append(os.environ['USERPROFILE'][:os.environ['USERPROFILE'].rfind('\\')] + "\\Default")
        queue2.append(os.environ['USERPROFILE'][:os.environ['USERPROFILE'].rfind('\\')] + "\\Public")
                    
        #button to start the data retreival operations
        self.StartNowButton = tk.Button(self.top, text='Start now', command=self.starttransfer)
        self.StartNowButton.pack()

        #button to start the data retreival operations
        self.FinishButton = tk.Button(self.top, text='Finish from abort', command=self.abortcontinue)
        self.FinishButton.pack()

        
        self.DataSizeButton = tk.Button(self.top, text='Get Data Size', command=self.Calculate)
        self.DataSizeButton.pack()        
        

        #******get profiles data*****
        
        #get all folders in csv to avoid showing to the user
        global rows2
        rows2 = ["settings"]
        ifile = open("profileavoid.csv", "rt") 
        reader = csv.reader(ifile)
        for row in reader:
            rows2.append(row)
        ifile.close()
        
        #get all designated appdata folders
        #get all folders in csv to avoid showing to the user
        global count4
        count4 = 0
        global rows4
        rows4 = ["empty"]
        ifile = open("appdatafolders.csv", "rt") 
        reader = csv.reader(ifile)
        for row in reader:
            rows4.append(row)
            count4 = count4 + 1
        ifile.close()

    def Calculate(self):
        inputDialog3 = CalculateSize(root)
        root.wait_window(inputDialog3.top)

    def starttransfer(self):
        from tkFileDialog import asksaveasfilename
        #create file from save file dialog
        global filename1
        filename = asksaveasfilename(defaultextension=".tar")
        filename1 = filename
        
        datawindow1 = datawindow(root)
        root.wait_window(datawindow1.top) 

    def abortcontinue(self):
        global filename1
        #self.getdata(filename1)
        
class datawindow(tk.Frame):
    def __init__(self, parent):
        tk.Frame.__init__(self, parent)
        top = self.top = tk.Toplevel(parent)
        global sizelabel
        global pausebutton
        global thread1
        sizelabel = tk.Label(top, text='Backing up data please wait...')
        sizelabel.pack()
        pausebutton = tk.Button(top, text='Get file size', command=self.filesizefunc)
        pausebutton.pack()
        time.sleep(2)
        thread1 = Thread( target=self.getdata, args=() )
        thread1.start()
        #self.waitthread()
        #self.getdata()

    def stopthread(self):
        thread1.join()

    def filesizefunc(self):
        global filename1
        global sizelabel
        zipfilesize1 = os.stat(filename1).st_size
        if(zipfilesize1 > (1024*1024*1024)):
            sizelabel.configure(text="Amount of data backed up: " + str(zipfilesize1/1024/1024/1024) + " GB")
        else:
            if(zipfilesize1 > (1024*1024)):
                sizelabel.configure(text="Amount of data backed up: " + str(zipfilesize1/1024/1024) + " MB")
            else:
                if(zipfilesize1 > 1024):
                    sizelabel.configure(text="Amount of data backed up: " + str(zipfilesize1/1024) + " KB")
                else:
                    sizelabel.configure(text="Amount of data backed up: " + str(zipfilesize1/1024) + " Bytes")
    def getdata(self):
        #get global variables
        global var1
        global queue1
        global queue2
        global count4
        global archivefiles
        global filename1
        archivefiles = ["init"]

        tarfile1 = tarfile.open(filename1, 'w')

        #get list of registry keys to register
        global count5
        dataget1 = datagetclass()
        count5 = 0
        global rows5
        global logger
        rows5 = ["empty"]
        ifile = open("registry2.csv", "rt") 
        reader = csv.reader(ifile)
        for row in reader:
            rows5.append(row)
            count5 = count5 + 1
        ifile.close()

        count3 = 1
        
        #get profiles registry keys that are selected
        #reg1 = _winreg.ConnectRegistry(None,_winreg.HKEY_CURRENT_USER)

        newpath = os.environ["USERPROFILE"] + "\\AppData\\Local\\FileTransfer" 
        if not os.path.exists(newpath):
            os.makedirs(newpath)
            
        for row in rows5:
            try:
                subprostring1 = "reg export \"" + row[1] + "\"" + os.environ["USERPROFILE"] + "\\AppData\\Local\\FileTransfer" + str(count3) + ".reg /y"
                #print(subprostring1)
                ch=subprocess.Popen(subprostring1)
                count3 = count3 + 1
            except Exception as e:
                logger.info(e)

        
        
        #add c drive folders
        global var1
        count1 = 1
        
        for row in queue1:
            #print(var1[count1].get(), ''.join(queue1[count1]))
            try:
                if var1[count1].get() == 1:
                    dataget1.addfilestozip(tarfile1,''.join(queue1[count1]) , 0, 0)
            except IndexError:
                logger.error('bad row4')
                        
            count1 = count1 + 1
        
        #add profile folders drive data 
        count1 = 1
        for row in queue2:
            try:
                str1 = ''.join(queue2[count1])
                dataget1.addfilestozip(tarfile1, str1, 1, 0)
                print(str1)
                #appdata folders
                count2 = 1
                while count2 < count4:
                    newstr = str1 + ''.join(rows4[count2])
                    logger.info(newstr)
                    try:
                        dataget1.addfilestozip(tarfile1, newstr, 0, 0)
                    except WindowsError:
                        logger.info('Specified folder not found in this profile')
                    count2 = count2 + 1 
            except IndexError:
                logger.info('bad row5')
                        
            count1 = count1 + 1

        try:
            dataget1.addfilestozip(tarfile1, newpath, 0, 0)
        except Exception as e:
            tkMessageBox.showinfo(e)

        tarfile1.close()
        tkMessageBox.showinfo("Data backup", "Backup completed!")

class datagetclass:
    def addfilestozip(self, zipfile1, path, avoidappdata, getsize):
        count1 = 0
        global rows2
        global filesize
        global filecount
        global archivefiles
        global filename1
        global sizelabel
        global logger
        rows3 = ["none"]
        if avoidappdata == 1:
            for row in rows2:
                str1 = path + '\\' + ''.join(row)
                str1 = str1.lower()
                rows3.append(str1)
                #print(rows3[count1])
                count1 = count1 + 1        
            
        for p in os.listdir(path):            
            abspath = os.path.join(path, p)
            isdir = os.path.isdir(abspath)
            str1 = ''.join(abspath)
            
            if isdir:
                #print(abspath.lower(), rows3)
                if abspath.lower() not in rows3:
                    fattrs = win32file.GetFileAttributes(str1)
                    #self.addfilestozip(zipfile1, abspath, avoidappdata)
                    if fattrs & win32con.FILE_ATTRIBUTE_SYSTEM :
                        logger.info("Skipped system folder")
                    else:
                        self.addfilestozip(zipfile1, abspath, avoidappdata, getsize)
                else:
                    logger.info("skipped folder" + abspath.lower())
            else:             
                #print(str1)
                try:
                    fattrs = win32file.GetFileAttributes(str1)
                    #zipfile1.add(str1)
                    if fattrs & win32con.FILE_ATTRIBUTE_SYSTEM or ((".ost" in str1 or ".old" in str1) and "Local\\Microsoft\\Outlook" in str1) or "OneDrive" in str1:
                        logger.info("Skipped file:" + str1)
                    else:
                        if(getsize == 0):
                            if str1 not in archivefiles:
                                archivefiles.append(str1)
                                zipfile1.add(str1)
                        else:
                            filecount = filecount + 1
                            filesize +=  os.path.getsize(str1)
                            #os.stat(str1).st_size
                except IOError:
                    logger.info("Access denied trying to zip file2")
                except WindowsError:
                    logger.info("Error accessing file:" + str1)

class CalculateSize(tk.Frame):
    def __init__(self, parent):
        tk.Frame.__init__(self, parent)
        top = self.top = tk.Toplevel(parent)
        self.myLabel = tk.Label(top, text='Results')
        self.myLabel.pack()

        self.TextBox = tk.Entry(top, width=100)
        self.TextBox.pack()

        self.TextBox2 = tk.Entry(top, width=100)
        self.TextBox2.pack()

        dataget1 = datagetclass()

        global filesize
        global filecount
        global var1
        #global var2
        global count4
        global queue1
        global queue2
        filesize = 0
        filecount = 0

        #add c drive folders
        count1 = 1
        
        for row in queue1:
            try:
                #print(var1[count1].get(), ''.join(queue1[count1]))
                if var1[count1].get() == 1:
                    #self.addfiles(''.join(queue1[count1]), 0)
                    dataget1.addfilestozip("", ''.join(queue1[count1]), 0, 1)
            except IndexError:
                print('bad row1')    
            count1 = count1 + 1

        #add profile folders drive data 
        count1 = 1
        for row in queue2:
            try:
                str1 = ''.join(queue2[count1])
                dataget1.addfilestozip("", ''.join(queue2[count1]), 1, 1)
                #appdata folders
                count2 = 1
                while count2 < count4:
                    newstr = str1 + ''.join(rows4[count2])
                    try:
                        #self.addfiles(newstr, 0)
                        dataget1.addfilestozip("", newstr, 0, 1)
                    except WindowsError:
                        print('Specified folder not found in this profile')
                    count2 = count2 + 1 
            except IndexError:
                print('bad row2')
            count1 = count1 + 1

        filesize = filesize / 1024 / 1024
        

        if(filesize > 1024):
            self.TextBox.insert(10, "File size: " + str(filesize / 1024) + " GB \n")
        else:
            self.TextBox.insert(10, "File size: " + str(filesize) + " MB \n")
            
        self.TextBox2.insert(10, "File count: " + str(filecount) + " \n")
                  
class ApplyTarDialog(tk.Frame):
    def __init__(self, parent):
        from tkFileDialog import askopenfilename
        filename = askopenfilename(defaultextension=".tar")
        tk.Frame.__init__(self, parent)
        top = self.top = tk.Toplevel(parent)
        self.myLabel = tk.Label(top, text='Writing...')
        self.myLabel.pack()
        
        tar = tarfile.open(filename)
        for member in tar.getmembers():
            #print "Extracting %s" % member.name
            try:
                tar.extract(member, path='c:\\')
            except IOError:
                self.TextBox = tk.Entry(top, width=100)
                self.TextBox.insert(10, "Skipped file: " + member.name + "\n")
                self.TextBox.pack()
            
#Open window to start getting data
def onClick():
    inputDialog = MyDialog(root)
    root.wait_window(inputDialog.top)

def profileslist():
    inputDialog2 = ProfilesDialog(root)
    root.wait_window(inputDialog2.top)

def ApplyTar():
    inputDialog2 = ApplyTarDialog(root)
    root.wait_window(inputDialog2.top)

#add a button on main screen to get data
GetDataButton = tk.Button(root, text='Get data', command=onClick)
GetDataButton.pack()

#add a button on main screen to restore data
PushDataButton = tk.Button(root, text='Restore data', command=ApplyTar)
PushDataButton.pack()

root.mainloop()
