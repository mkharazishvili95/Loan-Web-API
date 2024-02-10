using System.Threading.Tasks;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Loan_Web_API.Services;

namespace FakeServices
{
    public class FakeUserService : IUserService
    {
        public Task<User> RegisterNewUser(UserRegisterModel register)
        {
            var fakeUser = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Age = register.Age,
                ContactNumber = register.ContactNumber,
                IdentityNumber = register.IdentityNumber,
                Email = register.Email,
                UserName = register.UserName,
                Password = PasswordHashing.HashPassword(register.Password),
                Role = Roles.User,
                UserAddress = register.UserAddress
            };

            return Task.FromResult(fakeUser);
        }

        public Task<User> UserLoginForm(UserLoginModel login)
        {
            var fakeUser = new User
            {
                FirstName = "Misho",
                LastName = "Kharazishvili",
                Age = 28,
                ContactNumber = 555555555,
                IdentityNumber = "01010101010",
                Email = "Misho999@gmail.com",
                UserName = "Misho999",
                Password = PasswordHashing.HashPassword("Misho999"),
                Role = Roles.User
            };
            return Task.FromResult(fakeUser);
        }
    }
}
