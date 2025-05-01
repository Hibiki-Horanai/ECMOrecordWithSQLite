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
    public partial class FormPatientInfomation : Form
    {
        static private int PtID;
        static private int ECMOno;
        ClassSQLite db = new ClassSQLite(); //データベース通信を行うためのコンストラクタ生成

        public FormPatientInfomation(int id , int no)
        {
            InitializeComponent();

            //IDとECMOnoを受け取って表示させる
            PtID = id;
            ECMOno = no;


        }

        private void FormPatientInfomation_Load(object sender, EventArgs e)
        {
            //id,noでデータベース検索をかけて、情報を表示させる

            string[] arr = db.PatientInfo(PtID, ECMOno);
            if (arr == null) {
                return;
            }
            else
            {
                textBoxName.Text = arr[0];
                textBoxHeight.Text = arr[1];
                textBoxWeight.Text = arr[2];
                textBoxGender.Text = arr[4];
                textBoxBirth.Text = arr[5];
                textBoxBloodType.Text = arr[7];
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //表示されている情報をデータベースに登録する
            //登録完了後にメッセージを表示してフォームを閉じる

            string[] data = new string[6];

        }
    }
}
