using Application.DTOs.AccountDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.IRepositories
{
    public interface IAccountRepository
    {
        Task<SignInResult> Login(LoginDTO logginDTO);
        Task Logout();
        Task<string> GenerateJSONWebToken(User user);
    }
}

