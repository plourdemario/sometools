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
    public partial class FiltersModifier : Form
    {
        private Table datatable1;
        private TableEditor tableeditor1;
        private Dataset dataset1;
        private string runmode1 = "";
        private ExistingData existing1;
        private ArrayList filters1 = new ArrayList();
        public FiltersModifier(TableEditor tableeditortemp, Dataset datasettemp1)
        {
            InitializeComponent();
            dataset1 = datasettemp1;
            tableeditor1 = tableeditortemp;
            filters1 = dataset1.GetFilters();
            for(int i = 0; i < filters1.Count; i++)
            {
                FilterDesc filter1 = (FilterDesc)dataset1.GetFilters()[i];
            	FiltersBox.Items.Add(filter1.GetFilter());
            }
            runmode1 = "tableview";
        }

        public FiltersModifier(ExistingData existingtemp, Dataset datasettemp1)
        {
            InitializeComponent();
            dataset1 = datasettemp1;
            existing1 = existingtemp;
            for (int i = 0; i < dataset1.GetFilters().Count; i++)
            {
                FilterDesc filter1 = (FilterDesc)dataset1.GetFilters()[i];
                FiltersBox.Items.Add(filter1.GetFilter());
            }
            runmode1 = "queryview";
        }
        
        public void AddFilter(string field1, string action1, string criteria1, string filetype1)
        {
        	FilterDesc filter1 = new FilterDesc(field1, action1, criteria1, filetype1);
        	dataset1.GetFilters().Add(filter1);
            FiltersBox.Items.Add("Filter " + dataset1.GetFilters().Count.ToString() + " (" + field1 + " " + action1 + " " + criteria1);
        }
        
        public void SetFilter(FilterDesc filter1, string field1, string action1, string criteria1)
        {
        	filter1.SetValues(field1, action1, criteria1);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            FilterSelection filter1 = new FilterSelection(this, dataset1);
            filter1.ShowDialog();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (runmode1 == "tableview")
            {
                tableeditor1.SetFilters(dataset1.GetFilters());
            }
            else
            {
                existing1.SetFilters(dataset1.GetFilters());
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (FiltersBox.SelectedItem != null)
            {
                for (int i = 0; i < filters1.Count; i++)
                {
                    FilterDesc filter1 = (FilterDesc)dataset1.GetFilters()[i];
                    filters1.Remove(filter1);
                }
                FiltersBox.Items.Remove(FiltersBox.SelectedItem);
            }
        }
    }
}
