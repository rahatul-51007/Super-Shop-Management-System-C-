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
    public partial class FormProductManagment : Form
    {
        private DataAccess Da { set; get; }
        private FormAdminDashboard Ad { set; get; }
        public FormProductManagment()
        {
            this.Da = new DataAccess();
            InitializeComponent();
            this.PopulateGridView();
        }
        public FormProductManagment(FormAdminDashboard ad) : this()
        {
            this.Ad = ad;
        }
        private void PopulateGridView(string sql = "select * from product;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvProduct.AutoGenerateColumns = false;
            this.dgvProduct.DataSource = ds.Tables[0];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the empty fields");
                    return;
                }
                if (!this.IsValidName(this.txtProductname.Text))
                {
                    MessageBox.Show("Product name should only contain alphabetic characters (a-z, A-Z).");
                    return;
                }
                if (!this.IsValidName(this.cmbProductCategory.Text))
                {
                    MessageBox.Show("Product category name should only contain alphabetic characters (a-z, A-Z) and (space and hypen).");
                    return;
                }
                if (!this.IsValidQuantity(this.txtQuantity.Text))
                {
                    MessageBox.Show("Quantity should only contain 0-9");
                    return;
                }
                if (!this.IsValidPrice(this.txtPrice.Text))
                {
                    MessageBox.Show("Price must be non-negative value");
                    return;
                }
                var sql = "insert into Product values(" + this.txtProductId.Text + ", '" + this.txtProductname.Text + "', '" + this.cmbProductCategory.Text + "', " + this.txtPrice.Text + ", " + this.txtQuantity.Text + "); ";
                int count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show("Data has been added properly");
                else
                    MessageBox.Show("Data hasn't been added properly");

            }
            catch(Exception exc)
            {
                MessageBox.Show("An error has occured, please check: " + exc.Message);
            }

            this.PopulateGridView();
            this.AutoIdGenerate();
            this.ClearAll();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the empty fields");
                    return;
                }
                if (!this.IsValidName(this.txtProductname.Text))
                {
                    MessageBox.Show("Product name should only contain alphabetic characters (a-z, A-Z).");
                    return;
                }
                if (!this.IsValidName(this.cmbProductCategory.Text))
                {
                    MessageBox.Show("Product category name should only contain alphabetic characters (a-z, A-Z).");
                    return;
                }
                if (!this.IsValidQuantity(this.txtQuantity.Text))
                {
                    MessageBox.Show("Quantity should only contain 0-9");
                    return;
                }
                if (!this.IsValidPrice(this.txtPrice.Text))
                {
                    MessageBox.Show("Price must be non-negative value");
                    return;
                }
                var query = "select * from Product where Product_ID = " + this.txtProductId.Text + "";
                DataTable dt = this.Da.ExecuteQueryTable(query);
                if (dt.Rows.Count == 1)
                {
                    var sql = @"update Product
                                set Product_Name = '" + this.txtProductname.Text + @"',
                                Category = '" + this.cmbProductCategory.Text + @"',
                                Price = " + this.txtPrice.Text + @",
                                Quantity = " + this.txtQuantity.Text + @"
                                where Product_ID = " + this.txtProductId.Text + "; ";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count == 1)
                        MessageBox.Show("Data has been updated properly");
                    else
                        MessageBox.Show("Data hasn't been updated properly");

                }

            }
            catch(Exception exc)
            {
                MessageBox.Show("An error has occured, please check: " + exc.Message);
            }
            this.PopulateGridView();
            this.AutoIdGenerate();
            this.ClearAll();
        }
        private bool IsValidToSave()
        {
            if (string.IsNullOrEmpty(this.txtProductId.Text) || string.IsNullOrEmpty(this.txtProductname.Text) ||
                string.IsNullOrEmpty(this.cmbProductCategory.Text) || string.IsNullOrEmpty(this.txtQuantity.Text) ||
                string.IsNullOrEmpty(this.txtPrice.Text) )
                return false;
            else
                return true;
        }
        private bool IsValidName(string name)
        {
            foreach (char c in name)
            {
                if (!((c >= 65 && c <= 90) || (c >= 97 && c <= 122)||c==32||c==45))
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsValidQuantity(string quantity)
        {
            foreach (char c in quantity)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsValidPrice(string price)
        {
            try
            {
                double tempPrice;
                if (double.TryParse(price, out tempPrice))
                {
                    if (tempPrice >= 0)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            catch(Exception exc)
            {
                MessageBox.Show("Error!" + exc.Message);
                return false;
            }
        }

        private void dgvProduct_DoubleClick(object sender, EventArgs e)
        {
           
            this.txtProductId.Text = this.dgvProduct.CurrentRow.Cells[0].Value.ToString();
            this.txtProductname.Text = this.dgvProduct.CurrentRow.Cells[1].Value.ToString();
            this.cmbProductCategory.Text = this.dgvProduct.CurrentRow.Cells[2].Value.ToString();
            this.txtPrice.Text = this.dgvProduct.CurrentRow.Cells[3].Value.ToString();
            this.txtQuantity.Text = this.dgvProduct.CurrentRow.Cells[4].Value.ToString();
        }

        private void ProductManagment_Load(object sender, EventArgs e)
        {
            this.dgvProduct.ClearSelection();
            this.AutoIdGenerate();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll();
            this.PopulateGridView();
            this.dgvProduct.ClearSelection();
            this.AutoIdGenerate();

        }
        private void ClearAll()
        {
           
            this.txtProductname.Clear();
            this.cmbProductCategory.SelectedIndex=-1;
            this.txtQuantity.Clear();
            this.txtPrice.Clear();
            this.txtSearch.Clear();

            this.dgvProduct.ClearSelection();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvProduct.SelectedRows.Count < 1)
                {
                    MessageBox.Show("Please select a row first to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var id = this.dgvProduct.CurrentRow.Cells[0].Value.ToString();
                

                DialogResult result = MessageBox.Show("Are you sure you want to delete "  + "?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                var sql = "delete from Product where Product_ID = " + id + ";";
                var count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show( "Item has been removed from the list.");
                else
                    MessageBox.Show("Item hasn't been removed properly");

                this.PopulateGridView();
                this.ClearAll();
                this.AutoIdGenerate();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured, please check: " + exc.Message);
            }
        }
        private void AutoIdGenerate()
        {
            var query = "select max(Product_Id) from Product ;";
            var dt = this.Da.ExecuteQueryTable(query);
            var oldId = dt.Rows[0][0];
            var num = Convert.ToInt32(oldId);
            var newId =++num;          
            this.txtProductId.Text = newId.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var sql1 = "select * from product where Product_Name like '"+this.txtSearch.Text+"%';";
            this.PopulateGridView(sql1);
            this.dgvProduct.ClearSelection();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Ad.Show();
        }

        private void FormProductManagment_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
