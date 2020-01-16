using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SpendShelf.BankTransactionsImport.Infrastructure.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions) => _extensions = extensions;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           return _extensions.Contains(Path.GetExtension((value as IFormFile).FileName), StringComparer.InvariantCultureIgnoreCase) ?
                 ValidationResult.Success :
                 new ValidationResult("Extension is not allowed.");
        }
    }
}
