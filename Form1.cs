using System;
using System.Windows.Forms;

namespace CRM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DataProvider.Initialize();
        }

        private void btnSyncContacts_Click(object sender, EventArgs e)
        {
            DataProvider.GetContacts();
        }

        private void btnSyncSaleContacts_Click(object sender, EventArgs e)
        {
            DataProvider.GetSaleContacts();
        }

        private void btnSyncAccount_Click(object sender, EventArgs e)
        {
            DataProvider.GetAccounts();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataProvider.GetAllURLEntities();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            DataProvider.IsAuthenticated(username, password);
        }
    }
}
