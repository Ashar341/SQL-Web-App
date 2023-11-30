using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SQL_web.Pages.Buildings
{
    public class IndexModel : PageModel
    {
        public List<BuildingsInfo> listBuildings = new List<BuildingsInfo>();
        
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-S2E78MP\\WEBAPPPRODUCCION;Initial Catalog=Materials;User ID=MyLogin;Password=***********;Integrated Security=False;MultipleActiveResultSets=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Name=YourAppName;Network Library=dbmssocn";

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
                                buildingsinfo.PKBuilding ="" + reader.GetInt32(0);
                                buildingsinfo.Building = reader.GetString(2);

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

        // Class to submit all the values from the table Customers
        public class BuildingsInfo
        {
            public String PKBuilding;
            public String Building;
        }
    }
}
