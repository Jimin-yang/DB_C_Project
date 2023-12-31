﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace PT_Project
{
    public partial class 회원관리 : Form
    {
        트레이너 trainer;
        private int SelectID;
        private int SelectConsumerID = 0;
        private OracleConnection odpConn = new OracleConnection();
        public 회원관리(트레이너 trainer)
        {
            InitializeComponent();
            this.trainer = trainer;
            SelectID = trainer.getusernumber;
        }
        private void searchBtn2_Click(object sender, EventArgs e)
        {
            try
            {
                if ( checkBox1.Checked)
                {
                    Btn.Text = "수정";
                    odpConn.ConnectionString = "User Id=ptadmin; Password=1111; Data Source=(DESCRIPTION =   (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))   (CONNECT_DATA =     (SERVER = DEDICATED)     (SERVICE_NAME = xe)   ) );";
                    odpConn.Open();
                    OracleDataAdapter oda = new OracleDataAdapter()
                    {
                        SelectCommand = new OracleCommand(string.Concat("select P.U_NO as 회원번호, CD.P_date as 작성일자 from consumerDiet CD, program P  where P.P_NO = CD.P_NO and CD.P_Grade IS NOT NULL and P.U_NO = ", SelectConsumerID.ToString()), odpConn)
                    };
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    odpConn.Close();
                    DBGrid.DataSource = dt;
                    DBGrid.AutoResizeColumns();
                    DBGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    DBGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DBGrid.AllowUserToAddRows = false;
                }
                else
                {
                     Btn.Text = "추가";
                     odpConn.ConnectionString = "User Id=ptadmin; Password=1111; Data Source=(DESCRIPTION =   (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))   (CONNECT_DATA =     (SERVER = DEDICATED)     (SERVICE_NAME = xe)   ) );";
                     odpConn.Open();
                    OracleDataAdapter oda = new OracleDataAdapter()
                    {
                        SelectCommand = new OracleCommand(string.Concat("select P.U_NO as 회원번호, CD.P_date as 작성일자 from consumerDiet CD, program P  where P.P_NO = CD.P_NO and CD.P_Grade IS NULL and P.U_NO = ",  SelectConsumerID.ToString()),  odpConn)
                    };
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    odpConn.Close();
                    DBGrid.DataSource = dt;
                    DBGrid.AutoResizeColumns();
                    DBGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    DBGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DBGrid.AllowUserToAddRows = false;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                MessageBox.Show(string.Concat("에러 발생 : ", ex.ToString()));
                 odpConn.Close();
            }
        }

        private void DBGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (checkBox1.Checked)
            {
                int selectID = Convert.ToInt32(DBGrid.SelectedCells[0].Value);
                string selectDATE = DBGrid.SelectedCells[1].Value.ToString().Substring(0, 11);
                day.Text = selectDATE;
                odpConn.ConnectionString = "User Id = ptadmin; Password = 1111; Data Source = (DESCRIPTION = (ADDRESS =(PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = xe))); ";
                odpConn.Open();
                OracleCommand OraCmd = new OracleCommand("SELECT CD.P_Menu, CD.P_Grade, CD.P_Feedback FROM consumerDiet CD, Program P WHERE P.P_NO = CD.P_NO and P.U_NO= :U_NO and CD.P_date =:P_date",  odpConn);
                OraCmd.Parameters.Add("U_NO", OracleDbType.Int32).Value = selectID;
                OraCmd.Parameters.Add("P_date", OracleDbType.Date).Value = selectDATE;
                OracleDataReader odr = OraCmd.ExecuteReader();
                while (odr.Read())
                {
                    string m = Convert.ToString(odr.GetValue(0));
                    int index1 = m.IndexOf("@");
                    int index2 = m.IndexOf("@", index1 + 1);
                    int length = m.Length;
                    string breakfast = m.Substring(0, index1);
                    string lunch = m.Substring(index1 + 1, index2 - index1 - 1);
                    string dinner = m.Substring(index2 + 1, length - index2 - 1);
                    menu.Text = breakfast + "\r\n" + lunch + "\r\n" + dinner;
                    switch (Convert.ToInt32(odr.GetValue(1)))
                    {
                        case 1:
                            {
                                r1.Checked = true;
                                break;
                            }
                        case 2:
                            {
                                 r2.Checked = true;
                                break;
                            }
                        case 3:
                            {
                                 r3.Checked = true;
                                break;
                            }
                        case 4:
                            {
                                 r4.Checked = true;
                                break;
                            }
                        case 5:
                            {
                                 r5.Checked = true;
                                break;
                            }
                    }
                     textBox1.Text = Convert.ToString(odr.GetValue(2));
                }
                odr.Close();
                odpConn.Close();
            }
            else
            {
                textBox1.Clear();
                r1.Checked = false;
                r2.Checked = false;
                r3.Checked = false;
                r4.Checked = false;
                r5.Checked = false;
                int selectID = Convert.ToInt32( DBGrid.SelectedCells[0].Value);
                string selectDATE =  DBGrid.SelectedCells[1].Value.ToString().Substring(0, 11);
                day.Text = selectDATE;
                odpConn.ConnectionString = "User Id = ptadmin; Password = 1111; Data Source = (DESCRIPTION = (ADDRESS =(PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = xe))); ";
                odpConn.Open();
                OracleCommand OraCmd = new OracleCommand("SELECT CD.P_Menu FROM consumerDiet CD, Program P WHERE P.P_NO = CD.P_NO and P.U_NO= :U_NO and CD.P_date =:P_date", odpConn);
                OraCmd.Parameters.Add("U_NO", OracleDbType.Int32).Value = selectID;
                OraCmd.Parameters.Add("P_date", OracleDbType.Date).Value = selectDATE;
                OracleDataReader odr = OraCmd.ExecuteReader();
                while (odr.Read())
                {
                    string m = Convert.ToString(odr.GetValue(0));
                    int index1 = m.IndexOf("@");
                    int index2 = m.IndexOf("@", index1+1);
                    int length = m.Length;
                    string breakfast = m.Substring(0, index1);
                    string lunch = m.Substring(index1+1, index2-index1-1);
                    string dinner = m.Substring(index2+1, length-index2-1);
                    menu.Text = "아침 : " + breakfast + "\r\n" + "점심 : " + lunch + "\r\n" + "저녁 : " + dinner;
                }
                odr.Close();
                odpConn.Close();
            }
        }
        public int returnGrade()
        {
            int num;
            if ( r5.Checked)
            {
                num = 5;
            }
            else if ( r4.Checked)
            {
                num = 4;
            }
            else if ( r3.Checked)
            {
                num = 3;
            }
            else if ( r2.Checked)
            {
                num = 2;
            }
            else if (! r1.Checked)
            {
                MessageBox.Show("평점을 눌러주세요!");
                num = 0;
            }
            else
            {
                num = 1;
            }
            return num;
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            string name =  listView1.SelectedItems[0].SubItems[1].Text;
            searchBtn1.Text = string.Concat(name, "님 상세 조회");
            searchBtn2.Text = string.Concat(name, "님 식단 조회");
            SelectConsumerID = Convert.ToInt32( listView1.SelectedItems[0].Text);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            odpConn.ConnectionString = "User Id = ptadmin; Password = 1111; Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = xe))); ";
            odpConn.Open();
            int grade =  returnGrade();
            int selectID = Convert.ToInt32( DBGrid.SelectedCells[0].Value);
            string selectDATE =  DBGrid.SelectedCells[1].Value.ToString().Substring(0, 11);
            OracleCommand OraCmd = new OracleCommand("UPDATE consumerDiet SET P_Grade=:P_Grade, P_Feedback=:P_Feedback WHERE U_NO= :U_NO and P_date =:P_date",  odpConn);
            OraCmd.Parameters.Add("P_Grade", OracleDbType.Int32).Value = grade;
            OraCmd.Parameters.Add("P_Feedback", OracleDbType.Varchar2, 100).Value =  textBox1.Text.Trim();
            OraCmd.Parameters.Add("U_NO", OracleDbType.Int32).Value = selectID;
            OraCmd.Parameters.Add("P_date", OracleDbType.Date).Value = selectDATE;
            OraCmd.ExecuteReader().Close();
            odpConn.Close();
            MessageBox.Show(string.Concat( Btn.Text, "하셨습니다!"));
        }

        private void 회원관리_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            OracleConnection myConnection = new OracleConnection("User Id=ptadmin; Password=1111; Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = xe) ) );");
            string commandString = string.Concat("SELECT C.U_NO, C.cname, ",
                "CASE WHEN C.cweight / POWER(C.cheight / 100, 2) < 18.5 THEN '저체중' ",
                "WHEN C.cweight / POWER(C.cheight / 100, 2) >= 18.5 AND C.cweight / POWER(C.cheight / 100, 2) < 24.9 THEN '정상' ",
                "WHEN C.cweight / POWER(C.cheight / 100, 2) >= 25 AND C.cweight / POWER(C.cheight / 100, 2) < 29.9 THEN '과체중' ",
                "ELSE '비만' END AS BMI_Category ",
                "FROM program P, consumer C ",
                "WHERE P.U_NO = C.U_NO AND P.T_NO = ", SelectID.ToString());
            OracleCommand myCommand = new OracleCommand()
            {
                Connection = myConnection,
                CommandText = commandString
            };
            myConnection.Open();
            OracleDataReader MR = myCommand.ExecuteReader();
            while (MR.Read())
            {
                ListViewItem item = new ListViewItem(MR[0].ToString());
                item.SubItems.Add(MR[1].ToString());
                item.SubItems.Add(MR[2].ToString());
                listView1.Items.Add(item);
            }
            MR.Close();
            myConnection.Close();
        }
        private void searchBtn1_Click(object sender, EventArgs e)
        {
            trainer.open회원관리2(SelectConsumerID);
        }
    }
}
    