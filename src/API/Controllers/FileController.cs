using System.Collections.Generic;
using System.Threading.Tasks;
using API.Helpers;
using API.Messages;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("files")]
    public class FileController : Controller
    {
        private readonly IStorageHandler _storageHandler;

        public FileController(StorageHandlerHelper storageHelper)
        {
            _storageHandler = storageHelper.DefaultHandler;
        }

        [HttpGet]
        [Produces(typeof(List<StoredFileInfo>))]
        public async Task<IActionResult> GetFiles(GetFileInfoRequest input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            List<StoredFileInfo> files = await _storageHandler.GetFileInfo(input.Id);

            return this.Model(new GetFileInfoResponse
            {
                Body = files
            });
        }
    }
}
