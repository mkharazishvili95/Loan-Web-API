using Loan_Web_API.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Loan_Web_API.Identity.Roles;

namespace Loan_Web_API.Models
{
    public class User
    {
        [Key]
        public int Id {  get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age {  get; set; }
        public double Balance { get; set; } = 0;
        public int ContactNumber {  get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsBlocked { get; set; } = false;
        public string Role { get; set; } = Roles.User;
        [ForeignKey("AddressId")]
        public Address UserAddress { get; set; } = new Address();
    }
}
