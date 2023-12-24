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
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .SelectMany(ms => ms.Value!.Errors
                    .Select(e => new { Field = ms.Key, Error = e.ErrorMessage }))
                    .ToList();
                return BadRequest(errorList);
            }

            SignInResult SignInResult = await _accountRepository.Login(loginDTO);
            if (SignInResult.Succeeded)
            {
                User? user = await _userManager.FindByEmailAsync(loginDTO.Email);
                string tokenString = await _accountRepository.GenerateJSONWebToken(user!);
                IList<string> roles = await _userManager.GetRolesAsync(user!);
                return Ok(new
                {
                    token = tokenString,
                    role = roles,
                    id = user!.Id
                });

            }
            else return new ObjectResult("User name or Password is Wrong");
        }

        [HttpGet("Logout")]
        public async Task Logout()
        {
            await _accountRepository.Logout();
        }
    }
}
