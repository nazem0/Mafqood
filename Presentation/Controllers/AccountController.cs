using Domain.Entities;
using Application.Interfaces.IRepositories;
using Application.DTOs.AccountDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<User> _userManager;

        public AccountController(IAccountRepository accountRepository, UserManager<User> userManager)
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            SignInResult SignInResult = await _accountRepository.Login(loginDTO);

            if (!SignInResult.Succeeded) return BadRequest("User name or Password is Wrong");

            User? user = await _userManager.FindByNameAsync(loginDTO.Username);
            string tokenString = await _accountRepository.GenerateJSONWebToken(user!,loginDTO.RememberMe);
            IList<string> roles = await _userManager.GetRolesAsync(user!);
            return Ok(new
            {
                token = tokenString,
                role = roles.First(),
                id = user!.Id
            });


        }

        [HttpGet("Logout")]
        public async Task Logout()
        {
            await _accountRepository.Logout();
        }
    }
}
