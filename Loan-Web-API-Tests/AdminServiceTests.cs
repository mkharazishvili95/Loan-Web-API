using System.Collections.Generic;
using System.Threading.Tasks;
using FakeServices;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Loan_Web_API.Services;
using NUnit.Framework;

namespace FakeServices
{
    [TestFixture]
    public class AdminServiceTests
    {
        private IAdminService _adminService;

        [SetUp]
        public void Setup()
        {
            _adminService = new FakeAdminService();
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnFakeUsers()
        {
            var result = await _adminService.GetAllUsers();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<User>>(result);
        }

        [Test]
        public async Task BlockUser_ShouldBlockUser()
        {
            int userIdToBlock = 1;
            var result = await _adminService.BlockUser(userIdToBlock);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ChangeLoanStatusToApproved_ShouldChangeStatus()
        {
            int loanIdToApprove = 1;
            var result = await _adminService.ChangeLoanStatusToApproved(loanIdToApprove);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetPaidLoans_ShouldReturnPaidLoans()
        {
            var result = await _adminService.GetPaidLoans();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<Loan>>(result);
        }

        [Test]
        public async Task GetBlockedUsers_ShouldReturnBlockedUsers()
        {
            var result = await _adminService.GetBlockedUsers();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<User>>(result);
        }

        [Test]
        public async Task GetLoansByDateTimeRange_ShouldReturnLoans()
        {
            var loanDateRange = new LoanDateRange
            {
                MinDate = DateTime.Now.AddDays(-10),
                MaxDate = DateTime.Now.AddDays(10)
            };
            var result = await _adminService.GetLoansByDateTimeRange(loanDateRange);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<Loan>>(result);
        }

        [Test]
        public async Task GetUnpaidLoans_ShouldReturnUnpaidLoans()
        {
            var result = await _adminService.GetUnpaidLoans();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<Loan>>(result);
        }

        [Test]
        public async Task GetUserById_ShouldReturnFakeUser()
        {
            int userIdToRetrieve = 1;
            var result = await _adminService.GetUserById(userIdToRetrieve);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<User>(result);
            Assert.AreEqual(userIdToRetrieve, result.Id);
        }

        [Test]
        public async Task GetUsersByAge_ShouldReturnUsersInAgeRange()
        {
            var ageRange = new UserAgeRange { MinAge = 20, MaxAge = 35 };
            var result = await _adminService.GetUsersByAge(ageRange);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<User>>(result);
        }

        [Test]
        public async Task GetUsersByCity_ShouldReturnUsersInCity()
        {
            string cityToSearch = "City1";
            var result = await _adminService.GetUsersByCity(cityToSearch);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<User>>(result);
        }

        [Test]
        public async Task GetUsersByCountry_ShouldReturnUsersInCountry()
        {
            string countryToSearch = "Country1";
            var result = await _adminService.GetUsersByCountry(countryToSearch);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<User>>(result);
        }

        [Test]
        public async Task UnblockUser_ShouldUnblockUser()
        {
            int userIdToUnblock = 1;
            var result = await _adminService.UnblockUser(userIdToUnblock);
            Assert.IsTrue(result);
        }
    }
}
