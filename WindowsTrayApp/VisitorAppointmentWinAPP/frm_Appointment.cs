using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VisitorAppointmentDB;
using System.Data.Objects;
using System.Media;
using System.Runtime.InteropServices;
using System.Globalization;

namespace VisitorAppointmentWinAPP
{

    public partial class frm_Appointment : Form, IMessageFilter
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private HashSet<Control> controlsToMove = new HashSet<Control>();



        int userType = 4; //4 - deptchairman, 5 - chairman, value read from appsetting 

        Notification _notification;
        public frm_Appointment()
        {
            InitializeComponent();
            // Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            _notification = new Notification();

            string value = System.Configuration.ConfigurationSettings.AppSettings["WinAPPUserType"];
            userType = value == "Chairman" ? 5 : (value == "Boardroom" ? 7 : 4);
            this.Text = value + " - Messaging Application";// Gentle Remainder Application";
            lblTitle.Text = "Visitor Messages - " + value;// +" - Messaging Application";
            //this.Text = "";
            //this.ControlBox = false;
            //if (userType == 4)
            //{
            //dgView_Urgent.Height = dgView_Urgent.Height + 75;
            //this.Height = this.Height + 70;
            //flowLayoutPanel1.Height = flowLayoutPanel1.Height + 70;
            //}

            if (userType == 5 || userType == 7) //Chairman
            {
                // lblUrgent.Text = "Waiting to See You / Urgent";
                urgent1.Visible = true;// false;
                waitingToSee1.Visible = false;// true;
                userControl11.Visible = false;
                dgvWaiting.Visible = false;
                // lblHorizontal1.Visible = false;
                // ResponseContextMenuStrip.Visible = false;

                dgvHeader.Visible = true;
                dgvWaiting.ColumnHeadersVisible = false;
                dgView_Urgent.ColumnHeadersVisible = false;
                dgView_General.ColumnHeadersVisible = false;
                dgView_CarryForward.ColumnHeadersVisible = false;

                this.Size = new System.Drawing.Size(this.Width, 600);
            }
            else if (userType == 4) //Other than Chairman
            {
                urgent1.Visible = true;
                waitingToSee1.Visible = false;
                // lblUrgent.Text = "Urgent";
                userControl11.Visible = true;
                dgvWaiting.Visible = true;

                dgvHeader.Visible = false;
                dgvWaiting.ColumnHeadersVisible = true;
                dgView_Urgent.ColumnHeadersVisible = true;
                dgView_General.ColumnHeadersVisible = true;
                dgView_CarryForward.ColumnHeadersVisible = true;
                // ResponseContextMenuStrip.Visible = true;
                // lblHorizontal1.Visible = true;
                this.Size = new System.Drawing.Size(this.Width, 700);
            }


            Application.AddMessageFilter(this);

            controlsToMove.Add(this);
            controlsToMove.Add(this.panel1);//Ad

            dgView_Urgent.CellValueChanged +=
      new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dgView_Urgent.CurrentCellDirtyStateChanged +=
                 new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);

            dgView_General.CellValueChanged +=
 new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dgView_General.CurrentCellDirtyStateChanged +=
                 new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);

            dgView_CarryForward.CellValueChanged +=
 new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dgView_CarryForward.CurrentCellDirtyStateChanged +=
                 new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);

            dgvWaiting.CellValueChanged +=
      new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dgvWaiting.CurrentCellDirtyStateChanged +=
                 new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN &&
                 controlsToMove.Contains(Control.FromHandle(m.HWnd)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return true;
            }
            return false;
        }

        void loadExistingAppoint(string filterOption = "", string filterStr = "")
        {
            timer1.Enabled = false;
            try
            {
                int u_rowCnt = dgView_Urgent.Rows.Count;

                int g_rowCnt = dgView_General.Rows.Count;

                int w_rowCnt = dgvWaiting.Rows.Count;

                //int c_rowCnt = dgView_CarryForward.Rows.Count;

                DateTime datefilter = DateTime.Now.AddMinutes(10);

                VisitorAPPEntities ENT = new VisitorAPPEntities();

                var data = (from getdata in ENT.InfoMessages
                            join getmsgType in ENT.TypeMsgContents on getdata.msgTypeID equals getmsgType.MsgTypeId
                            join getmsgStatus in ENT.TypeMsgStatus on getdata.msgStatusID equals getmsgStatus.msgStatusID
                            join getfromuser in ENT.InfoUsers on getdata.FromUserID equals getfromuser.UID
                            join getTouser in ENT.InfoUsers on getdata.ToUserID equals getTouser.UID
                            join getfromrole in ENT.TypeUserRoles on getfromuser.UserRoleID equals getfromrole.UserRoleID
                            join gettorole in ENT.TypeUserRoles on getTouser.UserRoleID equals gettorole.UserRoleID
                            where getdata.isActive == true && getdata.ToUserID == userType
                            && (getmsgStatus.msgStatusID == 1)
                            && (getdata.isUrgent == true || getdata.msgTypeID == 1) //&& (getdata.isUrgent == true || getdata.isUrgent == false)
                            && getdata.CreatedDateTime <= datefilter // before 10 min, msg will display   
                            && EntityFunctions.TruncateTime(getdata.CreatedDateTime) == EntityFunctions.TruncateTime(DateTime.Now)
                            select new
                            {
                                u_msgID = getdata.msgID,
                                u_TodayID = getdata.TodayID,
                                u_CreatedDateTime = getdata.CreatedDateTime,
                                u_VisitorName = getdata.VisitorName,
                                u_MessageContent = getmsgType.MessageContent,
                                u_VisitorMessage = getdata.VisitorMessage
                            });

                if (userType == 4)
                {
                    data = (from getdata in ENT.InfoMessages
                            join getmsgType in ENT.TypeMsgContents on getdata.msgTypeID equals getmsgType.MsgTypeId
                            join getmsgStatus in ENT.TypeMsgStatus on getdata.msgStatusID equals getmsgStatus.msgStatusID
                            join getfromuser in ENT.InfoUsers on getdata.FromUserID equals getfromuser.UID
                            join getTouser in ENT.InfoUsers on getdata.ToUserID equals getTouser.UID
                            join getfromrole in ENT.TypeUserRoles on getfromuser.UserRoleID equals getfromrole.UserRoleID
                            join gettorole in ENT.TypeUserRoles on getTouser.UserRoleID equals gettorole.UserRoleID
                            where getdata.isActive == true && getdata.ToUserID == userType
                            && (getmsgStatus.msgStatusID == 1)
                            && getdata.isUrgent == true
                            && getdata.msgTypeID > 1
                            && getdata.CreatedDateTime <= datefilter // before 10 min, msg will display   
                            && EntityFunctions.TruncateTime(getdata.CreatedDateTime) == EntityFunctions.TruncateTime(DateTime.Now)
                            select new
                            {
                                u_msgID = getdata.msgID,
                                u_TodayID = getdata.TodayID,
                                u_CreatedDateTime = getdata.CreatedDateTime,
                                u_VisitorName = getdata.VisitorName,
                                u_MessageContent = getmsgType.MessageContent,
                                u_VisitorMessage = getdata.VisitorMessage
                            });
                }

                int rcnt = 0;
                if (dgView_Urgent.Rows.Count >= 1)
                    dgView_Urgent.Rows.Clear();
                int dbRowCnt = data.Count();
                if (dbRowCnt >= 1)
                {
                    dgView_Urgent.Rows.Add(dbRowCnt);
                    foreach (var dtrow in data)
                    {
                        dgView_Urgent.Rows[rcnt].Cells[0].Value = dtrow.u_msgID;

                        dgView_Urgent.Rows[rcnt].Cells[1].Value = dtrow.u_TodayID;

                        dgView_Urgent.Rows[rcnt].Cells[2].Value = Convert.ToDateTime(dtrow.u_CreatedDateTime).ToString("dd-MMM hh:mm tt");

                        dgView_Urgent.Rows[rcnt].Cells[3].Value = dtrow.u_VisitorName;

                        dgView_Urgent.Rows[rcnt].Cells[4].Value = dtrow.u_MessageContent;

                        dgView_Urgent.Rows[rcnt].Cells[5].Value = dtrow.u_VisitorMessage;

                        rcnt += 1;
                    }
                    //dgView_Urgent.Columns[1].Width = 20;
                    //dgView_Urgent.Columns[5].Width = 500;
                }
                var data1 = (from getdata in ENT.InfoMessages
                             join getmsgType in ENT.TypeMsgContents on getdata.msgTypeID equals getmsgType.MsgTypeId
                             join getmsgStatus in ENT.TypeMsgStatus on getdata.msgStatusID equals getmsgStatus.msgStatusID
                             join getfromuser in ENT.InfoUsers on getdata.FromUserID equals getfromuser.UID
                             join getTouser in ENT.InfoUsers on getdata.ToUserID equals getTouser.UID
                             join getfromrole in ENT.TypeUserRoles on getfromuser.UserRoleID equals getfromrole.UserRoleID
                             join gettorole in ENT.TypeUserRoles on getTouser.UserRoleID equals gettorole.UserRoleID
                             where getdata.isActive == true && getdata.ToUserID == userType
                            && (getmsgStatus.msgStatusID == 1)
                            && getdata.isUrgent == false
                            && getdata.msgTypeID > 1
                            && EntityFunctions.TruncateTime(getdata.CreatedDateTime) == EntityFunctions.TruncateTime(DateTime.Now)
                            && getdata.CreatedDateTime <= datefilter // before 10 min, msg will display   
                             //&& getdata.CreatedDateTime >= DateTime.Now.AddHours(-23).Date  
                             select new
                             {
                                 g_msgID = getdata.msgID,
                                 g_TodayID = getdata.TodayID,
                                 g_CreatedDateTime = getdata.CreatedDateTime,
                                 g_VisitorName = getdata.VisitorName,
                                 g_MessageContent = getmsgType.MessageContent,
                                 g_VisitorMessage = getdata.VisitorMessage
                             });


                dbRowCnt = 0;
                rcnt = 0;
                if (dgView_General.Rows.Count >= 1)
                    dgView_General.Rows.Clear();
                dbRowCnt = data1.Count();
                if (dbRowCnt >= 1)
                {
                    dgView_General.Rows.Add(data1.Count());
                    foreach (var dtrow in data1)
                    {
                        dgView_General.Rows[rcnt].Cells[0].Value = dtrow.g_msgID;

                        dgView_General.Rows[rcnt].Cells[1].Value = dtrow.g_TodayID;

                        dgView_General.Rows[rcnt].Cells[2].Value = Convert.ToDateTime(dtrow.g_CreatedDateTime).ToString("dd-MMM hh:mm tt");

                        dgView_General.Rows[rcnt].Cells[3].Value = dtrow.g_VisitorName;

                        dgView_General.Rows[rcnt].Cells[4].Value = dtrow.g_MessageContent;

                        dgView_General.Rows[rcnt].Cells[5].Value = dtrow.g_VisitorMessage;

                        rcnt += 1;
                    }
                }
                var data2 = (from getdata in ENT.InfoMessages
                             join getmsgType in ENT.TypeMsgContents on getdata.msgTypeID equals getmsgType.MsgTypeId
                             join getmsgStatus in ENT.TypeMsgStatus on getdata.msgStatusID equals getmsgStatus.msgStatusID
                             join getfromuser in ENT.InfoUsers on getdata.FromUserID equals getfromuser.UID
                             join getTouser in ENT.InfoUsers on getdata.ToUserID equals getTouser.UID
                             join getfromrole in ENT.TypeUserRoles on getfromuser.UserRoleID equals getfromrole.UserRoleID
                             join gettorole in ENT.TypeUserRoles on getTouser.UserRoleID equals gettorole.UserRoleID
                             where getdata.isActive == true && getdata.ToUserID == userType
                        && (getmsgStatus.msgStatusID == 3
                        || (getmsgStatus.msgStatusID == 1 && EntityFunctions.TruncateTime(getdata.CreatedDateTime) < EntityFunctions.TruncateTime(DateTime.Now)))

                             select new
                             {
                                 c_msgID = getdata.msgID,
                                 c_TodayID = getdata.TodayID,
                                 c_CreatedDateTime = getdata.CreatedDateTime,
                                 c_VisitorName = getdata.VisitorName,
                                 c_MessageContent = getmsgType.MessageContent,
                                 c_VisitorMessage = getdata.VisitorMessage
                             });

                dbRowCnt = 0;
                rcnt = 0;
                if (dgView_CarryForward.Rows.Count >= 1)
                    dgView_CarryForward.Rows.Clear();
                dbRowCnt = data2.Count();
                if (dbRowCnt >= 1)
                {
                    dgView_CarryForward.Rows.Add(dbRowCnt);

                    foreach (var dtrow in data2)
                    {

                        dgView_CarryForward.Rows[rcnt].Cells[0].Value = dtrow.c_msgID;

                        dgView_CarryForward.Rows[rcnt].Cells[1].Value = dtrow.c_TodayID;

                        dgView_CarryForward.Rows[rcnt].Cells[2].Value = Convert.ToDateTime(dtrow.c_CreatedDateTime).ToString("dd-MMM hh:mm tt");

                        dgView_CarryForward.Rows[rcnt].Cells[3].Value = dtrow.c_VisitorName;

                        dgView_CarryForward.Rows[rcnt].Cells[4].Value = dtrow.c_MessageContent;

                        dgView_CarryForward.Rows[rcnt].Cells[5].Value = dtrow.c_VisitorMessage;

                        rcnt += 1;
                    }
                }

                //Waiting to see you
                var data3 = (from getdata in ENT.InfoMessages
                             join getmsgType in ENT.TypeMsgContents on getdata.msgTypeID equals getmsgType.MsgTypeId
                             join getmsgStatus in ENT.TypeMsgStatus on getdata.msgStatusID equals getmsgStatus.msgStatusID
                             join getfromuser in ENT.InfoUsers on getdata.FromUserID equals getfromuser.UID
                             join getTouser in ENT.InfoUsers on getdata.ToUserID equals getTouser.UID
                             join getfromrole in ENT.TypeUserRoles on getfromuser.UserRoleID equals getfromrole.UserRoleID
                             join gettorole in ENT.TypeUserRoles on getTouser.UserRoleID equals gettorole.UserRoleID
                             where getdata.isActive == true && getdata.ToUserID == userType
                             && (getmsgStatus.msgStatusID == 1)
                             && (getdata.isUrgent == false || getdata.isUrgent == true)
                             && getdata.msgTypeID == 1
                             && getdata.CreatedDateTime <= datefilter // before 10 min, msg will display   
                             && EntityFunctions.TruncateTime(getdata.CreatedDateTime) == EntityFunctions.TruncateTime(DateTime.Now)
                             select new
                             {
                                 w_msgID = getdata.msgID,
                                 w_TodayID = getdata.TodayID,
                                 w_CreatedDateTime = getdata.CreatedDateTime,
                                 w_VisitorName = getdata.VisitorName,
                                 w_MessageContent = getmsgType.MessageContent,
                                 w_VisitorMessage = getdata.VisitorMessage
                             });
                dbRowCnt = 0;
                rcnt = 0;
                if (dgvWaiting.Rows.Count >= 1)
                    dgvWaiting.Rows.Clear();
                dbRowCnt = data3.Count();
                if (dbRowCnt >= 1)
                {
                    dgvWaiting.Rows.Add(dbRowCnt);

                    foreach (var dtrow in data3)
                    {

                        dgvWaiting.Rows[rcnt].Cells[0].Value = dtrow.w_msgID;

                        dgvWaiting.Rows[rcnt].Cells[1].Value = dtrow.w_TodayID;

                        dgvWaiting.Rows[rcnt].Cells[2].Value = Convert.ToDateTime(dtrow.w_CreatedDateTime).ToString("dd-MMM hh:mm tt");

                        dgvWaiting.Rows[rcnt].Cells[3].Value = dtrow.w_VisitorName;

                        dgvWaiting.Rows[rcnt].Cells[4].Value = dtrow.w_MessageContent;

                        dgvWaiting.Rows[rcnt].Cells[5].Value = dtrow.w_VisitorMessage;

                        rcnt += 1;
                    }
                }


                ENT.Connection.Close();

                dgView_Urgent.Columns[0].Visible = false;

                //dgView_Urgent.Columns[1].Width = 106;

                //dgView_Urgent.Columns[2].Width = 116;

                //dgView_Urgent.Columns[3].Width = 118;

                //dgView_Urgent.Columns[4].Width = 160;

                //dgView_Urgent.Columns[5].Width = 160;

                dgView_General.Columns[0].Visible = false;

                //dgView_General.Columns[1].Width = 106;

                //dgView_General.Columns[2].Width = 116;

                //dgView_General.Columns[3].Width = 118;

                //dgView_General.Columns[4].Width = 160;

                //dgView_General.Columns[5].Width = 160;

                dgView_CarryForward.Columns[0].Visible = false;

                //dgView_CarryForward.Columns[1].Width = 106;

                //dgView_CarryForward.Columns[2].Width = 116;

                //dgView_CarryForward.Columns[3].Width = 118;

                //dgView_CarryForward.Columns[4].Width = 160;

                //dgView_CarryForward.Columns[5].Width = 160;

                dgvWaiting.Columns[0].Visible = false;

                //dgvWaiting.Columns[1].Width = 106;

                //dgvWaiting.Columns[2].Width = 116;

                //dgvWaiting.Columns[3].Width = 118;

                //dgvWaiting.Columns[4].Width = 160;

                //dgvWaiting.Columns[5].Width = 160;

                int u_rowCnt_a = dgView_Urgent.Rows.Count;

                int g_rowCnt_a = dgView_General.Rows.Count;

                int w_rowCnt_a = dgvWaiting.Rows.Count;

                //int c_rowCnt_a = dgView_CarryForward.Rows.Count;

                if ((u_rowCnt != u_rowCnt_a && u_rowCnt_a > u_rowCnt) || (g_rowCnt != g_rowCnt_a && g_rowCnt_a > g_rowCnt) || (w_rowCnt != w_rowCnt_a && w_rowCnt_a > w_rowCnt))// || (c_rowCnt != c_rowCnt_a && c_rowCnt_a > c_rowCnt))
                {
                    //int newMsg= (u_rowCnt_a - u_rowCnt) + (g_rowCnt_a-g_rowCnt) + (w_rowCnt_a-
                    // notifyIcon1.ShowBalloonTip(60); //Commented on 20-Apr-2017
                    if (_notification == null)
                        _notification = new Notification();
                    else if (_notification.IsDisposed)
                        _notification = new Notification();


                    if (userType == 5 || userType == 7)
                    {
                        if ((u_rowCnt_a + g_rowCnt_a) > 1)
                            _notification.noOfPersons = "You have " + (u_rowCnt_a + g_rowCnt_a).ToString() + " new messages";
                        else
                            _notification.noOfPersons = "You have " + (u_rowCnt_a + g_rowCnt_a).ToString() + " new message";
                    }
                    else
                    {
                        if ((u_rowCnt_a + g_rowCnt_a + w_rowCnt_a) > 1)
                            _notification.noOfPersons = "You have " + (u_rowCnt_a + g_rowCnt_a + w_rowCnt_a).ToString() + " new messages";
                        else
                            _notification.noOfPersons = "You have " + (u_rowCnt_a + g_rowCnt_a + w_rowCnt_a).ToString() + " new message";
                    }

                    _notification.frmAppointment = this;
                    _notification.TopMost = true;
                    SystemSounds.Beep.Play();

                    if (this.Visible)
                        _notification.Close();
                    else
                        _notification.Show();
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
            finally { timer1.Enabled = true; }
        }

        void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView ctrl = (DataGridView)sender;
            if (ctrl.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                ctrl.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // My combobox column is the second one so I hard coded a 1, flavor to taste
            try
            {
                DataGridView ctrl = (DataGridView)sender;
                DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)ctrl.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cb.Value != null)
                {

                    timer1.Enabled = false;

                    if (MessageBox.Show("Visistor Name: " + ctrl.Rows[e.RowIndex].Cells[3].Value + ", Response Selected : " + ctrl.Rows[e.RowIndex].Cells[6].Value + ". Are you sure want to continue?", "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        long msgID = (long)ctrl.Rows[e.RowIndex].Cells[0].Value;

                        //processRec(msgID, ctrl.Rows[e.RowIndex].Cells[6].Value.ToString(), ctrl);
                        processRec(msgID, itemValue, ctrl);
                    }

                    timer1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        void processRec(long msgID, string resMsg, DataGridView ctrl)
        {
            try
            {

                VisitorAPPEntities ENT = new VisitorAPPEntities();
                InfoMessage imsg = (from data in ENT.InfoMessages
                                    where data.msgID == msgID
                                    select data).FirstOrDefault();


                imsg.ResponseTime = DateTime.Now;

                if (resMsg.Equals("Accept", StringComparison.InvariantCultureIgnoreCase) || resMsg.Equals("Reject", StringComparison.InvariantCultureIgnoreCase))
                {
                    imsg.ResponseString = resMsg == "Accept" ? "Accepted" : "Rejected";
                    imsg.msgStatusID = 2;

                }

                else if (resMsg.Equals("Postpone", StringComparison.InvariantCultureIgnoreCase))
                {

                    frm_PostponeTime frmTime = new frm_PostponeTime(imsg.CreatedDateTime);
                    frmTime.ShowDialog();
                    if (frmTime.DialogResult == System.Windows.Forms.DialogResult.Yes && frmTime.isValidTime)
                    {
                        imsg.msgStatusID = 2;
                        imsg.ResponseString = "Postpone";
                        imsg.VisitorMessage = "Postpone Comments : " + frmTime.executiveMessage + Environment.NewLine + "************" + Environment.NewLine + imsg.VisitorMessage;
                        imsg.CreatedDateTime = frmTime.seletedDateTime;
                    }

                }

                else if (resMsg == "Carry Forward")
                {
                    imsg.msgStatusID = 3;
                }
                else//For Deputy Chairman
                {
                    imsg.ResponseString = resMsg;
                    imsg.msgStatusID = 2;
                }
                ENT.SaveChanges();

                ENT.AcceptAllChanges();

                ENT.Connection.Close();

                //this.Visible = false;

                loadExistingAppoint();

            }
            catch (Exception ex)
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadDiplayData();
            loadExistingAppoint();
        }

        void LoadDiplayData()
        {
            // lbl_DateTime.Text = "Date Time: " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt");    //30-Mar-2017 10:00 AM" 
            toolStripStatusLabel1.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt");
            statusStrip1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {

                timer1.Enabled = true;
                LoadDiplayData();
                loadExistingAppoint();

            }
            catch (Exception ex)
            {

            }
        }

        private void visitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void frm_Appointment_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void frm_Appointment_Load(object sender, EventArgs e)
        {
            //string value = System.Configuration.ConfigurationSettings.AppSettings["WinAPPUserType"];
            //userType = value == "Chairman" ? 5 : 4;
            //this.Text = value + " - Gentle Remainder Application";


            //if (userType == 5) //Chairman
            //{
            //    lblUrgent.Text = "Waiting to See You / Urgent";
            //    lblWaiting.Visible = false;
            //    dgvWaiting.Visible = false;
            //}
            //else if (userType == 4) //Other than Chairman
            //{
            //    lblUrgent.Text = "Urgent";
            //    lblWaiting.Visible = true;
            //    dgvWaiting.Visible = true;
            //}

            //this.Size = new System.Drawing.Size(this.Width, flowLayoutPanel1.Height + 10);

            panel1.Width = this.Width;
            pictureBox3.Location = new Point(panel1.Width - 45, 4);
            flowLayoutPanel1.Width = this.Width;
            dgvWaiting.Width = this.Width - 15;
            dgView_Urgent.Width = this.Width - 15;
            dgView_General.Width = this.Width - 15;
            dgView_CarryForward.Width = this.Width - 15;

            waitingToSee1.Width = this.Width - 15;
            urgent1.Width = this.Width - 15;
            general1.Width = this.Width - 15;
            carryForward1.Width = this.Width - 15;

            if (userType == 5 || userType == 7)//Chairman
            {
                //3 Grid
                dgvWaiting.Height = this.Height / 4;
                dgView_Urgent.Height = this.Height / 4;
                dgView_General.Height = this.Height / 4;
                dgView_CarryForward.Height = this.Height / 5;
            }
            else
            {
                //4 Grid
                dgvWaiting.Height = this.Height / 6;
                dgView_Urgent.Height = this.Height / 6;
                dgView_General.Height = this.Height / 6;
                dgView_CarryForward.Height = this.Height / 5;
            }

            //this.AutoSize = true;

            loadExistingAppoint();

            LoadDiplayData();

            // this.Visible = false;
            // timer1.Enabled = true;
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }

        private void frm_Appointment_Activated(object sender, EventArgs e)
        {
            loadExistingAppoint();
        }
        ComboBox cb;
        private void dgView_General_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //if (dgView_General.CurrentCell.ColumnIndex == 6 || dgView_CarryForward.CurrentCell.ColumnIndex == 6 || dgView_Urgent.CurrentCell.ColumnIndex == 6 || dgvWaiting.CurrentCell.ColumnIndex == 6)
            if (((DataGridView)(sender)).CurrentCell.ColumnIndex == 6)
            {
                cb = e.Control as ComboBox;
                if (cb != null)
                    cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);
            }
        }
        string itemValue;
        void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemValue = cb.Text;
        }

        private void dgView_CarryForward_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;

            //DataGridView dataGridView1 = (DataGridView)sender;
            //List<int> columns = new List<int>() { 2, 3, 4, 5, 7, 8, 9 };
            //foreach (int i in columns)
            //{
            //    Rectangle headerRect = dataGridView1.GetCellDisplayRectangle(i, -1, true); //get the column header cell
            //    headerRect.X = headerRect.X + headerRect.Width - 2;
            //    headerRect.Y += 2;
            //    headerRect.Width = 2 * 2;
            //    headerRect.Height -= 3;
            //    DataGridViewColumn dataGridViewColumn = dataGridView1.Columns["<Column>"];
            //    Color cl;
            //    cl = dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            //    e.Graphics.FillRectangle(new SolidBrush(cl), headerRect);
            //}
            //if (e.ColumnIndex == 7)//Accept
            //{
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

            //    var w = 20;// Properties.Resources.Accept.Width;
            //    var h = 20;//Properties.Resources.Accept.Height;
            //    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
            //    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

            //    e.Graphics.DrawImage(Properties.Resources.Accept, new Rectangle(x, y, w, h));
            //    e.Handled = true;
            //}
            ////else if (e.ColumnIndex == 9)//Reject
            //{
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

            //    var w = 20;// Properties.Resources.Accept.Width;
            //    var h = 20;//Properties.Resources.Accept.Height;
            //    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
            //    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

            //    e.Graphics.DrawImage(Properties.Resources.Reject, new Rectangle(x, y, w, h));
            //    e.Handled = true;
            //}
            //else if (e.ColumnIndex == 8)//Postpone
            //{
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

            //    var w = 20;// Properties.Resources.Accept.Width;
            //    var h = 20;//Properties.Resources.Accept.Height;
            //    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
            //    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

            //    e.Graphics.DrawImage(Properties.Resources.time_icon_grey, new Rectangle(x, y, w, h));
            //    e.Handled = true;
            //}
            //else
            //{
            // }
        }

        private void dgvWaiting_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            timer1.Enabled = false;

            DataGridView senderGrid = (DataGridView)sender;
            if ((senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn || senderGrid.Columns[e.ColumnIndex] is DataGridViewImageColumn) && e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 7)//Accept 
                {

                    if (userType == 4)
                    {
                        frmResponse _frm = new frmResponse();
                        _frm.ShowDialog();
                        if (_frm.selectedValue.Length > 0)
                            itemValue = _frm.selectedValue.ToString();
                        else
                        {
                            timer1.Enabled = true;
                            return;
                        }
                    }
                    else
                        itemValue = "Accept";
                }
                if (e.ColumnIndex == 9) //Reject
                {
                    itemValue = "Reject";
                }
                else if (e.ColumnIndex == 8)//Postpone
                {
                    itemValue = "Postpone";
                }

                long msgID = (long)senderGrid.Rows[e.RowIndex].Cells[0].Value;
                processRec(msgID, itemValue, senderGrid);
            }

            //DataGridView senderGrid = (DataGridView)sender;
            //if ((senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn || senderGrid.Columns[e.ColumnIndex] is DataGridViewImageColumn) && e.RowIndex >= 0)
            //{
            //    if (ResponseContextMenuStrip.Visible == false && e.ColumnIndex == 7)//Chairman should not get context menu for accept
            //    {
            //        if (e.ColumnIndex == 7)//Accept
            //        {
            //            itemValue = "Accept";
            //        }
            //        else if (e.ColumnIndex == 9) //Reject
            //        {
            //            itemValue = "Reject";
            //        }
            //        else if (e.ColumnIndex == 8)//Postpone
            //        {
            //            itemValue = "Postpone";
            //        }

            //        long msgID = (long)senderGrid.Rows[e.RowIndex].Cells[0].Value;
            //        processRec(msgID, itemValue, senderGrid);
            //    }
            //    else
            //    {
            //        //Show context menu

            //    }
            //}
            timer1.Enabled = true;

        }

        private void dgvWaiting_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dataGridView1 = (DataGridView)sender;
            if (e.ColumnIndex == 7)
            {
                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = "Accept";
            }
            else if (e.ColumnIndex == 9) //Reject
            {
                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = "Reject";
            }
            else if (e.ColumnIndex == 8)//Postpone
            {
                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = "Postpone";
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
