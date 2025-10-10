using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StoredProcedure.Data;
using StoredProcedure.Models;

namespace StoredProcedure.Controllers
{
    public class CustomerController : Controller
    {
        public StoredProcDbContext _context;
        public IConfiguration _config;
        public CustomerController(StoredProcDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult SearchResult()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchCustomers";
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Customers> model = new List<Customers>();
                while (sdr.Read())
                {
                    var details = new Customers();
                    details.FirstName = sdr["FirstName"].ToString();
                    details.LastName = sdr["LastName"].ToString();
                    details.Gender = sdr["Gender"].ToString();
                    details.Email = sdr["Email"].ToString();
                    details.PhoneNumber = Convert.ToInt32(sdr["PhoneNumber"]);
                    model.Add(details);
                }

                return new JsonResult(model);
            }
        }
    }
}
