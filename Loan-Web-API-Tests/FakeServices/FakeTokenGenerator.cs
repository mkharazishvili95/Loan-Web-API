using Loan_Web_API.Identity;
using Loan_Web_API.Models;

namespace FakeServices
{
    public class FakeTokenGenerator : IAccessTokenGenerator
    {
        public string GenerateToken(User user)
        {
            string fakeToken = "Fake token =)";
            return fakeToken;
        }
    }
}
