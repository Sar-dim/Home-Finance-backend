using System;
using System.ComponentModel.DataAnnotations;

namespace net_core_backend.Models
{
    public class OperationRequest
    {
        public int Type { get; set; }
        public string Source { get; set; }
        [Required]
        public DateTime DateFirst { get; set; }
        public DateTime? DateSecond { get; set; }
    }
}
