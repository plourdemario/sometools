using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace CodeGenerator
{
    public partial class TableEditor : Form
    {
        private string editstate;
        private Creation creation1;
        private DataClass dataclass1;
        private Table datatable1;
        private Table parrenttable1;
        private Dataset dataset1;
        private ArrayList fields1;
        private ArrayList filters1;
        private FieldEditor fieldeditor1;
        private TableEditor oldeditor1;
        private string rightjointable1;
        private string parenttable1 = "";
        private string parentfield1 = "";
        private SQLDefinition sqldefinition1;
        private ArrayList sqlchanges1 = new ArrayList();
        private bool isquery1 = false;

        public TableEditor(TableEditor oldtableeditortemp, DataClass dataclasstemp, string datasetname1, Table datatabletemp, string fieldname1, Table parrenttabletemp)
        {
            InitializeComponent();
            //creation1 = creationtemp;
            oldeditor1 = oldtableeditortemp;
            dataclass1 = dataclasstemp;
            editstate = "ModifyInternal";
            parrenttable1 = parrenttabletemp;
            datatable1 = DeepCopy(datatabletemp);
            fields1 = datatable1.GetFields();
            filters1 = dataclass1.GetDataset(datasetname1).GetFilters();
            DatasetNameText.Text = datasetname1;
            DatasetNameText.Enabled = false;
            PrimaryIDText.Text = datatable1.GetPrimaryID();
            PrimaryIDText.Enabled = false;
            dataclass1.FillListBoxFeilds(fields1, FieldsBox);

            if (datatable1.GetProgramCreated() == false)
            {
                MessageBox.Show("Cannot edit this dataset as it is a query.");
                SetDisabled();
            }
            if (dataclass1.isDatasetUsed(DatasetNameText.Text))
            {
                MessageBox.Show("Cannot edit this dataset as it is being used by one of the components in this project.");
                SetDisabled();
            }
            sqldefinition1 = new SQLDefinition(dataclass1, true);
            isquery1 = dataclass1.GetDataset(datasetname1).isQuery();
        }

        public void SetRightJoinTableTemp(string tablename1)
        {
            rightjointable1 = tablename1;
        }

        public TableEditor(FieldEditor fieldeditortemp, TableEditor editortemp,  DataClass dataclasstemp, string datasetname1, string parenttabletemp, string parentfieldtemp)
        {
            InitializeComponent();
            parenttable1 = parenttabletemp;
            parentfield1 = parentfieldtemp;
            //creation1 = creationtemp;
            fieldeditor1 = fieldeditortemp;
            oldeditor1 = editortemp;
            dataclass1 = dataclasstemp;
            editstate = "CreateInternal";
            DatasetNameText.Text = datasetname1;
            datatable1 = new Table(parentfield1, parentfield1, true);
            PrimaryIDText.Text = parentfield1;
            DatasetNameText.Enabled = false;
            PrimaryIDText.Enabled = false;
            fields1 = new ArrayList();
            filters1 = new ArrayList();
            sqldefinition1 = new SQLDefinition(dataclass1, true);
            isquery1 = false;
        }

        public TableEditor(Creation creationtemp, DataClass dataclasstemp)
        {
            InitializeComponent();
            creation1 = creationtemp;
            dataclass1 = dataclasstemp;
            editstate = "Create";
            string tablename1 = "";
            datatable1 = new Table(tablename1, tablename1, true);
            PrimaryIDText.Text = tablename1;
            fields1 = new ArrayList();
            filters1 = new ArrayList();
            sqldefinition1 = new SQLDefinition(dataclass1, true);
            isquery1 = false;
        }

        

        public TableEditor(Creation creationtemp, DataClass dataclasstemp, Table datatabletemp, string datasetnametemp)
        {
            InitializeComponent();
            creation1 = creationtemp;
            dataclass1 = dataclasstemp;
            editstate = "Modify";
            dataset1 = DeepCopyDataset(dataclass1.GetDataset(datasetnametemp));
            datatable1 = DeepCopy(datatabletemp);
            
            //if(datatabletemp.GetProgramCreated)???
            
            DatasetNameText.Enabled = false;
            DatasetNameText.Text = datasetnametemp;
            PrimaryIDText.Text = datatable1.GetPrimaryID();
            PrimaryIDText.Enabled = false;
            fields1 = datatable1.GetFields();
            filters1 = dataclass1.GetDataset(datasetnametemp).GetFilters();
            dataclass1.FillListBoxFeilds(fields1, FieldsBox);

            if (dataclass1.isDatasetUsed(DatasetNameText.Text))
            {
                MessageBox.Show("Cannot edit this dataset in this version as it is being used by one of the forms or pages in this project.");
                SetDisabled();
            }
            else
            {
                if (dataclass1.isDatasetUsedQuery(DatasetNameText.Text) || isProgramCreated(dataclass1.GetDataset(datasetnametemp).GetTable()) == false)
                {
                    MessageBox.Show("Cannot edit this dataset as it is a query.");
                    SetDisabled();
                }
            }

            sqldefinition1 = new SQLDefinition(dataclass1, true);
            isquery1 = dataclass1.GetDataset(datasetnametemp).isQuery();
        }

        public bool isProgramCreated(Table table1)
        {
            if (table1.GetProgramCreated() == true)
            {
                return true;
            }
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Table table2 = (Table)table1.GetFields()[i];
                    return isProgramCreated(table2);  
                }
                catch
                {
                }
            }
            return false;
        }


        public static Table DeepCopy<Table>(Table other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (Table)formatter.Deserialize(ms);
            }
        }

        public static Dataset DeepCopyDataset<Dataset>(Dataset other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (Dataset)formatter.Deserialize(ms);
            }
        }

        public void SetFilters(ArrayList filterstemp)
        {
        }

        public bool InternalTableSet()
        {
            for (int i = 0; i < fields1.Count; i++)
            {
                try
                {
                    Table table1 = (Table)fields1[i];
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }

        public void AddTable(Table table1)
        {
            fields1.Add(table1);
            FieldsBox.Items.Add(table1.GetPrimaryID());
        }

        public void SetDisabled()
        {
            PrimaryIDText.Enabled = false;
            AddFieldButton.Enabled = false;
            RemoveFieldButton.Enabled = false;
            FiltersButton.Enabled = false;
            SaveTableButton.Enabled = false;
        }

        private bool IsSpecialCharacters(string string1)
        {
            char[] chars1 = string1.ToCharArray();

            for (int i = 0; i < chars1.Length; i++)
            {
                if (!Char.IsLetterOrDigit(chars1[i]))
                {
                    return true;
                }
            }
            return false;
        }

        private void SaveTableButton_Click(object sender, EventArgs e)
        {
            if (PrimaryIDText.Text.Contains(' ') || IsSpecialCharacters(PrimaryIDText.Text)  || sqldefinition1.IsSQLKeyword(PrimaryIDText.Text))
            {
                if (PrimaryIDText.Text.Contains(' ') || IsSpecialCharacters(PrimaryIDText.Text))
                {
                    MessageBox.Show("Table names must not have any spaces or special characters");
                }else
                {
                    MessageBox.Show("Table name cannot match an SQL keyword.");
                }
            }
            else
            {
                if (sqldefinition1.TableExist(PrimaryIDText.Text) && editstate != "Modify" && editstate != "ModifyInternal")
                {
                    MessageBox.Show("Table name already exists. Please modify");
                }
                else
                {
                    if (DatasetNameText.Text == "" || DatasetNameText.Text == null || DatasetNameText.Text == " ")
                    {
                        MessageBox.Show("Invalid table name. Please correct");
                    }
                    else
                    {
                        if (editstate == "Create")
                        {
                            datatable1.SetFields(fields1);

                            Dataset dataset2 = new Dataset(DatasetNameText.Text, "NEW");
                            datatable1.SetPrimaryID(PrimaryIDText.Text);
                            dataset2.SetTable(datatable1);
                            dataset2.SetFilters(filters1);
                            creation1.AddDataset(DatasetNameText.Text);

                            try
                            {
                                sqldefinition1.SaveDataset(dataset2, datatable1);
                                dataclass1.AddDataset(dataset2);
                                
                            }
                            catch
                            {
                                MessageBox.Show("Error: couldn't connect to the database. Action cancelled.");
                            }

                            creation1.SaveForDatasetChange();
                            this.Close();
                        }

                        if (editstate == "Modify")
                        {
                            Dataset dataset2 = dataclass1.GetDataset(DatasetNameText.Text);
                            dataset2.SetFilters(filters1);
                            datatable1.SetPrimaryID(PrimaryIDText.Text);


                            try
                            {
                                SaveEditedFields(datatable1, dataset1.GetTable());
                                SaveNewFields(datatable1, dataset1.GetTable());
                                SaveNewTables(datatable1, dataset1.GetTable());
                                //RemoveDroppedFields(datatable1, dataset1.GetTable());


                                dataset2.SetTable(datatable1);
                                
                            }
                            catch
                            {
                                MessageBox.Show("Error: couldn't connect to the database. Action cancelled.");
                            }
                            creation1.SaveForDatasetChange();
                            this.Close();
                        }

                        if (editstate == "CreateInternal")
                        {
                            Table table1 = new Table(PrimaryIDText.Text, PrimaryIDText.Text, true);
                            for (int i = 0; i < fields1.Count; i++)
                            {
                                try
                                {
                                    Field field1 = (Field)fields1[i];
                                    field1.SetParrents(parenttable1, parentfield1);
                                }
                                catch
                                {
                                }
                            }
                            table1.SetFields(fields1);
                            table1.SetPrimaryID(PrimaryIDText.Text);
                            oldeditor1.AddTable(table1);

                            this.Close();
                        }

                        if (editstate == "ModifyInternal")
                        {
                            datatable1.SetFields(fields1);
                            parrenttable1.SetInternalTable(datatable1, PrimaryIDText.Text);
                            this.Close();
                        }
                    }
                }
            }
        }

        public Field GetMatch(Field field1, Table oldtable1)
        {
            for (int i = 0; i < oldtable1.GetFields().Count; i++)
            {
                try
                {
                    Field field2 = (Field)oldtable1.GetFields()[i];
                    if (field1.GetFieldName() == field2.GetFieldName() && field1.GetParentField() == field2.GetParentField())
                    {
                        return field2;
                    }
                }
                catch
                {
                    Table table1 = (Table)oldtable1.GetFields()[i];
                    return GetMatch(field1, table1);
                }
            }
            return null;
        }

        private void SaveNewFields(Table savetable1, Table olddatatable1)
        {
            for (int i = 0; i < savetable1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)savetable1.GetFields()[i];
                    Field field2 = GetMatch(field1, olddatatable1);
                    if (field2 == null)
                    {
                        sqldefinition1.AddColumnSQL(field1, olddatatable1.GetTableName());
                    }
                }
                catch
                {
                    Table newtable1 = (Table)savetable1.GetFields()[i];
                    for(int i2 = 0; i2 < olddatatable1.GetFields().Count; i2++)
                    {
                        try
                        {
                            Table oldtable1 = (Table)olddatatable1.GetFields()[i2];
                            SaveNewFields(newtable1, oldtable1);
                        }
                        catch
                        {

                        }
                        
                    }
                }
            }
        }
        private void SaveEditedFields(Table savetable1, Table olddatatable1)
        {
            for (int i = 0; i < olddatatable1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)olddatatable1.GetFields()[i];
                    Field field2 = GetMatch(field1, savetable1);
                    if (field2 != null)
                    {
                        if(field1.GetFieldType() != field2.GetFieldType() || field2.GetSize() != field1.GetSize())
                        {
                            sqldefinition1.DropFieldSQL(field1, olddatatable1.GetTableName());
                            sqldefinition1.AddColumnSQL(field1, olddatatable1.GetTableName());
                        }
                    }
                    else
                    {
                        sqldefinition1.DropFieldSQL(field1, olddatatable1.GetTableName());
                    }
                }
                catch
                {
                    Table table1 = (Table)olddatatable1.GetFields()[i];
                    for(int i2 = 0; i2 < savetable1.GetFields().Count; i2++)
                    {
                        try
                        {
                            Table nexttable1 = (Table)savetable1.GetFields()[i2];
                            SaveEditedFields(nexttable1, table1);
                        }
                        catch
                        {

                        }
                    }
                    if (isTableInTable(savetable1, table1))
                    {
                        SaveEditedFields(savetable1, table1);
                        
                    }
                    else
                    {       
                        sqldefinition1.DropInternalTableSQL(table1);
                    }
                    
                }
            }
        }

        
        private bool isTableInTable(Table savetable1, Table currenttable1)
        {
            for (int i = 0; i < savetable1.GetFields().Count; i++)
            {
                try
                {
                    Table table1 = (Table)savetable1.GetFields()[i];
                    if (table1.GetTableName() == currenttable1.GetTableName())
                    {
                        return true;
                    }
                    else
                    {
                        isTableInTable(savetable1, table1);
                    }
                }
                catch
                {
                }
            }
            return false;
        }
        
        private void SaveNewTables(Table savetable1, Table oldtable1)
        {
            for(int i = 0; i < savetable1.GetFields().Count; i++)
            {
                try
                {
                    Table table1 = (Table)savetable1.GetFields()[i];
                    if (CheckTableExists(table1, oldtable1) == false)
                    {
                        sqldefinition1.SQLCreate(table1, DatasetNameText.Text, savetable1);
                        sqldefinition1.CreateLinkingTable(oldtable1, table1, savetable1);
                    }
                }catch
                {
                    
                }
            }
        }

        

        private bool CheckTableExists(Table newtable1, Table oldtable1)
        {
            for (int i = 0; i < oldtable1.GetFields().Count; i++)
            {
                try
                {
                    Table table1 = (Table)oldtable1.GetFields()[i];
                    if (table1.GetTableName() == newtable1.GetTableName())
                    {
                        return true;                        
                    }
                }catch
                {
                }
            }
            return false;
        }

        public void DropColumns(Table savetable1, Table oldtable1)
        {
            for (int i = 0; i < savetable1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)oldtable1.GetFields()[i];
                    Field field2 = GetMatch(field1, savetable1);
                    if(field2 == null)
                    {
                        sqldefinition1.DropFieldSQL(field1, oldtable1.GetTableName());
                    }
                }
                catch
                {
                    Table table1 = (Table)oldtable1.GetFields()[i];
                    DropColumns(savetable1, table1);
                }
            }
        }

        private Field GetSameField(Field field1, Table temptable1, string tablename1)
        {
            Field field2 = null;
            for (int i = 0; i < temptable1.GetFields().Count; i++)
            {
                try
                {
                    field2 = (Field)temptable1.GetFields()[i];
                    if (field2.GetFieldName() == field1.GetFieldName() && field2.GetParentField() == field1.GetParentField())
                    {
                        return field2;
                    }
                }
                catch
                {
                    Table table1 = (Table)temptable1.GetFields()[i];
                    return GetSameField(field1, table1, table1.GetTableName());
                }
            }
            return field2;
        }

        private void FiltersButton_Click(object sender, EventArgs e)
        {
            if (editstate != "Modify")
            {
                MessageBox.Show("Cannot set filters on new data. To create a filter on this dataset please create mirror dataset using existing data and set a filter to that new dataset.");
            }
            else
            {
                Dataset dataset1 = dataclass1.GetDataset(DatasetNameText.Text);
                FiltersModifier filtermodifier1 = new FiltersModifier(this, dataset1);
            }
        }

        private void GroupByButton_Click(object sender, EventArgs e)
        {
            if (editstate != "Modify")
            {
                MessageBox.Show("Cannot set groupby setting on new data. To group this dataset please create mirror dataset using existing data and set a groupby setting to that new dataset.");
            }
            else
            {
                GroupBySelection groupbymodifier = new GroupBySelection(datatable1, this);
            }
        }

        private void AddFieldButton_Click(object sender, EventArgs e)
        {
            if (isquery1)
            {
                MessageBox.Show("Cannot add a field to a dataset query in this version but you can chose the fields you want on the forms and pages.");
            }else
            {
                if (editstate == "ModifyInternal")
                {
                    FieldEditor editor1 = new FieldEditor(this, dataclass1, datatable1, DatasetNameText.Text, parrenttable1.GetTableName());
                    editor1.ShowDialog();
                }else
                {
                    FieldEditor editor1 = new FieldEditor(this, dataclass1, datatable1, DatasetNameText.Text, parenttable1);
                    editor1.ShowDialog();
                }
            }
        }

        public void AddField(Field fieldtemp)
        {
            fields1.Add(fieldtemp);
            datatable1.SetFields(fields1);
            FieldsBox.Items.Add(fieldtemp.GetFieldName());
        }

        public void AddFieldInternal(Field fieldtemp, Table internaltable1)
        {
            fields1.Add(fieldtemp);
            internaltable1.SetFields(fields1);
            FieldsBox.Items.Add(fieldtemp.GetFieldName());
        }

        private void RemoveFieldButton_Click(object sender, EventArgs e)
        {
            if (dataclass1.GetDataset(DatasetNameText.Text) == null)
            {
                if (FieldsBox.SelectedItem != null && datatable1.GetField(FieldsBox.SelectedItem.ToString()) != null)
                {
                    fields1.Remove(datatable1.GetField(FieldsBox.SelectedItem.ToString()));
                    FieldsBox.Items.Remove(FieldsBox.SelectedItem);
                }
                else
                {
                    fields1.Remove(datatable1.GetTable(FieldsBox.SelectedItem.ToString()));
                    FieldsBox.Items.Remove(FieldsBox.SelectedItem);
                }
            }else
            {
                if (dataclass1.GetDataset(DatasetNameText.Text).isQuery())
                {
                    MessageBox.Show("Cannot remove field from dataset querry in this version but you can chose the fields you want to display on the forms and pages.");
                }
                else
                {
                    DialogResult result1 = MessageBox.Show("Are you sure you want to delete this field?", "Deletion confirmation", MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        //foreach(Field fieldtemp in fields1)
                        int i = 0;
                        string selecteditem1 = FieldsBox.SelectedItem.ToString();
                        while(i < fields1.Count)
                        {
                            try
                            {
                                Field field1 = (Field)fields1[i];
                                if (field1.GetFieldName() == selecteditem1)
                                {
                                    //datatable1.AddDroppedField(field1);
                                    fields1.RemoveAt(i);
                                }
                            }
                            catch
                            {
                                Table field1 = (Table)fields1[i];
                                if (field1.GetTableName() == selecteditem1)
                                {
                                    fields1.RemoveAt(i);
                                    //FieldsBox.Items.Remove(FieldsBox.SelectedItem);
                                    //dataclass1.CommitedTablesDelete(field1);
                                }
                            }
                            i++;
                        }
                        FieldsBox.Items.Remove(FieldsBox.SelectedItem);
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

        private void FieldsBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void OpenFieldBox()
        {
            if (FieldsBox.SelectedItem != null)
            {
                if (GetTable(FieldsBox.SelectedItem.ToString()) != null)
                {
                    TableEditor editor1 = new TableEditor(this, dataclass1, DatasetNameText.Text, GetTable(FieldsBox.SelectedItem.ToString()), FieldsBox.SelectedItem.ToString(), datatable1);
                    //TableEditor editor1 = new TableEditor(this, dataclass1, DatasetNameText.Text, datatable1, FieldsBox.SelectedItem.ToString(), Table parrenttabletemp);
                    editor1.ShowDialog();
                }
                else
                {
                    if (FieldsBox.SelectedItem != null)
                    {
                        if (editstate == "ModifyInternal")
                        {
                            FieldEditor fieldeditoredit1 = new FieldEditor(this, dataclass1, datatable1, DatasetNameText.Text, datatable1.GetField(FieldsBox.SelectedItem.ToString()), parenttable1);
                            fieldeditoredit1.ShowDialog();
                        } else
                        {
                            FieldEditor fieldeditoredit1 = new FieldEditor(this, dataclass1, datatable1, DatasetNameText.Text, datatable1.GetField(FieldsBox.SelectedItem.ToString()), parenttable1);
                            fieldeditoredit1.ShowDialog();
                        }
                            
                    }
                }
            }
        }

        private void ModifyButton_Click(object sender, EventArgs e)
        {
            OpenFieldBox();
        }

        private Table GetTable(string tablename1)
        {
            for(int i= 0; i< fields1.Count; i++)
            {
                try
                {
                    Table table1 = (Table)fields1[i];
                    if (table1.GetTableName() == tablename1)
                        return table1;
                }
                catch
                {
                }
            }
            return null;
        }

        private void FieldsBox_DoubleClick(object sender, EventArgs e)
        {
            OpenFieldBox();
        }

        private void TableEditor_Load(object sender, EventArgs e)
        {

        }

        public ArrayList GetFeilds()
        {
            return fields1;
        }

        public Field GetField(Field field1, Table table1)
        {
            for (int i = 0; i < datatable1.GetFields().Count; i++)
            {
                try
                {
                    Field field2 = (Field)table1.GetFields()[i];
                    if (field1.GetFieldName() == field2.GetFieldName() && field1.GetParentField() == field2.GetParentField())
                    {
                        return field2;
                    }
                }
                catch
                {
                    Table table2 = (Table)table1.GetFields()[i];
                }
            }
            return null;
        }

        public void AddFieldChange(string tablenameedit1, Field field1, string LabelText, string fieldtype1, string sizetext1, string numbersizetext1)
        {
            Field editfield1 = GetField(field1, datatable1);
            editfield1.SetFeildLabel(LabelText);
            editfield1.SetType(fieldtype1);
            
            if (fieldtype1 == "Number" || fieldtype1 == "Currency" || fieldtype1 == "Decimal")
            {
                string[,] sizes1 = { { "1 Byte", "1" }, { "2 Bytes", "2" }, { "4 Bytes", "4" }, {"5 Bytes", "5"}, { "8 Bytes", "8" } };
                for (int i = 0; i < sizes1.GetLength(0); i++)
                {
                    if (sizetext1 == sizes1[i, 0])
                    {
                        field1.SetSize(UInt64.Parse(sizes1[i, 1]));
                        if(fieldtype1 == "Currency")
                        {
                            editfield1.SetSize(2);
                        }
                    }
                }
            }
            if(fieldtype1 == "Text")
            {
                editfield1.SetSize(UInt64.Parse(sizetext1));
            }
                    
        }

        private void DatasetNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void PrimaryIDText_TextChanged(object sender, EventArgs e)
        {
            datatable1.SetTableName(PrimaryIDText.Text);
        }

        private void DatasetNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
    }
}