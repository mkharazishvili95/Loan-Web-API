using FluentValidation;
using Loan_Web_API.Database;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Loan_Web_API.Services;
using Loan_Web_API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loan_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly LoanApiContext _context;
        private readonly double _loanAmount;
        public LoanController(LoanApiContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost("AddNewLoan")]
        public async Task<IActionResult> AddNewLoan(AddNewLoan newLoan)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                if(existingUser.IsBlocked == true)
                {
                    return BadRequest(new { ErrorMessage = "You have no permission to request a loan, because you are blocked!" });

                }
                var loanValidator = new LoanValidator();
                var validatorResults = loanValidator.Validate(newLoan);
                if(validatorResults.IsValid == false)
                {
                    return BadRequest(validatorResults.Errors);
                }
                else
                {
                    var addLoan = new Loan()
                    {
                        Amount = newLoan.Amount,
                        Currency = newLoan.Currency,
                        PeriodFrom = DateTime.Now,
                        PeriodTo = DateTime.Now.AddDays(newLoan.LoanPeriodDays),
                        LoanPeriodInDays = newLoan.LoanPeriodDays,
                        Type = newLoan.Type,
                        Status = LoanStatus.InProcessing.ToString(),
                        TotalAmountPayableInGEL = CalculateTotalAmountPayable(newLoan.Amount, newLoan.Currency),
                        UserId = userId
                    };
                    await _context.AddAsync(addLoan);
                    await _context.SaveChangesAsync();
                    if (newLoan.Currency == LoanCurrency.USD)
                    {
                        var newResultForUSD = addLoan.TotalAmountPayableInGEL * (CurrencyRate.USD_Rate);
                        _context.Update(addLoan);
                        return Ok(new { SuccessMessage = $"You have successfully received a loan: {addLoan.Amount} {addLoan.Currency}, which status is currently in process!" });
                    }
                    if (newLoan.Currency == LoanCurrency.EUR)
                    {
                        var newResultForEUR = addLoan.TotalAmountPayableInGEL * (CurrencyRate.EUR_Rate);
                        _context.Update(addLoan);
                        return Ok(new { SuccessMessage = $"You have successfully received a loan: {addLoan.Amount} {addLoan.Currency}, which status is currently in process!" });
                    }
                    return Ok(new { SuccessMessage = $"You have successfully received a loan: {addLoan.Amount} {addLoan.Currency}, which status is currently in process!" });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }

        private double CalculateTotalAmountPayable(int amount, string currency)
        {
            double interestRate = 0.12;

            switch (currency)
            {
                case LoanCurrency.USD:
                    return amount * (1 + interestRate) * CurrencyRate.USD_Rate;
                case LoanCurrency.EUR:
                    return amount * (1 + interestRate) * CurrencyRate.EUR_Rate;
                case LoanCurrency.GEL:
                    return amount * (1 + interestRate);
                default:
                    throw new ArgumentException("Invalid currency!");
            }
        }
        [Authorize]
        [HttpPut("UpdateLoan")]
        public async Task<IActionResult> UpdateLoan(int loanId, AddNewLoan updateLoan)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId && loan.UserId == userId);
                if (existingLoan == null)
                {
                    return BadRequest(new { Message = $"There is no loan with ID: {loanId}" });
                }
                if (existingUser.IsBlocked)
                {
                    return BadRequest(new { ErrorMessage = "You cannot update the loan because your account is blocked!" });
                }
                if (existingLoan.Status != LoanStatus.InProcessing)
                {
                    return BadRequest(new { Message = $"You cannot update the loan, because status of the loan is: {existingLoan.Status}." });
                }
                var loanValidator = new LoanValidator();
                var validatorResults = loanValidator.Validate(updateLoan);

                if (!validatorResults.IsValid)
                {
                    return BadRequest(validatorResults.Errors);
                }
                existingLoan.Amount = updateLoan.Amount;
                existingLoan.Currency = updateLoan.Currency;
                existingLoan.PeriodFrom = DateTime.Now;
                existingLoan.PeriodTo = DateTime.Now.AddDays(updateLoan.LoanPeriodDays);
                existingLoan.LoanPeriodInDays = updateLoan.LoanPeriodDays;
                existingLoan.Type = updateLoan.Type;
                existingLoan.Status = LoanStatus.InProcessing.ToString();
                existingLoan.TotalAmountPayableInGEL = CalculateTotalAmountPayable(updateLoan.Amount, updateLoan.Currency);

                _context.Update(existingLoan);
                await _context.SaveChangesAsync();
                if (updateLoan.Currency == LoanCurrency.USD)
                {
                    var newResultForUSD = updateLoan.TotalAmountPayable * CurrencyRate.USD_Rate;
                    return Ok(new { SuccessMessage = $"You have successfully updated a loan: {updateLoan.Amount} {updateLoan.Currency}, which status is currently in process!" });
                }
                else if (updateLoan.Currency == LoanCurrency.EUR)
                {
                    var newResultForEUR = updateLoan.TotalAmountPayable * CurrencyRate.EUR_Rate;
                    return Ok(new { SuccessMessage = $"You have successfully updated a loan: {updateLoan.Amount} {updateLoan.Currency}, which status is currently in process!" });
                }
                else
                {
                    return Ok(new { SuccessMessage = "You have successfully updated a loan!" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize]
        [HttpGet("GetMyLoans")]
        public async Task<IActionResult> GetMyLoans()
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                var myLoans = await _context.Loans.Where(loan => loan.UserId ==  userId).ToListAsync();
                if(myLoans.Count == 0)
                {
                    return NotFound(new { Message = "You have not any loan yet!" });
                }
                else 
                {
                    return Ok(myLoans); 
                }

            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize]
        [HttpGet("GetMyLoanById")]
        public async Task<IActionResult> GetMyLoanById(int loanId)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                var getMyLoanById = await _context.Loans.Where(loan => loan.Id == loanId && loan.UserId == userId).SingleOrDefaultAsync();
                if(getMyLoanById == null)
                {
                    return NotFound(new { Message = $"There is no any Loan by ID: {loanId}" });
                }
                else
                {
                    return Ok(getMyLoanById);
                }

            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize]
        [HttpPost("TransferMoneyToMyBalance")]
        public async Task<IActionResult> TransferMoneyToMyBalance(int loanId)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                var loanList = await _context.Loans.ToListAsync();
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == loanId);
                var bankStatement = await _context.Banks.SingleOrDefaultAsync() ?? new Bank();
                if(existingUser.IsBlocked == true)
                {
                    return BadRequest(new { ErrorMessage = "You have no permission to transfer money to your balance, because you are blocked!" });
                }
  
                if(await _context.Loans.Where(loan => loan.Id == loanId && loan.UserId == userId).SingleOrDefaultAsync() == null)
                {
                    return NotFound(new { Message = $"There is no any loan by ID: {loanId}!" });
                }
                if(existingLoan.Status == LoanStatus.InProcessing || existingLoan.Status == LoanStatus.Rejected)
                {
                    return BadRequest(new { ErrorMessage = $"Your loan status is: {existingLoan.Status}, and can not transfer money, until it will become Approved!" });
                }
                if(existingLoan.CashedOut == true)
                {
                    return BadRequest(new { ErrorMessage = "Your loan is already cashed out!" });
                }
                if(existingLoan.Status == LoanStatus.Approved && existingLoan.CashedOut == false && existingLoan.Currency == LoanCurrency.GEL)
                {
                    bankStatement.GEL_Balance -= existingLoan.Amount;
                    existingUser.Balance += existingLoan.Amount;
                    existingLoan.CashedOut = true;
                    _context.Update(bankStatement);
                    _context.Update(existingUser);
                    _context.Update(existingLoan);
                    await _context.SaveChangesAsync();
                }
                if (existingLoan.Status == LoanStatus.Approved && existingLoan.CashedOut == false && existingLoan.Currency == LoanCurrency.USD)
                {
                    bankStatement.USD_Balance -= existingLoan.Amount;
                    existingUser.Balance += (existingLoan.Amount * CurrencyRate.USD_Rate);
                    existingLoan.CashedOut = true;
                    _context.Update(bankStatement);
                    _context.Update(existingUser);
                    _context.Update(existingLoan);
                    await _context.SaveChangesAsync();
                }
                if (existingLoan.Status == LoanStatus.Approved && existingLoan.CashedOut == false && existingLoan.Currency == LoanCurrency.EUR)
                {
                    bankStatement.EUR_Balance -= existingLoan.Amount;
                    existingUser.Balance += (existingLoan.Amount * CurrencyRate.EUR_Rate);
                    existingLoan.CashedOut = true;
                    _context.Update(bankStatement);
                    _context.Update(existingUser);
                    _context.Update(existingLoan);
                    await _context.SaveChangesAsync();
                }
                return Ok(new { SuccessMessage = "You have successfully cashed out the loan amount!" });
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize]
        [HttpPost("CoverLoan")]
        public async Task<IActionResult> CoverLoan([FromBody] CoverLoan coverLoan)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                var existingLoan = await _context.Loans.SingleOrDefaultAsync(loan => loan.Id == coverLoan.LoanId && loan.UserId ==  userId);
                if(existingLoan == null)
                {
                    return NotFound(new { Message = $"There is no any loan by ID: {coverLoan.LoanId}" });
                }
                if(existingLoan.LoanPaid == true || existingLoan.TotalAmountPayableInGEL == 0)
                {
                    return BadRequest(new { Message = "That loan has already payed!" });
                }
                if (existingUser.IsBlocked == true)
                {
                    return BadRequest(new { Message = "You are blocked. Please contact to our Support!" });
                }
                if (existingLoan.Status != LoanStatus.Approved)
                {
                    return BadRequest(new { ErrorMessage = $"Loan status is not Approved to cover. It's status is: {existingLoan.Status} yet!" });
                }
                else
                {
                    var bankStatement = await _context.Banks.SingleOrDefaultAsync() ?? new Bank();
                    if (existingUser.Balance <= 0 || existingUser.Balance < coverLoan.Amount)
                    {
                        return BadRequest(new { ErrorMessage = "You have not anough money to cover loan!" });
                    }
                    if(coverLoan.Amount > existingLoan.TotalAmountPayableInGEL)
                    {
                        return BadRequest(new { Message = $"Amount of left loan is: {existingLoan.TotalAmountPayableInGEL} {LoanCurrency.GEL} and your entered amount is: {coverLoan.Amount}" });
                    }
                    if(coverLoan.Amount > (existingLoan.TotalAmountPayableInGEL - existingLoan.AmountOfCoveredLoan))
                    {
                        return BadRequest(new { Message = $"Amount of left loan is {existingLoan.TotalAmountPayableInGEL - existingLoan.AmountOfCoveredLoan}. Please enter correct amount" });
                    }
                    if (existingUser.Balance > 0 && coverLoan.Amount <= existingLoan.TotalAmountPayableInGEL && existingUser.Balance >= coverLoan.Amount)
                    {
                        existingLoan.AmountOfCoveredLoan += coverLoan.Amount;
                        existingUser.Balance -= coverLoan.Amount;
                        if (existingLoan.Currency == LoanCurrency.GEL)
                        {
                            bankStatement.GEL_Balance += coverLoan.Amount;
                        }
                        else if (existingLoan.Currency == LoanCurrency.USD)
                        {
                            bankStatement.USD_Balance += (coverLoan.Amount / CurrencyRate.USD_Rate);
                        }
                        else if (existingLoan.Currency == LoanCurrency.EUR)
                        {
                            bankStatement.EUR_Balance += (coverLoan.Amount / CurrencyRate.EUR_Rate);
                        }
                    }
                    _context.Update(bankStatement);
                    _context.Update(existingUser);
                    _context.Update(existingLoan);
                    await _context.SaveChangesAsync();
                    if (existingLoan.AmountOfCoveredLoan >= existingLoan.TotalAmountPayableInGEL)
                    {
                        existingLoan.LoanPaid = true;
                        await _context.SaveChangesAsync();
                    }
                    return Ok(new { SuccessMessage = $"You have successfully payed: {coverLoan.Amount} to the Bank's account!" });
                }

            }catch(Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, "Internal Server Error!");
            }
        }
    }
}
