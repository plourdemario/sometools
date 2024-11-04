using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace CodeGenerator
{
    public partial class JoinDatasets : Form
    {
        private DataClass dataclass1;
        private Dataset leftdataset1;
        private Dataset  rightdataset1;
        private ExistingData existing1;
        public JoinDatasets(ExistingData existingtemp, DataClass dataclasstemp, Dataset leftdatasettemp, Dataset rightdatasettemp)
        {
            existing1 = existingtemp;
            dataclass1 = dataclasstemp;
            InitializeComponent();
            leftdataset1 = leftdatasettemp;
            rightdataset1 = rightdatasettemp;
            SetLeftList();
            SetRightList();
        }

        public void SetLeftList()
        {
            ArrayList fields1 = leftdataset1.GetTable().GetFields();
            dataclass1.FillListBoxLeft(fields1, LeftListBox);
        }

        public void SetRightList()
        {
            ArrayList fields1 = rightdataset1.GetTable().GetFields();
            dataclass1.FillListBoxRight(fields1, RightListBox);
        }

        private void JoinDatasets_Load(object sender, EventArgs e)
        {

        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (LeftListBox.SelectedItem != null && RightListBox.SelectedItem != null)
            {
                existing1.AddLink(LeftListBox.SelectedItem.ToString(), RightListBox.SelectedItem.ToString());
                
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect selection..");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            existing1.SetRightNull();
            this.Close();
        }
    }
}
