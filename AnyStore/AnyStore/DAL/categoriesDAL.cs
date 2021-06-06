using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class categoriesDAL
    {
        // Static String Method for DAtaBase connection String
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Select Method
        public DataTable Select()
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            DataTable dt = new DataTable();


            try
            {
                //Writing sql Query to get all the data from database
                string sql = "SELECT * FROM tbl_categories";

                SqlCommand cmd = new SqlCommand(sql, conn);
                
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Open Database Connection
                conn.Open();

                //Adding the value from adapter to DataTable dt
                adapter.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        #endregion
        #region Insert new Category
        public bool Insert(categoriesBLL c)
        {
            //Creating a Boleean Variable and set its default value to false
            bool isSucces = false;

            //Connecting to Database
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Writing Query to add new Category
                string sql = "INSERT INTO tbl_categories (title, description, added_date, added_by) VALUES (@title, @description, @added_date, @added_by)";
                //Creating SQL Command to pass Values in our Quer
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Passing Values Throught parameter
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);
                
                //Open Database Connection
                conn.Open();

                //Creating the int variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //If the query is executed succesfully then its value will be greater than 0 else it will be less than 0

                if (rows > 0)
                {
                    isSucces = true;
                }
                else
                {
                    isSucces = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //ClosingDatabase Connection
                conn.Close();
            }
            return isSucces;
        }
        #endregion
        #region Update Method
        public bool Update(categoriesBLL c)
        {
            //Creating Boolean variable and set its default value to false
            bool isSucces = false;

            //Creating SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Query to Update Category
                string sql = "UPDATE tbl_categories SET title=@title, description=@description, added_date=@added_date, added_by=@added_by WHERE id=@id";

                //SQL command to Pass the Value on Sql query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing value using cmd
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);
                cmd.Parameters.AddWithValue("@id", c.id);

                //Open Database Connection
                conn.Open();

                //Creating a variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //if the query is succesfully executed than the value will be greater than 0
                if (rows > 0)
                {
                    isSucces = true;
                }
                else
                {
                    isSucces = false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return isSucces;
        }
        #endregion
        #region Delete Category Method
        public bool Delete(categoriesBLL c)
        {
            //Creating a bool variable and set by default False
            bool isSucces = false;

            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //SQL Query to delete from Database
                string sql = "DELETE FROM tbl_categories WHERE id=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                //Passing the value using cmd
                cmd.Parameters.AddWithValue("@id", c.id);

                //Open SqlConneciton
                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                //If the query is executed succefully then the value will be grater than 0 else less than 0
                if (rows > 0)
                {
                    isSucces = true;
                }
                else
                {
                    isSucces = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return isSucces;
        }
        #endregion
        #region Method for Search
        public DataTable Search(string keywords)
        {
            //SQL Connection for Database 
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Creating dataTable to hold the data from databaes
            DataTable dt = new DataTable();

            try
            {
                //SQL query to search Categories from DATABASE
                String sql = "SELECT * FROM tbl_categories WHERE id LIKE '%"+keywords+"%' OR title LIKE '%"+keywords+"%' OR description LIKE '%"+keywords+"%'";
                //Creating SQL comand to execute the query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting data from Database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DatabaseConnection
                conn.Open();

                //Passing values from adapter to Data table dt
                adapter.Fill(dt);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;

        }
        #endregion
    }
}
