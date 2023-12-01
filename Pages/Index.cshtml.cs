using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static SQL_web.Pages.PartNumbers.IndexModel;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.Configuration;


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

    public IActionResult OnGet()
    {
        try
        {
            Console.WriteLine("OnGet method is executed.");

            // Connection to database in localhost with Windows authentication
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Materials;User ID=MyLogin2;Password=1234";

            // Create connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open connection with the database
                connection.Open();

                // Select all from the all the tables needed
                // Need to reduce the list information to improve search time
                string sql = "SELECT * FROM PartNumbers " +
                    "LEFT JOIN Customers " +
                    "ON PartNumbers.FKCustomers = Customers.PKCustomers " +
                    "LEFT JOIN Buildings " +
                    "ON Customers.FKBuilding = Buildings.PKBuilding " +
                    "WHERE PartNumber LIKE @SearchTerm; ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Add parameter to the query
                    command.Parameters.AddWithValue("@SearchTerm", "%" + SearchTerm + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new object and read the information from the table
                            PartNumberInfo partinfo = new PartNumberInfo();
                            partinfo.PKPartNumber = "" + reader.GetInt32(0);
                            partinfo.PartNumber = reader.GetString(1);
                            partinfo.FKCustomer = "" + reader.GetInt32(2);
                            partinfo.Available = reader.GetBoolean(3) ? "Yes" : "No";
                            partinfo.PKCustomer = "" + reader.GetInt32(4);
                            partinfo.Customer = reader.GetString(5);
                            partinfo.Prefix = reader.GetString(6);
                            partinfo.FKBuilding = "" + reader.GetInt32(7);
                            partinfo.PKBuilding = "" + reader.GetInt32(8);
                            partinfo.Building = reader.GetString(9);

                            // Save the information on the list to show in the HTML
                            listpartnumbers.Add(partinfo);
                        }
                    }
                }
            }
            
            // Creating the Excel function
            if (Request.Query.TryGetValue("export", out var exportValue))
            {
                // Check if 'export' parameter is present in the request and equals "true"
                if (exportValue == "true")
                {
                    var info = GenerateExcel(listpartnumbers);
                    var byteArray = info.GetAsByteArray();

                    // Return the Excel file as a downloadable file
                    return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedData.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception " + ex.ToString());
        }

        return Page();
    }

    public class PartNumberInfo
    {
        public String PKPartNumber { get; set; }
        public String PartNumber { get; set; }
        public String FKCustomer { get; set; }
        public String Available { get; set; }
        public String PKCustomer { get; set; }
        public String Customer { get; set; }
        public String Prefix { get; set; }
        public String FKBuilding { get; set; }
        public String PKBuilding { get; set; }
        public String Building { get; set;}
    }

    public ExcelPackage GenerateExcel(List<PartNumberInfo> data)
    {

        ///Calling excel function
        ExcelPackage package = new ExcelPackage();
        ExcelWorksheet Sheet = package.Workbook.Worksheets.Add("Report");
        //Submitting the information on the excel headers
        Sheet.Cells["A1"].Value = "ID";
        Sheet.Cells["B1"].Value = "Part Number";
        Sheet.Cells["C1"].Value = "Customer";
        Sheet.Cells["D1"].Value = "Available";

        Sheet.Cells["A2"].LoadFromCollection(data);

        return package;

    }
}
