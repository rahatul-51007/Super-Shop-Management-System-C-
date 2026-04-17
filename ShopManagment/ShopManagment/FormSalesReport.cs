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
    public partial class FormSalesReport : Form
    {
        private DataAccess Da { set; get; }
        private FormAdminDashboard Ad { set; get; }
        public FormSalesReport()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.PopulateGridView();
            
        }
        public FormSalesReport(FormAdminDashboard ad):this()
        {
            this.Ad = ad;
        }


        private void PopulateGridView(string sql = "select * from Invoice;")
        {
            var ds = this.Da.ExecuteQuery(sql);
            this.dgvSaleReport.AutoGenerateColumns = false;
            this.dgvSaleReport.DataSource = ds.Tables[0];
            this.dgvSaleReport.ClearSelection();
            var sql2 = "SELECT SUM(Total) AS total_sell_amount FROM Invoice ; ";
            var ds2 = this.Da.ExecuteQuery(sql2);
            var count2 = ds2.Tables[0].Rows[0][0].ToString();
            this.txtTotal.Text = count2;
            


        }
        private void ClearAll()
        {
            this.dtpStartDate.Text = "";
            this.dtpEndDate.Text = "";
            this.txtSearch.Clear();
            this.dgvSaleReport.ClearSelection();

        }

        private void btnSearchDate_Click(object sender, EventArgs e)
        {
            var sql1 = "SELECT *FROM Invoice WHERE Date BETWEEN '"+this.dtpStartDate.Text+"' AND '"+this.dtpEndDate.Text+"'; ";
            this.PopulateGridView(sql1);
            this.dgvSaleReport.ClearSelection();

            var sql2 = "SELECT SUM(Total) AS total_sell_amount FROM Invoice WHERE Date BETWEEN '" + this.dtpStartDate.Text + "' AND '" + this.dtpEndDate.Text + "'; ";
            var ds2 = this.Da.ExecuteQuery(sql2);
            var count2 = ds2.Tables[0].Rows[0][0].ToString();
            this.dgvSaleReport.ClearSelection();
            this.txtTotal.Text = count2;
            
        }

        private void btnSearchId_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSearch.Text))
            {
                MessageBox.Show("Please give any ID.");
                return;
            }
            var sql1 = "SELECT *FROM Invoice WHERE EMP_ID= '" + this.txtSearch.Text + "'; ";
            this.PopulateGridView(sql1);
            this.dgvSaleReport.ClearSelection();

            var sql2 = "SELECT SUM(Total) AS total_sell_amount FROM Invoice WHERE EMP_ID = '"+this.txtSearch.Text+"'; ";
            var ds2 = this.Da.ExecuteQuery(sql2);
            var count2 = ds2.Tables[0].Rows[0][0].ToString();
            this.txtTotal.Text = count2;
           // this.dgvSaleReport.ClearSelection();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll();
            this.PopulateGridView();
           
            

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Ad.Show();
        }

        private void SalesReport_Load(object sender, EventArgs e)
        {
            this.dgvSaleReport.ClearSelection();
            
        }

        private void FormSalesReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
