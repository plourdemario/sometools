using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.IO;
using System.Collections;
using System.Resources;

namespace HL7App
{
	public partial class PropertiesForm : Form
	{
		public PropertiesForm()
		{
			InitializeComponent();
		}

		private void OpenFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog file1 = new OpenFileDialog();

            file1.Filter = "HL7 Message File (*.*)|*.*";
            file1.FilterIndex = 2;
            file1.RestoreDirectory = true;

			string[] filecontents = new string[5];
            if (file1.ShowDialog() == DialogResult.OK)
            {
                //OpenFileOldFormat(file1.FileName);
				int i = 0;
                using (StreamReader reader = new StreamReader(file1.FileName))
				{
					string line;
					// Read line by line
					while ((line = reader.ReadLine()) != null)
					{
						filecontents[i] = line;
						i++;
					}
				}	

				for(int i2 = 0; i2 < filecontents.Length; i2++)
				{
					string[] split1 = filecontents[i2].Split('|');
					if(split1[0] == "HSM")
					{
						HeaderText.Text = split1[1];
						HSMDateText.Text = split1[6];
						UpdateTypeText.Text = split1[8];
					}
					if(split1[0] == "EVN")
					{
						EVNDateText.Text = split1[2];
						EVNContactText.Text = split1[6];
					}
					if(split1[0] == "PID")
					{
						PIDIDText.Text = split1[3];
						PatientNameText.Text = split1[4];
						PIDAddressText.Text = split1[12];
						PIDPhoneText.Text = split1[14];
						LanguageText.Text = split1[16];
						SexText.Text = split1[17];
					}

					if(split1[0] == "PID")
					{
						PIDIDText.Text = split1[3];
						PatientNameText.Text = split1[4];
						PIDAddressText.Text = split1[12];
						PIDPhoneText.Text = split1[14];
						LanguageText.Text = split1[16];
						SexText.Text = split1[17];
					}

					if(split1[0] == "PVI")
					{
						RoomText.Text = split1[3];
						AppTypeText.Text = split1[4];
						DoctorNameText.Text = split1[8];
					}
				}
				/*BinaryReader reader1 = new BinaryReader(File.Open(file1.FileName, FileMode.Open));
                if (reader1  != null)
				{
					MessageBox.Show("test successful");
				}*/
            }

		}
	}
}
