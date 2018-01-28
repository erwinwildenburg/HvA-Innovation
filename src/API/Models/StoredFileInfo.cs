using System;

namespace API.Models
{
    public class StoredFileInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}