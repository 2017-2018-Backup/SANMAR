using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using VisitorAppointmentDB;  


namespace VisitorAppointmentWebAPP.iMembers
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

            try
            {
                if (Request.QueryString.AllKeys.Contains("FEIMENO"))
                {
                    string FEIMENO = Request.QueryString["FEIMENO"].ToString();
                    if (FEIMENO != null || FEIMENO != string.Empty)
                    {
                        //enable login based on ime no, session etc...
                        Response.Redirect("../iExecutive/allocatedJobs.aspx");
                    }
                    else
                    {
                        Response.Redirect("../errorpage/errorinfo.aspx?errorHeading=Login Issue&errorContent=Please Restart App or");
                    }
                }


            }
            catch (Exception ex)
            {
                //Response.Redirect("../errorpage/errorinfo.aspx?errorHeading=Login Issue&errorContent=" + ex.Message);
                displayLabelMessage(true, "Error: LOGIN100: " + ex.Message);
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

        void MenuAcceessContrl(int roleID)
        {
          
            //Reception - Booking, Report, Profile
            //Admin - Booking, Report, Profile, UserProfile
            string AccessableMenusAndStattupPage = (from getdata in new VisitorAPPEntities().TypeUserRoles
                                                    where getdata.UserRoleID == roleID
                                                    select getdata.UserMenuAccess).FirstOrDefault();

            Session["AccessableMenus"] = AccessableMenusAndStattupPage;

            //string StartupPage = AccessableMenusAndStattupPage.Split(':')[1];
            
            //Response.Redirect(StartupPage);

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //Normal Login
                VisitorAPPEntities ENT = new VisitorAPPEntities();
                var userAccount = (from uinfo in ENT.InfoUsers
                 where uinfo.Username.ToLower().Contains(txtUserName.Text.Trim().ToLower())
                    && uinfo.Password.Equals(txtPassWord.Text.Trim())
                     //&& (uinfo.TypeUserRole == "" || uinfo.UserRoleType == "SubUser")
                    && uinfo.isActive  == true
                 select new { uinfo.UID , uinfo.UserRoleID, uinfo.Name    }).FirstOrDefault();

                if (userAccount != null)
                {
                    Session["UserName"] = txtUserName.Text.Trim();
                    Session["UID"] = userAccount.UID;
                    Session["UserRoleID"] = userAccount.UserRoleID;
                    Session["IpAddress"] = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    //Session["UserBranchCode"] = userAccount.biBranchCode;

                    //if (userAccount.TypeUserRole == 92)
                    //{
                    //    Response.Redirect("../sms/smscompose.aspx");
                    //}
                    //else if (userAccount.TypeUserRole == 96)
                    //{
                    //    Response.Redirect("../cpanel/admintask.aspx");
                    //}
                    //else
                    //{

                    //}

                    MenuAcceessContrl(int.Parse(Session["UserRoleID"].ToString()));

                    //Response.Redirect("../iBooking/ordernew.aspx");

                    string StartupPage = userAccount.UserRoleID == 1 ? "../iAppointment/NewAppointment.aspx" : "../iReport/rpt_AppointStatus.aspx";

                    Response.Redirect(StartupPage);

                }
                else
                {
                    displayLabelMessage(true, "Access Is Denied. User Is Not Valid.");
                    Session.Clear();
                    Session.RemoveAll();
                    Session.Abandon();
                    Session["UserName"] = string.Empty;
                }



            }

            catch (Exception err)
            {
                displayLabelMessage(true, "Logn101: " + err.Message);
            }
        }
    }
}