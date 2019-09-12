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
    public partial class frm_PostponeTime : Form
    {
        public DateTime seletedDateTime;
        public bool isValidTime = false;
        public string executiveMessage = string.Empty;

        public frm_PostponeTime(DateTime entryTime)   
        {

            InitializeComponent();
            seletedDateTime = new DateTime();
            dateTimePicker1.Value = entryTime;
            dateTimePicker1.MinDate = System.DateTime.Now;

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();  
 
        }
        
        private void btn_SetTime_Click(object sender, EventArgs e)
        {
            try
            {
                seletedDateTime = dateTimePicker1.Value;
                executiveMessage = txtMsg.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                isValidTime = true;                
            }
            catch (Exception ex)
            {
                isValidTime = false;
            }
        }

    }
}
