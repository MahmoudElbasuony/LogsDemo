using LogsDemo.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogsDemo.Domain.Entities
{
    public class LogEntity : BaseEntity<string>
    {
        public LogEntity()
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
