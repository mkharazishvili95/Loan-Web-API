using Loan_Web_API.Database;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Loan_Web_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loan_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly LoanApiContext _context;
        private readonly IAdminService _adminService;
        public AdminController(LoanApiContext context, IAdminService adminService)
        {
            _context = context;
            _adminService = adminService;
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userList = await _adminService.GetAllUsers();
                if(userList == null)
                {
                    return NotFound(new { Message = "There is no any Users yet!" });
                }
                else
                {
                    return Ok(userList);
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var getUserById = await _adminService.GetUserById(userId);
                if(getUserById == null)
                {
                    return NotFound(new { Message = $"There is no any User by ID: {userId}" });
                }
                else
                {
                    return Ok(getUserById);
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetUsersByCountry")]
        public async Task<IActionResult> GetUsersByCountry(string country)
        {
            try
            {
                var getUsersByCountry = await _adminService.GetUsersByCountry(country);
                if(getUsersByCountry == null)
                {
                    return NotFound(new { Message = $"There is no any User from: {country}" });
                }
                else
                {
                    return Ok(getUsersByCountry);
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetUsersByCity")]
        public async Task<IActionResult> GetUsersByCity(string city)
        {
            try
            {
                var getUsersByCity = await _adminService.GetUsersByCity(city);
                if(getUsersByCity == null)
                {
                    return NotFound(new { Message = $"There is no any User from: {city}" });
                }
                else
                {
                    return Ok(getUsersByCity);
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetUsersByAge")]
        public async Task<IActionResult> GetUsersByAge([FromBody]UserAgeRange userAgeRange)
        {
            try
            {
                var getUsersByAge = await _adminService.GetUsersByAge(userAgeRange);
                if(getUsersByAge == null)
                {
                    return NotFound(new { Message = "There is no any User by this age range!" });
                }
                else
                {
                    return Ok(getUsersByAge);
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetBlockedUsers")]
        public async Task<IActionResult> GetBlockedUsers()
        {
            try
            {
                var getBlockedUsers = await _adminService.GetBlockedUsers();
                if(getBlockedUsers == null)
                {
                    return NotFound(new { Message = "There is no any block User in the database yet!" });
                }
                else
                {
                    return Ok(getBlockedUsers);
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetLoansByDateTimeRange")]
        public async Task<IActionResult> GetLoansByDateTimeRange([FromBody]LoanDateRange loanDateRange)
        {
            try
            {
                var getLoansByDateTimeRange = await _adminService.GetLoansByDateTimeRange(loanDateRange);
                if(getLoansByDateTimeRange == null)
                {
                    return NotFound(new { Message = $"There is no any loan from: {loanDateRange.MinDate} to {loanDateRange.MaxDate} range!" });
                }
                else
                {
                    return Ok(getLoansByDateTimeRange);
                }

            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAllLoggs")]
        public async Task<IActionResult> GetAllLoggs()
        {
            try
            {
                var loggList = await _adminService.GetAllLogs();
                return Ok(loggList);
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("BlockUser")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                if (existingUser == null)
                {
                    return NotFound(new { Message = $"There is no any User by ID: {userId} to block!" });
                }
                if (existingUser.Role == Roles.Admin)
                {
                    return BadRequest(new { ErrorMessage = $"You can not block another Admin!" });
                }
                if (existingUser.IsBlocked == true)
                {
                    return BadRequest(new { Message = "That User is already blocked!" });
                }
                else
                {
                    await _adminService.BlockUser(userId);
                    await _context.SaveChangesAsync();
                    return Ok(new { Message = $"User: {existingUser.UserName}, by ID: {userId} has been successfully blocked!" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("UnblockUser")]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                if (existingUser == null)
                {
                    return NotFound(new { Message = $"There is no any User by ID: {userId} to unblock!" });
                }
                if (existingUser.Role == Roles.Admin)
                {
                    return BadRequest(new { ErrorMessage = "You can not unblock another Admin!" });
                }
                if (existingUser.IsBlocked == false)
                {
                    return BadRequest(new { Message = "That User is already unblocked!" });
                }
                else
                {
                    await _adminService.UnblockUser(userId);
                    await _context.SaveChangesAsync();
                    return Ok(new { SuccessMessage = $"User: {existingUser.UserName}, by ID: {userId} has been successfully unblocked!" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("BlockingOwnersOfOverdueLoans")]
        public async Task<IActionResult> BlockingOwnersOfOverdueLoans()
        {
            try
            {
                var userList = await _context.Users.ToListAsync();
                var loanList = await _context.Loans.ToListAsync();
                var usersWithOverdueLoans = userList
                            .Where(user => loanList.Any(loan => loan.UserId == user.Id && loan.PeriodTo < DateTime.Now && loan.LoanPaid == false))
                            .ToList();
                if (usersWithOverdueLoans == null)
                {
                    return BadRequest(new { Message = "There are no any user, who has overdue loans!" });
                }
                if (!usersWithOverdueLoans.Any())
                {
                    return BadRequest(new { Message = "There are no any user, who has overdue loans!" });

                }
                else
                {
                    await _adminService.BlockingOwnersOfOverdueLoans();
                    await _context.SaveChangesAsync();
                    return Ok(new { Message = "User list has refreshed!" });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("ChangeLoanStatusToRejected")]
        public async Task<IActionResult> ChangeLoanStatusToRejected(int loanId)
        {
            try
            {
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId);
                if(existingLoan == null)
                {
                    return NotFound(new { Message = $"There is no any loan by ID: {loanId}" });
                }
                if(existingLoan.Status == LoanStatus.Rejected)
                {
                    return BadRequest(new { ErrorMessage = "That loan is already rejected!" });
                }
                if(existingLoan.Status == LoanStatus.Approved)
                {
                    return BadRequest(new { ErrorMessage = "You can not change approved loan status!" });
                }
                else
                {
                    await _adminService.ChangeLoanStatusToRejected(loanId);
                    await _context.SaveChangesAsync();
                    return Ok(new { SuccessMessage = "Loan status has successfully changed as Rejected!" });
                }

            }catch(Exception ex)

            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("ChangeLoanStatusToApproved")]
        public async Task<IActionResult> ChangeLoanStatusToApproved(int loanId)
        {
            try
            {
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId);
                if (existingLoan == null)
                {
                    return NotFound(new { Message = $"There is no any loan by ID: {loanId}" });
                }
                if (existingLoan.Status == LoanStatus.Approved)
                {
                    return BadRequest(new { Message = "That loan is already Approved!" });
                }
                if (existingLoan.Status == LoanStatus.Rejected)
                {
                    return BadRequest(new { Message = "You can just change loans with status InProcessing" });
                }
                if (existingLoan.Status == LoanStatus.InProcessing)
                {
                    existingLoan.Status = LoanStatus.Approved;
                    await _adminService.ChangeLoanStatusToApproved(loanId);
                    await _context.SaveChangesAsync();
                }
                return Ok(new { SuccessMessage = "Loan status has successfully changed from InProcessing to Approved!" });

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetPaidLoans")]
        public async Task<IActionResult> GetPaidLoans()
        {
            try
            {
                var getallPaidLoans = await _adminService.GetPaidLoans();
                if(getallPaidLoans == null)
                {
                    return NotFound(new { Message = "There is no any paid loan yet!" });
                }
                else
                {
                    return Ok(getallPaidLoans);
                }

            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetUnpaidLoans")]
        public async Task<IActionResult> GetUnpaidLoans()
        {
            try
            {
                var getAllUnpaidLoans = await _adminService.GetUnpaidLoans();
                if(getAllUnpaidLoans == null)
                {
                    return NotFound(new { Message = "There is no any unpaid loan yet!" });
                }
                else
                {
                    return Ok(getAllUnpaidLoans);
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("DeleteLoan")]
        public async Task<IActionResult> DeleteLoan(int loanId)
        {
            try
            {
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId);
                if(existingLoan == null)
                {
                    return NotFound(new { Message = $"There is no any loan by ID: {loanId}" });
                }
                if(existingLoan.Status == LoanStatus.Approved || existingLoan.CashedOut == true)
                {
                    return BadRequest(new {ErrorMessage = "You can not delete that loan, because it's already Approved or cashed out!"});
                }
                else
                {
                    await _adminService.DeleteLoan(loanId);
                    await _context.SaveChangesAsync();
                    return Ok(new { SuccessMessage = "Loan has successfully deleted!" });
                }

            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("DeleteExpireAndPaidLoan")]
        public async Task<IActionResult> DeleteExpireAndPaidLoan(int loanId)
        {
            try
            {
                var expiredAndPaidLoan = await _context.Loans.SingleOrDefaultAsync(oldLoan => oldLoan.Id == loanId);
                if(expiredAndPaidLoan == null)
                {
                    return NotFound(new { Message = $"There is no any loan by ID: {loanId}" });
                }
                if(expiredAndPaidLoan.Status != LoanStatus.Approved)
                {
                    return BadRequest(new { Error = "You can not delete any loan by In Processing or Rejected statuses!" });
                }
                if(expiredAndPaidLoan.AmountOfCoveredLoan != expiredAndPaidLoan.TotalAmountPayableInGEL || expiredAndPaidLoan.LoanPaid != true)
                {
                    return BadRequest(new { Error = "You can not delete that loan, because it has not payed yet!" });
                }
                else
                {
                    await _adminService.DeleteExpireAndPaidLoan(loanId);
                    await _context.SaveChangesAsync();
                    return Ok(new { SuccessMessage = "Expired and payed loan has successfully deleted from the database!" });
                }

            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
    }
}
