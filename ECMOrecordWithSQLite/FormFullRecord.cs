using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace ECMOrecordWithSQLite
{
    public partial class FormFullRecord : Form
    {
        //引数を受け取るための変数を定義
        static int PtID;
        static int ECMOno;
        static string[] ECMOdataSet = new string[8];
        static string[] CDIdataSet = new string[12];
        static string[] VitalDataSet;
        static DateTime dt;
        static string site = "CDI";

        public FormFullRecord(int id, int no, string[] ecmo, string[] cdi, string[] vital)
        {
            InitializeComponent();
            PtID = id;
            ECMOno = no;
            ECMOdataSet = ecmo;
            CDIdataSet = cdi;
            VitalDataSet = vital;

            dt = DateTime.Now;
            //記録時刻の表示(変更不可)
            labelTime.Text = dt.ToString();

            //受け取ったデータセットが使用出来るのか判断
            //ECMOデータが現在時刻から1分以上ズレていたらフォームへの自動入力はしない
            if (ECMOdataSet[0] != null)
            {
                TimeSpan ts = dt - DateTime.Parse(ECMOdataSet[0]);
                labelTime.Text = dt.ToString();
                if (ts.Minutes > 1)
                {
                }
                else
                {
                    ViewECMO();
                }
            }

            //CDIデータが現在時刻から2分以上ズレていたらフォームへの自動入力はしない
            if (CDIdataSet[0] != null)
            {
                TimeSpan ts = dt - DateTime.Parse(CDIdataSet[0]);
                if (ts.Minutes > 2)
                {

                }
                else
                {
                    ViewCDI();
                }
            }
            //Vitalデータが現在時刻から2分以上ズレていたらフォームへの自動入力はしない
            if (VitalDataSet[0] != null)
            {
                TimeSpan ts = dt - DateTime.Parse(VitalDataSet[0]);
                if (ts.Minutes > 2)
                {

                }
                else
                {
                    ViewVital();
                }
            }
        }
        public void ViewECMO()
        {
            textBoxFlow.Text = ECMOdataSet[1];
            textBoxRPM.Text = ECMOdataSet[6];
            textBoxPress1.Text = ECMOdataSet[2];
            textBoxPress2.Text = ECMOdataSet[3];
            textBoxTemp1.Text = ECMOdataSet[4];
            textBoxTemp2.Text = ECMOdataSet[5];
        }
        public void ViewCDI()
        {
            textBoxpH.Text = CDIdataSet[2];
            textBoxPCO2.Text = CDIdataSet[3];
            textBoxPO2.Text = CDIdataSet[4];
            textBoxHCO3.Text = CDIdataSet[6];
            textBoxBE.Text = CDIdataSet[7];
            textBoxHct.Text = CDIdataSet[8];
            textBoxHb.Text = CDIdataSet[9];
            textBoxSO2.Text = CDIdataSet[10];
            textBoxK.Text = CDIdataSet[11];
        }

        public void ViewVital()
        {
            textBoxHR.Text = VitalDataSet[1];
            textBoxABP.Text = VitalDataSet[2];
            textBoxPAP.Text = VitalDataSet[3];
            textBoxCVP.Text = VitalDataSet[4];
            textBoxSpO2.Text = VitalDataSet[5];
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {               

        }
        private void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //イベントsenderをTextBoxでキャスト
                var name = (TextBox)sender;
                //対象オブジェクトを現在のフォームから探す
                Control[] cs = this.Controls.Find($"{name.Name}", true);
                //textにオブジェクトのテキストを代入する
                var text = ((TextBox)cs[0]).Text;

                if (text != "" & text.IndexOf(".") > 0)
                {
                    try
                    {
                        //doubleに変換出来れば次のコントロールへ
                        ((TextBox)cs[0]).Text = "";
                        ((TextBox)cs[0]).Text = double.Parse(text).ToString();
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (text != "" & text.IndexOf(".") < 0)
                {
                    try
                    {
                        //intに変換出来れば次のコントロールへ
                        ((TextBox)cs[0]).Text = "";
                        ((TextBox)cs[0]).Text = int.Parse(text).ToString();
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }else if (e.KeyCode == Keys.Tab)
            {
                //Tabキー押したらメソッドの終了
                e.IsInputKey = true;
                return;
            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

            if(textBoxComment.Text == "")
            {
                textBoxComment.Text = "定時チェック";
            }
            //SQL登録した後フォームを閉じる
            ClassSQLite db = new ClassSQLite();
            //ECMOデータSQL登録
            dt = DateTime.Now;
            db.InsertECMOdata(ECMOno,PtID,dt.ToString("yyyy/MM/dd/ HH:mm:ss"),"",textBoxFlow.Text,textBoxRPM.Text, textBoxPress1.Text, textBoxPress2.Text,"",
                textBoxTemp1.Text, textBoxTemp2.Text, textBoxGasFlow.Text, textBoxO2.Text, textBoxHCtemp.Text, textBoxComment.Text);

            //Labデータの追加
            db.InsertLab(PtID, ECMOno, dt.ToString("yyyy/MM/dd/ HH:mm:ss"),site,textBoxpH.Text, textBoxPCO2.Text, textBoxPO2.Text, textBoxHCO3.Text, textBoxBE.Text, textBoxHct.Text, 
                textBoxHb.Text, textBoxSO2.Text, textBoxK.Text, textBoxNa.Text, textBoxLac.Text, textBoxCa.Text, textBoxGlu.Text);

            db.InsertVital(PtID, ECMOno, dt.ToString("yyyy/MM/dd/ HH:mm:ss"),textBoxHR.Text, textBoxABP.Text, textBoxPAP.Text, textBoxCVP.Text, textBoxSpO2.Text);
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ViewCDI();
            site = "CDI";
        }

        private void LabelReset()
        {
            textBoxpH.Text = "";
            textBoxPCO2.Text = "";
            textBoxPO2.Text = "";
            textBoxK.Text = "";
            textBoxNa.Text = "";
            textBoxLac.Text = "";
            textBoxGlu.Text = "";
            textBoxCa.Text = "";
            textBoxHCO3.Text = "";
            textBoxBE.Text = "";
            textBoxHb.Text = "";
            textBoxHct.Text = "";
            textBoxSO2.Text = "";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            site = "人工肺後";
            //ラベルをリセット
            LabelReset();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            site = "人工肺前";
            //ラベルをリセット
            LabelReset();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            site = "右手";
            //ラベルをリセット
            LabelReset();

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            site = "左手";
            //ラベルをリセット
            LabelReset();

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            site = "CV";
            //ラベルをリセット
            LabelReset();

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBoxSupport_DoubleClick(object sender, EventArgs e)
        {
            textBoxComment.Text = listBoxSupport.SelectedItem.ToString();
        }
    }
}
