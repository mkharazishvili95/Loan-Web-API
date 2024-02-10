using FluentValidation;
using FluentValidation.AspNetCore;
using Loan_Web_API.Database;
using Loan_Web_API.Identity;
using Microsoft.EntityFrameworkCore;

namespace Loan_Web_API.Validation
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterModel>
    {
        private readonly LoanApiContext _context;
        public UserRegisterValidator(LoanApiContext context) 
        {
            _context = context;

            RuleFor(user => user.FirstName).NotEmpty().WithMessage("Enter your FirstName!");
            RuleFor(user => user.LastName).NotEmpty().WithMessage("Enter your LastName!");
            RuleFor(user => user.Age).NotEmpty().WithMessage("Enter your Age!")
                .GreaterThanOrEqualTo(21).WithMessage("Your age should be more than 20 and less than 66 to register!")
                .LessThanOrEqualTo(65).WithMessage("Your age should be more than 20 and less than 66 to register!");
            RuleFor(user => user.ContactNumber).NotEmpty().WithMessage("Enter your Contact Number!")
                .GreaterThanOrEqualTo(500000000).WithMessage("Enter valid Contact Number!")
                .LessThanOrEqualTo(599999999).WithMessage("Enter valid Contact Number!")
                .Must(DifferentContactNumber).WithMessage("Contact Number already exists. Try another!");
            RuleFor(user => user.IdentityNumber).NotEmpty().WithMessage("Enter your Identity Number!")
                .Length(11, 15).WithMessage("Enter your valid Identity Number!")
                .Must(DifferentIdentityNumber).WithMessage("Identity number already exists. Try another!");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Enter your Email address!")
                .EmailAddress().WithMessage("Enter your valid Email address!")
                .Must(DifferentEmailAddress).WithMessage("Email address already exists. Try another!");
            RuleFor(user => user.UserName).NotEmpty().WithMessage("Enter your UserName!")
                .Length(6, 15).WithMessage("UserName length should be from 6 to 15 chars or numbers!")
                .Must(DifferentUserName).WithMessage("UserName already exists. Try another!");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Enter your Password!")
                .Length(6, 15).WithMessage("Password length should be from 6 to 15 chars or numbers!");
        }
        private bool DifferentEmailAddress(string eMail)
        {
            var theSameEmail = _context.Users.SingleOrDefault(user => user.Email.ToUpper() == eMail.ToUpper());
            return theSameEmail == null;
        }
        private bool DifferentUserName(string userName)
        {
            var theSameUserName = _context.Users.SingleOrDefault(user => user.UserName.ToUpper() == userName.ToUpper());
            return theSameUserName == null;
        }
        private bool DifferentContactNumber(int contactNumber)
        {
            var theSameContactNumber = _context.Users.SingleOrDefault(user => user.ContactNumber == contactNumber);
            return theSameContactNumber == null;
        }
        private bool DifferentIdentityNumber(string identityNumber)
        {
            var theSameIdentityNumberr = _context.Users.SingleOrDefault(user => user.IdentityNumber == identityNumber);
            return theSameIdentityNumberr == null;
        }
    }
}
