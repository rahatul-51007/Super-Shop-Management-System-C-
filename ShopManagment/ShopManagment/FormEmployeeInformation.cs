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
    public partial class FormEmployeeInformation : Form
    {
        private DataAccess Da { set; get;}
        private FormAdminDashboard Ad { set; get; }
        public FormEmployeeInformation()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.PopulateGridView();
            
        }
        public FormEmployeeInformation(FormAdminDashboard ad):this()
        {
            this.Ad = ad;
        }

        private void PopulateGridView(string sql = "select * from emptable;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvAddemp.AutoGenerateColumns = false;
            this.dgvAddemp.DataSource = ds.Tables[0];
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the informations.");
                    return;
                }
                if (!this.IsValidName(this.txtEmployeeName.Text))
                {
                    MessageBox.Show("Employee name should only contain alphabetic characters (A-Z, a-z).", "Invalid name");
                    return;
                }
                if (!this.IsValidContact(this.txtContact.Text))
                {
                    MessageBox.Show("Contact number should only contain numeric characters (0-9).", "Invalid contact");
                    return;
                }
                var sql = "insert into emptable values('" + this.txtEmployeeId.Text + "', '" + this.txtEmployeeName.Text + "', '" + this.txtPassword.Text + "', '" + this.txtContact.Text + "', '" + this.txtAddress.Text + "', '" + this.dtpDateOfBirth.Text + "','"+this.cmbRole.Text+"'); ";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count == 1)
                        MessageBox.Show("The data has been successfully added.", "Success");
                    else
                        MessageBox.Show("There was an issue adding the data. Please try again.", "Error");
            }
            catch(Exception exc)
            {
                MessageBox.Show("An error has occured, please check: " + exc.Message);
            }
            this.PopulateGridView();
            this.ClearAll();

        }
        

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll();
        }

        private void dgvAddemp_DoubleClick(object sender, EventArgs e)
        {
            
            this.txtEmployeeId.Text = this.dgvAddemp.CurrentRow.Cells[0].Value.ToString();
            this.txtEmployeeName.Text = this.dgvAddemp.CurrentRow.Cells[1].Value.ToString();
            this.txtPassword.Text = this.dgvAddemp.CurrentRow.Cells[2].Value.ToString();
            this.txtContact.Text = this.dgvAddemp.CurrentRow.Cells[3].Value.ToString();
            this.txtAddress.Text = this.dgvAddemp.CurrentRow.Cells[4].Value.ToString();
            this.dtpDateOfBirth.Text = this.dgvAddemp.CurrentRow.Cells[5].Value.ToString();
            this.cmbRole.Text = " ";
            
        }

        private bool IsValidToSave()
        {
            if (string.IsNullOrEmpty(this.cmbRole.Text) || string.IsNullOrEmpty(this.txtEmployeeId.Text) ||
                string.IsNullOrEmpty(this.txtEmployeeName.Text) || string.IsNullOrEmpty(this.txtPassword.Text) ||
                string.IsNullOrEmpty(this.txtContact.Text) || string.IsNullOrEmpty(this.txtAddress.Text))
                return false;
            else
                return true;
        }
        private bool IsValidName(string name)
        {
            foreach (char c in name)
            {
                if (!((c >= 65 && c <= 90) || (c >= 97 && c <= 122)))
                {
                    return false; 
                }
            }
            return true; 
        }
        private bool IsValidContact(string contact)
        {
            foreach (char c in contact)
            {
                if (c<48 || c>57)
                {
                    return false;
                }
            }
            return true;
        }
        private void FormEmployeeInformation_Load(object sender, EventArgs e)
        {
            this.dgvAddemp.ClearSelection();
           // this.AutoIdGenerate();
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.txtEmployeeId.Clear();    
            AutoIdGenerate();
            
        }
       

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvAddemp.SelectedRows.Count < 1)
                {
                    MessageBox.Show("Please select a row first to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var id = this.dgvAddemp.CurrentRow.Cells[0].Value.ToString();
                var title = this.dgvAddemp.CurrentRow.Cells[1].Value.ToString();

                if (id == "a-001")
                {
                    MessageBox.Show("You cannot delete the employee with ID 'a-001'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (id == "e-001")
                {
                    MessageBox.Show("You cannot delete the employee with ID 'e-001'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                DialogResult result = MessageBox.Show("Are you sure you want to delete " + title + "?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                var sql = "delete from emptable where EMP_ID = '" + id + "';";
                var count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show(title.ToUpper() + " has been removed from the list.");
                else
                    MessageBox.Show("Data hasn't been removed properly");

                this.PopulateGridView();
                this.ClearAll();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured, please check: " + exc.Message);
            }
        }
        private void ClearAll()
        {
            this.cmbRole.SelectedIndex = -1;
            this.txtEmployeeId.Clear();
            this.txtEmployeeName.Clear();
            this.txtPassword.Clear();
            this.txtContact.Clear();
            this.txtAddress.Clear();
            this.dtpDateOfBirth.Text = "";

            this.dgvAddemp.ClearSelection();
        }
        private void AutoIdGenerate()
        {
            
                if (this.cmbRole.Text == "admin")
                {
                    var query = "select max(EMP_ID) from emptable where role like 'a%';";
                    var dt = this.Da.ExecuteQueryTable(query);
                    var oldId = dt.Rows[0][0].ToString();
                    string[] temp = oldId.Split('-');
                    var num = Convert.ToInt32(temp[1]);
                    var newId = "a-" + (++num).ToString("d3");
                    //MessageBox.Show(newId);
                    this.txtEmployeeId.Text = newId;
                }
                else if (this.cmbRole.Text == "employee")
                {
                    var query = "select max(EMP_ID) from emptable where role like 'e%';";
                    var dt = this.Da.ExecuteQueryTable(query);
                    var oldId = dt.Rows[0][0].ToString();
                    string[] temp = oldId.Split('-');
                    var num = Convert.ToInt32(temp[1]);
                    var newId = "e-" + (++num).ToString("d3");
                    //MessageBox.Show(newId);
                    this.txtEmployeeId.Text = newId;
                }
                else
                {
                    return;
                }
        }
        

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Ad.Show();
            this.Ad.RefreshCounts();
        }

        private void FormEmployeeInformation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the informations.");
                    return;
                }
                if (!this.IsValidName(this.txtEmployeeName.Text))
                {
                    MessageBox.Show("Employee name should only contain alphabetic characters (A-Z, a-z).", "Invalid name");
                    return;
                }
                if (!this.IsValidContact(this.txtContact.Text))
                {
                    MessageBox.Show("Contact number should only contain numeric characters (0-9).", "Invalid contact");
                    return;
                }

                var query = "select * from emptable where EMP_ID = '" + this.txtEmployeeId.Text + "'";
                DataTable dt = this.Da.ExecuteQueryTable(query);
                if (dt.Rows.Count == 1)
                {
                    var sql = @"update emptable
                                set NAME = '" + this.txtEmployeeName.Text + @"',
                                PASSWORD = '" + this.txtPassword.Text + @"',
                                CONTACT = '" + this.txtContact.Text + @"',
                                ADDRESS = '" + this.txtAddress.Text + @"',
                                DOB = '" + this.dtpDateOfBirth.Text + @"'
                                
                                where EMP_ID = '" + this.txtEmployeeId.Text + "'; ";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count == 1)
                        MessageBox.Show("Data has been successfully updated.", "Success");
                    else
                        MessageBox.Show("There was an issue updating the data. Please try again.", "Error");


                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured, please check: " + exc.Message);
            }
            this.PopulateGridView();
            this.ClearAll();
        }
    }
}
