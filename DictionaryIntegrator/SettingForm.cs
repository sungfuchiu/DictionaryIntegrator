using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictionaryIntegrator
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
            tbxAPIKey.Text = Properties.Settings.Default.ApiKey;
            tbxDatabaseID.Text = Properties.Settings.Default.DatabaseId;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //return api key and database id to main form
            Properties.Settings.Default.ApiKey = tbxAPIKey.Text.Trim();
            Properties.Settings.Default.DatabaseId = tbxDatabaseID.Text.Trim();
            this.Close();
        }
    }
}
