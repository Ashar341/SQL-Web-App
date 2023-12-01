using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static SQL_web.Pages.Customers.IndexModel;
using static SQL_web.Pages.PartNumbers.IndexModel;

namespace SQL_web.Pages.PartNumbers
{
    public class CreateModel : PageModel
    {
        public partNumberInfo partinfo = new partNumberInfo();
        public List<CustomertInfo> listCustomers = new List<CustomertInfo>();
        public string errorMessage = "";
        public string success = "";
        public void OnGet()
        {

            try
            {
                //Connection to database in localhost with windows authenthication
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;User ID=MyLogin2;Password=1234";

                //Create connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Customers";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Continue operation of connection and operation
                            while (reader.Read())
                            {
                                //Create a new object and read the info from table
                                CustomertInfo customersinfo = new CustomertInfo();
                                customersinfo.PKCustomers = "" + reader.GetInt32(0);
                                customersinfo.Customer = reader.GetString(1);
                                customersinfo.Prefix = reader.GetString(2);
                                customersinfo.FKBuilding = "" + reader.GetInt32(3);

                                // Save the information from table to the list
                                listCustomers.Add(customersinfo);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public void OnPost() 
        {
            //Obtain information from the HTML request
            partinfo.PartNumber = Request.Form["partnumber"];
            partinfo.FKCustomer = Request.Form["FKCustomer"];
            partinfo.Available = Request.Form["Available"];

            //Check if all the fields are submitted

            if (partinfo.PartNumber.Length == 0 || partinfo.FKCustomer.Length == 0 ||
                partinfo.Available.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //Submit the information in db
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Input new info

                    string sql = "INSERT INTO PartNumbers " +
                        "(PartNumber, FKCustomers, Available) VALUES " +
                        "(@partnumber, @FKCustomer, @Available)";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@partnumber", partinfo.PartNumber);
                        command.Parameters.AddWithValue("@FKCustomer", partinfo.FKCustomer);
                        command.Parameters.AddWithValue("@Available", partinfo.Available);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = errorMessage + ex.Message;
                return;
            }


            partinfo.PartNumber = "";
            partinfo.FKCustomer = "";
            partinfo.Available = "";
            success = "All information saved";

            Response.Redirect("/PartNumbers/Index");

            
        }
    }
}
