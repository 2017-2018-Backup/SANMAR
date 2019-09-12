using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SanmarWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISanmarWS" in both code and config file together.
    [ServiceContract]
    public interface ISanmarWS
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                              RequestFormat = WebMessageFormat.Json,
                              BodyStyle = WebMessageBodyStyle.Bare,
                              UriTemplate = "/loadExistingAppoint")]
        List<getAppoint_Result> loadExistingAppoint();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                              RequestFormat = WebMessageFormat.Json,
                              BodyStyle = WebMessageBodyStyle.Bare,
                              UriTemplate = "/loadDeputyChairmanresponse")]
        List<string> loadDeputyChairmanresponse();

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "Accepted/{msgID}")]
        bool Accepted(string msgID);

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "Rejected/{msgID}")]
        bool Rejected(string msgID);

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "CarryForward/{msgID}")]
        bool CarryForward(string msgID);

        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "Postpone/{msgID}/{executiveMessage}/{postponeDateTime}")]
        //[WebInvoke(Method = "POST",
        //        ResponseFormat = WebMessageFormat.Json,
        //        RequestFormat = WebMessageFormat.Json,
        //        BodyStyle = WebMessageBodyStyle.Wrapped,
        //        UriTemplate = "/Postpone")]
        //bool Postpone(postpone postponeInfo);
        bool Postpone(string msgID, string executiveMessage, string postponeDateTime);

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "DeputyChairmanResponse/{msgID}/{resMsg}")]
        bool DeputyChairmanResponse(string msgID, string resMsg);

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "/ChairmanDeviceToken/{deviceToken}")]
        bool ChairmanDeviceToken(string deviceToken);

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "/DeputyChairmanDeviceToken/{deviceToken}")]
        bool DeputyChairmanDeviceToken(string deviceToken);

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "/Chairman/{Action}")]
        bool Chairman(string Action);

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "/DChairman/{Action}")]
        bool DChairman(string Action);
    }

    [DataContract(Name = "PostPone")]
    public class postpone
    {
        [DataMember(Name = "msgId", Order = 0)]
        public string msgID { get; set; }
        [DataMember(Name = "executiveMessage", Order = 1)]
        public string executiveMessage { get; set; }
        [DataMember(Name = "postponeDateTime", Order = 2)]
        public string postponeDateTime { get; set; }
    }

    [DataContract(Name = "AppointmentInfo")]
    public class getAppoint_Result
    {
        [DataMember(Name = "msgId", Order = 0)]
        public long msgID { get; set; }

        [DataMember(Name = "TodayID", Order = 1)]
        public int? TodayID { get; set; }

        [DataMember(Name = "VisitorName", Order = 2)]
        public string VisitorName { get; set; }

        [DataMember(Name = "FromUserID", Order = 3)]
        public int FromUserID { get; set; }

        [DataMember(Name = "FromUserName", Order = 4)]
        public string FromUserName { get; set; }

        [DataMember(Name = "FromUserRole", Order = 5)]
        public string FromUserRole { get; set; }

        [DataMember(Name = "ToUserID", Order = 6)]
        public int ToUserID { get; set; }

        [DataMember(Name = "ToUserName", Order = 7)]
        public string ToUserName { get; set; }

        [DataMember(Name = "ToUserRole", Order = 8)]
        public string ToUserRole { get; set; }

        [DataMember(Name = "msgTypeID", Order = 9)]
        public byte msgTypeID { get; set; }

        [DataMember(Name = "MessageContent", Order = 10)]
        public string MessageContent { get; set; }

        [DataMember(Name = "isUrgent", Order = 11)]
        public bool isUrgent { get; set; }

        [DataMember(Name = "VisitorMessage", Order = 12)]
        public string VisitorMessage { get; set; }

        [DataMember(Name = "msgStatusDesc", Order = 13)]
        public string msgStatusDesc { get; set; }

        [DataMember(Name = "_CreatedDateTimeTemp", Order = 22)]
        public DateTime? _CreatedDateTimeTemp { get; set; }

        private string _tempCreatedDateTime { get; set; }
        [DataMember(Name = "CreatedDateTime", Order = 14)]
        public string CreatedDateTime
        {
            get
            {
                if (_CreatedDateTimeTemp.HasValue)
                {
                    //return Convert.ToDateTime(_CreatedDateTimeTemp.Value).ToString("dd-MM-yyyy HH:mm:ss");
                    return Convert.ToDateTime(_CreatedDateTimeTemp.Value).ToString("dd-MM-yyyy");
                }
                else
                    return "";
            }
            set { _temp = value; }
        }
        [DataMember(Name = "CreatedTimeF", Order = 23)]
        public string CreatedDateTimeF
        {
            get
            {
                if (_CreatedDateTimeTemp.HasValue)
                {
                    return Convert.ToDateTime(_CreatedDateTimeTemp.Value).ToString("dd-MM-yyyy hh:mm tt");
                }
                else
                    return "";
            }
            set { _temp = value; }
        }

        [DataMember(Name = "isActive", Order = 15)]
        public bool isActive { get; set; }

        [DataMember(Name = "msgStatusID", Order = 16)]
        public byte msgStatusID { get; set; }

        [DataMember(Name = "ResponseString", Order = 17)]
        public string ResponseString { get; set; }

        [DataMember(Name = "_responseTimeTemp", Order = 20)]
        public DateTime? _responseTimeTemp { get; set; }

        private string _temp { get; set; }
        [DataMember(Name = "ResponseTime", Order = 18)]
        public string ResponseTime
        {
            get
            {
                if (_responseTimeTemp.HasValue)
                {
                   // return Convert.ToDateTime(_responseTimeTemp.Value).ToString("dd-MM-yyyy HH:mm:ss");
                    return Convert.ToDateTime(_responseTimeTemp.Value).ToString("dd-MM-yyyy");
                }
                else
                    return "";
            }
            set { _temp = value; }
        }
        [DataMember(Name = "ResponseTimeF", Order = 24)]
        public string ResponseTimeF
        {
            get
            {
                if (_responseTimeTemp.HasValue)
                {
                    // return Convert.ToDateTime(_responseTimeTemp.Value).ToString("dd-MM-yyyy HH:mm:ss");
                    return Convert.ToDateTime(_responseTimeTemp.Value).ToString("dd-MM-yyyy hh:mm tt");
                }
                else
                    return "";
            }
            set { _temp = value; }
        }

        [DataMember(Name = "_PostponeTimeTemp", Order = 21)]
        public DateTime _PostponeTimeTemp { get; set; }

        private string _tempPTime { get; set; }
        [DataMember(Name = "PostponeTime", Order = 19)]
        public string PostponeTime
        {
            get
            {
                if (_PostponeTimeTemp != null)
                {
                   // return Convert.ToDateTime(_PostponeTimeTemp).ToString("dd-MM-yyyy HH:mm:ss");
                    return Convert.ToDateTime(_PostponeTimeTemp).ToString("dd-MM-yyyy");
                }
                else
                    return "";
            }
            set { _tempPTime = value; }
        }
    }
}
