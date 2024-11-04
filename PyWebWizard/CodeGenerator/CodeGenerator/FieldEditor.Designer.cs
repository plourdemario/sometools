namespace CodeGenerator
{
    partial class FieldEditor
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
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FieldEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.FieldName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LabelText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TypeBox = new System.Windows.Forms.ComboBox();
            this.SaveFieldButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SizeText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.NumberSize = new System.Windows.Forms.ComboBox();
            this.WarningLabel = new System.Windows.Forms.Label();
            this.PrecisionText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DropDownButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Field Name :";
            // 
            // FieldName
            // 
            this.FieldName.Location = new System.Drawing.Point(88, 11);
            this.FieldName.Name = "FieldName";
            this.FieldName.Size = new System.Drawing.Size(153, 20);
            this.FieldName.TabIndex = 1;
            this.FieldName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FieldName_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Label Text :";
            // 
            // LabelText
            // 
            this.LabelText.Location = new System.Drawing.Point(88, 41);
            this.LabelText.Name = "LabelText";
            this.LabelText.Size = new System.Drawing.Size(194, 20);
            this.LabelText.TabIndex = 2;
            this.LabelText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LabelText_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Data Type :";
            // 
            // TypeBox
            // 
            this.TypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeBox.FormattingEnabled = true;
            this.TypeBox.Location = new System.Drawing.Point(88, 67);
            this.TypeBox.Name = "TypeBox";
            this.TypeBox.Size = new System.Drawing.Size(194, 21);
            this.TypeBox.TabIndex = 3;
            this.TypeBox.SelectedIndexChanged += new System.EventHandler(this.TypeBox_SelectedIndexChanged);
            this.TypeBox.SelectionChangeCommitted += new System.EventHandler(this.TypeBox_SelectionChangeCommitted);
            // 
            // SaveFieldButton
            // 
            this.SaveFieldButton.Location = new System.Drawing.Point(19, 252);
            this.SaveFieldButton.Name = "SaveFieldButton";
            this.SaveFieldButton.Size = new System.Drawing.Size(94, 25);
            this.SaveFieldButton.TabIndex = 7;
            this.SaveFieldButton.Text = "Save";
            this.SaveFieldButton.UseVisualStyleBackColor = true;
            this.SaveFieldButton.Click += new System.EventHandler(this.SaveFieldButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(119, 252);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 25);
            this.CancelButton.TabIndex = 8;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Size :";
            this.label4.Visible = false;
            // 
            // SizeText
            // 
            this.SizeText.Location = new System.Drawing.Point(88, 127);
            this.SizeText.Name = "SizeText";
            this.SizeText.Size = new System.Drawing.Size(53, 20);
            this.SizeText.TabIndex = 4;
            this.SizeText.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Size :";
            this.label5.Visible = false;
            // 
            // NumberSize
            // 
            this.NumberSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NumberSize.FormattingEnabled = true;
            this.NumberSize.Location = new System.Drawing.Point(88, 156);
            this.NumberSize.Name = "NumberSize";
            this.NumberSize.Size = new System.Drawing.Size(194, 21);
            this.NumberSize.TabIndex = 5;
            this.NumberSize.Visible = false;
            // 
            // WarningLabel
            // 
            this.WarningLabel.AutoSize = true;
            this.WarningLabel.Location = new System.Drawing.Point(19, 225);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(319, 13);
            this.WarningLabel.TabIndex = 14;
            this.WarningLabel.Text = "Warning: changing the data type will erase any data in this column";
            this.WarningLabel.Visible = false;
            // 
            // PrecisionText
            // 
            this.PrecisionText.Location = new System.Drawing.Point(88, 186);
            this.PrecisionText.Name = "PrecisionText";
            this.PrecisionText.Size = new System.Drawing.Size(53, 20);
            this.PrecisionText.TabIndex = 6;
            this.PrecisionText.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 189);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Precision :";
            this.label6.Visible = false;
            // 
            // DropDownButton
            // 
            this.DropDownButton.Location = new System.Drawing.Point(88, 94);
            this.DropDownButton.Name = "DropDownButton";
            this.DropDownButton.Size = new System.Drawing.Size(194, 25);
            this.DropDownButton.TabIndex = 16;
            this.DropDownButton.Text = "Set drop down values...";
            this.DropDownButton.UseVisualStyleBackColor = true;
            this.DropDownButton.Visible = false;
            this.DropDownButton.Click += new System.EventHandler(this.DropDownButton_Click);
            // 
            // FieldEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 309);
            this.Controls.Add(this.DropDownButton);
            this.Controls.Add(this.PrecisionText);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.WarningLabel);
            this.Controls.Add(this.NumberSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SizeText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SaveFieldButton);
            this.Controls.Add(this.TypeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LabelText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FieldName);
            this.Controls.Add(this.label1);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FieldEditor";
            this.Text = "FieldEditor";
            this.Load += new System.EventHandler(this.FieldEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FieldName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox LabelText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox TypeBox;
        private System.Windows.Forms.Button SaveFieldButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SizeText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox NumberSize;
        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.TextBox PrecisionText;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button DropDownButton;
    }
}