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
    public partial class FormEmployeeDashboard : Form
    {
        private DataAccess Da { set; get; }
        private FormLogin F1 { set; get; }
        private string id;
        public FormEmployeeDashboard()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.lblDate.Text += DateTime.Now.ToString("dd-MM-yyyy");
        }
        public FormEmployeeDashboard(string info,string id,FormLogin f1) : this()
        {
            this.F1 = f1;
            this.lblEmployeeName.Text += info;
            this.lblEmployeeId.Text += id;
            this.id = id;

            this.lblWelcome.Text += info;

            /*var sql1 = " select count(Invoice_Id) from Invoice where EMP_ID='"+id+"';";
            var ds1 = this.Da.ExecuteQuery(sql1);
            var count1 = ds1.Tables[0].Rows[0][0].ToString();
            this.lblTCS.Text += count1;*/
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Logged out from the system");
            this.Hide();
            this.F1.Show();
            F1.ClearAll();
        }

        private void btnAddProductToCart_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormProductCart(id,this).Show();
        }

        private void FormEmployeeDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FormEmployeeDashboard_Load(object sender, EventArgs e)
        {
            var sql1 = " select count(Invoice_Id) from Invoice where EMP_ID='"+id+"';";
            var ds1 = this.Da.ExecuteQuery(sql1);
            var count1 = ds1.Tables[0].Rows[0][0].ToString();
            this.lblTCS.Text += count1;

            var sql2 = " select sum(Total) from Invoice where EMP_ID='" + id + "';";
            var ds2 = this.Da.ExecuteQuery(sql2);
            var count2 = ds2.Tables[0].Rows[0][0].ToString();
            this.lblSum.Text += count2;
        }
    }
}
