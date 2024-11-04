using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class PickDatasetType : Form
    {
        private Creation creation1;
        private DataClass dataclass1;
        public PickDatasetType(Creation creationtemp, DataClass dataclasstemp)
        {
            InitializeComponent();
            creation1 = creationtemp;
            dataclass1 = dataclasstemp;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            SQLDefinition definition1 = new SQLDefinition(dataclass1, true);
            if (definition1.isDBConnectivity() == false)
            {
                MessageBox.Show("Database missing or not connected.");
            } else
            {
                if (NewDataOption.Checked)
                {
                    TableEditor table1 = new TableEditor(creation1, dataclass1);
                    table1.ShowDialog();
                    this.Close();
                }

                if (ExistingDataOption.Checked)
                {
                    ExistingData data1 = new ExistingData(creation1, dataclass1);
                    data1.ShowDialog();
                    this.Close();
                }

                if (ExisstingDBTableOptoin.Checked)
                {
                    dbdataset dbdata1 = new dbdataset(creation1, dataclass1);
                    dbdata1.ShowDialog();
                    this.Close();
                }
            }
        }

    }
}
