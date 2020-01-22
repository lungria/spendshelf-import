using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var ms = new MemoryStream();
            
            await fileInfo.FormFile.CopyToAsync(ms);
            await _channel.WriteAsync(ms);
            
            return Ok();
        }
    }
}
