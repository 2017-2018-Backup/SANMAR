using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VisitorAppointmentDB;

namespace MBSERPs.iMembers
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txt_UserId.Text = Session["UserName"].ToString();

            if (!IsPostBack)
            {
                try
                {
                    
                }

                catch (Exception err)
                {
                    displayLabelMessage(true, "CPWD101: " + err.Message);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (txt_CurrentPwd.Text != "" && txtNewPwd.Text != "" && txt_ReEnterNewPwd.Text != "")
                {

                    string username = Session["UserName"].ToString();
                    VisitorAPPEntities ENT = new VisitorAPPEntities();

                    InfoUser isvalidOldPwd = (from p in ENT.InfoUsers
                                              where p.Username == username && p.Password == txt_CurrentPwd.Text
                                              select p).FirstOrDefault();

                    
                    if (isvalidOldPwd != null)
                    {
                        if (txtNewPwd.Text == txt_ReEnterNewPwd.Text)
                        {
                            isvalidOldPwd.Password = txtNewPwd.Text;
                            ENT.InfoUsers.ApplyCurrentValues(isvalidOldPwd);
                            ENT.SaveChanges();

                            displayLabelMessage(false, "Password successfully reset!!");

                        }
                        else
                        {
                            displayLabelMessage(true, "Re Enter Password Mismatch with New Password Entered!!");
                        }
                    }
                    else
                    {
                        displayLabelMessage(true, "Old password is incorrect!!");

                    }

                }

                else
                {
                    displayLabelMessage(true, "Fill all fields!");
                }
            }

            catch (Exception err)
            {
                displayLabelMessage(true, "CPWD103: " + err.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("../dashboard/home.aspx");
            }

            catch (Exception err)
            {
                displayLabelMessage(true, "CPWD103: " + err.Message);
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
    }
}