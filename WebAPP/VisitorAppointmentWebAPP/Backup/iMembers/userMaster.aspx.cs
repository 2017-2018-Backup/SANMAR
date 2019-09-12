using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VisitorAppointmentDB;

namespace VisitorAppointmentWebAPP.iMembers
{
    public partial class userMaster : System.Web.UI.Page
    {
        void clearMainCtrl()
        {
            Session["EditUID"] = "";
            //txt_Address.Text = "";
            txt_Email.Text = "";
            txt_MobileNo.Text = "";
            txt_Name.Text = "";
            txt_Pass.Text = "";
            txt_PassConfirm.Text = "";
            txt_UserName.Text = "";
            drp_UserRole.SelectedValue = "1";
            txt_MobileIMENo.Text = "";

        }

        void visibilityCtrl(bool isADDEnable)
        {
            btnBook.Visible = isADDEnable;
            btnEdit.Visible = !(isADDEnable);
            txt_Pass.ReadOnly = !(isADDEnable);
            txt_PassConfirm.ReadOnly = !(isADDEnable);
            btnAddNew.Visible = !(isADDEnable);
            txt_UserName.ReadOnly = !(isADDEnable);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;

                lblSuccess.Visible = false;

                if (!IsPostBack)
                {
                    clearMainCtrl();

                    visibilityCtrl(true);

                    loadExistingUser();

                }
            }

            catch (Exception err)
            {
                displayLabelMessage(true, "UAC101: " + err.Message);
            }
        }

        protected void isUsernameAllreadyExistSeverValidation(object sender, ServerValidateEventArgs e)
        {
            string username = e.Value.Trim().TrimEnd().TrimStart();

            if (isUsernameAllreadyExist(username))
                e.IsValid = false;
            else
                e.IsValid = true;

        }

        bool isUsernameAllreadyExist(string username)
        {
            bool res = false;

            var isExist = (from getdata in new VisitorAPPEntities().InfoUsers
                           where getdata.Username  == username
                           select getdata.UID).ToList();

            if (isExist.Count >= 1) res = true;

            return res;

        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (isUsernameAllreadyExist(txt_UserName.Text.Trim().TrimEnd().TrimStart()))
                {
                    displayLabelMessage(true, "UserName : " + txt_UserName.Text + ",  Already Exist!!! Please Choose Different name!!!");
                    return;
                }

                VisitorAPPEntities ENT = new VisitorAPPEntities();

                InfoUser infouser = new InfoUser();

                //string biBranchCode = Session["UserBranchCode"].ToString();

                //infouser.biBranchCode = biBranchCode;

                infouser.Name = txt_Name.Text.Trim().TrimEnd().TrimStart();

                infouser.MobileNo = txt_MobileNo.Text.Trim().TrimEnd().TrimStart();

                infouser.EmailID = txt_Email.Text.Trim().TrimEnd().TrimStart();

                infouser.UserRoleID = byte.Parse(drp_UserRole.SelectedValue);

                //infouser.biUserRole = int.Parse(drp_UserRole.SelectedValue);

                infouser.Username = txt_UserName.Text.Trim().TrimEnd().TrimStart();

                infouser.Password  = txt_Pass.Text.Trim().TrimEnd().TrimStart();

                if (txt_MobileIMENo.Text != "" || txt_MobileIMENo != null) infouser.MobileIMENo  = txt_MobileIMENo.Text.Trim().TrimEnd().TrimStart();

                infouser.isActive  = chk_isActive.Checked;

                ENT.AddToInfoUsers(infouser);

                ENT.SaveChanges();

                ENT.Connection.Close();

                clearMainCtrl();

                loadExistingUser();

                visibilityCtrl(true);

                displayLabelMessage(false, "UserName : " + infouser.Username + ",  Successfully Created!!!");

            }
            catch (Exception err)
            {
                displayLabelMessage(true, "UAC102: " + err.Message);
            }
        }

        protected void dg_getOrderHistory_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    Session["EditUID"] = e.Item.Cells[0].Text;
                    txt_UserName.Text = e.Item.Cells[1].Text;
                    txt_Name.Text = e.Item.Cells[2].Text;
                    txt_MobileNo.Text = e.Item.Cells[3].Text;
                    txt_Email.Text = e.Item.Cells[4].Text;
                    txt_MobileIMENo.Text = e.Item.Cells[6].Text.Replace("&nbsp;", "");
                    //txt_Address.Text = e.Item.Cells[8].Text;
                    chk_isActive.Checked = bool.Parse(e.Item.Cells[8].Text);
                    drp_UserRole.SelectedIndex = drp_UserRole.Items.IndexOf(drp_UserRole.Items.FindByValue(e.Item.Cells[7].Text));
                    visibilityCtrl(false);

                    if (e.Item.Cells[7].Text == "2" || e.Item.Cells[7].Text == "3")
                        lbl_MobileIMENo.Visible = txt_MobileIMENo.Visible = true;
                    else
                        lbl_MobileIMENo.Visible = txt_MobileIMENo.Visible = false;

                }
            }
            catch (Exception err)
            {
                displayLabelMessage(true, "UAC103: " + err.Message);
            }
        }

        void loadExistingUser(string filterOption, string filterStr)
        {
            try
            {

                VisitorAPPEntities ENT = new VisitorAPPEntities();
                var data = (from getdate in ENT.InfoUsers
                            join getrole in ENT.TypeUserRoles on getdate.UserRoleID equals getrole.UserRoleID
                            where getrole.UserRoleID != 1 
                            select new
                            {
                                getdate.UID,
                                getdate.Username,
                                getdate.MobileNo,
                                getdate.EmailID,
                                getdate.UserRoleID ,
                                vcRole = getrole.UserRoleDesc,
                                getdate .Name ,
                                getdate.MobileIMENo,                               
                                getdate.isActive 
                            }).ToList();

                if (filterOption == "Username") data = data.Where(k => k.Username.ToLower().Contains(filterStr.ToLower())).ToList();

                if (filterOption == "FirstName") data = data.Where(k => k.Name.ToLower().Contains(filterStr.ToLower())).ToList();

                if (filterOption == "MobileNo") data = data.Where(k => k.MobileNo.ToLower().Contains(filterStr.ToLower())).ToList();

                if (filterOption == "MailID") data = data.Where(k => k.EmailID.ToLower().Contains(filterStr.ToLower())).ToList();

                dg_getUser.DataSource = data;

                dg_getUser.DataBind();

                ENT.Connection.Close();


            }
            catch (Exception err)
            {
                displayLabelMessage(true, "UAC104: " + err.Message);
            }
        }

        void loadExistingUser()
        {
            try
            {
                VisitorAPPEntities ENT = new VisitorAPPEntities();
                var data = (from getdate in ENT.InfoUsers
                            join getrole in ENT.TypeUserRoles on getdate.UserRoleID equals getrole.UserRoleID
                            where getrole.UserRoleID != 4 
                            select new
                            {
                                getdate.UID,
                                getdate.Name,
                                getdate.Username,
                                getdate.MobileNo,
                                getdate.EmailID,
                                getdate.UserRoleID,
                                vcRole = getrole.UserRoleDesc,                                
                                getdate.MobileIMENo,
                                getdate.isActive
                            }).ToList();

                dg_getUser.DataSource = data;

                dg_getUser.DataBind();

                ENT.Connection.Close();


            }
            catch (Exception err)
            {
                displayLabelMessage(true, "UAC104: " + err.Message);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int UID1 = int.Parse(Session["EditUID"].ToString());

                VisitorAPPEntities ENT = new VisitorAPPEntities();

                InfoUser infouser = (from getdata in ENT.InfoUsers
                                     where getdata.UID == UID1
                                     select getdata).FirstOrDefault();

                if (infouser != null)
                {
                    infouser.Name = txt_Name.Text;
                    infouser.MobileNo = txt_MobileNo.Text;
                    infouser.EmailID = txt_Email.Text;
                    infouser.UserRoleID  = byte.Parse(drp_UserRole.SelectedValue);
                    infouser.MobileIMENo = txt_MobileIMENo.Text;                    
                    infouser.isActive = chk_isActive.Checked;

                    ENT.SaveChanges();

                    ENT.Connection.Close();

                    clearMainCtrl();

                    loadExistingUser();

                    visibilityCtrl(true);

                    displayLabelMessage(false, "UserName : " + infouser.Username + ",  Update Successfully!!!");

                }
                else
                {
                    displayLabelMessage(false, "Select User Recored from below table and then Continue!!");
                }


            }
            catch (Exception err)
            {
                displayLabelMessage(true, "UAC105: " + err.Message);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                clearMainCtrl();
                visibilityCtrl(true);
            }
            catch (Exception err)
            {
                displayLabelMessage(true, "UAC106: " + err.Message);
            }
        }

        protected void drp_UserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //lbl_MobileIMENo.Visible = txt_MobileIMENo.Visible =(drp_UserRole.SelectedValue == "90" || drp_UserRole.SelectedValue == "91");

                if (drp_UserRole.SelectedValue == "2" || drp_UserRole.SelectedValue == "3")
                    lbl_MobileIMENo.Visible = txt_MobileIMENo.Visible = true;
                else
                    lbl_MobileIMENo.Visible = txt_MobileIMENo.Visible = false;
            }
            catch (Exception err)
            {
                displayLabelMessage(true, "UAC107: " + err.Message);
            }
        }

        protected void btnSearchFilter_Click(object sender, EventArgs e)
        {
            try
            {
                loadExistingUser(drp_FilterBy.SelectedValue, txt_fileterUsername.Text);
            }
            catch (Exception err)
            {
                displayLabelMessage(true, "UAC108: " + err.Message);
            }
        }
       
    }
}