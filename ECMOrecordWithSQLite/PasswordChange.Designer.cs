namespace ECMOrecordWithSQLite
{
    partial class PasswordChange
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
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelNewPWAlert = new System.Windows.Forms.Label();
            this.labelOldPWAlert = new System.Windows.Forms.Label();
            this.buttonConf = new System.Windows.Forms.Button();
            this.textBoxNewPW2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxNewPW1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxOldPW = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(287, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "app Title";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.Controls.Add(this.labelNewPWAlert);
            this.panel2.Controls.Add(this.labelOldPWAlert);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonConf);
            this.panel2.Controls.Add(this.textBoxNewPW2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.textBoxNewPW1);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.textBoxOldPW);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(-1, -1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(686, 397);
            this.panel2.TabIndex = 2;
            // 
            // labelNewPWAlert
            // 
            this.labelNewPWAlert.AutoSize = true;
            this.labelNewPWAlert.Font = new System.Drawing.Font("MS UI Gothic", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelNewPWAlert.ForeColor = System.Drawing.Color.Red;
            this.labelNewPWAlert.Location = new System.Drawing.Point(261, 284);
            this.labelNewPWAlert.Name = "labelNewPWAlert";
            this.labelNewPWAlert.Size = new System.Drawing.Size(246, 15);
            this.labelNewPWAlert.TabIndex = 3;
            this.labelNewPWAlert.Text = "入力されたパスワードが一致しません。";
            this.labelNewPWAlert.Visible = false;
            // 
            // labelOldPWAlert
            // 
            this.labelOldPWAlert.AutoSize = true;
            this.labelOldPWAlert.Font = new System.Drawing.Font("MS UI Gothic", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelOldPWAlert.ForeColor = System.Drawing.Color.Red;
            this.labelOldPWAlert.Location = new System.Drawing.Point(261, 153);
            this.labelOldPWAlert.Name = "labelOldPWAlert";
            this.labelOldPWAlert.Size = new System.Drawing.Size(249, 15);
            this.labelOldPWAlert.TabIndex = 3;
            this.labelOldPWAlert.Text = "もう一度パスワードを確認してください。";
            this.labelOldPWAlert.Visible = false;
            // 
            // buttonConf
            // 
            this.buttonConf.Location = new System.Drawing.Point(391, 326);
            this.buttonConf.Name = "buttonConf";
            this.buttonConf.Size = new System.Drawing.Size(75, 34);
            this.buttonConf.TabIndex = 4;
            this.buttonConf.Text = "OK";
            this.buttonConf.UseVisualStyleBackColor = true;
            this.buttonConf.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxNewPW2
            // 
            this.textBoxNewPW2.Font = new System.Drawing.Font("MS UI Gothic", 16F);
            this.textBoxNewPW2.Location = new System.Drawing.Point(261, 247);
            this.textBoxNewPW2.Name = "textBoxNewPW2";
            this.textBoxNewPW2.PasswordChar = '*';
            this.textBoxNewPW2.Size = new System.Drawing.Size(205, 34);
            this.textBoxNewPW2.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(89, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "reEnterPW:";
            // 
            // textBoxNewPW1
            // 
            this.textBoxNewPW1.Font = new System.Drawing.Font("MS UI Gothic", 16F);
            this.textBoxNewPW1.Location = new System.Drawing.Point(261, 195);
            this.textBoxNewPW1.Name = "textBoxNewPW1";
            this.textBoxNewPW1.PasswordChar = '*';
            this.textBoxNewPW1.Size = new System.Drawing.Size(205, 34);
            this.textBoxNewPW1.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(120, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "New PW:";
            // 
            // textBoxOldPW
            // 
            this.textBoxOldPW.Font = new System.Drawing.Font("MS UI Gothic", 16F);
            this.textBoxOldPW.Location = new System.Drawing.Point(261, 112);
            this.textBoxOldPW.Name = "textBoxOldPW";
            this.textBoxOldPW.PasswordChar = '*';
            this.textBoxOldPW.Size = new System.Drawing.Size(205, 34);
            this.textBoxOldPW.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(131, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "Old PW:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(264, 326);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 34);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // PasswordChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 396);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PasswordChange";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password Change";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxOldPW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelNewPWAlert;
        private System.Windows.Forms.Label labelOldPWAlert;
        private System.Windows.Forms.Button buttonConf;
        private System.Windows.Forms.TextBox textBoxNewPW2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxNewPW1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonCancel;
    }
}