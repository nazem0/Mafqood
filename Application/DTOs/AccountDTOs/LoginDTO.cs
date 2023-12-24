using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AccountDTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}

