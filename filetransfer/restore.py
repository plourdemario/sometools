import os
import csv
import tarfile
import copy
import subprocess
#class that controls the get data functions

            
#Open window to start getting data
filename = "C:\\datatransfer\\data\\l397966.tar"
tar = tarfile.open(filename)
for member in tar.getmembers():
    try:
        tar.extract(member, path='c:\\')
    except IOError:
        print "Skipped file: " + member.name
