
namespace Loan_Web_API.Models
{
    public class AddNewLoan
    {
        public int Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int LoanPeriodDays { get; set; }
        public double TotalAmountPayable { get; set; }
    }
}
