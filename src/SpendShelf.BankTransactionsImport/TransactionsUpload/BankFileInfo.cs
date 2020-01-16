using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SpendShelf.BankTransactionsImport.Infrastructure.Attributes;

namespace SpendShelf.BankTransactionsImport.TransactionsUpload
{
    public class BankFileInfo
    {
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".xls" })]
        public IFormFile FormFile { get; set; } = default!;
    }
}
