using ScottPlot.Drawing.Colormaps;
using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECMOrecordWithSQLite
{
    public partial class FormInjection : Form
    {
        static int id;
        static int ECMOno;

        public FormInjection(int ecmono, int ptid)
        {
            InitializeComponent();
            //アプリ起動時にフェードインしてくるようなアニメーション設定
            this.Opacity = 0;
            this.Visible = true;
            for (int i = 0; i <= 10000; i++)
            {
                this.Opacity = (double)i / 10000;
            }

            //引数の受け取り
            id = ptid;
            ECMOno = ecmono;

            //SQLから最新の輸液設定情報を読み取り表示する
            //インスタンス作成
            ClassSQLite db = new ClassSQLite();
            //テーブルの作成
            db.CreateMedicationTable(ptid,ecmono);
            //データベース記録から最新の値を読み取り反映させる。データがない場合は0を入力
            string[] line = db.LatestMedication(id, ECMOno).Split(',');
            try
            {
                trackBarHeparin.Value = int.Parse(line[0]);
            }
            catch
            {
                trackBarHeparin.Value = 0;
            }
            try
            {
                trackBarNAD.Value = int.Parse(line[1]);
            }
            catch
            {
                trackBarNAD.Value = 0;
            }
            try
            {
                trackBarDOA.Value = int.Parse(line[2]);
            }
            catch
            {
                trackBarDOA.Value = 0;
            }
            try
            {
                trackBarDOB.Value = int.Parse(line[3]);
            }
            catch
            {
                trackBarDOB.Value = 0;
            }
            try
            {
                trackBarMain.Value = int.Parse(line[4]);
            }
            catch
            {
                trackBarMain.Value = 0;
            }
            try
            {
                trackBarOther.Value = int.Parse(line[5]);
            }
            catch
            {
                trackBarOther.Value = 0;
            }
            textBoxHeparin.Text = trackBarHeparin.Value.ToString();
            textBoxNAD.Text = trackBarNAD.Value.ToString();
            textBoxDOA.Text = trackBarDOA.Value.ToString();
            textBoxDOB.Text = trackBarDOB.Value.ToString();
            textBoxMain.Text = trackBarMain.Value.ToString();
            textBoxOther.Text = trackBarOther.Value.ToString();           
        }


        private void trackBarHeparin_Scroll(object sender, EventArgs e)
        {
            textBoxHeparin.Text = trackBarHeparin.Value.ToString();
        }

        private void trackBarNAD_Scroll(object sender, EventArgs e)
        {
            textBoxNAD.Text = trackBarNAD.Value.ToString();
        }

        private void trackBarDOA_Scroll(object sender, EventArgs e)
        {
            textBoxDOA.Text = trackBarDOA.Value.ToString();
        }

        private void trackBarDOB_Scroll(object sender, EventArgs e)
        {
            textBoxDOB.Text = trackBarDOB.Value.ToString();
        }

        private void trackBarMain_Scroll(object sender, EventArgs e)
        {
            textBoxMain.Text = trackBarMain.Value.ToString();
        }

        private void trackBarOther_Scroll(object sender, EventArgs e)
        {
            textBoxOther.Text = trackBarOther.Value.ToString();
        }

        private void textBoxHeparin_TextChanged(object sender, EventArgs e)
        {
            if (textBoxHeparin.Text == "") return;
            trackBarHeparin.Value = int.Parse(textBoxHeparin.Text);
        }

        private void textBoxNAD_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNAD.Text == "") return;
            trackBarNAD.Value = int.Parse(textBoxNAD.Text);
        }

        private void textBoxDOA_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDOA.Text == "") return;
            trackBarDOA.Value = int.Parse(textBoxDOA.Text);
        }

        private void textBoxDOB_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDOB.Text == "") return;
            trackBarDOB.Value = int.Parse(textBoxDOB.Text);
        }

        private void textBoxMain_TextChanged(object sender, EventArgs e)
        {
            if (textBoxMain.Text == "") return;
            trackBarMain.Value = int.Parse(textBoxMain.Text);
        }

        private void textBoxOther_TextChanged(object sender, EventArgs e)
        {
            if (textBoxOther.Text == "") return;
            trackBarOther.Value = int.Parse(textBoxOther.Text);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //データをSQL　MEDICATIONテーブルに登録
            ClassSQLite db = new ClassSQLite();
            int heparin = int.Parse(textBoxHeparin.Text);
            double nad = double.Parse(textBoxNAD.Text);
            double doa = double.Parse(textBoxDOA.Text);
            double dob = double.Parse(textBoxDOB.Text);
            int main = int.Parse(textBoxMain.Text);
            int other = int.Parse(textBoxOther.Text);
            db.InsertMedication(heparin, nad, doa, dob, main, other, id, ECMOno);
            this.Close();
        }
    }
}
