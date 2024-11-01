namespace CodeGenerator
{
    partial class ExistingData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExistingData));
            this.ExistingDatasets = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DatasetNameText = new System.Windows.Forms.TextBox();
            this.AddedDatasets = new System.Windows.Forms.ListBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.FiltersButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ListJoinButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExistingDatasets
            // 
            this.ExistingDatasets.FormattingEnabled = true;
            this.ExistingDatasets.Location = new System.Drawing.Point(13, 56);
            this.ExistingDatasets.Name = "ExistingDatasets";
            this.ExistingDatasets.Size = new System.Drawing.Size(295, 238);
            this.ExistingDatasets.TabIndex = 0;
            this.ExistingDatasets.SelectedIndexChanged += new System.EventHandler(this.ExistingDatasets_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "New dataset name :";
            // 
            // DatasetNameText
            // 
            this.DatasetNameText.Location = new System.Drawing.Point(124, 7);
            this.DatasetNameText.Name = "DatasetNameText";
            this.DatasetNameText.Size = new System.Drawing.Size(239, 20);
            this.DatasetNameText.TabIndex = 3;
            // 
            // AddedDatasets
            // 
            this.AddedDatasets.FormattingEnabled = true;
            this.AddedDatasets.Location = new System.Drawing.Point(416, 56);
            this.AddedDatasets.Name = "AddedDatasets";
            this.AddedDatasets.Size = new System.Drawing.Size(328, 238);
            this.AddedDatasets.TabIndex = 4;
            this.AddedDatasets.SelectedIndexChanged += new System.EventHandler(this.AddedDatasets_SelectedIndexChanged);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(319, 104);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(78, 51);
            this.AddButton.TabIndex = 5;
            this.AddButton.Text = "< Join By Primary Key>";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "List of datasets to join";
            // 
            // FiltersButton
            // 
            this.FiltersButton.Location = new System.Drawing.Point(215, 317);
            this.FiltersButton.Name = "FiltersButton";
            this.FiltersButton.Size = new System.Drawing.Size(104, 23);
            this.FiltersButton.TabIndex = 8;
            this.FiltersButton.Text = "Filters...";
            this.FiltersButton.UseVisualStyleBackColor = true;
            this.FiltersButton.Visible = false;
            this.FiltersButton.Click += new System.EventHandler(this.FiltersButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(119, 317);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(90, 23);
            this.CancelButton.TabIndex = 12;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 317);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(104, 23);
            this.SaveButton.TabIndex = 11;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "List of datasets";
            // 
            // ListJoinButton
            // 
            this.ListJoinButton.Location = new System.Drawing.Point(622, 300);
            this.ListJoinButton.Name = "ListJoinButton";
            this.ListJoinButton.Size = new System.Drawing.Size(122, 23);
            this.ListJoinButton.TabIndex = 15;
            this.ListJoinButton.Text = "List join datasets";
            this.ListJoinButton.UseVisualStyleBackColor = true;
            this.ListJoinButton.Click += new System.EventHandler(this.ListJoinButton_Click);
            // 
            // ExistingData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 352);
            this.Controls.Add(this.ListJoinButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.FiltersButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.AddedDatasets);
            this.Controls.Add(this.DatasetNameText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ExistingDatasets);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExistingData";
            this.Text = "Create Dataset From Existing Data";
            this.Load += new System.EventHandler(this.ExistingData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ExistingDatasets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DatasetNameText;
        private System.Windows.Forms.ListBox AddedDatasets;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button FiltersButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ListJoinButton;
    }
}