using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ShopManagment
{
    public partial class FormPaymentProcess : Form
    {
        private DataAccess Da { set; get; }
        private FormProductCart F1 { set; get; }
        private string date = DateTime.Now.ToString("yyyy-MM-dd");
        private string id;
        private int invoice1;
        private double total1;
        public FormPaymentProcess()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.lblDate.Text+= date;

        }
        public FormPaymentProcess(string id,string invoice,string total,FormProductCart f1):this()
        {
            this.txtInvoice.Text = invoice;
            this.invoice1 = Convert.ToInt32(invoice);
            this.lblEmployeeId.Text += id;
            this.id = id;
            this.txtProductBill.Text = total;
            this.total1 = Convert.ToDouble(total);
            this.F1 = f1;

            var sql1 = " select NAME from Emptable where EMP_ID = '" + id + "';";
            var ds1 = this.Da.ExecuteQuery(sql1);
            var count1 = ds1.Tables[0].Rows[0][0].ToString();

            /*var name = " select NAME from Emptable where EMP_ID = '"+id+"';";
            this.Da.ExecuteQuery(name);*/
            this.lblEmployeeName.Text += count1;
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.rbCash.Checked)
            {
                this.txtPaymentMethod.Text = this.rbCash.Text;
            }
            else if (this.rbBkash.Checked)
            {
                this.txtPaymentMethod.Text = this.rbBkash.Text;
            }
            else if (this.rbNagad.Checked)
            {
                this.txtPaymentMethod.Text = this.rbNagad.Text;
            }
            else if (this.rbCard.Checked)
            {
                this.txtPaymentMethod.Text = this.rbCard.Text;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtInvoice.Text) || string.IsNullOrEmpty(this.txtProductBill.Text) || string.IsNullOrEmpty(this.txtPaymentMethod.Text))
                {
                    MessageBox.Show("Please select payment method.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var sql = "insert into invoice values("+ invoice1 +",'"+id+" ', '" + date + "', " + total1 + ", '" + this.txtPaymentMethod.Text + "'); ";
                int count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show("Payment Successful");
                else
                    MessageBox.Show("Please pay first.");
                this.Hide();
                this.F1.Show();
                this.F1.ClearAllFinal();
            }
            catch(Exception exc)
            {
                MessageBox.Show("Error" + exc);
            }
        }

        private void FormPaymentProcess_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }    
    }
}
