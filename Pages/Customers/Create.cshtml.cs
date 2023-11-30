using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static SQL_web.Pages.Customers.IndexModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SQL_web.Pages.Customers
{
    public class CreateModel : PageModel
    {

        public CustomertInfo customerInfo = new CustomertInfo();

        public string errorMessage = "";

        public string success = "";

        public void OnGet()
        {
        }

        public void OnPost() 
        { 
            //Grab information submited from the Create HTML
            customerInfo.Customer = Request.Form["customer"];
            customerInfo.Prefix = Request.Form["prefix"];
            customerInfo.FKBuilding = Request.Form["FKBuilding"];

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
                string connectionString = "Data Source = DESKTOP - S2E78MP\\WEBAPPPRODUCCION; Initial Catalog = Materials; User ID = MyLogin; Password = ***********; Trust Server Certificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Input
                    string qsl = "INSERT INTO Customers" +
                        "(Customer, Prefix, FKBuilding) VALUES" +
                        "(@customer, @prefix, @FKBuilding)";

                    using(SqlCommand command = new SqlCommand(qsl, connection))
                    {
                        command.Parameters.AddWithValue("@customer", customerInfo.Customer);
                        command.Parameters.AddWithValue("@prefix", customerInfo.Prefix);
                        command.Parameters.AddWithValue("@KFBuilding", customerInfo.FKBuilding);

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
