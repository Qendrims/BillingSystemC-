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
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();

        }
        categoriesBLL c = new categoriesBLL();
        categoriesDAL dal = new categoriesDAL();
        userDAL udal = new userDAL();

        private void btnADD_Click(object sender, EventArgs e)
        {
            //GEt the values from Category form
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //Passing the id of Logged in User in added by field
            c.added_by = usr.id;

            //Creating Boolean method to insert data into Database
            bool success = dal.Insert(c);

            //If the category is inserted succesfully then the value will be true else will be false
            if (success == true)
            {
                MessageBox.Show("New Category Inserted Successfully");
                Clear();
                //Refresh Data Grid View
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Insert new Category.");
            }

        }
        public void Clear()
        {
            txtCategoryID.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtSearch.Text = "";
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            //Here write the code to display all the categories in Data grid view
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;
        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Find the row Index of the Row Clicked in data Grid View
            int RowIndex = e.RowIndex;
            txtCategoryID.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from the Categories form
            c.id = int.Parse(txtCategoryID.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;
            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //Passing the id of Logged in User in added by field
            c.added_by = usr.id;

            //Creating variable to update category and Check
            bool success = dal.Update(c);
            //If the category is updated succesfully the nthe value will be true lese it will be false
            if (success == true)
            {
                MessageBox.Show("Category Updated Successfully");
                Clear();
                //Refresh Data Grid View
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Update Category");
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get the ID of the CAtrgory wich we want to delete
            c.id = int.Parse(txtCategoryID.Text);

            // creating boolean variable to delete the category
            bool success = dal.Delete(c);

            // If the category is deleted succesfull the value will be true else false
            if (success == true)
            {
                MessageBox.Show("Category deleted successfully");
                Clear();
                //Refresh DAta Grid View
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Delete Category");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the Keywords
            string keywords = txtSearch.Text;

            //Filte the categories based on Keywords
            if (keywords != null)
            {
                //Use search method to Display categories
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;

            }
            else
            {
                //Use Select method to Display all Categories
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;

            }
        }
    }
}
