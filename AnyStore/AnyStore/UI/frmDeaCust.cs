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
    public partial class frmDeaCust : Form
    {
        public frmDeaCust()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        DeaCustBLL dc = new DeaCustBLL();
        DeaCustDAL dcDal = new DeaCustDAL();

        userDAL uDal = new userDAL();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //GEt the values Form
            dc.type = cmbDeaCust.Text;
            dc.name = cmbDeaCust.Text;
            dc.email = cmbDeaCust.Text;
            dc.contact = cmbDeaCust.Text;
            dc.address = cmbDeaCust.Text;
            dc.added_date = DateTime.Now;
            //Getting the ID to Logged in user
            string loggedUsr = frmLogin.loggedIn;
            userBLL usr = uDal.GetIDFromUsername(loggedUsr);
            dc.added_by = usr.id;

            bool success = dcDal.Insert(dc);
            if (success == true)
            {
                MessageBox.Show("Dealer or Costumer added Succesfully");
                Clear();
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed To Add");
            }
        }
        public void Clear()
        {
            txtDeaCustID.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtSearch.Text = "";
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            DataTable dt = dcDal.Select();
            dgvDeaCust.DataSource = dt;
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;

            txtDeaCustID.Text = dgvDeaCust.Rows[rowIndex].Cells[0].Value.ToString();
            cmbDeaCust.Text = dgvDeaCust.Rows[rowIndex].Cells[1].Value.ToString();
            txtName.Text = dgvDeaCust.Rows[rowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvDeaCust.Rows[rowIndex].Cells[3].Value.ToString();
            txtContact.Text = dgvDeaCust.Rows[rowIndex].Cells[4].Value.ToString();
            txtAddress.Text = dgvDeaCust.Rows[rowIndex].Cells[5].Value.ToString();




        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dc.id = int.Parse(txtDeaCustID.Text);
            dc.type = cmbDeaCust.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtContact.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;

            string loggedUsr = frmLogin.loggedIn;
            userBLL usr = uDal.GetIDFromUsername(loggedUsr);
            dc.added_by = usr.id;

            bool succes =dcDal.Update(dc);

            if (succes == true)
            {
                MessageBox.Show("Dealer&Costumer Updated Successfully");
                Clear();
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Update");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get the id of the User
            dc.id = int.Parse(txtDeaCustID.Text);

            bool succes = dcDal.Delete(dc);

            if (succes == true)
            {
                MessageBox.Show("Dealer or Costumer Deleted Successfully");
                Clear();
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Delete Delaer or Costumer");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyWOrd form TextBox
            string keyword = txtSearch.Text;

            if (keyword != null)
            {
                DataTable dt = dcDal.Search(keyword);
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
        }
    }
}
