using API.Messages.ModelBinding;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace API.Messages
{
    public class GetFileInfoRequest : ApiRequest
    {
        [FromQuery]
        public Guid Id { get; set; }
    }

    public class GetFileInfoResponse
    {
        [ToResult]
        public List<StoredFileInfo> Body { get; set; }
    }
}