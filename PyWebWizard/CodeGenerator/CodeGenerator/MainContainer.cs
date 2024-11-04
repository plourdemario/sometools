using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
/*
        This file describes the actions of the main window cotaining the child windows and the menu
*/
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace CodeGenerator
{
    public partial class MainContainer : Form
    {
        //set program settings
        private NewProject project1;
        private ArrayList creations1;
        private Int32 version1 = 10;
        private bool licenseok1 = false;

        //open program without opening a file
        public MainContainer()
        {
            InitializeComponent();
            creations1 = new ArrayList();
        }

        //open program and open a file that was in the shortcut
        public MainContainer(string filename1)
        {
            InitializeComponent();
            creations1 = new ArrayList();
            OpenNewFileFunction(filename1);
        }

        //Public function to close the program
        public void CloseMain()
        {
            this.Close();
        }

        //public function to activate the software
        public void SetLicenseOk()
        {
            if(Directory.Exists("C:\\ProgramData\\PWAW") == false)
            {
                Directory.CreateDirectory("C:\\ProgramData\\PWAW");
            }
            BinaryWriter writer1 = new BinaryWriter(File.Open("C:\\ProgramData\\PWAW\\info.dat", FileMode.Create));
            writer1.Write("Ahjhoiu1u9089ajkjkl123");
            writer1.Close();
        }

        //check that the program is registered
        private bool CheckRegisteredTrial()
        {
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (File.Exists("C:\\ProgramData\\PWAW\\info.dat") == true)
            {
                BinaryReader reader1 = new BinaryReader(File.Open("C:\\ProgramData\\PWAW\\info.dat", FileMode.Open));                
                if (reader1.ReadString() == "Ahjhoiu1u9089ajkjkl123")
                {
                    return true;
                }else
                {
                    return false;
                }
            }else
            {
                return false;
            }
            
        }

        //puclic functino to check the type of characters entered 
        public bool VerifyKeyPressed(string string2verify1)
        {
            if (string2verify1.Contains('|') || string2verify1.Contains(';'))
            {
                //MessageBox.Show("Unable to use \"|\" character in this feild.");
                return true;
            }
            return false;
        }

        //remote a child window from main collection
        public void RemoveCreation(Creation creationtemp1)
        {
            creations1.Remove(creationtemp1);
        }

        //set program registery settings
        public void SetRegistry(string filepath1)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey key2 = key.OpenSubKey("QesdaSoftware");
            if (key2 == null)
            {
                key.CreateSubKey("QesdaSoftware");
            }

            key = key.OpenSubKey("QesdaSoftware", true);

            key2 = key.OpenSubKey("PythonWAW");
            if (key2 == null)
            {
                key.CreateSubKey("PythonWAW");
            }
            
            key = key.OpenSubKey("PythonWAW");

            //no permission: key.SetValue("currentfolder", filepath1);
        }

        //action performed when user click on "Save As"
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < creations1.Count; i++)
            {
                Creation creation1 = (Creation)creations1[i];
                SaveFormNew(creation1);
            }
        }

        //Saves the data of a project with folder browser
        public void SaveFormNew(Creation creation1)
        {

            SaveFileDialog file1 = new SaveFileDialog();

            file1.Filter = "Project File (*.wprj)|*.wprj";
            file1.FilterIndex = 2;
            file1.RestoreDirectory = true;

            //Open file dialog and continue if a proper filename is chosen
            if (file1.ShowDialog() == DialogResult.OK)
            {
                //Verify that the file format is correct
                try
                {
                    BinaryWriter writer1 = new BinaryWriter(File.Open(file1.FileName, FileMode.Create));
                    writer1.Write(version1);
                    writer1.Write(creation1.GetDataclass().GetPublicKey());
                    creation1.GetDataclass().SaveClass(writer1);
                    writer1.Close();
                }catch(Exception exception1)
                {
                    MessageBox.Show(exception1.Message);
                }
            }

            //save filename location and save last saved location in register
            creation1.SetFileName(file1.FileName);
            SetRegistry(file1.FileName);
        }

        //action performed when opening a file from the using the file menu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog file1 = new OpenFileDialog();

            file1.Filter = "Project File (*.wprj)|*.wprj";
            file1.FilterIndex = 2;
            file1.RestoreDirectory = true;

            //look for file and open if file is selected
            if (file1.ShowDialog() == DialogResult.OK)
            {
                OpenNewFileFunction(file1.FileName);
                
            }
        }

        /*old method of opening file
        private void OpenOldFileFunction(string tempfilename1)
        {
            if (File.Exists(tempfilename1))
            {
                try
                {
                    BinaryReader reader1 = new BinaryReader(File.Open(tempfilename1, FileMode.Open));
                    Int32 versionloading1 = reader1.ReadInt32();
                    string publickey1 = reader1.ReadString();
                    DataClass dataclass1 = new DataClass(reader1, true);
                    //dataclass1.SetKey(publickey1);
                    Creation creation1 = new Creation(this, dataclass1);
                    creation1.OpenData();
                    creation1.MdiParent = this;
                    SetRegistry(tempfilename1);
                    creation1.SetFileName(tempfilename1);
                    if (!File.Exists(dataclass1.GetDBHost()) && dataclass1.GetDBType() == "SQLite")
                    {
                        MessageBox.Show("SQLite database has been moved or deleted. Please reopen.");
                        OpenFileDialog filename1 = new OpenFileDialog();

                        filename1.Filter = "SQLite Database (*.sqlite)|*.sqlite|SQLite Database (*.db)|*.db";
                        filename1.FilterIndex = 2;
                        filename1.RestoreDirectory = true;

                        if (filename1.ShowDialog() == DialogResult.OK)
                        {
                            dataclass1.SetDBPathSQLite(filename1.FileName);
                        }
                        else
                        {
                            MessageBox.Show("The output will not work properly without the correct database file.");
                        }
                    }
                    creation1.Show();
                    creations1.Add(creation1);
                    reader1.Close();
                }
                catch
                {
                    MessageBox.Show("File format not recognized.");
                }
            }
        }*/

        //open file with new/current method
        private void OpenNewFileFunction(string tempfilename1)
        {
            //confirm that file exists before opening.
            if (File.Exists(tempfilename1))
            {
                //use binaryready to start file opening
                BinaryReader reader1 = new BinaryReader(File.Open(tempfilename1, FileMode.Open));
                Int32 versionloading1 = reader1.ReadInt32();
                string publickey1 = reader1.ReadString();
                DataClass dataclass1 = new DataClass(reader1);
                //dataclass1.SetKey(publickey1);
                Creation creation1 = new Creation(this, dataclass1);
                creation1.OpenData();
                creation1.MdiParent = this;
                SetRegistry(tempfilename1);
                creation1.SetFileName(tempfilename1);
                //verify that SQLite database has not been moved if they are using an SQLite database
                if (!File.Exists(dataclass1.GetDBHost()) && dataclass1.GetDBType() == "SQLite")
                {
                    MessageBox.Show("SQLite database has been moved or deleted. Please reopen.");
                    OpenFileDialog filename1 = new OpenFileDialog();

                    filename1.Filter = "SQLite Database (*.sqlite)|*.sqlite|SQLite Database (*.db)|*.db";
                    filename1.FilterIndex = 2;
                    filename1.RestoreDirectory = true;

                    if (filename1.ShowDialog() == DialogResult.OK)
                    {
                        dataclass1.SetDBPathSQLite(filename1.FileName);
                    }
                    else
                    {
                        MessageBox.Show("The output will not work properly without the correct database file.");
                    }
                }

                //Open a project
                creation1.Show();
                creations1.Add(creation1);
                reader1.Close();
                
            }
        }
        
        //action taken when a new project is created
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            //open a new project window
            project1 = new NewProject(this);

            project1.ShowDialog();
            
        }

        //public function to open a project
        public void AddCreation(Creation creation1)
        {
            creations1.Add(creation1);
        }
        
        //action taken when choosing to save a project without specifying a file name
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //save all projects
            for (int i = 0; i < creations1.Count; i++)
            {
                Creation creation1 = (Creation)creations1[i];
                SaveSingleNew(creation1);
            }
        }

        //Save a single project
        public void SaveSingleNew(Creation creation1)
        {
            if (creation1.GetSaveRequired())
            {
                if (creation1.GetFileName() == null)
                {
                    //save when filename was never chosen
                    SaveFormNew(creation1);
                }
                else
                {
                    //use binary writer to save project
                    BinaryWriter writer1 = new BinaryWriter(File.Open(creation1.GetFileName(), FileMode.Create));
                    writer1.Write(version1);
                    writer1.Write(creation1.GetDataclass().GetPublicKey());
                    creation1.GetDataclass().SaveClass(writer1);
                    writer1.Close();
                }
            }

        }

        //action taken when project loads
        private void MainContainer_Load(object sender, EventArgs e)
        {
            //check that the software is activated
            if (CheckRegisteredTrial() == false)
            {
                GetLicenseKey license1 = new GetLicenseKey(this);
                license1.ShowDialog();
            }
        }
        
        

        private void saeBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        //action taken when user clicks on "about" from help menu
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Python 3 Web Application Wizard\nVersion 1.1\n© 2021 Copyright Aqesda Software Solutions Inc.\nAll rights reserved.", "About Python 3 Web Application Wizard");
        }

        /*old method taken when opening a file
        private void openOldToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog file1 = new OpenFileDialog();

            file1.Filter = "Project File (*.wprj)|*.wprj";
            file1.FilterIndex = 2;
            file1.RestoreDirectory = true;

            if (file1.ShowDialog() == DialogResult.OK)
            {
                OpenOldFileFunction(file1.FileName);
                //stream1 = new StreamReader(file1.FileName);
                //if (stream1 != null)
            }
        }*/
    }

    /*old method of opening a file. Now binary instead of string
    public class OpenFile
    {
        private string[] openstring1;
        public string[] BreakApart(string newline1)
        {
            string[] splitline1 = newline1.TrimEnd('\n').TrimEnd(';').Substring(newline1.IndexOf('=') + 1).Split('|');
            return splitline1;
        }

        public string GetNextType()
        {
            if (openstring1[0].Trim('\n').StartsWith("table"))
            {
                return "Table";
            }
            if (openstring1[0].Trim('\n').StartsWith("Field"))
            {
                return "Field";
            }
            return "";
        }

        public string[] GetCurrentLineSplit()
        {
            return BreakApart(openstring1[0]);
        }

        public OpenFile(string openstringtemp)
        {
            openstring1 = openstringtemp.TrimEnd('\n').TrimEnd(';').Split(';');
        }

        public void RemoveAtStart()
        {
            int i = 0;
            foreach (string string1 in openstring1)
            {
                i++;
            }
            string[] newstringlist1 = new string[i - 1];
            int i3 = 0;
            for (int i2 = 1; i2 < i; i2++)
            {
                newstringlist1[i3] = openstring1[i2];
                i3++;
            }
            openstring1 = newstringlist1;
        }
        
    }*/
}
