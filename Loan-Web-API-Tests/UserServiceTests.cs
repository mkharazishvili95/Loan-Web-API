using System.Threading.Tasks;
using FakeServices;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Loan_Web_API.Services;
using NUnit.Framework;

namespace FakeServices
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new FakeUserService();
        }

        [Test]
        public async Task RegisterNewUser_SuccessfulRegistration()
        {
            var userRegisterModel = new UserRegisterModel
            {
                FirstName = "Misho",
                LastName = "Kharazishvili",
                Age = 28,
                ContactNumber = 555555555,
                IdentityNumber = "01010101010",
                Email = "Misho999@gmail.com",
                UserName = "Misho999",
                Password = PasswordHashing.HashPassword("Misho999"),
                UserAddress = new Address{
                    Country = "Georgia",
                    City = "Tbilisi"
                }
            };
            var registeredUser = await _userService.RegisterNewUser(userRegisterModel);
            Assert.IsNotNull(registeredUser);
            Assert.AreEqual(userRegisterModel.FirstName, registeredUser.FirstName);
            Assert.AreEqual(userRegisterModel.LastName, registeredUser.LastName);
            Assert.AreEqual(userRegisterModel.Age, registeredUser.Age);
            Assert.AreEqual(userRegisterModel.ContactNumber, registeredUser.ContactNumber);
            Assert.AreEqual(userRegisterModel.IdentityNumber, registeredUser.IdentityNumber);
            Assert.AreEqual(userRegisterModel.Email, registeredUser.Email);
            Assert.AreEqual(userRegisterModel.UserName, registeredUser.UserName);
            Assert.AreEqual(PasswordHashing.HashPassword(userRegisterModel.Password),registeredUser.Password);
            Assert.AreEqual(userRegisterModel.UserAddress, registeredUser.UserAddress);
        }

        [Test]
        public async Task UserLoginForm_SuccessfulLogin()
        {
            var userLoginModel = new UserLoginModel
            {
                Email = "Misho999@gmail.com",
                UserName = "Misho999",
                Password = "Misho999"
            };
            var loggedInUser = await _userService.UserLoginForm(userLoginModel);
            Assert.IsNotNull(loggedInUser);
            Assert.AreEqual("Misho", loggedInUser.FirstName);
            Assert.AreEqual("Misho999", loggedInUser.UserName);
            Assert.AreNotEqual("Gio", loggedInUser.UserName);
        }
    }
}
