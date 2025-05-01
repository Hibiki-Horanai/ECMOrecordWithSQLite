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
    public partial class FormVolumeOut : Form
    {

        static int id;
        static int ECMOno;
        public FormVolumeOut(int ecmono, int ptid)
        {
            InitializeComponent();
            //アプリ起動時にフェードインしてくるようなアニメーション設定
            this.Opacity = 0;
            this.Visible = true;
            for (int i = 0; i <= 10000; i++)
            {
                this.Opacity = (double)i / 10000;
            }
            //引数を変数に代入
            id = ptid;
            ECMOno = ecmono;
            listBoxOut.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //データベースに登録してフォームを閉じる
            ClassSQLite db = new ClassSQLite();
            //テーブルが存在しなければ作成
            db.CreateInOut(id,ECMOno);

            //入力された値をデータベースへ登録する
            string name;
            int volume;
            try
            {
                name = textBoxName.Text.ToString();
                volume = int.Parse("-"+textBoxVolume.Text);
                db.InsertInOut(ECMOno, id, name, volume);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.Close();
        }

        private void listBoxOut_DoubleClick(object sender, EventArgs e)
        {
            textBoxName.Text = listBoxOut.SelectedItem.ToString();
        }
    }
}
