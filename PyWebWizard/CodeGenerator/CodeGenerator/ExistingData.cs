using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace CodeGenerator
{
    public partial class ExistingData : Form
    {
        private Creation creation1;
        private DataClass dataclass1;
        private Dataset dataset1;
        private Dataset leftdataset1;
        private Dataset rightdataset1;
        private string leftjoinfield1;
        private string rightjoinfield1;
        private ArrayList filters1;

        

        public ExistingData(Creation creationtemp, DataClass dataclasstemp)
        {
            InitializeComponent();
            creation1 = creationtemp;
            dataclass1 = dataclasstemp;
            dataclass1.SetListDatasets(ExistingDatasets);
            filters1 = new ArrayList();
        }

        public void SetRightNull()
        {
            rightdataset1 = null;
        }

        private void ExistingData_Load(object sender, EventArgs e)
        {
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (dataclass1.DatasetExists(DatasetNameText.Text) == false)
            {
                if (DatasetNameText.Text == "")
                {
                    MessageBox.Show("Please enter a dataset name.");
                }
                else
                {
                    if (AddedDatasets.SelectedItem != null && ExistingDatasets.SelectedItem != null)
                    {
                        if (ExistingDatasets.SelectedItem.ToString() != AddedDatasets.SelectedItem.ToString())
                        {
                            ExistingDatasets.Enabled = false;
                            AddedDatasets.Enabled = false;
                            AddButton.Enabled = false;
                            MessageBox.Show("Datasets will be joined with primary IDs when you click the save button.");
                            //JoinDatasets joiner1 = new JoinDatasets(this, dataclass1, leftdataset1, rightdataset1);
                            //joiner1.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Error: cannot join a dataset to itself"); 
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please make a selection");
                        
                    }
                }
            }
            else
            {
                MessageBox.Show("Dataset name already exists. Please modify.");
            }
        }

        public static Dataset DeepCopy<Dataset>(Dataset other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (Dataset)formatter.Deserialize(ms);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (dataclass1.DatasetExists(DatasetNameText.Text) == false)
            {
                if (leftdataset1 != null && rightdataset1 != null && DatasetNameText.Text != "")
                {
                    Dataset dataset1 = new Dataset(DatasetNameText.Text, "Existing");
            		SQLDefinition sqldefinition1 = new SQLDefinition(dataclass1, true);
            		//sqldefinition1.CreateLinkingTable(leftdataset1.GetTable(), rightdataset1.GetTable(), DatasetNameText.Text);
                    Dataset leftcopy1 = DeepCopy(leftdataset1);
                    
                    leftcopy1.SetDatasetName(DatasetNameText.Text);                    
                    Dataset rightcopy1 = DeepCopy(rightdataset1);
                    rightcopy1.SetFilters(filters1);
                    rightcopy1.SetDatasetName("Right Dataset1");
                    rightcopy1.GetTable().SetProgramCreatedFalse();
                    Table lefttable1 = dataclass1.GetLeftTable(leftcopy1.GetTable());
                    lefttable1.SetProgramCreatedFalse();
                    //rightdataset1.GetTable().SetPrimaryID(rightjoinfield1);
                    lefttable1.AddTableField(rightcopy1.GetTable());

                    for (int i = 0; i < rightcopy1.GetTable().GetFields().Count; i++)
                    {
                        try
                        {
                            Field field1 = (Field)rightcopy1.GetTable().GetFields()[i];
                            field1.SetParrents(lefttable1.GetPrimaryID(), rightcopy1.GetTable().GetPrimaryID());
                        }
                        catch
                        {
                        }
                    }
                    
                    dataset1.SetTable(leftcopy1.GetTable());
                    
                    sqldefinition1.CreateLinkingTable(lefttable1, rightcopy1.GetTable(), lefttable1);
                    creation1.AddDataset(dataset1.GetName());
                    dataset1.SetFilters(filters1);
                    
                    dataclass1.AddDataset(dataset1);
                    this.Close();
                }
                else
                {
                    if (leftdataset1 != null && rightdataset1 == null)
                    {
                        Dataset leftcopy1 = DeepCopy(leftdataset1);
                        leftcopy1.SetDatasetName(DatasetNameText.Text);
                        //dataset1.SetTable(leftdataset1.GetTable());
                        leftcopy1.GetTable().SetProgramCreatedFalse();
                        creation1.AddDataset(leftcopy1.GetName());
                        //dataset1.SetFilters(filters1);
                        dataclass1.AddDataset(leftcopy1);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please select a dataset.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Dataset name already exists. Please choose another dataset name.");
            }
            
        }


        /*public Table SetLinks(Table tabletemp1)
        {
            SQLDefinition definition1 = new SQLDefinition(dataclass1);
            for (int i = 0; i < tabletemp1.GetFields().Count; i++)
            {
                try
                {
                    Table table1 = (Table)tabletemp1.GetFields()[i];
                    definition1.CreateLinkingTable(tabletemp1, table1, tabletemp1);
                    return SetLinks(table1);
                }
                catch
                {
                }
            }
            return tabletemp1;
        }*/

        
        
        private void ExistingDatasets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ExistingDatasets.SelectedItem != null)
            {
                leftdataset1 = dataclass1.GetDataset(ExistingDatasets.SelectedItem.ToString());
            }
        }

        private void AddedDatasets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AddedDatasets.SelectedItem != null)
            {
                rightdataset1 = dataclass1.GetDataset(AddedDatasets.SelectedItem.ToString());
            }
        }

        public void AddLink(string leftfieldname1, string rightfieldname1)
        {
            ExistingDatasets.Enabled = false;
            AddedDatasets.Enabled = false;
            rightjoinfield1 = rightfieldname1;
            leftjoinfield1 = leftfieldname1;
        }

        private void LeftDataSelection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FiltersButton_Click(object sender, EventArgs e)
        {
            if (ExistingDatasets.SelectedItem != null)
            {
                Dataset dataset1 = dataclass1.GetDataset(ExistingDatasets.SelectedItem.ToString());
                FiltersModifier modifier1 = new FiltersModifier(this, dataset1);
                modifier1.ShowDialog();
            }else
            {
                MessageBox.Show("A left side dataset must be selected");
            }
        }

        public void SetFilters(ArrayList filterstemp)
        {
            filters1 = filterstemp;
        }

        private void ListJoinButton_Click(object sender, EventArgs e)
        {
            dataclass1.SetListDatasets(AddedDatasets);
        }
    }

}
