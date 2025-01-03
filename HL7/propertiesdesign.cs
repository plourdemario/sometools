using System.Windows.Forms;
using System.Windows;

namespace HL7App
{
	partial class PropertiesForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Button OpenFile = new Button();
		private Label HSMSector = new Label();
		private Label HeaderLabel = new Label();
		private TextBox HeaderText = new TextBox();
		private Label HSMDateLabel = new Label();
		private TextBox HSMDateText = new TextBox();
		private Label UpdateTypeLabel = new Label();
		private TextBox UpdateTypeText = new TextBox();
		private Label EVNSector = new Label();
		private Label EVNDateLabel = new Label();
		private TextBox EVNDateText = new TextBox();
		private Label EVNContactLabel = new Label();
		private TextBox EVNContactText = new TextBox(); 
		private Label PIDSector = new Label();
		private Label PIDIDLabel = new Label();
		private TextBox PIDIDText = new TextBox(); 
		private Label PatientNameLabel = new Label();
		private TextBox PatientNameText  = new TextBox(); 
		private Label PIDAddressLabel = new Label();
		private TextBox PIDAddressText  = new TextBox();
		private Label PIDPhoneLabel = new Label();
		private TextBox PIDPhoneText  = new TextBox();
		private Label LanguageLabel = new Label();
		private TextBox LanguageText  = new TextBox();
		private Label SexLabel = new Label();
		private TextBox SexText = new TextBox();
		private Label PVISector = new Label();
		private Label RoomLabel = new Label();
		private TextBox RoomText = new TextBox();
		private Label AppTypeLabel = new Label();
		private TextBox AppTypeText = new TextBox();
		private Label DoctorNameLabel = new Label();
		private TextBox DoctorNameText = new TextBox();

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
		
		

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormMDI));
            this.SuspendLayout();
			
			// 
            // Open file button
            // 
            OpenFile.Location = new System.Drawing.Point(5,  25);
            OpenFile.Name = "OpenFile";
            OpenFile.Size = new System.Drawing.Size(170, 25);
            OpenFile.TabIndex = 0;
            OpenFile.Text = "Open file...";
            OpenFile.UseVisualStyleBackColor = true;
            OpenFile.Click += this.OpenFile_Click;

			// 
            // HSM section 
            // 
            HSMSector.Location = new System.Drawing.Point(5, 70);
            HSMSector.Name = "HSMSector";
            HSMSector.Size = new System.Drawing.Size(100, 25);
            HSMSector.TabIndex = 1;
            HSMSector.Text = "HSM";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            HeaderLabel.Location = new System.Drawing.Point(5, 100);
            HeaderLabel.Name = "HeaderID";
            HeaderLabel.Size = new System.Drawing.Size(100, 25);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "ID :";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            HeaderText.Location = new System.Drawing.Point(110,  100);
            HeaderText.Name = "HeaderText";
            HeaderText.Size = new System.Drawing.Size(105, 25);
            HeaderText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            HSMDateLabel.Location = new System.Drawing.Point(5, 130);
            HSMDateLabel.Name = "HSMDateLabel";
            HSMDateLabel.Size = new System.Drawing.Size(100, 25);
            HSMDateLabel.TabIndex = 1;
            HSMDateLabel.Text = "Date : ";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            HSMDateText.Location = new System.Drawing.Point(110,  130);
            HSMDateText.Name = "HSMDateText";
            HSMDateText.Size = new System.Drawing.Size(105, 25);
            HSMDateText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;


			// 
            // Header section marker
            // 
            UpdateTypeLabel.Location = new System.Drawing.Point(5, 160);
            UpdateTypeLabel.Name = "UpdateTypeLabel";
            UpdateTypeLabel.Size = new System.Drawing.Size(100, 25);
            UpdateTypeLabel.TabIndex = 1;
            UpdateTypeLabel.Text = "Message Type : ";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            UpdateTypeText.Location = new System.Drawing.Point(110,  160);
            UpdateTypeText.Name = "UpdateTypeText";
            UpdateTypeText.Size = new System.Drawing.Size(105, 25);
            UpdateTypeText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // EVN section 
            // 
            EVNSector.Location = new System.Drawing.Point(5, 190);
            EVNSector.Name = "EVNSector";
            EVNSector.Size = new System.Drawing.Size(100, 25);
            EVNSector.TabIndex = 1;
            EVNSector.Text = "EVN";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            EVNDateLabel.Location = new System.Drawing.Point(5, 230);
            EVNDateLabel.Name = "EVNDateLabel";
            EVNDateLabel.Size = new System.Drawing.Size(100, 25);
            EVNDateLabel.TabIndex = 1;
            EVNDateLabel.Text = "Date : ";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            EVNDateText.Location = new System.Drawing.Point(110,  230);
            EVNDateText.Name = "EVNDateText";
            EVNDateText.Size = new System.Drawing.Size(105, 25);
            EVNDateText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            EVNContactLabel.Location = new System.Drawing.Point(5, 260);
            EVNContactLabel.Name = "EVNContact";
            EVNContactLabel.Size = new System.Drawing.Size(100, 25);
            EVNContactLabel.TabIndex = 1;
            EVNContactLabel.Text = "Contact :";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            EVNContactText.Location = new System.Drawing.Point(110,  260);
            EVNContactText.Name = "EVNContactText";
            EVNContactText.Size = new System.Drawing.Size(105, 25);
            EVNContactText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // HSM section 
            // 
            PIDSector.Location = new System.Drawing.Point(5, 290);
            PIDSector.Name = "PIDSector";
            PIDSector.Size = new System.Drawing.Size(100, 25);
            PIDSector.TabIndex = 1;
            PIDSector.Text = "PID";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            PIDIDLabel.Location = new System.Drawing.Point(5, 320);
            PIDIDLabel.Name = "PID ID Label";
            PIDIDLabel.Size = new System.Drawing.Size(100, 25);
            PIDIDLabel.TabIndex = 1;
            PIDIDLabel.Text = "ID :";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            PIDIDText.Location = new System.Drawing.Point(110,  320);
            PIDIDText.Name = "PID ID Text";
            PIDIDText.Size = new System.Drawing.Size(105, 25);
            PIDIDText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            PatientNameLabel.Location = new System.Drawing.Point(5, 350);
            PatientNameLabel.Name = "PatientNameLabel";
            PatientNameLabel.Size = new System.Drawing.Size(100, 25);
            PatientNameLabel.TabIndex = 1;
            PatientNameLabel.Text = "Patient Name : ";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            PatientNameText.Location = new System.Drawing.Point(110,  350);
            PatientNameText.Name = "PatientNameText";
            PatientNameText.Size = new System.Drawing.Size(105, 25);
            PatientNameText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;


			// 
            // Header section marker
            // 
            PIDAddressLabel.Location = new System.Drawing.Point(5, 380);
            PIDAddressLabel.Name = "PID Address Label";
            PIDAddressLabel.Size = new System.Drawing.Size(100, 25);
            PIDAddressLabel.TabIndex = 1;
            PIDAddressLabel.Text = "Address :";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            PIDAddressText.Location = new System.Drawing.Point(110,  380);
            PIDAddressText.Name = "PID Address Text";
            PIDAddressText.Size = new System.Drawing.Size(105, 25);
            PIDAddressText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            PIDPhoneLabel.Location = new System.Drawing.Point(5, 410);
            PIDPhoneLabel.Name = "PIDPhoneLabel";
            PIDPhoneLabel.Size = new System.Drawing.Size(100, 25);
            PIDPhoneLabel.TabIndex = 1;
            PIDPhoneLabel.Text = "Phone Number : ";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            PIDPhoneText.Location = new System.Drawing.Point(110,  410);
            PIDPhoneText.Name = "PIDPhoneText";
            PIDPhoneText.Size = new System.Drawing.Size(105, 25);
            PIDPhoneText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            LanguageLabel.Location = new System.Drawing.Point(5, 440);
            LanguageLabel.Name = "LanguageLabel";
            LanguageLabel.Size = new System.Drawing.Size(100, 25);
            LanguageLabel.TabIndex = 1;
            LanguageLabel.Text = "Language :";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            LanguageText.Location = new System.Drawing.Point(110,  440);
            LanguageText.Name = "LanguageText";
            LanguageText.Size = new System.Drawing.Size(105, 25);
            LanguageText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            SexLabel.Location = new System.Drawing.Point(5, 470);
            SexLabel.Name = "SexLabel";
            SexLabel.Size = new System.Drawing.Size(100, 25);
            SexLabel.TabIndex = 1;
            SexLabel.Text = "Sex : ";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            SexText.Location = new System.Drawing.Point(110,  470);
            SexText.Name = "SexText";
            SexText.Size = new System.Drawing.Size(105, 25);
            SexText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // HSM section 
            // 
            PVISector.Location = new System.Drawing.Point(5, 500);
            PVISector.Name = "PVISector";
            PVISector.Size = new System.Drawing.Size(100, 25);
            PVISector.TabIndex = 1;
            PVISector.Text = "PVI";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            RoomLabel.Location = new System.Drawing.Point(5, 530);
            RoomLabel.Name = "RoomLabel";
            RoomLabel.Size = new System.Drawing.Size(100, 25);
            RoomLabel.TabIndex = 1;
            RoomLabel.Text = "Room :";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            RoomText.Location = new System.Drawing.Point(110,  530);
            RoomText.Name = "RoomText";
            RoomText.Size = new System.Drawing.Size(105, 25);
            RoomText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            AppTypeLabel.Location = new System.Drawing.Point(5, 560);
            AppTypeLabel.Name = "AppTypeLabel";
            AppTypeLabel.Size = new System.Drawing.Size(100, 25);
            AppTypeLabel.TabIndex = 1;
            AppTypeLabel.Text = "Appointment type : ";
			//HeaderLabel.Click += this.OpenFile_Click;

			// 
            // Header section marker
            // 
            AppTypeText.Location = new System.Drawing.Point(110,  560);
            AppTypeText.Name = "AppTypeText";
            AppTypeText.Size = new System.Drawing.Size(105, 25);
            AppTypeText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;


			// 
            // Header section marker
            // 
            DoctorNameLabel.Location = new System.Drawing.Point(5, 590);
            DoctorNameLabel.Name = "DoctorNameLabel";
            DoctorNameLabel.Size = new System.Drawing.Size(100, 25);
            DoctorNameLabel.TabIndex = 1;
            DoctorNameLabel.Text = "Doctor :";
			//HeaderLabel.Click += this.OpenFile_Click;
			// 
            // Header section marker
            // 
            DoctorNameText.Location = new System.Drawing.Point(110,  590);
            DoctorNameText.Name = "DoctorNameText";
            DoctorNameText.Size = new System.Drawing.Size(105, 25);
            DoctorNameText.TabIndex = 1;
            //HeaderLabel.Click += this.OpenFile_Click;





            // 
            // Designer window
            // 
			this.Left = 800;
			this.Top = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 700);
			Controls.Add(OpenFile);
			Controls.Add(HSMSector);
			Controls.Add(HeaderLabel);
			Controls.Add(HeaderText);
			Controls.Add(HSMDateLabel);
			Controls.Add(HSMDateText);
			Controls.Add(HSMDateText);
			Controls.Add(UpdateTypeLabel);
			Controls.Add(UpdateTypeText);
			Controls.Add(EVNSector);
			Controls.Add(EVNDateLabel);
			Controls.Add(EVNDateText);
			Controls.Add(EVNContactLabel);
			Controls.Add(EVNContactText);
			Controls.Add(PIDSector);	
			Controls.Add(PIDIDLabel);
			Controls.Add(PIDIDText);
			Controls.Add(PatientNameLabel);
			Controls.Add(PatientNameText);
			Controls.Add(PIDAddressLabel);
			Controls.Add(PIDAddressText);
			Controls.Add(PIDPhoneLabel);
			Controls.Add(PIDPhoneText);
			Controls.Add(LanguageLabel);
			Controls.Add(LanguageText);
			Controls.Add(SexLabel);
			Controls.Add(SexText);
			Controls.Add(PVISector);
			Controls.Add(RoomLabel);
			Controls.Add(RoomText);
			Controls.Add(AppTypeLabel);
			Controls.Add(AppTypeText);
			Controls.Add(DoctorNameLabel);
			Controls.Add(DoctorNameText);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            
            this.Name = "HL7Main";
            this.Text = "HL7 Application";
            this.ResumeLayout(false);
            this.PerformLayout();
		}

		#endregion
	}
}
