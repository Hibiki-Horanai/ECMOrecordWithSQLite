namespace ECMOrecordWithSQLite
{
    partial class FormVolumeIn
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
            this.listBoxInfusion = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxVolume = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxInfusion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxInfusion
            // 
            this.listBoxInfusion.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.listBoxInfusion.FormattingEnabled = true;
            this.listBoxInfusion.ItemHeight = 20;
            this.listBoxInfusion.Items.AddRange(new object[] {
            "ボルベン",
            "ビカネイト",
            "メイロン",
            "RBC",
            "PC",
            "FFP"});
            this.listBoxInfusion.Location = new System.Drawing.Point(12, 38);
            this.listBoxInfusion.Name = "listBoxInfusion";
            this.listBoxInfusion.Size = new System.Drawing.Size(114, 204);
            this.listBoxInfusion.TabIndex = 0;
            this.listBoxInfusion.DoubleClick += new System.EventHandler(this.listBoxInfusion_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(474, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "ml";
            // 
            // textBoxVolume
            // 
            this.textBoxVolume.Font = new System.Drawing.Font("MS UI Gothic", 16F);
            this.textBoxVolume.Location = new System.Drawing.Point(365, 80);
            this.textBoxVolume.Multiline = true;
            this.textBoxVolume.Name = "textBoxVolume";
            this.textBoxVolume.Size = new System.Drawing.Size(103, 56);
            this.textBoxVolume.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(208, 209);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(95, 29);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "入力支援";
            // 
            // textBoxInfusion
            // 
            this.textBoxInfusion.Font = new System.Drawing.Font("MS UI Gothic", 16F);
            this.textBoxInfusion.Location = new System.Drawing.Point(148, 80);
            this.textBoxInfusion.Multiline = true;
            this.textBoxInfusion.Name = "textBoxInfusion";
            this.textBoxInfusion.Size = new System.Drawing.Size(155, 56);
            this.textBoxInfusion.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "輸液名";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(362, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "輸液量";
            // 
            // FormVolumeIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 283);
            this.Controls.Add(this.textBoxInfusion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxVolume);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxInfusion);
            this.Name = "FormVolumeIn";
            this.Text = "FormVolumeIn";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxInfusion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxVolume;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxInfusion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}