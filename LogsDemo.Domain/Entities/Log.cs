using LogsDemo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LogsDemo.Domain.Entities
{
    public class Log : BaseEntity<string>
    {
        public Log()
        {
            this.Params = new List<string>();
        }

        public LogSeverity Severity { get; set; }
        public LogType Type { get; set; }

        [Required(ErrorMessage = "Log's Message Required")]
        public string Message { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Log's User Id Required")]
        public string UserId { get; set; }

        public ICollection<string> Params { get; set; }

    }
}
