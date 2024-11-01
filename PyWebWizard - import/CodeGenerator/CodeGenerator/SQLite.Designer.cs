namespace CodeGenerator
{
    partial class SQLite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLite));
            this.TestButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.RetypeText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PasswordText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.UsernameText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DBOptions = new System.Windows.Forms.GroupBox();
            this.ExistingDBOption = new System.Windows.Forms.RadioButton();
            this.NewDatabaseOption = new System.Windows.Forms.RadioButton();
            this.SQLiteDBPasswordText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.SQLitePathText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DBOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(11, 260);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(107, 25);
            this.TestButton.TabIndex = 8;
            this.TestButton.Text = "Test Connection";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(124, 260);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(94, 25);
            this.OKButton.TabIndex = 9;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(224, 260);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 25);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // RetypeText
            // 
            this.RetypeText.Location = new System.Drawing.Point(179, 65);
            this.RetypeText.Name = "RetypeText";
            this.RetypeText.PasswordChar = '*';
            this.RetypeText.Size = new System.Drawing.Size(177, 20);
            this.RetypeText.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Portal Admin Password  (retype) :";
            // 
            // PasswordText
            // 
            this.PasswordText.Location = new System.Drawing.Point(179, 39);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.PasswordChar = '*';
            this.PasswordText.Size = new System.Drawing.Size(177, 20);
            this.PasswordText.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Portal Admin Password :";
            // 
            // UsernameText
            // 
            this.UsernameText.Location = new System.Drawing.Point(179, 11);
            this.UsernameText.Name = "UsernameText";
            this.UsernameText.Size = new System.Drawing.Size(176, 20);
            this.UsernameText.TabIndex = 1;
            this.UsernameText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UsernameText_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Portal Admin Username :";
            // 
            // DBOptions
            // 
            this.DBOptions.Controls.Add(this.ExistingDBOption);
            this.DBOptions.Controls.Add(this.NewDatabaseOption);
            this.DBOptions.Location = new System.Drawing.Point(15, 99);
            this.DBOptions.Name = "DBOptions";
            this.DBOptions.Size = new System.Drawing.Size(362, 90);
            this.DBOptions.TabIndex = 45;
            this.DBOptions.TabStop = false;
            this.DBOptions.Text = "SQLite Database Options";
            // 
            // ExistingDBOption
            // 
            this.ExistingDBOption.AutoSize = true;
            this.ExistingDBOption.Location = new System.Drawing.Point(14, 52);
            this.ExistingDBOption.Name = "ExistingDBOption";
            this.ExistingDBOption.Size = new System.Drawing.Size(110, 17);
            this.ExistingDBOption.TabIndex = 5;
            this.ExistingDBOption.TabStop = true;
            this.ExistingDBOption.Text = "Existing Database";
            this.ExistingDBOption.UseVisualStyleBackColor = true;
            this.ExistingDBOption.CheckedChanged += new System.EventHandler(this.ExistingDBOption_CheckedChanged);
            // 
            // NewDatabaseOption
            // 
            this.NewDatabaseOption.AutoSize = true;
            this.NewDatabaseOption.Location = new System.Drawing.Point(13, 25);
            this.NewDatabaseOption.Name = "NewDatabaseOption";
            this.NewDatabaseOption.Size = new System.Drawing.Size(96, 17);
            this.NewDatabaseOption.TabIndex = 4;
            this.NewDatabaseOption.TabStop = true;
            this.NewDatabaseOption.Text = "New Database";
            this.NewDatabaseOption.UseVisualStyleBackColor = true;
            this.NewDatabaseOption.CheckedChanged += new System.EventHandler(this.NewDatabaseOption_CheckedChanged);
            // 
            // SQLiteDBPasswordText
            // 
            this.SQLiteDBPasswordText.Location = new System.Drawing.Point(179, 224);
            this.SQLiteDBPasswordText.Name = "SQLiteDBPasswordText";
            this.SQLiteDBPasswordText.PasswordChar = '*';
            this.SQLiteDBPasswordText.Size = new System.Drawing.Size(177, 20);
            this.SQLiteDBPasswordText.TabIndex = 7;
            this.SQLiteDBPasswordText.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 227);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "Database Password :";
            this.label5.Visible = false;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(397, 195);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(94, 25);
            this.BrowseButton.TabIndex = 6;
            this.BrowseButton.Text = "Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // SQLitePathText
            // 
            this.SQLitePathText.Enabled = false;
            this.SQLitePathText.Location = new System.Drawing.Point(179, 198);
            this.SQLitePathText.Name = "SQLitePathText";
            this.SQLitePathText.Size = new System.Drawing.Size(217, 20);
            this.SQLitePathText.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 50;
            this.label4.Text = "File path :";
            // 
            // SQLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 309);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.SQLitePathText);
            this.Controls.Add(this.SQLiteDBPasswordText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DBOptions);
            this.Controls.Add(this.RetypeText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PasswordText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UsernameText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SQLite";
            this.Text = "SQLite/Admin Account Configuration";
            this.DBOptions.ResumeLayout(false);
            this.DBOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.TextBox RetypeText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PasswordText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UsernameText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox DBOptions;
        private System.Windows.Forms.RadioButton ExistingDBOption;
        private System.Windows.Forms.RadioButton NewDatabaseOption;
        private System.Windows.Forms.TextBox SQLiteDBPasswordText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox SQLitePathText;
        private System.Windows.Forms.Label label4;
    }
}