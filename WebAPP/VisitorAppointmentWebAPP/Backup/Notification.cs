using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using JdSoft.Apple.Apns.Notifications;

namespace VisitorAppointmentWebAPP
{
    public class VMSNotification
    {
        bool result = false;
        public bool PushNotificationToApple(int Badge, string strPushMessage = "VMS Message", string testDeviceToken = "90609CC397BB3626C2F64C80924A274A22CCB709EF34A6CF3E134559414557C7", bool IncludeCustomItem = false, string disabled = "")//Messages messageObject)
        {
            #region APNS
            // Int64 result = 0;
            //try
            //{
            //    // strDeviceToken is a DeviceToken of length 64
            //    string strDeviceToken = "90609CC397BB3626C2F64C80924A274A22CCB709EF34A6CF3E134559414557C7";

            //    var payload1 = new NotificationPayload(strDeviceToken, strPushMessage, 1, "default");
            //    //payload1.AddCustom("RegionID", "IDQ10150");

            //    var p = new List<NotificationPayload> { payload1 };
            //    string certificatePath = HttpContext.Current.Server.MapPath("/bin/ChairmanAppDev.p12");
            //    var push = new PushNotification(true, certificatePath, "123");
            //    //string strfilename = push.P12File;
            //    var message1 = push.SendToApple(p);
            //    foreach (var item in message1)
            //    {
            //        result = 1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
            //return result;
            #endregion
            try
            {

                bool sandbox = true;

                //Put your device token in here
                // "fe58fc8f527c363d1b775dca133e04bff24dc5032d08836992395cc56bfa62ef";

                //Put your PKCS12 .p12 or .pfx filename here.
                // Assumes it is in the same directory as your app
                string p12File = "ChairmanAppDev.p12";// "apn_developer_identity.p12";

                //This is the password that you protected your p12File 
                //  If you did not use a password, set it as null or an empty string
                string p12FilePassword = "123";// "yourpassword";

                //Number of notifications to send
                int count = 3;

                //Number of milliseconds to wait in between sending notifications in the loop
                // This is just to demonstrate that the APNS connection stays alive between messages
                int sleepBetweenNotifications = 15000;


                //Actual Code starts below:
                //--------------------------------

                string p12Filename = HttpContext.Current.Server.MapPath("/bin/ChairmanAppDev.p12");// System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);

                p12Filename = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "bin\\ChairmanAppDev.p12");
                //p12Filename = @"D:\sanmar publish\bin\ChairmanAppDev.p12";
                NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);

                service.SendRetries = 5; //5 retries before generating notificationfailed event
                service.ReconnectDelay = 5000; //5 seconds

                service.Error += new NotificationService.OnError(service_Error);
                service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

                service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
                service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
                service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
                service.Connecting += new NotificationService.OnConnecting(service_Connecting);
                service.Connected += new NotificationService.OnConnected(service_Connected);
                service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);
                //Create a new notification to send
                Notification alertNotification = new Notification(testDeviceToken);

                alertNotification.Payload.Alert.Body = string.Format(strPushMessage);
                alertNotification.Payload.Sound = "default";
                alertNotification.Payload.Badge = Badge;
                if (IncludeCustomItem == true)
                    alertNotification.Payload.Disabled = disabled;

                //{
                //    alertNotification.Payload.AddCustom("Disabled", disabled);
                //}

                //Queue the notification to be sent
                if (service.QueueNotification(alertNotification))
                    return true;//   Console.WriteLine("Notification Queued!");
                else
                    return false;// Console.WriteLine("Notification Failed to be Queued!");

                #region Reference Code
                ////Sleep in between each message
                //if (i < count)
                //{
                //    Console.WriteLine("Sleeping " + sleepBetweenNotifications + " milliseconds before next Notification...");
                //    System.Threading.Thread.Sleep(sleepBetweenNotifications);
                //}


                ////The notifications will be sent like this:
                ////		Testing: 1...
                ////		Testing: 2...
                ////		Testing: 3...
                //// etc...
                //for (int i = 1; i <= count; i++)
                //{
                //    //Create a new notification to send
                //    Notification alertNotification = new Notification(testDeviceToken);

                //    alertNotification.Payload.Alert.Body = string.Format("Testing {0}...", i);
                //    alertNotification.Payload.Sound = "default";
                //    alertNotification.Payload.Badge = i;

                //    //Queue the notification to be sent
                //    if (service.QueueNotification(alertNotification))
                //        Console.WriteLine("Notification Queued!");
                //    else
                //        Console.WriteLine("Notification Failed to be Queued!");

                //    //Sleep in between each message
                //    if (i < count)
                //    {
                //        // Console.WriteLine("Sleeping " + sleepBetweenNotifications + " milliseconds before next Notification...");
                //        System.Threading.Thread.Sleep(sleepBetweenNotifications);
                //    }
                //}

                //Console.WriteLine("Cleaning Up...");

                //First, close the service.  
                //This ensures any queued notifications get sent befor the connections are closed
                #endregion
                service.Close();

                //Clean up
                service.Dispose();

                // Console.WriteLine("Done!");
                // Console.WriteLine("Press enter to exit...");
                // Console.ReadLine();
            }
            catch (Exception exx)
            {
                return false;
            }
        }

        static void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            System.Diagnostics.Debug.Write("Bad Device Token: {0}", ex.Message);
        }

        static void service_Disconnected(object sender)
        {
            System.Diagnostics.Debug.Write("Disconnected...");
        }

        static void service_Connected(object sender)
        {
            System.Diagnostics.Debug.Write("Connected");
            // Console.WriteLine("Connected...");
        }

        static void service_Connecting(object sender)
        {
            System.Diagnostics.Debug.Write("Connecting...");
        }

        static void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            System.Diagnostics.Debug.Write(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));
        }

        static void service_NotificationSuccess(object sender, Notification notification)
        {
            System.Diagnostics.Debug.Write(string.Format("Notification Success: {0}", notification.ToString()));
        }

        static void service_NotificationFailed(object sender, Notification notification)
        {

            System.Diagnostics.Debug.Write(string.Format("Notification Failed: {0}", notification.ToString()));
        }

        static void service_Error(object sender, Exception ex)
        {
            System.Diagnostics.Debug.Write(string.Format("Error: {0}", ex.Message));
        }
    }
}