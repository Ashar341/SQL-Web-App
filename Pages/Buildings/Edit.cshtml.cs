using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using static SQL_web.Pages.Buildings.IndexModel;

namespace SQL_web.Pages.Buildings
{
    public class EditModel : PageModel
    {

        public BuildingsInfo buildingsInfo = new BuildingsInfo();
        public string errorMessage = "";
        public string success = "";

        public void OnGet()
        {
            try
            {
                String id = Request.Query["id"];

                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Buildings WHERE PKBuilding=@id";

                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                buildingsInfo.PKBuilding = "" + reader.GetInt32(0);
                                buildingsInfo.Building = reader.GetString(1);
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
            String id = Request.Query["id"];

            buildingsInfo.PKBuilding = Request.Form["id"];
            buildingsInfo.Building = Request.Form["Building"];

            if (buildingsInfo.Building.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try 
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;Integrated Security=True";

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE Buildings " +
                        "SET Building = @Building " +
                        "WHERE PKBuilding = @id";

                    using( SqlCommand command = new SqlCommand(sql,connection))
                    {
                        command.Parameters.AddWithValue("@id", buildingsInfo.PKBuilding);
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
