using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SpendShelf.BankTransactionsImport.Infrastructure.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize) => _maxFileSize = maxFileSize;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return (value as IFormFile).Length <= _maxFileSize ?
                  ValidationResult.Success :
                  new ValidationResult($"{_maxFileSize} is max file size allowed.");
        }
    }
}