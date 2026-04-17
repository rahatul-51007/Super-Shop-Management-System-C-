using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagment
{
    public partial class FormAdminDashboard : Form
    {
        private DataAccess Da{ get;set; }
        private FormLogin F1 { set; get; }

        public FormAdminDashboard()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.CurrentDate.Text+=DateTime.Now.ToString("dd-MM-yyyy");
        }
        public FormAdminDashboard(string info,string id, FormLogin f1) : this()
        {
           
            this.lblAdminName.Text += info;
            this.lblAdminID.Text += id;
            this.lblWelcome.Text +=" "+info;
            this.F1 = f1;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            var sql1 = "select count(role) from emptable where role = 'admin';";
            var ds1 = this.Da.ExecuteQuery(sql1);
            var count1 = ds1.Tables[0].Rows[0][0].ToString();
            this.lblTotalAdmin.Text = count1;
            var sql2 = "select count(role) from emptable where role = 'employee';";
            var ds2 = this.Da.ExecuteQuery(sql2);
            var count2 = ds2.Tables[0].Rows[0][0].ToString();
            this.lblTotalSeller.Text= count2;
            var sql3 = "SELECT SUM(Total) AS total_sell_amount FROM Invoice ;";
            var ds3 = this.Da.ExecuteQuery(sql3);
            var count3 = ds3.Tables[0].Rows[0][0].ToString();
            this.lblTotalSellAmount.Text = count3;
        }
        public void RefreshCounts()
        {
            var sql1 = "select count(role) from emptable where role = 'admin';";
            var ds1 = this.Da.ExecuteQuery(sql1);
            var count1 = ds1.Tables[0].Rows[0][0].ToString();
            this.lblTotalAdmin.Text = count1;
            var sql2 = "select count(role) from emptable where role = 'employee';";
            var ds2 = this.Da.ExecuteQuery(sql2);
            var count2 = ds2.Tables[0].Rows[0][0].ToString();
            this.lblTotalSeller.Text = count2;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("You have successfully logged out.");
            this.Hide();
            this.F1.Show();
            F1.ClearAll();
        }

        private void btnAddSeller_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormEmployeeInformation(this).Show();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormProductManagment(this).Show();
        }

        private void btnSearchRecord_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormSalesReport(this).Show();
        }

        private void FormAdminDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        
    }
}
