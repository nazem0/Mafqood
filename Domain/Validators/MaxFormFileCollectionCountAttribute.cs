using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
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
