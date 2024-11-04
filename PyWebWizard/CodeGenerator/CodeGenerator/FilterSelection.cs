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
    public partial class FilterSelection : Form
    {
        private FiltersModifier filtermodifier1;
        private FilterDesc filterdesc1;
        private Dataset dataset1;
        private Field selectedfield1;
        private ArrayList fields1 = new ArrayList();
        public FilterSelection(FiltersModifier modifiertemp, FilterDesc filter1, Dataset datasettemp1, Field fieldtemp1)
        {
            InitializeComponent();
            filtermodifier1 = modifiertemp;
            filterdesc1 = filter1;
            dataset1 = datasettemp1;
            FieldSelection.SelectedItem = filterdesc1.GetFieldName();
            selectedfield1 = fieldtemp1;
            BindingSelection.SelectedItem = filterdesc1.GetAction();

            ValueSelection.Text = filterdesc1.GetCriteria();
        }
        
        public FilterSelection(FiltersModifier modifiertemp, Dataset datasettemp1)
        {
            InitializeComponent();
            selectedfield1 = null;
            dataset1 = datasettemp1;
            filtermodifier1 = modifiertemp;
        }

        private void LoadFilters()
        {
            LoadTable(dataset1.GetTable());
        }

        private void LoadTable(Table table1)
        {
            for (int i = 0; i < table1.GetFields().Count; i++)
            {
                try
                {
                    Field field1 = (Field)table1.GetFields()[i];
                    fields1.Add(field1);
                }
                catch
                {
                    Table table2 = (Table)table1.GetFields()[i];
                    LoadTable(table2);
                }
            }
        }


        private void OKButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}
