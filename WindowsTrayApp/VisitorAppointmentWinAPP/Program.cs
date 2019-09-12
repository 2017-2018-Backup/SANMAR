using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace VisitorAppointmentWinAPP
{
    //class Singleton
    //{
    //    private static Singleton instance;

    //    private Singleton() { }

    //    public static Singleton Instance()
    //    {
    //        if (instance == null)
    //            instance = new Singleton();

    //        return instance;
    //    }
    //}


    static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            string value = System.Configuration.ConfigurationSettings.AppSettings["WinAPPUserType"];
            using (Mutex mutex = new Mutex(true, value + "VisitorManagementApplication", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frm_Appointment());
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frm_Appointment());
        }
    }
}
