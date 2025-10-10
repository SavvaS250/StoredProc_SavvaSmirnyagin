using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StoredProcedure.Data;
using StoredProcedure.Models;

namespace StoredProcedure.Controllers
{
    public class EmployeeController : Controller
    {
        public StoredProcDbContext _context;
        public IConfiguration _config;
        public EmployeeController(StoredProcDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DynamicSQL()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchEmployees";
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (sdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = sdr["FirstName"].ToString();
                    details.LastName = sdr["LastName"].ToString();
                    details.Gender = sdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(sdr["Salary"]);
                    model.Add(details);
                }

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult DynamicSQL(string firstName, string lastName, string gender, int salary)
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                StringBuilder sbCommand = new StringBuilder("SELECT * FROM Employees WHERE 1=1" );

                if (!string.IsNullOrEmpty(firstName))
                {
                    sbCommand.Append(" AND FirstName=@FirstName");
                    SqlParameter parameter = new SqlParameter("@FirstName", firstName);
                    cmd.Parameters.Add(parameter);
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    sbCommand.Append(" AND LastName=@LastName");
                    SqlParameter parameter = new SqlParameter("@LastName", lastName);
                    cmd.Parameters.Add(parameter);
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    sbCommand.Append(" AND Gender=@Gender");
                    SqlParameter parameter = new SqlParameter("@Gender", gender);
                    cmd.Parameters.Add(parameter);
                }

                if (salary != 0)
                {
                    sbCommand.Append(" AND Salary=@Salary");
                    SqlParameter parameter = new SqlParameter("@Salary", salary);
                    cmd.Parameters.Add(parameter);
                }
                cmd.CommandText = sbCommand.ToString();
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();

                while (sdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = sdr["FirstName"].ToString();
                    details.LastName = sdr["LastName"].ToString();
                    details.Gender = sdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(sdr["Salary"]);
                    model.Add(details);
                }

                return View(model);
            }
        } 


    }
}
