namespace CodeGenerator
{
    partial class TableEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.DatasetNameText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FieldsBox = new System.Windows.Forms.ListBox();
            this.AddFieldButton = new System.Windows.Forms.Button();
            this.RemoveFieldButton = new System.Windows.Forms.Button();
            this.ModifyButton = new System.Windows.Forms.Button();
            this.SaveTableButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.FiltersButton = new System.Windows.Forms.Button();
            this.PrimaryIDText = new System.Windows.Forms.TextBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dataset Name :";
            // 
            // DatasetNameText
            // 
            this.DatasetNameText.Location = new System.Drawing.Point(157, 19);
            this.DatasetNameText.Name = "DatasetNameText";
            this.DatasetNameText.Size = new System.Drawing.Size(258, 20);
            this.DatasetNameText.TabIndex = 1;
            this.DatasetNameText.TextChanged += new System.EventHandler(this.DatasetNameText_TextChanged);
            this.DatasetNameText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DatasetNameText_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fields";
            // 
            // FieldsBox
            // 
            this.FieldsBox.FormattingEnabled = true;
            this.FieldsBox.Location = new System.Drawing.Point(14, 86);
            this.FieldsBox.Name = "FieldsBox";
            this.FieldsBox.Size = new System.Drawing.Size(400, 147);
            this.FieldsBox.TabIndex = 3;
            this.FieldsBox.SelectedIndexChanged += new System.EventHandler(this.FieldsBox_SelectedIndexChanged);
            this.FieldsBox.DoubleClick += new System.EventHandler(this.FieldsBox_DoubleClick);
            // 
            // AddFieldButton
            // 
            this.AddFieldButton.Location = new System.Drawing.Point(14, 239);
            this.AddFieldButton.Name = "AddFieldButton";
            this.AddFieldButton.Size = new System.Drawing.Size(107, 25);
            this.AddFieldButton.TabIndex = 4;
            this.AddFieldButton.Text = "Add Field...";
            this.AddFieldButton.UseVisualStyleBackColor = true;
            this.AddFieldButton.Click += new System.EventHandler(this.AddFieldButton_Click);
            // 
            // RemoveFieldButton
            // 
            this.RemoveFieldButton.Location = new System.Drawing.Point(227, 239);
            this.RemoveFieldButton.Name = "RemoveFieldButton";
            this.RemoveFieldButton.Size = new System.Drawing.Size(92, 25);
            this.RemoveFieldButton.TabIndex = 6;
            this.RemoveFieldButton.Text = "Remove...";
            this.RemoveFieldButton.UseVisualStyleBackColor = true;
            this.RemoveFieldButton.Click += new System.EventHandler(this.RemoveFieldButton_Click);
            // 
            // ModifyButton
            // 
            this.ModifyButton.Location = new System.Drawing.Point(127, 239);
            this.ModifyButton.Name = "ModifyButton";
            this.ModifyButton.Size = new System.Drawing.Size(94, 25);
            this.ModifyButton.TabIndex = 5;
            this.ModifyButton.Text = "Modify";
            this.ModifyButton.UseVisualStyleBackColor = true;
            this.ModifyButton.Click += new System.EventHandler(this.ModifyButton_Click);
            // 
            // SaveTableButton
            // 
            this.SaveTableButton.Location = new System.Drawing.Point(12, 272);
            this.SaveTableButton.Name = "SaveTableButton";
            this.SaveTableButton.Size = new System.Drawing.Size(109, 25);
            this.SaveTableButton.TabIndex = 9;
            this.SaveTableButton.Text = "Save";
            this.SaveTableButton.UseVisualStyleBackColor = true;
            this.SaveTableButton.Click += new System.EventHandler(this.SaveTableButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Primary ID/Table Name :";
            // 
            // FiltersButton
            // 
            this.FiltersButton.Enabled = false;
            this.FiltersButton.Location = new System.Drawing.Point(227, 272);
            this.FiltersButton.Name = "FiltersButton";
            this.FiltersButton.Size = new System.Drawing.Size(92, 25);
            this.FiltersButton.TabIndex = 7;
            this.FiltersButton.Text = "Filters...";
            this.FiltersButton.UseVisualStyleBackColor = true;
            this.FiltersButton.Visible = false;
            this.FiltersButton.Click += new System.EventHandler(this.FiltersButton_Click);
            // 
            // PrimaryIDText
            // 
            this.PrimaryIDText.Location = new System.Drawing.Point(157, 45);
            this.PrimaryIDText.Name = "PrimaryIDText";
            this.PrimaryIDText.Size = new System.Drawing.Size(257, 20);
            this.PrimaryIDText.TabIndex = 2;
            this.PrimaryIDText.TextChanged += new System.EventHandler(this.PrimaryIDText_TextChanged);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(127, 272);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 25);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // TableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 309);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.PrimaryIDText);
            this.Controls.Add(this.FiltersButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SaveTableButton);
            this.Controls.Add(this.ModifyButton);
            this.Controls.Add(this.RemoveFieldButton);
            this.Controls.Add(this.AddFieldButton);
            this.Controls.Add(this.FieldsBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DatasetNameText);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TableEditor";
            this.Text = "Dataset Editor";
            this.Load += new System.EventHandler(this.TableEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DatasetNameText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox FieldsBox;
        private System.Windows.Forms.Button AddFieldButton;
        private System.Windows.Forms.Button RemoveFieldButton;
        private System.Windows.Forms.Button ModifyButton;
        private System.Windows.Forms.Button SaveTableButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button FiltersButton;
        private System.Windows.Forms.TextBox PrimaryIDText;
        private System.Windows.Forms.Button CancelButton;
    }
}