using LogsDemo.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LogsDemo.API.Models
{
    public class LogCreateDto
    {
        public LogCreateDto()
        {
            this.Params = new List<string>();
        }

        public LogSeverity Severity { get; set; }
        public LogType Type { get; set; }
        public ICollection<string> Params { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Log's Message Required")]
        public string Message { get; set; }

       
    }
}
