using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VisitorAppointmentWebAPP.master
{
    public partial class admin : System.Web.UI.MasterPage
    {
        //90-BOOKING STAFF  -- Booking,Report,Complaints
        //91-BACK OFFICE ADMIN -- Booking,Assigning,Dispatch,Report,Tracking,Complaints
        //92-SERVICE MANAGER Booking,Assigning,Dispatch,Report,Tracking,Complaints
        //93-OPERATION MANAGER Booking,Assigning,Dispatch,Master,Report,Tracking,Complaints
        //94-IT SUPPORT Booking,Complaints
        //95-TRAINING Booking,Assigning,Dispatch,Complaints
        //96-HR Master,Report,Complaints
        //97-CGITS Booking,Assigning,Dispatch,Master,Report,Tracking,Complaints
        //98-BDO Report
        //99-ACCOUNTS Report
        //100-VP Booking,Assigning,Dispatch,Master,Report,Tracking,Complaints
        //101-MD       Booking,Tracking
               

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Reception - Booking, Report, Profile
                //Admin - Booking, Report, Profile, UserProfile
                //for testing - Admin
                //Session["UserName"] = "admin";
                //Session["UserRoleID"] = "4";
                //Session["UID"] = "3";
                //Session["AccessableMenus"] = "Report, Master";

                //for testing - Reception
                //Session["UserName"] = "David";
                //Session["UserRoleID"] = "1";
                //Session["UID"] = "1";
                //Session["AccessableMenus"] = "VMS, Report, Master"; // for testing master add

                if (Session["UserName"] != null)
                {
                    if (!IsPostBack)
                    {                      

                        lblLoginName.Text = Session["UserName"].ToString();

                        int uid = int.Parse(Session["UID"].ToString());

                        string AccessableMenus = Session["AccessableMenus"].ToString();

                        foreach (string AccessableMenu in AccessableMenus.Split(','))
                        {
                            this.FindControl("divMenu_" + AccessableMenu.Trim().TrimEnd().TrimStart() ).Visible = true;
                        }

                    }
                }
                else
                {
                    //Session.Clear();
                    //Session.RemoveAll();
                    //Session.Abandon();
                    Session["UserName"] = string.Empty;
                    Session["UserAccountId"] = string.Empty;
                    Session["UserBranchCode"] = string.Empty;
                    Session.Clear();
                    Response.Redirect("../iMembers/login.aspx");
                }
            }
            catch (Exception err)
            {
                //displayLabelMessage(true, "Logn101: " + err.Message);
            }
        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            Response.Redirect("../iMembers/ChangePassword.aspx");
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                Session["UserName"] = string.Empty;
                Response.Redirect("../iMembers/login.aspx");

            }

            catch (Exception err)
            {
                //displayLabelMessage(true, "Logn101: " + err.Message);
            }
        }

        //void displayLabelMessage(bool isError, string messageString)
        //{
        //    lblError.Visible = false;
        //    lblSuccess.Visible = false;
        //    if (isError)
        //    {
        //        lblError.Visible = true;
        //        lblError.Text = messageString;
        //    }
        //    else
        //    {
        //        lblSuccess.Visible = true;
        //        lblSuccess.Text = messageString;
        //    }
        //}
    }
}