/*
        File describing actions for the second new project window describing the database connection and admin account
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class AdminPassword : Form
    {
        //set variables for the project
        DataClass dataclass1;
        MainContainer container1;
        string loginoption2 = "";

        //main function for the window opening
        public AdminPassword(DataClass dataclasstemp, MainContainer containertemp, string loginoption1, string portnumber)
        {
            InitializeComponent();
            //set the main variables for the project
            container1 = containertemp;
            loginoption2 = loginoption1;
            //show the options according to options selected on previous window
            if (loginoption1 == "NOLOGIN")
            {
                UsernameText.Enabled = false;
                PasswordText.Enabled = false;
                PasswordRetypeText.Enabled = false;
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
            PortNumberText.Text = portnumber;
            dataclass1 = dataclasstemp;
        }

        //Ok button actions
        private void OKButton_Click(object sender, EventArgs e)
        {
            //initialiase settings
            SetSettings();
            //set database connection class
            SQLDefinition sqldefinition1 = new SQLDefinition(dataclass1, false);
            //verify entered data
            if ((PasswordText.Text.Length < 8 || DBPasswordText.Text.Length < 8) && dataclass1.GetLogonType() != "NOLOGIN")
            {
                MessageBox.Show("Passwords must be at least 8 characters long.");
            }
            else
            {
                if (PasswordText.Text != PasswordRetypeText.Text)
                {
                    MessageBox.Show("Passwords don't match.");
                }
                else
                {
                    //test database connection
                    if (sqldefinition1.isDBConnectivity() == false)
                    {
                        MessageBox.Show("Invalid SQL database.");
                    }
                    else
                    {
                        //encrypt the password for protection 
                        dataclass1.SetPasswordsEncrypt();

                        //verify is a user table exists
                        if (sqldefinition1.TableExist("PWAW_users") && dataclass1.GetLogonType() == "DBLOGIN")
                        {
                            DialogResult result1 = MessageBox.Show("The main DB table already exists in this database. Would you like to use it?", "User table exists", MessageBoxButtons.YesNo);
                            if (result1 == DialogResult.Yes)
                            {

                                Creation creation1 = new Creation(container1, dataclass1, "Main Menu");
                                creation1.MdiParent = container1;
                                container1.AddCreation(creation1);
                                //check database type....
                                if (loginoption2 == "DBLOGIN")
                                {
                                    sqldefinition1.CreateAdminDataset(creation1, true);
                                }
                                
                                creation1.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Action cancelled.");
                            }
                        }
                        else
                        {
                            //Create the project
                            Creation creation1 = new Creation(container1, dataclass1, "Main Menu");
                            creation1.MdiParent = container1;
                            container1.AddCreation(creation1);
                            if (dataclass1.GetLogonType() == "DBLOGIN")
                            {
                                sqldefinition1.CreateUserTable(creation1);
                            }
                            creation1.Show();
                            this.Close();
                        }
                    }
                }
            }
        }

        //action when user cancels the creation of the project
        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialog1 = MessageBox.Show("Are you sure you want to cancel this project?", "Cancel Project", MessageBoxButtons.YesNo);
            if (dialog1 == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void AdminPassword_Load(object sender, EventArgs e)
        {

        }

        //set project settings
        private void SetSettings()
        {
            dataclass1.SetCredentials(UsernameText.Text, PasswordText.Text);
            dataclass1.SetCredentialsDB(DBHostText.Text, UserIDText.Text, DBPasswordText.Text, PortNumberText.Text, DBText.Text);
        }

        //action take when user tests the database connection
        private void TestButton_Click(object sender, EventArgs e)
        {
            SetSettings();
            SQLDefinition definition1 = new SQLDefinition(dataclass1, false);
            definition1.TestDB();
        }
    }
}
