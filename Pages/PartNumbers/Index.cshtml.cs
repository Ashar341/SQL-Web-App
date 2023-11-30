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
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;Integrated Security=True";

                //Create connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //open connection with db

                    connection.Open();

                    // Select all from table part numbers
                    string sql = "SELECT * FROM PartNumbers";

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
                                partinfo.Available = reader.GetBoolean(3) ? "1" : "0";

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
            public String Available { get; set;}
        }
    }
}
