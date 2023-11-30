using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SQL_web.Pages.Customers
{
    public class IndexModel : PageModel
    {
        public List<CustomertInfo> listCustomers = new List<CustomertInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-S2E78MP\\WEBAPPPRODUCCION;Initial Catalog=Test;Integrated Security=True;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Customers";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomertInfo customersinfo = new CustomertInfo
                                {
                                    PKCustomers = reader.GetInt32(0).ToString(),
                                    Customer = reader.GetString(1),
                                    Prefix = reader.GetString(2),
                                    FKBuilding = reader.GetInt32(3).ToString()
                                };

                                listCustomers.Add(customersinfo);
                            }
                        }
                    }
                }
            }

            catch ( Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        // Class to submit all the values from the table Customers
        public class CustomertInfo
        {
            public String PKCustomers { get; set; }
            public String Customer { get; set; }
            public String Prefix { get; set; }
            public String FKBuilding { get; set; }
        }
    }
}
