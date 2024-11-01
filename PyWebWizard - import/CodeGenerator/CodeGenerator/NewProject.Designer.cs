namespace CodeGenerator
{
    partial class NewProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProject));
            this.label1 = new System.Windows.Forms.Label();
            this.WebSiteNameText = new System.Windows.Forms.TextBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.CreateProjectButton = new System.Windows.Forms.Button();
            this.HeaderText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FooterText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DBLoginOption = new System.Windows.Forms.RadioButton();
            this.HardCodedOption = new System.Windows.Forms.RadioButton();
            this.NoLoginOption = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SQLiteOption = new System.Windows.Forms.RadioButton();
            this.MySQLOption = new System.Windows.Forms.RadioButton();
            this.MSSQLOption = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Web Site Name :";
            // 
            // WebSiteNameText
            // 
            this.WebSiteNameText.Location = new System.Drawing.Point(125, 10);
            this.WebSiteNameText.Name = "WebSiteNameText";
            this.WebSiteNameText.Size = new System.Drawing.Size(215, 20);
            this.WebSiteNameText.TabIndex = 1;
            this.WebSiteNameText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WebSiteNameText_KeyPress);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(250, 358);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 25);
            this.CancelButton.TabIndex = 18;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // CreateProjectButton
            // 
            this.CreateProjectButton.Location = new System.Drawing.Point(127, 358);
            this.CreateProjectButton.Name = "CreateProjectButton";
            this.CreateProjectButton.Size = new System.Drawing.Size(107, 25);
            this.CreateProjectButton.TabIndex = 17;
            this.CreateProjectButton.Text = "Create";
            this.CreateProjectButton.UseVisualStyleBackColor = true;
            this.CreateProjectButton.Click += new System.EventHandler(this.CreateProjectButton_Click);
            // 
            // HeaderText
            // 
            this.HeaderText.Location = new System.Drawing.Point(125, 37);
            this.HeaderText.Name = "HeaderText";
            this.HeaderText.Size = new System.Drawing.Size(215, 20);
            this.HeaderText.TabIndex = 2;
            this.HeaderText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HeaderText_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Header Title : ";
            // 
            // FooterText
            // 
            this.FooterText.Location = new System.Drawing.Point(125, 65);
            this.FooterText.Name = "FooterText";
            this.FooterText.Size = new System.Drawing.Size(215, 20);
            this.FooterText.TabIndex = 3;
            this.FooterText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FooterText_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Footer Text : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DBLoginOption);
            this.groupBox1.Controls.Add(this.HardCodedOption);
            this.groupBox1.Controls.Add(this.NoLoginOption);
            this.groupBox1.Location = new System.Drawing.Point(12, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 102);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Authentication Method";
            // 
            // DBLoginOption
            // 
            this.DBLoginOption.AutoSize = true;
            this.DBLoginOption.Location = new System.Drawing.Point(13, 68);
            this.DBLoginOption.Name = "DBLoginOption";
            this.DBLoginOption.Size = new System.Drawing.Size(100, 17);
            this.DBLoginOption.TabIndex = 6;
            this.DBLoginOption.TabStop = true;
            this.DBLoginOption.Text = "Database Login";
            this.DBLoginOption.UseVisualStyleBackColor = true;
            // 
            // HardCodedOption
            // 
            this.HardCodedOption.AutoSize = true;
            this.HardCodedOption.Location = new System.Drawing.Point(13, 45);
            this.HardCodedOption.Name = "HardCodedOption";
            this.HardCodedOption.Size = new System.Drawing.Size(141, 17);
            this.HardCodedOption.TabIndex = 5;
            this.HardCodedOption.TabStop = true;
            this.HardCodedOption.Text = "Hard Coded (admin only)";
            this.HardCodedOption.UseVisualStyleBackColor = true;
            // 
            // NoLoginOption
            // 
            this.NoLoginOption.AutoSize = true;
            this.NoLoginOption.Location = new System.Drawing.Point(13, 22);
            this.NoLoginOption.Name = "NoLoginOption";
            this.NoLoginOption.Size = new System.Drawing.Size(68, 17);
            this.NoLoginOption.TabIndex = 4;
            this.NoLoginOption.TabStop = true;
            this.NoLoginOption.Text = "No Login";
            this.NoLoginOption.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SQLiteOption);
            this.groupBox2.Controls.Add(this.MySQLOption);
            this.groupBox2.Controls.Add(this.MSSQLOption);
            this.groupBox2.Location = new System.Drawing.Point(12, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 97);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SQL Server";
            // 
            // SQLiteOption
            // 
            this.SQLiteOption.AutoSize = true;
            this.SQLiteOption.Location = new System.Drawing.Point(14, 67);
            this.SQLiteOption.Name = "SQLiteOption";
            this.SQLiteOption.Size = new System.Drawing.Size(106, 17);
            this.SQLiteOption.TabIndex = 2;
            this.SQLiteOption.TabStop = true;
            this.SQLiteOption.Text = "SQLite Database";
            this.SQLiteOption.UseVisualStyleBackColor = true;
            // 
            // MySQLOption
            // 
            this.MySQLOption.AutoSize = true;
            this.MySQLOption.Location = new System.Drawing.Point(14, 44);
            this.MySQLOption.Name = "MySQLOption";
            this.MySQLOption.Size = new System.Drawing.Size(94, 17);
            this.MySQLOption.TabIndex = 1;
            this.MySQLOption.TabStop = true;
            this.MySQLOption.Text = "MySQL Server";
            this.MySQLOption.UseVisualStyleBackColor = true;
            this.MySQLOption.CheckedChanged += new System.EventHandler(this.MySQLOption_CheckedChanged);
            // 
            // MSSQLOption
            // 
            this.MSSQLOption.AutoSize = true;
            this.MSSQLOption.Location = new System.Drawing.Point(14, 21);
            this.MSSQLOption.Name = "MSSQLOption";
            this.MSSQLOption.Size = new System.Drawing.Size(126, 17);
            this.MSSQLOption.TabIndex = 0;
            this.MSSQLOption.TabStop = true;
            this.MSSQLOption.Text = "Microsoft SQL Server";
            this.MSSQLOption.UseVisualStyleBackColor = true;
            this.MSSQLOption.CheckedChanged += new System.EventHandler(this.MSSQLOption_CheckedChanged);
            // 
            // NewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 389);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.FooterText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.HeaderText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.CreateProjectButton);
            this.Controls.Add(this.WebSiteNameText);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewProject";
            this.Text = "New Project";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox WebSiteNameText;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button CreateProjectButton;
        private System.Windows.Forms.TextBox HeaderText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FooterText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton NoLoginOption;
        private System.Windows.Forms.RadioButton HardCodedOption;
        private System.Windows.Forms.RadioButton DBLoginOption;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton MySQLOption;
        private System.Windows.Forms.RadioButton MSSQLOption;
        private System.Windows.Forms.RadioButton SQLiteOption;
    }
}