using System.ComponentModel.DataAnnotations;

namespace Loan_Web_API.Identity
{
    public class Loggs
    {
        [Key]
        public int Id { get; set; }
        public string LoggedUser { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserRole { get; set; } = string.Empty;
        public string UserEmail {  get; set; } = string.Empty;
        public DateTime LoggDate { get; set; } = DateTime.Now;
    }
}
