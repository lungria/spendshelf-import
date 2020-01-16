using Microsoft.AspNetCore.Http;
using SpendShelf.BankTransactionsImport.Infrastructure.Attributes;
using System.IO;
using System.Text;
using Xunit;

namespace SpendShelf.BankTransactionsImport.Tests.Infrastructure.Attributes
{
    public class MaxFileSizeAttributeTests
    {
        [Theory]
        [InlineData(50 * 1024 * 1024)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidatesCorrectly_WhenSizeIsLessOrEqual(int maxSize)
        {
            var allowedExtensionsAttribute = new MaxFileSizeAttribute(maxSize);
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 10, "Data", "testfile.xls");

            var result = allowedExtensionsAttribute.IsValid(file);

            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        public void IsValid_ValidatesCorrectly_WhenSizeIsBiggerThenMax(int maxSize)
        {
            var allowedExtensionsAttribute = new MaxFileSizeAttribute(maxSize);
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 10, "Data", "testfile.xls");

            var result = allowedExtensionsAttribute.IsValid(file);

            Assert.False(result);
        }
    }
}