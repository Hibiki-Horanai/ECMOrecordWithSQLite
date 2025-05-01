using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using ScottPlot.Drawing.Colormaps;
using static ScottPlot.Generate;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Net;
using ScottPlot.Statistics;
using System.Security.Claims;
using System.Windows.Media.Animation;
using Dapper;

namespace ECMOrecordWithSQLite
{
    internal class ClassSQLite
    {
        //接続データベース、入力情報文字列、実行関数などを与えるとデータベースとやり取りをしてくれる        
        public void ExecuteNonQuery(string query)
        {
            //SQLの実行関数
            try
            {
                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                using (var command = con.CreateCommand())
                {
                    //データベース接続
                    con.Open();

                    //コマンド実行処理
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message+"LOGIC");
            }
        }

        //pass word関係
        //ここから--------------------------------------------------------------------------------------------------------------------------
        public void CreatePWTable()
        {

            //PASSWORD tableがなければ作成
            //no(primary key),word,dateの項目を作成

            StringBuilder query = new StringBuilder();
            query.Clear();
            query.Append("CREATE TABLE IF NOT EXISTS PASSWORD (");
            query.Append("NO INTEGER NOT NULL");
            query.Append(", WORD TEXT NOT NULL");
            query.Append(", DATETIME TEXT NOT NULL");
            query.Append(",primary key (NO)");
            query.Append(")");
            ExecuteNonQuery(query.ToString());
        }

        public void InsertPassWord(string pw,string date)
        {
            //pwとdateを受け取りデータベースへ登録する
            //パスワードは4文字以上半角英数字の組み合わせ、大文字小文字の区別無

            if (pw.Length < 4)
            {
                MessageBox.Show("パスワードは4文字以上の半角英数字の組み合わせで登録して下さい。");
                return;
            }

            //登録用クエリ―
            var query = "INSERT INTO PASSWORD (WORD,DATETIME) VALUES(" + $"{pw},'{date}')";
            //クエリ―実行
            ExecuteNonQuery(query.ToString());
        }

        public string GetPass()
        {

            CreatePWTable();
            string text = "";
            //登録されているパスワードの取得
            try
            {

                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    string sql = "SELECT WORD FROM PASSWORD ORDER BY NO DESC";
                    SQLiteCommand command = new SQLiteCommand(sql,con);

                    using (var reader = command.ExecuteReader())
                    {
                        {
                            reader.Read();
                            text += (string)reader["WORD"];
                        }
                    }
                }
            }
            catch 
            {
                MessageBox.Show("パスワードは未設定のため初期値です。");
                InsertPassWord("2151", System.DateTime.Now.ToString());
            }
            return text;
        }
        //ここまで--------------------------------------------------------------------------------------------------------------------------

        //患者情報
        //ここから--------------------------------------------------------------------------------------------------------------------------
        public DataTable PatientList()
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection con = new SQLiteConnection("Data Source=myDataBase.sqlite"))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM PATIENTLIST", con))
            {
                StringBuilder query = new StringBuilder();
                query.Clear();
                query.Append("CREATE TABLE IF NOT EXISTS PATIENTLIST (");
                query.Append(" NO INTEGER NOT NULL");  //登録番号
                query.Append(", ID INTEGER NOT NULL");  //患者ID
                query.Append(", NAME TEXT");            //患者氏名
                query.Append(", HEIGHT REAL");          //身長cm
                query.Append(", WEIGHT REAL");          //体重kg
                query.Append(", BSA REAL");             //BSA
                query.Append(", GENDER TEXT");          //性別
                query.Append(", BIRTHDAY TEXT");        //生年月日
                query.Append(", AGE INTEGER");          //年齢
                query.Append(", BLOODTYPE TEXT");       //血液型
                query.Append(", primary key (NO)");     //主キー
                query.Append(")");

                ExecuteNonQuery(query.ToString());
                try
                {
                    adapter.Fill(dt);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            return dt;
            
        }

        public bool InsertPtInf(int id, string name, double height, double weight, double bsa, string gender, string birthday, int age, string bloodtype)
        {
            bool flag = true;
            if (JudgeID(id) == id.ToString())
            {
                DialogResult result = MessageBox.Show("同一IDが存在します。続けますか?", "確認", MessageBoxButtons.OKCancel);
                if (result != DialogResult.OK)
                {
                    flag = false;
                    return flag;
                }
            }
            var query = "INSERT INTO PATIENTLIST (ID,NAME,HEIGHT,WEIGHT,BSA,GENDER,BIRTHDAY,AGE,BLOODTYPE) VALUES(" + $"{id},'{name}',{height},{weight},{bsa},'{gender}','{birthday}',{age},'{bloodtype}')";
            ExecuteNonQuery(query.ToString());
            MessageBox.Show("登録しました。");
            return flag;
        }

        public string JudgeID(int id)
        {
            string text = "";
            //引数としてidを受け取り、すでに登録されているかどうか判定
            try
            {

                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    string sql = $"SELECT ID FROM PATIENTLIST WHERE ID={id.ToString()}";
                    SQLiteCommand command = new SQLiteCommand(sql, con);

                    using (var reader = command.ExecuteReader())
                    {
                        {
                            reader.Read();
                            text += reader["ID"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                text = e.Message;
            }
            return text;
        }

        public void DataUpLoad(DataTable dt)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=myDataBase.sqlite"))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM PATIENTLIST", con))
            {
                SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter);
                try
                {
                    adapter.Update(dt);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public bool SearchNOID(int no,int id)
        {
            bool flag = false;
            //NoとIDの組み合わせでECMO記録データが存在しているかどうか
            try
            {

                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    //ここのSQL文はECMOテーブルができてから変更になるので、仮で記載
                    string sql = $"SELECT * FROM ECMORECORD WHERE NO={no} AND ID={id}";
                    SQLiteCommand command = new SQLiteCommand(sql, con);

                    using (var reader = command.ExecuteReader())
                    {
                        {
                            flag = reader.Read();//一つでもレコードが存在していればtrueとなり継続記録
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return flag;
        }

        public string SearchPtInfo(int id, int no, string name)
        {
            string text = "";
            try
            {
                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    string sql = $"SELECT * FROM PATIENTLIST WHERE NO={no} AND ID={id}";
                    SQLiteCommand command = new SQLiteCommand(sql, con);

                    using (var reader = command.ExecuteReader())
                    {
                        {
                            if (reader.Read())//レコードが存在していれば読み取り
                            {
                                text = reader[$"{name}"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "PtInfo検索でエラー");
            }

            return text;
        }

        public string[] PatientInfo(int id, int no)
        {
            //PatientInfomationに表示させるための患者データ検索
            string[] text = new string[8];
            try
            {
                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    string sql = $"SELECT * FROM PATIENTLIST WHERE NO={no} AND ID={id}";
                    SQLiteCommand command = new SQLiteCommand(sql, con);

                    using (var reader = command.ExecuteReader())
                    {
                        {
                            if (reader.Read())//レコードが存在していれば読み取り
                            {
                                text[0] = reader["NAME"].ToString();
                                text[1] = reader["HEIGHT"].ToString();
                                text[2] = reader["WEIGHT"].ToString();
                                text[3] = reader["BSA"].ToString();
                                text[4] = reader["GENDER"].ToString();
                                text[5] = reader["BIRTHDAY"].ToString();
                                text[6] = reader["AGE"].ToString();
                                text[7] = reader["BLOODTYPE"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "PtInfo検索でエラー");
            }

            return text;
        }

        public void PtInf_update(string[] data)
        {

        }
        //ここまで--------------------------------------------------------------------------------------------------------------------------



        public void CreateInOut(int id, int no)
        {
            //INOUT tableが無ければ作成
            //id,ecmono,date,InOut
            string table_name = $"INOUT_{no}-{id}";
            StringBuilder query = new StringBuilder();
            query.Clear();
            query.Append($"CREATE TABLE IF NOT EXISTS '{table_name}' (");
            query.Append("NO INTEGER NOT NULL");
            query.Append(", ID INTEGER NOT NULL");
            query.Append(", DATETIME TEXT NOT NULL");
            query.Append(", NAME TEXT NOT NULL");
            query.Append(", INOUT INTEGER NOT NULL");
            query.Append(", TOTALINOUT INTEGER NOT NULL");
            query.Append(")");
            ExecuteNonQuery(query.ToString());
        }

        public void InsertInOut(int ecmono,int id,string name ,int volume)
        {
            string table_name = $"INOUT_{ecmono}-{id}";
            //現在の値をデータベースに登録
            System.DateTime dateTime = System.DateTime.Now;
            string dt = dateTime.ToString("yyyy/MM/dd/ HH:mm:ss");

            //一つ前のTOTALINNOUTの値を呼び出す
            int prev_vol=0;
            try
            {
                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    string sql = $"SELECT TOTALINOUT FROM '{table_name}' WHERE NO={ecmono} AND ID={id} ORDER BY DATETIME DESC";
                    SQLiteCommand command = new SQLiteCommand(sql, con);

                    using (var reader = command.ExecuteReader())
                    {
                        {
                            if (reader.Read())//レコードが存在していれば読み取り
                            {
                                prev_vol = int.Parse(reader["TOTALINOUT"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            int total = prev_vol + volume;
            //登録用クエリ―
            var query = $"INSERT INTO '{table_name}' (NO,ID,DATETIME,NAME,INOUT,TOTALINOUT) VALUES(" + $"{ecmono},{id},'{dt}','{name}',{volume},{total})";
            //クエリ―実行
            ExecuteNonQuery(query.ToString());

        }

        public string SearchStartTime(int id, int no)
        {
            string table_name = $"ECMORECORD_{no}-{id}";
            string text = "";
            try
            {
                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    string comment = "ECMO START";
                    string sql = $"SELECT * FROM '{table_name}' WHERE NO={no} AND ID={id} AND COMMENT='{comment}' ORDER BY STARTTIME DESC";
                    SQLiteCommand command = new SQLiteCommand(sql, con);

                    using (var reader = command.ExecuteReader())
                    {
                        {
                            if (reader.Read())//レコードが存在していれば読み取り
                            {
                                text = reader["RECORDTIME"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+"サーチタイムでエラー");
            }

            return text;
        }


        public DataTable Historyt(int id, int no)
        {
            string table_name = $"ECMORECORD_{no}-{id}";
            DataTable dt = new DataTable();
            using (SQLiteConnection con = new SQLiteConnection("Data Source=myDataBase.sqlite"))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                $"SELECT RECORDTIME,COMMENT FROM '{table_name}' " +
                $"WHERE ID = {id} AND NO = {no} AND NOT (COMMENT ='' OR COMMENT = 'NULL' OR COMMENT = 'null') ORDER BY RECORDTIME DESC",
                con))
            {
                try
                {
                    adapter.Fill(dt);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "表示リストがありません");
                }
            }

            return dt;
        }

        //ECMO関係のSQL
        //ここから--------------------------------------------------------------------------------------------------------------------------
        public void CreateECMOtable(int id, int no)
        {
            string table_name = $"ECMORECORD_{no}-{id}";
            StringBuilder query = new StringBuilder();
            query.Clear();
            query.Append($"CREATE TABLE IF NOT EXISTS '{table_name}' (");//ECMORECORD tableがなければ作成
            query.Append("NO INTEGER NOT NULL");        //ecmono
            query.Append(", ID INTEGER NOT NULL");      //Pt ID
            query.Append(", RECORDTIME TEXT NOT NULL"); //記録取り込み日時
            query.Append(", STARTTIME TEXT NOT NULL");  //ECMO開始日時                                              
            query.Append(", FLOW TEXT");                //流量
            query.Append(", RPM TEXT");                 //回転数
            query.Append(", PRESS1 TEXT");              //圧力１
            query.Append(", PRESS2 TEXT");              //圧力2
            query.Append(", TEMP1 TEXT");               //温度1
            query.Append(", TEMP2 TEXT");               //温度2
            query.Append(", ALARM TEXT");               //アラーム
            query.Append(", GASFLOW TEXT");             //ガス流量
            query.Append(", GASCONC TEXT");             //酸素濃度
            query.Append(", HCTEMP TEXT");              //冷温水槽温度
            query.Append(", COMMENT TEXT");             //コメント 
            query.Append(",primary key (RECORDTIME)");
            query.Append(")");
            ExecuteNonQuery(query.ToString());
        }
        public void InsertECMOdata(int ecmono, int id, string rectime, string starttime,
           string flow, string rpm, string press1, string press2, string temp1, string temp2, string alarm, string gasflow, string gasconc, string hctemp, string comment)
        {

            //テーブルが存在していなければ作成する
            CreateECMOtable(id,ecmono);
            string table_name = $"ECMORECORD_{ecmono}-{id}";

            //取得したECMOデータをデータベースに登録する
            var query = $"INSERT INTO '{table_name}' (NO,ID,RECORDTIME,STARTTIME,FLOW,RPM,PRESS1,PRESS2,TEMP1,TEMP2,ALARM,GASFLOW,GASCONC,HCTEMP,COMMENT) " +
               "VALUES(" + $"{ecmono},{id},'{rectime}','{starttime}','{flow}','{rpm}','{press1}','{press2}','{temp1}','{temp2}','{alarm}','{gasflow}','{gasconc}','{hctemp}','{comment}')";
            ExecuteNonQuery(query.ToString());

        }
        
        public void Output_ECMO_data(int id, int no,out List<double> dt,out List<double> flow,out List<double> rpm,out List<double> press)
        {
            string table_name = $"ECMORECORD_{no}-{id}";
            dt = new List<double>();
            flow = new List<double>();
            rpm = new List<double>();
            press = new List<double>();
            try
            {
                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    string sql = $"SELECT * FROM '{table_name}' WHERE NO={no} AND ID={id} ORDER BY RECORDTIME ASC";
                    SQLiteCommand command = new SQLiteCommand(sql, con);
                    var result = con.Query(sql);
                    
                    foreach (var item in result)
                    {
                        //doubleでキャストしていき、エラー時はdouble.NaNを入力するようにする
                        try
                        {
                            dt.Add(System.DateTime.Parse(item.RECORDTIME.ToString()).ToOADate());
                            try
                            {
                                flow.Add(double.Parse(item.FLOW.ToString()));
                            }
                            catch { flow.Add(double.NaN); }
                            try
                            {
                                rpm.Add(double.Parse(item.RPM.ToString()));
                            }
                            catch { rpm.Add(double.NaN); }
                            try
                            {
                                press.Add(double.Parse(item.PRESS1.ToString()));
                            }
                            catch { press.Add(double.NaN); }

                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "エラー");
            }
        }

        public void Output_ECMO_data_main(int id, int no,out string gas, out string o2,out string hc)
        {

            //動作確認必要　2023/9/25
            gas = "";
            o2 = "";
            hc = "";

            string table_name = $"ECMORECORD_{no}-{id}";
            try
            {

                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    //gasデータの取得用のコマンド
                    string sql = $"SELECT * FROM '{table_name}' WHERE NO={no} AND ID={id} AND NOT(GASFLOW = '') AND NOT(GASFLOW = 'Gas flow') ORDER BY RECORDTIME DESC";
                    SQLiteCommand command = new SQLiteCommand(sql, con);
                    using (var reader = command.ExecuteReader())
                    {
                        {
                            if (reader.Read())//レコードが存在していれば読み取り
                            {
                                gas = reader["GASFLOW"].ToString();
                            }
                        }
                    }

                    //o2データの取得用のコマンド
                    sql = $"SELECT * FROM '{table_name}' WHERE NO={no} AND ID={id} AND NOT(GASCONC = '') AND NOT(GASCONC = 'percent') ORDER BY RECORDTIME DESC";
                    command = new SQLiteCommand(sql, con);
                    using (var reader = command.ExecuteReader())
                    {
                        {
                            if (reader.Read())//レコードが存在していれば読み取り
                            {
                                o2 = reader["GASCONC"].ToString();
                            }
                        }
                    }

                    //冷温水データの取得
                    sql = $"SELECT * FROM '{table_name}' WHERE NO={no} AND ID={id} AND NOT(HCTEMP = '') ORDER BY RECORDTIME DESC";
                    command = new SQLiteCommand(sql, con);
                    using (var reader = command.ExecuteReader())
                    {
                        {
                            if (reader.Read())//レコードが存在していれば読み取り
                            {
                                hc = reader["HCTEMP"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "エラー");
            }
        }

        public void Output_ECMO_data_pastLog(int id, int no, string time, out string data)
        {
            data = null;
            string table_name = $"ECMORECORD_{no}-{id}";
            try
            {
                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    //gasデータの取得用のコマンド
                    string sql = $"SELECT * FROM '{table_name}' WHERE NO={no} AND ID={id} AND RECORDTIME = '{time}' ";
                    SQLiteCommand command = new SQLiteCommand(sql, con);
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())//レコードが存在していれば読み取り
                        {
                            data += reader["FLOW"]   + ","  + reader["RPM"]     + "," +
                                reader["PRESS1"]     + ","  + reader["PRESS2"]  + "," +
                                reader["TEMP1"]      + ","  + reader["TEMP2"]   + "," +
                                reader["GASFLOW"]    + ","  + reader["GASCONC"] + "," +
                                reader["HCTEMP"]     + ","  + reader["COMMENT"];
                        }
                        
                    }
                }
            }
            catch
            {

            }
        }

        //ここまで--------------------------------------------------------------------------------------------------------------------------


        //vital関係のSQL
        //ここから--------------------------------------------------------------------------------------------------------------------------
        public void CreateVitaltable(int id, int no)
        {
            string table_name = $"VITAL_{no}-{id}";
            StringBuilder query = new StringBuilder();
            query.Clear();
            query.Append($"CREATE TABLE IF NOT EXISTS '{table_name}' (");//vital tableがなければ作成
            query.Append("NO INTEGER NOT NULL");        //ecmono
            query.Append(", ID INTEGER NOT NULL");      //Pt ID
            query.Append(", RECORDTIME TEXT NOT NULL"); //記録取り込み日時                                            
            query.Append(", HR TEXT");                  //HR
            query.Append(", MABP TEXT");                //mABP
            query.Append(", MPAP TEXT");                //mPAP
            query.Append(", MCVP TEXT");                //mCVP
            query.Append(", SPO2 TEXT");                //SpO2 
            query.Append(",primary key (RECORDTIME)");
            query.Append(")");
            ExecuteNonQuery(query.ToString());
        }
        public void InsertVital(int id, int no ,string time, string hr, string abp , string pap, string cvp ,string spo2)
        {
            CreateVitaltable (id, no);
            string table_name = $"VITAL_{no}-{id}";

            //取得したvitalデータをデータベースに登録する
            var query = $"INSERT INTO '{table_name}' (NO,ID,RECORDTIME,HR,MABP,MPAP,MCVP,SPO2) " +
               "VALUES(" + $"{no},{id},'{time}','{hr}','{abp}','{pap}','{cvp}','{spo2}')";
            ExecuteNonQuery(query.ToString());
        }
        //ここまで--------------------------------------------------------------------------------------------------------------------------


        //Lab関係のSQL
        //ここから--------------------------------------------------------------------------------------------------------------------------
        public void CreateLabtable(int id, int no)
        {
            string table_name = $"LAB_{no}-{id}";
            StringBuilder query = new StringBuilder();
            query.Clear();
            query.Append($"CREATE TABLE IF NOT EXISTS '{table_name}' (");//vital tableがなければ作成
            query.Append("NO INTEGER NOT NULL");        //ecmono
            query.Append(", ID INTEGER NOT NULL");      //Pt ID
            query.Append(", RECORDTIME TEXT NOT NULL"); //記録取り込み日時                                            
            query.Append(", SITE TEXT");                //採血部位
            query.Append(", PH TEXT");                  //CDI共通
            query.Append(", PCO2 TEXT");                //CDI共通
            query.Append(", PO2 TEXT");                 //CDI共通
            query.Append(", TEMP TEXT");                //
            query.Append(", HCO3 TEXT");                //CDI共通
            query.Append(", BE TEXT");                  //CDI共通
            query.Append(", HCT TEXT");                 //CDI共通
            query.Append(", HB TEXT");                  //CDI共通
            query.Append(", SO2 TEXT");                 //CDI共通
            query.Append(", K TEXT");                   //CDI共通
            query.Append(", NA TEXT");                  //
            query.Append(", LAC TEXT");                 //
            query.Append(", CA TEXT");                  //
            query.Append(", GLU TEXT");                 //
            query.Append(", ACT TEXT");                 //
            query.Append(",primary key (RECORDTIME)");
            query.Append(")");
            ExecuteNonQuery(query.ToString());
        }

        public void InsertLab(int id , int no , string time,string site,
            string ph,string pco2,string po2,string hco3,string be,string hct,string hb,string so2,string k,
            string na,string lac,string ca,string glu)
        {
            CreateLabtable(id, no);

            string table_name = $"LAB_{no}-{id}";
            //現在の値をデータベースに登録

            //登録用クエリ―
            var query = $"INSERT INTO '{table_name}' (NO,ID,RECORDTIME,SITE,PH,PCO2,PO2,HCO3,BE,HCT,HB,SO2,K,NA,LAC,CA,GLU) VALUES(" + 
                $"{no},{id},'{time}','{site}','{ph}','{pco2}','{po2}','{hco3}','{be}','{hct}','{hb}','{so2}','{k}','{na}','{lac}','{ca}','{glu}')";
            //クエリ―実行
            ExecuteNonQuery(query.ToString());
        }

        public void InserACT(int id, int no , int act, string time)
        {
            string table_name = $"LAB_{no}-{id}";
            //現在の値をデータベースに登録

            //登録用クエリ―
            var query = $"INSERT INTO '{table_name}' (NO,ID,RECORDTIME,ACT) VALUES(" + $"{no},{id},'{time}',{act})";
            //クエリ―実行
            ExecuteNonQuery(query.ToString());
        }

        //ここまで--------------------------------------------------------------------------------------------------------------------------


        //薬関係
        //ここから--------------------------------------------------------------------------------------------------------------------------
        public void CreateMedicationTable(int id, int no)
        {
            string table_name = $"MEDICATION_{no}-{id}";
            //MEDICATION tableがなければ作成
            //no,id,heparin,NAD,DOA,DOB,Main,それぞれの時間項目を作成

            StringBuilder query = new StringBuilder();
            query.Clear();
            query.Append($"CREATE TABLE IF NOT EXISTS '{table_name}' (");
            query.Append("NO INTEGER NOT NULL");
            query.Append(", ID INTEGER NOT NULL");
            query.Append(", DATETIME TEXT NOT NULL");
            query.Append(", HEPARIN INTEGER");
            query.Append(", NAD REAL");
            query.Append(", DOA REAL");
            query.Append(", DOB REAL");
            query.Append(", MAIN INTEGER");
            query.Append(", OTHER INTEGER");
            query.Append(", TOTALVOLUME REAL");
            query.Append(")");
            ExecuteNonQuery(query.ToString());
        }

        public string LatestMedication(int id, int no)
        {
            CreateMedicationTable(id, no);
            //最新の投薬一覧をstring型で取得
            string table_name = $"MEDICATION_{no}-{id}";
            string text = "";
            try
            {
                using (var con = new SQLiteConnection("Data Source = myDataBase.sqlite"))
                {
                    //データベース接続
                    con.Open();
                    //コマンド実行処理
                    string sql = $"SELECT * FROM '{table_name}' WHERE NO={no} AND ID={id} ORDER BY DATETIME DESC";
                    SQLiteCommand command = new SQLiteCommand(sql, con);

                    using (var reader = command.ExecuteReader())
                    {
                        {
                            if (reader.Read())//レコードが存在していれば読み取り
                            {
                                text = reader["HEPARIN"].ToString() + ",";
                                text += reader["NAD"].ToString() + ",";
                                text += reader["DOA"].ToString() + ",";
                                text += reader["DOB"].ToString() + ",";
                                text += reader["MAIN"].ToString() + ",";
                                text += reader["OTHER"].ToString() + ",";
                                text += reader["TOTALVOLUME"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return text;
        }

        public void InsertMedication(int heparin, double nad, double doa, double dob, int main, int other, int id, int ecmono)
        {
            CreateMedicationTable(id, ecmono);

            //現在の値をデータベースに登録
            //いずれは時間計算を行ってボリューム加算していくように変更していく
            //現状は登録するだけ
            System.DateTime dateTime = System.DateTime.Now;
            string dt = dateTime.ToString("yyyy/MM/dd/ HH:mm:ss");
            string table_name = $"MEDICATION_{ecmono}-{id}";
            //登録用クエリ―
            var query = $"INSERT INTO '{table_name}' (NO,ID,DATETIME,HEPARIN,NAD,DOA,DOB,MAIN,OTHER) VALUES(" + $"{ecmono},{id},'{dt}',{heparin},{nad},{doa},{dob},{main},{other})";
            //クエリ―実行
            ExecuteNonQuery(query.ToString());
        }


        //ここまで--------------------------------------------------------------------------------------------------------------------------


    }
}

