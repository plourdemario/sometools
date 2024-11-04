using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class PickPrimaryID : Form
    {
        DataClass dataclass1;
        ExistingData existing1;
        ArrayList fields1;
        public PickPrimaryID(DataClass dataclasstemp, Table table1, ExistingData existingtemp)
        {
            InitializeComponent();
            dataclass1 = dataclasstemp;
            existing1 = existingtemp;
            fields1 = new ArrayList();
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    fields1.Add(field1);
                }
                catch
                {
                }
            }
            dataclass1.FillListBoxRight(fields1, LeftListBox);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
    
}
