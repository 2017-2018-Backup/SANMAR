using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MBSERPs.iMembers
{
    public partial class callPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;

            try
            {
                if (Request.QueryString.AllKeys.Contains("custname") && Request.QueryString.AllKeys.Contains("custmobileno"))
                {
                    string custname = Request.QueryString["custname"].ToString();
                    string custmobileno = Request.QueryString["custmobileno"].ToString();
                    txt_Mobile.Text = custmobileno;
                    if (custname == "" || custname == string.Empty)
                    {
                        txt_CustName.Text = "";
                        txt_CType.Text = "NEW CUSTOMER";
                    }
                    else
                    {
                        txt_CustName.Text = custname.ToUpper(); ;
                        txt_CType.Text = "EXISTING CUSTOMER";
                    }
                }
                else
                {
                    displayLabelMessage(true, "Error: CALLP100: Data mismatch error!!");
                }
            }

            catch (Exception ex)
            {
                //Response.Redirect("../errorpage/errorinfo.aspx?errorHeading=Login Issue&errorContent=" + ex.Message);
                displayLabelMessage(true, "Error: CALLP101 " + ex.Message);
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Response.Redirect("../iBooking/ordernew.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}