using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections;

namespace CodeGenerator
{
    public partial class DropDownValues : Form
    {
        ArrayList list1 = new ArrayList();
        FieldEditor editor1;
        public DropDownValues(ArrayList listtemp1, FieldEditor editortemp1)
        {
            InitializeComponent();
            list1 = listtemp1;
            editor1 = editortemp1;
            foreach(string item1 in list1)
            {
                DropDownList.Items.Add(item1);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string text1 = Interaction.InputBox("Please enter a string", "New dropdown item", "", -1, -1);
            if (text1 != "")
            { 
                DropDownList.Items.Add(text1);
                list1.Add(text1);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if(DropDownList.SelectedIndex != -1)
            {
                list1.Remove(DropDownList.SelectedItem.ToString());
                DropDownList.Items.RemoveAt(DropDownList.SelectedIndex);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            editor1.SetDropDownList(list1);
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
