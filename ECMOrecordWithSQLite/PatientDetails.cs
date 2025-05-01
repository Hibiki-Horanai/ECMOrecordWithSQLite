using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ECMOrecordWithSQLite
{
    public partial class PatientDetails : Form
    {
        public PatientDetails()
        {
            InitializeComponent();
        }

        private void PatientDetails_Load(object sender, EventArgs e)
        {
            //アプリ起動時にフェードインしてくるようなアニメーション設定
            this.Opacity = 0;
            this.Visible = true;
            for (int i = 0; i <= 10000; i++)
            {
                this.Opacity = (double)i / 10000;
            }
            listBox1.SelectedIndex = 8;
        }

        private void buttonConf_Click(object sender, EventArgs e)
        {
            //IDと名前は空欄不可能とする
            if(textBoxID.Text=="" || textBoxName.Text == "")
            {
                MessageBox.Show("IDと名前は必ず入力してください。");
                return;
            }

            int id;
            double height, weight,bsa;
            string gender,birth,name;
            DateTime dtBirth;
            try
            {
                id = int.Parse(textBoxID.Text);
                name = textBoxName.Text;
            }catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                height = double.Parse(textBoxHeight.Text);
            }
            catch{height = 0.0; }
            try
            {
                weight = double.Parse(textBoxWeight.Text);
            }
            catch { weight = 0.0; }

            bsa = Math.Pow(height,0.725) * Math.Pow(weight,0.425) * 0.007184;
            if (radioButton1.Checked)
            {
                gender = "M";
            }else if (radioButton2.Checked)
            {
                gender = "F";
            }else
            {
                gender = "Other";
            }
            birth = textBoxBirth.Text;
            try
            {
                dtBirth = DateTime.Parse(birth);
            }
            catch { dtBirth = DateTime.Now; }
            DateTime dt = DateTime.Now;
            int age = dt.Year - dtBirth.Year;
            if(dt < dtBirth.AddYears(age))
            {
                age--;
            }

            string bloodType = listBox1.SelectedItem.ToString();

            //SQLiteへ登録
            ClassSQLite db = new ClassSQLite();
            bool flag = db.InsertPtInf(id, name, height, weight, bsa, gender, birth, age, bloodType);
            if (flag)
            {
                //登録が完了したらフォームを閉じる
                this.Close();
                
            }
        }
    }
}
