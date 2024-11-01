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
    public partial class DBPasswordUpdate : Form
    {
        private DataClass dataclass1;
        public DBPasswordUpdate(DataClass datasettemp)
        {
            InitializeComponent();
            dataclass1 = datasettemp;
        }

        private void DBPasswordButton_Click(object sender, EventArgs e)
        {
                DialogResult result = MessageBox.Show("Are you sure you want to", "Save changes", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    dataclass1.SetNewDBPassword(PasswordText.Text);
                    this.Close();
                }
            }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
