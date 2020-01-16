using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendShelf.BankTransactionsImport.TransactionsParser;

namespace SpendShelf.BankTransactionsImport.TransactionsUpload
{
    [ApiController]
    [Route("api/transactions/upload")]
    public class TransactionUploadController : ControllerBase
    {
        private readonly IBankTransactionsXlsParser _xlsFileParser;

        public TransactionUploadController(IBankTransactionsXlsParser xlsFileParser) => _xlsFileParser = xlsFileParser;

        [HttpPost("privat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadPrivat([FromForm] BankFileInfo fileInfo)
        {
            using var ms = new MemoryStream();
            await fileInfo.FormFile.CopyToAsync(ms);

            // ToDo add actual result processing
            var result = _xlsFileParser.ParseTransactions(ms);

            return Ok();
        }
    }
}
