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
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }
        userBLL u = new userBLL();
        userDAL dal = new userDAL();

        public object DataGridViewUsers { get; private set; }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            

            //Getting data from UI
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAddress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_time = DateTime.Now;

            //Getting user name by the loged in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = dal.GetIDFromUsername(loggedUser);
            u.added_by = usr.id;

            //Inserting Data into Database
            bool succes = dal.Insert(u);
            //If the data succesfully inserted then the value of succes will bw true else will be false
            if (succes == true)
            {
                //Data Succesfully Inserted
                MessageBox.Show("User succesfully created.");
                clear();
            }
            else
            {
                //Failed
                MessageBox.Show("Failed to add new user.");
            }
            //Refreshing Data Grid Wiew
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;

        }

        private void frmUseres_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }
        private void clear()
        {
            txtUserID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            cmbGender.Text = "";
            cmbUserType.Text = "";
        }

        private void dgvUsers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the index of particular row
            int rowIndex = e.RowIndex;
            txtUserID.Text = dgvUsers.Rows[rowIndex].Cells[0].Value.ToString();
            txtFirstName.Text = dgvUsers.Rows[rowIndex].Cells[1].Value.ToString();
            txtLastName.Text = dgvUsers.Rows[rowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvUsers.Rows[rowIndex].Cells[3].Value.ToString();
            txtUsername.Text = dgvUsers.Rows[rowIndex].Cells[4].Value.ToString();
            txtPassword.Text = dgvUsers.Rows[rowIndex].Cells[5].Value.ToString();
            txtContact.Text = dgvUsers.Rows[rowIndex].Cells[6].Value.ToString();
            txtAddress.Text = dgvUsers.Rows[rowIndex].Cells[7].Value.ToString();
            cmbGender.Text = dgvUsers.Rows[rowIndex].Cells[8].Value.ToString();
            cmbUserType.Text = dgvUsers.Rows[rowIndex].Cells[9].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from User UI
            u.id = Convert.ToInt32(txtUserID.Text);
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAddress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_time = DateTime.Now;
            u.added_by = 1;

            // Updating data to database
            bool success = dal.Update(u);
            //if data is updated then the value will be true else will be false
            if(success == true)
            {
                //Data Updated Succesfully
                MessageBox.Show("User succesfully updated");
                clear();
            }
            else
            {
                //failed to update
                MessageBox.Show("Failed to update");
            }
            //Refreshing data Grid View
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Getting User ID from Form
            u.id = Convert.ToInt32(txtUserID.Text);

            bool success = dal.Delete(u);
            //if data is deleted then the value will be true else it will be false
            if(success == true)
            {
                //User Deleted Successfully
                MessageBox.Show("User deleted successfully");
            }
            else
            {
                MessageBox.Show("Failed to delete user");
            }
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }
    }
}
