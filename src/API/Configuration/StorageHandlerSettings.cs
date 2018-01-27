using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Configuration
{
    public class StorageHandlerSettings
    {
        public ExtSettings Ext { get; set; }

        [Required]
        public string Provider { get; set; }

        public Dictionary<string, object> Params { get; set; }
    }
}
