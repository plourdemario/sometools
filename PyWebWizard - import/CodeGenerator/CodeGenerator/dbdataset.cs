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
    public partial class dbdataset : Form
    {
        private Creation creation1;
        private DataClass dataclass1;
        private Dataset dataset1;
        public dbdataset(Creation creationtemp, DataClass dataclasstemp)
        {
            creation1 = creationtemp;
            dataclass1 = dataclasstemp;
            InitializeComponent();
            SQLDefinition definition1 = new SQLDefinition(dataclass1, true);
            if (dataclasstemp.GetDBType() == "SQLite")
            {
                AutoIncrementCB.Visible = true;
                MessageBox.Show("Please be advised that SQLite doesn't have any date, time or datetime types.");
            }
            else
            {
                AutoIncrementCB.Visible = false;
            }
            definition1.FillListbox(ExistingDBTables);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (DatasetNameText.Text == "")
            {
                MessageBox.Show("Please enter a dataset name");
            }
            else
            {
                if (ExistingDBTables.SelectedItem == null)
                {
                    MessageBox.Show("Please select a table");
                }
                else
                {
                    dataset1 = dataclass1.GetDataset(ExistingDBTables.SelectedItem.ToString());
                    dataset1 = new Dataset(DatasetNameText.Text, "Existing");
                    SQLDefinition definition1 = new SQLDefinition(dataclass1, true);
                    if (definition1.CheckSupportTypes(ExistingDBTables.SelectedItem.ToString()) == true)
                    {
                        string primarykey1 = definition1.GetPrimaryKey(ExistingDBTables.SelectedItem.ToString());
                        bool isincrement1 = false;
                        if (dataclass1.GetDBType() == "SQLite")
                        {
                            if(AutoIncrementCB.Checked)
                            {
                                isincrement1 = true;
                            }
                        }else
                        {
                            isincrement1 = definition1.IsIncrement(ExistingDBTables.SelectedItem.ToString(), primarykey1);
                        }
                        
                        Table lefttabletemp1 = new Table(ExistingDBTables.SelectedItem.ToString(), definition1.GetPrimaryKey(ExistingDBTables.SelectedItem.ToString()), isincrement1);
                        if(isincrement1 == false)
                        {
                            Field field2 = new Field(primarykey1, definition1.GetFieldType(ExistingDBTables.SelectedItem.ToString(), primarykey1), primarykey1, 255, 0, 0);
                            ArrayList list1 = new ArrayList();
                            list1.Add(field2);
                            list1.AddRange(definition1.GetFieldsFromDB(ExistingDBTables.SelectedItem.ToString(), primarykey1));
                            lefttabletemp1.SetFields(list1);
                        }else
                        {
                            lefttabletemp1.SetFields(definition1.GetFieldsFromDB(ExistingDBTables.SelectedItem.ToString(), primarykey1));
                        }
                        
                        
                        
                        dataset1.SetTable(lefttabletemp1);
                        dataset1.GetTable().SetProgramCreatedFalse();
                        //dataset1.SetTable(lefttabletemp1);
                        creation1.AddDataset(dataset1.GetName());
                        dataclass1.AddDataset(dataset1);
                        
                        this.Close();
                    }
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Are you sure you want to cancel these changes?", "Discard changes", MessageBoxButtons.YesNo);
            if (result1 == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dbdataset_Load(object sender, EventArgs e)
        {

        }

        
    }
}
