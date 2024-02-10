using Loan_Web_API.Database;
using Loan_Web_API.Helpers;
using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Loan_Web_API.Services;
using Loan_Web_API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Loan_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly LoanApiContext _context;
        private readonly IUserService _service;
        private readonly AppSettings _appSettings;
        public UserController(LoanApiContext context, IUserService service, AppSettings appSettings)
        {
            _context = context;
            _service = service;
            _appSettings = appSettings;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterModel newUser)
        {
            try
            {
                var userValidator = new UserRegisterValidator(_context);
                var validatorResults = userValidator.Validate(newUser);
                if (!validatorResults.IsValid)
                {
                    return BadRequest(validatorResults.Errors);
                }
                else
                {
                    await _service.RegisterNewUser(newUser);
                    await _context.SaveChangesAsync();
                    return Ok(new { SuccessMessage = $"User: {newUser.UserName} has been successfully registered!" });
                }
            }catch(Exception ex)

            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500,"Internal Server Error!");
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginModel login)
        {
            try
            {
                var userLogin = await _service.UserLoginForm(login);
                if (userLogin == null)
                {
                    return BadRequest(new { message = "Username, Password or E-Mail is incorrect!" });
                }
                else
                {
                    Loggs newLog = new Loggs
                    {
                        LoggedUser = userLogin.UserName.ToString(),
                        UserEmail = userLogin.Email.ToString(),
                        UserRole = userLogin.Role.ToString(),
                        UserId = userLogin.Id,
                        LoggDate = DateTime.Now
                        
                    };
                    await _context.Loggs.AddAsync(newLog);
                    await _context.SaveChangesAsync();


                    var tokenString = GenerateToken(userLogin);

                    return Ok(new
                    {
                        Message = "You have successfully Logged!",
                        UserName = userLogin.UserName,
                        Email = userLogin.Email,
                        Role = userLogin.Role,
                        Token = tokenString
                    });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize]
        [HttpGet("GetMyBalance")]
        public async Task<IActionResult> GetMyBalance()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                return Ok(new { Message = $"Your balance is: {existingUser.Balance} GEL." });

            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize]
        [HttpGet("GetMyProfile")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
                return Ok(existingUser);

            }catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Errorr!");
            }
        }
        private object GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
