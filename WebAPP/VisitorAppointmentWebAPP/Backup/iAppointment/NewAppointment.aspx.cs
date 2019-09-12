using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VisitorAppointmentDB;
using System.Drawing;
using System.Data.Objects;
using System.Linq.Expressions;

namespace VisitorAppointmentWebAPP.iAppointment
{
    public partial class NewAppointment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_Date.Text = DateTime.Now.ToString("dd-MMM-yyy");
                //txt_DateTime.Text = DateTime.Now.ToString("hh:mm tt");
                txt_SlNo.Text = getTodayNextSlNo().ToString();
                //txt_ReceivedBy.Text = Session["UserName"].ToString();

                Session["sortExpression"] = "CreatedDateTime";
                ViewState["sortColumn"] = "CreatedDateTime";
                ViewState["sortDirection"] = "DESC";

                bindTo();
                visibilityCtrl(true);

                loadExistingAppoint();
                //// Timer1.Enabled = true;
            }
        }

        int getTodayNextSlNo()
        {
            int res = 1;
            try
            {

                var data = (from getdata in new VisitorAPPEntities().InfoMessages
                            where EntityFunctions.TruncateTime(getdata.CreatedDateTime) == EntityFunctions.TruncateTime(DateTime.Today)
                            select getdata).ToList();

                if (data.Count >= 1)
                    res = data.Count + 1;

            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "NAPT101: " + ex.Message);
            }
            return res;
        }



        void displayLabelMessage(bool isError, string messageString)
        {
            msgLabel.Enabled = false;

            lblError.Visible = false;
            lblSuccess.Visible = false;
            if (isError)
            {
                lblError.Visible = true;
                lblError.Text = messageString;
            }
            else
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = messageString;
            }
            msgLabel.Enabled = true;
        }

        protected void msgLabel_Tick(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;
            msgLabel.Enabled = false;
            //UpdatePanel2.Update();
        }

        void clearMainCtrl()
        {
            Session["EditUID"] = "";
            //txt_Address.Text = "";
            txt_Date.Text = DateTime.Now.ToString("dd-MMM-yyy");
            //txt_DateTime.Text = DateTime.Now.ToString("hh:mm tt");
            txt_SlNo.Text = getTodayNextSlNo().ToString();
            txt_FromName.Text = "";
            txt_Message.Text = "";
            txtOrganization.Text = "";
            drp_MeggasePriority.SelectedIndex = 0;
            Session["Edit"] = "0";
            btnBook.Text = "Create Appointment";

            txtOrganization.Visible = false;
            chkExternal.Checked = false;

            //rd_isUrgentYes.Checked = false;
            //rd_isUrgentNo.Checked = true;  
            UpdatePanel2.Update();
            UpdatePanel3.Update();
            pnl3.Update();
        }

        void visibilityCtrl(bool isADDEnable)
        {
            btnBook.Visible = isADDEnable;
            //btnEdit.Visible = !(isADDEnable);            
            //btnNew.Visible = !(isADDEnable);

        }

        void bindTo()
        {
            try
            {
                VisitorAPPEntities ENT = new VisitorAPPEntities();

                var toUser = (from getdata in ENT.InfoUsers
                              join getrole in ENT.TypeUserRoles on getdata.UserRoleID equals getrole.UserRoleID
                              where getdata.UserRoleID == 2 || getdata.UserRoleID == 3
                              where getdata.isActive == true
                              select new
                              {
                                  TO_NAME = getdata.Name,
                                  TO_UID = getdata.UID
                              }).ToList();

                drp_To.DataSource = toUser;

                drp_To.DataTextField = "TO_NAME";

                drp_To.DataValueField = "TO_UID";

                drp_To.DataBind();

                drp_To.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "NAPT102: " + ex.Message);
            }
        }

        void loadExistingAppoint(string filterOption = "", string filterStr = "")
        {
            Timer1.Enabled = false;
            try
            {
                VisitorAPPEntities ENT = new VisitorAPPEntities();
                var data = (from getdata in ENT.InfoMessages
                            join getmsgType in ENT.TypeMsgContents on getdata.msgTypeID equals getmsgType.MsgTypeId
                            join getmsgStatus in ENT.TypeMsgStatus on getdata.msgStatusID equals getmsgStatus.msgStatusID
                            join getfromuser in ENT.InfoUsers on getdata.FromUserID equals getfromuser.UID
                            join getTouser in ENT.InfoUsers on getdata.ToUserID equals getTouser.UID
                            join getfromrole in ENT.TypeUserRoles on getfromuser.UserRoleID equals getfromrole.UserRoleID
                            join gettorole in ENT.TypeUserRoles on getTouser.UserRoleID equals gettorole.UserRoleID
                            where getdata.isActive == true && getdata.msgStatusID < 4
                            //&& ((getdata.msgStatusID == 2 && EntityFunctions.TruncateTime(getdata.CreatedDateTime) == EntityFunctions.TruncateTime(DateTime.Now))
                            //|| getdata.msgStatusID == 1 || getdata.msgStatusID == 3)
                            select (new getAppoint_Result
                            {
                                msgID = getdata.msgID,
                                TodayID = getdata.TodayID,
                                VisitorName = getdata.VisitorName,
                                FromUserID = getdata.FromUserID,
                                FromUserName = getfromuser.Name,
                                FromUserRole = getfromrole.UserRoleDesc,
                                ToUserID = getdata.ToUserID,
                                ToUserName = getTouser.Name,
                                ToUserRole = gettorole.UserRoleDesc,
                                msgTypeID = getdata.msgTypeID,
                                MessageContent = getmsgType.MessageContent,
                                isUrgent = getdata.isUrgent,
                                VisitorMessage = getdata.VisitorMessage,
                                msgStatusDesc = getmsgStatus.msgStatusDesc,
                                CreatedDateTime = getdata.Autotime, //getdata.CreatedDateTime,
                                isActive = getdata.isActive,
                                msgStatusID = getdata.msgStatusID,
                                ResponseString = getdata.ResponseString,
                                ResponseTime = getdata.ResponseTime,
                                PostponeTime = getdata.CreatedDateTime
                            }));

                //.Where(c => c.ResponseString.Length <= 0 && c.ResponseTime.Value.ToString().Length > 0).ToList();//.ForEach(cc => cc.ResponseString = "Postpone");

                if (dg_getAppoint.Items.Count > 0 && filterOption.Length <= 0 && filterStr.Length <= 0)
                {
                    List<getAppoint_Result> oldList = new List<getAppoint_Result>();
                    oldList = (List<getAppoint_Result>)Session["collectedData"];

                    List<getAppoint_Result> newList = data.ToList();

                    bool result = CompareList<getAppoint_Result>(oldList, newList);
                    if (result)
                    {
                        ENT.Connection.Close();
                        return;
                    }
                }

                if ((!(filterOption.Equals("")) && !(filterStr.Equals(""))) || (txt_fileterStr.Text.Length > 0))
                {
                    if (filterOption.Length <= 0)
                    {
                        filterOption = drp_FilterBy.SelectedValue;
                    }
                    if (filterOption == "Visitor Name") data = data.Where(k => k.VisitorName.ToLower().Contains(filterStr.ToLower()));

                    if (filterOption == "Visiting To") data = data.Where(k => k.ToUserName.ToLower().Contains(filterStr.ToLower()));

                    if (filterOption == "Read Status") data = data.Where(k => k.msgStatusDesc.ToLower().Contains(filterStr.ToLower()));

                }


                //report for only current reception user
                if (Session["UserRoleID"].ToString().Equals("1"))
                {
                    int currentUID = int.Parse(Session["UID"].ToString());
                    data = data.Where(k => k.FromUserID == currentUID);
                }
                var data1 = data.ToList();



                dg_getAppoint.CurrentPageIndex = 0;

                dg_getAppoint.VirtualItemCount = data1.Count();

                //dg_getAppoint.DataSource = data.AsQueryable<getAppoint_Result>().OrderBy(sortExpression);
                //dg_getAppoint.DataBind();
                var sortingString = Session["sortExpression"].ToString();
                var param = Expression.Parameter(typeof(getAppoint_Result), sortingString);
                var sortExpression = Expression.Lambda<Func<getAppoint_Result, object>>(Expression.Convert(Expression.Property(param, sortingString), typeof(object)), param);

                if (sortDirection(sortingString).Equals("ASC", StringComparison.InvariantCultureIgnoreCase))
                    dg_getAppoint.DataSource = data1.AsQueryable<getAppoint_Result>().OrderBy(sortExpression);// data1;
                else
                    dg_getAppoint.DataSource = data1.AsQueryable<getAppoint_Result>().OrderByDescending(sortExpression);// data1;

                dg_getAppoint.DataBind();

                //dg_getAppoint.DataSource = data1;
                //dg_getAppoint.DataBind();
                if (filterOption.Length <= 0 && filterStr.Length <= 0)
                    Session["collectedData"] = data1;
                Session["gridLoadedData"] = data1;

                ENT.Connection.Close();


            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "NAPT103: " + ex.Message);
            }
            finally
            {
                Timer1.Enabled = true;
            }
        }

        public bool CompareList<T>(List<T> list1, List<T> list2)
        {
            //if any of the list is null, return false
            if ((list1 == null && list2 != null) || (list2 == null && list1 != null))
                return false;
            //if both lists are null, return true, since its same
            else if (list1 == null && list2 == null)
                return true;
            //if count don't match between 2 lists, then return false
            if (list1.Count != list2.Count)
                return false;
            bool IsEqual = true;
            foreach (T item in list1)
            {
                T Object1 = item;
                T Object2 = list2.ElementAt(list1.IndexOf(item));
                Type type = typeof(T);
                //if any of the object inside list is null and other list has some value for the same object  then return false
                if ((Object1 == null && Object2 != null) || (Object2 == null && Object1 != null))
                {
                    IsEqual = false;
                    break;
                }

                foreach (System.Reflection.PropertyInfo property in type.GetProperties())
                {
                    if (property.Name != "ExtensionData")
                    {
                        string Object1Value = string.Empty;
                        string Object2Value = string.Empty;
                        if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
                            Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();
                        if (type.GetProperty(property.Name).GetValue(Object2, null) != null)
                            Object2Value = type.GetProperty(property.Name).GetValue(Object2, null).ToString();
                        //if any of the property value inside an object in the list didnt match, return false
                        if (Object1Value.Trim() != Object2Value.Trim())
                        {
                            IsEqual = false;
                            break;
                        }
                    }
                }

            }
            //if all the properties are same then return true
            return IsEqual;
        }

        private string sortDirection(string sortExpression, bool triggeredFromUI = false)
        {
            if (!triggeredFromUI)
                return ViewState["sortDirection"].ToString();

            if (ViewState["sortColumn"].ToString().Equals(sortExpression.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if ("ASC" == ViewState["sortDirection"].ToString())
                {
                    ViewState["sortDirection"] = "DESC";
                    return "DESC";
                }
                else
                {
                    ViewState["sortDirection"] = "ASC";
                    return "ASC";
                }
            }
            else
            {
                ViewState["sortColumn"] = sortExpression.ToString();
                ViewState["sortDirection"] = "ASC";
                return "ASC";
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string buildFromName = txt_FromName.Text.Trim();
                    if (chkExternal.Checked)
                        buildFromName += "-" + txtOrganization.Text.Trim();

                    if (btnBook.Text.Equals("Update Appointment", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (txt_SlNo.Text.Length > 0)
                        {
                            VisitorAPPEntities ENT = new VisitorAPPEntities();

                            var query = from data in ENT.InfoMessages
                                        orderby data.msgID
                                        select data;

                            foreach (InfoMessage Item in query)
                            {
                                if (Item.msgID == Convert.ToInt32(txt_SlNo.Text))
                                {
                                    Item.ToUserID = Convert.ToInt32(drp_To.SelectedValue);
                                    Item.isUrgent = drp_MeggasePriority.SelectedIndex == 1 ? true : false;
                                    Item.msgTypeID = byte.Parse(drp_MessageType.SelectedValue);
                                    Item.VisitorName = buildFromName;// txt_FromName.Text;
                                    Item.VisitorMessage = txt_Message.Text;
                                }
                            }

                            ENT.SaveChanges();
                            ENT.Connection.Close();
                            displayLabelMessage(false, "Message ID : " + txt_SlNo.Text + ", Successfully Updated!!!");
                        }

                    }
                    else
                    {
                        VisitorAPPEntities ENT = new VisitorAPPEntities();
                        string screen = "";
                        if (drp_MeggasePriority.SelectedIndex == 1)
                            screen = "Urgent";
                        else
                            screen = "General";

                        int userid = 5;
                        if (drp_To.SelectedValue.ToString().Equals("5"))
                            userid = 5;
                        else
                            userid = 4;

                        SettingsForm _settings = (from data in ENT.SettingsForms
                                                  where data.UserID == userid && data.SettingsName.Equals(screen, StringComparison.InvariantCultureIgnoreCase)
                                                  select data).FirstOrDefault();
                        if (_settings != null)
                        {
                            if (_settings.Enable == false)
                            {
                                displayLabelMessage(true, screen + " turned off in settings");
                                return;
                            }
                        }

                        InfoMessage infomsg = new InfoMessage();

                        infomsg.CreatedDateTime = DateTime.Now;

                        infomsg.FromUserID = int.Parse(Session["UID"].ToString());

                        infomsg.ToUserID = int.Parse(drp_To.SelectedValue);

                        infomsg.isActive = true;

                        infomsg.isUrgent = drp_MeggasePriority.SelectedIndex == 1 ? true : false;

                        infomsg.msgStatusID = 1;

                        infomsg.msgTypeID = byte.Parse(drp_MessageType.SelectedValue);

                        infomsg.TodayID = getTodayNextSlNo();

                        infomsg.VisitorName = buildFromName;// txt_FromName.Text.Trim().TrimEnd().TrimStart();

                        infomsg.VisitorMessage = txt_Message.Text.Trim().TrimEnd().TrimStart();
                        infomsg.Autotime = DateTime.Now;

                        ENT.AddToInfoMessages(infomsg);

                        ENT.SaveChanges();


                        InfoUser imsg = (from data in ENT.InfoUsers
                                         where data.UID == infomsg.ToUserID
                                         select data).FirstOrDefault();
                        string deviceToken = imsg.MobileIMENo;

                        ENT.Connection.Close();



                        VMSNotification _notification = new VMSNotification();
                        int badge = 0;
                        ENT = new VisitorAPPEntities();

                        try
                        {
                            BadgeDetail _badge = (from data in ENT.BadgeDetails
                                                  where data.UserID == userid
                                                  select data).FirstOrDefault();

                            if (_badge != null)
                            {
                                if (_badge.LastCloseTime != null)
                                {
                                    badge = (int)_badge.BadgeCount + 1;
                                    _badge.BadgeCount = badge;
                                    ENT.SaveChanges();
                                }
                            }
                        }
                        catch { }
                        ENT.Connection.Close();

                        if (_notification.PushNotificationToApple(badge, infomsg.VisitorName + " - " + drp_MessageType.SelectedItem.Text, deviceToken))
                            displayLabelMessage(false, "Success");
                        else
                            displayLabelMessage(true, "Message Created but Notification Failed");
                    }
                    clearMainCtrl();

                    visibilityCtrl(true);

                    txt_SlNo.Text = getTodayNextSlNo().ToString();

                    loadExistingAppoint();



                }
                catch (Exception ex)
                {
                    // displayLabelMessage(true, "NAPT104: " + ex.Message);
                    displayLabelMessage(true, "NAPT104: Message creation failed");
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "NAPT105: " + ex.Message);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "NAPT106: " + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearMainCtrl();
                visibilityCtrl(true);

            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "NAPT107: " + ex.Message);
            }
        }

        protected void btnSearchFilter_Click(object sender, EventArgs e)
        {
            try
            {
                loadExistingAppoint(drp_FilterBy.SelectedValue, txt_fileterStr.Text.Trim().TrimStart().TrimEnd());
            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "NAPT108: " + ex.Message);
            }
        }

        protected void dg_getAppoint_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            try
            {
                if (e.Item.ItemType.ToString() != "Header")
                {
                    TableCell cell = e.Item.Cells[5];
                    int quantity = int.Parse(e.Item.Cells[9].Text);
                    if (quantity == 1)
                    {
                        cell.CssClass = "btn btn-block btn-danger";
                        //cell.BackColor = Color.Red;
                    }
                    else if (quantity == 2)
                    {
                        cell.CssClass = "btn btn-block btn-success";
                        //cell.BackColor = Color.Yellow;
                    }
                    else if (quantity == 3)
                    {
                        cell.CssClass = "btn btn-block btn-warning";
                        //cell.BackColor = Color.Orange;
                    }
                    cell = e.Item.Cells[10];
                    if ("" + e.Item.Cells[10].Text.Replace("&nbsp;", "").Trim().TrimEnd().TrimStart() != "")
                    {
                        if (e.Item.Cells[10].Text.Replace("&nbsp;", "").Trim().Equals("Accepted", StringComparison.InvariantCultureIgnoreCase))
                        {
                            cell.CssClass = "btn btn-block btn-success";
                        }
                        else if (e.Item.Cells[10].Text.Replace("&nbsp;", "").Trim().Equals("Rejected", StringComparison.InvariantCultureIgnoreCase))
                        {
                            cell.CssClass = "btn btn-block btn-danger";
                        }
                        else if (e.Item.Cells[10].Text.Replace("&nbsp;", "").Trim().StartsWith("Postpone", StringComparison.InvariantCultureIgnoreCase))
                        {
                            cell.CssClass = "btn btn-block btn-warning";
                        }
                        else
                        {
                            cell.CssClass = "btn btn-block btn-primary";
                        }
                        //cell.CssClass = "btn btn-block btn-info";
                    }

                    try
                    {
                        TableCell createdTimeCell = e.Item.Cells[7];
                        if (!string.IsNullOrEmpty(createdTimeCell.Text))
                            createdTimeCell.Text = Convert.ToDateTime(createdTimeCell.Text).ToString("dd-MMM-yyyy hh:mm tt");
                    }
                    catch { }

                    TableCell responseTimeCell = e.Item.Cells[11];
                    if (!string.IsNullOrEmpty(responseTimeCell.Text.Trim()) && !responseTimeCell.Text.StartsWith("&nbsp", StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            responseTimeCell.Text = Convert.ToDateTime(responseTimeCell.Text).ToString("dd-MMM-yyyy hh:mm tt");
                            foreach (Control editCtrl in e.Item.Cells[12].Controls)
                            {
                                editCtrl.Visible = false;
                            }
                            foreach (Control editCtrl in e.Item.Cells[13].Controls)
                            {
                                editCtrl.Visible = true;
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        foreach (Control editCtrl in e.Item.Cells[13].Controls)
                        {
                            editCtrl.Visible = false;
                        }
                    }

                    if (e.Item.Cells[10].Text.Equals("Postpone", StringComparison.InvariantCultureIgnoreCase))
                        e.Item.Cells[10].ToolTip = e.Item.Cells[14].Text + Environment.NewLine + e.Item.Cells[15].Text;
                }
            }
            catch (Exception ex)
            {
                //displayLabelMessage(true, "NAPT108: " + ex.Message);
            }

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            loadExistingAppoint();
        }

        protected void dg_getAppoint_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            try
            {


                //dg_getSMSReport.DataSource = msgComposes;

                //LoadMaingridData();

                dg_getAppoint.DataSource = null;//= new DataGrid();
                dg_getAppoint.CurrentPageIndex = 0;
                dg_getAppoint.DataBind();


                List<getAppoint_Result> msgComposes = (List<getAppoint_Result>)Session["gridLoadedData"];

                dg_getAppoint.DataSource = msgComposes.ToList().Skip(e.NewPageIndex * 20).Take(20);

                dg_getAppoint.CurrentPageIndex = e.NewPageIndex;

                dg_getAppoint.DataBind();



                //dg_getSMSReportDetailed.sk

            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "NAPT109: " + ex.Message);
            }
        }

        protected void dg_getAppoint_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            List<getAppoint_Result> data = (List<getAppoint_Result>)Session["gridLoadedData"];


            var param = Expression.Parameter(typeof(getAppoint_Result), e.SortExpression);
            var sortExpression = Expression.Lambda<Func<getAppoint_Result, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);

            if (sortDirection(e.SortExpression, true).Equals("ASC", StringComparison.InvariantCultureIgnoreCase))
                dg_getAppoint.DataSource = data.AsQueryable<getAppoint_Result>().OrderBy(sortExpression);// data1;
            else
                dg_getAppoint.DataSource = data.AsQueryable<getAppoint_Result>().OrderByDescending(sortExpression);// data1;

            //dg_getAppoint.DataSource = data.AsQueryable<getAppoint_Result>().OrderBy(sortExpression);
            dg_getAppoint.DataBind();
            Session["sortExpression"] = e.SortExpression;
        }

        protected void dg_getAppoint_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!e.Item.Cells[0].Text.Equals("UID", StringComparison.InvariantCultureIgnoreCase))
            {
                //if (String.IsNullOrEmpty(e.Item.Cells[11].Text))
                //{
                string msgId = e.Item.Cells[0].Text;

                Timer1.Enabled = false;

                if (e.CommandName.Equals("edit", StringComparison.InvariantCultureIgnoreCase))
                {
                    Session["Edit"] = msgId;
                    List<getAppoint_Result> data = (List<getAppoint_Result>)Session["gridLoadedData"];
                    getAppoint_Result Item = data.Where(a => a.msgID.ToString().Equals(msgId)).First();

                    drp_To.SelectedValue = Item.ToUserID.ToString();
                    drp_MeggasePriority.SelectedIndex = Item.isUrgent ? 1 : 0;
                    drp_MessageType.SelectedValue = Item.msgTypeID.ToString();
                    string[] name = Item.VisitorName.Split('-');
                    try
                    {
                        txt_FromName.Text = name[0].ToString(); //Item.VisitorName;
                        if (name.Length > 1)
                        {
                            chkExternal.Checked = true;
                            txtOrganization.Visible = true;
                            txtOrganization.Text = name[1].ToString();
                        }
                        else
                        {
                            chkExternal.Checked = false;
                            txtOrganization.Text = "";
                        }
                    }
                    catch { }
                    txt_Message.Text = Item.VisitorMessage;
                    txt_SlNo.Text = msgId;
                    btnBook.Text = "Update Appointment";
                    UpdatePanel2.Update();
                    UpdatePanel3.Update();

                    Timer1.Enabled = true;
                }
                else
                {
                    VisitorAPPEntities ENT = new VisitorAPPEntities();

                    var query = from data in ENT.InfoMessages
                                orderby data.msgID
                                select data;

                    foreach (InfoMessage Item in query)
                    {
                        if (Item.msgID == Convert.ToInt32(msgId))
                        {
                            Item.msgStatusID = 4;
                        }
                    }

                    ENT.SaveChanges();
                    ENT.Connection.Close();

                    loadExistingAppoint();
                }



                //ENT.SaveChanges();
                //ENT.Connection.Close();
                //clearMainCtrl();
                //visibilityCtrl(true);
                //}
            }
        }

        protected void chkExternal_CheckedChanged(object sender, EventArgs e)
        {
            txtOrganization.Visible = chkExternal.Checked;
            UpdatePanel2.Update();
        }

        //public void SortList<getAppoint_Result>(List<getAppoint_Result> list, string columnName)
        //{
        //    var property = typeof(getAppoint_Result).GetProperty(columnName);
        //    //var multiplier = direction == SortDirection.Descending ? -1 : 1;
        //    list.Sort((t1, t2) =>
        //    {
        //        var col1 = property.GetValue(t1);
        //        var col2 = property.GetValue(t2);
        //        return Comparer<object>.Default.Compare(col1, col2);
        //    });
        //}

    }

    public class getAppoint_Result
    {


        public long msgID { get; set; }
        public int? TodayID { get; set; }
        public string VisitorName { get; set; }
        public int FromUserID { get; set; }
        public string FromUserName { get; set; }
        public string FromUserRole { get; set; }
        public int ToUserID { get; set; }
        public string ToUserName { get; set; }
        public string ToUserRole { get; set; }
        public byte msgTypeID { get; set; }
        public string MessageContent { get; set; }
        public bool isUrgent { get; set; }
        public string VisitorMessage { get; set; }
        public string msgStatusDesc { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public bool isActive { get; set; }
        public byte msgStatusID { get; set; }
        public string ResponseString { get; set; }
        public DateTime? ResponseTime { get; set; }
        public DateTime? PostponeTime { get; set; }
    }
}