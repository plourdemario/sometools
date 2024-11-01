using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CodeGenerator
{
    public partial class NewProject : Form
    {
        private DataClass dataclass1;
        private MainContainer container1;
        private Creation creation1;
        private bool newproject1;
        

        public NewProject(MainContainer containertemp)
        {
            newproject1 = true;
            container1 = containertemp;
            InitializeComponent();
        }
        
        public NewProject(MainContainer containertemp, DataClass dataclasstemp, Creation creationtemp)
        {
            newproject1 = false;
            dataclass1 = dataclasstemp;
            container1 = containertemp;
            creation1 = creationtemp;
            InitializeComponent();
            if (dataclass1.GetDBType() == "MSSQL")
            {
                MSSQLOption.Checked = true;
            }
            if (dataclass1.GetDBType() == "MySQL")
            {
                MySQLOption.Checked = true;
            }
            if (dataclass1.GetDBType() == "SQLite")
            {
                SQLiteOption.Checked = true;
            }
            if (dataclass1.GetLogonType() == "NOLOGIN")
            {
                NoLoginOption.Checked = true;
            }
            if (dataclass1.GetLogonType() == "HARDLOGIN")
            {
                HardCodedOption.Checked = true;
            }
            if (dataclass1.GetLogonType() == "DBLOGIN")
            {
                DBLoginOption.Checked = true;
            }

            CreateProjectButton.Text = "Update";
            NoLoginOption.Enabled = false;
            HardCodedOption.Enabled = false;
            DBLoginOption.Enabled = false;
            MSSQLOption.Enabled = false;
            MySQLOption.Enabled = false;
            SQLiteOption.Enabled = false;
            dataclass1.ReloadData(WebSiteNameText, HeaderText, FooterText, NoLoginOption, HardCodedOption, DBLoginOption, MSSQLOption, MySQLOption);
            
        }


        private void CreateProjectButton_Click(object sender, EventArgs e)
        {
            if (newproject1)
            {
                if (this.WebSiteNameText.Text == "" || this.WebSiteNameText.Text == null || this.WebSiteNameText.Text == " ")
                {
                    MessageBox.Show("Invalid web site name. Please correct");
                }
                else
                {

                    string logintype1 = "";
                    string dbtypetemp1 = "";
                    if (NoLoginOption.Checked)
                    {
                        logintype1 = "NOLOGIN";
                    }
                    if (HardCodedOption.Checked)
                    {
                        logintype1 = "HARDLOGIN";
                    }
                    if (DBLoginOption.Checked)
                    {
                        logintype1 = "DBLOGIN";
                    }
                    if (MSSQLOption.Checked)
                    {
                        dbtypetemp1 = "MSSQL";
                    }
                    if (MySQLOption.Checked)
                    {
                        dbtypetemp1 = "MySQL";
                    }

                    if (SQLiteOption.Checked)
                    {
                        dbtypetemp1 = "SQLite";
                    }

                    if (logintype1 != "" && dbtypetemp1 != "")
                    {
                        dataclass1 = new DataClass(WebSiteNameText.Text, HeaderText.Text, FooterText.Text, logintype1, dbtypetemp1);
                        //dataclass1 = new DataClass(WebSiteNameText.Text, HeaderText.Text, FooterText.Text, DataValidationCB.Checked);
                        //string filename1 = GetFileName();
                        if (dbtypetemp1 != "SQLite")
                        {
                            AdminPassword credentials1;
                            if (dbtypetemp1 == "MySQL")
                            {
                                credentials1 = new AdminPassword(dataclass1, container1, logintype1, "3306");
                            }
                            else
                            {
                                if (dbtypetemp1 == "MSSQL")
                                {
                                    credentials1 = new AdminPassword(dataclass1, container1, logintype1, "1433");
                                }
                                else
                                {
                                    credentials1 = new AdminPassword(dataclass1, container1, logintype1, "");
                                }
                            }
                            credentials1.ShowDialog();

                        }
                        else
                        {
                            SQLite sqlite1 = new SQLite(dataclass1, container1, logintype1);
                            sqlite1.ShowDialog();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid selection.");
                    }
                }
            }
            else
            {
                string logintype1 = "";
                string dbtypetemp1 = "";
                if (NoLoginOption.Checked)
                {
                    logintype1 = "NOLOGIN";
                }
                if (HardCodedOption.Checked)
                {
                    logintype1 = "HARDLOGIN";
                }
                if (DBLoginOption.Checked)
                {
                    logintype1 = "DBLOGIN";
                }
                if (MSSQLOption.Checked)
                {
                    dbtypetemp1 = "MSSQL";
                }
                if (MySQLOption.Checked)
                {
                    dbtypetemp1 = "MySQL";
                }
                if (SQLiteOption.Checked)
                {
                    dbtypetemp1 = "SQLite";
                }
                dataclass1.SetSettings(WebSiteNameText.Text, HeaderText.Text, FooterText.Text, logintype1, dbtypetemp1);
                creation1.SetTitle(WebSiteNameText.Text);
                this.Close();
            }
        }

        public string GetFileName()
        {
            IFormatter formatter = new BinaryFormatter();
            SaveFileDialog savefile1 = new SaveFileDialog();
            // set a default file name
            savefile1.FileName = "project1.sprj";
            // set filters - this can be done in properties as well
            savefile1.Filter = "Project Files (*.sprj)|*.sprj|All files (*.*)|*.*";

            if (savefile1.ShowDialog() == DialogResult.OK)
            {
                FileStream stream1 = new FileStream(savefile1.FileName, FileMode.CreateNew, FileAccess.Write);
                if (stream1 != null)
                {
                    formatter.Serialize(stream1, dataclass1);
                    stream1.Close();
                }
                return savefile1.FileName;
            }

            return "";
        }

        private void MSSQLOption_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void MySQLOption_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TestButton_Click(object sender, EventArgs e)
        {
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WebSiteNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "|" || e.KeyChar.ToString() == ";")
            {
                e.Handled = true;
            }
        }

        private void HeaderText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "|" || e.KeyChar.ToString() == ";")
            {
                e.Handled = true;
            }
        }

        private void FooterText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "|" || e.KeyChar.ToString() == ";")
            {
                e.Handled = true;
            }
        }


    }
}
