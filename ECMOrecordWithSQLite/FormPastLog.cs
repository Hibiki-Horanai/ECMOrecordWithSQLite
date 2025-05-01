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
    public partial class FormPastLog : Form
    {

        static int ID;
        static int No;
        static string site = "人工肺後";

        public FormPastLog(int id, int no)
        {
            InitializeComponent();
            //アプリ起動時にフェードインしてくるようなアニメーション設定
            this.Opacity = 0;
            this.Visible = true;
            for (int i = 0; i <= 10000; i++)
            {
                this.Opacity = (double)i / 10000;
            }

            dateTimePickerLogTime.CustomFormat = "yyyy/MM/dd HH:mm";
            dateTimePickerLogTime.Value = DateTime.Now;

            listBoxSupport.SelectedIndex = 0;

            ID = id;
            No = no;
        }



        private void listBoxSupport_DoubleClick(object sender, EventArgs e)
        {
            textBoxComment.Text = listBoxSupport.SelectedItem.ToString();
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*            //そのコントロールがTAB or Enterが入力されていたらメッセージ
                        //イベントsenderをTextBoxでキャスト
                        var name = (TextBox)sender;
                        //対象オブジェクトを現在のフォームから探す
                        Control[] cs = this.Controls.Find($"{name.Name}", true);
                        //オブジェクトテキストからtab文字を削除する
                       // MessageBox.Show(((TextBox)cs[0]).Text);
                        ((TextBox)cs[0]).Text = ((TextBox)cs[0]).Text.Replace("\t", "");
                        MessageBox.Show(e.ToString());
                        if (e.KeyCode == Keys.Tab)
                        {
                            if(((TextBox)cs[0]).Text == "")
                            {
                                return;
                            }
                            else
                            {
                                ((TextBox)cs[0]).Text = ((TextBox)cs[0]).Text.Replace("\t", "").Trim();
                            }

                        }*/
        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox_PreviewKeyDown(object sender ,PreviewKeyDownEventArgs e)
        {
            //イベントsenderをTextBoxでキャスト
            var name = (TextBox)sender;
            //対象オブジェクトを現在のフォームから探す
            Control[] cs = this.Controls.Find($"{name.Name}", true);
            //textにオブジェクトのテキストを代入する
            var text = ((TextBox)cs[0]).Text.Replace("\r\n","");
            if (text == "")
            {
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
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
                else if (text != "" & text.IndexOf(".") ==-1)
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
                        MessageBox.Show("int"+ex.Message);
                    }
                }
            }
            else if (e.KeyCode == Keys.Tab)
            {
                //Tabキー押しても次のコントロールに移動しないで処理
                e.IsInputKey = true;
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
                else if (text != "" & text.IndexOf(".") == -1)
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
                        MessageBox.Show("int" + ex.Message);
                    }
                }

            }
        }
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            //SQL登録用のインスタンス生成
            ClassSQLite db = new ClassSQLite();

            //ECMO情報登録用の変数に代入
            string datetime = dateTimePickerLogTime.Text;   //記録日時採血情報とも共用
            string starttime = "";
            string flow = textBoxFlow.Text;
            string rpm = textBoxRPM.Text;
            string press1 = textBoxPress1.Text;
            string press2 = textBoxPress2.Text;
            string temp1 = textBoxTemp1.Text;
            string temp2 = textBoxTemp2.Text;
            string gas = textBoxGasFlow.Text;
            string O2 = textBoxO2.Text;
            string HCtemp = textBoxHCtemp.Text;
            string comment = textBoxComment.Text;
            if(comment == "ECMO START")
            {
                starttime = datetime;
            }
            //ECMO情報の登録
            db.InsertECMOdata(No, ID, datetime, starttime, flow, rpm, press1, press2, temp1, temp2, "", gas, O2, HCtemp, comment);


            if (textBoxComment.Text.IndexOf("採血") != -1)
            {
                //採血情報登録用の変数に代入
                string pH = textBoxpH.Text;
                string PO2 = textBoxPO2.Text;
                string PCO2 = textBoxPCO2.Text;
                string K = textBoxK.Text;
                string Na = textBoxNa.Text;
                string Lac = textBoxLac.Text;
                string Ca = textBoxCa.Text;
                string Glu = textBoxGlu.Text;
                string HCO3 = textBoxHCO3.Text;
                string BE = textBoxBE.Text;
                string Hb = textBoxHb.Text;
                string Hct = textBoxHct.Text;
                string SO2 = textBoxSO2.Text;

                //採血情報の登録
                db.InsertLab(ID,No,datetime,site,pH,PCO2,PO2,HCO3,BE,Hct,Hb,SO2,K,Na,Lac,Ca,Glu);
            }
            this.Close();
        }

        private void ButtonCancel_Click(object sender,EventArgs e)
        {
            this.Close();
        }



        //-―――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        //radioButtonを押したときにサイトを自動で変更する
        //-―――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            site = "CDI";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            site = "人工肺後";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            site = "人工肺前";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            site = "右手";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            site = "左手";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            site = "CV";
        }
    }
}
