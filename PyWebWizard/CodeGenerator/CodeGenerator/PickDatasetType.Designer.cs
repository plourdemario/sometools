namespace CodeGenerator
{
    partial class PickDatasetType
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
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickDatasetType));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ExisstingDBTableOptoin = new System.Windows.Forms.RadioButton();
            this.ExistingDataOption = new System.Windows.Forms.RadioButton();
            this.NewDataOption = new System.Windows.Forms.RadioButton();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ExisstingDBTableOptoin);
            this.groupBox1.Controls.Add(this.ExistingDataOption);
            this.groupBox1.Controls.Add(this.NewDataOption);
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 111);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dataset type";
            // 
            // ExisstingDBTableOptoin
            // 
            this.ExisstingDBTableOptoin.AutoSize = true;
            this.ExisstingDBTableOptoin.Location = new System.Drawing.Point(13, 69);
            this.ExisstingDBTableOptoin.Name = "ExisstingDBTableOptoin";
            this.ExisstingDBTableOptoin.Size = new System.Drawing.Size(137, 17);
            this.ExisstingDBTableOptoin.TabIndex = 2;
            this.ExisstingDBTableOptoin.TabStop = true;
            this.ExisstingDBTableOptoin.Text = "Exising Database Table";
            this.ExisstingDBTableOptoin.UseVisualStyleBackColor = true;
            this.ExisstingDBTableOptoin.Visible = false;
            // 
            // ExistingDataOption
            // 
            this.ExistingDataOption.AutoSize = true;
            this.ExistingDataOption.Location = new System.Drawing.Point(13, 46);
            this.ExistingDataOption.Name = "ExistingDataOption";
            this.ExistingDataOption.Size = new System.Drawing.Size(106, 17);
            this.ExistingDataOption.TabIndex = 1;
            this.ExistingDataOption.TabStop = true;
            this.ExistingDataOption.Text = "Exisiting datasets";
            this.ExistingDataOption.UseVisualStyleBackColor = true;
            // 
            // NewDataOption
            // 
            this.NewDataOption.AutoSize = true;
            this.NewDataOption.Location = new System.Drawing.Point(13, 23);
            this.NewDataOption.Name = "NewDataOption";
            this.NewDataOption.Size = new System.Drawing.Size(71, 17);
            this.NewDataOption.TabIndex = 0;
            this.NewDataOption.TabStop = true;
            this.NewDataOption.Text = "New data";
            this.NewDataOption.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(109, 127);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(81, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "Ok";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(197, 127);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(81, 23);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // PickDatasetType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 162);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.groupBox1);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PickDatasetType";
            this.Text = "Using new or existing data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton ExistingDataOption;
        private System.Windows.Forms.RadioButton NewDataOption;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.RadioButton ExisstingDBTableOptoin;
    }
}