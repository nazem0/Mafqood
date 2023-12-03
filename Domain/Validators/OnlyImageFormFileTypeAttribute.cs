using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class OnlyImageFormFileTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return false;
            IFormFileCollection? files = value as IFormFileCollection;
            if (files is null)
                return false;
            return files.Where(f => f.ContentType.StartsWith("image")).Count() == files.Count;

        }
    }
}
