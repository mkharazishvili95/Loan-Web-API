namespace Loan_Web_API.Models
{
    public class BlockedUsers
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public int ContactNumber { get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
