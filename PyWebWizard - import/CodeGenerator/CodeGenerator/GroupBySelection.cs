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
    public partial class GroupBySelection : Form
    {
        private TableEditor tableeditor1;
        private ExistingData existingdata1;
        private Table datatable1;
        private ArrayList groups1;
        public GroupBySelection(Table datatabletemp, TableEditor tableeditortemp)
        {
            datatable1 = datatabletemp;
            tableeditor1 = tableeditortemp;
            InitializeComponent();
        }

        public GroupBySelection(ExistingData existingdatatemp, ArrayList GroupsTemp)
        {
            existingdata1 = existingdatatemp;
            groups1 = GroupsTemp;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            foreach(var item1 in FieldsBox.SelectedItems)
            {
            }
        }
    }
}
