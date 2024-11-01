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
    public partial class FileList : Form
    {
        public FileList(string filestring1, string pythonpath1)
        {
            //
            InitializeComponent();
            FilesBox.Text = filestring1;
            label2.Text = label2.Text + " " + pythonpath1 + "\\Lib folder.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
