using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace StoredProcedure.Models
{
    public class Customers
    {
        [Key]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
    }
}
