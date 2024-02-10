using Autofac.Features.OwnedInstances;
using Loan_Web_API.Database;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loan_Web_API.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int userId);
        Task<bool> BlockUser(int userId);
        Task<bool> UnblockUser(int userId);
        Task<IEnumerable<Loggs>> GetAllLogs();
        Task<IEnumerable<User>> GetBlockedUsers();
        Task<IEnumerable<User>> GetUsersByCountry(string country);
        Task<IEnumerable<User>> GetUsersByCity(string city);
        Task<IEnumerable<User>> GetUsersByAge(UserAgeRange ageRange);
        Task<bool> ChangeLoanStatusToRejected(int loanId);
        Task<bool> ChangeLoanStatusToApproved(int loanId);
        Task<IEnumerable<Loan>> GetLoansByDateTimeRange(LoanDateRange loanDateRange);
        Task<IEnumerable<Loan>> GetPaidLoans();
        Task<IEnumerable<Loan>> GetUnpaidLoans();
        Task<bool> BlockingOwnersOfOverdueLoans();
        Task<bool> DeleteLoan(int loanId);
        Task<bool> DeleteExpireAndPaidLoan(int loanId);
    }
    public class AdminService : IAdminService
    {
        private readonly LoanApiContext _context;
        public AdminService(LoanApiContext context)
        {
            _context = context;
        }

        public async Task<bool> BlockingOwnersOfOverdueLoans()
        {
            try
            {
                var userList = await _context.Users.ToListAsync();
                var loanList = await _context.Loans.ToListAsync();
                var usersWithOverdueLoans = userList
                            .Where(user => loanList.Any(loan => loan.UserId == user.Id && loan.PeriodTo < DateTime.Now && loan.LoanPaid == false))
                            .ToList();
                if (!usersWithOverdueLoans.Any())
                {
                    return false;
                }
                else
                {
                    foreach (var user in usersWithOverdueLoans)
                    {
                        user.IsBlocked = true;
                    }
                    _context.Update(userList);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> BlockUser(int userId)
        {
            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                if(existingUser == null)
                {
                    return false;
                }
                if(existingUser.Role == Roles.Admin)
                {
                    return false;
                }
                if(existingUser.IsBlocked == true)
                {
                    return false;
                }
                else
                {
                    var blockedUser = new BlockedUsers()
                    {
                        UserId = existingUser.Id,
                        FirstName = existingUser.FirstName,
                        LastName = existingUser.LastName,
                        Age = existingUser.Age,
                        ContactNumber = existingUser.ContactNumber,
                        IdentityNumber = existingUser.IdentityNumber,
                        Email = existingUser.Email,
                        UserName = existingUser.UserName
                    };
                    await _context.AddAsync(blockedUser);
                    existingUser.IsBlocked = true;
                    _context.Update(existingUser.IsBlocked);
                    return true;
                }
                
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeLoanStatusToApproved(int loanId)
        {
            try
            {
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId);
                if(existingLoan == null)
                {
                    return false;
                }
                if(existingLoan.Status ==  LoanStatus.Approved)
                {
                    return false;
                }
                if(existingLoan.Status == LoanStatus.Rejected)
                {
                    return false;
                }
                if(existingLoan.Status == LoanStatus.InProcessing)
                {
                    existingLoan.Status  = LoanStatus.Approved;
                    _context.Update(existingLoan);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeLoanStatusToRejected(int loanId)
        {
            try
            {
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId);
                if(existingLoan == null)
                {
                    return false;
                }
                if(existingLoan.Status == LoanStatus.InProcessing) 
                {
                    existingLoan.Status = LoanStatus.Rejected;
                    _context.Update(existingLoan);

                }
                else
                {
                    return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteExpireAndPaidLoan(int loanId)
        {
            try
            {
                var expiredAndPaidLoan = await _context.Loans.SingleOrDefaultAsync(oldLoan => oldLoan.Id == loanId);
                if(expiredAndPaidLoan == null)
                {
                    return false;
                }
                if(expiredAndPaidLoan.Status != LoanStatus.Approved)
                {
                    return false;
                }
                if(expiredAndPaidLoan.AmountOfCoveredLoan != expiredAndPaidLoan.TotalAmountPayableInGEL)
                {
                    return false;
                }
                if(expiredAndPaidLoan.LoanPaid != true)
                {
                    return false;
                }
                else
                {
                    _context.Remove(expiredAndPaidLoan);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteLoan(int loanId)
        {
            try
            {
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId);
                if(existingLoan == null)
                {
                    return false;
                }
                if(existingLoan.Status == LoanStatus.Approved)
                {
                    return false;
                }
                if (existingLoan.CashedOut == true)
                {
                    return false;
                }
                else
                {
                    _context.Remove(existingLoan);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Loggs>> GetAllLogs()
        {
            try
            {
                var logList = await _context.Loggs.ToListAsync();
                return logList;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                var userList = await _context.Users.Include(user => user.UserAddress).ToListAsync();
                if(userList.Count == 0)
                {
                    return null;
                }
                else
                {
                    return userList;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetBlockedUsers()
        {
            try
            {
                var getBlockedUsers = await _context.Users.Include(user => user.UserAddress)
                    .Where(user => user.IsBlocked == true).ToListAsync();
                if(getBlockedUsers.Count == 0)
                {
                    return null;
                }
                else
                {
                    return getBlockedUsers;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Loan>> GetLoansByDateTimeRange(LoanDateRange loanDateRange)
        {
            try
            {
                var getLoansByDateTimeRange = await _context.Loans
                    .Where(loan => loan.PeriodFrom <= loanDateRange.MaxDate && loan.PeriodTo >= loanDateRange.MinDate)
                    .ToListAsync();
                if (getLoansByDateTimeRange.Count == 0)
                {
                    return null;
                }
                else
                {
                    return getLoansByDateTimeRange;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Loan>> GetPaidLoans()
        {
            try
            {
                var paidLoans = await _context.Loans.Where(loan => loan.LoanPaid == true || loan.AmountOfCoveredLoan >= loan.TotalAmountPayableInGEL).ToListAsync();
                if(paidLoans.Count == 0)
                {
                    return null;
                }
                else
                {
                    return paidLoans;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Loan>> GetUnpaidLoans()
        {
            try
            {
                var paidLoans = await _context.Loans.Where(loan => loan.LoanPaid == false || loan.AmountOfCoveredLoan < loan.TotalAmountPayableInGEL).ToListAsync();
                if (paidLoans.Count == 0)
                {
                    return null;
                }
                else
                {
                    return paidLoans;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                var getUserById = await _context.Users.Include(user => user.UserAddress)
                    .SingleOrDefaultAsync(user => user.Id == userId);
                if(getUserById == null)
                {
                    return null;
                }
                else
                {
                    return getUserById;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsersByAge(UserAgeRange ageRange)
        {
            try
            {
                var getUsersByAge = await _context.Users.Include(user => user.UserAddress)
                    .Where(user => user.Age >= ageRange.MinAge && user.Age <= ageRange.MaxAge).ToListAsync();
                if(getUsersByAge.Count == 0)
                {
                    return null;
                }
                else
                {
                    return getUsersByAge;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsersByCity(string city)
        {
            try
            {
                var getUsersByCity = await _context.Users.Include(user => user.UserAddress)
                    .Where(user => user.UserAddress.City.ToUpper().Contains(city.ToUpper())).ToListAsync();
                if(getUsersByCity.Count == 0)
                {
                    return null;
                }
                else
                {
                    return getUsersByCity;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsersByCountry(string country)
        {
            try
            {
                var getUsersByCountry = await _context.Users.Include(user => user.UserAddress)
                    .Where(user => user.UserAddress.Country.ToUpper().Contains(country.ToUpper())).ToListAsync();
                if(getUsersByCountry.Count == 0)
                {
                    return null;
                }
                else
                {
                    return getUsersByCountry;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UnblockUser(int userId)
        {
            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                if (existingUser == null)
                {
                    return false;
                }
                if(existingUser.Role == Roles.Admin)
                {
                    return false;
                }
                if(existingUser.IsBlocked == false)
                {
                    return false;
                }
                else
                {
                    var existingUserInBlockedList = await _context.BlockedUsers.Where(user => user.UserId == userId).FirstOrDefaultAsync();
                    _context.Remove(existingUserInBlockedList);
                    existingUser.IsBlocked = false;
                    _context.Update(existingUser.IsBlocked);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
