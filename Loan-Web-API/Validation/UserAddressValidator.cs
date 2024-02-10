using FluentValidation;
using Loan_Web_API.Models;

namespace Loan_Web_API.Validation
{
    public class UserAddressValidator : AbstractValidator<Address>
    {
        public UserAddressValidator() 
        {
            RuleFor(address => address.Country).NotEmpty().WithMessage("Enter your Country!");
            RuleFor(address => address.City).NotEmpty().WithMessage("Enter your City!");
        }
    }
}
