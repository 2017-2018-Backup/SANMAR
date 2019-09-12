using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using VisitorAppointmentDB;

namespace SanmarWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SanmarWS" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SanmarWS.svc or SanmarWS.svc.cs at the Solution Explorer and start debugging.
    public class SanmarWS : ISanmarWS
    {
        public bool Chairman(string Action)
        {
            try
            {
                if (Action.Equals("close", StringComparison.InvariantCultureIgnoreCase))
                {
                    VisitorAPPEntities ENT = new VisitorAPPEntities();
                    BadgeDetail imsg = (from data in ENT.BadgeDetails
                                        where data.UserID == 5
                                        select data).FirstOrDefault();
                    if (imsg != null)
                    {
                        imsg.LastCloseTime = DateTime.Now;
                        imsg.BadgeCount = 0;

                        ENT.SaveChanges();
                        ENT.AcceptAllChanges();
                        ENT.Connection.Close();
                        return true;
                    }
                    return false;
                }
                else if (Action.Equals("open", StringComparison.InvariantCultureIgnoreCase))
                {
                    VisitorAPPEntities ENT = new VisitorAPPEntities();
                    BadgeDetail imsg = (from data in ENT.BadgeDetails
                                        where data.UserID == 5
                                        select data).FirstOrDefault();
                    if (imsg != null)
                    {
                        imsg.LastOpenTime = DateTime.Now;
                        imsg.LastCloseTime = null;
                        imsg.BadgeCount = 0;

                        ENT.SaveChanges();
                        ENT.AcceptAllChanges();
                        ENT.Connection.Close();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch { return false; }
        }

        public bool DChairman(string Action)
        {
            try
            {
                if (Action.Equals("close", StringComparison.InvariantCultureIgnoreCase))
                {
                    VisitorAPPEntities ENT = new VisitorAPPEntities();
                    BadgeDetail imsg = (from data in ENT.BadgeDetails
                                        where data.UserID == 4
                                        select data).FirstOrDefault();
                    if (imsg != null)
                    {
                        imsg.LastCloseTime = DateTime.Now;
                        imsg.BadgeCount = 0;

                        ENT.SaveChanges();
                        ENT.AcceptAllChanges();
                        ENT.Connection.Close();

                        return true;
                    }
                    return false;
                }
                else if (Action.Equals("open", StringComparison.InvariantCultureIgnoreCase))
                {
                    VisitorAPPEntities ENT = new VisitorAPPEntities();
                    BadgeDetail imsg = (from data in ENT.BadgeDetails
                                        where data.UserID == 4
                                        select data).FirstOrDefault();
                    if (imsg != null)
                    {
                        imsg.LastOpenTime = DateTime.Now;
                        imsg.LastCloseTime = null;
                        imsg.BadgeCount = 0;

                        ENT.SaveChanges();
                        ENT.AcceptAllChanges();
                        ENT.Connection.Close();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch { return false; }
        }


        //User Type ID = 5 for chairman
        //User Type Id = 4 for deputy chairman
        public List<getAppoint_Result> loadExistingAppoint()
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
                            where getdata.isActive == true && getdata.msgStatusID < 4 && getdata.msgStatusID != 2
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
                                _CreatedDateTimeTemp = getdata.CreatedDateTime, //CreatedDateTime = getdata.CreatedDateTime,                                
                                isActive = getdata.isActive,
                                msgStatusID = getdata.msgStatusID,
                                ResponseString = getdata.ResponseString,
                                _responseTimeTemp = getdata.ResponseTime,
                                _PostponeTimeTemp = getdata.CreatedDateTime// PostponeTime = getdata.CreatedDateTime
                            }));

                ENT.Connection.Close();

                return data.ToList();
            }
            catch (Exception ex)
            {
                return new List<getAppoint_Result>();
            }
        }

        public List<string> loadDeputyChairmanresponse()
        {
            List<string> result = new List<string>();
            VisitorAPPEntities ENT = new VisitorAPPEntities();
            var lst = (from data in ENT.TypeMsgStatus
                       where data.msgStatusID > 10
                       select data);

            foreach (TypeMsgStatu item in lst)
            {
                result.Add(item.msgStatusDesc);
            }

            return result;
        }

        public bool Accepted(string msgID)
        {
            return updateResponse(Convert.ToInt64(msgID), "Accept", "", DateTime.Now);
        }

        public bool Rejected(string msgID)
        {
            return updateResponse(Convert.ToInt64(msgID), "Reject", "", DateTime.Now);
        }

        public bool CarryForward(string msgID)
        {
            return updateResponse(Convert.ToInt64(msgID), "Carry Forward", "", DateTime.Now);
        }

        public bool Postpone(postpone postponeInfo)
        {
            return Postpone(postponeInfo.msgID, postponeInfo.executiveMessage, postponeInfo.postponeDateTime);
        }

        public bool Postpone(string msgID, string executiveMessage, string postponeDateTime)
        {
            try
            {
                //DateTime PostPoneDateTime = DateTime.ParseExact("19-12-2017 02:05:02", "dd-MM-yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                //DateTime PostPoneDateTime = DateTime.ParseExact(postponeDateTime, "dd-MM-yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                DateTime PostPoneDateTime = DateTime.ParseExact(postponeDateTime, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                return updateResponse(Convert.ToInt64(msgID), "Postpone", executiveMessage, PostPoneDateTime);
            }
            catch { return false; }
        }

        public bool DeputyChairmanResponse(string msgID, string resMsg)
        {
            return updateResponse(Convert.ToInt64(msgID), resMsg, "", DateTime.Now);
        }

        public bool updateResponse(long msgID, string resMsg, string executiveMessage, DateTime PostponeTime)
        {
            try
            {

                VisitorAPPEntities ENT = new VisitorAPPEntities();
                InfoMessage imsg = (from data in ENT.InfoMessages
                                    where data.msgID == msgID
                                    select data).FirstOrDefault();


                imsg.ResponseTime = DateTime.Now;

                if (resMsg.Equals("Accept", StringComparison.InvariantCultureIgnoreCase) || resMsg.Equals("Reject", StringComparison.InvariantCultureIgnoreCase))
                {
                    imsg.ResponseString = resMsg == "Accept" ? "Accepted" : "Rejected";
                    imsg.msgStatusID = 2;

                }

                else if (resMsg.Equals("Postpone", StringComparison.InvariantCultureIgnoreCase))
                {
                    imsg.msgStatusID = 2;
                    imsg.ResponseString = "Postpone";
                    if (!string.IsNullOrEmpty(executiveMessage))
                        imsg.VisitorMessage = "Postpone Comments : " + executiveMessage + Environment.NewLine + "************" + Environment.NewLine;// +imsg.VisitorMessage;
                    else
                        imsg.VisitorMessage = "";
                    imsg.CreatedDateTime = PostponeTime;
                }

                else if (resMsg == "Carry Forward")
                {
                    imsg.msgStatusID = 3;
                }
                else//For Deputy Chairman
                {
                    imsg.ResponseString = resMsg;
                    imsg.msgStatusID = 2;
                }
                ENT.SaveChanges();

                ENT.AcceptAllChanges();

                ENT.Connection.Close();

                //this.Visible = false;

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        int getTodayNextSlNo()
        {
            int res = 1;
            try
            {

                var data = (from getdata in new VisitorAPPEntities().InfoMessages
                            where EntityFunctions.TruncateTime(getdata.CreatedDateTime) == EntityFunctions.TruncateTime(DateTime.Today)
                            select getdata).ToList();

                if (data.Count >= 1)
                    res = data.Count + 1;

            }
            catch (Exception ex)
            {
                //displayLabelMessage(true, "NAPT101: " + ex.Message);
            }
            return res;
        }

        public bool ChairmanDeviceToken(string deviceToken)
        {
            try
            {

                VisitorAPPEntities ENT = new VisitorAPPEntities();
                InfoUser imsg = (from data in ENT.InfoUsers
                                 where data.UID == 5
                                 select data).FirstOrDefault();
                imsg.MobileIMENo = deviceToken;
                //{
                //"deviceToken" : "90609CC397BB3626C2F64C80924A274A22CCB709EF34A6CF3E134559414557C7"
                //}
                ENT.SaveChanges();
                ENT.AcceptAllChanges();
                ENT.Connection.Close();
                return true;
            }
            catch
            {
            }
            return false;
        }


        public bool DeputyChairmanDeviceToken(string deviceToken)
        {
            try
            {

                VisitorAPPEntities ENT = new VisitorAPPEntities();
                InfoUser imsg = (from data in ENT.InfoUsers
                                 where data.UID == 4
                                 select data).FirstOrDefault();
                imsg.MobileIMENo = deviceToken;
                ENT.SaveChanges();
                ENT.AcceptAllChanges();
                ENT.Connection.Close();
                return true;
            }
            catch
            {
            }
            return false;
        }

    }




}
