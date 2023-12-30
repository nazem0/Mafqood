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

        public async Task<SignInResult> Login(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByNameAsync(loginDTO.Username);
            if (User != null)
                return await _signInManager.PasswordSignInAsync
                    (User, loginDTO.Password, loginDTO.RememberMe, false);
            else
                return SignInResult.Failed;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GenerateJSONWebToken(User user, bool rememberMe)
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

            return 
                rememberMe is true ? 
                new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                    signingCredentials: credentials,
                    claims: claims))
                :
                new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: credentials,
                    claims: claims));
        }
    }
}
