using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VisitorAppointmentDB;

namespace VisitorAppointmentWinAPP
{
    public partial class frmResponse : Form
    {
        public frmResponse()
        {
            InitializeComponent();
            btnSubmit.Enabled = false;
        }

        public string selectedValue = "";
        private void frmResponse_Load(object sender, EventArgs e)
        {
            VisitorAPPEntities ENT = new VisitorAPPEntities();
            var lst = (from data in ENT.TypeMsgStatus
                       where data.msgStatusID > 10
                       select data);

            foreach (TypeMsgStatu item in lst)
            {
                lstReponse.Items.Add(item.msgStatusDesc);
            }
        }

        private void lstReponse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstReponse.SelectedItems.Count > 0)
            {
                btnSubmit.Enabled = true;
            }
            else
            {
                btnSubmit.Enabled = false;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            selectedValue = lstReponse.SelectedItem.ToString();
            this.Close();
        }
    }
}
