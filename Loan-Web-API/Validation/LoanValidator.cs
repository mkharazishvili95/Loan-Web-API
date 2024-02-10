using FluentValidation;
using Loan_Web_API.Models;
using System.Linq;

namespace Loan_Web_API.Validation
{
    public class LoanValidator : AbstractValidator<AddNewLoan>
    {
        public LoanValidator()
        {
            RuleFor(loan => loan.Amount).NotEmpty().WithMessage("Enter Loan amount!")
                .GreaterThanOrEqualTo(500).WithMessage("Minimal loan amount must be more or equal to 500!")
                .LessThanOrEqualTo(300000).WithMessage("Maximum loan amount must be less or equal to 300,000!");

            RuleFor(loan => loan.LoanPeriodDays).NotEmpty().WithMessage("Loan days must be more than 0!")
                .LessThanOrEqualTo(1825).WithMessage("Loan days must be less or equal to 1825, that is 5 years!");

            RuleFor(loan => loan.Currency).NotEmpty().WithMessage("Enter loan currency!")
                .Must(ValidLoanCurrency).WithMessage($"Enter currency from them: {LoanCurrency.GEL}, {LoanCurrency.USD}, {LoanCurrency.EUR}");

            RuleFor(loan => loan.Type).NotEmpty().WithMessage("Enter loan type!")
                .Must(ValidLoanType).WithMessage("Enter a valid loan type from them:" +
                "FastLoan, CarLoan, EmergencyLoan, EducationLoan, HomeLoan, BusinessLoan, BuyWithCredit, PersonalLoan");

        }

        private bool ValidLoanCurrency(string currency)
        {
            return currency.Equals(LoanCurrency.GEL) || currency.Equals(LoanCurrency.USD) || currency.Equals(LoanCurrency.EUR);
        }

        private bool ValidLoanType(string type)
        {
            var validLoanTypes = new[]
            {
                LoanType.EmergencyLoan,
                LoanType.FastLoan,
                LoanType.EducationLoan,
                LoanType.HomeLoan,
                LoanType.CarLoan,
                LoanType.BusinessLoan,
                LoanType.BuyWithCredit,
                LoanType.PersonalLoan
            };

            return validLoanTypes.Any(validType => type.Equals(validType));
        }
    }
}
