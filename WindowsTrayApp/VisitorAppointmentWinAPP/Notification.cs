using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisitorAppointmentWinAPP
{
    public partial class Notification : Form
    {
        public frm_Appointment frmAppointment;
        //Timer _timer = new Timer();
        public string noOfPersons = "";
        public Notification()
        {
            InitializeComponent();
            this.Load += new EventHandler(Notification_Load);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            frmAppointment.Visible = true;
            this.Close();
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            this.Top = 0;
            //this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            this.TopMost = true;

            //_timer.Start();
            //_timer.Interval = 6000;
            //_timer.Enabled = true;
            //_timer.Tick += new EventHandler(_timer_Tick);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            //_timer.Enabled = false;
            this.Close();
            //throw new NotImplementedException();
        }

        private void Notification_Activated(object sender, EventArgs e)
        {
            label1.Text = noOfPersons;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
