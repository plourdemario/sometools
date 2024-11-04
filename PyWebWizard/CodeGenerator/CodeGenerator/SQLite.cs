using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace CodeGenerator
{
    public partial class SQLite : Form
    {
        DataClass dataclass1;
        MainContainer container1;
        public SQLite(DataClass dataclasstemp, MainContainer containertemp, string loginoption1)
        {
            InitializeComponent();
            dataclass1 = dataclasstemp;
            container1 = containertemp;
            if (loginoption1 == "NOLOGIN")
            {
                UsernameText.Enabled = false;
                PasswordText.Enabled = false;
                RetypeText.Enabled = false;
            }
            if (loginoption1 == "HARDLOGIN")
            {
                UsernameText.Enabled = true;
                PasswordText.Enabled = true;
            }
            if (loginoption1 == "DBLOGIN")
            {
                UsernameText.Enabled = true;
                PasswordText.Enabled = true;
            }
            if (loginoption1 == "ADLOGIN")
            {
                UsernameText.Enabled = true;
                PasswordText.Enabled = false;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialog1 = MessageBox.Show("Are you sure you want to cancel this project?", "Cancel Project", MessageBoxButtons.YesNo);
            if (dialog1 == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            dataclass1.SetCredentials(UsernameText.Text, RetypeText.Text);
            
            SQLDefinition sqldefinition1 = new SQLDefinition(dataclass1, false);
            if(PasswordText.Text.Length < 8 && dataclass1.GetLogonType() != "NOLOGIN")
            {
            	MessageBox.Show("Admin password must be at least 8 characters long.");
            }else
            {
	            if (PasswordText.Text != RetypeText.Text)
	            {
	                MessageBox.Show("Passwords don't match.");
	            }
	            else
	            {
	                if (sqldefinition1.isDBConnectivity() == false && ExistingDBOption.Checked)
	                {
	                    MessageBox.Show("Invalid SQLite database.");
	                }
	                else
	                {
	                    if (NewDatabaseOption.Checked)
	                    {
                            if (SQLiteDBPasswordText.Text != "")
	                        {
	                            SQLiteConnection sqliteconnection1 = new SQLiteConnection("Data Source=" + SQLitePathText.Text + ";Version=3;");

                                dataclass1.SetCredentialsDB(SQLitePathText.Text, "", "", "", "");
                                //sqldefinition1.SetPassword(SQLiteDBPasswordText.Text);
                                sqldefinition1.SetConnectionSQLite();
                                sqldefinition1.TestDB();

	                        }
	                    }
                        if (ExistingDBOption.Checked)
                        {
                            if (SQLiteDBPasswordText.Text != "")
                            {
                                dataclass1.SetCredentialsDB(SQLitePathText.Text, "", "", "", "");
                            }
                        }
	                    Creation creation1 = new Creation(container1, dataclass1, "Main Menu");
	                    if (sqldefinition1.TableExist("PWAW_Users") == false && dataclass1.GetLogonType() == "DBLOGIN")
	                    {
                            dataclass1.SetPasswordsEncrypt();
	                        sqldefinition1.CreateUserTable(creation1);
	                        creation1.MdiParent = container1;
	                        container1.AddCreation(creation1);
                            MessageBox.Show("Please note that once the code is generated a copy of this database will be placed in the working folder and the database at this new location will receive all the database changes.");
                            
	                        creation1.Show();
	                        this.Close();
	                    }
	                    else
	                    {
                            if (dataclass1.GetLogonType() == "HARDLOGIN")
                            {
                                dataclass1.SetPasswordsEncrypt();
                                creation1.MdiParent = container1;
                                container1.AddCreation(creation1);
                                MessageBox.Show("Please note that once the code is generated a copy of this database will be placed in the working folder and the database at this new location will receive all the database changes.");

                                creation1.Show();
                                this.Close();
                            }
                            else
                            {
                                if (dataclass1.GetLogonType() == "NOLOGIN")
                                {
                                    dataclass1.SetNoLogin();
                                    creation1.MdiParent = container1;
                                    container1.AddCreation(creation1);
                                    MessageBox.Show("Please note that once the code is generated a copy of this database will be placed in the working folder and the database at this new location will receive all the database changes.");

                                    creation1.Show();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("File already being used for another project. Please create or use another file.");
                                }
                            }
	                    }
	                }
	            }
            }
        }

        private void NewDatabaseOption_CheckedChanged(object sender, EventArgs e)
        {
            SQLitePathText.Enabled = true;
        }

        private void ExistingDBOption_CheckedChanged(object sender, EventArgs e)
        {
            ExistingDBOption.Enabled = true;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (NewDatabaseOption.Checked)
            {
                SaveFileDialog file1 = new SaveFileDialog();
                file1.Filter = "SQLite Database (*.sqlite)|*.sqlite|SQLite Database (*.db)|*.db";
                file1.FilterIndex = 2;
                file1.RestoreDirectory = true;

                if (file1.ShowDialog() == DialogResult.OK)
                {
                    SQLitePathText.Text = file1.FileName;
                    dataclass1.SetCredentialsDB(SQLitePathText.Text, "", "", "", "");
                    SQLDefinition sqldefinition1 = new SQLDefinition(dataclass1, false);
                    sqldefinition1.CreateSQLiteDB(file1.FileName);
                    SQLiteDBPasswordText.Enabled = false;
                }

            }
            else
            {
                OpenFileDialog file1 = new OpenFileDialog();

                file1.Filter = "SQLite Database (*.sqlite)|*.sqlite|SQLite Database (*.db)|*.db";
                file1.FilterIndex = 2;
                file1.RestoreDirectory = true;

                if (file1.ShowDialog() == DialogResult.OK)
                {
                    SQLitePathText.Text = file1.FileName;
                    ExistingDBOption.Checked = true;
                    dataclass1.SetCredentialsDB(SQLitePathText.Text, "", SQLiteDBPasswordText.Text, "", "");
                    SQLDefinition sqldefinition1 = new SQLDefinition(dataclass1, false);
                }
            }
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            SQLDefinition sqldefinition1 = new SQLDefinition(dataclass1, false);
            sqldefinition1.TestDB();
        }

        private void UsernameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "|" || e.KeyChar.ToString() == ";")
            {
                e.Handled = true;
            }
        }
    }
}
