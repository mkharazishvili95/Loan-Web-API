using System.Collections.Generic;
using System.Threading.Tasks;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Loan_Web_API.Services;

namespace FakeServices
{
    public class FakeAdminService : IAdminService
    {
        public Task<bool> BlockingOwnersOfOverdueLoans()
        {
            return Task.FromResult(true);
        }

        public Task<bool> BlockUser(int userId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ChangeLoanStatusToApproved(int loanId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ChangeLoanStatusToRejected(int loanId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteExpireAndPaidLoan(int loanId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteLoan(int loanId)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Loggs>> GetAllLogs()
        {
            var fakeLogs = new List<Loggs>
            {
                new Loggs { Id = 1},
                new Loggs { Id = 2}
            };
            return Task.FromResult<IEnumerable<Loggs>>(fakeLogs);
        }

        public Task<IEnumerable<User>> GetAllUsers()
        {
            var fakeUsers = new List<User>
            {
                new User { Id = 1, UserName = "user1" },
                new User { Id = 2, UserName = "user2" }
            };
            return Task.FromResult<IEnumerable<User>>(fakeUsers);
        }

        public Task<IEnumerable<User>> GetBlockedUsers()
        {
            var fakeBlockedUsers = new List<User>
            {
                new User { Id = 3, UserName = "blockedUser1", IsBlocked = true },
                new User { Id = 4, UserName = "blockedUser2", IsBlocked = true }
            };
            return Task.FromResult<IEnumerable<User>>(fakeBlockedUsers);
        }

        public Task<IEnumerable<Loan>> GetLoansByDateTimeRange(LoanDateRange loanDateRange)
        {
            var fakeLoans = new List<Loan>
            {
                new Loan { Id = 1, Status = LoanStatus.InProcessing },
                new Loan { Id = 2, Status = LoanStatus.Approved }
            };
            return Task.FromResult<IEnumerable<Loan>>(fakeLoans);
        }

        public Task<IEnumerable<Loan>> GetPaidLoans()
        {
            var fakePaidLoans = new List<Loan>
            {
                new Loan { Id = 5, LoanPaid = true },
                new Loan { Id = 6, LoanPaid = true }
            };
            return Task.FromResult<IEnumerable<Loan>>(fakePaidLoans);
        }

        public Task<IEnumerable<Loan>> GetUnpaidLoans()
        {
            var fakeUnpaidLoans = new List<Loan>
            {
                new Loan { Id = 7, LoanPaid = false },
                new Loan { Id = 8, LoanPaid = false }
            };
            return Task.FromResult<IEnumerable<Loan>>(fakeUnpaidLoans);
        }

        public Task<User> GetUserById(int userId)
        {
            var fakeUser = new User { Id = userId, UserName = "fakeUser" };
            return Task.FromResult(fakeUser);
        }

        public Task<IEnumerable<User>> GetUsersByAge(UserAgeRange ageRange)
        {
            var fakeUsers = new List<User>
            {
                new User { Id = 9, Age = 25 },
                new User { Id = 10, Age = 30 }
            };
            return Task.FromResult<IEnumerable<User>>(fakeUsers);
        }

        public Task<IEnumerable<User>> GetUsersByCity(string city)
        {
            var fakeUsers = new List<User>
            {
                new User { Id = 11, UserAddress = new Address { City = "City1" } },
                new User { Id = 12, UserAddress = new Address { City = "City2" } }
            };
            return Task.FromResult<IEnumerable<User>>(fakeUsers);
        }

        public Task<IEnumerable<User>> GetUsersByCountry(string country)
        {
            var fakeUsers = new List<User>
            {
                new User { Id = 13, UserAddress = new Address { Country = "Country1" } },
                new User { Id = 14, UserAddress = new Address { Country = "Country2" } }
            };
            return Task.FromResult<IEnumerable<User>>(fakeUsers);
        }

        public Task<bool> UnblockUser(int userId)
        {
            return Task.FromResult(true);
        }
    }
}
