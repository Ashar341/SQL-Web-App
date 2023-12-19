using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SQL_web.Pages.Customers
{
    public class IndexModel : PageModel
    {
        //Create list to save the variables
        public List<CustomertInfo> listCustomers = new List<CustomertInfo>();
        public void OnGet()
        {

            try
            {
                //Connection to database in localhost with windows authenthication
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;Integrated Security=True";

                //Create connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Customers " +
                        "LEFT JOIN Buildings "+
                        "ON Customers.FKBuilding = Buildings.PKBuilding;";

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
                                customersinfo.PKBuilding = ""+ reader.GetInt32(4);
                                customersinfo.Building = reader.GetString(5);
                                
                                // Save the information from table to the list
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
            public String PKBuilding { get; set; }
            public String Building { get; set; }
        }
    }
}
