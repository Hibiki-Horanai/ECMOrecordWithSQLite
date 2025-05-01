namespace ECMOrecordWithSQLite
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxTotalPt = new System.Windows.Forms.TextBox();
            this.textBoxTotalRecords = new System.Windows.Forms.TextBox();
            this.buttonNewPt = new System.Windows.Forms.Button();
            this.buttonSelectPt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 36F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Menu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "新規患者登録";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "患者選択";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(92, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(447, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "(患者選択後、記録を継続するかレポート出力画面に移動するか選択する)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(272, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "登録患者数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(468, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "症例総数";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 24F);
            this.label7.Location = new System.Drawing.Point(356, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 40);
            this.label7.TabIndex = 0;
            this.label7.Text = "人";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 24F);
            this.label8.Location = new System.Drawing.Point(541, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 40);
            this.label8.TabIndex = 0;
            this.label8.Text = "例";
            // 
            // textBoxTotalPt
            // 
            this.textBoxTotalPt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxTotalPt.Location = new System.Drawing.Point(275, 40);
            this.textBoxTotalPt.Multiline = true;
            this.textBoxTotalPt.Name = "textBoxTotalPt";
            this.textBoxTotalPt.Size = new System.Drawing.Size(79, 58);
            this.textBoxTotalPt.TabIndex = 2;
            // 
            // textBoxTotalRecords
            // 
            this.textBoxTotalRecords.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxTotalRecords.Location = new System.Drawing.Point(460, 40);
            this.textBoxTotalRecords.Multiline = true;
            this.textBoxTotalRecords.Name = "textBoxTotalRecords";
            this.textBoxTotalRecords.Size = new System.Drawing.Size(79, 58);
            this.textBoxTotalRecords.TabIndex = 2;
            // 
            // buttonNewPt
            // 
            this.buttonNewPt.Location = new System.Drawing.Point(42, 139);
            this.buttonNewPt.Name = "buttonNewPt";
            this.buttonNewPt.Size = new System.Drawing.Size(108, 33);
            this.buttonNewPt.TabIndex = 3;
            this.buttonNewPt.Text = "New Patient";
            this.buttonNewPt.UseVisualStyleBackColor = true;
            this.buttonNewPt.Click += new System.EventHandler(this.buttonNewPt_Click);
            // 
            // buttonSelectPt
            // 
            this.buttonSelectPt.Location = new System.Drawing.Point(42, 258);
            this.buttonSelectPt.Name = "buttonSelectPt";
            this.buttonSelectPt.Size = new System.Drawing.Size(140, 33);
            this.buttonSelectPt.TabIndex = 3;
            this.buttonSelectPt.Text = "Select Patient";
            this.buttonSelectPt.UseVisualStyleBackColor = true;
            this.buttonSelectPt.Click += new System.EventHandler(this.buttonSelectPt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonSelectPt);
            this.Controls.Add(this.buttonNewPt);
            this.Controls.Add(this.textBoxTotalRecords);
            this.Controls.Add(this.textBoxTotalPt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "ECMO Driving Record";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxTotalPt;
        private System.Windows.Forms.TextBox textBoxTotalRecords;
        private System.Windows.Forms.Button buttonNewPt;
        private System.Windows.Forms.Button buttonSelectPt;
    }
}

