using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECMOrecordWithSQLite
{
    public partial class LogIn : Form
    {
        static int logincount = 0;//ログイン失敗回数
        static ClassSQLite db = new ClassSQLite();
        public LogIn()
        {
            InitializeComponent();

            //アプリ起動時にフェードインしてくるようなアニメーション設定
            this.Opacity = 0;
            this.Visible = true;
            for (int i = 0; i <= 10000; i++)
            {
                this.Opacity = (double)i / 10000;
            }

            //パスワードが設定されているか判断
            string temp = db.GetPass();

            //タイトルバーを非表示にする
/*            this.ControlBox = false;
            this.Text = "";
*/        
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            //前回入力したパスワードを記憶しておく?
            //データベースとの接続

            //試しにBrowserコントロールを使ってみる
            //webBrowser1.Navigate(@"C:/Users/hibik/Desktop/HTML/HTML CSS標準入門/lesson3-6/index.html");
        }

        private void labelLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            //passwordを判定して、正しければログイン違えば警告を出すようにする
            labelinputPW.Visible = false;

            //クラスのインスタンス生成
            ClassSQLite myDB = new ClassSQLite();

            //入力されたパスワードを取得
            string inputPass = textBoxLoginPW.Text;
            string password = myDB.GetPass();
            if (password == "") return;

            //取得したパスワードが条件を満たしているか確認
            if (inputPass == "" || inputPass.Length < 4 || inputPass != password)
            {
                logincount++;
                labelinputPW.Text = $"パスワードが違います {logincount}回目";
                labelinputPW.Visible = true;
            }
            else if (inputPass == password)
            {
                //患者管理用の画面に移動
                this.Hide();
                PatientForm patientForm = new PatientForm();
                patientForm.Show();
            }
        }

        private void labelChangePW_Click(object sender, EventArgs e)
        {
            //password変更画面を最前面に固定で表示(passwordChange.cs内に記載)
            PasswordChange passwordChange = new PasswordChange();
            passwordChange.Show();

        }

        private void textBoxLoginPW_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
