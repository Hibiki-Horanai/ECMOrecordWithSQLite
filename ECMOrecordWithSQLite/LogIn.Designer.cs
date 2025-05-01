namespace ECMOrecordWithSQLite
{
    partial class LogIn
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelinputPW = new System.Windows.Forms.Label();
            this.labelChangePW = new System.Windows.Forms.Label();
            this.labelLogin = new System.Windows.Forms.Label();
            this.textBoxLoginPW = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-5, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(690, 70);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(32, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login...";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.labelinputPW);
            this.panel2.Controls.Add(this.labelChangePW);
            this.panel2.Controls.Add(this.labelLogin);
            this.panel2.Controls.Add(this.textBoxLoginPW);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(-3, 69);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(690, 385);
            this.panel2.TabIndex = 0;
            // 
            // labelinputPW
            // 
            this.labelinputPW.AutoSize = true;
            this.labelinputPW.Font = new System.Drawing.Font("MS UI Gothic", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelinputPW.ForeColor = System.Drawing.Color.Red;
            this.labelinputPW.Location = new System.Drawing.Point(293, 184);
            this.labelinputPW.Name = "labelinputPW";
            this.labelinputPW.Size = new System.Drawing.Size(137, 15);
            this.labelinputPW.TabIndex = 4;
            this.labelinputPW.Text = "パスワードが違います";
            this.labelinputPW.Visible = false;
            // 
            // labelChangePW
            // 
            this.labelChangePW.AutoSize = true;
            this.labelChangePW.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelChangePW.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Italic);
            this.labelChangePW.ForeColor = System.Drawing.Color.Blue;
            this.labelChangePW.Location = new System.Drawing.Point(257, 240);
            this.labelChangePW.Name = "labelChangePW";
            this.labelChangePW.Size = new System.Drawing.Size(202, 20);
            this.labelChangePW.TabIndex = 2;
            this.labelChangePW.Text = "パスワードの変更はこちら";
            this.labelChangePW.Click += new System.EventHandler(this.labelChangePW_Click);
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelLogin.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Italic);
            this.labelLogin.ForeColor = System.Drawing.Color.Blue;
            this.labelLogin.Location = new System.Drawing.Point(472, 152);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(63, 24);
            this.labelLogin.TabIndex = 2;
            this.labelLogin.Text = "Login";
            this.labelLogin.Click += new System.EventHandler(this.labelLogin_Click);
            // 
            // textBoxLoginPW
            // 
            this.textBoxLoginPW.Font = new System.Drawing.Font("MS UI Gothic", 16F);
            this.textBoxLoginPW.Location = new System.Drawing.Point(261, 147);
            this.textBoxLoginPW.Name = "textBoxLoginPW";
            this.textBoxLoginPW.PasswordChar = '*';
            this.textBoxLoginPW.Size = new System.Drawing.Size(205, 34);
            this.textBoxLoginPW.TabIndex = 1;
            this.textBoxLoginPW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxLoginPW_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(105, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(287, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "app Title";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(362, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LogIn
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(682, 453);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LogIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.LogIn_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLoginPW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Label labelinputPW;
        private System.Windows.Forms.Label labelChangePW;
        private System.Windows.Forms.Button button1;
    }
}