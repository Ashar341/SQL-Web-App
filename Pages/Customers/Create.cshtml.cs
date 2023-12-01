using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using static SQL_web.Pages.Buildings.IndexModel;
using static SQL_web.Pages.Customers.IndexModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SQL_web.Pages.Customers
{
    public class CreateModel : PageModel
    {

        public CustomertInfo customerInfo = new CustomertInfo();
        public List<BuildingsInfo> listBuildings = new List<BuildingsInfo>();
        public string errorMessage = "";
        public string success = "";

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;User ID=MyLogin2;Password=1234";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Buildings";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BuildingsInfo buildingsinfo = new BuildingsInfo();
                                buildingsinfo.PKBuilding = "" + reader.GetInt32(0);
                                buildingsinfo.Building = reader.GetString(1);

                                listBuildings.Add(buildingsinfo);
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
            //Grab information submited from the Create HTML
            customerInfo.Customer = Request.Form["customer"];
            customerInfo.Prefix = Request.Form["prefix"];
            customerInfo.FKBuilding = Request.Form["FKBuilding"];

            //check if there is information in all the 
            if (customerInfo.FKBuilding.Length == 0 || customerInfo.Prefix.Length == 0 || 
                customerInfo.Customer.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }
            //Save information in db
            try
            {
                //connect to db
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Input and connection to table from db
                    string qsl = "INSERT INTO Customers" +
                        "(Customer, Prefix, FKBuilding) VALUES" +
                        "(@customer, @prefix, @FKBuilding)";

                    using (SqlCommand command = new SqlCommand(qsl, connection))
                    {
                        command.Parameters.AddWithValue("@customer", customerInfo.Customer);
                        command.Parameters.AddWithValue("@prefix", customerInfo.Prefix);
                        command.Parameters.AddWithValue("@FKBuilding", customerInfo.FKBuilding);

                        command.ExecuteNonQuery();
                    }
                }
            } catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            customerInfo.Customer = "";
            customerInfo.Prefix = "";
            customerInfo.FKBuilding = "1";
            success = "All information saved correctly!";

            Response.Redirect("/Customers/Index");
        }
    }
}
