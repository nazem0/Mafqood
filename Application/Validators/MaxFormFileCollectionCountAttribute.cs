using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Validators
{
    public class MaxFormFileCollectionCountAttribute : ValidationAttribute
    {
        private readonly int _maxFileCount;
        public MaxFormFileCollectionCountAttribute(int maxFileCount)
        {
            _maxFileCount = maxFileCount;
        }
        public override bool IsValid(object? value)
        {
            IFormFileCollection? files = value as IFormFileCollection;
            if (files is null)
                return false;
            return files.Count <= _maxFileCount;
        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_maxFileCount.ToString());
        }
    }
}
