using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagment
{
   
    public partial class FormProductCart : Form
    {
        private DataAccess Da { set; get; }
        private FormEmployeeDashboard F1 { set; get; }
        private string id;
        public FormProductCart()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.AutoIdGenerate();
            this.PopulateGridView();
            

        }
        public FormProductCart(string id, FormEmployeeDashboard f1) : this()
        {
            
            this.F1 = f1;
            this.id = id;
        }
        private void PopulateGridView(string sql = "select * from product;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvProduct.AutoGenerateColumns = false;
            this.dgvProduct.DataSource = ds.Tables[0];

        }

        private void txtAutoSearch_TextChanged(object sender, EventArgs e)
        {
            var sql = "SELECT * FROM PRODUCT where product_name like '"+this.txtAutoSearch.Text+"%';";
            this.PopulateGridView(sql);

        }
        private void dgvProduct_DoubleClick(object sender, EventArgs e)
        { 
            this.txtProductId.Text = this.dgvProduct.CurrentRow.Cells[0].Value.ToString();
            this.txtProductName.Text= this.dgvProduct.CurrentRow.Cells[1].Value.ToString();
            this.txtCategory.Text = this.dgvProduct.CurrentRow.Cells[2].Value.ToString();
            this.txtProductPrice.Text = this.dgvProduct.CurrentRow.Cells[3].Value.ToString();
            this.txtQuantity.Clear();
            this.dgvProduct.ClearSelection();
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ExistingProduct())
                {
                    for (int row = 0; row < this.dgvShowProducts.Rows.Count; row++)
                    {
                        var id = this.txtProductId.Text.ToString();
                        var id1 = this.dgvShowProducts.Rows[row].Cells[0].Value.ToString();
                        if (id == id1)
                        {
                            int existingQty = Convert.ToInt32(dgvShowProducts.Rows[row].Cells[3].Value);
                            int quantity1 = Convert.ToInt32(this.txtQuantity.Text);
                            int newQty = existingQty + quantity1;

                            int availableQuantity = GetAvailableQuantity(id);

                            if (availableQuantity < newQty)
                            {
                                MessageBox.Show("Not enough stock to increase quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            double price = Convert.ToDouble(this.txtProductPrice.Text);
                            dgvShowProducts.Rows[row].Cells[3].Value = newQty;
                            dgvShowProducts.Rows[row].Cells[5].Value = newQty * price;
                            upadteQuantity(id, quantity1, true);
                            this.dgvShowProducts.ClearSelection();
                            this.dgvProduct.ClearSelection();

                            double sum = 0;
                            for (int row1 = 0; row1 < dgvShowProducts.Rows.Count; row1++)
                            {
                                sum = sum + Convert.ToDouble(dgvShowProducts.Rows[row1].Cells[5].Value);
                            }
                            this.txtTotal.Text = Convert.ToString(sum);
                            this.txtProductId.Clear();
                            this.txtProductName.Clear();
                            this.txtProductPrice.Clear();
                            this.txtCategory.Clear();
                            this.txtQuantity.Clear();

                            break;
                        }

                    }

                }

                else
                {
                    string id = this.txtProductId.Text;
                    string name = this.txtProductName.Text;
                    string category = this.txtCategory.Text;
                    string quantity = this.txtQuantity.Text;
                    string price = this.txtProductPrice.Text;
                    if (!IsValid())
                    {
                        MessageBox.Show("Please fill in all the fields to proceed.");
                        return;
                    }
                    if (!this.IsValidContact(this.txtQuantity.Text))
                    {
                        MessageBox.Show("Please enter a valid quantity(between 0 and 9).");
                        return;
                    }
                    int quantity1 = Convert.ToInt32(quantity);
                    double price1 = Convert.ToDouble(price);
                    double unitTotal = quantity1 * price1;
                    int availableQuantity = GetAvailableQuantity(id);
                    if (availableQuantity < quantity1)
                    {
                        MessageBox.Show("Insufficient quantity in stock!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.dgvShowProducts.Rows.Add(id, name, category, quantity1, price1, unitTotal);
                    upadteQuantity(id, quantity1, true);
                    this.dgvShowProducts.ClearSelection();
                    this.dgvProduct.ClearSelection();
                    double sum = 0;
                    for (int row = 0; row < dgvShowProducts.Rows.Count; row++)
                    {
                        sum = sum + Convert.ToDouble(dgvShowProducts.Rows[row].Cells[5].Value);
                    }
                    this.txtTotal.Text = Convert.ToString(sum);
                    this.txtProductId.Clear();
                    this.txtProductName.Clear();
                    this.txtProductPrice.Clear();
                    this.txtCategory.Clear();
                    this.txtQuantity.Clear();
                }

            }
            catch(Exception exc)
            {
                MessageBox.Show("Error!"+exc);
            }

        }
        private int GetAvailableQuantity(string productId)
        {
           
            var sql = "SELECT Quantity FROM PRODUCT WHERE product_id = " + productId + ";";
            DataSet ds = this.Da.ExecuteQuery(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count= Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                return count;
            }
            return 0;
        }
        private bool IsValid()
        {
            if (string.IsNullOrEmpty(this.txtProductId.Text) || string.IsNullOrEmpty(this.txtProductName.Text) ||
                string.IsNullOrEmpty(this.txtCategory.Text) || string.IsNullOrEmpty(this.txtQuantity.Text) ||
                string.IsNullOrEmpty(this.txtProductPrice.Text)||string.IsNullOrEmpty(this.txtInvoice.Text))
                return false;
            else
                return true;
        }
        private bool IsValidContact(string quantity)
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
        private void upadteQuantity(string id,int quantity1,bool isAddInCart)
        {
            try
            {
                var sql = "SELECT Quantity FROM PRODUCT where product_id=" + id + ";";
                DataSet ds = this.Da.ExecuteQuery(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int update;
                    if (isAddInCart)
                    {
                        update = count - quantity1;
                        if (update >= 0)
                        {
                            var sql1 = "update product set quantity=" + update + "where product_id=" + id + ";";
                            int count1 = this.Da.ExecuteDMLQuery(sql1);

                            //if (count1 == 1) { }

                            //else 
                            //    MessageBox.Show("Delete form cart!");
                        }
                        else
                        {
                            MessageBox.Show("Insufficient quantity in stock!");
                        }
                    }
                    else if (!isAddInCart)
                    {
                        update = count + quantity1;
                        if (update >= 0)
                        {
                            var sql1 = "update product set quantity=" + update + "where product_id=" + id + ";";
                            int count1 = this.Da.ExecuteDMLQuery(sql1);

                            //if (count1 == 1) { }
                            ////MessageBox.Show("Data has been updated properly");
                            //else { }
                            //    //
                        }
                        else
                        {
                            MessageBox.Show("Insufficient quantity in stock!");
                        }
                    }
                }
                
                else
                {
                    MessageBox.Show("Product not found!");
                }
                }
            
            catch (Exception exc)
            {
                MessageBox.Show("Some Error!" + exc.Message);
            }
            this.PopulateGridView();
        }

        private void btnDeleteFromCart_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvShowProducts.SelectedRows.Count > 0)
                {
                    //MessageBox.Show("Please select a row first to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //return;
                    string productName = this.dgvShowProducts.SelectedRows[0].Cells[1].Value.ToString();
                    DialogResult res = MessageBox.Show("Are you sure want to delete " + productName.ToString() + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.Yes)
                    {
                        var id = this.dgvShowProducts.SelectedRows[0].Cells[0].Value.ToString();
                        var quantity = this.dgvShowProducts.SelectedRows[0].Cells[3].Value.ToString();
                        int quantity1 = Convert.ToInt32(quantity);
                        upadteQuantity(id, quantity1, false);
                        dgvShowProducts.Rows.RemoveAt(dgvShowProducts.SelectedRows[0].Index);
                        double sum = 0;//
                        for (int row = 0; row < dgvShowProducts.Rows.Count; row++)
                        {
                            sum = sum + Convert.ToDouble(dgvShowProducts.Rows[row].Cells[5].Value);
                        }
                        this.txtTotal.Text = Convert.ToString(sum);
                        this.dgvShowProducts.ClearSelection();
                        this.dgvProduct.ClearSelection();

                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row first");
                }                     
            }
            catch(Exception exc)
            {
                MessageBox.Show("An error has occured, please check: " + exc.Message);
            }
        }

        private void FormProductCart_Load(object sender, EventArgs e)
        {
            this.dgvProduct.ClearSelection();
            this.AutoIdGenerate();
            this.dgvShowProducts.Rows.Clear();
            this.txtTotal.Clear();
        }
        private void AutoIdGenerate()
        {
            var query = "select max(Invoice_Id) from Invoice ;";
            var dt = this.Da.ExecuteQueryTable(query);
            var oldId = dt.Rows[0][0];
            var num = Convert.ToInt32(oldId);
            var newId = ++num;
            this.txtInvoice.Text = newId.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll();
        }
        private void ClearAll()
        {
            this.txtProductId.Clear();
            this.txtProductName.Clear();
            this.txtProductPrice.Clear();
            this.txtCategory.Clear();
            this.txtQuantity.Clear();
            this.dgvProduct.ClearSelection();
            
        }
        public void ClearAllFinal()
        {
            this.txtProductId.Clear();
            this.txtProductName.Clear();
            this.txtProductPrice.Clear();
            this.txtCategory.Clear();
            this.txtQuantity.Clear();
            this.dgvProduct.ClearSelection();
            this.AutoIdGenerate();
            this.dgvShowProducts.Rows.Clear();
            this.txtTotal.Clear();

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.F1.Show();
            
        }

        private void btnProceedTopay_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtTotal.Text))
                {
                    MessageBox.Show("Select Product first!");
                    return;
                }
                
                string invoice = this.txtInvoice.Text;
                string total = this.txtTotal.Text;
                if(total=="0")
                {
                    MessageBox.Show("Please add product first!");
                    return;
                }
                this.Hide();
                new FormPaymentProcess(id, invoice, total, this).Show();
            }
            catch(Exception exc)
            {
                MessageBox.Show("An error occured!"+exc);
                return;
            }
        }

        private void FormProductCart_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                for (int row = 0; row < this.dgvShowProducts.Rows.Count; row++)
                {
                    int existingQty = Convert.ToInt32(dgvShowProducts.Rows[row].Cells[3].Value);
                    var id = this.dgvShowProducts.Rows[row].Cells[0].Value.ToString();

                    var sql = "SELECT Quantity FROM PRODUCT WHERE product_id = '" + id + "';";
                    var ds = this.Da.ExecuteQuery(sql);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int quantityInDb = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                        int updatedQty = existingQty + quantityInDb;

                        if (updatedQty > 0)
                        {
                            var sql1 = "UPDATE product SET quantity = " + updatedQty + " WHERE product_id = '" + id + "';";
                            this.Da.ExecuteDMLQuery(sql1);
                        }
                    }
                }
                Application.Exit();
            }

            catch (Exception exc)
            {
                MessageBox.Show("An error occured!" + exc);
                return;
            }

        }

        private bool ExistingProduct()
        {
            for (int row = 0; row < this.dgvShowProducts.Rows.Count; row++)
            {
                var id = this.txtProductId.Text.ToString();
                var id1= this.dgvShowProducts.Rows[row].Cells[0].Value.ToString();
                if (id == id1)
                {
                   return false;
                }
                
            }
            return true;
        }

        
    }
}
