using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static SQL_web.Pages.PartNumbers.IndexModel;
using System.Data.SqlClient;

namespace SQL_web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public List<PartNumberInfo> listpartnumbers = new List<PartNumberInfo>();

    [BindProperty(SupportsGet = true)]
    public String SearchTerm { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

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
                string sql = "SELECT * FROM PartNumbers WHERE PartNumber LIKE @SearchTerm";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Add parameter to the query
                    command.Parameters.AddWithValue("@SearchTerm", "%" + SearchTerm + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Create a new object and read the information from table
                            PartNumberInfo partinfo = new PartNumberInfo();
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

    public class PartNumberInfo
    {
        public String PKPartNumber { get; set; }
        public String PartNumber { get; set; }
        public String FKCustomer { get; set; }
        public String Available { get; set; }
    }

    public IActionResult OnGetExportToExcel()
    {
        // Implement export to Excel logic here
        // ...

        // For now, redirect to the same page
        return RedirectToPage("QueryResults");
    }
}
