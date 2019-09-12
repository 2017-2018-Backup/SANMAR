using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MBSDB;
using MBSCommonWS;
using System.Drawing;
using System.Globalization;

namespace MBSERPs.iMembers
{
    public partial class editCustomer : System.Web.UI.Page
    {

        void clearMainCtrl()
        {
            txt_MobileNo.Text = "";
            txt_Name.Text = "";
            txt_AltMobileNo.Text = "";
            txt_Email.Text = "";
            drp_SourceType.SelectedValue = "71";
            txt_Location.Text = "";
            txt_Address.Text = "";
            txt_Landmark.Text = "";
            rd_isPromNo.Checked = true;
            Session["custType"] = "";
            Session["CustImage"] = "";
            Session["CustImageName"] = "";
            Session["CID"] = "";
            lbl_custType.Text = "";
            txt_KMToCustomerHouse.Text = "";
            img_CustomerPhoto.ImageUrl = "";
            txt_MembershipcardBardCodeNo.Text = "";
            txt_MembershipcardExpireDt.Text = "";
            txt_MembershipcardNumber.Text = "";
            txt_MembershipcardRegiDt.Text = "";
           
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

        void visibilityCtrl(bool isADDEnable)
        {
            btnBook.Visible = isADDEnable;
            btnEdit.Visible = !(isADDEnable);
            btnAddNew.Visible = !(isADDEnable);
            txt_MobileNo.ReadOnly = !(isADDEnable);
        }

        bool isCustomerAllreadyExist(string mobileno)
        {
            bool res = false;
            string last10No = mobileno.GetLast(10);
            var isExist = (from getdata in new MBSDBEntities().InfoCustomers
                           where getdata.vcMobileNo.EndsWith(last10No)
                           select getdata.biCID).ToList();

            if (isExist.Count >= 1) res = true;

            return res;

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

                    if (Request.QueryString.AllKeys.Contains("reqType"))
                    {
                        string reqType = Request.QueryString["reqType"].ToString();

                        if (reqType == "edit")
                        {
                            if (Request.QueryString.AllKeys.Contains("custmobileno"))
                            {
                                string mobno = Request.QueryString["custmobileno"].ToString().iStrTrim().GetLast(10);
                                txt_MobileNo.Text = mobno;
                                LoadCustomerData(mobno);
                            }
                            else
                            {
                                displayLabelMessage(true, "Request not in correct format!!!");
                            }
                        }
                        else
                        {
                            displayLabelMessage(true, "Request not in correct format!!!");
                        }
                    }
                    else
                    {
                        Div_isMem.Visible = false;
                        rd_isMembershipcardNo.Checked = true;
                    }

                }
            }

            catch (Exception err)
            {
                displayLabelMessage(true, "ECUST101: " + err.Message);
            }
        }

        void LoadCustomerData(string mobileNo)
        {
            InfoCustomer custInfo = new iCommonUtility().getCust(mobileNo);

            if (custInfo != null)
            {

                lbl_custType.Text = "Existing User";

                Session["custType"] = "Existing User";

                Session["CID"] = custInfo.biCID;

                txt_Name.Text = custInfo.vcFirstName;

                txt_AltMobileNo.Text = custInfo.vcLandlineNo;

                txt_Email.Text = custInfo.vcMailID;

                drp_SourceType.SelectedIndex = drp_SourceType.Items.IndexOf(drp_SourceType.Items.FindByValue(custInfo.biSourceType.ToString()));

                txt_Location.Text = custInfo.vcLocation;

                txt_KMToCustomerHouse.Text = custInfo.vcLocationDistance;

                txt_Address.Text = custInfo.vcAddress;

                txt_Landmark.Text = custInfo.vcLandmark;

                if ((bool)custInfo.isNeedPromoSMSMAIL)
                {
                    rd_isPromYes.Checked = true;
                    rd_isPromNo.Checked = false;
                }
                else
                {
                    rd_isPromYes.Checked = false;
                    rd_isPromNo.Checked = true;
                }
                visibilityCtrl(false);
                try
                {
                    if ((bool)custInfo.isMembership)
                    {
                        rd_isMembershipcardYes.Checked = true;
                        rd_isMembershipcardNo.Checked = false;
                        Div_isMem.Visible = true;

                        InfoCustMembershipCard custMInfo = (from getdata in new MBSDBEntities().InfoCustMembershipCards
                                                            where getdata.biCID == custInfo.biCID
                                                            select getdata).FirstOrDefault();
                        if (custMInfo != null)
                        {
                            drp_MembercardType.SelectedIndex = drp_MembercardType.Items.IndexOf(drp_MembercardType.Items.FindByValue(custMInfo.biMemCardTypeID.Value.ToString()));
                            txt_MembershipcardNumber.Text = custMInfo.MemCardNo;
                            txt_MembershipcardBardCodeNo.Text = custMInfo.MemBarCodeNO;
                            txt_MembershipcardRegiDt.Text = custMInfo.dtMemRegiDate.HasValue ? custMInfo.dtMemRegiDate.Value.ToString("dd/MM/yyyy") : "";
                            txt_MembershipcardExpireDt.Text = custMInfo.dtMemExpireDate.HasValue ? custMInfo.dtMemExpireDate.Value.ToString("dd/MM/yyyy") : "";

                        }
                    }
                    else
                    {
                        rd_isMembershipcardNo.Checked = true;
                        Div_isMem.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                }

                //For Customer Images
                if (custInfo.imgCustomerPhoto != null)
                {
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(custInfo.imgCustomerPhoto);
                    System.Drawing.Image CustomerImage = System.Drawing.Image.FromStream(memoryStream);
                    CustomerImage.Save(Server.MapPath("~/UploadedFiles/Temp Loc/Images/" + custInfo.imgCustomerFileName));

                    img_CustomerPhoto.ImageUrl = "~/UploadedFiles/Temp Loc/Images/" + custInfo.imgCustomerFileName;

                    Session["CustImage"] = CustomerImage;
                    Session["CustImageName"] = custInfo.imgCustomerFileName;
                }
            }
            else
            {
                visibilityCtrl(true);

                lbl_custType.Text = "New User";

                Session["custType"] = "New User";

                Session["CID"] = "";

                rd_isMembershipcardNo.Checked = true;

                Div_isMem.Visible = false;

                displayLabelMessage(true, "Mobile Number not found in Customer Data");

            }
        }

        protected void btn_getCust_Click(object sender, EventArgs e)
        {
            try
            {

                string mobno = txt_MobileNo.Text.iStrTrim().GetLast(10);

                LoadCustomerData(mobno);

            }
            catch (Exception err)
            {
                displayLabelMessage(true, "ECUST102: " + err.Message);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int CID = int.Parse(Session["CID"].ToString());

                //MBSDBEntities ENT = new MBSDBEntities();

                //InfoCustomer custInfo = (from getdate in ENT.InfoCustomers
                //                         where getdate.biCID == CID
                //                         select getdate).FirstOrDefault();

                //if (custInfo != null)
                //{
                //    //custInfo.vcFirstName = txt_Name.Text;

                //    //custInfo.vcLandlineNo = txt_AltMobileNo.Text;

                //    //custInfo.vcMailID = txt_Email.Text;

                //    //custInfo.biSourceType = int.Parse(drp_SourceType.SelectedItem.Value);

                //    //custInfo.vcLocation = txt_Location.Text;

                //    //custInfo.vcLocationDistance = txt_KMToCustomerHouse.Text;

                //    //custInfo.vcAddress = txt_Address.Text;

                //    //custInfo.vcLandmark = txt_Landmark.Text;

                //    //if (rd_isPromYes.Checked)
                //    //{
                //    //    custInfo.isNeedPromoSMSMAIL = true;
                //    //}

                //    //else if (rd_isPromNo.Checked)
                //    //{
                //    //    custInfo.isNeedPromoSMSMAIL = false;
                //    //}

                //    //custInfo.vcUpdatedBy = Session["UserName"].ToString();

                //    //custInfo.dtUpdatetime = DateTime.Now;

                //    //ENT.SaveChanges();

                //    //ENT.Connection.Close();
                    
                    getCreatecustInfo();

                    clearMainCtrl();

                    displayLabelMessage(false, "Customer Data Updated Successfully!!!");

                //}
                //else
                //{

                //    displayLabelMessage(true, "Request not in correct format!!!");
                //}
            }
            catch (Exception err)
            {
                displayLabelMessage(true, "ECUST103: " + err.Message);
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            try
            {
                getCreatecustInfo();
            }
            catch (Exception err)
            {
                displayLabelMessage(true, "ECUST104: " + err.Message);
            }
        }

        InfoCustomer getCreatecustInfo()
        {
            string empName = Session["UserName"].ToString();

            string biBranchCode = Session["UserBranchCode"].ToString();

            InfoCustomer custInfo = new InfoCustomer();

            string mobileNo = txt_MobileNo.Text.Trim().TrimEnd().TrimStart().GetLast(10);

            if (lbl_custType.Text == "" || lbl_custType.Text == string.Empty)
                LoadCustomerData(mobileNo);

            if (Session["custType"] == "New User")
            {
                MBSDBEntities ENT = new MBSDBEntities();

                ENT.Connection.Open();

                System.Data.Common.DbTransaction dbCon = ENT.Connection.BeginTransaction();

                try
                {
                    custInfo = new InfoCustomer();

                    custInfo.biBranchCode = biBranchCode;

                    custInfo.vcUsername = mobileNo;

                    custInfo.vcPassword = new Random().Next(0, 1000000).ToString("D6");

                    custInfo.biUserRole = 97;

                    custInfo.vcFirstName = txt_Name.Text.Trim();

                    custInfo.vcLastName = "";

                    custInfo.vcMobileNo = mobileNo;

                    custInfo.vcLandlineNo = txt_AltMobileNo.Text.Trim();

                    custInfo.vcMailID = txt_Email.Text.Trim();

                    custInfo.vcLocation = txt_Location.Text.Trim();

                    custInfo.vcLocationDistance = txt_KMToCustomerHouse.Text.Trim();

                    custInfo.vcAddress = txt_Address.Text.Trim();

                    custInfo.vcLandmark = txt_Landmark.Text.Trim();

                    custInfo.biSourceType = int.Parse(drp_SourceType.SelectedValue);

                    custInfo.vcCreatedBy = empName;

                    custInfo.dtCreatedtime = DateTime.Now;

                    custInfo.isNeedPromoSMSMAIL = rd_isPromYes.Checked == true ? true : false;

                    custInfo.dtPromoUpdateDatetime = DateTime.Now;

                    custInfo.isAtive = true;

                    bool isMem = false;

                    if (rd_isMembershipcardYes.Checked)
                    {
                        custInfo.isMembership = true;
                        isMem = true;
                    }
                    else if (rd_isMembershipcardNo.Checked)
                        custInfo.isMembership = false;

                    //to upload photos
                    if (Session["CustImage"] != null)
                    {

                        System.Drawing.Image driverImage = (System.Drawing.Image)Session["CustImage"];
                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                        driverImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageByte = memoryStream.ToArray();
                        custInfo.imgCustomerPhoto = imageByte;
                        custInfo.imgCustomerFileName = Session["CustImageName"].ToString();

                    }                   

                    ENT.AddToInfoCustomers(custInfo);
                    
                    ENT.SaveChanges();

                    if (isMem)
                    {
                        InfoCustMembershipCard custMCard = new InfoCustMembershipCard();
                        custMCard.biCID = custInfo.biCID;
                        custMCard.MemCardNo = txt_MembershipcardNumber.Text.iStrTrim();
                        custMCard.MemBarCodeNO = txt_MembershipcardBardCodeNo.Text.iStrTrim();
                        custMCard.biMemCardTypeID = int.Parse(drp_MembercardType.SelectedItem.Value);
                        custMCard.dtMemRegiDate = DateTime.Parse(txt_MembershipcardRegiDt.Text, CultureInfo.GetCultureInfo("en-gb"));
                        custMCard.dtMemExpireDate = DateTime.Parse(txt_MembershipcardExpireDt.Text, CultureInfo.GetCultureInfo("en-gb"));
                        custMCard.vcCreatedBy = empName;
                        custMCard.dtCreatedTime = DateTime.Now;
                        custMCard.isActive = true;
                        ENT.AddToInfoCustMembershipCards(custMCard);
                    }
                    
                    ENT.SaveChanges();

                    dbCon.Commit();

                    ENT.Connection.Close();

                    displayLabelMessage(false, "User Added Successfully!!");

                    clearMainCtrl();

                }
                catch (Exception ex)
                {
                    dbCon.Rollback();
                    displayLabelMessage(true, "ECUST105: " + ex.Message);
                }

            }
            else
            {
                MBSDBEntities ENT = new MBSDBEntities();

                ENT.Connection.Open();

                System.Data.Common.DbTransaction dbCon = ENT.Connection.BeginTransaction();

                try
                {

                    custInfo = (from getdate in ENT.InfoCustomers
                                where getdate.vcMobileNo == mobileNo
                                select getdate).FirstOrDefault();

                    custInfo.vcFirstName = txt_Name.Text.Trim();

                    custInfo.vcLandlineNo = txt_AltMobileNo.Text.Trim();

                    custInfo.vcMailID = txt_Email.Text.Trim();

                    custInfo.vcLocation = txt_Location.Text.Trim();

                    custInfo.vcLocationDistance = txt_KMToCustomerHouse.Text.Trim();

                    custInfo.vcAddress = txt_Address.Text.Trim();

                    custInfo.vcUpdatedBy = empName;

                    custInfo.dtUpdatetime = DateTime.Now;

                    bool isMem = false;

                    if (rd_isMembershipcardYes.Checked)
                    {
                        custInfo.isMembership = true;
                        isMem = true;
                    }
                    else if (rd_isMembershipcardNo.Checked)
                        custInfo.isMembership = false;


                    if (custInfo.isNeedPromoSMSMAIL != rd_isPromYes.Checked)
                    {

                        custInfo.isNeedPromoSMSMAIL = rd_isPromYes.Checked == true ? true : false;
                        custInfo.dtPromoUpdateDatetime = DateTime.Now;
                    }                    

                    //to upload photos
                    if (Session["CustImage"] != null)
                    {

                        System.Drawing.Image driverImage = (System.Drawing.Image)Session["CustImage"];
                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                        driverImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageByte = memoryStream.ToArray();
                        custInfo.imgCustomerPhoto = imageByte;
                        custInfo.imgCustomerFileName = Session["CustImageName"].ToString();

                    }

                    ENT.SaveChanges();

                    if (isMem)
                    {
                        InfoCustMembershipCard custMCard = (from getdata in ENT.InfoCustMembershipCards
                                                            where getdata.biCID == custInfo.biCID
                                                            select getdata).FirstOrDefault();

                        if (custMCard == null)
                        {

                            custMCard = new InfoCustMembershipCard();
                            custMCard.biCID = custInfo.biCID;
                            custMCard.MemCardNo = txt_MembershipcardNumber.Text.iStrTrim();
                            custMCard.MemBarCodeNO = txt_MembershipcardBardCodeNo.Text.iStrTrim();
                            custMCard.biMemCardTypeID = int.Parse(drp_MembercardType.SelectedItem.Value);
                            custMCard.dtMemRegiDate = DateTime.Parse(txt_MembershipcardRegiDt.Text, CultureInfo.GetCultureInfo("en-gb"));
                            custMCard.dtMemExpireDate = DateTime.Parse(txt_MembershipcardExpireDt.Text, CultureInfo.GetCultureInfo("en-gb"));
                            custMCard.vcCreatedBy = empName;
                            custMCard.dtCreatedTime = DateTime.Now;
                            custMCard.isActive = true;
                            ENT.AddToInfoCustMembershipCards(custMCard);
                            ENT.SaveChanges();
                        }
                        else
                        {
                            custMCard.biCID = custInfo.biCID;
                            custMCard.MemCardNo = txt_MembershipcardNumber.Text.iStrTrim();
                            custMCard.MemBarCodeNO = txt_MembershipcardBardCodeNo.Text.iStrTrim();
                            custMCard.biMemCardTypeID = int.Parse(drp_MembercardType.SelectedItem.Value);
                            custMCard.dtMemRegiDate = DateTime.Parse(txt_MembershipcardRegiDt.Text, CultureInfo.GetCultureInfo("en-gb"));
                            custMCard.dtMemExpireDate = DateTime.Parse(txt_MembershipcardExpireDt.Text, CultureInfo.GetCultureInfo("en-gb"));
                            custMCard.vcUpdatedBy = empName;
                            custMCard.dtUpdatedTime = DateTime.Now;
                            custMCard.isActive = true;
                            ENT.SaveChanges();
                        }
                    }

                    ENT.SaveChanges();

                    dbCon.Commit();

                    ENT.Connection.Close();

                    displayLabelMessage(false, "User Update Successfully!!");

                    clearMainCtrl();
                }
                catch (Exception ex)
                {
                    dbCon.Rollback();
                    displayLabelMessage(true, "ECUST105: " + ex.Message);
                }

            }

            return custInfo;

        }

        protected void btn_UploadPhotos_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(Server.MapPath("~/UploadedFiles/Temp Loc/Images/"));

                System.IO.FileInfo[] fileInfoArray = directoryInfo.GetFiles();

                foreach (System.IO.FileInfo fileInfo in fileInfoArray)
                {
                    if (fileInfo.CreationTime < DateTime.Now.AddSeconds(-5))
                        fileInfo.Delete();
                }
            }

            catch (Exception err)
            {
            }

            if (imgCustSnap.HasFile)
            {
                try
                {
                    int fileContentLengthInt = imgCustSnap.PostedFile.ContentLength;

                    byte[] fileByte = new byte[fileContentLengthInt];

                    HttpPostedFile httpPostedFile = imgCustSnap.PostedFile;

                    Session["CustImageName"] = httpPostedFile.FileName;

                    httpPostedFile.InputStream.Read(fileByte, 0, fileContentLengthInt);

                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(fileByte, 0, fileByte.Length);

                    System.Drawing.Image driverImage = System.Drawing.Image.FromStream(memoryStream);

                    driverImage.Save(Server.MapPath("~/UploadedFiles/Temp Loc/Images/" + httpPostedFile.FileName));

                    img_CustomerPhoto.ImageUrl = "~/UploadedFiles/Temp Loc/Images/" + httpPostedFile.FileName;

                    Session["CustImage"] = driverImage;
                }

                catch (Exception err)
                {

                    displayLabelMessage(true, "ECUST104: " + err.Message);

                    img_CustomerPhoto.ImageUrl = "";

                    Session["CustImage"] = null;
                    Session["CustImageName"] = null;
                }
            }

            else
            {

                img_CustomerPhoto.ImageUrl = "";
                Session["CustImage"] = null;
                Session["CustImageName"] = null;

            }

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            stringBuilder.Append("VisibleMeter();");
            stringBuilder.Append("enableOrDisableBlueToothAccount();");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Jesus", stringBuilder.ToString(), true);
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {

            clearMainCtrl();
            visibilityCtrl(true);

        }

        protected void rd_isPromYes_NO_CheckedChanged(object sender, EventArgs e)
        {
            Div_isMem.Visible = !(rd_isMembershipcardNo.Checked);
        }


    }
}