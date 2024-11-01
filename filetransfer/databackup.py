import os
import Tkinter as tk
import csv
import tarfile
import copy
import _winreg
import win32file
import win32con

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

        folderCheckButton = [tk.Checkbutton(self.top, text=queue1[0], variable=var1[0], onvalue=1, offvalue=0)]
        folderCheckButton[0].pack()

        #get all folders in csv to avoid showing to the user
        ifile = open("cdriveavoid.csv", "rt") 
        reader = csv.reader(ifile)
        for row in reader:
            rows.append(row)

        #Check all folders on C: drive and create a button for each that aren't exceptions
        #rows.sort()
        for p in os.listdir(path):            
            abspath = os.path.join(path, p)
            isdir = os.path.isdir(abspath)
            
            if isdir:
                try:
                    #print(rows[count2], abspath)
                    str1 = ''.join(rows[count2])
                except IndexError:
                    print('bad row')
                    
                if abspath.lower() in str1.lower():
                    count2 = count2 + 1
                else:
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
        global var2
        global queue1
        global queue2
        var2 = [tk.IntVar()]
        count1 = 1
        path = 'c:\\Users'
        folderCheckButton2 = [tk.Checkbutton(self.top, text=queue2[0], variable=var2[0], onvalue=1, offvalue=0)]

        #Get a list of profiles
        for p in os.listdir(path):
            abspath = os.path.join(path, p)
            isdir = os.path.isdir(abspath)
            
            if isdir:
                var2.append(tk.IntVar())
                folderCheckButton2.append(tk.Checkbutton(self.top, text=abspath, variable=var2[count1], onvalue=1, offvalue=0))
                queue2.append(abspath)
                folderCheckButton2[count1].pack()
                count1 = count1 + 1
                    
        #button to start the data retreival operations
        self.mySubmitButton = tk.Button(self.top, text='Start now', command=self.getdata)
        self.mySubmitButton.pack()    
    
    #start data pull and put into a zip file        
    def getdata(self):
        print("starting transfer...")
        from tkFileDialog import asksaveasfile
        #put data in a zip file
        global queue1
        global queue2
        global var1
        global var2
        count1 = 1
        filename = asksaveasfile(mode='w',defaultextension=".zip")
        #zipf = zipfile.ZipFile(filename, 'w', zipfile.ZIP_DEFLATED)
        tarfile1 = tarfile.open("sample.tar", "w")
        #add C drive data (except files)
        for row in queue1:
            try:
                #print(var1[count1].get(), ''.join(queue1[count1]))
                if var1[count1].get() == 1:
                    self.addfilestozip(tarfile1, ''.join(queue1[count1]), 0)   
            except IndexError:
                print('bad row1')
            count1 = count1 + 1           

        #get all folders in csv to avoid showing to the user
        global rows2
        rows2 = ["settings"]
        ifile = open("profileavoid.csv", "rt") 
        reader = csv.reader(ifile)
        for row in reader:
            rows2.append(row)
        ifile.close()
        
        #add profile folders drive data 
        count1 = 0
        for row in queue2:
            try:
                if var2[count1].get() == 1:
                    str1 = ''.join(queue2[count1])
                    self.addfilestozip(tarfile1, ''.join(queue2[count1]), 1)
                    count2 = count2 + 1
            except IndexError:
                print('bad row2')
            count1 = count1 + 1
        tarfile1.close()
        
    #add a folder to a zip file created in data transfer
    def addfilestozip(self, zipfile1, path, avoidappdata):
        
        count1 = 0
        global rows2
        rows3 = ["none"]
        if avoidappdata == 1:
            for row in rows2:
                str1 = path + '\\' + ''.join(row)
                str1 = str1.lower()
                rows3.append(str1)
                count1 = count1 + 1        
            
        for p in os.listdir(path):            
            abspath = os.path.join(path, p)
            isdir = os.path.isdir(abspath)
            str1 = ''.join(abspath)
            
            if isdir:
                print(abspath.lower(), rows3)
                if abspath.lower() not in rows3:
                    fattrs = win32file.GetFileAttributes(str1)
                    if fattrs & win32con.FILE_ATTRIBUTE_SYSTEM:
                        print("Skipped system folder")
                    else:
                        self.addfilestozip(zipfile1, abspath, avoidappdata)
                else:
                    print("skipped folder" + abspath.lower())
            else:
                
                #print(str1)
                try:
                    fattrs = win32file.GetFileAttributes(str1)
                    if fattrs & win32con.FILE_ATTRIBUTE_SYSTEM:
                        print("Skipped system files")
                    else:
                        zipfile1.add(str1)
                except IOError:
                    print("Access denied trying to zip file2")

                    
#Open window to start getting data
def onClick():
    inputDialog = MyDialog(root)
    root.wait_window(inputDialog.top)

def profileslist():
    inputDialog2 = ProfilesDialog(root)
    root.wait_window(inputDialog2.top)
    
#add a button on main screen to get data
GetDataButton = tk.Button(root, text='Get data', command=onClick)
GetDataButton.pack()

root.mainloop()
