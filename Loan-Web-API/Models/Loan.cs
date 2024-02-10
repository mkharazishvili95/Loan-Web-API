using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;

namespace Loan_Web_API.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime PeriodFrom { get; set; } = DateTime.Now;
        public DateTime PeriodTo { get; set; }
        public int LoanPeriodInDays {  get; set; }
        public string Status { get; set; } = string.Empty;
        public double TotalAmountPayableInGEL {  get; set; }
        public double AmountOfCoveredLoan { get; set; } = 0;
        public bool CashedOut { get; set; } = false;
        public bool LoanPaid { get; set; } = false;
        public int UserId {  get; set; }
    }
}
