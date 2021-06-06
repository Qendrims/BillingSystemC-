using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Close the windows
            this.Hide();
        }

        categoriesDAL cdal = new categoriesDAL();
        productsBLL p = new productsBLL();
        productsDAL pdal = new productsDAL();
        userDAL udal = new userDAL();
        private void frmProducts_Load(object sender, EventArgs e)
        {
            DataTable categoriesDT = cdal.Select();
            //Specify dataSource for category ComboBox
            cmbCategory.DataSource = categoriesDT;
            //Specify Display Member and Value member for combobox
            cmbCategory.DisplayMember = "title";
            cmbCategory.ValueMember = "title";

            //Load all the product in data Grid View
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get all the values from Product from
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.qty = 0;
            p.added_date = DateTime.Now;
            // Getting user name of logged in user
            String loggedUsr = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUsr);

            p.added_by = usr.id;

            //Create bool var to check if the product is sccessfully or not
            bool success = pdal.Insert(p);

            if (success == true)
            {
                MessageBox.Show("Product Added Succesfully");
                //Calling the clear method
                Clear();
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Add new Product");
            }
        }
        public void Clear()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtRate.Text = "";
            txtSearch.Text = "";
        }

        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;

            txtID.Text = dgvProducts.Rows[rowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvProducts.Rows[rowIndex].Cells[1].Value.ToString();
            cmbCategory.Text = dgvProducts.Rows[rowIndex].Cells[2].Value.ToString();
            txtDescription.Text = dgvProducts.Rows[rowIndex].Cells[3].Value.ToString();
            txtRate.Text = dgvProducts.Rows[rowIndex].Cells[4].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from UI
            p.id = int.Parse(txtID.Text);
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.added_date = DateTime.Now;
            // Getting user name of logged in user
            String loggedUsr = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUsr);

            p.added_by = usr.id;

            bool success = pdal.Update(p);

            if (success == true)
            {
                MessageBox.Show("Product Updated Successfully");
                Clear();

                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Product Failed to Update");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get the ID  of the product to Deleted
            p.id = int.Parse(txtID.Text);

            bool success = pdal.Delete(p);

            if (success == true)
            {
                MessageBox.Show("Product Deleted Succesfully");
                Clear();

                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;

            }
            else
            {
                MessageBox.Show("Failed to Delete Product");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword from Form
            string keywords = txtSearch.Text;

            if (keywords != null)
            {
                DataTable dt = pdal.Search(keywords);
                dgvProducts.DataSource = dt;
            }
            else
            {
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
        }
    }
}
