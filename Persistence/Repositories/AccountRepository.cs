using Domain.Entities;
using Application.Interfaces.IRepositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Application.DTOs.AccountDTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger<AccountRepository> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository
        (
            ILogger<AccountRepository> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<SignInResult> Login(LoginDTO logginDTO)
        {
            var User = await _userManager.FindByEmailAsync(logginDTO.Email);
            if (User != null)
                return await _signInManager.PasswordSignInAsync
                    (User, logginDTO.Password, logginDTO.RememberMe, logginDTO.RememberMe);
            else
                return SignInResult.Failed;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GenerateJSONWebToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = userRoles.Select(o => new Claim(ClaimTypes.Role, o));
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
            }
            .Union(roles);

            var token = new JwtSecurityToken(
              expires: DateTime.Now.AddDays(30),
              signingCredentials: credentials,
              claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
