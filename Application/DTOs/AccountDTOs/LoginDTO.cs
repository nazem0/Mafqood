using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AccountDTOs
{
    public class LoginDTO
    {
        [Required]
        public required string Username { get; set; }

        [Required,DataType(DataType.Password)]
        public required string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}

