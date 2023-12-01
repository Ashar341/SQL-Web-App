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
                                buildingsinfo.PKBuilding ="" + reader.GetInt32(0);
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

        // Class to submit all the values from the table Customers
        public class BuildingsInfo
        {
            public String PKBuilding { get; set; }
            public String Building { get; set; }
        }
    }
}
