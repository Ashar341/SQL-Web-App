using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static SQL_web.Pages.Customers.IndexModel;
using System.Data.SqlClient;
using static SQL_web.Pages.Buildings.IndexModel;

namespace SQL_web.Pages.Buildings
{
    public class CreateModel : PageModel
    {


        public BuildingsInfo buildingsInfo = new BuildingsInfo();
        public string errorMessage = "";
        public string success = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            //Grab information submited from the Create HTML
            buildingsInfo.Building = Request.Form["Building"];

            if (buildingsInfo.Building.Length == 0)
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
                    //Input
                    string qsl = "INSERT INTO Buildings" +
                        "(Building) VALUES" +
                        "(@Building)";

                    using (SqlCommand command = new SqlCommand(qsl, connection))
                    {
                        command.Parameters.AddWithValue("@Building", buildingsInfo.Building);
                        

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            buildingsInfo.Building = "";
            
            success = "All information saved correctly!";

            Response.Redirect("/Buildings/Index");
        }
    }
}
