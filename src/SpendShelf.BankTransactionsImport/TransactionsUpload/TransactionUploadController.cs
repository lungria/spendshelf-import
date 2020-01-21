using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendShelf.BankTransactionsImport.TransactionsParser;
using SpendShelf.BankTransactionsImport.TransactuinsProcessor;

namespace SpendShelf.BankTransactionsImport.TransactionsUpload
{
    [ApiController]
    [Route("api/transactions/upload")]
    public class TransactionUploadController : ControllerBase
    {
        private readonly ChannelWriter<Stream> _channel;

        public TransactionUploadController(
            ChannelWriter<Stream> channel)
        {
            _channel = channel;
        }
            
        [HttpPost("privat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadPrivat([FromForm] BankFileInfo fileInfo)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var ms = new MemoryStream();
#pragma warning restore CA2000 // Dispose objects before losing scope
            
            await fileInfo.FormFile.CopyToAsync(ms);

            await _channel.WriteAsync(ms);
            
            return Ok();
        }
    }
}
