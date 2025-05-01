namespace ECMOrecordWithSQLite
{
    partial class FormPatientInfomation
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
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textBoxBirth = new System.Windows.Forms.TextBox();
            this.textBoxWeight = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelBloodType = new System.Windows.Forms.Label();
            this.labelBirth = new System.Windows.Forms.Label();
            this.labelGender = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelIDNO = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxGender = new System.Windows.Forms.TextBox();
            this.textBoxBloodType = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(313, 314);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 27;
            this.buttonUpdate.Text = "更新";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textBoxBirth
            // 
            this.textBoxBirth.Location = new System.Drawing.Point(403, 159);
            this.textBoxBirth.Name = "textBoxBirth";
            this.textBoxBirth.Size = new System.Drawing.Size(100, 22);
            this.textBoxBirth.TabIndex = 25;
            this.textBoxBirth.Tag = "7";
            // 
            // textBoxWeight
            // 
            this.textBoxWeight.Location = new System.Drawing.Point(403, 81);
            this.textBoxWeight.Name = "textBoxWeight";
            this.textBoxWeight.Size = new System.Drawing.Size(100, 22);
            this.textBoxWeight.TabIndex = 20;
            this.textBoxWeight.Tag = "4";
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(403, 42);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(100, 22);
            this.textBoxHeight.TabIndex = 13;
            this.textBoxHeight.Tag = "3";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(403, 3);
            this.textBoxName.Multiline = true;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(194, 33);
            this.textBoxName.TabIndex = 12;
            this.textBoxName.Tag = "2";
            // 
            // labelBloodType
            // 
            this.labelBloodType.AutoSize = true;
            this.labelBloodType.Location = new System.Drawing.Point(3, 195);
            this.labelBloodType.Name = "labelBloodType";
            this.labelBloodType.Size = new System.Drawing.Size(52, 15);
            this.labelBloodType.TabIndex = 18;
            this.labelBloodType.Text = "血液型";
            // 
            // labelBirth
            // 
            this.labelBirth.AutoSize = true;
            this.labelBirth.Location = new System.Drawing.Point(3, 156);
            this.labelBirth.Name = "labelBirth";
            this.labelBirth.Size = new System.Drawing.Size(67, 15);
            this.labelBirth.TabIndex = 17;
            this.labelBirth.Text = "生年月日";
            // 
            // labelGender
            // 
            this.labelGender.AutoSize = true;
            this.labelGender.Location = new System.Drawing.Point(3, 117);
            this.labelGender.Name = "labelGender";
            this.labelGender.Size = new System.Drawing.Size(37, 15);
            this.labelGender.TabIndex = 16;
            this.labelGender.Text = "性別";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(3, 78);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(61, 15);
            this.labelWeight.TabIndex = 15;
            this.labelWeight.Text = "体重[kg]";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(3, 39);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(66, 15);
            this.labelHeight.TabIndex = 14;
            this.labelHeight.Text = "身長[cm]";
            // 
            // labelIDNO
            // 
            this.labelIDNO.AutoSize = true;
            this.labelIDNO.Location = new System.Drawing.Point(144, 269);
            this.labelIDNO.Name = "labelIDNO";
            this.labelIDNO.Size = new System.Drawing.Size(21, 15);
            this.labelIDNO.TabIndex = 19;
            this.labelIDNO.Text = "ID";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelHeight, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelWeight, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelGender, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelBirth, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelBloodType, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.textBoxBirth, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxHeight, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxWeight, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxGender, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxBloodType, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 47);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(801, 237);
            this.tableLayoutPanel1.TabIndex = 28;
            // 
            // textBoxGender
            // 
            this.textBoxGender.Location = new System.Drawing.Point(403, 120);
            this.textBoxGender.Name = "textBoxGender";
            this.textBoxGender.Size = new System.Drawing.Size(100, 22);
            this.textBoxGender.TabIndex = 25;
            this.textBoxGender.Tag = "7";
            // 
            // textBoxBloodType
            // 
            this.textBoxBloodType.Location = new System.Drawing.Point(403, 198);
            this.textBoxBloodType.Name = "textBoxBloodType";
            this.textBoxBloodType.Size = new System.Drawing.Size(100, 22);
            this.textBoxBloodType.TabIndex = 25;
            this.textBoxBloodType.Tag = "7";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(455, 307);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 30);
            this.label1.TabIndex = 29;
            this.label1.Text = "SQLからデータを読み込んできて表示する\r\n編集したデータはボタンで更新する\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "氏名";
            // 
            // FormPatientInfomation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 358);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.labelIDNO);
            this.Name = "FormPatientInfomation";
            this.Text = "PatientInfomation";
            this.Load += new System.EventHandler(this.FormPatientInfomation_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TextBox textBoxBirth;
        private System.Windows.Forms.TextBox textBoxWeight;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelBloodType;
        private System.Windows.Forms.Label labelBirth;
        private System.Windows.Forms.Label labelGender;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelIDNO;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxGender;
        private System.Windows.Forms.TextBox textBoxBloodType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}