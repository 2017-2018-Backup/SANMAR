using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VisitorAppointmentDB;

namespace VisitorAppointmentWebAPP.iAppointment
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblSuccess.Visible = false;
            if (!IsPostBack)
            {
                if (getInfo(4, "General") == true)
                    rdoDGen1.Checked = true;
                else
                    rdoDGen2.Checked = true;

                if (getInfo(4, "Urgent") == true)
                    RadioButton3.Checked = true;
                else
                    RadioButton4.Checked = true;

                if (getInfo(5, "General") == true)
                    rdoGen1.Checked = true;
                else
                    rdoGen2.Checked = true;

                if (getInfo(5, "Urgent") == true)
                    RadioButton1.Checked = true;
                else
                    RadioButton2.Checked = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //User Type ID = 5 for chairman
            //User Type Id = 4 for deputy chairman

            //if(CGeneral.SelectedValue.ToString().Equals("Hide", StringComparison.InvariantCultureIgnoreCase))
            //    updateInfo(5, "General", false);
            //else
            //    updateInfo(5, "General", true);
            bool b_Info_1 = true;
            bool b_Info_2 = true;
            bool b_Info_3 = true;
            bool b_Info_4 = true;

            if (rdoDGen1.Checked == true)
                b_Info_1 = updateInfo(4, "General", true);
            else if (rdoDGen2.Checked == true)
                b_Info_1 = updateInfo(4, "General", false);

            if (RadioButton3.Checked == true)
                b_Info_2 = updateInfo(4, "Urgent", true);
            else if (RadioButton4.Checked == true)
                b_Info_2 = updateInfo(4, "Urgent", false);

            if (rdoGen1.Checked == true)
                b_Info_3 = updateInfo(5, "General", true);
            else if (rdoGen2.Checked == true)
                b_Info_3 = updateInfo(5, "General", false);

            if (RadioButton1.Checked == true)
                b_Info_4 = updateInfo(5, "Urgent", true);
            else if (RadioButton2.Checked == true)
                b_Info_4 = updateInfo(5, "Urgent", false);

            if (b_Info_1 && b_Info_2 && b_Info_3 && b_Info_4)
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Success";

                VMSNotification _notification = new VMSNotification();

                try
                {
                    for (int iUserId = 4; iUserId <= 5; iUserId++)
                    {
                        string message = "";
                        if (iUserId == 4)
                        {
                            if (rdoDGen2.Checked == true)
                            {
                                //General = false
                                message = "General";
                            }
                            else if (RadioButton4.Checked == true)
                            {
                                //Urgent = false
                                message = "Urgent";
                            }
                        }
                        else
                        {
                            if (rdoGen2.Checked == true)
                            {
                                //General = false
                                message = "General";
                            }
                            if (RadioButton2.Checked == true)
                            {
                                //Urgent = false
                                message = "Urgent";
                            }
                        }
                        InfoUser imsg = (from data in ENT.InfoUsers
                                         where data.UID == iUserId
                                         select data).FirstOrDefault();
                        string deviceToken = imsg.MobileIMENo;


                        if (_notification.PushNotificationToApple(0, "Message settings changed.", deviceToken, true, message))
                        {
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Success";
                        }
                        else
                        {
                            lblSuccess.Visible = false;

                            lblError.Visible = true;
                            lblError.Text = "Failed to Notify";
                        }
                    }
                }
                catch { }

                ENT.Connection.Close();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Failed to update info";
            }
        }

        VisitorAPPEntities ENT = new VisitorAPPEntities();
        private bool getInfo(int UserID, string screen)
        {
            SettingsForm _settings = (from data in ENT.SettingsForms
                                      where data.UserID == UserID && data.SettingsName.Equals(screen, StringComparison.InvariantCultureIgnoreCase)
                                      select data).FirstOrDefault();
            if (_settings != null)
            {
                if (_settings.Enable == true)
                    return true;
            }
            return false;
        }

        private bool updateInfo(int UserID, string screen, bool value)
        {
            try
            {
                VisitorAPPEntities ENT = new VisitorAPPEntities();
                SettingsForm _settings = (from data in ENT.SettingsForms
                                          where data.UserID == UserID && data.SettingsName.Equals(screen, StringComparison.InvariantCultureIgnoreCase)
                                          select data).FirstOrDefault();
                if (_settings != null)
                {
                    _settings.Enable = value;
                    ENT.SaveChanges();
                    ENT.AcceptAllChanges();
                    ENT.Connection.Close();
                }
                return true;
            }
            catch { return false; }
        }

        protected void rdoGen1_CheckedChanged(object sender, EventArgs e)
        {
            //General - Show - Chairman
        }

        protected void rdoGen2_CheckedChanged(object sender, EventArgs e)
        {
            //General - Hide - Chairman
            if (RadioButton2.Checked == true)
            {
                RadioButton2.Checked = false;
                RadioButton1.Checked = true;
            }
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //Urgent - Show - Chairman
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //Urgent - Hide - Chairman
            if (rdoGen2.Checked == true)
            {
                rdoGen2.Checked = false;
                rdoGen1.Checked = true;
            }
        }

        protected void rdoDGen1_CheckedChanged(object sender, EventArgs e)
        {
            //General - Show - Deputy Chairman
        }

        protected void rdoDGen2_CheckedChanged(object sender, EventArgs e)
        {
            //General - Hide - Deputy Chairman
            if (RadioButton4.Checked == true)
            {
                RadioButton4.Checked = false;
                RadioButton3.Checked = true;
            }
        }

        protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            //Urgent - Show - Deputy Chairman
        }

        protected void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            //Urgent - Hide - Deputy Chairman
            if (rdoDGen2.Checked == true)
            {
                rdoDGen2.Checked = false;
                rdoDGen1.Checked = true;
            }
        }
    }
}