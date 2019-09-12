using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using VisitorAppointmentDB;
using System.Linq.Expressions;

namespace VisitorAppointmentWebAPP.iReport
{
    public partial class rpt_AppointStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadExistingAppoint();

                Session["sortExpression"] = "CreatedDateTime";
                ViewState["sortColumn"] = "CreatedDateTime";
                ViewState["sortDirection"] = "DESC";
            }
        }

        void displayLabelMessage(bool isError, string messageString)
        {
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
        }

        void loadExistingAppoint(string filterOption = "", string filterStr = "")
        {
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
                            where getdata.isActive == true
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
                                CreatedDateTime = getdata.Autotime,// getdata.CreatedDateTime,
                                isActive = getdata.isActive,
                                msgStatusID = getdata.msgStatusID,
                                ResponseString = getdata.ResponseString,
                                ResponseTime = getdata.ResponseTime
                            }));

                if (!(filterOption.Equals("")) && !(filterStr.Equals("")))
                {
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
                
                dg_getAppoint.DataSource = data.ToList();

                dg_getAppoint.DataBind();

                Session["gridLoadedData"] = data.ToList();
                ENT.Connection.Close();


            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "RPT101: " + ex.Message);
            }
        }

        protected void dg_getAppoint_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            List<getAppoint_Result>data =(List<getAppoint_Result>)Session["gridLoadedData"];

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
        protected void btnSearchFilter_Click(object sender, EventArgs e)
        {
            try
            {
                loadExistingAppoint(drp_FilterBy.SelectedValue, txt_fileterStr.Text.Trim().TrimStart().TrimEnd());
            }
            catch (Exception ex)
            {
                displayLabelMessage(true, "RPT102: " + ex.Message);
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
                        cell.CssClass = "btn btn-block btn-info";
                    }
                }
            }
            catch (Exception ex)
            {
                //displayLabelMessage(true, "NAPT108: " + ex.Message);
            }

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





    }
}