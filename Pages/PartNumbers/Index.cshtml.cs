using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace SQL_web.Pages.PartNumbers
{
    public class IndexModel : PageModel
    {
        public List<partNumberInfo> listpartnumbers = new List<partNumberInfo>();

        public void OnGet()
        {
            try
            {
                //Connection to database in localhost with windows authenthication
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;User ID=MyLogin2;Password=1234";

                //Create connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //open connection with db

                    connection.Open();

                    // Select the desired information from table part numbers
                    // Left join to grab the correct customer name
                    string sql = "SELECT * FROM PartNumbers " +
                        "LEFT JOIN Customers "+
                        "ON PartNumbers.FKCustomers = Customers.PKCustomers;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //Create a new object and read the information from table
                                partNumberInfo partinfo = new partNumberInfo();
                                partinfo.PKPartNumber = "" + reader.GetInt32(0);
                                partinfo.PartNumber = reader.GetString(1);
                                partinfo.FKCustomer = "" + reader.GetInt32(2);
                                partinfo.Available = reader.GetBoolean(3) ? "Yes" : "No";
                                partinfo.PKCustomer = "" + reader.GetInt32(4);
                                partinfo.Customer = reader.GetString(5);
                                partinfo.Prefix = reader.GetString(6);
                                partinfo.FKBuilding = "" + reader.GetInt32(7);


                                //Save the information on the list to show in the html

                                listpartnumbers.Add(partinfo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
            }
        }

        public class partNumberInfo
        {
            public String PKPartNumber { get; set; }
            public String PartNumber { get; set; }
            public String FKCustomer { get; set; }
            public String Available { get; set; }
            public String PKCustomer {  get; set; }
            public String Customer { get; set; }
            public String Prefix { get; set; }
            public String FKBuilding { get; set; }
        }
    }
}
