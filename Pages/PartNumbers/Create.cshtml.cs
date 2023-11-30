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
        public string errorMessage = "";
        public string success = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            
            
            //Obtain information from the HTML request
            partinfo.PartNumber = Request.Form["partnumber"];
            partinfo.FKCustomer = Request.Form["fkcustomer"];
            partinfo.Available = Request.Form["available"];

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
                        "(@partnumber, @fkcustomer, @available)";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@partnumber", partinfo.PartNumber);
                        command.Parameters.AddWithValue("@fkcustomer", partinfo.FKCustomer);
                        command.Parameters.AddWithValue("@available", partinfo.Available);

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
