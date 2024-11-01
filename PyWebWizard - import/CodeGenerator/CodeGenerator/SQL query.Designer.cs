namespace CodeGenerator
{
    partial class SQL_query
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQL_query));
            this.TestButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.CreateProjectButton = new System.Windows.Forms.Button();
            this.DatasetSQLText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DSNameText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(12, 136);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(107, 25);
            this.TestButton.TabIndex = 29;
            this.TestButton.Text = "List of DB Tables";
            this.TestButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(528, 136);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 25);
            this.CancelButton.TabIndex = 28;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // CreateProjectButton
            // 
            this.CreateProjectButton.Location = new System.Drawing.Point(405, 136);
            this.CreateProjectButton.Name = "CreateProjectButton";
            this.CreateProjectButton.Size = new System.Drawing.Size(107, 25);
            this.CreateProjectButton.TabIndex = 27;
            this.CreateProjectButton.Text = "Create";
            this.CreateProjectButton.UseVisualStyleBackColor = true;
            // 
            // DatasetSQLText
            // 
            this.DatasetSQLText.Location = new System.Drawing.Point(12, 24);
            this.DatasetSQLText.Name = "DatasetSQLText";
            this.DatasetSQLText.Size = new System.Drawing.Size(610, 20);
            this.DatasetSQLText.TabIndex = 30;
            this.DatasetSQLText.Text = "SELECT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Enter SELECT Query below";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Dataset name :";
            // 
            // DSNameText
            // 
            this.DSNameText.Location = new System.Drawing.Point(12, 73);
            this.DSNameText.Name = "DSNameText";
            this.DSNameText.Size = new System.Drawing.Size(139, 20);
            this.DSNameText.TabIndex = 33;
            // 
            // SQL_query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 173);
            this.Controls.Add(this.DSNameText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DatasetSQLText);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.CreateProjectButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SQL_query";
            this.Text = "SQL Query Entry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button CreateProjectButton;
        private System.Windows.Forms.TextBox DatasetSQLText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DSNameText;

    }
}