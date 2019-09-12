using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using VisitorAppointmentDB;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace SanmarWebservice
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string SanmarTestSerice()
        {
            return "Success";
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void loadExistingAppoint()
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
                            where getdata.isActive == true && getdata.msgStatusID < 4
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

                ENT.Connection.Close();
                
                var obj =  new JavaScriptSerializer().Serialize(data.ToList());
                Context.Response.Write(obj);
            }
            catch (Exception ex)
            {
                Context.Response.Write(string.Format("[ERROR: {0}]", ex.Message));
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
        public DateTime? PostponeTime { get; set; }
    }
}