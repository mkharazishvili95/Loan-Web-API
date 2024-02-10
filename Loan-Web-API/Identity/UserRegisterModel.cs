using Loan_Web_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loan_Web_API.Identity
{
    public class UserRegisterModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public int ContactNumber { get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Address UserAddress { get; set; } = new Address();
    }
}
