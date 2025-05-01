using System.Windows.Forms;
using System;

namespace ECMOrecordWithSQLite
{
    public partial class PasswordChange : Form
    {
        public PasswordChange()
        {
            InitializeComponent();

            //アプリ起動時にフェードインしてくるようなアニメーション設定
            this.Opacity = 0;
            this.Visible = true;
            for (int i = 0; i <= 3000; i++)
            {
                this.Opacity = (double)i / 3000;
            }
            this.TopMost = true;
        }

        const int CS_DROPSHADOW = 0x00020000;
        protected override CreateParams CreateParams
        {
            get { 
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
          


        private void button1_Click(object sender, System.EventArgs e)
        {
            //password変更に成功したら現在のFormを閉じて「login」に戻る
            //SQLiteクラスのインスタンス作成
            ClassSQLite db = new ClassSQLite();

            //現在のパスワードの取得
            string passwordNow = db.GetPass();
            string inputOldPW = textBoxOldPW.Text;
            //もし入力されたパスワードと取得したパスワードが異なる場合はテキストを変更する
            if(passwordNow != inputOldPW)
            {
                labelOldPWAlert.Visible = true;
                return;
            }
            else
            {
                labelOldPWAlert.Visible = false;
            }

            //新規入力パスワードの入力が同一かどうか判断
            if(textBoxNewPW1.Text != textBoxNewPW2.Text)
            {
                //同一でない場合
                labelNewPWAlert.Visible = true;
                labelNewPWAlert.Text = "入力されたパスワードが一致しません。";
                return;
            }else if (textBoxNewPW1.Text.Length < 4 || textBoxNewPW2.Text.Length < 4)
            {
                //文字数が足りない場合
                labelNewPWAlert.Visible = true;
                labelNewPWAlert.Text = "4文字以上で入力してください。";
            }
            else
            {
                labelNewPWAlert.Visible = false;
                //パスワードの更新
                DateTime dt = DateTime.Now;
                db.InsertPassWord(textBoxNewPW1.Text,dt.ToString());
                MessageBox.Show("更新されました");
                this.Close();
            }

        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();

        }
    }
}
