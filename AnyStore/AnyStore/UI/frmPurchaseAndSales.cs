using AnyStore.BLL;
using AnyStore.DAL;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public frmPurchaseAndSales()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();
        DataTable transactionDT = new DataTable();
        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            // Get the transactionType value from UserDashboard
            string type = frmUserDashboard.transactionType;
            //Set the value on lblTop
            lblTop.Text = type;

            //Specify Colums for transiction DATATABLE
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword from the txtBox
            string keyword = txtSearch.Text;

            if (keyword == "")
            {
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }
            DeaCustBLL dc = dcDAL.SearchDealerCostumerForTransaction(keyword);

            //Now Transfer or set the value from DeaCustBLL to textboxes
            txtName.Text = dc.name;
            txtEmail.Text = dc.email;
            txtContact.Text = dc.contact;
            txtAddress.Text = dc.address;


        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword from product Search TextBox
            string keyword = txtNameProduct.Text;

            if (keyword == "")
            {
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
                return;
            }

            //Search the product and display on respective textboxes
            productsBLL p = pDAL.GetProductsForTransaction(keyword);

            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            decimal Rate = decimal.Parse(txtRate.Text);
            decimal qty = decimal.Parse(txtQty.Text);

            decimal Total = Rate * qty;

            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            subTotal = subTotal + Total;

            if (productName == "")
            {
                MessageBox.Show("Select the product first. Try Again!");
            }
            else
            {
                transactionDT.Rows.Add(productName,Rate,qty,Total);

                dgvAddedProducts.DataSource = transactionDT;

                txtSubTotal.Text = subTotal.ToString();

                txtSearch.Text = "";
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            string value = txtDiscount.Text;

            if(value == "")
            {
                MessageBox.Show("Please ADD Discount first");
            }
            else
            {
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                decimal discount = decimal.Parse(txtDiscount.Text);

                decimal grandTotal = ((100 - discount) / 100)* subTotal;

                txtGrandTotal.Text = grandTotal.ToString();
            }
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            string check = txtGrandTotal.Text;
            if (check == "")
            {
                MessageBox.Show("Calculate the discount and set the Grand Total first.");
            }
            else
            {
                decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                decimal vat = decimal.Parse(txtVat.Text);
                decimal grandTotalWithVAT = ((100 + vat) / 100) *previousGT;

                txtGrandTotal.Text = grandTotalWithVAT.ToString();
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
            decimal paidAmount = decimal.Parse(txtPaidAmount.Text);

            decimal returnAmount = paidAmount - grandTotal;

            txtReturnAmount.Text = returnAmount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Get the Values from Purchase Sales
            transactionsBLL transaction = new transactionsBLL();

            transaction.type = lblTop.Text;

            //Get the id of Costumer
            string deaCustName = txtName.Text;
            DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

            transaction.dea_cust_id = dc.id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text),2);
            transaction.transaction_date = DateTime.Now;
            transaction.tax = decimal.Parse(txtVat.Text);
            transaction.discount = decimal.Parse(txtDiscount.Text);

            string username = frmLogin.loggedIn;
            userBLL u = uDAL.GetIDFromUsername(username);

            transaction.added_by = u.id;
            transaction.transactionDetails = transactionDT;

            bool success = false;

            //Actual code to insert Transaction and Transaction Details
            using (TransactionScope scope = new TransactionScope())
            {
                int transactionID = -1;
                bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                //Use For loop to insert TD
                for(int i=0;i<transactionDT.Rows.Count; i++)
                {
                    transactionDetailBLL transactionDetail = new transactionDetailBLL();
                    //Get the product name and convert to ID
                    string ProductName = transactionDT.Rows[i][0].ToString();
                    productsBLL p = pDAL.GetProductIDFromName(ProductName);

                    transactionDetail.product_id = p.id;
                    transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                    transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                    transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()),2);
                    transactionDetail.dea_cust_id = dc.id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.id;

                    //Here Increase or Decrease Product Qty
                    string transactionType = lblTop.Text;
                    bool x =false;
                    if(transactionType == "Purchase")
                    {
                        //Increase PRoduct
                        x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }
                    else
                    {
                        //Decrease product
                         x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }
                    //Insert TransactionDT insine the dataBase
                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && x && y;
                    
                }

                if (success == true)
                {

                    scope.Complete();

                    //Code to print Bill
                    DGVPrinter printer = new DGVPrinter();

                    printer.Title = "\r\n\r\nAnyStore PVT.LTD. \r\n\r\n";
                    printer.SubTitle = "Prishtine, Kosovo\r\n Phone:01-xxxxx \r\n\r\n";
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = "Discount: "+ txtDiscount.Text + "%\r\n" + "VAT: " + txtVat.Text + "%\r\n" + "Grand Total: " + txtGrandTotal.Text + "\r\n\r\n" + "Thank You For Doing bussines with us";
                    printer.FooterSpacing = 15;
                    printer.PrintDataGridView(dgvAddedProducts);

                    MessageBox.Show("Transaction Completed Succesfully");
                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();

                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtContact.Text = "";
                    txtAddress.Text = "";
                    //txtSearchProduct.Text = "";
                    txtProductName.Text = "";
                    txtInventory.Text = "0";
                    txtRate.Text = "0";
                    txtQty.Text = "0";
                    txtSubTotal.Text = "0";
                    txtDiscount.Text = "0";
                    txtVat.Text = "0";
                    txtGrandTotal.Text = "0";
                    txtPaidAmount.Text = "0";
                    txtReturnAmount.Text = "0";

                }
                else
                {
                    MessageBox.Show("Transaction Failed");
                }

            }
        }
    }
}
