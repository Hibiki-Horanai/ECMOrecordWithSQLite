using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.IO;
using System.IO.Ports;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using Microsoft.Xaml.Behaviors;
using Microsoft.VisualBasic;
using System.Drawing.Drawing2D;
using System.Windows.Media.TextFormatting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Microsoft.Win32;

namespace ECMOrecordWithSQLite
{
    public partial class FormRecord : Form
    {

        static int ECMOno;
        static int PtID;
        static int btncnt = 0;
        ClassSQLite db = new ClassSQLite();
        static private IPAddress ECMOip;
        static private int ECMOport;
        static private bool ConnectFlag = false;
        static private TcpListener listener = null;
        static private string filePath;
        static private string filePathOne;
        static private bool connect_status = false;
        static private int interval = 6000;
        static DateTime dtstart = DateTime.Now;
        static private int intervalECMO = 5000;//ECMO記録のためのインターバル設定
        static private int cntECMO = 0;         //ECMO dataをSQLiteに登録するかどうかをカウントする変数
        static private int rec_interval = 600;  //取得データをSQLiteに登録する時間間隔(sec)
        static private double BSA;
        static private string[] ecmo_buff = new string[8];
        static private string[] CDI_buff = new string[12];
        static private string[] vital_buff = new string[5];
        static private string[] resp_buff = new string[10];
        SerialPort portCDI = new SerialPort();
        static private string COMCDI = "COM4";
        static string disconnectFilePath;
        static string connectFilePath;
        static string connectingFilePath;

        //ループ条件定義
        static bool CancelStatus = true;
        static bool monitorCancelStatus = true;
        static bool respCancelStatus = true;

        //グラフデータ収集用flag
        static bool connectECMO = false;        //初期値false 接続時：true　切断時：false
        static bool connectCDI = false;         //初期値false 接続時：true　切断時：false
        static bool connectMonitor = false;     //初期値false 接続時：true　切断時：false
        static bool drawFlag = false;

        //採血ボタン制御用
        static int blood_cnt = 0;


        public FormRecord(int ecmono, int ptid)
        {
            InitializeComponent();
            //アプリ起動時にフェードインしてくるようなアニメーション設定
            this.Opacity = 0;
            this.Visible = true;
            for (int i = 0; i <= 10000; i++)
            {
                this.Opacity = (double)i / 10000;
            }
            //フォームのサイズを最大化
            this.WindowState = FormWindowState.Maximized;

            //もし引数を受け取れなかった場合の例外処理を記載する必要あり。
            ECMOno = ecmono;
            PtID = ptid;
            //もし引数を受け取れなかった場合の例外処理を記載する必要あり。

            labelECMOno.Text = $"ECMO No:{ECMOno}";
            labelID.Text = $"Pt ID:{PtID}";

            //iconのパスを指定
            disconnectFilePath = Environment.CurrentDirectory + "\\disconnect.png";
            connectFilePath = Environment.CurrentDirectory + "\\connect.png";
            connectingFilePath = Environment.CurrentDirectory + "\\connecting.jpg";
            pictureBoxECMO.Image = Image.FromFile(disconnectFilePath);
            pictureBoxCDI.Image = Image.FromFile(disconnectFilePath);
            pictureBoxMonitor.Image = Image.FromFile(disconnectFilePath);
            pictureBoxResp.Image = Image.FromFile(disconnectFilePath);
            pictureBoxCRRT.Image = Image.FromFile(disconnectFilePath);

            //患者情報
            try
            {
                BSA = double.Parse(db.SearchPtInfo(PtID, ECMOno, "BSA"));
            }
            catch
            {
                BSA = 0.0;
            }

            //履歴に表示
            dataGridViewHistory.DataSource = db.Historyt(PtID, ECMOno);
            try
            {
                dataGridViewHistory.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //列を作成
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                //buttonColumn.DataPropertyName = "詳細";
                //名前とヘッダーを設定
                buttonColumn.Name = "detail";
                buttonColumn.HeaderText = "詳細へ";
                //列を追加する
                dataGridViewHistory.Columns.Add(buttonColumn);
            }
            catch
            { }
            //DGV作成直後はセル選択が安定せず、一度リフレッシュ目的に再読み込み
            ReLoad_Main();

            //グラフ描画テスト

            //get List test
            GraphDataAdd_SQL();

        }

        //custom classの宣言
        class classECMO
        {
            public double time;
            public double flow;
            public double rpm;
            public double press;


            public classECMO(double time, double flow, double rpm, double press)
            {
                this.flow = flow;
                this.time = time;
                this.rpm = rpm;
                this.press = press;
            }
        }
        //classECMOインスタンス生成
        static List<classECMO> ECMOdataList = new List<classECMO>();

        //採血ログ保存用のList配列を宣言(時間、採血部位)同時に更新しないと大変なことになる…
        List<string> collect_time = new List<string>();
        List<string> collect_site = new List<string>();

        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        //使用開始からはECMOdataListに追加していけば良いが、再開時はSQLから読み込んできて表示させるようにしないといけない
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――

        class classVital
        {
            public double time;
            public double hr;
            public double abp;
            public double cvp;
            public double spo2;


            public classVital(double time, double hr, double abp, double cvp, double spo2)
            {
                this.time = time;
                this.hr = hr;
                this.abp = abp;
                this.cvp = cvp;
                this.spo2 = spo2;
            }
        }
        //classVitalのインスタンス生成
        static List<classVital> VitaldataList = new List<classVital>();

        class classCDI
        {
            public double time;
            public double hb;
            public double pao2;
            public double paco2;

            public classCDI(double time, double hb, double pao2, double paco2)
            {
                this.time = time;
                this.hb = hb;
                this.pao2 = pao2;
                this.paco2 = paco2;
            }
        }
        //classCDIのインスタンス生成
        static List<classCDI> CDIdataList = new List<classCDI>();

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        //Nav-buttonの設定　ここから
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        private async void buttonRec_Click(object sender, EventArgs e)
        {

            //ECMO STARTログがある場合は読み込む 無ければ現在日時を仮で登録
            try
            {
                dtstart = DateTime.Parse(db.SearchStartTime(PtID, ECMOno));
                labelStartDateTime.Text = dtstart.ToString();
            }
            catch
            {
                //開始時刻にすでに(仮)データがある場合はその値を採用する
                if (labelStartDateTime.Text.IndexOf("(仮)") > 0)
                    dtstart = DateTime.Parse(labelStartDateTime.Text.Replace("(仮)", "").Trim());
                else
                    dtstart = DateTime.Now;
                labelStartDateTime.Text = dtstart.ToString() + " (仮)";
            }


            //開始日時ラベルに仮で記録開始ボタンを押した時刻を入力して経過時間カウントを開始する
            if (btncnt % 2 == 0)
            {
                buttonRec.Text = "記録停止";
                buttonRec.BackColor = Color.Red;
                timer1.Enabled = true;
                timer1.Start();
            }
            else
            {
                buttonRec.Text = "記録開始";
                buttonRec.BackColor = Color.Navy;
                timer1.Stop();
            }
            btncnt++;
        }
        private void buttonInput_Click(object sender, EventArgs e)
        {
            //戻ってきた時にグラフを再描画するためのflagを立てる
            drawFlag = true;

            //新しい入力フォームを表示させてデータ入力する
            //出力するデータセットはid,no,ECMO_buff,CDI_buff,Vital_buff
            //データセットを使うかどうかは表示したフォームで判断する
            FormFullRecord formFull = new FormFullRecord(PtID, ECMOno, ecmo_buff, CDI_buff, vital_buff);
            formFull.Show();

        }
        private void buttonInputComment_Click(object sender, EventArgs e)
        {
            //現在の記録に対してCommentを残す
            //インプットダイアログのようなものを想定
            string[] tempECMO = ecmo_buff;
            DateTime common_time = DateTime.Now;
            string text = "コメントを入力しますか？";
            
            try 
            {
               DateTime.Parse(ecmo_buff[0]).ToString("yyyy/MM/dd/ HH:mm:ss"); 
            }catch(Exception ex)
            {
                MessageBox.Show("記録できるデータが取得出来ません");
                return;
            }
            DialogResult result = MessageBox.Show(text, "メッセージ", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                string temp = (Microsoft.VisualBasic.Interaction.InputBox($"コメントを入力してください"));
                if (temp == "")
                {
                    return;
                }
                //入力されたコメントでSQLデータ追加

                //tempECMO[0]に登録された時間を基準に他データを判断
                TimeSpan ts = DateTime.Now - DateTime.Parse(ecmo_buff[0]);
                if (ts.Minutes > 2)
                {
                    //データが2分を超えて更新されていない場合は破棄
                    MessageBox.Show("ECMOデータが最新でない可能性があります。\r\n通信を確認してください。");
                }
                else
                {
                    //Commentを追加してECMOデータの登録
                    common_time = DateTime.Parse(ecmo_buff[0]);
                    db.InsertECMOdata(ECMOno, PtID, common_time.ToString("yyyy/MM/dd/ HH:mm:ss"), "",
                        tempECMO[1], tempECMO[6], tempECMO[2], tempECMO[3], tempECMO[4], tempECMO[5], tempECMO[6], "", "", "", temp);

                    //common_timeとのずれを計算してCDIの値を登録
                    try { 
                        DateTime dt_temp = DateTime.Parse(CDI_buff[0]);
                        ts = common_time - dt_temp;
                        if (ts.Minutes > 2)
                        {
                            //ECMOとの時間差が2分以上ある場合CDIの処理はスキップ
                        }
                        else
                        {
                            db.InsertLab(PtID, ECMOno, common_time.ToString("yyyy/MM/dd/ HH:mm:ss"), "CDI",
                                CDI_buff[2], CDI_buff[3], CDI_buff[4], CDI_buff[6], CDI_buff[7], CDI_buff[8], CDI_buff[9], CDI_buff[10], CDI_buff[11], "", "", "", "");
                        }
                    }
                    catch
                    {

                    }

                    //common_timeとのずれを計算してVitalの値を登録
                    /*labelHR.Text = list[1];
                    labelABP.Text = list[2];
                    labelPAP.Text = list[3];
                    labelCVP.Text = list[4];*/
                    try
                    {
                        DateTime.Parse(vital_buff[0]);
                        ts = common_time - DateTime.Parse(vital_buff[0]);
                        if (ts.Minutes > 2)
                        {
                            //ECMOとの時間差が2分以上ある場合はVitalの処理はスキップ
                        }
                        else
                        {
                            db.InsertVital(PtID, ECMOno, common_time.ToString("yyyy/MM/dd/ HH:mm:ss"), vital_buff[1], vital_buff[2], vital_buff[3], vital_buff[4], vital_buff[5]);
                        }
                    }
                    catch
                    {

                    }


                }
            }
            ReLoad_Main();


        }
        private void buttonCollectionBlood_Click(object sender, EventArgs e)
        {
            //結果入力メソッド
            //採血は結果が出るまで時間がかかるので、記録のログを残しておきそれを編集する方向にする
            //検索ができるように、id,no,time,collection siteを情報として持たせておく
            //採血ログのみ立てて置き、そこに後からデータを入力するようにする
            //ボタンの名前も「入力待ち」に変更する

            string text = "記録を入力しますか？";
            DialogResult result = MessageBox.Show(text, "結果入力",
                MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //入力項目の初期化
                string[] lab_data_label = new string[]{
                        "日時",
                        "採血部位",
                        "pH",
                        "PCO2",
                        "PO2",
                        "HCO3",
                        "BE",
                        "Hct",
                        "Hb",
                        "SO2",
                        "K",
                        "Na",
                        "Lac",
                        "Ca",
                        "Glu",
                    };


                //ログの逐次入力
                List<string> blood = new List<string>();
                string[] blood_list = new string[] { };
                for (int i = 2; i < lab_data_label.Length; i++)
                {
                    bool cont_flag = true;
                    string temp = "";
                    while (cont_flag)
                    {
                        //入力形式がdoubleに変換出来ないものは再入力をさせるようなループ構造
                        temp = (Microsoft.VisualBasic.Interaction.InputBox($"{lab_data_label[i]}"));
                        try
                        {
                            if (temp != "")
                            {
                                double.Parse(temp);
                            }
                            cont_flag = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            lab_data_label[i] = "再入力" + lab_data_label[i].Replace("再入力", "");
                            cont_flag = true;
                        }
                    }
                    blood.Add(temp);
                }
                blood_cnt++;

                //結果ボタンの無効化と採血ボタンの有効化
                buttonResult.Enabled = false;
                buttonResult.ForeColor = Color.Gainsboro;
                buttonPre.Enabled = true;
                buttonPre.FlatStyle = FlatStyle.Popup;
                buttonPre.ForeColor = Color.White;
                buttonPost.Enabled = true;
                buttonPost.FlatStyle = FlatStyle.Popup;
                buttonPost.ForeColor = Color.White;
                buttonCV.Enabled = true;
                buttonCV.FlatStyle = FlatStyle.Popup;
                buttonCV.ForeColor = Color.White;
                buttonRt.Enabled = true;
                buttonRt.FlatStyle = FlatStyle.Popup;
                buttonRt.ForeColor = Color.White;
                buttonLt.Enabled = true;
                buttonLt.FlatStyle = FlatStyle.Popup;
                buttonLt.ForeColor = Color.White;
            }


        }

        private void Process_CollectionBlood(string site)
        {
            //採血受付けメソッド
            string s = site;
            if (blood_cnt % 2 == 0)
            {
                string text = $"{s}から採血を記録しますか？";
                DialogResult result = MessageBox.Show(text, $"採血実施",
                    MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    //ダイアログでOKをクリックした場合の処理
                    //採血ボタンの無効化と結果ボタンの有効化
                    buttonResult.Enabled = true;
                    buttonResult.FlatStyle = FlatStyle.Popup;
                    buttonResult.ForeColor = Color.White;
                    buttonPre.Enabled = false;
                    buttonPre.FlatStyle = FlatStyle.Flat;
                    buttonPre.ForeColor = Color.Gainsboro;
                    buttonPost.Enabled = false;
                    buttonPost.FlatStyle = FlatStyle.Flat;
                    buttonPost.ForeColor = Color.Gainsboro;
                    buttonCV.Enabled = false;
                    buttonCV.FlatStyle = FlatStyle.Flat;
                    buttonCV.ForeColor = Color.Gainsboro;
                    buttonRt.Enabled = false;
                    buttonRt.FlatStyle = FlatStyle.Flat;
                    buttonRt.ForeColor = Color.Gainsboro;
                    buttonLt.Enabled = false;
                    buttonLt.FlatStyle = FlatStyle.Flat;
                    buttonLt.ForeColor = Color.Gainsboro;

                    blood_cnt++;

                }
            }
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            string t = "人工肺後";
            Process_CollectionBlood(t);
        }
        private void buttonPre_Click(object sender, EventArgs e)
        {
            string t = "人工肺前";
            Process_CollectionBlood(t);
        }
        private void buttonCV_Click(object sender, EventArgs e)
        {
            string t = "CV";
            Process_CollectionBlood(t);
        }
        private void buttonRt_Click(object sender, EventArgs e)
        {
            string t = "右手";
            Process_CollectionBlood(t);
        }
        private void buttonLt_Click(object sender, EventArgs e)
        {
            string t = "左手";
            Process_CollectionBlood(t);
        }
        private void buttonPast_Click(object sender, EventArgs e)
        {
            //戻ってきた時にグラフを再描画するためのflagを立てる
            drawFlag = true;
            
            //過去の記録を入力　主にHCUに帰ってくるまでに紙媒体で記録していたデータを入力する
            //過去記録を入力するためのフォームを開き、まとめて入力できるようにする
            FormPastLog form = new FormPastLog(PtID, ECMOno);
            form.Show();
        }
        private void buttonVolumeIn_Click(object sender, EventArgs e)
        {
            FormVolumeIn formVolumeIn = new FormVolumeIn(ECMOno, PtID);
            formVolumeIn.Show();
        }

        private void buttonVolumeOut_Click(object sender, EventArgs e)
        {
            FormVolumeOut formVolumeOut = new FormVolumeOut(ECMOno, PtID);
            formVolumeOut.Show();
        }
        private void buttonInjection_Click(object sender, EventArgs e)
        {
            FormInjection formInjection = new FormInjection(ECMOno, PtID);
            formInjection.Show();
        }

        private void buttonECMOend_Click(object sender, EventArgs e)
        {
            //すべての記録スクリプトの停止と、アプリ終了ボタンの有効化
            //初期状態では押せないようにする予定
        }
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        //Nav-buttonの設定　ここまで
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――

        //Monitor関連
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――

        private async void MonitorConnect_Click(object sender, EventArgs e)
        {
            if (CancelStatus == true)
            {
                labelMonitor.Text = "Monitor : Connecting";
                pictureBoxMonitor.Image = Image.FromFile(connectingFilePath);
                CancelStatus = !CancelStatus;
            }
            else
            {
                labelMonitor.Text = "Monitor : disconnect";
                pictureBoxMonitor.Image = Image.FromFile(disconnectFilePath);
                CancelStatus = !CancelStatus;
                return;
            }

            //モニタの値を取得する
            while (!CancelStatus)
            {
                string data = await Task.Factory.StartNew(ReceiveMonitor);
                //ループ条件定義として’CancelStatus’を判定して実行
                if (data == "")
                {
                    pictureBoxMonitor.Image = Image.FromFile(disconnectFilePath);
                    CancelStatus = true; ;
                }
                else
                {
                    pictureBoxMonitor.Image = Image.FromFile(connectFilePath);
                }
                //検索パラメータ配列
                string[] search = {
                @"HR\|1\|\d+" ,
                @"ART\(M\)\|1\|\d+",
                @"PAP\(M\)\|1\|\d+[.]?\d+",
                @"CVP\(M\)\|1\|[-]?\d+",
                //@"TRECT\|1\|\d+[.]?\d+",
                @"SpO2\|1\|[-]?\d+",
            };
                //削除パラメータ配列
                string[] delete =
                {
                "HR|1|" ,
                "ART(M)|1|",
                "PAP(M)|1|",
                "CVP(M)|1|",
                //"TRECT|1|",
                "SpO2|1|",
            };
                List<string> list = new List<string>();
                list.Add(DateTime.Now.ToString());
                for (int i = 0; i < search.Length; i++)
                {
                    var temp = Regex.Match(data, search[i]).Value.Replace(delete[i], "");
                    if (temp != "")
                        list.Add(temp);
                    else
                        list.Add("");

                }

                //メインフォームに表示
                labelHR.Text = list[1];
                labelABP.Text = list[2];
                labelPAP.Text = list[3];
                labelCVP.Text = list[4];

                //カスタムリスト用バッファーへ登録
                vital_buff = list.ToArray();

                list.Clear();
            }
        }

        private string ReceiveMonitor()
        {
            //Monitor接続を記載する非同期コード
            //クライアント側のコード
            IPAddress ipmonitor = IPAddress.Parse("192.168.254.252");
            int portmonitor = 7998;
            IPEndPoint remoteEPm = new IPEndPoint(ipmonitor, portmonitor);
            Socket socketm = new Socket(ipmonitor.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            string data;
            try
            {
                //作成したソケットに接続
                socketm.Connect(remoteEPm);
                //receiveで受信
                byte[] bytes = new byte[4096];

                int bytesRec = socketm.Receive(bytes);
                data = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                //ソケットを終了
                socketm.Shutdown(SocketShutdown.Both);
                socketm.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                data = "";
            }
            return data;
        }
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――

        //ECMO関連
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        private void ECMOconnect_Click(object sender, EventArgs e)
        {
            ECMOport = 8001;
            ECMOip = IPAddress.Parse("192.168.254.253");

            if (!ConnectFlag)
            {
                try
                {
                    labelECMO.Text = "ECMO:connecting";
                    labelECMO.Enabled = false;
                    pictureBoxECMO.Image = Image.FromFile(connectingFilePath);
                    listener = new TcpListener(ECMOip, ECMOport);
                    listener.Start();
                }
                catch (Exception ex)
                {
                    connectECMO = false;
                    labelECMO.Text = "ECMO:disconnect";
                    labelECMO.Enabled = true;
                    pictureBoxECMO.Image = Image.FromFile(disconnectFilePath);
                    MessageBox.Show(ex.Message);
                    return;
                }
                ConnectFlag = !ConnectFlag;
                timerECMO.Enabled = true;
                timerECMO.Interval = intervalECMO;
                timerECMO.Start();
            }
            else
            {
                //警告出して接続を切る
                MessageBox.Show("ECMOの「LAN」を無効にして下さい");
                timerECMO.Stop();
                listener.Stop();
                ConnectFlag = !ConnectFlag;
                labelECMO.Text = "ECMO:disconnected";
                pictureBoxECMO.Image = Image.FromFile(disconnectFilePath);
                connectECMO = false;
                return;
            }
        }

        private async void timerECMO_Tick(object sender, EventArgs e)
        {
            var data = await Task.Factory.StartNew(Receive_ECMO);
            if (data != "")
            {
                labelECMO.Text = "ECMO:connected";
                pictureBoxECMO.Image = Image.FromFile(connectFilePath);
                connectECMO = true;
                labelECMO.Enabled = true;
            }
            else
            {
                connectECMO = false;
                timerECMO.Stop();
                listener.Stop();
                ConnectFlag = false;
                labelECMO.Text = "ECMO:disconnected";
                labelECMO.Enabled = true;
                pictureBoxECMO.Image = Image.FromFile(disconnectFilePath);
                connectECMO = false;
                MessageBox.Show("ECMOの通信設定を確認してください。");
            }
            View_ECMOdata(data);
            cntECMO += 5;
        }

        static string Receive_ECMO()
        {

            TcpClient tcp = listener.AcceptTcpClient();
            connect_status = true;
            string result;
            using (NetworkStream ns = tcp.GetStream())
            {
                ns.ReadTimeout = 5000;
                ns.WriteTimeout = 5000;
                Encoding enc = Encoding.UTF8;
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] resBytes = new byte[1024];
                    int resSize = 0;
                    resSize = ns.Read(resBytes, 0, resBytes.Length);
                    if (resSize == 0)
                    {
                        MessageBox.Show("no data");
                    }
                    ms.Write(resBytes, 0, resSize);
                    result = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                }
                return result;
            }

        }

        private void View_ECMOdata(string ECMOtext)
        {
            //bufferの中身をリセット
            Array.Clear(ecmo_buff, 0, ecmo_buff.Length);
            //textの中から正規表現で特定のデータを検索する
            string data_text = "";
            string[] search =
            {
                    "Flow Ch.*?Value.*?/Value",                 //Flowの検索          =>  [Flow Ch="1"><Value>??</Value]
                    "Pressure Ch=\"1\".*?Value.*?/Value",       //Pressure1の検索     =>  [Pressure Ch="1"><Value>??</Value]
                    "Pressure Ch=\"2\".*?Value.*?/Value",       //Pressure2の検索     =>  [Pressure Ch="2"><Value>??</Value]                    
                    "Temperature Ch=\"1\".*?Value.*?/Value",    //Temparture1の検索   =>  [Temperature Ch="1"><Value>??</Value]
                    "Temperature Ch=\"2\".*?Value.*?/Value",    //Temparture2の検索   =>  [Temperature Ch="2"><Value>??</Value]
                    "Speed Ch=\"1\".*?Value.*?/Value",          //Speed(回転数)の検索 =>  [Speed Ch="1"><Value>???</Value]
                    "Alarm.*?Value.*?/Value",                   //Alarmの検索         =>  [Alarm><Value>0x000000000000000000000000000000040000000000000000</Value]
            };
            //削除する文字列の指定
            string[,] text_align =
            {
                    { "Flow Ch=\"1\"><Value>"       ,"</Value" },
                    { "Pressure Ch=\"1\"><Value>"   ,"</Value" },
                    { "Pressure Ch=\"2\"><Value>"   ,"</Value" },
                    { "Temperature Ch=\"1\"><Value>","</Value" },
                    { "Temperature Ch=\"2\"><Value>","</Value" },
                    { "Speed Ch=\"1\"><Value>"      ,"</Value" },
                    { "Alarm><Value>"               ,"</Value" },
            };

            int N = search.Length;

            for (int i = 0; i < N; i++)
            {
                MatchCollection IndexData = Regex.Matches(
                    ECMOtext,
                    search[i],
                    RegexOptions.IgnoreCase
                    );

                try
                {
                    data_text += IndexData[0].ToString().Replace(text_align[i, 0], "").Replace(text_align[i, 1], "") + ",";
                    //Alarmの部分は51文字の羅列で全てのアラームを表している可能性あり。
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); };
            }
            DateTime dt = DateTime.Now;
            data_text = dt.ToString("yyyy/MM/dd/ HH:mm:ss") + "," + data_text;
            data_text = data_text.Replace("NG", "null");

            //ECMOデータをFormに表示するための部分
            string[] data = data_text.Split(',');
            
            labelFlow.Text = data[1];
            double PI = 0.0;
            if (BSA != 0.0)
            {
                try { 
                    PI = double.Parse(data[1]) / BSA;
                    labelPI.Text = PI.ToString("F2");
                }catch
                {}
            }
            labelRPM.Text = data[6];
            labelPress.Text = data[2];
            labelPress2.Text = data[3];
            labelTemp.Text = data[4];
            labelTemp2.Text = data[5];
            //alertデータの変換
            //2023.11.14データ入力中
            //2023.11.18　仮リスト作成完了要動作確認

            data[7] = ECMO_alert(data[7]);//ただの数字の羅列のため変換のための関数ECMO_alert
            labelMessage.Text = data[7];

            //bufferの中身をリセット後buffer配列に最新の値を格納
            Array.Clear(ecmo_buff, 0, ecmo_buff.Length);
            ecmo_buff = data;
            GraphDataAdd();

            if (cntECMO % rec_interval == 0)
            {
                //rec_interval(初期値600sec)ごとにデータ登録
                string rectime = DateTime.Now.ToString("yyyy/MM/dd/ HH:mm:ss");
                string starttime = dtstart.ToString("yyyy/MM/dd/ HH:mm:ss");
                TimeSpan tspan = (DateTime.Parse(rectime) - dtstart);

                int etimesec = (int)(Math.Round(tspan.TotalSeconds));
                double etimemin = tspan.TotalMinutes;
                double etimehour = tspan.TotalHours;
                string flow = data[1];
                string rpm = data[6];
                string press1 = data[2];
                string press2 = data[3];
                string temp1 = data[4];
                string temp2 = data[5];
                string alarm = data[7];
                string gasflow = null;
                string gasconc = null;
                gasflow = labelGas.Text;
                gasconc = labelO2.Text;
                string hctemp = null;
                if (labelHC.Text != "H/C")
                    hctemp = labelHC.Text;
                string comment = null;
                db.InsertECMOdata(ECMOno, PtID, rectime, starttime,
                                flow, rpm, press1, press2, temp1, temp2, alarm, gasflow, gasconc, hctemp, comment);
            }
        }

        private string ECMO_alert(string str)
        {
            string result = "";
            string alert_text = str.Replace("0X", "");
            string[] arr = new string[12];
            for (int i = 0; i < 48; i++)
            {
                arr[i] = alert_text.Substring(i, 1);
            }
            string temp12 = arr[0];
            string pr = arr[1] + arr[2];
            string fl = arr[3];
            string fl_con = arr[17];
            string p_t_con = arr[24] + arr[25];
            string batt = arr[26];
            string lan = arr[31];
            string signal = arr[39];


            if (temp12 == "1") result += " T1高温度警告 ";
            if (temp12 == "2") result += " T2高温度警告 ";
            if (temp12 == "3") result += " T1T2高温度警告 ";
            if (temp12 == "4") result += " T1低温度警告 ";
            if (temp12 == "6") result += " T1低温T2高温度警告 ";
            if (temp12 == "8") result += " T2低温度警告 ";
            if (temp12 == "9") result += " T1高温T2低温度警告 ";
            if (temp12 == "C") result += " T1T2低温度警告 ";
            if (pr == "01") result += " P1高圧力警報";
            if (pr == "02") result += " P2高圧力警報";
            if (pr == "03") result += " P1高圧力警報,P2高圧力警報";
            if (pr == "04") result += " P1高圧力警告";
            if (pr == "06") result += " P1高圧力警告,P2高圧力警報";
            if (pr == "08") result += " P2高圧力警告";
            if (pr == "09") result += " P1高圧力警報,P2高圧力警告";
            if (pr == "0C") result += " P1高圧力警告,P2高圧力警告";
            if (pr == "10") result += " P1低圧力警報";
            if (pr == "12") result += " P1低圧力警報,P2高圧力警報"; 
            if (pr == "18") result += " P1低圧力警報,P2高圧力警告";
            if (pr == "20") result += " P2低圧力警報";
            if (pr == "21") result += " P1高圧力警報,P2低圧力警報";
            if (pr == "24") result += " P1高圧力警告,P2低圧力警報";
            if (pr == "30") result += " P1低圧力警報,P2低圧力警報";
            if (pr == "40") result += " P1低圧力警告";
            if (pr == "42") result += " P1低圧力警告,P2高圧力警報";
            if (pr == "48") result += " P1低圧力警告,P2高圧力警告";
            if (pr == "60") result += " P1低圧力警告,P2低圧力警報";
            if (pr == "80") result += " P2低圧力警告";
            if (pr == "81") result += " P1高圧力警報,P2低圧力警告";
            if (pr == "84") result += " P1高圧力警告,P2低圧力警告";
            if (pr == "90") result += " P1低圧力警報,P2低圧力警告";
            if (pr == "C0") result += " P1低圧力警告,P2低圧力警告";
            if (fl == "1") result += " 気泡検出 ";
            if (fl == "2") result += " 低流量 ";
            if (fl == "4") result += " 高流量 ";
            if (fl == "8") result += " 逆流 ";
            if (fl_con == "2") result += " 流量センサ未接続";
            if (p_t_con == "02") result += " P1未接続";
            if (p_t_con == "04") result += " P2未接続";
            if (p_t_con == "08") result += " T1未接続";
            if (p_t_con == "10") result += " T2未接続";
            if (p_t_con == "1A") result += " P1,T1,T2未接続";
            if (p_t_con == "1C") result += " P2,T1,T2未接続";
            if (p_t_con == "1E") result += " P1,P2,T1,T2未接続";
            if (p_t_con == "06") result += " P1,P2未接続";
            if (p_t_con == "0A") result += " P1,T1未接続";
            if (p_t_con == "12") result += " P1,T2未接続";
            if (p_t_con == "0C") result += " P2,T1未接続";
            if (p_t_con == "14") result += " P2,T2未接続";
            if (p_t_con == "18") result += " T1,T2未接続";
            if (p_t_con == "16") result += " P1,P2,T2未接続";
            if (p_t_con == "0E") result += " P1,P2,T1未接続";
            if (batt == "1") result += " バッテリ駆動中";
            if (lan == "4") result += " Lan接続エラー";
            if (signal == "4") result += " 流量信号不安定";


            //結果が空欄でなければ結果を出力、空欄の場合は受けたデータをそのまま出力
            if (result == "") return str;
            else return result;
        }
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――


        //toolStripMenuクリックイベント
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        private void console1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //console1に接続する場合
            labelECMOsetting.Text = "console1 IP:192.168.254.253 Port:8001";
        }

        private void console2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //console2に接続する場合
            labelECMOsetting.Text = "console2 IP:192.168.254.202 Port:8002";
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            labelMonitorsetting.Text = "ICU201 IP:192.168.254.252 Port:7998";
        }

        private void 患者一覧へToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //患者一覧へ戻るボタンを追加

            string text = "";
            for(int i = 0; i < Application.OpenForms.Count; i++)
            {
                Form f = Application.OpenForms[i];
                
                text = f.Name;
                if (text != "PatientForm")
                {
                    f.Hide();
                }
                else
                {
                    f.Show();
                }

            }


        }
        private void 閉じるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            labelMonitorsetting.Text = "ICU201 IP:192.168.254.212 Port:7998";
        }

        private void buttonConnectSetting_Click(object sender, EventArgs e)
        {
            //ECMO、周辺機器接続のためのダイアログを表示させる
            //IPアドレスなどの設定値はSQLiteを通して各フォームで連携する

        }

        private void 患者情報編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientInfomation formPtInf = new FormPatientInfomation(PtID, ECMOno);
            formPtInf.Show();
        }
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――





        //CDI関係
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        private void CDIconnect_Click(object sender, EventArgs e)
        {
            if (portCDI.IsOpen == false)
            {
                try
                {
                    portCDI.PortName = COMCDI;
                    portCDI.BaudRate = 9600;
                    portCDI.Parity = Parity.None;
                    portCDI.DataBits = 8;
                    portCDI.StopBits = StopBits.One;
                    portCDI.ReadTimeout = 6000;
                    portCDI.Open();
                    pictureBoxCDI.Image = Image.FromFile(connectingFilePath);
                    //データの空読み込み
                    try
                    {
                        //空読み込みが成功したらタイマースタート
                        string temp = portCDI.ReadLine();
                        labelCDI.Text = "CDI:Connected";
                        pictureBoxCDI.Image = Image.FromFile(connectFilePath);
                        timerCDI.Enabled = true;
                        timerCDI.Start();
                    }
                    catch (Exception ex)
                    {
                        portCDI.Close();
                        MessageBox.Show(ex.Message);
                        labelCDI.Text = "CDI:Disconnected";
                        pictureBoxCDI.Image = Image.FromFile(disconnectFilePath);
                    }
                }
                catch
                {
                    MessageBox.Show("接続出来ませんでした");
                    labelCDI.Text = "CDI:Disconnected";
                    pictureBoxCDI.Image = Image.FromFile(disconnectFilePath);
                    timerCDI.Stop();
                }
            }
            else
            {
                labelCDI.Text = "CDI:Disconnected";
                portCDI.Close(); pictureBoxCDI.Image = Image.FromFile(disconnectFilePath);
            }

        }
        private async void timerCDI_Tick(object sender, EventArgs e)
        {
            string rcv = "";
            if (portCDI.IsOpen)
            {
                //一度バッファ内のデータをクリアする
                Array.Clear(CDI_buff, 0, CDI_buff.Length);
                try
                {
                    rcv = portCDI.ReadLine();
                    //CDIデータの整理
                    string[] CDIraw = rcv.Split('\t');
                    string[] CDI = new string[12];

                    if (rcv != "")
                    {
                        CDI[0] = System.DateTime.Now.ToString("yyyy/MM/dd/ HH:mm:ss");//日付
                        CDI[1] = "CDI";//採血部位
                        //pH
                        if (!CDIraw[1].Contains("-.--"))
                            CDI[2] = CDIraw[1].Substring(4);
                        else if (!CDIraw[10].Contains("-.--"))
                            CDI[2] = CDIraw[10].Substring(4);
                        else
                            CDI[2] = "null";
                        //PCO2
                        if (!CDIraw[2].Contains("---"))
                            CDI[3] = CDIraw[2].Substring(4);
                        else if (!CDIraw[11].Contains("---"))
                            CDI[3] = CDIraw[11].Substring(4);
                        else
                            CDI[3] = "null";
                        //PO2
                        if (!CDIraw[3].Contains("---"))
                            CDI[4] = CDIraw[3].Substring(4);
                        else if (!CDIraw[12].Contains("---"))
                            CDI[4] = CDIraw[12].Substring(4);
                        else
                            CDI[4] = "null";
                        //temp
                        if (!CDIraw[4].Contains("--.-"))
                            CDI[5] = CDIraw[4].Substring(4);
                        else if (!CDIraw[13].Contains("--.-"))
                            CDI[5] = CDIraw[13].Substring(4);
                        else
                            CDI[5] = "null";
                        //HCO3
                        if (!CDIraw[6].Contains("--"))
                            CDI[6] = CDIraw[6].Substring(4);
                        else if (!CDIraw[15].Contains("--"))
                            CDI[6] = CDIraw[15].Substring(4);
                        else
                            CDI[6] = "null";
                        //BE
                        if (!CDIraw[7].Contains("--"))
                            CDI[7] = CDIraw[7].Substring(4);
                        else if (!CDIraw[16].Contains("--"))
                            CDI[7] = CDIraw[16].Substring(4);
                        else
                            CDI[7] = "null";
                        //HCT
                        if (!CDIraw[17].Contains("--"))
                            CDI[8] = CDIraw[17].Substring(4);
                        else
                            CDI[8] = "null";
                        //Hgb
                        if (!CDIraw[18].Contains("-.-"))
                            CDI[9] = CDIraw[18].Substring(4);
                        else
                            CDI[9] = "null";
                        //SO2
                        if (!CDIraw[5].Contains("---"))
                            CDI[10] = int.Parse(CDIraw[5].Substring(4)).ToString();
                        else if (!CDIraw[14].Contains("---"))
                            CDI[10] = int.Parse(CDIraw[14].Substring(4)).ToString();
                        else
                            CDI[10] = "null";
                        //K
                        if (!CDIraw[8].Contains("-.-"))
                            CDI[11] = CDIraw[8].Substring(4);
                        else
                            CDI[11] = "null";
                        CDI_buff = CDI;
                    }
                    //CDIにデータをMainに表示させる為の部分
                    labelPO2.Text = CDI[4];
                    labelPCO2.Text = CDI[3];
                    labelSaO2.Text = CDI[10];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    portCDI.Close();
                }


            }
            else
            {
                //警告
                MessageBox.Show(COMCDI + "を確認してください");
            }

        }
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――





        //グラフ描画関連
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        private void GraphDataAdd()
        {
            //データリストに追加するためのメソッド

            //グラフにするのは「流量、回転数、圧力1」、「ABP、CVP、HR、SpO2」、「PaO2、PaCO2、Hb」
            //「流量、回転数、圧力1」  double x_ECMO
            //「ABP、CVP、HR、SpO2」   double x_vital
            //Hb,PaO2,PaCO2            double x_CDI  

            //データ数をカウントして最大4000程度にとどめる（1サンプル当たり5分程度で20000分）

            //bufferに記録されているデータをリストに追加する あまりにも時間がかけ離れている場合はスルー
            //共通パラメータ
            System.DateTime dtnow;
            //ECMO用パラメータ
            bool ECMOflag = true;
            System.DateTime dtCommon;
            double ecmo_time;
            double ecmo_flow;
            double ecmo_rpm;
            double ecmo_press;
            //Vital用パラメータ
            bool Vitalflag = true;
            double vital_time;
            double abp;
            double cvp;
            double hr;
            double spo2;
            //CDI用パラメータ
            bool CDIflag = true;
            double CDI_time;
            double hb;
            double pao2;
            double paco2;
            //ECMO日付データ処理
            try
            {
                //datetimeに変換出来ない場合はフラグをfalseにして処理をスキップさせる
                dtCommon = DateTime.Parse(ecmo_buff[0]);
            }
            catch
            {
                ECMOflag = false;
                dtCommon = DateTime.Now;//仮で現在日時を登録しておく
            }

            if (ECMOflag)
            {
                dtnow = DateTime.Now;
                var time_span = dtnow - dtCommon;
                if (time_span.Minutes <= 1)
                {
                    //1分以内だった場合データを確認して格納用の変数に代入していく
                    ecmo_time = dtCommon.ToOADate();

                    //一度全てのデータをtemp配列にdoubleでキャストし、エラー発生時はdouble.NaNを代入する
                    //rpmのみ、グラフ描画の関係でkrpmに変換
                    //ecmo_buff[6] = (int.Parse(ecmo_buff[6]) / 1000).ToString();
                    double[] temp = new double[7];
                    for (int i = 0; i < 7; i++)
                    {
                        try
                        {
                            temp[i] = double.Parse(ecmo_buff[i]);
                        }
                        catch
                        {
                            temp[i] = double.NaN;
                        }
                    }
                    ecmo_flow = temp[1];
                    ecmo_rpm = temp[6];
                    ecmo_press = temp[2];

                    //上記で確認したデータをListに格納する
                    classECMO[] ECMOdata = new classECMO[]{
                        new classECMO(ecmo_time,ecmo_flow,ecmo_rpm,ecmo_press)
                    };
                    ECMOdataList.AddRange(ECMOdata);
                    //追加したデータをソート
                    ECMOdataList.Sort((a, b) => a.time.CompareTo(b.time));
                }
            }

            //CDI日付データ処理
            try
            {
                //datetimeに変換出来ない場合はフラグをfalseにして処理をスキップさせる
                DateTime.Parse(CDI_buff[10]);
            }
            catch
            {
                CDIflag = false;
            }

            if (CDIflag)
            {
                dtnow = DateTime.Now;
                var time_span = dtnow - DateTime.Parse(CDI_buff[10]); ;
                if (time_span.Minutes <= 1)
                {
                    //1分以内だった場合データを確認して格納用の変数に代入していく

                    //一度全てのデータをtemp配列にdoubleでキャストし、エラー発生時はdouble.NaNを代入する
                    double[] temp = new double[10];
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            temp[i] = double.Parse(CDI_buff[i]);
                        }
                        catch
                        {
                            temp[i] = double.NaN;
                        }
                    }
                    pao2 = temp[2];
                    paco2 = temp[1];
                    hb = temp[7];
                    CDI_time = dtCommon.ToOADate();
                    //上記で確認したデータをListに格納する
                    classCDI[] CDIdata = new classCDI[]{
                        new classCDI(CDI_time,hb,pao2,paco2)
                    };
                    CDIdataList.AddRange(CDIdata);
                    //追加したデータをソート
                    CDIdataList.Sort((a, b) => a.time.CompareTo(b.time));
                }

            }

            //vital日付データ処理
            try
            {
                //datetimeに変換出来ない場合はフラグをfalseにして処理をスキップさせる
                DateTime.Parse(vital_buff[0]);
            }
            catch
            {
                Vitalflag = false;
            }

            if (Vitalflag)
            {
                //3min以上空いていたらスキップ
                dtnow = DateTime.Now;
                var time_span = dtnow - DateTime.Parse(vital_buff[0]); ;
                if (time_span.Minutes <= 2)
                {
                    //3分以内だった場合データを確認して格納用の変数に代入していく

                    //一度全てのデータをtemp配列にdoubleでキャストし、エラー発生時はdouble.NaNを代入する
                    double[] temp = new double[6];
                    for (int i = 0; i < 6; i++)
                    {
                        try
                        {
                            temp[i] = double.Parse(vital_buff[i]);
                        }
                        catch
                        {
                            temp[i] = double.NaN;
                        }
                    }
                    hr = temp[1];
                    abp = temp[2];
                    //pap = temp[3];
                    cvp = temp[4];
                    spo2 = temp[5];//ここでエラー　インデックス境界外
                    vital_time = dtCommon.ToOADate();
                    //上記で確認したデータをListに格納する
                    classVital[] VitalData = new classVital[]{
                        new classVital(vital_time,hr,abp,cvp,spo2)
                    };
                    VitaldataList.AddRange(VitalData);
                    //追加したデータをソート
                    VitaldataList.Sort((a, b) => a.time.CompareTo(b.time));
                }

            }
            GraphDraw();
        }
        private void GraphDataAdd_SQL()
        {
            //一度カスタムListをリセットしてSQLからデータリストに追加するためのメソッド
            //SQL側で全てdoubleにデータをセットしているため、こちらではただ代入処理を行っていく

            //カスタムリストのリセット
            ECMOdataList.Clear();
            CDIdataList.Clear();
            VitaldataList.Clear();

            //SQLデータを受け取るためのListを宣言
            //ECMO用
            List<double> sql_time = new List<double>();
            List<double> sql_flow = new List<double>();
            List<double> sql_rpm = new List<double>();
            List<double> sql_press = new List<double>();
            //Vital用
            List<double> sql_vital_time = new List<double>();
            List<double> sql_vital_abp = new List<double>();
            List<double> sql_vital_cvp = new List<double>();
            List<double> sql_vital_hr = new List<double>();
            List<double> sql_vital_spo2 = new List<double>();
            //CDI用
            List<double> sql_CDI_time = new List<double>();
            List<double> sql_CDI_pao2 = new List<double>();
            List<double> sql_CDI_paco2 = new List<double>();
            List<double> sql_CDI_hb = new List<double>();


            //グラフにするのは「流量、回転数、圧力1」、「ABP、CVP、HR、SpO2」、「PaO2、PaCO2、Hb」

            //引数データをリストに追加する
            //ひとまずECMOデータ
            db.Output_ECMO_data(PtID, ECMOno, out sql_time, out sql_flow, out sql_rpm, out sql_press);
            if (sql_time.Count == 0)
                MessageBox.Show("グラフデータがありません");
            else
            {
                for (int i = 0; i < sql_time.Count; i++)
                {
                    //上記で確認したデータをListに格納する
                    classECMO[] ECMOdata = new classECMO[]{
                        new classECMO(sql_time[i],sql_flow[i],sql_rpm[i],sql_press[i])
                    };
                    ECMOdataList.AddRange(ECMOdata);
                }
            }

            //追加したデータをソート
            ECMOdataList.Sort((a, b) => a.time.CompareTo(b.time));
            GraphDraw();

            //ここまで確認
            return;

            //ECMO用パラメータ
            bool ECMOflag = true;
            System.DateTime dtCommon;
            double ecmo_time;
            double ecmo_flow;
            double ecmo_rpm;
            double ecmo_press;
            //Vital用パラメータ
            bool Vitalflag = true;
            double vital_time;
            double abp;
            double cvp;
            double hr;
            double spo2;
            //CDI用パラメータ
            bool CDIflag = true;
            double CDI_time;
            double hb;
            double pao2;
            double paco2;
            //ECMO日付データ処理
            try
            {
                //datetimeに変換出来ない場合はフラグをfalseにして処理をスキップさせる
                dtCommon = DateTime.Parse(ecmo_buff[0]);
            }
            catch
            {
                ECMOflag = false;
                dtCommon = DateTime.Now;//仮で現在日時を登録しておく
            }

            if (ECMOflag)
            {
                //rpmのみ、グラフ描画の関係でkrpmに変換
                ecmo_buff[6] = (int.Parse(ecmo_buff[6]) / 1000).ToString();
                double[] temp = new double[7];
                for (int i = 0; i < 7; i++)
                {
                    try
                    {
                        temp[i] = double.Parse(ecmo_buff[i]);
                    }
                    catch
                    {
                        temp[i] = double.NaN;
                    }
                }
                ecmo_flow = temp[1];
                ecmo_rpm = temp[6];
                ecmo_press = temp[2];

                //上記で確認したデータをListに格納する
                classECMO[] ECMOdata = new classECMO[]{
                        new classECMO(ecmo_time,ecmo_flow,ecmo_rpm,ecmo_press)
                    };
                ECMOdataList.AddRange(ECMOdata);
                //追加したデータをソート
                ECMOdataList.Sort((a, b) => a.time.CompareTo(b.time));
            }
        }
    
        [Obsolete]
        private void GraphDraw()
        {
            //data set はList配列に入れておき、最後に配列変換すればよさそうか動作速度的には毎回変換するのではない方がよさそうだけれども

            //listからECMOデータを取り出す　取り出す際には時間でソートしてから
            ECMOdataList.Sort((a, b) => a.time.CompareTo(b.time));
            double[] ecmo_time = ECMOdataList.Select(e => e.time).ToArray();
            double[] ecmo_flow = ECMOdataList.Select(e => e.flow).ToArray();
            double[] ecmo_rpm = ECMOdataList.Select(e => e.rpm).ToArray();
            double[] ecmo_press = ECMOdataList.Select(e => e.press).ToArray();


            trendPlot.Plot.Clear();
            trendPlot.Plot.Title("trend");
            trendPlot.Plot.XLabel("時間");
            trendPlot.Plot.YLabel("値");
            //trendPlot.Plot.AddScatter(pointX, pointY,label:"sample").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
            if(ECMOdataList.Count != 0)
            {
                //flowデータがあれば追加
                try
                {
                    var trendFlow = trendPlot.Plot.PlotScatter(ecmo_time.ToArray(), ecmo_flow.ToArray(), label: "Flow", color: Color.Blue);
                    trendFlow.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
                    trendFlow.YAxisIndex = 1;
/*                    trendPlot.Plot.PlotScatter(ecmo_time.ToArray(), ecmo_flow.ToArray(), label: "Flow", color: Color.Blue).OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
                    trendPlot.Plot.PlotScatter(ecmo_time.ToArray(), ecmo_flow.ToArray(), label: "Flow", color: Color.Blue).YAxisIndex = 1;
*/                }
                catch { }
                //rpmデータがあれば追加
                try
                {
                    var trendRPM = trendPlot.Plot.PlotScatter(ecmo_time.ToArray(), ecmo_rpm.ToArray(), label: "RPM", color: Color.Orange);
                    trendRPM.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
                    trendRPM.YAxisIndex = 0;
                }
                catch { }
                //pressデータがあれば追加
                try
                {
                    var trendPress = trendPlot.Plot.PlotScatter(ecmo_time.ToArray(), ecmo_press.ToArray(), label: "Press ch1", color: Color.Red);
                    trendPress.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
                    trendPress.YAxisIndex = 0;
                }
                catch { }
            }

            trendPlot.Plot.XAxis.DateTimeFormat(true);
            trendPlot.Plot.YAxis2.Ticks(true);
            trendPlot.Plot.Legend(false);
            trendPlot.Plot.AxisAuto();
            trendPlot.Plot.SetOuterViewLimits(xMin: double.Parse("1234"));
            trendPlot.Refresh();
            trendPlot.Render();
        }
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――


        //main画面更新関係
        private void ReLoad_Main()
        {

            if (PtID == 0)
            {
                return;
            }
            //メイン画面の再描画メソッド
            //timerイベントとAcivateイベントで呼び出される

            //medicineの更新 最新のデータで置換 空白だったら何もしない
            string medicine_rcv = db.LatestMedication(PtID,ECMOno);
            string[] medicin = medicine_rcv.Split(',');
            if(medicine_rcv != "")
            {
                labelHeparin.Text = medicin[0];
                labelNAD.Text = medicin[1];
                labelDOA.Text = medicin[2];
                labelDOB.Text = medicin[3];
            }


            //history更新 最新のコメントあり履歴一覧を表示
            dataGridViewHistory.DataSource = db.Historyt(PtID, ECMOno);
            dataGridViewHistory.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //DO2更新 必要なパラメータがあるか確認して表示
            //Hb、CO、SaO2が分かれば計算可能

            //BSAの更新 必要なパラメータがあるか確認して表示
            string BSAinf = db.SearchPtInfo(PtID,ECMOno,"BSA");
            if(BSAinf != "")
            {
                BSA = double.Parse(BSAinf);
            }
            
            //



            //ガス流量や酸素濃度といった手書き入力されたデータを表示
            string gas_flow;
            string O2;
            string HC;
            db.Output_ECMO_data_main(PtID,ECMOno,out gas_flow,out O2,out HC);
            if (gas_flow != "")
            {
                labelGas.Text = $"{gas_flow}" ;
            }
            if(O2 != "")
            {
                labelO2.Text = $"{O2}";
            }
            if(HC != "")
            {
                labelHC.Text = $"{HC}";
            }

            //操作再開した場合のグラフ描画と各種リストへの入力
            //まずECMOから
            string ECMOrcv = "";




        }

        private void labelACT_Click(object sender, EventArgs e)
        {
            //ACTの値を更新するためのボタン
            string text = "ACTの値を入力しますか？";
            DialogResult result = MessageBox.Show(text, "メッセージ",
                MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                string temp = "";
                string mess = "ACT";
                bool flag = true;
                while (flag)
                {
                    //入力形式がintに変換出来ないものは再入力をさせるようなループ構造
                    temp = (Microsoft.VisualBasic.Interaction.InputBox(mess));
                    try
                    {
                        int.Parse(temp);
                        flag = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        mess = text.Replace("(再入力)", "") + "(再入力)";
                        flag = true;
                    }
                }
                labelACT.Text = temp;
                DateTime dt = DateTime.Now;
                labelACTtime.Text = dt.ToString("HH:mm");
                //SQLへACTの値を登録する
                db.InserACT(PtID, ECMOno, int.Parse(temp), dt.ToString());
            }
            else
            {
                //一つ前の値をSQLから検索して表示させる?
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //経過時間表示用のタイマー

            DateTime dt1 = DateTime.Now;
            TimeSpan tspan = (dt1 - dtstart);
            labelElapsedTime.Text = tspan.Days.ToString("0") + "日" + tspan.Hours.ToString("00") + "時間" + tspan.Minutes.ToString("00") + "分" + tspan.Seconds.ToString("00") + "秒";

            //PI計算に必要な患者データなどがそろっているのか判断して自動更新させるような関数を加える

            //時刻によってデータ取得を実施する
            //10秒ごとに更新判断
            if (DateTime.Now.Second % 10 == 0)
            {
                ReLoad_Main();
            }
        }
        private void labelStartDateTime_Click(object sender, EventArgs e)
        {
            //開始日時の更新を確認して、SQLデータベースへアクセス
            //更新しますかメッセージ出してSQLからデータ抽出し、もし無ければログ入力を促すように
            string text = "開始日時を更新しますか？";
            DialogResult result = MessageBox.Show(text, "更新",
                MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //”ECMO START”ログがある場合は読み込む 無ければ警告
                try
                {
                    dtstart = DateTime.Parse(db.SearchStartTime(PtID, ECMOno));
                    labelStartDateTime.Text = dtstart.ToString();
                    //SQLite操作で全てのスタートタイムを変更するとともに、elapsedtimeの更新も行う

                }
                catch
                {
                    MessageBox.Show("記録が見つかりません。開始ログを入力して下さい");
                }
            }

        }

        private void FormRecord_Activated(object sender, EventArgs e)
        {
            ReLoad_Main();

            if (drawFlag)
            {
                GraphDataAdd_SQL();
                drawFlag = false;
            }
        }

        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        //履歴関連
        private void DataGridViewHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            //インデックス列をクリックしてしまった場合の処理
            if (e.ColumnIndex == -1)
                return;

            if (dgv.Columns[e.ColumnIndex].Name == "detail")
            {

                
                string d = "";
                try
                {
                    d = dgv[1, e.RowIndex].Value.ToString();
                    DateTime.Parse(d);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    d = "";
                }

                if(d!= "")
                {
                    //日付の取得ができたらメッセージで詳細ページに行くかどうかを確認する
                    string m = "詳細を開きますか？";
                    DialogResult result = MessageBox.Show(m, "メッセージ",
                        MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        MessageBox.Show($"ID:{PtID},ECMOno:{ECMOno},日付:{d}");

                    }
                }

            }
        }
        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――


        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        //動作確認用メソッド

        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――

        //――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――
        //フォント表示のレスポンシブ設定(可能であれば)
        //4px = 3pt
        private void FormRecord_SizeChanged(object sender, EventArgs e)
        {
            //ラベルACTの横幅取得して表示する
            //MessageBox.Show(labelACT.Width.ToString()+"px");
            //80px:12pt 16px

            //フォントサイズの更新設定
            int fontSize = 4 * labelACT.Width / 20;
            int fontSize2 = fontSize / 2;
            //最小化された場合の処理
            if(fontSize == 0)
            {
                fontSize = 1;
                fontSize2 = 1;
            }
            //採血データの更新
            labelACT.Font = new Font("MS UI Gothic", fontSize);
            labelnameACT.Font = new Font("MS UI Gothic", fontSize2);
            labelPO2.Font = new Font("MS UI Gothic", fontSize);
            labelnamePO2.Font = new Font("MS UI Gothic", fontSize2);
            labelPCO2.Font = new Font("MS UI Gothic", fontSize);
            labelnamePCO2.Font = new Font("MS UI Gothic", fontSize2);
            labelSaO2.Font = new Font("MS UI Gothic", fontSize);
            labelnameSaO2.Font = new Font("MS UI Gothic", fontSize2);
            //バイタルデータの更新
            labelHR.Font = new Font("MS UI Gothic", fontSize);
            labelnameHR.Font = new Font("MS UI Gothic", fontSize2);
            labelABP.Font = new Font("MS UI Gothic", fontSize);
            labelnameABP.Font = new Font("MS UI Gothic", fontSize2);
            labelPAP.Font = new Font("MS UI Gothic", fontSize);
            labelnamePAP.Font = new Font("MS UI Gothic", fontSize2);
            labelCVP.Font = new Font("MS UI Gothic", fontSize);
            labelnameCVP.Font = new Font("MS UI Gothic", fontSize2);
            //点滴データの更新
            labelHeparin.Font = new Font("MS UI Gothic", fontSize);
            labelnameHeparin.Font = new Font("MS UI Gothic", fontSize2);
            labelNAD.Font = new Font("MS UI Gothic", fontSize);
            labelnameNAD.Font = new Font("MS UI Gothic", fontSize2);
            labelDOA.Font = new Font("MS UI Gothic", fontSize);
            labelnameDOA.Font = new Font("MS UI Gothic", fontSize2);
            labelDOB.Font = new Font("MS UI Gothic", fontSize);
            labelnameDOB.Font = new Font("MS UI Gothic", fontSize2);


            //tabデータtopの更新
            labelFlow.Font = new Font("MS UI Gothic", fontSize);
            labelnameFlow.Font = new Font("MS UI Gothic", fontSize2);
            labelPI.Font = new Font("MS UI Gothic", fontSize);
            labelnamePI.Font = new Font("MS UI Gothic", fontSize2);
            labelRPM.Font = new Font("MS UI Gothic", fontSize);
            labelnameRPM.Font = new Font("MS UI Gothic", fontSize2);
            labelPress.Font = new Font("MS UI Gothic", fontSize);
            labelnamePress.Font = new Font("MS UI Gothic", fontSize2);
            labelPress2.Font = new Font("MS UI Gothic", fontSize);
            labelnamePress2.Font = new Font("MS UI Gothic", fontSize2);
            labelInOut.Font = new Font("MS UI Gothic", fontSize);
            labelTemp.Font = new Font("MS UI Gothic", fontSize);
            labelnameTemp.Font = new Font("MS UI Gothic", fontSize2);
            labelTemp2.Font = new Font("MS UI Gothic", fontSize);
            labelnameTemp2.Font = new Font("MS UI Gothic", fontSize2);
            labelHC.Font = new Font("MS UI Gothic", fontSize);
            labelnameHC.Font = new Font("MS UI Gothic", fontSize2);
            labelGas.Font = new Font("MS UI Gothic", fontSize);
            labelnameGas.Font = new Font("MS UI Gothic", fontSize2);
            labelO2.Font = new Font("MS UI Gothic", fontSize);
            labelnameO2.Font = new Font("MS UI Gothic", fontSize2);
            labelDO2.Font = new Font("MS UI Gothic", fontSize);


        }


        //―――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――

    }
}
