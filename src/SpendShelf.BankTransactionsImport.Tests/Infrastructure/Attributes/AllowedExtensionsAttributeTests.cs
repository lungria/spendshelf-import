using Microsoft.AspNetCore.Http;
using SpendShelf.BankTransactionsImport.Infrastructure.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SpendShelf.BankTransactionsImport.Tests.Infrastructure.Attributes
{
    public class AllowedExtensionsAttributeTests
    {
        [Theory]
        [MemberData(nameof(ValidData))]
        public void IsValid_ValidatesCorrectly_WhenInputValid(string[] allowedTypes, string fileName)
        {
            var allowedExtensionsAttribute = new AllowedExtensionsAttribute(allowedTypes);
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 0, "Data", fileName);

            var result = allowedExtensionsAttribute.IsValid(file);

            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public void IsValid_ValidatesCorrectly_WhenInputInvalid(string[] allowedTypes, string fileName)
        {
            var allowedExtensionsAttribute = new AllowedExtensionsAttribute(allowedTypes);
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 0, "Data", fileName);

            var result = allowedExtensionsAttribute.IsValid(file);

            Assert.False(result);
        }

        public static IEnumerable<object[]> ValidData =>
        new List<object[]>
        {
                new object[] { new string[] { ".xls" }, "test.xls"},
                new object[] { new string[] { ".txt", ".xls" }, "test.xls"},
                new object[] { new string[] { ".cs", ".xls" }, "test.cs"},
        };

        public static IEnumerable<object[]> InvalidData =>
        new List<object[]>
        {
                    new object[] { new string[] {  }, "test.xls"},
                    new object[] { new string[] { ".txt"}, "test.xls"},
                    new object[] { new string[] { ".png", ".xls" }, "test.cs"},
                    new object[] { new string[] { "xls"}, "test.xls"},
        };
    }
}
