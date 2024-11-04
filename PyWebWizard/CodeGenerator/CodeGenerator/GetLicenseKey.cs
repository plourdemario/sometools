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
    public partial class GetLicenseKey : Form
    {
        private MainContainer container1;
        public GetLicenseKey(MainContainer containertemp1)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            container1 = containertemp1;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if(LicenseText.Text == "Ta18ksaw-nMw98io-Ay72900s")
            { 
                container1.SetLicenseOk();
                this.Close();
            }else
            {
                MessageBox.Show("You entered an invalid registration key.", "Product Registration");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            container1.CloseMain();
        }
    }
}
