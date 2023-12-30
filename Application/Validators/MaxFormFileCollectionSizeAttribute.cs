using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Validators
{
    public class MaxFormFileCollectionSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInMegaBytes;
        public MaxFormFileCollectionSizeAttribute(int maxFileSizeInMegaBytes)
        {
            _maxFileSizeInMegaBytes = maxFileSizeInMegaBytes;
        }
        public override bool IsValid(object? value)
        {
            if (value is IFormFileCollection files)
            {
                long largestFileSizeInBytes = files.Sum(f => f.Length);
                //Dividing by 1024^2 which is 1,048,576 to transfer from bytes to megabytes.
                double largestFileSizeInMegaBytes = Math.Ceiling((double)largestFileSizeInBytes / 1048576);
                return largestFileSizeInMegaBytes <= _maxFileSizeInMegaBytes;
            }
            else if (value is IFormFile file)
            {
                double largestFileSizeInMegaBytes = Math.Ceiling((double)file.Length / 1048576);
                return largestFileSizeInMegaBytes <= _maxFileSizeInMegaBytes;
            }
            else if (value is null) return true;
            else return false;
        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_maxFileSizeInMegaBytes.ToString());
        }
    }
}
