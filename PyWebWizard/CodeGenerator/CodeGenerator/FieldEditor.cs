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
    public partial class FieldEditor : Form
    {
        private TableEditor tableeditor1;
        private DataClass dataclass1;
        private Table datatable1;
        private string tablename1;
        private string datasetname1;
        private ArrayList fields1;
        private Field field1;
        private string editstate;
        private SQLDefinition sqldefinition1;
        private string previoustable1;
        private ArrayList dropdownlist1 = new ArrayList();

        public FieldEditor(TableEditor tableeditortemp, DataClass dataclasstemp, Table datatabletemp, string datasetnametemp, string previoustabletemp)
        {
            InitializeComponent();
            tableeditor1 = tableeditortemp;
            dataclass1 = dataclasstemp;
            datatable1 = datatabletemp;
            datasetname1 = datasetnametemp;
            previoustable1 = previoustabletemp;
            editstate = "Insert";
            sqldefinition1 = new SQLDefinition(dataclass1, true);
            sqldefinition1.AddProgramTypes(this.TypeBox);
            TypeBox.Items.Add("Internal Table");
        }

        public FieldEditor(TableEditor tableeditortemp, DataClass dataclasstemp, Table datatabletemp, string datasetnametemp, Field fieldtemp, string previoustabletemp)
        {
            InitializeComponent();
            tableeditor1 = tableeditortemp;
            dataclass1 = dataclasstemp;
            datatable1 = datatabletemp;
            datasetname1 = datasetnametemp;
            field1 = fieldtemp;
            dropdownlist1 = field1.GetDropDownValues();
            previoustable1 = previoustabletemp;
            FieldName.Text =field1.GetName();
            LabelText.Text = field1.GetLabel();
            TypeBox.SelectedItem = field1.GetFieldType();
            FieldName.Enabled = false;
            editstate = "Modify";
            if (dataclass1.GetDataset(datasetnametemp) != null)
            {
                if (dataclass1.GetDataset(datasetnametemp).isQuery() || isProgramCreated(dataclass1.GetDataset(datasetnametemp).GetTable()))
                {
                    //MessageBox.Show("To hold data consistency you cannot edit the data type and size of this field. This can only be done from a creating dataset.");
                    label4.Enabled = false;
                    SizeText.Enabled = false;
                    label5.Enabled = false;
                    TypeBox.Enabled = false;
                    NumberSize.Enabled = false;
                    WarningLabel.Visible = false;
                    PrecisionText.Enabled = false;
                }
            }
            field1 = fieldtemp; 
            sqldefinition1 = new SQLDefinition(dataclass1, true);
            sqldefinition1.AddProgramTypes(this.TypeBox);
            for (int i = 0; i < TypeBox.Items.Count; i++)
            {
                if (field1.GetFieldType() == TypeBox.Items[i].ToString())
                {
                    TypeBox.SelectedItem = TypeBox.Items[i];
                }
            }

            if(field1.GetFieldType() == "Text")
            {
                SizeText.Text = field1.GetSize().ToString();
            }

            string [,] intsize1 = {{ "1 Byte", "1" }, {"2 Bytes", "2"}, {"4 Bytes", "4"}, {"8 Bytes", "8"}};
            string[,] floatsize1 = { { "4 Bytes", "4" }, { "8 Bytes", "8" } };
            string[,] currencysize1 = { { "4 Bytes", "4" }, { "8 Bytes", "8" }};
            if(TypeBox.SelectedItem.ToString() == "Number")
            {
                for (int i = 0; i < intsize1.GetLength(0); i++)
                {
                    NumberSize.Visible = true;
                    label5.Visible = true;
                    if (field1.GetSize().ToString() == intsize1[i, 1])
                    {
                        NumberSize.SelectedItem = NumberSize.Items[i];
                    }
                }
            }
            if (TypeBox.SelectedItem.ToString() == "DropDowns")
            {
                DropDownButton.Visible = true;
            }
            if (TypeBox.SelectedItem.ToString() == "Decimal")
            {
                label6.Visible = true;
                PrecisionText.Visible = true;
                SizeText.Visible = true;
                label4.Visible = false;
                SizeText.Visible = false;
                //SizeText.Text = field1.GetSize().ToString();
                for (int i = 0; i < floatsize1.GetLength(0); i++)
                {
                    if (field1.GetSize().ToString() == floatsize1[i, 1])
                    {
                        NumberSize.SelectedItem = NumberSize.Items[i];
                    }
                }
                PrecisionText.Text = field1.GetPrecisionSize().ToString();
            }

            if(TypeBox.SelectedItem.ToString() == "Currency")
            {
                for (int i = 0; i < currencysize1.GetLength(0); i++)
                {
                    NumberSize.Visible = true;
                    label5.Visible = true;
                    NumberSize.Items.Add(currencysize1[i, 0]);
                    if (field1.GetSize().ToString() == currencysize1[i, 1])
                    {
                        NumberSize.SelectedItem = NumberSize.Items[i];
                    }
                }
            }
            TypeBox.Items.Add("Internal Table");
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


        private void TypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeBox.SelectedItem.ToString() == "Text")
            {
                label4.Visible = true;
                SizeText.Visible = true;
                DropDownButton.Visible = false;
            }
            else
            {
                if (TypeBox.SelectedItem.ToString() == "Number" || TypeBox.SelectedItem.ToString() == "Decimal" || TypeBox.SelectedItem.ToString() == "Currency")
                {
                    if (TypeBox.SelectedItem.ToString() == "Decimal")
                    {
                        label4.Visible = false;
                        SizeText.Visible = false;
                        label5.Visible = true;
                        NumberSize.Visible = true;
                        label6.Visible = true;
                        PrecisionText.Visible = true;
                        DropDownButton.Visible = false;
                    }
                    else
                    {

                        label4.Visible = false;
                        SizeText.Visible = false;
                        label5.Visible = true;
                        NumberSize.Visible = true;
                        label6.Visible = false;
                        PrecisionText.Visible = false;
                        DropDownButton.Visible = false;
                    }
                }
                else
                {
                    label5.Visible = false;
                    NumberSize.Visible = false;
                    label4.Visible = false;
                    SizeText.Visible = false;
                    label6.Visible = false;
                    PrecisionText.Visible = false;
                    DropDownButton.Visible = false;
                }

                if(TypeBox.SelectedItem.ToString() == "Number")
                {
                    SetInteger();
                }
                if (TypeBox.SelectedItem.ToString() == "Currency")
                {
                    SetCurrency();
                }
                if (TypeBox.SelectedItem.ToString() == "Decimal")
                {
                    SetDecimal();
                }

                if (TypeBox.SelectedItem.ToString() == "DropDowns")
                {
                    SetDropDown();
                }
            }
        }

        public void SetDropDown()
        {
            label4.Visible = false;
            SizeText.Visible = false;
            DropDownButton.Visible = true;
        }

        public void SetDecimal()
        {
            NumberSize.Items.Clear();
            NumberSize.Items.Add("4 Bytes");
            NumberSize.Items.Add("8 Bytes");
            NumberSize.SelectedItem = "4 Bytes";
        }


        public void SetInteger()
        {
            NumberSize.Items.Clear();
            NumberSize.Items.Add("1 Byte");
            NumberSize.Items.Add("2 Bytes");
            NumberSize.Items.Add("4 Bytes");
            NumberSize.Items.Add("8 Bytes");
            NumberSize.SelectedItem = "4 Bytes";
        }

        public void SetCurrency()
        {
            NumberSize.Items.Clear();
            NumberSize.Items.Add("4 Bytes");
            NumberSize.Items.Add("8 Bytes");
            NumberSize.SelectedItem = "4 Bytes";
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsSpecialCharacters(string string1)
        {
            char[] chars1 = string1.ToCharArray();

            for (int i = 0; i < chars1.Length; i++)
            {
                if (!Char.IsLetterOrDigit(chars1[i]) && chars1[i] != ' ')
                {
                    return true;
                }
            }
            return false;
        }

        private void SaveFieldButton_Click(object sender, EventArgs e)
        {
            if(IsSpecialCharacters(FieldName.Text))
            {
                MessageBox.Show("The field name shouldn't contain any special characters");
            }else
            {
                if (sqldefinition1.IsSQLKeyword(FieldName.Text))
                {
                    MessageBox.Show("Field name cannot match an SQL keyword.");
                }else
                {
                    try
                    {
                        if (sqldefinition1.TableExist(FieldName.Text) && TypeBox.Text == "Internal Table")
                        {
                            MessageBox.Show("Cannot create internal table with the name indicated as that table already exists.");
                        }
                        else
                        {
                            if (tableeditor1.InternalTableSet() && TypeBox.Text == "Internal Table")
                            {
                                MessageBox.Show("Only 1 internal table possible per table possible in this version.");
                            }
                            else
                            {
                                ulong maximum1 = 0;
                                ulong minimum1 = 0;
                                int precision1 = 0;
                                int bytes = 0;
                                int outint1;
                                bool isNumeric = int.TryParse(SizeText.Text, out outint1);
                                if (FieldName.Text == "" || (DoesFieldExist(FieldName.Text) && editstate != "Modify" && FieldName.Enabled == true))
                                {
                                    MessageBox.Show("Please put enter a field name or a field name that wasn't previously chosen.");
                                }
                                else
                                {
                                    if (TypeBox.SelectedItem == null)
                                    {
                                        MessageBox.Show("Please select a type");
                                    }
                                    else
                                    {

                                        if (LabelText.Text == "")
                                        {
                                            MessageBox.Show("Please enter a label");
                                        }
                                        else
                                        {

                                            if (TypeBox.SelectedItem.ToString() == "Internal Table")
                                            {
                                                TableEditor editor1 = new TableEditor(this, tableeditor1, dataclass1, datasetname1, datatable1.GetTableName(), FieldName.Text);
                                                editor1.ShowDialog();
                                            }
                                            if ((SizeText.Text == "" && TypeBox.SelectedItem.ToString() == "Text") || (isNumeric == false && TypeBox.SelectedItem.ToString() == "Text"))
                                            {
                                                MessageBox.Show("Invalid size value");
                                            }
                                            else
                                            {
                                                if (TypeBox.SelectedItem.ToString() != "Internal Table")
                                                {
                                                    if (editstate == "Modify")
                                                    {
                                                        //field1.ModifyValues();
                                                        tableeditor1.AddFieldChange(tablename1, field1, LabelText.Text, TypeBox.SelectedItem.ToString(), SizeText.Text, NumberSize.Text);
                                                    }
                                                    else
                                                    {
                                                        if (TypeBox.SelectedItem.ToString() == "Text")
                                                        {
                                                            if (int.TryParse(SizeText.Text, out outint1))
                                                            {
                                                                maximum1 = UInt64.Parse(SizeText.Text);
                                                                minimum1 = 0;
                                                                precision1 = 0;
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Incorrect value entered in size field. Must be a number.");
                                                            }
                                                        }

                                                        //missing minus
                                                        if (TypeBox.SelectedItem.ToString() == "Number" || TypeBox.SelectedItem.ToString() == "Currency" || TypeBox.SelectedItem.ToString() == "Decimal")
                                                        {
                                                            ulong minimum3 = 0;
                                                            ulong maximum3 = 0;
                                                            string[,] sizes1 = { { "1 Byte", "128" }, { "2 Bytes", "32767" }, { "3 Bytes", "8388607" }, { "4 Bytes", "2147483647" }, { "8 Bytes", "9223372036854775807" } };
                                                            for (int i = 0; i < sizes1.GetLength(0); i++)
                                                            {
                                                                if (NumberSize.SelectedItem.ToString() == sizes1[i, 0])
                                                                {
                                                                    maximum3 = Convert.ToUInt64(sizes1[i, 1]);
                                                                    if (TypeBox.SelectedItem.ToString() == "Currency")
                                                                    {
                                                                        precision1 = 2;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (TypeBox.SelectedItem.ToString() == "Decimal")
                                                                        {
                                                                            precision1 = Int32.Parse(PrecisionText.Text);
                                                                        }
                                                                        else
                                                                        {
                                                                            precision1 = 0;
                                                                        }
                                                                    }
                                                                }
                                                            }


                                                            minimum1 = minimum3 + 1;
                                                            maximum1 = maximum3;
                                                        }

                                                        Field field1 = new Field(FieldName.Text, TypeBox.SelectedItem.ToString(), LabelText.Text, maximum1, minimum1, precision1);
                                                        if (TypeBox.SelectedItem.ToString() == "DropDowns")
                                                        {
                                                            field1.SetDropDownValues(dropdownlist1);
                                                        }

                                                        if (previoustable1 != "" && previoustable1 != null)
                                                        {
                                                            field1.SetParrents(previoustable1, datatable1.GetTableName());
                                                        }
                                                        //marker1
                                                        //Add parents here
                                                        tableeditor1.AddField(field1);
                                                    }
                                                }//not internal table

                                            }
                                        }
                                        this.Close();
                                    }
                                }
                            }
                        }
                    }catch
                    {
                        MessageBox.Show("Error: cannot connect to the database");
                    }
                }
            }
        }

        public bool DoesFieldExist(string fieldname1)
        {
            for (int i = 0; i < tableeditor1.GetFeilds().Count; i++)
            {
                try
                {
                    Field field1 = (Field)tableeditor1.GetFeilds()[i];
                    if (field1.GetFieldName() == fieldname1)
                    {
                        return true;
                    }
                }
                catch
                {
                    Table table1 = (Table)tableeditor1.GetFeilds()[i];
                    if (table1.GetTableName() == fieldname1)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public void SetTableData(string tablenametemp, ArrayList fieldstemp)
        {
            tablename1 = tablenametemp;
            fields1 = fieldstemp;
        }

        private void TypeBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (TypeBox.SelectedItem.ToString() == "Text")
            {
                SizeText.Visible = true;
                label4.Visible = true;
            }
            else
            {
                SizeText.Visible = false;
                label4.Visible = false;
            }
        }

        private void FieldEditor_Load(object sender, EventArgs e)
        {

        }

        private void FieldName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '|' || e.KeyChar == ' ' || e.KeyChar == ';')
            {
                e.Handled = true;
            }
        }

        private void LabelText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '|' || e.KeyChar == ';')
            {
                e.Handled = true;
            }
        }
        

        private void DropDownButton_Click(object sender, EventArgs e)
        {
            DropDownValues values1 = new DropDownValues(dropdownlist1, this);
            values1.Show();
        }

        public void SetDropDownList(ArrayList list1)
        {
            dropdownlist1 = list1;
        }
    }
}
