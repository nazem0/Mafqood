using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Validators
{
    public class OnlyImageFormFileTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is IFormFileCollection files)
                return files.All(f => f.ContentType.StartsWith("image"));
            else if (value is IFormFile file)
                return file.ContentType.StartsWith("image");
            else if (value is null) return true;
            else return false;
        }
    }
}
