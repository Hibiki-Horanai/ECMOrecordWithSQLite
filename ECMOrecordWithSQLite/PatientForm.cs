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
    public partial class PatientForm : Form
    {
        public PatientForm()
        {
            InitializeComponent();
            //アプリ起動時にフェードインしてくるようなアニメーション設定
            this.Opacity = 0;
            this.Visible = true;
            for (int i = 0; i <= 10000; i++)
            {
                this.Opacity = (double)i / 10000;
            }


        }

        ClassSQLite db = new ClassSQLite();

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            //選択された患者idとecmonoで新規記録or継続記録を行う
            int id, no;
            no = int.Parse(dgvPatientList.CurrentRow.Cells[0].Value.ToString());//一番左
            id = int.Parse(dgvPatientList.CurrentRow.Cells[1].Value.ToString());//左から2番目
            db.CreateECMOtable(id,no);

            if (!db.SearchNOID(no, id))
            {
                //新規で記録を行う場合 db.SearchNOID==false
                string text = $"ID:{id}で新規記録を開始します";
                DialogResult result = MessageBox.Show(text, "記録開始",
                    MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    this.Visible = false;
                    //id とnoを引数としてレコードフォームを表示する
                    FormRecord formRecord = new FormRecord(no, id);
                    formRecord.Show();
                }

            }
            else
            {
                //すでに記録がある場合
                string text = $"ID:{id}で記録を再開します";
                DialogResult result = MessageBox.Show(text, "記録再開",
                    MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    //
                    //MessageBox.Show("OKが選択されました。");
                    this.Visible = false;
                    FormRecord formRecord = new FormRecord(no, id);
                    formRecord.Show();
                }
            }
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("選択患者でレポート作成します","レポート作成",
                MessageBoxButtons.OKCancel);
            if(result == DialogResult.OK)
            {
                MessageBox.Show("レポート作成画面へ移動します。");
            }


        }

        private void buttonCnacel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonRegi_Click(object sender, EventArgs e)
        {
            //IDと名前は空欄不可能とする
            if (textBoxID.Text == "" || textBoxName.Text == "")
            {
                MessageBox.Show("IDと名前は必ず入力してください。");
                return;
            }

            int id;
            double height, weight, bsa;
            string gender, birth, name;
            try
            {
                id = int.Parse(textBoxID.Text);
                name = textBoxName.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                height = double.Parse(textBoxHeight.Text);
            }
            catch { height = 0.0; }
            try
            {
                weight = double.Parse(textBoxWeight.Text);
            }
            catch { weight = 0.0; }

            bsa = Math.Pow(height, 0.725) * Math.Pow(weight, 0.425) * 0.007184;
            if (radioButton1.Checked)
            {
                gender = "M";
            }
            else if (radioButton2.Checked)
            {
                gender = "F";
            }
            else
            {
                gender = "Other";
            }
            birth = "";
            DateTime dt = DateTime.Now;
            

            //SQLiteへ登録
            //ClassSQLite db = new ClassSQLite();
            bool flag = db.InsertPtInf(id, name, height, weight, bsa, gender, birth, 0, "不明");

            if (flag)
            {
                //dataGrid更新
                //ClassSQLite db = new ClassSQLite();
                dgvPatientList.DataSource = db.PatientList();
                dgvPatientList.FirstDisplayedScrollingRowIndex = dgvPatientList.Rows.Count - 1;
            }

        }

        private void PatientForm_Load(object sender, EventArgs e)
        {

            //SQLiteから患者情報を取得して表示する、また最終行を選択する
            ClassSQLite db = new ClassSQLite();
            try
            {
                dgvPatientList.DataSource = db.PatientList();
                dgvPatientList.FirstDisplayedScrollingRowIndex = dgvPatientList.Rows.Count - 1;
                //SQLiteから患者情報を取得して表示する
                labelNOR.Text = dgvPatientList.BindingContext[dgvPatientList.DataSource, dgvPatientList.DataMember].Count.ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void PatientForm_Activate(object sender, EventArgs e)
        {
            //SQLiteから患者情報を取得して表示する
            ClassSQLite db = new ClassSQLite();
            try
            {
                dgvPatientList.DataSource = db.PatientList();
                dgvPatientList.FirstDisplayedScrollingRowIndex = dgvPatientList.Rows.Count - 1;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }


        private void buttonPtDetails_Click(object sender, EventArgs e)
        {
            //患者情報の詳細を入力するフォームを表示
            PatientDetails ptd = new PatientDetails();
            ptd.Show();

        }

        private void buttonModi_Click(object sender, EventArgs e)
        {
            //SQLテーブルへデータグリッドの値を反映させる
            try {
                //datagridviewの値をDataTableに変換
                var data = (DataTable)dgvPatientList.DataSource;
                //ClassSQLite内のメソッドで渡したデータテーブルに更新
                db.DataUpLoad(data);
                //データグリッドビューの更新
                dgvPatientList.DataSource = db.PatientList();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PatientForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
