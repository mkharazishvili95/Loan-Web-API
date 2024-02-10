using Loan_Web_API.Database;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Loan_Web_API.Validation;
using Microsoft.EntityFrameworkCore;

namespace Loan_Web_API.Services
{
    public interface IUserService
    {
        Task<User> RegisterNewUser(UserRegisterModel register);
        Task<User> UserLoginForm(UserLoginModel login);
    }
    public class UserService : IUserService
    {
        private readonly LoanApiContext _context;
        public UserService(LoanApiContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterNewUser(UserRegisterModel newUser)
        {
            try
            {
                var userValidator = new UserRegisterValidator(_context);
                var validatorResults = userValidator.Validate(newUser);
                if (!validatorResults.IsValid)
                {
                    return null;
                }
                else
                {
                    var registerNewUser = new User()
                    {
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        Age = newUser.Age,
                        ContactNumber = newUser.ContactNumber,
                        IdentityNumber = newUser.IdentityNumber,
                        Email = newUser.Email,
                        UserName = newUser.UserName,
                        Password = PasswordHashing.HashPassword(newUser.Password),
                        Role = Roles.User,
                        UserAddress = newUser.UserAddress
                    };
                    
                    await _context.Users.AddAsync(registerNewUser);
                    return registerNewUser;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> UserLoginForm(UserLoginModel login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password) || string.IsNullOrEmpty(login.Email))
                {
                    return null;
                }

                var existingUser = await _context.Users
                    .SingleOrDefaultAsync(user => user.UserName == login.UserName);


                if (existingUser == null || PasswordHashing.HashPassword(login.Password) != (existingUser.Password) ||
                    login.Email.ToUpper() != existingUser.Email.ToUpper() || login.UserName != existingUser.UserName)
                {
                    return null;
                }

                return existingUser;
            }
            catch
            {
                return null;
            }
        }


    }
}
